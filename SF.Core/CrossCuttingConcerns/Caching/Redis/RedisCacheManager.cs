using System;
using Newtonsoft.Json;
using SF.Core.Utilities.ExtensionMethods;
using StackExchange.Redis;


namespace SF.Core.CrossCuttingConcerns.Caching.Redis
{
    /// <summary>
    /// RedisCacheManager
    /// </summary>
    public class RedisCacheManager : ICacheManager
    {
        //  private readonly ConnectionMultiplexer _redisEndpoint;

        private static Lazy<ConnectionMultiplexer> _lazyConnection;
        private static IDatabase _db;

        public RedisCacheManager()
        {


            var redisOptions = ConfigurationOptions.Parse(Config.Configuration["Redis:Host"] + ":" + Config.Configuration["Redis:Port"]); // host1:port1, host2:port2, ...
            redisOptions.Password = Config.Configuration["Redis:Password"];
            redisOptions.DefaultDatabase = Config.Configuration["Redis:DatabaseNum"].ToInt32();
            _lazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(redisOptions));
            _db = _lazyConnection.Value.GetDatabase();
        }

        public T Get<T>(string key)
        {
            key = $"{Config.Configuration["Cache:Prefix"]}{key.ToLowerInvariant()}";


            var res = _db.StringGet(key);

            if (res.IsNull)
                return default(T);
            else
                return JsonConvert.DeserializeObject<T>(res);


        }

        public object Get(string key)
        {
            return Get<object>(key);
        }

        public void Add(string key, object data, int duration)
        {
            key = $"{Config.Configuration["Cache:Prefix"]}{key.ToLowerInvariant()}";
            _db.StringSet(key, data.ToJson(), TimeSpan.FromMinutes(duration));
        }



        public bool IsAdd(string key)
        {
            key = $"{Config.Configuration["Cache:Prefix"]}{key.ToLowerInvariant()}";
            return _db.KeyExists(key);

        }

        public void Remove(string key)
        {
            key = $"{Config.Configuration["Cache:Prefix"]}{key.ToLowerInvariant()}";

            _db.KeyDelete(key);

        }

        public void RemoveByPattern(string pattern)
        {
            pattern = $"{Config.Configuration["Cache:Prefix"]}{pattern.ToLowerInvariant()}";

            var serverKeys = _lazyConnection.Value.GetServer(_lazyConnection.Value.GetEndPoints()[0]).Keys(pattern: pattern);
            foreach (var skey in serverKeys)
            {
                _db.KeyDelete(skey);

            }


        }

        public void Clear()
        {
            RemoveByPattern("*");
        }



        public System.Collections.Generic.List<string> GetList()
        {
            var list = new System.Collections.Generic.List<string>();


            var serverKeys = _lazyConnection.Value.GetServer(_lazyConnection.Value.GetEndPoints()[0]).Keys(pattern: $"{Config.Configuration["Cache:Prefix"]}*");
            foreach (var skey in serverKeys)
            {
                list.Add(skey.ToString().Replace(Config.Configuration["Cache:Prefix"], ""));
            }

            return list;
        }
    }
}