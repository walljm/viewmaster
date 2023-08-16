using System.Diagnostics;
using ViewMaster.Core.Models.Common;
using ViewMaster.Core.Models.Operations;

namespace ViewMaster.Core.Models.Writers;

public class LogWriter : IWriter
{
    private string prefix => $"Writer ({Id}) Operation> ";

    public ushort Id { get; init; }

    public ushort SendDelay => 0;

    public Task SendAction(Axis action, short speed)
    {
        Debug.WriteLine($"{prefix}{nameof(SendAction)}({nameof(Action)}: {action}, Speed: {speed})");
        return Task.CompletedTask;
    }

    public Task SendPanTilt(short panSpeed, short tiltSpeed)
    {
        Debug.WriteLine($"{nameof(SendPanTilt)}({prefix}Pan: {panSpeed}, Tilt: {tiltSpeed})");
        return Task.CompletedTask;
    }

    public Task SendPanTiltZoom(short panSpeed, short tiltSpeed, short zoomSpeed)
    {
        Debug.WriteLine($"{prefix}{nameof(SendPanTiltZoom)}(Pan: {panSpeed}, Tilt: {tiltSpeed}, Zoom: {zoomSpeed})");
        return Task.CompletedTask;
    }

    public Task SendPositionAbsolute(UDegrees16 coordinate)
    {
        Debug.WriteLine($"{prefix}{nameof(SendPositionAbsolute)}(Pan: {coordinate.PanValue}, Tilt: {coordinate.TiltValue})");
        return Task.CompletedTask;
    }

    public Task SendPositionSpeedAbsolute(UDegrees16 coordinate, ushort? speed)
    {
        Debug.WriteLine($"{prefix}{nameof(SendPositionSpeedAbsolute)}(Pan: {coordinate.PanValue}, Tilt: {coordinate.TiltValue}, Speed: {speed})");
        return Task.CompletedTask;
    }

    public Task SendPositionRelative(UDegrees16 coordinate)
    {
        Debug.WriteLine($"{prefix}{nameof(SendPositionRelative)}(Pan: {coordinate.PanValue}, Tilt: {coordinate.TiltValue})");
        return Task.CompletedTask;
    }

    public Task SendZoomAbsolute(ushort speed)
    {
        Debug.WriteLine($"{prefix}{nameof(SendZoomAbsolute)}(Speed: {speed})");
        return Task.CompletedTask;
    }

    public Task SendStop()
    {
        Debug.WriteLine($"{prefix}{nameof(SendStop)}");
        return Task.CompletedTask;
    }
}
