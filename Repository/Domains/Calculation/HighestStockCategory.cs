using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Domains.Calculation
{
    /// <summary>
    /// Domain class to get the highest stock value category
    /// </summary>
    [NotMapped]
    public class HighestStockCategory
    {
        /// <summary>
        /// Product category
        /// </summary>
        public string Category      { get; set; }
    }
}
