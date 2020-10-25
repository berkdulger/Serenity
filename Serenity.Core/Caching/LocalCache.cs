﻿
namespace Serenity
{
#if NET45
    using Serenity.Abstractions;
    using KeyType = System.String;
#else
    using ILocalCache = Microsoft.Extensions.Caching.Memory.IMemoryCache;
    using Microsoft.Extensions.Caching.Memory;
    using KeyType = System.Object;
#endif
    using System;

    /// <summary>
    /// Contains helper functions to access currently registered ILocalCache provider.
    /// </summary>
    public static class LocalCache
    {
        /// <summary>
        /// Use to skip Dependency.Resolve calls only when performance is critical
        /// </summary>
#if !NET45
        [Obsolete(Dependency.UseDI)]
#endif
        public static ILocalCache StaticProvider;

        /// <summary>
        /// Gets current local cache provider, static one or the one configured in dependency resolver
        /// </summary>
#if !NET45
        [Obsolete(Dependency.UseDI)]
#endif        
        public static ILocalCache Provider
        {
            get
            {
                return StaticProvider ?? Dependency.Resolve<ILocalCache>();
            }
        }

        /// <summary>
        /// Adds a value to cache with a given key
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <param name="expiration">Expire time (Use TimeSpan.Zero to hold value with no expiration)</param>
#if !NET45
        [Obsolete(Dependency.UseDI)]
#endif
        public static void Add(string key, object value, TimeSpan expiration)
        {
            Add(Provider, key, value, expiration);
        }

        /// <summary>
        /// Adds a value to cache with a given key
        /// </summary>
        /// <param name="cache">Cache</param>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <param name="expiration">Expire time (Use TimeSpan.Zero to hold value with no expiration). 
        /// If expiration is negative value nothing is added to the cache but removed if exists.</param>
        /// <returns>The value</returns>
        public static TItem Add<TItem>(this ILocalCache cache, KeyType key, TItem value, TimeSpan expiration)
        {
            if (cache == null)
                throw new ArgumentNullException(nameof(cache));

#if NET45
            cache.Add(key, value, expiration);
#else
            if (expiration == TimeSpan.Zero)
                return cache.Set(key, value);

            if (expiration > TimeSpan.Zero)
                return cache.Set(key, value, expiration);

            cache.Remove(key);
#endif
            return value;
        }

        /// <summary>
        /// Reads the value with specified key from the local cache. If it doesn't exists in cache, calls the loader 
        /// function to generate value (from database etc.) and adds it to the cache. If loader returns a null value, 
        /// it is written to the cache as DBNull.Value.
        /// </summary>
        /// <typeparam name="TItem">Data type</typeparam>
        /// <param name="cacheKey">Key</param>
        /// <param name="expiration">Expiration (TimeSpan.Zero means no expiration)</param>
        /// <param name="loader">Loader function that will be called if item doesn't exist in the cache.</param>
#if !NET45
        [Obsolete(Dependency.UseDI)]
#endif
        public static TItem Get<TItem>(string cacheKey, TimeSpan expiration, Func<TItem> loader)
            where TItem : class
        {
            return Get(Provider, cacheKey, expiration, loader);
        }

        /// <summary>
        /// Reads the value with specified key from the local cache. If it doesn't exists in cache, calls the loader 
        /// function to generate value (from database etc.) and adds it to the cache. If loader returns a null value, 
        /// it is written to the cache as DBNull.Value.
        /// </summary>
        /// <typeparam name="TItem">Data type</typeparam>
        /// <param name="cache">Cache</param>
        /// <param name="cacheKey">Key</param>
        /// <param name="expiration">Expiration (TimeSpan.Zero means no expiration)</param>
        /// <param name="loader">Loader function that will be called if item doesn't exist in the cache.</param>
        public static TItem Get<TItem>(this ILocalCache cache, KeyType cacheKey, TimeSpan expiration, Func<TItem> loader)
            where TItem : class
        {
            if (cache == null)
                throw new ArgumentNullException(nameof(cache));

            var cachedObj = cache.Get<object>(cacheKey);
            
            if (cachedObj == DBNull.Value)
            {
                return null;
            }

            if (cachedObj == null)
            {
                var item = loader();
                cache.Add(cacheKey, (object) item ?? DBNull.Value, expiration);
                return item;
            }

            return (TItem) cachedObj;
        }

        /// <summary>
        /// Reads the value of given type with specified key from the local cache. If the value doesn't exist or not
        /// of given type, it returns null.
        /// </summary>
        /// <typeparam name="TItem">Expected type</typeparam>
        /// <param name="cacheKey">Key</param>
#if !NET45
        [Obsolete(Dependency.UseDI)]
#endif
        public static TItem TryGet<TItem>(string cacheKey)
            where TItem : class
        {
            return TryGet<TItem>(Provider, cacheKey);
        }

        /// <summary>
        /// Reads the value of given type with specified key from the local cache. If the value doesn't exist or not
        /// of given type, it returns null.
        /// </summary>
        /// <typeparam name="TItem">Expected type</typeparam>
        /// <param name="cache">Cache</param>
        /// <param name="cacheKey">Key</param>
        public static TItem TryGet<TItem>(this ILocalCache cache, string cacheKey)
            where TItem : class
        {
            if (cache == null)
                throw new ArgumentNullException(nameof(cache));

            return cache.Get<object>(cacheKey) as TItem;
        }

        /// <summary>
        /// Removes the value with specified key from the local cache. If the value doesn't exist, no error is raised.
        /// </summary>
        /// <param name="cacheKey">Key</param>
#if !NET45
        [Obsolete(Dependency.UseDI)]
#endif
        public static object Remove(string cacheKey)
        {
#if NET45
            return Provider.Remove(cacheKey);
#else
            Provider.Remove(cacheKey);
            return null;
#endif
        }

        /// <summary>
        /// Removes all items from the cache (avoid except unit tests).
        /// </summary>
#if !NET45
        [Obsolete(Dependency.UseDI)]
#endif
        public static void RemoveAll()
        {
#if !NET45
            RemoveAll(Provider);
#else
            if (Provider == null)
                throw new ArgumentNullException(nameof(Provider));

            Provider.RemoveAll();
#endif
        }

#if !NET45
        /// <summary>
        /// Removes all items from the cache (avoid except unit tests).
        /// </summary>
        /// <param name="cache">Cache</param>
        public static void RemoveAll(this ILocalCache cache)
        {
            if (cache == null)
                throw new ArgumentNullException(nameof(cache));

            if (cache is MemoryCache memCache)
            {
                memCache.Compact(1.0);
                return;
            }

            throw new NotImplementedException();
        }
#endif
    }
}