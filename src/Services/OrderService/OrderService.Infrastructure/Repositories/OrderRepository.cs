using OrderService.Application.Interfaces.Repositories;
using OrderService.Domain.AggregateModels.OrderAggregate;
using OrderService.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Infrastructure.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly OrderDbContext dbContext;
        public OrderRepository(OrderDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// After orderStarted we don't write any data to database
        /// so entity will be null, if we cannot find the data in database  we look at Local
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includes"></param>
        /// <returns>order</returns>
        public override async Task<Order> GetByIdAsync(Guid id, params Expression<Func<Order, object>>[] includes)
        {
            var entity = await base.GetByIdAsync(id, includes);

            if (entity == null)
                entity = dbContext.Orders.Local.FirstOrDefault(o => o.Id == id);

            return entity;
        } 
    }
}
