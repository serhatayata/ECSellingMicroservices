using MediatR;
using OrderService.Domain.SeedWork;
using OrderService.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Infrastructure.Extensions
{
    public static class MediatorExtension
    {
        public static async Task DispatchDomainEventsAsync(this IMediator mediator, OrderDbContext ctx)
        {
            // With ChangeTracker (changed items), this will get all BaseEntity entities
            var domainEntities = ctx.ChangeTracker.Entries<BaseEntity>().Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());
            
            var domainEvents = domainEntities.SelectMany(x => x.Entity.DomainEvents).ToList();

            domainEntities.ToList().ForEach(entity => entity.Entity.ClearDomainEvents());

            foreach (var item in domainEvents)
                await mediator.Publish(domainEvents);
        }
    }
}
