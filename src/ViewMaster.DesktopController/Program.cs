using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace ViewMaster.DesktopController
{
    internal static class Program
    {
        [STAThread]
        private static async Task Main()
        {
            await CreateHostBuilder().Build().RunAsync();
        }

        private static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.TryAddSingleton<ICueDispatcher, SessionHostedService>();
                    services.AddHostedService(provider => (SessionHostedService)provider.GetRequiredService<ICueDispatcher>());

                    services.AddHostedService<ApplicationLaunch>();
                });
        }
    }
}