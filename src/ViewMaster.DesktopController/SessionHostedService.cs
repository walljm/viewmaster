using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Channels;
using ViewMaster.Core.Models.Sequences;

namespace ViewMaster.DesktopController
{
    public sealed record CueArguments(
        Cue Cue,
        CancellationToken CancellationToken
    );

    public interface ICueDispatcher
    {
        [return: NotNull]
        Task DispatchAsync(CueArguments arguments);
    }

    public class SessionHostedService : BackgroundService, ICueDispatcher
    {
        private readonly Channel<CueArguments> channel = Channel.CreateUnbounded<CueArguments>();

        public async Task DispatchAsync(CueArguments arguments)
        {
            await this.channel.Writer.WriteAsync(arguments);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                try
                {
                    await this.channel.Reader.WaitToReadAsync(stoppingToken);

                    if (this.channel.Reader.TryRead(out var arguments))
                    {
                        if (stoppingToken.IsCancellationRequested)
                        {
                            return;
                        }

                        // we use a different stopping token here so we can cancel
                        await arguments.Cue.Execute(arguments.CancellationToken).ConfigureAwait(false);
                    }
                }
                catch
                {
                    // this shouldn't happen.  but if it does, it will happen very loudly, forcing it to be addresses quickly. - walljm
                    Environment.ExitCode = -1;
                    return;
                }
            }
        }
    }
}
