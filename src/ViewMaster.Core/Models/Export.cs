using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json.Serialization;
using ViewMaster.Core.Models.Common;
using ViewMaster.Core.Models.Operations;
using ViewMaster.Core.Models.Sequences;
using ViewMaster.Core.Models.Writers;

namespace ViewMaster.Core.Models;

public enum WriterType
{
    PtzWriter,
    LogWriter,
}

public enum OperationType
{
    Move,
    Zoom,
    PanType1,
    PanType2,
}

public record WriterData(IPAddress Address, WriterType Kind);

public record DegreeData(
    [Range(0.0, 360.0)] double Pan,
    [Range(0.0, 360.0)] double Tilt
);

[JsonDerivedType(typeof(MoveOperationData), nameof(OperationType.Move))]
public abstract class OperationData
{
    public string? Label { get; set; }

    [JsonPropertyName("$type")]
    public abstract OperationType Kind { get; }
}

public class MoveOperationData : OperationData
{
    public Degrees? Location { get; set; }
    public override OperationType Kind => OperationType.Move;
}

public class ZoomOperationData : OperationData
{
    [Range(0, 2730)]
    public ushort Speed { get; set; } = 0;

    public override OperationType Kind => OperationType.Zoom;
}

public class PanOperationDataType1 : OperationData
{
    public Degrees? Start { get; set; }
    public Degrees? Stop { get; set; }
    public TimeSpan TimeSpan { get; set; }

    [Range(0, 1.0)]
    public double Scale { get; set; }

    [Range(-49, 49)]
    public short Zoom { get; set; } = 0;

    public override OperationType Kind => OperationType.PanType1;
}

public class PanOperationDataType2 : OperationData
{
    public Degrees? Start { get; set; }

    [Range(0.0, 360.0)]
    public double Angle { get; set; }

    public TimeSpan TimeSpan { get; set; }

    [Range(0, 1.0)]
    public double Scale { get; set; }

    [Range(-49, 49)]
    public short Zoom { get; set; } = 0;

    public override OperationType Kind => OperationType.PanType2;
}

public record TargetData(IEnumerable<WriterData> Writers, OperationData Operation)
{
    public CueAction ToCueTarget()
    {
        return new CueAction(
            Writers.Select(o => o.Kind switch
            {
                WriterType.PtzWriter => (IWriter)new PanasonicPtzWriter(o.Address),
                WriterType.LogWriter => (IWriter)new LogWriter(),
                _ => throw new InvalidOperationException($"Unsupported Writer Type: {o.Kind}"),
            }),
            Operation switch
            {
                MoveOperationData o => new MoveOperation(o.Location ?? throw new ArgumentNullException(nameof(o.Location))),
                ZoomOperationData o => new ZoomOperation(o.Speed),
                PanOperationDataType1 o => new PanOperation(o.Start ?? throw new ArgumentNullException(nameof(o.Start)), o.Stop ?? throw new ArgumentNullException(nameof(o.Stop)), o.TimeSpan, o.Scale, o.Zoom),
                PanOperationDataType2 o => new PanOperation(o.Start ?? throw new ArgumentNullException(nameof(o.Start)), o.Angle, o.TimeSpan, o.Scale, o.Zoom),
                _ => throw new InvalidOperationException($"Unsupported Operation Type: {Operation.GetType()}"),
            });
    }
}

public record CueData(int Ordinal, string Label, IEnumerable<TargetData> Targets)
{
    public Cue ToCue()
    {
        return new Cue(Ordinal, Label, Targets.Select(o => o.ToCueTarget()));
    }
}

public record SequenceData(string Label, IList<CueData> Cues)
{
    public Sequence ToSequence()
    {
        return new Sequence(
            Label,
            Cues.Select(o => o.ToCue())
                .OrderBy(o => o.Ordinal)
                .ToList()
        );
    }
}
