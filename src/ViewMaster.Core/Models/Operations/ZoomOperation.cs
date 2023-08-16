using ViewMaster.Core.Models.Export;
using ViewMaster.Core.Models.Writers;

namespace ViewMaster.Core.Models.Operations;

public class ZoomOperation : Operation
{
    private readonly ushort speed;

    /// <summary>
    ///  Moves the camera to a specific zoom value
    /// </summary>
    /// <param name="speed">Value between between 0 and 2730</param>
    public ZoomOperation(string label, ushort speed)
        : base(label, OperationType.Zoom)
    {
        this.speed = speed;
    }

    public ZoomOperation(ZoomOperationData data)
        : base(data.Label, OperationType.Circle)
    {
        this.speed = data.Speed;
    }

    public override async Task Execute(IWriter writer, CancellationToken cancellationToken)
    {
        await writer.SendStop();
        await Task.Delay(writer.SendDelay, cancellationToken);

        if (cancellationToken.IsCancellationRequested) { return; }
        await writer.SendZoomAbsolute(this.speed);
    }
}
