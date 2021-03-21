using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NLog;

namespace Console
{
    public sealed class Service : IHostedService, IDisposable
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public Service(IConfiguration configuration, ILogger logger)
        {
            _configuration = configuration;
            _logger = logger;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            _logger.Info("ServiceStarted");
        }



        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.Info("ServiceStopped");
            _cancellationTokenSource.Cancel();
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _cancellationTokenSource.Dispose();

            try
            {

                LogManager.Shutdown();
            }
            catch
            {
                //shallow
            }
        }

    }
}