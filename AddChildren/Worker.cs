using Microsoft.Extensions.Hosting;
using AddChildren.Data;

namespace AddChildren
{
    internal class Worker : BackgroundService
    {
        private readonly ServiceBus.ISubscriptionReceiver _subscriptionReceiver;

        public Worker(ServiceBus.ISubscriptionReceiver subscriptionReceiver)
        {
            _subscriptionReceiver = subscriptionReceiver;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await _subscriptionReceiver.ProcessMessagesAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            await _subscriptionReceiver.StopProcessingAsync();
        }
    }
}