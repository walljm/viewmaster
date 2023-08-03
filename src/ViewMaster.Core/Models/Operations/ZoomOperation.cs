using ViewMaster.Core.Models.Writers;

namespace ViewMaster.Core.Models.Operations;

public class ZoomOperation : IOperation
{
    private readonly ushort speed;

    /// <summary>
    ///  Moves the camera to a specific zoom value
    /// </summary>
    /// <param name="speed">Value between between 0 and 2730</param>
    public ZoomOperation(ushort speed)
    {
        this.speed = speed;
    }

    public async Task Execute(IWriter writer)
    {
        await writer.SendZoomAbsolute(this.speed);
    }
}
