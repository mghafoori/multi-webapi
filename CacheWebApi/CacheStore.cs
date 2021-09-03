using System.Collections.Concurrent;

namespace Cache.WebApi
{
    public class CacheStore : ICacheStore
    {
        private readonly ConcurrentDictionary<string, string> _cache = new ConcurrentDictionary<string, string>();

        public bool Add(string key, string value)
        {
            if (_cache.ContainsKey(key))
            {
                return false;
            }
            return _cache.TryAdd(key, value);
        }

        public bool Delete(string key)
        {
            return _cache.TryRemove(key, out _);
        }

        public string Get(string key)
        {
            if (_cache.TryGetValue(key, out var value))
            {
                return value;
            }
            return null;
        }

        public bool Update(string key, string value)
        {
            var oldValue = Get(key);
            if (oldValue == null)
            {
                return false;
            }
            return _cache.TryUpdate(key, value, oldValue);
        }
    }
}