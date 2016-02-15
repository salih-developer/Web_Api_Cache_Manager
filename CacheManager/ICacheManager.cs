namespace CacheManager
{
    /// <summary>The CacheManager interface.</summary>
    public interface ICacheManager
    {
        #region Public Methods and Operators

        /// <summary> Clear all cache data </summary>
        void Clear();

        /// <summary>Gets or sets the value associated with the specified key. </summary>
        /// <typeparam name="T">Type parameter</typeparam>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>The value associated with the specified key.</returns>
        T Get<T>(string key);

        /// <summary>The get un sliding.</summary>
        /// <param name="key">The key.</param>
        /// <typeparam name="T">Type parameter</typeparam>
        /// <returns>The value associated with the specified key.</returns>
        T GetUnSliding<T>(string key);

        /// <summary>Gets a value indicating whether the value associated with the specified key is cached </summary>
        /// <param name="key">key value</param>
        /// <returns>key is set</returns>
        bool IsSet(string key);

        /// <summary>Removes the value with the specified key from the cache </summary>
        /// <param name="key">key to be removed</param>
        void Remove(string key);

        /// <summary>The remove by key.</summary>
        /// <param name="key">The key.</param>
        /// <param name="languageIndex">The language index.</param>
        void RemoveByKey(string key, int languageIndex);

        /// <summary>Removes items by pattern </summary>
        /// <param name="pattern">pattern to be searched</param>
        void RemoveByPattern(string pattern);

        /// <summary>Adds the specified key and object to the cache. </summary>
        /// <param name="key">Type parameter</param>
        /// <param name="data">Data object</param>
        /// <param name="cacheTime">Cache time</param>
        void Set(string key, object data, int cacheTime);

        /// <summary>The set second.</summary>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        /// <param name="cacheTime">The cache time.</param>
        void SetSecond(string key, object data, int cacheTime);

        #endregion
    }
}