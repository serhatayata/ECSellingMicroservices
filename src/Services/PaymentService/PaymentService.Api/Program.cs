using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.Factory;
using PaymentService.Api.Extensions;
using PaymentService.Api.IntegrationEvents.EventHandlers;
using PaymentService.Api.IntegrationEvents.Events;
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

#region SERVICES
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddLogging(configure => {
    configure.AddConsole();
    configure.AddDebug();
});
builder.Services.AddTransient<OrderStartedIntegrationEventHandler>();

builder.Services.AddSingleton<IEventBus>(sp =>
{
    EventBusConfig config = new()
    {
        ConnectionRetryCount = 5,
        EventNameSuffix = "IntegrationEvent",
        SubscriberClientAppName = "PaymentService",
        EventBusType = EventBusType.RabbitMQ,
        Connection = new ConnectionFactory()
        {
            HostName = "c_rabbitmq"
        }
    };

    return EventBusFactory.Create(config, sp);
});
#endregion

var app = builder.Build();

#region PIPELINE
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
logger.LogInformation("System Up and Running - Payment Service");

app.MapControllers();

IEventBus eventBus = app.Services.GetRequiredService<IEventBus>();
eventBus.Subscribe<OrderStartedIntegrationEvent, OrderStartedIntegrationEventHandler>();

#endregion

app.Run();
