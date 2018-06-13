using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DateTimeProviderService
{
    public class DateTimeProviderService : IHostedService, IDisposable
    {
        private readonly ILogger<DateTimeProviderService> _logger;
        private readonly DateTimeProviderConfig _dateTimeProviderConfig;

        private Timer _timer;
        private IDateTimeProvider _dateTimeProvider;

        public DateTimeProviderService(IDateTimeProvider dateTimeProvider, ILogger<DateTimeProviderService> logger, IOptions<DateTimeProviderConfig> config)
        {
            _dateTimeProvider = dateTimeProvider;
            _logger = logger;
            _dateTimeProviderConfig = config.Value;
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Starting {nameof(DateTimeProviderService)}");
            
            _timer = new Timer(ShowDateTime, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
            
            return Task.CompletedTask;
        }

        private void ShowDateTime(object state)
        {
           _logger.LogInformation("Now = {0}", _dateTimeProvider.SystemDateTime.ToString(_dateTimeProviderConfig.Formmatter));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Stoping {nameof(DateTimeProviderService)}");

            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
            
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}