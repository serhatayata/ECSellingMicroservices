using EventBus.Base.Events;

namespace PaymentService.Api.IntegrationEvents.Events
{
    public class OrderStartedIntegrationEvent:IntegrationEvent
    {
        public string OrderId { get; set; }

        public OrderStartedIntegrationEvent()
        {

        }

        public OrderStartedIntegrationEvent(string orderId)
        {
            OrderId = orderId;
        }
    }
}
