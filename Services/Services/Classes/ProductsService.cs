using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Repository.Domains;
using Repository.Domains.Calculation;
using Repository.Repositories.Interfaces;
using Services.Services.Interfaces;

namespace Services.Services.Classes
{
    /// <summary>
    /// Service implementation for product-specific operations
    /// </summary>
    public class ProductsService : BaseService, IProductsService
    {
        // Repositories
        private IGenericRepository<Products> _productRepo;
        private ICalculation                 _calculation;

        public ProductsService(IGenericRepository<Products> productRepo, ICalculation calculation, IMemoryCache cache, ILogger<BaseService> logger) : base(cache, logger)
        {
            _productRepo = productRepo;
            _calculation = calculation;
        }

        /// <summary>
        /// Retrieves all entities from the data source.
        /// </summary>
        /// <returns>
        /// A collection of all entities in the data source.
        /// </returns>
        public async Task<List<ProductsDto>> GetAllAsync()
        {
            // gets all products from the cache
            var products = await Cached(ServiceCacheKeys.ProductList, async () =>
            {
                return await _productRepo.GetAllAsync();
            });

            if (products is null || !products.Any())
                return new List<ProductsDto>();

            // converts products to DTO
            var productDtos = products.Select(p => new ProductsDto
            {
                ProductId   = p.ProductId,
                Name        = p.Name,
                Category    = p.Category,
                Price       = p.Price,
                Stock       = p.Stock
            })
            .ToList();

            return productDtos;
        }

        /// <summary>
        /// Gets an entity by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the entity to retrieve.</param>
        /// <returns>
        /// The task result contains the entity if found; otherwise empty entity.
        /// </returns>
        public async Task<ProductsDto> GetByIdAsync(int id)
        {
            // gets product from the repository
            var product = await _productRepo.GetByIdAsync(id);
            if (product.ProductId <= 0)
                return new ProductsDto();

            // converts product to DTO
            var productDtos = new ProductsDto
            {
                ProductId = product.ProductId,
                Name      = product.Name,
                Category  = product.Category,
                Price     = product.Price,
                Stock     = product.Stock
            };

            return productDtos;
        }

        /// <summary>
        /// Adds a new product entity to the context and saves changes.
        /// </summary>
        /// <param name="entity">The product entity to add.</param>
        /// <returns>
        /// A null if all good; otherwise error message.
        /// </returns>
        public async Task<string> AddAsync(ProductsDto entity)
        {
            if (entity is null)
                return "Required data not found.";

            // maps DTO with entitiy
            var data = new Products
            {
                Name     = entity.Name,
                Category = entity.Category,
                Price    = entity.Price,
                Stock    = entity.Stock
            };

            // gets result
            var result = await _productRepo.AddAsync(data);
            if (string.IsNullOrEmpty(result))
                // removes the cached data
                RemoveCached(ServiceCacheKeys.ProductList);

            // returns result
            return result;
        }

        /// <summary>
        /// Removes an entity from the data source based on its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to delete.</param>
        /// <returns>
        /// An error message if the operation fails; otherwise, null.
        /// </returns>
        public async Task<string> DeleteAsync(int id)
        {
            if (id <= 0)
                return "Required data not valid.";

            // Get result
            var result = await _productRepo.DeleteAsync(id);
            if (string.IsNullOrEmpty(result))
                // Remove the cached data
                RemoveCached(ServiceCacheKeys.ProductList);

            // Return result
            return result;
        }

        /// <summary>
        /// Updates an existing entity in the data source with new values.
        /// </summary>
        /// <param name="entity">The entity with updated values to be saved.</param>
        /// <returns>
        /// An error message if the operation fails; otherwise, null.
        /// </returns>
        public async Task<string> UpdateAsync(ProductsDto entity)
        {
            if (entity is null)
                return "Required data not found.";

            // Map DTO with entitiy
            var data = new Products
            {
                ProductId = entity.ProductId,
                Name      = entity.Name,
                Category  = entity.Category,
                Price     = entity.Price,
                Stock     = entity.Stock
            };

            // Get result
            var result = await _productRepo.UpdateAsync(data);
            if (string.IsNullOrEmpty(result))
                // Remove the cached data
                RemoveCached(ServiceCacheKeys.ProductList);

            // Return result
            return result;
        }

        /// <summary>
        /// Calculates the average price for each product category in the database.
        /// </summary>
        /// <returns>
        /// A list of <see cref="AveragePerCategory"/> objects, where each object contains the name of a category 
        /// and its corresponding average price. Returns an empty list <see cref="AveragePerCategory"/> object if no categories are found.
        /// </returns>
        /// <remarks>
        /// This method calls the stored procedure "GetAveragePricePerCategory," which retrieves the category names and their average prices from the database.
        /// </remarks>
        public async Task<List<AveragePerCategory>> GetAveragePricePerCategory()
        {
            // Return result
            return await _calculation.GetAveragePricePerCategory();
        }

        /// <summary>
        /// Identifies the product category with the highest stock value.
        /// </summary>
        /// <returns>
        /// An object of <see cref="HighestStockCategory"/> containing the category name with the highest stock value. 
        /// Returns empty object of <see cref="HighestStockCategory"/> if no category is found.
        /// </returns>
        /// <remarks>
        /// This method calls the stored procedure "GetHighestStockValueCategory," which returns the category with the highest aggregate stock value from the database.
        /// </remarks>
        public async Task<HighestStockCategory> GetHighestStockValueCategory()
        {
            // Return result
            return await _calculation.GetHighestStockValueCategory();
        }
    }
}
