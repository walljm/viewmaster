using ViewMaster.Core.Models.Export;
using ViewMaster.Core.Models.Writers;

namespace ViewMaster.Core.Models.Operations;

public class CircleOperation : Operation
{
    /// <summary>
    /// This is as fast as the panasonic cameras can recieve requests.
    /// </summary>

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
    public CircleOperation(string label, TimeSpan timeSpan, double scale) : base(label, OperationType.Circle)
    {
        this.timeSpan = timeSpan;

        if (scale < .1 || scale > 1)
        {
            throw new ArgumentException("Scale must be between 0.1 and 1");
        }
        this.scale = scale;
    }

    public override async Task Execute(IWriter writer, CancellationToken cancellationToken = default)
    {
        await writer.SendStop();
        await Task.Delay(writer.SendDelay, cancellationToken);

        var iterations = Math.Round(this.timeSpan.TotalMilliseconds / writer.SendDelay, 0);
        var time = iterations * writer.SendDelay; // 10 seconds

        for (var i = 0; i < time; i += writer.SendDelay)
        {
            if (cancellationToken.IsCancellationRequested) { break; }

            // step size of the angle
            var dtheta = 2 * Math.PI / iterations;

            // angle
            var theta = dtheta * (i / writer.SendDelay);

            var xspeed = Math.Sin(theta) * maxSpeed * this.scale * -1;
            var yspeed = Math.Cos(theta) * maxSpeed * this.scale;

            var panSpeed = Convert.ToInt16(Math.Round(xspeed));
            var tiltSpeed = Convert.ToInt16(Math.Round(yspeed));

            await writer.SendPanTilt(panSpeed, tiltSpeed);
            await Task.Delay(writer.SendDelay, cancellationToken);
        }

        await writer.SendStop();
        await Task.Delay(writer.SendDelay, cancellationToken);
        await writer.SendStop();
    }
}
