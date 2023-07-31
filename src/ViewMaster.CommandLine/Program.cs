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

            var writers = new List<IWriter>(); // { new PtzWriter(IPAddress.Parse("10.101.0.174")) };
            foreach (var ip in opts.Cameras)
            {
                writers.Add(new PtzWriter(IPAddress.Parse(ip)));
            }

            var cue1 = new Cue(1, "Cue1", writers, new CircleOperation(TimeSpan.FromSeconds(30), 0.25));
            var cue2 = new Cue(2, "Cue2", writers, new PanOperation(new Coordinate(8000, 8000), 95, TimeSpan.FromSeconds(10), 0.25));
            var cue3 = new Cue(3, "Cue3", writers, new MoveOperation(new Coordinate(8000, 8000), 80));

            var sequence = new Sequence("Run in a circle", new List<Cue> { cue1, cue2, cue3 });

            var session = new Session(sequence);
            while (await session.FireNextCue())
            {
                // NOP
            }
        }
    }
}
