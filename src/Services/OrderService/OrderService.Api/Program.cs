using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.Factory;
using OrderService.Api.Extensions;
using OrderService.Api.Extensions.Registration;
using OrderService.Api.Extensions.Registration.EventHandlerRegistration;
using OrderService.Api.Extensions.Registration.ServiceDiscovery;
using OrderService.Api.IntegrationEvents.EventHandlers;
using OrderService.Api.IntegrationEvents.Events;
using OrderService.Application;
using OrderService.Infrastructure;
using OrderService.Infrastructure.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
                     .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                     .AddEnvironmentVariables();

ConfigurationManager configuration = builder.Configuration;
IWebHostEnvironment environment = builder.Environment;

//builder.Host.UseDefaultServiceProvider((context, options) =>
//{
//    options.ValidateOnBuild = false;
//}).ConfigureAppConfiguration(i => i.AddConfiguration(configuration));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging(configure => configure.AddConsole())
                .AddApplicationRegistration()
                .AddPersistenceRegistration(configuration)
                .ConfigureEventHandlers()
                .AddServiceDiscoveryRegistration(configuration);

builder.Services.AddSingleton<IEventBus>(sp =>
{
    EventBusConfig config = new()
    {
        ConnectionRetryCount = 5,
        EventNameSuffix = "IntegrationEvent",
        SubscriberClientAppName = "OrderService",
        EventBusType = EventBusType.RabbitMQ
    };

    return EventBusFactory.Create(config, sp);
});

var app = builder.Build();

app.MigrateDbContext<OrderDbContext>(async (context, services) =>
{
    var logger = services.GetRequiredService<ILogger<OrderDbContext>>();

    var dbContextSeeder = new OrderDbContextSeed();
    await dbContextSeeder.SeedAsync(context, logger);
    
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

#region EventHandlers
var eventBus = app.Services.GetRequiredService<IEventBus>();

eventBus.Subscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();
#endregion

app.Start();

app.RegisterWithConsul(app.Lifetime);

app.WaitForShutdown();

