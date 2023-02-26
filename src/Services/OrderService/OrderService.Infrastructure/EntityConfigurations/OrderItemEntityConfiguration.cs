using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.AggregateModels.OrderAggregate;
using OrderService.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Infrastructure.EntityConfigurations
{
    public class OrderItemEntityConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> orderItemConfiguration)
        {
            // Table named orderItems created with schema
            orderItemConfiguration.ToTable("orderItems", OrderDbContext.DEFAULT_SCHEMA);
            // Primary key set
            orderItemConfiguration.HasKey(o => o.Id);
            // Domain events ignored
            orderItemConfiguration.Ignore(o => o.DomainEvents);
            // Id value generated while adding the database
            orderItemConfiguration.Property(o => o.Id).ValueGeneratedOnAdd();
            orderItemConfiguration.Property<int>("OrderId").IsRequired();
        }
    }
}
