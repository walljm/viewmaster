using ViewMaster.Core.Models.Operations;
using ViewMaster.Core.Models.Sequences;
using ViewMaster.Core.Models.Writers;

namespace ViewMaster.Core.Models.Export;

public class TargetData
{
    public IEnumerable<ushort>? Writers { get; set; }
    public OperationData? Operation { get; set; }

    public CueAction ToCueTarget(IDictionary<ushort, WriterData> WriterLookup)
    {
        if (this.Writers is null || this.Operation is null)
        {
            throw new InvalidOperationException();
        }

        return new CueAction(
            Writers.Select(o => (id: o, writer: WriterLookup[o]))
                   .Select(o => o.writer.Kind switch
                    {
                        WriterType.PtzWriter => (IWriter)new PanasonicPtzWriter(o.id, ThrowIfNull(o.writer.Address)),
                        WriterType.LogWriter => (IWriter)new LogWriter { Id = o.id },
                        _ => throw new InvalidOperationException($"Unsupported Writer Type: {o.writer.Kind}"),
                    }),
            Operation switch
            {
                MoveOperationData o => new MoveOperation(o),
                ZoomOperationData o => new ZoomOperation(o),
                PanOperationDataType1 o => new PanOperation(o),
                PanOperationDataType2 o => new PanOperation(o),
                _ => throw new InvalidOperationException($"Unsupported Operation Type: {Operation.GetType()}"),
            });
    }

    private static T ThrowIfNull<T>(T? o) => o ?? throw new ArgumentNullException();
}
