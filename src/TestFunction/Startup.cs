using Amido.Stacks.Configuration;
using Amido.Stacks.Configuration.Extensions;
using Amido.Stacks.Messaging.Azure.ServiceBus.Configuration;
using Amido.Stacks.Messaging.Azure.ServiceBus.Extensions;
using Amido.Stacks.Messaging.Azure.ServiceBus.Factories;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Templates;
using TestFunction;

[assembly: FunctionsStartup(typeof(Startup))]

namespace TestFunction;

public class Startup : FunctionsStartup
{
    private const string ServiceBusConfigurationKeyName = "ServiceBusConfiguration";

    public override void Configure(IFunctionsHostBuilder builder)
    {
        var configuration = LoadConfiguration(builder);

        builder.Services
            .AddSecrets()
            .AddTransient<IMessageMetadataBuilder, MessageMetadataBuilder>()
            .AddTransient(typeof(ILogger<>), typeof(LogAdapter<>))
            .Configure<ServiceBusConfiguration>(
                configuration.GetSection(ServiceBusConfigurationKeyName))
            .AddOptions()
            .AddLogging(loggingBuilder =>
                loggingBuilder.AddSerilog(
                    new LoggerConfiguration()
                        .MinimumLevel.Warning()
                        .Enrich.FromLogContext()
                        .Enrich.WithThreadId()
                        .Enrich.WithMachineName()
                        .WriteTo.Console(new ExpressionTemplate(
                            "{ {Timestamp: @t, Level: @l, MessageTemplate: @mt, Message: @m, Exception: @x, Properties: if IsDefined(@p[?]) then @p else undefined()} }\\n"))
                        .Filter.ByExcluding(
                            "@l='Error' and SourceContext='Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddleware' and @mt='An unhandled exception has occurred while executing the request.'")
                        .CreateLogger()))
            .AddServiceBus(serviceBusConfiguration =>
                serviceBusConfiguration.Sender = new ServiceBusSenderConfiguration
                {
                    Topics = new[]
                    {
                        new ServiceBusTopicConfiguration()
                        {
                            Name = "notification-event",
                            Serializer = "CloudEventMessageSerializer",
                            ConnectionStringSecret = new Secret
                            {
                                Identifier = "SERVICEBUS_CONNECTIONSTRING",
                                Source = "Environment"
                            }
                        }
                    },
                    Routing = new MessageRoutingConfiguration
                    {
                        Topics = new[]
                        {
                            new MessageRoutingTopicRouterConfiguration
                            {
                                SendTo = new []
                                {
                                    "notification-event"
                                },
                                TypeFilter = new []
                                {
                                    "TestCommon.NotifyEvent"
                                }
                            }
                        }
                    }
                }
            )
            ;
    }

    private static IConfiguration LoadConfiguration(IFunctionsHostBuilder builder)
    {
        return new ConfigurationBuilder()
            .SetBasePath(builder.GetContext().ApplicationRootPath)
            .AddJsonFile("appsettings.json", false)
            .AddJsonFile("appsettings.local.json", true)
            .AddEnvironmentVariables()
            .Build();
    }
}

