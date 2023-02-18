using EventBus.Base.Abstraction;
using Microsoft.Extensions.Logging;
using NotificationService.IntegrationEvents.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.IntegrationEvents.EventHandlers
{
    public class OrderPaymentSuccessIntegrationEventHandler : IIntegrationEventHandler<OrderPaymentSuccessIntegrationEvent>
    {
        private readonly ILogger<OrderPaymentSuccessIntegrationEvent> logger;

        public OrderPaymentSuccessIntegrationEventHandler(ILogger<OrderPaymentSuccessIntegrationEvent> logger)
        {
            this.logger = logger;
        }

        public Task Handle(OrderPaymentSuccessIntegrationEvent @event)
        {
            //Send success email or notification sms or push...

            logger.LogInformation($"Order payment completed successfully with OrderId {@event.OrderId}");

            return Task.CompletedTask;
        }
    }
}
