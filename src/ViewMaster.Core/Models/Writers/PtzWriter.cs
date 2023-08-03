﻿using System.Net;
using ViewMaster.Core.Models.Common;

namespace ViewMaster.Core.Models.Writers;

/// <summary>
/// Writer for the Panasonic:
/// AW-HE120
/// AW-HE40
/// AW-HE42
/// AW-HE50
/// AW-HE60
/// AW-HE130
/// AW-UE70
/// AW-UE150
/// AW-UN145
/// AW-UB300
/// AW-SFU01
/// </summary>
public class PtzWriter : IDisposable, IWriter
{
    private readonly IPAddress DestinationIp;
    private readonly HttpClient Client = new();
    private bool disposedValue;

    public PtzWriter(IPAddress ip)
    {
        this.DestinationIp = ip;
    }

    public async Task SendPanTilt(short panSpeed, short tiltSpeed)
    {
        // we expect a number 0 - 49. 0 being stop, 1 being slow progressing faster to up to the max sdpeed of 49.
        if (panSpeed < -49 || panSpeed > 49 || tiltSpeed < -49 || tiltSpeed > 49)
        {
            throw new ArgumentOutOfRangeException("Speed must be between -49 and 49");
        }

        // Panasonic uses 50 as 0.  Why?  who knows...
        var psp = (panSpeed + 50).ToString().PadLeft(2, '0'); // shift decimal to the right by 50
        var tsp = (tiltSpeed + 50).ToString().PadLeft(2, '0'); // shift decimal to the right by 50

        // send the api call
        _ = await Client.GetAsync($"http://{this.DestinationIp}/cgi-bin/aw_ptz?cmd=%23PTS{psp}{tsp}&res=1");
    }

    public async Task SendPanTiltZoom(short panSpeed, short tiltSpeed, short zoomSpeed)
    {
        // we expect a number 0 - 49. 0 being stop, 1 being slow progressing faster to up to the max sdpeed of 49.
        if (panSpeed < -49 || panSpeed > 49 || tiltSpeed < -49 || tiltSpeed > 49 || zoomSpeed < -49 || zoomSpeed > 49)
        {
            throw new ArgumentOutOfRangeException("Speed must be between -49 and 49");
        }

        // Panasonic uses 50 as 0.  Why?  who knows...
        var psp = (panSpeed + 50).ToString().PadLeft(2, '0'); // shift decimal to the right by 50
        var tsp = (tiltSpeed + 50).ToString().PadLeft(2, '0'); // shift decimal to the right by 50
        var zsp = (zoomSpeed + 50).ToString().PadLeft(2, '0'); // shift decimal to the right by 50

        // send the api call
        _ = await Client.GetAsync($"http://{this.DestinationIp}/cgi-bin/aw_ptz?cmd=%23PTS{psp}{tsp}&res=1");
        Thread.Sleep(135); // don't send operations too fast.
        _ = await Client.GetAsync($"http://{this.DestinationIp}/cgi-bin/aw_ptz?cmd=%23Z{zsp}&res=1");
    }

    public async Task SendAction(Action action, short speed)
    {
        var op = action switch
        {
            Action.Tilt => 'T', // Tilt i.e the Y axis (vertical)
            Action.Pan => 'P', // Pan i.e. the X axis (horizontal)
            Action.Zoom => 'Z', // Zoom
            _ => throw new NotImplementedException(),
        };

        // we expect a number 0 - 49. 0 being stop, 1 being slow progressing faster to up to the max sdpeed of 49.
        if (speed < -49 || speed > 49)
        {
            throw new ArgumentOutOfRangeException("Speed must be between -49 and 49");
        }

        // Panasonic uses 50 as 0.  Why?  who knows...
        var sp = (speed + 50).ToString().PadLeft(2, '0'); // shift decimal to the right by 50

        // send the api call
        _ = await Client.GetAsync($"http://{this.DestinationIp}/cgi-bin/aw_ptz?cmd=%23{op}{sp}&res=1");
    }

    public async Task SendPositionAbsolute(Coordinate coordinate)
    {
        // send the api call
        var pan = coordinate.PanCoordinate.ToString("X").PadLeft(4, '0');
        var tilt = coordinate.TiltCoordinate.ToString("X").PadLeft(4, '0');
        _ = await this.Client.GetAsync($"http://{this.DestinationIp}/cgi-bin/aw_ptz?cmd=%23APC{pan}{tilt}&res=1");
    }

    public async Task SendPositionRelative(Coordinate coordinate)
    {
        var pan = coordinate.PanCoordinate.ToString("X").PadLeft(4, '0');
        var tilt = coordinate.TiltCoordinate.ToString("X").PadLeft(4, '0');
        _ = await this.Client.GetAsync($"http://{this.DestinationIp}/cgi-bin/aw_ptz?cmd=%23RPC{pan}{tilt}&res=1");
    }

    public async Task SendPositionSpeedAbsolute(Coordinate coordinate, ushort? speed)
    {
        if (speed > 90)
        {
            throw new ArgumentException(nameof(speed), "Speed must be a value between 1 and 90");
        }

        // send the api call
        var pan = coordinate.PanCoordinate.ToString("X").PadLeft(4, '0');
        var tilt = coordinate.TiltCoordinate.ToString("X").PadLeft(4, '0');
        var tbl = speed switch
        {
            _ when speed <= 30 => "0",
            _ when speed > 30 && speed <= 60 => "1",
            _ when speed > 60 && speed <= 90 => "2",
            _ => string.Empty,
        };
        var spd = speed switch
        {
            _ when speed <= 30 => speed?.ToString("X").PadLeft(2, '0'),
            _ when speed > 30 && speed <= 60 => (speed - 30)?.ToString("X").PadLeft(2, '0'),
            _ when speed > 60 && speed <= 90 => (speed - 60)?.ToString("X").PadLeft(2, '0'),
            _ => string.Empty,
        };
        _ = await this.Client.GetAsync($"http://{this.DestinationIp}/cgi-bin/aw_ptz?cmd=%23APS{pan}{tilt}{spd}{tbl}&res=1");
    }

    public async Task SendZoomAbsolute(ushort zoom)
    {
        if (zoom < 0 || zoom > 2730)
        {
            throw new ArgumentException(nameof(zoom), "Zoom value must be between 0 and 2730.");
        }
        // send the api call
        var zm = (zoom + 1365).ToString("X").PadLeft(3, '0');
        _ = await this.Client.GetAsync($"http://{this.DestinationIp}/cgi-bin/aw_ptz?cmd=%23AXZ{zm}&res=1");
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                this.Client.Dispose();
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
