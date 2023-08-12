using System.ComponentModel.DataAnnotations;

namespace ViewMaster.Core.Models.Export;

public class DegreeData
{
    [Range(0.0, 360.0)]
    public double Pan { get; set; }

    [Range(0.0, 360.0)]
    public double Tilt { get; set; }
};
