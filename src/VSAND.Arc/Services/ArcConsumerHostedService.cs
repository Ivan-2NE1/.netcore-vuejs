using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using System;
using System.Threading;
using System.Threading.Tasks;
using VSAND.Arc.Data;

namespace VSAND.Arc.Services
{
    // adapted from https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-2.2&tabs=visual-studio#timed-background-tasks
    public class ArcConsumerHostedService : IHostedService, IDisposable
    {
        private readonly Logger Log = LogManager.GetCurrentClassLogger();
        private Timer _timer;

        private bool _syncLock = false;

        private readonly IServiceProvider _provider;

        public ArcConsumerHostedService(IServiceProvider provider)
        {
            _provider = provider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Log.Info("Arc consumer background service is starting.");

            _timer = new Timer(StartUpdateRoutine, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));

            return Task.CompletedTask;
        }

        private async void StartUpdateRoutine(object state)
        {
            if (_syncLock)
            {
                return;
            }

            _syncLock = true;

            Log.Info("Arc consumer background service is working.");

            try
            {
                using (IServiceScope scope = _provider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ArcContentContext>();

                    await Util.Kinesis.UpdateStoriesFromStream(context);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
            }

            _syncLock = false;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Log.Info("Arc consumer background service is stopping.");

            // stop the timer
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}


