using EventBus.Base.Events;

namespace PaymentService.Api.IntegrationEvents.Events
{
    public class OrderPaymentSuccessIntegrationEvent:IntegrationEvent
    {
        public string OrderId { get; set; }
        public OrderPaymentSuccessIntegrationEvent(string orderId) => OrderId = orderId;
    }
}
