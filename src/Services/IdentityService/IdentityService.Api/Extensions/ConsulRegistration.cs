﻿using Consul;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;

namespace IdentityService.Api.Extensions
{
    public static class ConsulRegistration
    {
        public static IServiceCollection ConfigureConsul(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
            {
                var address = configuration["ConsulConfig:Address"];
                consulConfig.Address = new Uri(address);
            }));

            return services;
        }

        public static IApplicationBuilder RegisterWithConsul(this IApplicationBuilder app, IHostApplicationLifetime lifeTime)
        {
            var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
            var loggingFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();
            var server = app.ApplicationServices.GetRequiredService<IServer>();

            var logger = loggingFactory.CreateLogger<IApplicationBuilder>();

            //Get server IP address
            var addressFeature = server.Features.Get<IServerAddressesFeature>();
            var addresses = addressFeature.Addresses;
            var address = addresses.First();

            //Register service with consul
            var uri = new Uri(address);
            var registration = new AgentServiceRegistration()
            {
                ID = "IdentityService",
                Name = "IdentityService",
                Address = $"{uri.Host}",
                Port = uri.Port,
                Tags = new[] { "Identity Service", "Identity", "Token", "JWT" }
            };

            logger.LogInformation("Registering with consul");
            consulClient.Agent.ServiceDeregister(registration.ID).Wait();
            consulClient.Agent.ServiceRegister(registration).Wait();

            //When application stops, this service will be deregistered.
            lifeTime.ApplicationStopping.Register(() =>
            {
                logger.LogInformation("Deregistering from Consul");
                consulClient.Agent.ServiceDeregister(registration.ID).Wait();
            });

            return app;
        }
    }
}