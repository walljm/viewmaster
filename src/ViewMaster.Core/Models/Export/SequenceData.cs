using ViewMaster.Core.Models.Sequences;

namespace ViewMaster.Core.Models.Export;

public record SequenceData
{
    public string Label { get; set; } = string.Empty;
    public IDictionary<ushort, WriterData>? Writers { get; set; }
    public IDictionary<ushort, CueData>? Cues { get; set; }

    public Sequence ToSequence()
    {
        if (Writers is null || Cues is null)
        {
            throw new InvalidOperationException();
        }

        return new Sequence(
            Label,
            Cues.Select(o => o.Value.ToCue(Writers, o.Key))
                .OrderBy(o => o.Ordinal)
                .ToList()
        );
    }
}
