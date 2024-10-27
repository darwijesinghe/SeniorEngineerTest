using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Repository.Data;
using Repository.Domains;
using Repository.Repositories.Classes;
using Repository.Repositories.Interfaces;
using Services.Services.Classes;

namespace Test
{
    [TestClass]
    public class ProductsServiceTests
    {
        // Dbcontext
        private DataContext                   _context;

        // Repositories
        private IGenericRepository<Products>  _repository;
        private ICalculation                  _calculation;

        // Services
        private ProductsService               _productService;
        private IMemoryCache                  _cache;

        // Logs
        private ILogger<ProductsRepository>   _rlogger;
        private ILogger<ProductsService>      _slogger;
        private ILogger<ProductsServiceTests> _logger;

        /// <summary>
        /// Initializes the test setup before each test is run.
        /// This method sets up the mock database context, initializes the ProductsRepository and 
        /// ProductsService, and handles any exceptions that occur during the setup process.
        /// </summary>
        [TestInitialize]
        public async Task Setup()
        {
            try
            {
                // Set up the mock database context and other necessary components
                await SetupMockDbContext();

                // Initialize the ProductsRepository with the mock context and logger
                _calculation = new ProductsRepository(_context, _rlogger);

                // Initialize the ProductsService with the mock repository, calculation logic, cache, and logger
                _productService = new ProductsService(_repository, _calculation, _cache, _slogger);
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during the setup process
                _logger.LogError(ex.Message);
            }
        }

        /// <summary>
        /// Sets up the mock database context, in-memory cache, and necessary loggers for unit testing.
        /// This method initializes an in-memory database, adds sample data to the Products table,
        /// and prepares the mock repository for use in unit tests.
        /// </summary>
        private async Task SetupMockDbContext()
        {
            // Set up a service provider to support logging in the application
            var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();

            // Initialize loggers for the repository, service, and unit tests
            _rlogger = serviceProvider.GetRequiredService<ILogger<ProductsRepository>>();
            _slogger = serviceProvider.GetRequiredService<ILogger<ProductsService>>();
            _logger  = serviceProvider.GetRequiredService<ILogger<ProductsServiceTests>>();

            // Configure in-memory database options for testing purposes
            var options = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase(databaseName: "MockDatabase").Options;

            // Initialize the DataContext with the in-memory database options
            _context = new DataContext(options);

            // Add test data to the Products table in the in-memory database
            await _context.Products.AddRangeAsync(new List<Products>
            {
                new Products { ProductId = 1, Name = "Apple iPhone", Category = "Electronic", Price = 1500, Stock = 10 },
                new Products { ProductId = 2, Name = "Apple iMac"  , Category = "Electronic", Price = 1600, Stock = 5 },
                new Products { ProductId = 3, Name = "Mens Trouser", Category = "Clothing"  , Price = 650 , Stock = 10 },
                new Products { ProductId = 4, Name = "Mens Shirt"  , Category = "Clothing"  , Price = 550 , Stock = 15 }
            });

            // Save the changes to the in-memory database
            await _context.SaveChangesAsync();

            // Initialize an in-memory cache
            _cache = new MemoryCache(new MemoryCacheOptions());

            // Create a mock instance of the ProductsRepository with the test context and logger
            _repository = new ProductsRepository(_context, _rlogger);
        }

        /// <summary>
        /// Tests the retrieval of all products from the data source.
        /// </summary>
        [TestMethod]
        public async Task GetAllAsyncTest()
        {
            // Act: Retrieve all products using the service.
            var result = await _productService.GetAllAsync();

            try
            {
                // Assert: Ensure the result is not null, indicating products were retrieved successfully.
                Assert.IsNotNull(result, "Expected result not found.");
            }
            catch (Exception ex)
            {
                // Fail the test if an exception occurs during assertion.
                Assert.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Tests retrieving a product by its identifier from the data source.
        /// </summary>
        [TestMethod]
        public async Task GetByIdAsyncTest()
        {
            // Arrange: Define a specific product ID to retrieve.
            int productId = 2;

            // Act: Retrieve the product by its ID using the service.
            var result = await _productService.GetByIdAsync(productId);

            try
            {
                // Assert: Check if the retrieved product's ID matches the expected ID.
                Assert.AreEqual(productId, result.ProductId, "Arrange id is not equal to result id.");
            }
            catch (Exception ex)
            {
                // Fail the test if an exception occurs during assertion.
                Assert.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Tests adding a new product to the data source.
        /// </summary>
        [TestMethod]
        public async Task AddAsyncTest()
        {
            // Arrange: Create a new product DTO to add.
            var newEntity = new ProductsDto
            {
                Name     = "Ladies Shirt",
                Category = "Clothing",
                Price    = 850,
                Stock    = 4
            };

            // Act: Add the new product using the service.
            await _productService.AddAsync(newEntity);

            try
            {
                // Assert: Verify that the product was added by checking the total count and the details of the last product.
                var result = await _productService.GetAllAsync();

                // 4 existing + 1 new
                Assert.AreEqual(5, result.Count, "Expected product count does not match the actual count.");
                Assert.AreEqual("Ladies Shirt", result.LastOrDefault()?.Name, "Expected product name does not match the actual name.");
            }
            catch (Exception ex)
            {
                // Fail the test if an exception occurs during assertion.
                Assert.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Tests updating an existing product in the data source.
        /// </summary>
        [TestMethod]
        public async Task UpdateAsyncTest()
        {
            // Arrange: Create a product DTO with updated details.
            var entity = new ProductsDto
            {
                ProductId = 2,
                Name      = "Test Update",
                Category  = "Clothing",
                Price     = 850,
                Stock     = 4
            };

            // Act: Update the product details using the service.
            await _productService.UpdateAsync(entity);

            try
            {
                // Assert: Verify that the product details were updated by retrieving the product and checking its name.
                var result = await _productService.GetByIdAsync(entity.ProductId);
                Assert.AreEqual("Test Update", result.Name, "Expected updated name does not match the actual name.");
            }
            catch (Exception ex)
            {
                // Fail the test if an exception occurs during assertion.
                Assert.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Tests deleting a product from the data source.
        /// </summary>
        [TestMethod]
        public async Task DeleteAsyncTest()
        {
            // Arrange: Specify the ID of the product to delete.
            int productId = 4;

            // Act: Delete the product using the service.
            await _productService.DeleteAsync(productId);

            try
            {
                // Assert: Verify that the product was deleted by checking the total count.
                var result = await _productService.GetAllAsync();

                // 4 existing; 1 deleted
                Assert.AreEqual(3, result.Count, "Expected product count after deletion does not match the actual count.");
            }
            catch (Exception ex)
            {
                // Fail the test if an exception occurs during assertion.
                Assert.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Cleans up resources after each test is run.
        /// This method disposes of the database context to ensure that resources are properly released
        /// after each test completes.
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            // Dispose of the database context to free up resources
            _context?.Dispose();
        }
    }
}
