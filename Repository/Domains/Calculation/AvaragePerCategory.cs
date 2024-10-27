using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Domains.Calculation
{
    /// <summary>
    /// Domain class to get the average price for each category
    /// </summary>
    [NotMapped]
    public class AveragePerCategory
    {
        /// <summary>
        /// Product category
        /// </summary>
        public string Category      { get; set; }

        /// <summary>
        /// Average price of the category
        /// </summary>
        public decimal AveragePrice { get; set; }
    }
}
