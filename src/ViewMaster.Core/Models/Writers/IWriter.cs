using ViewMaster.Core.Models.Common;
using ViewMaster.Core.Models.Operations;

namespace ViewMaster.Core.Models.Writers;

public interface IWriter
{
    ushort Id { get; init; }

    ushort SendDelay { get; }

    Task SendAction(Axis action, short speed);

    Task SendPanTilt(short panSpeed, short tiltSpeed);

    Task SendPanTiltZoom(short panSpeed, short tiltSpeed, short zoom);

    Task SendPositionAbsolute(Coordinate coordinate);

    Task SendPositionSpeedAbsolute(Coordinate coordinate, ushort? speed);

    Task SendPositionRelative(Coordinate coordinate);

    Task SendZoomAbsolute(ushort speed);

    Task SendStop();
}
