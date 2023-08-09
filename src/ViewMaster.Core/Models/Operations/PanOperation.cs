using ViewMaster.Core.Models.Common;
using ViewMaster.Core.Models.Writers;

namespace ViewMaster.Core.Models.Operations;

public class PanOperation : IOperation
{
    private const ushort maxSpeed = 49;

    private readonly Coordinate start;
    private readonly TimeSpan timeSpan;
    private readonly double scale;
    private readonly short zoom;

    private readonly double absSlope;
    private readonly int panDir;
    private readonly int tiltDir;

    /// <summary>
    ///  Define a panning motion using a starting point and an ending point.
    /// </summary>
    /// <param name="start">Absolute position of camera</param>
    /// <param name="stop">Absolute position that is used to define the slope (direction) to move the camera.</param>
    /// <param name="timeSpan">How long to move the camera</param>
    /// <param name="scale">How fast to move, 1 being MAX, 0.1 being MIN</param>
    /// <exception cref="ArgumentException"></exception>
    public PanOperation(Degrees start, Degrees stop, TimeSpan timeSpan, double scale, short zoom = 0)
    {
        if (scale < 0 || scale > 1)
        {
            throw new ArgumentException(nameof(scale), "Speed must be a value between 0.1 and 1");
        }

        this.start = start;
        this.timeSpan = timeSpan;
        this.zoom = zoom;
        var stopCoord = (Coordinate)stop;
        var panOffset = stopCoord.PanCoordinate - this.start.PanCoordinate;
        var tiltOffset = stopCoord.TiltCoordinate - this.start.TiltCoordinate
                         + 0.00001; // hack to prevent divide by 0

        this.absSlope = Math.Abs(tiltOffset / panOffset); // y/x
        this.panDir = panOffset < 0 ? -1 : 1;
        this.tiltDir = tiltOffset < 0 ? -1 : 1;
    }

    /// <summary>
    ///  Define a panning motion using a starting point and a direction.
    /// </summary>
    /// <param name="start">Absolute position of camera</param>
    /// <param name="angle">Direction, in degrees, with 0 being up, for the camera to move.</param>
    /// <param name="timeSpan">How long to move the camera</param>
    /// <param name="scale">How fast to move, 1 being MAX, 0.1 being MIN</param>
    /// <exception cref="ArgumentException"></exception>
    public PanOperation(Degrees start, double angle, TimeSpan timeSpan, double scale, short zoom = 0)
    {
        if (scale < 0 || scale > 1)
        {
            throw new ArgumentException(nameof(scale), "Speed must be a value between 0.1 and 1");
        }

        this.start = start;
        this.timeSpan = timeSpan;
        this.scale = scale;
        this.zoom = zoom;

        this.absSlope = Math.Abs(Math.Tan((90 - angle) / 180 * Math.PI));
        this.panDir = angle > 180 && angle < 360 ? -1 : 1;
        this.tiltDir = angle < 90 || angle > 270 ? 1 : -1;
    }

    public async Task Execute(IWriter writer, CancellationToken cancellationToken = default)
    {
        await writer.SendStop();
        await Task.Delay(writer.SendDelay, cancellationToken);

        var max = maxSpeed * scale; // change how fast the camera can move.
        var panSpd = Convert.ToInt16((absSlope > 1 ? max / absSlope : max) * panDir);
        var tiltSpd = Convert.ToInt16((absSlope > 1 ? max : max * absSlope) * tiltDir);

        if (!cancellationToken.IsCancellationRequested)
        {
            // move camera to first position
            await writer.SendPositionAbsolute(this.start);
            // TODO you should query for position from camera until its there
            //  instead of waiting for an arbitray period of time.
            await Task.Delay(1000, cancellationToken);
        }

        if (!cancellationToken.IsCancellationRequested)
        {
            await writer.SendPanTiltZoom(panSpd, tiltSpd, this.zoom);
        }

        // wait for the allotted time.
        if (!cancellationToken.IsCancellationRequested)
        {
            await Task.Delay(Convert.ToInt32(this.timeSpan.TotalMilliseconds), cancellationToken);
        }

        // always send the stop moving. send twice just to make sure you get it
        await writer.SendStop();
        await Task.Delay(writer.SendDelay, cancellationToken);
        await writer.SendStop();
    }
}
