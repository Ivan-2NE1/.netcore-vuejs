using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace VSAND.Test.Data
{
    public class HubUtil
    {
        public static IHubContext<T> GetHub<T>() where T : Hub
        {
            var services = new ServiceCollection();
            services.AddSignalRCore();
            services.AddLogging();

            var provider = services.BuildServiceProvider();

            return provider.GetService<IHubContext<T>>();
        }
    }
}
