using Microsoft.AspNetCore.Mvc;
using Services.Services.Interfaces;
using System.Diagnostics;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        // Services
        private readonly ILogger<HomeController> _logger;
        private readonly IProductsService        _productsService;

        public HomeController(IProductsService productsService, ILogger<HomeController> logger)
        {
            _productsService = productsService;
            _logger          = logger;
        }

        /// <summary>
        /// Loads the Index view.
        /// URL: /Home/Index
        /// </summary>
        /// <returns>
        /// Returns index view.
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewBag.isLogged = false;
            return View();
        }

        /// <summary>
        /// Loads the Products view.
        /// URL: /Home/Products
        /// </summary>
        /// <returns>
        /// Returns products view.
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> Products()
        {
            ViewBag.isLogged = true;
            return View();
        }

        /// <summary>
        /// Loads the Report view with average prices and highest stock value category.
        /// URL: /Home/Report
        /// </summary>
        /// <returns>
        /// Returns report view.
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> Report()
        {
            ViewBag.isLogged = true;

            // view data
            var report = new Average()
            {
                List         = await _productsService.GetAveragePricePerCategory(),
                HighestValue = await _productsService.GetHighestStockValueCategory()
            };

            return View(report);
        }

        /// <summary>
        /// Loads a partial view with a list of products.
        /// URL: /Home/ProductList
        /// </summary>
        /// <returns>
        /// Returns partial view with product list.
        /// </returns>
        [HttpGet]
        public async Task<PartialViewResult> ProductList()
        {
            // view data
            var products = new ProductsViewModel()
            {
                List = await _productsService.GetAllAsync()
            };

            return PartialView("SharedMini/_ProductList", products);
        }

        /// <summary>
        /// Loads a partial view for adding or editing a product.
        /// URL: /Home/AddProduct/{id}
        /// </summary>
        /// <param name="id">Product ID for edit, or 0 for add.</param>
        /// <returns>
        /// Returns partial view for add/edit product.
        /// </returns>
        [HttpGet]
        public async Task<PartialViewResult> AddProduct(int id)
        {
            var product = new ProductsViewModel();

            if (id != 0)
                // gets product
                product.Product = await _productsService.GetByIdAsync(id);

            return PartialView("SharedMini/_AddProduct", product);
        }

        /// <summary>
        /// Saves a new or existing product.
        /// URL: /Home/AddProduct
        /// </summary>
        /// <param name="model">The product view model.</param>
        /// <param name="id">Product ID for edit, or 0 for add.</param>
        /// <returns>
        /// Returns JSON result indicating success or failure.
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(ProductsViewModel model, int id)
        {
            string error = null;

            if (ModelState.IsValid)
            {
                if (id != 0)
                {
                    // update a product
                    model.Product.ProductId = id;
                    error                   = await _productsService.UpdateAsync(model.Product);
                }
                else
                {
                    // adds a product
                    error = await _productsService.AddAsync(model.Product);
                }

                if (string.IsNullOrEmpty(error))
                    return Json(new { success = true, message = "Product saved successfully." });

                return Json(new { success = false, message = error });
            }

            return Json(new { success = false, message = "Fields are not valid." });
        }

        /// <summary>
        /// Loads a partial view for confirming product deletion.
        /// URL: /Home/DeleteProduct/{id}
        /// </summary>
        /// <param name="id">The product ID to delete.</param>
        /// <returns>
        /// Returns partial view for delete confirmation.
        /// </returns>
        [HttpGet]
        public async Task<PartialViewResult> DeleteProduct(int id)
        {
            // gets data to the ID
            var product = await _productsService.GetByIdAsync(id);

            // view data
            var model = new ProductsViewModel
            {
                Product = product
            };

            return PartialView("SharedMini/_DeleteProduct", model);
        }

        /// <summary>
        /// Deletes a product.
        /// URL: /Home/DeleteProduct
        /// </summary>
        /// <param name="model">The product DTO containing the ID of the product to delete.</param>
        /// <returns>
        /// Returns <see cref="JsonResult"/> result indicating success or failure.
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> DeleteProduct(ProductsViewModel model)
        {
            // deletes the product
            string error = await _productsService.DeleteAsync(model.Product.ProductId);

            if (string.IsNullOrEmpty(error))
                return Json(new { success = true, message = "Product deleted successfully." });

            return Json(new { success = false, message = error });
        }

        /// <summary>
        /// Displays an error view.
        /// URL: /Home/Error
        /// </summary>
        /// <returns>
        /// Returns error view.
        /// </returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
