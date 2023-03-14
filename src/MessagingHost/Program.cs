using Amido.Stacks.Configuration.Extensions;
using Amido.Stacks.Messaging.Azure.ServiceBus.Configuration;
using Amido.Stacks.Messaging.Azure.ServiceBus.Events;
using MessagingHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TestCommon;
using TestHost;

const string serviceBusConfigurationKeyName = "ServiceBusConfiguration";

var host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging(builder => builder.AddConsole())
    .ConfigureServices((hostContext, services) =>
    {
        services.AddSecrets()
            .AddTransient(typeof(ILogger<>), typeof(LogAdapter<>))
            .Configure<ServiceBusConfiguration>(
                hostContext.Configuration.GetSection(serviceBusConfigurationKeyName))
            .AddOptions()
            .AddLogging(l => l.AddConsole())
            .AddTransient<IEventHandler<NotifyEvent>, NotifyEventHandler>()
            .AddServiceBus()
            .AddHostedService<ServiceBusReceiverService>()
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
