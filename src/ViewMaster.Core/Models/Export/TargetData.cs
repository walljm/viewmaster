using ViewMaster.Core.Models.Operations;
using ViewMaster.Core.Models.Sequences;
using ViewMaster.Core.Models.Writers;

namespace ViewMaster.Core.Models.Export;

public class TargetData
{
    public IEnumerable<ushort>? Writers { get; set; }
    public OperationData? Operation { get; set; }

    public CueOperation ToCueTarget(IDictionary<ushort, IWriter> WriterLookup)
    {
        if (this.Writers is null || this.Operation is null)
        {
            throw new InvalidOperationException();
        }

        return new CueOperation(
            Operation switch
            {
                MoveOperationData o => new MoveOperation(o),
                ZoomOperationData o => new ZoomOperation(o),
                PanOperationDataType1 o => new PanOperation(o),
                PanOperationDataType2 o => new PanOperation(o),
                _ => throw new InvalidOperationException($"Unsupported Operation Type: {Operation.GetType()}"),
            },
            Writers.Select(o => WriterLookup[o]));
    }
}
