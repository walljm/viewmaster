using ViewMaster.Core.Models.Common;
using ViewMaster.Core.Models.Writers;

namespace ViewMaster.Core.Models.Operations;

public class MoveOperation : IOperation
{
    private readonly Coordinate coordinate;

    /// <summary>
    ///  Moves the camera to a specific position
    /// </summary>
    /// <param name="coordinate">A coordinates using 1 - 65535 scale.</param>
    public MoveOperation(Coordinate coordinate)
    {
        this.coordinate = coordinate;
    }

    public async Task Execute(IWriter writer)
    {
        await writer.SendPositionAbsolute(coordinate);
    }
}
