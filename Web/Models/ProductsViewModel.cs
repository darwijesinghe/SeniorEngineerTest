using Repository.Domains;

namespace Web.Models
{
    /// <summary>
    /// View model for managing product data
    /// </summary>
    public class ProductsViewModel
    {
        public ProductsViewModel()
        {
            Product = new ProductsDto();
            List    = new List<ProductsDto>();
        }

        /// <summary>
        /// List of products.
        /// </summary>
        public List<ProductsDto> List   { get; set; }

        /// <summary>
        /// Current product.
        /// </summary>
        public ProductsDto Product      { get; set; }
    }

}
