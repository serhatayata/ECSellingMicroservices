using EventBus.Base.Events;

namespace PaymentService.Api.IntegrationEvents.Events
{
    public class OrderPaymentFailedIntegrationEvent:IntegrationEvent
    {
        public string OrderId { get; }
        public string ErrorMessage { get; }

        public OrderPaymentFailedIntegrationEvent(string orderId, string errorMessage)
        {
            OrderId = orderId;
            ErrorMessage = errorMessage;
        }
    }
}
