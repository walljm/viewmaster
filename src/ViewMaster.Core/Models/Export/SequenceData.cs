using ViewMaster.Core.Models.Sequences;
using ViewMaster.Core.Models.Writers;

namespace ViewMaster.Core.Models.Export;

public record SequenceData
{
    public string Label { get; set; } = string.Empty;
    public IDictionary<ushort, WriterData>? Writers { get; set; }
    public IDictionary<ushort, CueData>? Cues { get; set; }

    public IList<Cue> ToCueList()
    {
        if (Writers is null || Cues is null)
        {
            throw new InvalidOperationException();
        }

        var writers = this.Writers.Select(o => o.Value.Kind switch
        {
            WriterType.PtzWriter => (IWriter)new PanasonicPtzWriter(o.Key, ThrowIfNull(o.Value.Address)),
            WriterType.LogWriter => (IWriter)new LogWriter { Id = o.Key },
            _ => throw new InvalidOperationException($"Unsupported Writer Type: {o.Value.Kind}"),
        }).ToDictionary(k => k.Id, v => v);

        return Cues.Select(o => o.Value.ToCue(writers, o.Key))
                .OrderBy(o => o.Ordinal)
                .ToList()
        ;
    }

    private static T ThrowIfNull<T>(T? o) => o ?? throw new ArgumentNullException();
}
