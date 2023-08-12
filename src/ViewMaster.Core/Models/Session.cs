using System.ComponentModel;
using System.Diagnostics;
using ViewMaster.Core.Models.Export;
using ViewMaster.Core.Models.Operations;
using ViewMaster.Core.Models.Writers;

namespace ViewMaster.Core.Models.Sequences;

public record Cue
{
    public Cue() { }
    public Cue(int ordinal, string label, IEnumerable<CueAction> actions)
    {
        this.Ordinal = ordinal;
        this.Label = label;
        this.Actions = actions;
    }

    [DisplayName("Order")]
    public int Ordinal { get; init; }

    public string Label { get; init; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public IEnumerable<CueAction> Actions { get; init; } = Array.Empty<CueAction>();

    public async Task Execute(CancellationToken cancellationToken = default)
    {
        try
        {
            Debug.WriteLine($"Cue ({Ordinal})> {Label} is executing");
            this.Status = "Executing";
            var psTasks = new List<Task>();
            foreach (var target in Actions)
            {
                psTasks.Add(target.Execute(cancellationToken));
            }
            await Task.WhenAll(psTasks);
            this.Status = "Complete";
        }
        catch (TaskCanceledException)
        {
            this.Status = "Cancelled";
            // NOP: we don't want these to stop program execution.
            Debug.WriteLine($"Cue ({Ordinal})> {Label} was cancelled.");
        }
    }
};

public record CueAction(IEnumerable<IWriter> Writers, IOperation Operation)
{
    public async Task Execute(CancellationToken cancellationToken = default)
    {
        var psTasks = new List<Task>();
        foreach (var writer in Writers)
        {
            psTasks.Add(Operation.Execute(writer, cancellationToken));
        }
        await Task.WhenAll(psTasks);
    }
};

public record Sequence(string Label, IList<Cue> Cues);

public class Session
{
    private int currentCue = 0;

    public Session(Sequence sequence)
    {
        ArgumentNullException.ThrowIfNull(sequence.Cues, nameof(sequence));
        this.Sequence = new Sequence(sequence.Label, sequence.Cues.OrderBy(o => o.Ordinal).ToList());
    }

    public Session(SequenceData sequence)
    {
        ArgumentNullException.ThrowIfNull(sequence.Cues, nameof(sequence));

        this.Sequence = sequence.ToSequence();
    }

    public Sequence Sequence { get; init; }

    public async Task<bool> FireNextCue(CancellationToken cancellationToken = default)
    {
        if (this.Sequence.Cues.Count == this.currentCue)
        {
            // loop back around to the beginning.
            return false;
        }

        await this.Sequence.Cues[this.currentCue].Execute(cancellationToken);
        this.currentCue++;
        return true;
    }

    public async Task FireCue(int ordinal, CancellationToken cancellationToken = default)
    {
        this.currentCue = ordinal;
        await FireNextCue(cancellationToken);
    }
}
