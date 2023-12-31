﻿using ViewMaster.Core.Models.Common;
using ViewMaster.Core.Models.Export;
using ViewMaster.Core.Models.Writers;

namespace ViewMaster.Core.Models.Operations;

public class MoveOperation : Operation
{
    private readonly UDegrees16 coordinate;

    /// <summary>
    ///  Moves the camera to a specific position
    /// </summary>
    /// <param name="location">A set of coordinates using degrees, where 0 for pan and tilt points behind.</param>
    public MoveOperation(string label, Degrees360 location) : base(label, OperationType.Circle)
    {
        this.coordinate = location; // implicit conversion to 16 bit int coordinates
    }

    public MoveOperation(MoveOperationData data) : base(data.Label, OperationType.Move)
    {
        this.coordinate = data.Location ?? throw new ArgumentNullException(nameof(data.Location));
    }

    public override async Task Execute(IWriter writer, CancellationToken cancellationToken = default)
    {
        await writer.SendStop();
        await Task.Delay(writer.SendDelay, cancellationToken);

        if (cancellationToken.IsCancellationRequested) { return; }
        await writer.SendPositionAbsolute(coordinate);
    }
}
