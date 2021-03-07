using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rebus.Bus;
using SagaPatternLibraryComparison.Domain.Orders.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SagaPatternLibraryComparison.Rebus
{
    public class OrderWorkerService : BackgroundService
    {
        private readonly IBus _bus;
        private readonly ILogger<OrderWorkerService> _logger;

        public OrderWorkerService(
            IBus bus,
            ILogger<OrderWorkerService> logger)
        {
            _bus = bus;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(3000, stoppingToken);
            }
        }
    }
}
