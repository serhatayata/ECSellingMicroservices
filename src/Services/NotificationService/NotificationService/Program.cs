using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.Factory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NotificationService.IntegrationEvents.EventHandlers;
using NotificationService.IntegrationEvents.Events;

ServiceCollection services = new();

#region ConfigureServices
services.AddLogging(configure => {
    configure.AddConsole();
});

services.AddTransient<OrderPaymentFailedIntegrationEventHandler>();
services.AddTransient<OrderPaymentSuccessIntegrationEventHandler>();

services.AddSingleton<IEventBus>(sp =>
{
    EventBusConfig config = new()
    {
        ConnectionRetryCount = 5,
        EventNameSuffix = "IntegrationEvent",
        SubscriberClientAppName = "NotificationService",
        EventBusType = EventBusType.RabbitMQ
    };

    return EventBusFactory.Create(config, sp);
});
#endregion

var sp = services.BuildServiceProvider();

IEventBus eventBus = sp.GetRequiredService<IEventBus>();
eventBus.Subscribe<OrderPaymentSuccessIntegrationEvent, OrderPaymentSuccessIntegrationEventHandler>();
eventBus.Subscribe<OrderPaymentFailedIntegrationEvent, OrderPaymentFailedIntegrationEventHandler>();

Console.WriteLine("App is running.");
Console.ReadLine();