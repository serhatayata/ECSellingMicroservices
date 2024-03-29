using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.Factory;
using OrderService.Api.Extensions;
using OrderService.Api.Extensions.Registration.EventHandlerRegistration;
using OrderService.Api.Extensions.Registration.ServiceDiscovery;
using OrderService.Api.IntegrationEvents.EventHandlers;
using OrderService.Api.IntegrationEvents.Events;
using OrderService.Application;
using OrderService.Infrastructure;
using OrderService.Infrastructure.Context;
using RabbitMQ.Client;
using Serilog;

var config = ConfigurationExtension.appConfig;
var serilogConf = ConfigurationExtension.serilogConfig;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddConfiguration(config);
builder.Host.UseDefaultServiceProvider((context, options) =>
{
    options.ValidateOnBuild = false;
    options.ValidateScopes = false;
});

builder.Host.ConfigureLogging(s => s.ClearProviders()) // Remove all added providers before
             // https://github.com/serilog/serilog-aspnetcore
            .UseSerilog(
            //(context, serv, cfg) =>
            //{
            //cfg.ReadFrom.Configuration(context.Configuration)
            //   .ReadFrom.Services(serv)
            //   .Enrich.FromLogContext()
            //   .WriteTo.Console();
            //}, writeToProviders: true
            );

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
        EventBusType = EventBusType.RabbitMQ,
        Connection = new ConnectionFactory()
        {
            HostName = "c_rabbitmq"
        }
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

Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(serilogConf)
                .CreateLogger();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("System Up and Running - Order Service");

app.MapControllers();

app.Start();

app.RegisterWithConsul(app.Lifetime, configuration);

var eventBus = app.Services.GetRequiredService<IEventBus>();

eventBus.Subscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();

app.WaitForShutdown();

