using System;
using System.Threading.Tasks;
using DateTimeProviderService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace DateTimeProviderServiceHost
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.Trace()
                .Enrich.FromLogContext()
                .CreateLogger();
            
            IHostBuilder hostBuilder = CreateHostBuilder(args);
            await hostBuilder.RunConsoleAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return new HostBuilder().ConfigureAppConfiguration((context, configBuilder) =>
                {
                    configBuilder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                    configBuilder.AddEnvironmentVariables();

                    if (args != null)
                    {
                        configBuilder.AddCommandLine(args);
                    }
                })
                .ConfigureServices((hostBuilderContext, services) =>
                {
                    services.AddOptions();
                    services.Configure<DateTimeProviderConfig>(hostBuilderContext.Configuration.GetSection("System DateTime"));
                    
                    services.AddLogging(loggingBuilder => { loggingBuilder.AddSerilog(dispose: true); });

                    services.AddTransient<IDateTimeProvider, DateTimeProvider>();
                    
                    
                    services.AddHostedService<DateTimeProviderService.DateTimeProviderService>();
                });
        }
    }
}