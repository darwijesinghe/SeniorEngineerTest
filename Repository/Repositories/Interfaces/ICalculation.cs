using Repository.Domains.Calculation;

namespace Repository.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for calculation operations
    /// </summary>
    public interface ICalculation
    {
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
