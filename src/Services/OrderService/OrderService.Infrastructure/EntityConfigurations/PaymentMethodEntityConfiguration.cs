using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.AggregateModels.BuyerAggregate;
using OrderService.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Infrastructure.EntityConfigurations
{
    public class PaymentMethodEntityConfiguration : IEntityTypeConfiguration<PaymentMethod>
    {
        public void Configure(EntityTypeBuilder<PaymentMethod> paymentMethodConfiguration)
        {
            paymentMethodConfiguration.ToTable("paymentmethods", OrderDbContext.DEFAULT_SCHEMA);

            paymentMethodConfiguration.Ignore(p => p.DomainEvents);
            paymentMethodConfiguration.HasKey(p => p.Id);

            paymentMethodConfiguration.Property(p => p.Id).HasColumnName("id").ValueGeneratedOnAdd();
            paymentMethodConfiguration.Property<int>("BuyerId").IsRequired();

            paymentMethodConfiguration.Property(p => p.CardHolderName)
                                      .UsePropertyAccessMode(PropertyAccessMode.Field)
                                      .HasColumnName("CardHolderName")
                                      .HasMaxLength(200)
                                      .IsRequired();

            paymentMethodConfiguration.Property(p => p.Alias)
                                      .UsePropertyAccessMode(PropertyAccessMode.Field)
                                      .HasColumnName("Alias")
                                      .HasMaxLength(200)
                                      .IsRequired();

            paymentMethodConfiguration.Property(p => p.CardNumber)
                                      .UsePropertyAccessMode(PropertyAccessMode.Field)
                                      .HasColumnName("CardNumber")
                                      .HasMaxLength(25)
                                      .IsRequired();

            paymentMethodConfiguration.Property(p => p.Expiration)
                                      .UsePropertyAccessMode(PropertyAccessMode.Field)
                                      .HasColumnName("Expiration")
                                      .HasMaxLength(25)
                                      .IsRequired();

            paymentMethodConfiguration.Property(p => p.CardTypeId)
                                      .UsePropertyAccessMode(PropertyAccessMode.Field)
                                      .HasColumnName("CardTypeId")
                                      .IsRequired();

            paymentMethodConfiguration.HasOne(p => p.CardType)
                                      .WithMany()
                                      .HasForeignKey(p => p.CardTypeId);
        }
    }
}
