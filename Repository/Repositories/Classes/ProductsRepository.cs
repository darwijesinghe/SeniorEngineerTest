using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repository.Data;
using Repository.Domains;
using Repository.Domains.Calculation;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories.Classes
{
    /// <summary>
    /// Repository implementation for product-specific operations
    /// </summary>
    public class ProductsRepository : IGenericRepository<Products>, ICalculation
    {
        // Data context
        private DataContext _context { get; }

        // Services
        private readonly ILogger<ProductsRepository> _logger;

        public ProductsRepository(DataContext context, ILogger<ProductsRepository> logger) 
        {
            _context = context;
            _logger  = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Retrieves all entities from the data source.
        /// </summary>
        /// <returns>
        /// A collection of all entities in the data source.
        /// </returns>
        public async Task<IEnumerable<Products>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        /// <summary>
        /// Finds and returns an entity based on its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to retrieve.</param>
        /// <returns>
        /// The entity matching the provided identifier, or empty entity if not found.
        /// </returns>
        public async Task<Products> GetByIdAsync(int id)
        {
            // Find the record
            var found = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == id);
            if (found is null)
                return new Products();

            // Return result
            return found;
        }

        /// <summary>
        /// Adds a new entity to the data source and saves the changes.
        /// </summary>
        /// <param name="entity">The entity to be added to the data source.</param>
        /// <returns>
        /// An error message if the operation fails; otherwise, null.
        /// </returns>
        public async Task<string> AddAsync(Products entity)
        {
            // adds the product entity to the context
            await _context.Products.AddAsync(entity);

            try
            {
                // saves changes to the context
                await _context.SaveChangesAsync();
                return null;
            }
            catch (Exception ex)
            {
                // logs the exception
                _logger.LogError(ex, "An error occurred while adding the product entity.");
                return ex.Message;
            }
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
            // Find the record
            var found = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == id);
            if (found is null)
                return "Record not found.";

            // Remove the product from the context
            _context.Products.Remove(found);

            try
            {
                // save changes to the context
                await _context.SaveChangesAsync();
                return null;
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An error occurred while deleting the product entity.");
                return ex.Message;
            }
        }

        /// <summary>
        /// Updates an existing entity in the data source with new values.
        /// </summary>
        /// <param name="entity">The entity with updated values to be saved.</param>
        /// <returns>
        /// An error message if the operation fails; otherwise, null.
        /// </returns>
        public async Task<string> UpdateAsync(Products entity)
        {
            // Find the record
            var found = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == entity.ProductId);
            if (found is null)
                return "Record not found.";

            // Update the found entity with new values
            // Ensure the entity is not already tracked by the context to avoid conflicts
            _context.Entry(found).CurrentValues.SetValues(entity);

            try
            {
                // Save changes to the context
                await _context.SaveChangesAsync();
                return null;
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An error occurred while updating the product entity.");
                return ex.Message;
            }
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
            try
            {
                // Execute the stored procedure and map the results
                var results = await _context.AverageCategory
                                            .FromSqlRaw("EXEC GetAveragePricePerCategory")
                                            .ToListAsync();

                // Return the list of results
                return results;
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An error occurred while executing the 'GetAveragePricePerCategory' procedure.");
                return new List<AveragePerCategory>();
            }
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
            try
            {
                // Execute the stored procedure and map the results
                var result = await _context.HighestStock.FromSqlRaw("EXEC GetHighestStockValueCategory")
                                                        .ToListAsync();

                // Return the result
                return result.FirstOrDefault() ?? new HighestStockCategory();
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An error occurred while executing the 'GetHighestStockValueCategory' procedure.");
                return new HighestStockCategory();
            }
        }
    }

}
