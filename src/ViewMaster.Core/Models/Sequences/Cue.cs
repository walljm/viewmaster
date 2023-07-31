using ViewMaster.Core.Models.Operations;
using ViewMaster.Core.Models.Writers;

namespace ViewMaster.Core.Models.Sequences;

public record Cue(int Ordinal, string Label, ICollection<IWriter> Targets, IOperation Operation)
{
    public async Task Execute()
    {
        var psTasks = new List<Task>();
        foreach (var target in Targets)
        {
            psTasks.Add(Operation.Execute(target));
        }
        await Task.WhenAll(psTasks);
    }
};
