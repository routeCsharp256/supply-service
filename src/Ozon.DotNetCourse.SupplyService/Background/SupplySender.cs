using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ozon.DotNetCourse.SupplyService.Domain.Entities;
using Ozon.DotNetCourse.SupplyService.Domain.Interfaces.Services;

namespace Ozon.DotNetCourse.SupplyService.Background
{
    public class SupplySender: BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger _logger;
        public SupplySender(IServiceScopeFactory serviceScopeFactory, ILogger<SupplySender> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            return StartSender(cancellationToken);
        }

        private async Task StartSender(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var supplyService = scope.ServiceProvider.GetRequiredService<ISupplyService>();
                    var now = DateTimeOffset.UtcNow;
                    var supplies = await supplyService.Get(SupplyState.Created, now.AddMinutes(-1),
                        cancellationToken);

                    foreach (var supply in supplies)
                    {
                        await supplyService.Ship(supply, cancellationToken);
                    }
                }
                catch(Exception ex)
                {
                    _logger.LogError($"Failed to ship supplies: {ex.Message}");
                }

                await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);
            }
        }
    }
}