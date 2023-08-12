using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json.Serialization;
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

public class WriterData
{
    public WriterType Kind { get; set; }
    public string Label { get; set; } = string.Empty;
    public IPAddress? Address { get; set; }
}

public class DegreeData
{
    [Range(0.0, 360.0)]
    public double Pan { get; set; }

    [Range(0.0, 360.0)]
    public double Tilt { get; set; }
};

#region Operations

[JsonDerivedType(typeof(MoveOperationData), nameof(OperationType.Move))]
[JsonDerivedType(typeof(ZoomOperationData), nameof(OperationType.Zoom))]
[JsonDerivedType(typeof(PanOperationDataType1), nameof(OperationType.PanType1))]
[JsonDerivedType(typeof(PanOperationDataType2), nameof(OperationType.PanType2))]
public abstract class OperationData
{
    public string? Label { get; set; }

    [JsonPropertyName("$type")]
    public abstract OperationType Kind { get; }
}

public class MoveOperationData : OperationData
{
    public DegreeData? Location { get; set; }
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
    public DegreeData? Start { get; set; }
    public DegreeData? Stop { get; set; }
    public TimeSpan TimeSpan { get; set; }

    [Range(0, 1.0)]
    public double Scale { get; set; }

    [Range(-49, 49)]
    public short Zoom { get; set; } = 0;

    public override OperationType Kind => OperationType.PanType1;
}

public class PanOperationDataType2 : OperationData
{
    public DegreeData? Start { get; set; }

    [Range(0.0, 360.0)]
    public double Angle { get; set; }

    public TimeSpan TimeSpan { get; set; }

    [Range(0, 1.0)]
    public double Scale { get; set; }

    [Range(-49, 49)]
    public short Zoom { get; set; } = 0;

    public override OperationType Kind => OperationType.PanType2;
}

#endregion Operations

public class TargetData
{
    public IEnumerable<int>? Writers { get; set; }
    public OperationData? Operation { get; set; }

    public CueAction ToCueTarget(IDictionary<int, WriterData> WriterLookup)
    {
        if (this.Writers is null || this.Operation is null)
        {
            throw new InvalidOperationException();
        }

        return new CueAction(
            Writers.Select(o => WriterLookup[o]).Select(o => o.Kind switch
            {
                WriterType.PtzWriter => (IWriter)new PanasonicPtzWriter(o.Address ?? throw new InvalidOperationException(nameof(o.Address))),
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

public class CueData
{
    public string Label { get; set; } = string.Empty;
    public IEnumerable<TargetData>? Targets { get; set; }

    public Cue ToCue(IDictionary<int, WriterData> Writers, int ordinal)
    {
        if (Targets is null)
        {
            throw new InvalidOperationException();
        }

        return new Cue(ordinal, Label, Targets.Select(o => o.ToCueTarget(Writers)));
    }
}

public record SequenceData
{
    public string Label { get; set; } = string.Empty;
    public IDictionary<int, WriterData>? Writers { get; set; }
    public IDictionary<int, CueData>? Cues { get; set; }

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
