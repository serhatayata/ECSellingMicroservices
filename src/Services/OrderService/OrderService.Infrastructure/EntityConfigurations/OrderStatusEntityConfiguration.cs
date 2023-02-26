﻿using Microsoft.EntityFrameworkCore;
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
    public class OrderStatusEntityConfiguration : IEntityTypeConfiguration<OrderStatus>
    {
        public void Configure(EntityTypeBuilder<OrderStatus> orderStatusConfiguration)
        {
            orderStatusConfiguration.ToTable("orderstatus", OrderDbContext.DEFAULT_SCHEMA);

            orderStatusConfiguration.HasKey(o => o.Id);
            orderStatusConfiguration.Property(o => o.Id).ValueGeneratedOnAdd();

            orderStatusConfiguration.Property(o => o.Id)
                                    .HasDefaultValue(1)
                                    .ValueGeneratedNever()
                                    .IsRequired();

            orderStatusConfiguration.Property(o => o.Name)
                                    .HasMaxLength(200)
                                    .IsRequired();
        }
    }
}
