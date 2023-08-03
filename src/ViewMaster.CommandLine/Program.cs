using CommandLine;
using System.Diagnostics;
using System.Net;
using ViewMaster.Core.Models.Common;
using ViewMaster.Core.Models.Operations;
using ViewMaster.Core.Models.Sequences;
using ViewMaster.Core.Models.Writers;

namespace ViewMaster.CommandLine
{
    public class CommandLineOptions
    {
        [Option('c', "cameras")]
        public IEnumerable<string>? Cameras { get; set; }
    }

    internal class Program
    {
        public static async Task Main(string[] args)
        {
            #region CommandLine Handling

            var success = false;
            CommandLineOptions? opts = null;

            Parser.Default.ParseArguments<CommandLineOptions>(args)
               .WithParsed(options =>
               {
                   if (opts?.Cameras is null)
                   {
                       success = false;
                   }
                   opts = options;
                   success = true;
               });

            if (!success || opts is null)
            {
                return;
            }
            Debug.Assert(opts.Cameras is not null);

            #endregion CommandLine Handling

            var writers = new List<IWriter>();
            foreach (var ip in opts.Cameras)
            {
                writers.Add(new PtzWriter(IPAddress.Parse(ip)));
            }

            var sequence = new Sequence("Run in a circle", new List<Cue> {
                //new Cue(1, "Cue1", writers, new CircleOperation(TimeSpan.FromSeconds(30), 0.25)),

                // set zoom to a specific level
                new Cue(1, "Zoom", writers, new ZoomOperation(1000)),
                new Cue(2, "Pan Left", writers, new PanOperation(new Degrees(180, 90), 280, TimeSpan.FromSeconds(15), 0.20, -10)),
                new Cue(3, "Move", writers, new MoveOperation(new Degrees(180, 90))),
            });

            var session = new Session(sequence);
            while (await session.FireNextCue())
            {
                // NOP
            }
        }
    }
}
