using ViewMaster.Core.Models.Common;
using ViewMaster.Core.Models.Writers;

namespace ViewMaster.Core.Models.Operations;

public class MoveOperation : IOperation
{
    private readonly Coordinate coordinate;
    private readonly ushort speed;

    /// <summary>
    ///  Moves the camera to a specific position
    /// </summary>
    /// <param name="coordinate">A coordinates using 1 - 65535 scale.</param>
    /// <param name="speed">Value between 1 and 90</param>
    public MoveOperation(Coordinate coordinate, ushort speed)
    {
        this.coordinate = coordinate;
        this.speed = speed;
    }

    public async Task Execute(IWriter writer)
    {
        await writer.SendPositionAbsolute(coordinate, this.speed);
    }
}
