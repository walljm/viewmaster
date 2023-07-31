using ViewMaster.Core.Models.Common;

namespace ViewMaster.Core.Models.Writers;

public interface IWriter
{
    Task SendAction(Action action, short speed);

    Task SendPanTilt(short panSpeed, short tiltSpeed);

    Task SendPositionAbsolute(Coordinate coordinate, ushort? speed);

    Task SendPositionRelative(Coordinate coordinate, ushort? speed);
}
