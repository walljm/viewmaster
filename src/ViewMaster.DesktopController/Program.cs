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
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddCommandLine(args);
                })
                .ConfigureServices(services =>
                {
                    services.TryAddSingleton<ICueDispatcher, SessionHostedService>();
                    services.AddHostedService(provider => (SessionHostedService)provider.GetRequiredService<ICueDispatcher>());
                })
                // the main application form.
                // call this after configure services.
                .ConfigureWinForms<MainWindow>()
                .UseWinFormsLifetime()
                .Build()
                .RunAsync();
        }
    }
}