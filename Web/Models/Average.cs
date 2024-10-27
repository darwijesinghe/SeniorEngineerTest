using Repository.Domains.Calculation;

namespace Web.Models
{
    /// <summary>
    /// Holds average price data and the category with the highest stock value
    /// </summary>
    public class Average
    {
        /// <summary>
        /// List of average prices per product category
        /// </summary>
        public List<AveragePerCategory> List     { get; set; }

        /// <summary>
        /// Product category with the highest stock value
        /// </summary>
        public HighestStockCategory HighestValue { get; set; }
    }

}
