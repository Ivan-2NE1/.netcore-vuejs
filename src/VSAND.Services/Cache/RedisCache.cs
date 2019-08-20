using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace VSAND.Services.Cache
{
    public class RedisCache : ICache
    {
        private readonly ConnectionMultiplexer _redis;

        public RedisCache(IConfiguration configuration)
        {
            var redisConfig = new ConfigurationOptions();
            redisConfig.Password = configuration["RedisPassword"];
            redisConfig.EndPoints.Add(configuration["RedisUrl"]);

            _redis = ConnectionMultiplexer.Connect(redisConfig);
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var db = _redis.GetDatabase();

            string sVal = await db.StringGetAsync(key.ToUpperInvariant());
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
        }

        public async Task SetAsync<T>(string key, T value)
        {
            if (value == null)
            {
                return;
            }

            var db = _redis.GetDatabase();

            string sVal = JsonConvert.SerializeObject(value);
            await db.StringSetAsync(key.ToUpperInvariant(), sVal);
        }

        public async Task RemoveAsync(string key)
        {
            var db = _redis.GetDatabase();
            await db.KeyDeleteAsync(key.ToUpperInvariant());
        }
    }
}
