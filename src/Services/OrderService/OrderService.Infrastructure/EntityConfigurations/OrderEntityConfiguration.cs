using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.AggregateModels.OrderAggregate;
using OrderService.Infrastructure.Context;

namespace OrderService.Infrastructure.EntityConfigurations
{
    public class OrderEntityConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("orders", OrderDbContext.DEFAULT_SCHEMA);
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).ValueGeneratedOnAdd();

            builder.Ignore(o => o.DomainEvents);

            // With owns one with owner, address columns will be created on Order tables as Order_Street, Order_City...
            builder.OwnsOne(o => o.Address, a =>
            {
                a.WithOwner();
            });
            // orderStatusId is a private field for polymorphism, we can set orderStatusId with this way (Backing field)
            builder.Property<int>("orderStatusId")
                   .UsePropertyAccessMode(PropertyAccessMode.Field)
                   .HasColumnName("OrderStatusId")
                   .IsRequired();
             
            // We cannot set Order.OrderItems from outside, it has to be set with _orderItems and it is private
            var navigation = builder.Metadata.FindNavigation(nameof(Order.OrderItems));
            // We can reach this property with field access mode
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasOne(o => o.Buyer)
                   .WithMany()
                   .HasForeignKey(o => o.BuyerId);

            builder.HasOne(o => o.OrderStatus)
                   .WithMany()
                   .HasForeignKey("orderStatusId");
        }
    }
}
