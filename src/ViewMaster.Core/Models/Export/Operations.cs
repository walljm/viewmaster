using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ViewMaster.Core.Models.Export;

public enum OperationType
{
    Move,
    Zoom,
    PanType1,
    PanType2,
}

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
    public override OperationType Kind => OperationType.Move;
    public DegreeData? Location { get; set; }
}

public class ZoomOperationData : OperationData
{
    public override OperationType Kind => OperationType.Zoom;

    [Range(0, 2730)]
    public ushort Speed { get; set; } = 0;
}

public class PanOperationDataType1 : OperationData
{
    public override OperationType Kind => OperationType.PanType1;

    public DegreeData? Start { get; set; }
    public DegreeData? Stop { get; set; }
    public TimeSpan TimeSpan { get; set; }

    [Range(0, 1.0)]
    public double Scale { get; set; }

    [Range(-49, 49)]
    public short Zoom { get; set; } = 0;
}

public class PanOperationDataType2 : OperationData
{
    public override OperationType Kind => OperationType.PanType2;

    public DegreeData? Start { get; set; }

    [Range(0.0, 360.0)]
    public double Angle { get; set; }

    public TimeSpan TimeSpan { get; set; }

    [Range(0, 1.0)]
    public double Scale { get; set; }

    [Range(-49, 49)]
    public short Zoom { get; set; } = 0;
}
