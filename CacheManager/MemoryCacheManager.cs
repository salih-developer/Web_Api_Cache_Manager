namespace CacheManager
{
    #region

    using System;
    using System.Linq;
    using System.Runtime.Caching;
    using System.Text.RegularExpressions;

    #endregion

    /// <summary>The memory cache manager.</summary>
    public class MemoryCacheManager : ICacheManager
    {
        #region Properties

        /// <summary>
        /// Gets the cache.
        /// </summary>
        protected ObjectCache Cache
        {
            get
            {
                return MemoryCache.Default;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Clear all cache data
        /// </summary>
        public void Clear()
        {
            foreach (var item in this.Cache)
            {
                this.Remove(item.Key);
            }
        }

        /// <summary>Gets or sets the value associated with the specified key.</summary>
        /// <typeparam name="T">Type parameter</typeparam>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>The value associated with the specified key.</returns>
        public T Get<T>(string key)
        {
            return (T)this.Cache[key];
        }

        /// <summary>The get un sliding.</summary>
        /// <typeparam name="T">Type parameter</typeparam>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>The value associated with the specified key.</returns>
        public T GetUnSliding<T>(string key)
        {
            return (T)this.Cache[key];
        }

        /// <summary>Gets a value indicating whether the value associated with the specified key is cached</summary>
        /// <param name="key">key value</param>
        /// <returns>key value is set</returns>
        public bool IsSet(string key)
        {
            return this.Cache.Contains(key);
        }

        /// <summary>Removes the value with the specified key from the cache</summary>
        /// <param name="key">key value</param>
        public void Remove(string key)
        {
            this.Cache.Remove(key);
        }

        /// <summary>The remove by key.</summary>
        /// <param name="key">The key.</param>
        /// <param name="languageIndex">The language index.</param>
        public void RemoveByKey(string key, int languageIndex)
        {
            try
            {
                key = string.Format("test_", languageIndex) + "." + key;
                this.Cache.Remove(key);
            }
            catch (Exception ex)
            {
                //// this.logging.Error(string.Format("Key : {0}", key), ex);
            }
        }

        /// <summary>Removes items by pattern</summary>
        /// <param name="pattern">pattern to be searched</param>
        public void RemoveByPattern(string pattern)
        {
            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var keysToRemove = (from item in this.Cache where regex.IsMatch(item.Key) select item.Key).ToList();

            foreach (var key in keysToRemove)
            {
                this.Remove(key);
            }
        }

        /// <summary>Adds the specified key and object to the cache.</summary>
        /// <param name="key">key value</param>
        /// <param name="data">Data object</param>
        /// <param name="cacheTime">Cache time</param>
        public void Set(string key, object data, int cacheTime)
        {
            this.SetSecond(key, data, 60 * cacheTime);
        }

        /// <summary>The set second.</summary>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        /// <param name="cacheTime">The cache time.</param>
        public void SetSecond(string key, object data, int cacheTime)
        {
            if (data == null)
            {
                return;
            }

            var policy = new CacheItemPolicy { AbsoluteExpiration = DateTime.Now + TimeSpan.FromSeconds(cacheTime) };
            this.Cache.Add(new CacheItem(key, data), policy);
        }

        #endregion
    }
}