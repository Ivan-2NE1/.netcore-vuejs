using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using VSAND.Services.Cache;

namespace VSAND.Test.Data
{
    public class CacheUtil
    {
        public static InMemoryCache GetInMemoryCache()
        {
            var services = new ServiceCollection();
            services.AddMemoryCache();

            var provider = services.BuildServiceProvider();

            return new InMemoryCache(provider.GetService<IMemoryCache>());
        }
    }
}
