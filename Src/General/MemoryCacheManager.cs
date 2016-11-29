using System;
using System.Runtime.Caching;
using System.Text;
using General.Configuration;

namespace General
{
    /// <summary>
    /// Encapsulates generic methods to implement in memory caching.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>
    /// To implement a customized cache for a specific entity, create a derived class from this class. For instance:
    /// <code>
    ///     public sealed class MyEntityMemoryCacheManager : MemoryCacheManager{MyEntity}
    ///     {
    ///         public static void Set(MyEntity entity, CacheItemPolicy policy)
    ///         {
    ///             // implementation
    ///             // generate the key based on the entity
    ///         }
    ///
    ///         public static void Get(long entityID, string revision) // Or any other parameters that make sense for MyEntity to recreate the "cache key"
    ///         {
    ///             // implementation
    ///             // get the entity based on the parameters
    ///         }
    ///     }
    /// </code>
    /// </remarks>
    public abstract class MemoryCacheManager<T> where T : class
    {
        /// <summary>
        /// Gets or sets default global absolute expiration.
        /// </summary>
        /// <remarks>
        /// If no policy pass to the GenericSet() methos, and the value of this field is not zero,
        /// a new <see cref="CacheItemPolicy"/> with AbsoluteExpiration equals to the value of this field will be assign to the cached entity.
        /// </remarks>
        public static double DefaultGlobalAbsoluteExpiration = Manager.Instance.GlobalSetting.MemoryCacheAbsoluteExpiration;

        private static string _generateCacheKey(params object[] args)
        {
            if (args == null || args.Length == 0)
            {
                throw new ArgumentException("At least one parameter is needed.", nameof(args));
            }

            StringBuilder keyBuilder = new StringBuilder(typeof(T).FullName);

            foreach (object arg in args)
            {
                keyBuilder.Append('.').Append(arg);
            }

            //NOTE: MemoryCache internally uses the hash code of the key you give it. 
            //      Effectively every key is stored as an integer. So we have nothing to worry about in regard to the length of the string key.
            return keyBuilder.ToString();
        }

        public static void GenericSet(T entity, params object[] keys)
        {
            GenericSet(entity: entity, policy: null, keys: keys);
        }

        /// <summary>
        /// Saves <paramref name="entity"/> in the memory cache.
        /// </summary>
        /// <param name="entity">An instance of any entity to be cached in memory.</param>
        /// <param name="policy">Cache policy</param>
        /// <param name="keys">Values to be used for creating a unique cache key.</param>
        public static void GenericSet(T entity, CacheItemPolicy policy, params object[] keys)
        {
            if (keys == null || keys.Length == 0)
            {
                throw new ArgumentException("To generate a unique key for MemoryCacheManager<T>, at least one parameter is needed.", nameof(keys));
            }

            CacheItemPolicy cacheItemPolicy =
                policy ??
                    (DefaultGlobalAbsoluteExpiration == 0d
                        ? null
                        : new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(DefaultGlobalAbsoluteExpiration) });

            string key = _generateCacheKey(keys);

            MemoryCache.Default.Set(key, entity, cacheItemPolicy);
        }

        /// <summary>
        /// Gets chahed value for <paramref name="keys"/>, if there's any in the memory cache.
        /// </summary>
        /// <param name="keys">Values to be used for creating a unique cache key.</param>
        /// <returns>Chahed value, if there's any; otherwise, null.</returns>
        public static T GenericGet(params object[] keys)
        {
            string key = _generateCacheKey(keys);
            // Note: if no cache entity exists with the cache key of 'key', the code returns null.
            return MemoryCache.Default.Get(key) as T;
        }
    }
}
