using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Services.Services.Classes
{
    /// <summary>
    /// Base service class for the service layer
    /// </summary>
    public abstract class BaseService
    {
        // Services
        private readonly IMemoryCache _cache;
        private readonly ILogger<BaseService> _logger;

        protected BaseService()
        {
                
        }

        protected BaseService(IMemoryCache cache, ILogger<BaseService> logger) 
        {
            _cache  = cache;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Retrieves data from the cache or fetches it using the provided 
        /// function if not already cached. Cache duration is 5 minuts.
        /// </summary>
        /// <typeparam name="T">The type of data to be cached and retrieved.</typeparam>
        /// <param name="key">An enum value representing the cache key for the data.</param>
        /// <param name="create">A function that asynchronously retrieves the data if it is not found in the cache.</param>
        /// <returns>
        /// The cached data if available; otherwise, data retrieved by the function.
        /// </returns>
        protected async Task<T> Cached<T>(ServiceCacheKeys key, Func<Task<T>> create) where T : class
        {
            // Convert the enum key to string
            string cacheKey = key.ToString();

            // Call the method to get the data from the cache or fetch it if not cached
            return await LoadCached(cacheKey, 5, create);
        }

        /// <summary>
        /// Retrieves data from the cache or fetches it using the provided 
        /// function if not already cached. Cache duration is 60 minuts.
        /// </summary>
        /// <typeparam name="T">The type of data to be cached and retrieved.</typeparam>
        /// <param name="key">An enum value representing the cache key for the data.</param>
        /// <param name="create">A function that asynchronously retrieves the data if it is not found in the cache.</param>
        /// <returns>
        /// The cached data if available; otherwise, data retrieved by the function.
        /// </returns>
        protected async Task<T> CachedLong<T>(ServiceCacheKeys key, Func<Task<T>> create) where T : class
        {
            // Convert the enum key to string
            string cacheKey = key.ToString();

            // Call the method to get the data from the cache or fetch it if not cached
            return await LoadCached(cacheKey, 60, create);
        }

        /// <summary>
        /// Removes a specific cache entry based on the provided key.
        /// </summary>
        /// <param name="key">The cache key for the data to remove.</param>
        protected void RemoveCached(ServiceCacheKeys key)
        {
            // Convert the enum key to string
            string cacheKey = key.ToString();

            if (_cache.TryGetValue(cacheKey, out _))
            {
                _cache.Remove(cacheKey);
                _logger.LogInformation("Cache entry with key {CacheKey} has been removed.", key);
            }
        }

        /// <summary>
        /// Loads data from the cache or retrieves it using the provided function if not cached.
        /// </summary>
        /// <typeparam name="T">The type of data to be cached and retrieved.</typeparam>
        /// <param name="internalKey">The key used to store and retrieve the cached data.</param>
        /// <param name="duration">The duration for which the data should be cached.</param>
        /// <param name="create">A function that retrieves the data if it is not found in the cache.</param>
        /// <returns>
        /// The cached data if available; otherwise, data retrieved by the function; null on error.
        /// </returns>
        private async Task<T> LoadCached<T>(string internalKey, int duration, Func<Task<T>> create) where T : class
        {
            // Attempt to get the cached data
            if (_cache.TryGetValue(internalKey, out T data))
            {
                // Return cached data if available
                return data;
            }

            try
            {
                // Retrieve data from the provided function
                data = await create();
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An error occurred while loading the cached data.");
                return null;
            }

            if (data is not null)
            {
                // Set the cache options
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(duration)
                };

                // Add the data to the cache
                _cache.Set(internalKey, data, cacheEntryOptions);
            }

            // Return the retrieved data
            return data; 
        }
    }
}
