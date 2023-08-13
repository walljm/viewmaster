using ViewMaster.Core.Models.Common;
using ViewMaster.Core.Models.Export;
using ViewMaster.Core.Models.Writers;

namespace ViewMaster.Core.Models.Operations;

public class MoveOperation : IOperation
{
    private readonly Coordinate coordinate;

    /// <summary>
    ///  Moves the camera to a specific position
    /// </summary>
    /// <param name="location">A set of coordinates using degrees, where 0 for pan and tilt points behind.</param>
    public MoveOperation(Degrees location)
    {
        this.coordinate = location;
    }

    public MoveOperation(MoveOperationData data)
    {
        this.coordinate = data.Location ?? throw new ArgumentNullException(nameof(data.Location));
    }

    public async Task Execute(IWriter writer, CancellationToken cancellationToken = default)
    {
        await writer.SendStop();
        await Task.Delay(writer.SendDelay, cancellationToken);

        if (cancellationToken.IsCancellationRequested) { return; }
        await writer.SendPositionAbsolute(coordinate);
    }
}
