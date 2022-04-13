using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using SF.Core.Utilities.IoC;

namespace SF.Core.CrossCuttingConcerns.Caching.Microsoft
{
    public class DistributedCacheManager : ICacheManager
    {
        private IDistributedCache _cache;
        public DistributedCacheManager()
        {
           _cache =  ServiceTool.ServiceProvider.GetService<IDistributedCache>();
        }
        public T Get<T>(string key)
        {

            var value = _cache.GetString(key);
            return string.IsNullOrEmpty(value) ? default(T) : JsonSerializer.Deserialize<T>(value);
        }

        public object Get(string key)
        {
           return Get<object>(key);
        }

        public void Add(string key, object data, int duration)
        {
            _cache.SetString(key, JsonSerializer.Serialize(data), new DistributedCacheEntryOptions {AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(duration) });

        }

        public bool IsAdd(string key)
        {
         
            return !string.IsNullOrEmpty(_cache.GetString(key));
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }

        public void RemoveByPattern(string pattern)
        {
            throw new NotImplementedException(".net7 de gelecek");    
        }


        public List<string> GetList()
        {
            throw new NotImplementedException(".net7 de gelecek");

        }
    }
}
