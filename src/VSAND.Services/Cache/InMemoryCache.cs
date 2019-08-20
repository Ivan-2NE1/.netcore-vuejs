using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace VSAND.Services.Cache
{
    public class InMemoryCache : ICache
    {
        private readonly IMemoryCache _cache;
        public InMemoryCache(IMemoryCache cache)
        {
            _cache = cache;
        }

        public Task<T> GetAsync<T>(string key)
        {
            return Task.Run(() =>
            {
                string sVal = _cache.Get<string>(key.ToUpperInvariant());
                if (sVal == null || sVal == "")
                {
                    return default;
                }

                try
                {
                    return JsonConvert.DeserializeObject<T>(sVal);
                }
                catch
                {
                    return default;
                }
                // Made this behave like the redis cache implementation
                //return _cache.Get<T>(key.ToUpperInvariant());
            });
        }

        public Task SetAsync<T>(string key, T value)
        {
            return Task.Run(() =>
            {
                if (value == null)
                {
                    return;
                }

                string sVal = JsonConvert.SerializeObject(value);
                _cache.Set(key.ToUpperInvariant(), sVal);

                // Made this behave like the redis cache implementation
                //_cache.Set(key.ToUpperInvariant(), value);
            });
        }

        public Task RemoveAsync(string key)
        {
            return Task.Run(() =>
            {
                _cache.Remove(key.ToUpperInvariant());
            });
        }
    }
}

