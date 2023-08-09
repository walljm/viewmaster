using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ViewMaster.DesktopController
{
    internal class ApplicationLaunch : BackgroundService
    {
        private readonly IServiceProvider services;
        private readonly IHostApplicationLifetime hostApplication;

        public ApplicationLaunch(
            IServiceProvider services,
            IHostApplicationLifetime hostApplication
        )
        {
            this.services = services;
            this.hostApplication = hostApplication;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            var dispatcher = this.services.GetRequiredService<ICueDispatcher>();
            Application.Run(new MainWindow(dispatcher, this.hostApplication));

            return Task.CompletedTask;
        }
    }
}
