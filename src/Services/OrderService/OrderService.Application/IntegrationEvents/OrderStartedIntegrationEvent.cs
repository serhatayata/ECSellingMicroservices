using EventBus.Base.Events;

namespace OrderService.Application.IntegrationEvents
{
    public class OrderStartedIntegrationEvent : IntegrationEvent
    {
        public string UserName { get; set; }
        public string OrderId { get; set; }
        public OrderStartedIntegrationEvent(string userName, string orderId)
        {
            UserName = userName;
            OrderId = orderId;
        }
    }
}
