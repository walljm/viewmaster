using ViewMaster.Core.Models.Writers;

namespace ViewMaster.Core.Models.Operations;

public class CircleOperation : IOperation
{
    /// <summary>
    /// This is as fast as the panasonic cameras can recieve requests.
    /// </summary>
    private const ushort increment = 130; // in milliseconds.

    private const ushort maxSpeed = 49;

    private readonly TimeSpan timeSpan;
    private readonly double scale;

    /// <summary>
    ///  Moves the camera in a circle.
    /// </summary>
    /// <param name="timeSpan">Determines how long it takes to draw the circle</param>
    /// <param name="scale">Determines the size of the circle by changing the max speed
    /// of the motion of the camera. MIN is 0.1, MAX is 1.
    /// </param>
    public CircleOperation(TimeSpan timeSpan, double scale)
    {
        this.timeSpan = timeSpan;

        if (scale < .1 || scale > 1)
        {
            throw new ArgumentException("Scale must be between 0.1 and 1");
        }
        this.scale = scale;
    }

    public async Task Execute(IWriter writer)
    {
        var iterations = Math.Round(this.timeSpan.TotalMilliseconds / increment, 0);
        var time = iterations * increment; // 10 seconds

        for (var i = 0; i < time; i += increment)
        {
            // step size of the angle
            var dtheta = 2 * Math.PI / iterations;

            // angle
            var theta = dtheta * (i / increment);

            var xspeed = Math.Sin(theta) * maxSpeed * this.scale * -1;
            var yspeed = Math.Cos(theta) * maxSpeed * this.scale;

            var panSpeed = Convert.ToInt16(Math.Round(xspeed));
            var tiltSpeed = Convert.ToInt16(Math.Round(yspeed));

            await writer.SendPanTilt(panSpeed, tiltSpeed);

            Thread.Sleep(increment);
        }

        await writer.SendPanTilt(0, 0);
    }
}
