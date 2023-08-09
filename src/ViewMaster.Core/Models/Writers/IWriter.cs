using ViewMaster.Core.Models.Common;

namespace ViewMaster.Core.Models.Writers;

public interface IWriter
{
    ushort SendDelay { get; }

    Task SendAction(Action action, short speed);

    Task SendPanTilt(short panSpeed, short tiltSpeed);

    Task SendPanTiltZoom(short panSpeed, short tiltSpeed, short zoom);

    Task SendPositionAbsolute(Coordinate coordinate);

    Task SendPositionSpeedAbsolute(Coordinate coordinate, ushort? speed);

    Task SendPositionRelative(Coordinate coordinate);

    Task SendZoomAbsolute(ushort speed);

    Task SendStop();
}
