using MediatR;
using OrderService.Application.Interfaces.Repositories;
using OrderService.Domain.AggregateModels.BuyerAggregate;
using OrderService.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.DomainEventHandlers
{
    /// <summary>
    /// When order started in this layer, I'd like to do something here. But only in Order domain.
    /// </summary>
    public class OrderStartedDomainEventHandler:INotificationHandler<OrderStartedDomainEvent>
    {
        private readonly IBuyerRepository buyerRepository;

        public OrderStartedDomainEventHandler(IBuyerRepository buyerRepository)
        {
            this.buyerRepository = buyerRepository;
        }

        public async Task Handle(OrderStartedDomainEvent orderStartedEvent, CancellationToken cancellationToken)
        {
            var cardTypeId = (orderStartedEvent.CardTypeId != 0) ? orderStartedEvent.CardTypeId : 1;

            var buyer = await buyerRepository.GetSingleAsync(i => i.Name == orderStartedEvent.UserName, i => i.PaymentMethods);

            bool buyerOriginallyExisted = buyer != null;

            if (!buyerOriginallyExisted)
                buyer = new Buyer(orderStartedEvent.UserName);

            buyer.VerifyOrAddPaymentMethod(cardTypeId, $"Payment Method on {DateTime.UtcNow}",
                                           orderStartedEvent.CardNumber, orderStartedEvent.CardSecurityNumber,
                                           orderStartedEvent.CardHolderName, orderStartedEvent.CardExpiration, orderStartedEvent.Order.Id);

            var buyerUpdated = buyerOriginallyExisted ? buyerRepository.Update(buyer) : await buyerRepository.AddAsync(buyer);

            await buyerRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            // buyer created if not exists but order current order doesn't have any buyer now... so we should use UpdateOrderWhenBuyerAndPaymentMethodVerifiedDomainEventHandler

            // order status changed event may be fired here.
        }
    }
}
