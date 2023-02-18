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
    public class OrderPaymentFailedIntegrationEventHandler : IIntegrationEventHandler<OrderPaymentFailedIntegrationEvent>
    {
        private readonly ILogger<OrderPaymentFailedIntegrationEvent> logger;

        public OrderPaymentFailedIntegrationEventHandler(ILogger<OrderPaymentFailedIntegrationEvent> logger)
        {
            this.logger = logger;
        }

        public Task Handle(OrderPaymentFailedIntegrationEvent @event)
        {
            //Send failed email or notification sms or push...

            logger.LogInformation($"Order payment failed with OrderId {@event.OrderId}, Error Message : {@event.ErrorMessage}");

            return Task.CompletedTask;
        }
    }
}
