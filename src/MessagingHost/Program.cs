using Amido.Stacks.Configuration;
using Amido.Stacks.Configuration.Extensions;
using Amido.Stacks.Messaging.Azure.ServiceBus.Configuration;
using Amido.Stacks.Messaging.Azure.ServiceBus.Events;
using Amido.Stacks.Messaging.Azure.ServiceBus.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using TestCommon;
using TestHost;
using TestHost.ServiceBusListener;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddSecrets()
            .AddTransient(typeof(ILogger<>), typeof(LogAdapter<>))
            //.Configure<ServiceBusSubscriptionListenerConfiguration>(serviceBusConfiguration =>
            //{
            //    serviceBusConfiguration.Name = "notification-event";
            //    serviceBusConfiguration.SubscriptionName = "notification-event";
            //    serviceBusConfiguration.ConcurrencyLevel = 1;
            //    serviceBusConfiguration.ConnectionStringSecret = new Secret
            //    {
            //        Identifier = "SERVICEBUS_CONNECTIONSTRING",
            //        Source = "Environment"
            //    };
            //})
            .AddOptions()
            .AddLogging(loggingBuilder =>
                loggingBuilder.AddSerilog(
                    new LoggerConfiguration()
                        .MinimumLevel.Information()
                        .Enrich.FromLogContext()
                        .Enrich.WithThreadId()
                        .Enrich.WithMachineName()
                        .WriteTo.Console(
                            outputTemplate:
                            "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}",
                            restrictedToMinimumLevel: LogEventLevel.Verbose)
                        .CreateLogger()))
            .AddTransient<IEventHandler<NotifyEvent>, NotifyEventHandler>()
            .AddServiceBus(serviceBusConfiguration =>
                serviceBusConfiguration.Listener = new ServiceBusListenerConfiguration
                {
                    Topics = new[]
                    {
                        new ServiceBusSubscriptionListenerConfiguration
                        {
                            Name = "notification-event",
                            SubscriptionName = "notification-event",
                            ConcurrencyLevel = 1,
                            ConnectionStringSecret = new Secret
                            {
                                Identifier = "SERVICEBUS_CONNECTIONSTRING",
                                Source = "Environment"
                            }
                        }
                    }
                })
            //.AddHostedService<ServiceBusListenerHostedService>()
            //.AddTransient<IServiceBusListener, NotifyEventServiceBusListener>()
            ;
    })
    .ConfigureAppConfiguration(builder =>
    {
        // Add the configuration file
        builder.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true);
    })
    .Build(); // Build the host, as per configurations.

await host.RunAsync();
