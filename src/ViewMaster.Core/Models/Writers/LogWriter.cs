using System.Diagnostics;
using ViewMaster.Core.Models.Common;

namespace ViewMaster.Core.Models.Writers;

public class LogWriter : IWriter
{
    public Task SendAction(Action action, short speed)
    {
        Debug.WriteLine($"{nameof(Action)}: {action}, Speed: {speed}");
        return Task.CompletedTask;
    }

    public Task SendPanTilt(short panSpeed, short tiltSpeed)
    {
        Debug.WriteLine($"Pan: {panSpeed}, Tilt: {tiltSpeed}");
        return Task.CompletedTask;
    }

    public Task SendPositionAbsolute(Coordinate coordinate, ushort? speed)
    {
        Debug.WriteLine($"Pan: {coordinate.PanCoordinate}, Tilt: {coordinate.TiltCoordinate}, Speed: {speed}");
        return Task.CompletedTask;
    }

    public Task SendPositionRelative(Coordinate coordinate, ushort? speed)
    {
        Debug.WriteLine($"Pan: {coordinate.PanCoordinate}, Tilt: {coordinate.TiltCoordinate}, Speed: {speed}");
        return Task.CompletedTask;
    }
}
