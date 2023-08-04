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

    public Task SendPanTiltZoom(short panSpeed, short tiltSpeed, short zoomSpeed)
    {
        Debug.WriteLine($"Pan: {panSpeed}, Tilt: {tiltSpeed}, Zoom: {zoomSpeed}");
        return Task.CompletedTask;
    }

    public Task SendPositionAbsolute(Coordinate coordinate)
    {
        Debug.WriteLine($"Pan: {coordinate.PanCoordinate}, Tilt: {coordinate.TiltCoordinate}");
        return Task.CompletedTask;
    }

    public Task SendPositionSpeedAbsolute(Coordinate coordinate, ushort? speed)
    {
        Debug.WriteLine($"Pan: {coordinate.PanCoordinate}, Tilt: {coordinate.TiltCoordinate}, Speed: {speed}");
        return Task.CompletedTask;
    }

    public Task SendPositionRelative(Coordinate coordinate)
    {
        Debug.WriteLine($"Pan: {coordinate.PanCoordinate}, Tilt: {coordinate.TiltCoordinate}");
        return Task.CompletedTask;
    }

    public Task SendZoomAbsolute(ushort speed)
    {
        Debug.WriteLine($"Speed: {speed}");
        return Task.CompletedTask;
    }

    public Task SendStop()
    {
        Debug.WriteLine($"Stop");
        return Task.CompletedTask;
    }
}
