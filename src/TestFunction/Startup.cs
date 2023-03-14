using Amido.Stacks.Configuration.Extensions;
using Amido.Stacks.Messaging.Azure.ServiceBus.Configuration;
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
                .AddServiceBus()
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

