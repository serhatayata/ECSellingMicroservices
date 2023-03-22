using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.Factory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NotificationService.Extensions;
using NotificationService.IntegrationEvents.EventHandlers;
using NotificationService.IntegrationEvents.Events;
using RabbitMQ.Client;
using Serilog;

ServiceCollection services = new();

#region ConfigureServices
services.AddLogging(configure =>
{
    configure.AddSerilog();
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
        EventBusType = EventBusType.RabbitMQ,
        Connection = new ConnectionFactory()
        {
            HostName = "c_rabbitmq",
            Port = 5672,
            UserName = "guest",
            Password = "guest",
            Ssl =
            {
                Enabled = true
            }
        }
    };

    return EventBusFactory.Create(config, sp);
});
#endregion

var sp = services.BuildServiceProvider();

var host = AppStartup();

var logger = sp.GetRequiredService<ILogger<Program>>();
logger.LogInformation("System Up and Running - Notification Service");

IEventBus eventBus = sp.GetRequiredService<IEventBus>();
eventBus.Subscribe<OrderPaymentSuccessIntegrationEvent, OrderPaymentSuccessIntegrationEventHandler>();
eventBus.Subscribe<OrderPaymentFailedIntegrationEvent, OrderPaymentFailedIntegrationEventHandler>();

Console.WriteLine("App is running.");
Console.ReadKey();

#region AppSettings
//static void BuildConfig(IConfigurationBuilder builder)
//{
//    // Check the current directory that the application is running on 
//    // Then once the file 'appsetting.json' is found, we are adding it.
//    // We add env variables, which can override the configs in appsettings.json
//    builder.SetBasePath(Directory.GetCurrentDirectory())
//        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
//        .AddEnvironmentVariables();
//}
#endregion

static IHost AppStartup()
{
    var builder = new ConfigurationBuilder();

    #region AppSettings
    //BuildConfig(builder);

    // Specifying the configuration for serilog
    //Log.Logger = new LoggerConfiguration() // initiate the logger configuration
    //                .ReadFrom.Configuration(builder.Build()) // connect serilog to our configuration folder
    //                .Enrich.FromLogContext() //Adds more information to our logs from built in Serilog 
    //                .WriteTo.Console() // decide where the logs are going to be shown
    //                .CreateLogger(); //initialise the logger
    #endregion

    var serilogConf = ConfigurationExtension.serilogConfig;

    Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(serilogConf)
                    .CreateLogger();

    var host = Host.CreateDefaultBuilder() // Initialising the Host 
                .ConfigureServices((context, services) => { // Adding the DI container for configuration

                })
                .UseSerilog() // Add Serilog
                .Build(); // Build the Host

    return host;
}