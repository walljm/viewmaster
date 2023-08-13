using ViewMaster.Core.Models.Export;

namespace ViewMaster.Core.Models.Sequences;

public class Sequence
{
    private int currentCue = 0;

    public Sequence(Sequence sequence)
    {
        ArgumentNullException.ThrowIfNull(sequence.Cues, nameof(sequence));
        this.Label = sequence.Label;
        this.Cues = sequence.Cues.OrderBy(o => o.Ordinal).ToList();
    }

    public Sequence(SequenceData sequence)
    {
        ArgumentNullException.ThrowIfNull(sequence.Cues, nameof(sequence));

        this.Label = sequence.Label;
        this.Cues = sequence.ToCueList();
    }

    public Sequence(string label, IList<Cue> cues)
    {
        ArgumentNullException.ThrowIfNull(cues, nameof(cues));

        this.Label = label ?? string.Empty;
        this.Cues = cues.OrderBy(o => o.Ordinal).ToList();
    }

    public string Label { get; init; }
    public IList<Cue> Cues { get; init; }

    public async Task<bool> FireNextCue(CancellationToken cancellationToken = default)
    {
        if (this.Cues.Count == this.currentCue)
        {
            // loop back around to the beginning.
            return false;
        }

        await this.Cues[this.currentCue].Execute(cancellationToken);
        this.currentCue++;
        return true;
    }

    public async Task FireCue(int ordinal, CancellationToken cancellationToken = default)
    {
        this.currentCue = ordinal;
        await FireNextCue(cancellationToken);
    }
}
