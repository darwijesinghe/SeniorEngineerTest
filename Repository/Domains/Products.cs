using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Domains
{
    /// <summary>
    /// Domain class for product entity
    /// </summary>
    public class Products
    {
        /// <summary>
        /// Product ID PK
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId        { get; set; }

        /// <summary>
        /// Product name
        /// </summary>
        public string Name          { get; set; }

        /// <summary>
        /// Product category name
        /// </summary>
        public string Category      { get; set; }

        /// <summary>
        /// Product price
        /// </summary>
        public decimal Price        { get; set; }

        /// <summary>
        /// Products stock
        /// </summary>
        public int Stock            { get; set; }
    }

    /// <summary>
    /// Data transfer models
    /// </summary>
    public class ProductsDto
    {
        /// <summary>
        /// Product ID PK
        /// </summary>
        public int ProductId        { get; set; }

        /// <summary>
        /// Product name
        /// </summary>
        public string Name          { get; set; }

        /// <summary>
        /// Product category name
        /// </summary>
        public string Category      { get; set; }

        /// <summary>
        /// Product price
        /// </summary>
        public decimal Price        { get; set; }

        /// <summary>
        /// Products stock
        /// </summary>
        public int Stock            { get; set; }
    }
}
