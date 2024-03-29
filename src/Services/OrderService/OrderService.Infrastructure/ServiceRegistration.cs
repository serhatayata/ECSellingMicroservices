﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Interfaces.Repositories;
using OrderService.Infrastructure.Context;
using OrderService.Infrastructure.Repositories;

namespace OrderService.Infrastructure
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddPersistenceRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<OrderDbContext>(opt =>
            {
                opt.UseSqlServer(configuration["OrderDbConnectionString"]);
                opt.EnableSensitiveDataLogging();
            }, ServiceLifetime.Transient);

            services.AddTransient<IBuyerRepository, BuyerRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();

            var optionsBuilder = new DbContextOptionsBuilder<OrderDbContext>()
                                .UseSqlServer(configuration["OrderDbConnectionString"]);

            using var dbContext = new OrderDbContext(optionsBuilder.Options, null);
            dbContext.Database.EnsureCreated();
            dbContext.Database.Migrate();

            return services;
        }
    }
}
