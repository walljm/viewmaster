using Dapplo.Microsoft.Extensions.Hosting.WinForms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace ViewMaster.DesktopController
{
    internal static class Program
    {
        [STAThread]
        public static async Task Main(string[] args)
        {
            // setup the configuration.
            ApplicationConfiguration.Initialize();

            await Host.CreateDefaultBuilder(args)
                .ConfigureConfiguration(args)
                .ConfigureServices(services =>
                {
                    services.TryAddSingleton<ICueDispatcher, SessionHostedService>();
                    services.AddHostedService(provider => (SessionHostedService)provider.GetRequiredService<ICueDispatcher>());
                })
                .ConfigureWinForms<MainWindow>()
                .UseWinFormsLifetime()
                .Build()
                .RunAsync();
        }

        private static IHostBuilder ConfigureConfiguration(this IHostBuilder hostBuilder, string[] args)
        {
            return hostBuilder.ConfigureHostConfiguration(configHost =>
            {
                configHost
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddCommandLine(args);
            })
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    configApp
                        .AddJsonFile("appSettings.json", optional: true)
                        .AddCommandLine(args);
                });
        }
    }
}