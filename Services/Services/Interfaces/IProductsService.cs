using Repository.Domains;
using Repository.Domains.Calculation;

namespace Services.Services.Interfaces
{
    /// <summary>
    /// Service interface for product-specific operations
    /// </summary>
    public interface IProductsService
    {
        /// <summary>
        /// Retrieves all entities from the data source.
        /// </summary>
        /// <returns>
        /// A collection of all entities in the data source.
        /// </returns>
        Task<List<ProductsDto>> GetAllAsync();

        /// <summary>
        /// Gets an entity by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the entity to retrieve.</param>
        /// <returns>
        /// The task result contains the entity if found; otherwise empty entity.
        /// </returns>
        Task<ProductsDto> GetByIdAsync(int id);

        /// <summary>
        /// Adds a new product entity to the context and saves changes.
        /// </summary>
        /// <param name="entity">The product entity to add.</param>
        /// <returns>
        /// A null if all good; otherwise error message.
        /// </returns>
        Task<string> AddAsync(ProductsDto entity);

        /// <summary>
        /// Updates an existing entity in the data source with new values.
        /// </summary>
        /// <param name="entity">The entity with updated values to be saved.</param>
        /// <returns>
        /// An error message if the operation fails; otherwise, null.
        /// </returns>
        Task<string> UpdateAsync(ProductsDto entity);

        /// <summary>
        /// Removes an entity from the data source based on its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to delete.</param>
        /// <returns>
        /// An error message if the operation fails; otherwise, null.
        /// </returns>
        Task<string> DeleteAsync(int id);

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
        Task<List<AveragePerCategory>> GetAveragePricePerCategory();

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
        Task<HighestStockCategory> GetHighestStockValueCategory();
    }

}
