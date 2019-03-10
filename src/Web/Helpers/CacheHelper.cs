using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Web.Data;
using Z.EntityFramework.Plus;
using System.Runtime.Caching;
using Web.Enum;
using Web.Data.Models;
using Web.Models.Api.Sensor;
using AutoMapper;
using System.Web.Caching;

namespace Web.Helpers
{
    public static class CacheHelper
    {
        public static long Count => MemoryCache.Default.GetCount();

        public static void AddOrUpdate<T>(string key, T value) where T : class
        {
            Set<T>(key, value, new CacheItemPolicy
            {
                AbsoluteExpiration = DateTime.UtcNow.Add(_defaultTimeDurationInCache)
            });
        }

        public static void AddOrUpdate<T>(string key, T value, DateTimeOffset absoluteExpiration) where T : class
        {
            Set<T>(key, value, new CacheItemPolicy
            {
                AbsoluteExpiration = absoluteExpiration
            });
        }

        public static void AddOrUpdate<T>(string key, T value, TimeSpan slidingExpiration) where T : class
        {
            Set<T>(key, value, new CacheItemPolicy
            {
                SlidingExpiration = slidingExpiration
            });
        }

        public static void Clear()
        {
            MemoryCache.Default.Trim(100);
        }

        public static bool Contains(string key)
        {
            return MemoryCache.Default.Contains(key);
        }

        public static T Get<T>(string key) where T : class
        {
            var obj = MemoryCache.Default.Get(key);

            return obj as T;
        }

        public static T? GetValue<T>(string key) where T : struct
        {
            var obj = MemoryCache.Default.Get(key);

            if (obj is T)
            {
                return (T)obj;
            }
            return null;
        }

        public static T GetOrAdd<T>(string key, Func<T> valueFunc) where T : class
        {
            return GetOrAdd<T>(key, valueFunc, new CacheItemPolicy
            {
                AbsoluteExpiration = DateTime.UtcNow.Add(_defaultTimeDurationInCache)
            });
        }

        public static T GetOrAdd<T>(string key, Func<T> valueFunc, DateTimeOffset absoluteExpiration) where T : class
        {
            return GetOrAdd<T>(key, valueFunc, new CacheItemPolicy
            {
                AbsoluteExpiration = absoluteExpiration
            });
        }

        public static T GetOrAdd<T>(string key, Func<T> valueFunc, TimeSpan slidingExpiration) where T : class
        {
            return GetOrAdd<T>(key, valueFunc, new CacheItemPolicy
            {
                SlidingExpiration = slidingExpiration
            });
        }

        public static T? GetOrAddValue<T>(string key, Func<T> valueFunc, TimeSpan slidingExpiration) where T : struct
        {
            return GetOrAddValue<T>(key, valueFunc, new CacheItemPolicy
            {
                SlidingExpiration = slidingExpiration
            });
        }

        public static async Task<T?> GetOrAddValueAsync<T>(string key, Func<Task<T>> valueFunc, TimeSpan slidingExpiration) where T : struct
        {
            return await GetOrAddValueAsync<T>(key, valueFunc, new CacheItemPolicy
            {
                SlidingExpiration = slidingExpiration
            });
        }

        public static async Task<T?> GetOrAddValueAsync<T>(string key, Func<Task<T>> valueFunc, DateTimeOffset absoluteExpiration) where T : struct
        {
            return await GetOrAddValueAsync<T>(key, valueFunc, new CacheItemPolicy
            {
                AbsoluteExpiration = absoluteExpiration
            });
        }

        public static async Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> valueFunc) where T : class
        {
            return await GetOrAddAsync<T>(key, valueFunc, new CacheItemPolicy
            {
                AbsoluteExpiration = DateTime.UtcNow.Add(_defaultTimeDurationInCache)
            });
        }

        public static async Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> valueFunc,
            DateTimeOffset absoluteExpiration) where T : class
        {
            return await GetOrAddAsync<T>(key, valueFunc, new CacheItemPolicy
            {
                AbsoluteExpiration = absoluteExpiration
            });
        }

        public static async Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> valueFunc,
            TimeSpan slidingExpiration) where T : class
        {
            return await GetOrAddAsync<T>(key, valueFunc, new CacheItemPolicy
            {
                SlidingExpiration = slidingExpiration
            });
        }

        public static void Remove(string key)
        {
            MemoryCache.Default.Remove(key);
        }

        public static void Remove(Func<KeyValuePair<string, object>, bool> where)
        {
            var keys = MemoryCache.Default.Where(@where).Select(s => s.Key).ToList();

            if (keys.Any())
            {
                keys.ForEach(Remove);
            }
        }

        private static async Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> valueFunc, CacheItemPolicy cacheItemPolicy) where T : class
        {
            var valueInCache = default(T);

            if (Contains(key))
            {
                valueInCache = Get<T>(key);

                if (valueInCache == default(T))     // this key was used by other object type
                {
                    Set(key, valueInCache = (T)await valueFunc(), cacheItemPolicy);
                }
            }
            else
            {
                Set(key, valueInCache = (T)await valueFunc(), cacheItemPolicy);
            }

            return valueInCache;
        }

        private static T GetOrAdd<T>(string key, Func<T> valueFunc, CacheItemPolicy cacheItemPolicy) where T : class
        {
            var valueInCache = default(T);

            if (Contains(key))
            {
                valueInCache = Get<T>(key);

                if (valueInCache == default(T))     // this key was used by other object type
                {
                    valueInCache = (T)valueFunc();
                    Set(key, valueInCache, cacheItemPolicy);
                }
            }
            else
            {
                valueInCache = (T)valueFunc();
                Set(key, valueInCache, cacheItemPolicy);
            }

            return valueInCache;
        }

        private static async Task<T?> GetOrAddValueAsync<T>(string key, Func<Task<T>> valueFunc, CacheItemPolicy cacheItemPolicy) where T : struct
        {
            var valueInCache = default(T?);

            if (Contains(key))
            {
                valueInCache = GetValue<T>(key);

                if (valueInCache.Equals(default(T?)))     // this key was used by other object type
                {
                    SetValue<T>(key, (T)await valueFunc(), cacheItemPolicy);
                }
            }
            else
            {
                SetValue<T>(key, (T)await valueFunc(), cacheItemPolicy);
            }

            return valueInCache ?? GetValue<T>(key);
        }

        private static T? GetOrAddValue<T>(string key, Func<T> valueFunc, CacheItemPolicy cacheItemPolicy) where T : struct
        {
            var valueInCache = default(T?);

            if (Contains(key))
            {
                valueInCache = GetValue<T>(key);

                if (valueInCache.Equals(default(T?)))     // this key was used by other object type
                {
                    SetValue<T>(key, (T)valueFunc(), cacheItemPolicy);
                }
            }
            else
            {
                SetValue<T>(key, (T)valueFunc(), cacheItemPolicy);
            }

            return valueInCache ?? GetValue<T>(key);
        }

        private static void Set<T>(string key, T value, CacheItemPolicy cacheItemPolicy) where T : class
        {
            if (value != default(T))
            {
                MemoryCache.Default.Set(key, value, cacheItemPolicy);
            }
        }

        private static void SetValue<T>(string key, T value, CacheItemPolicy cacheItemPolicy) where T : struct
        {
            MemoryCache.Default.Set(key, value, cacheItemPolicy);
        }

        private static TimeSpan _defaultTimeDurationInCache = new TimeSpan(1, 0, 0);   // 1 hour
    }
}