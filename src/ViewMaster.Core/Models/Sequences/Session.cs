namespace ViewMaster.Core.Models.Sequences;

public class Session
{
    private int currentCue = 0;
    private readonly List<Cue> Cues;

    public Session(Sequence sequence)
    {
        ArgumentNullException.ThrowIfNull(sequence.Cues, nameof(sequence));
        this.Cues = sequence.Cues.OrderBy(o => o.Ordinal).ToList();
    }

    public async Task<bool> FireNextCue()
    {
        if (this.Cues.Count == this.currentCue)
        {
            // loop back around to the beginning.
            return false;
        }

        await this.Cues[this.currentCue].Execute();
        this.currentCue++;
        return true;
    }

    public async Task FireCue(int ordinal)
    {
        this.currentCue = this.Cues.FindIndex(o => o.Ordinal == ordinal);
        await FireNextCue();
    }
}
