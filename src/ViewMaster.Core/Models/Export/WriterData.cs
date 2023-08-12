using System.Net;

namespace ViewMaster.Core.Models.Export;

public class WriterData
{
    public WriterType Kind { get; set; }
    public string Label { get; set; } = string.Empty;
    public IPAddress? Address { get; set; }
}
