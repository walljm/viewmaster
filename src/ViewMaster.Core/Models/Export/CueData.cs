﻿using ViewMaster.Core.Models.Sequences;
using ViewMaster.Core.Models.Writers;

namespace ViewMaster.Core.Models.Export;

public class CueData
{
    public string Label { get; set; } = string.Empty;
    public IEnumerable<TargetData>? Targets { get; set; }

    public Cue ToCue(IDictionary<ushort, IWriter> Writers, ushort ordinal)
    {
        if (Targets is null)
        {
            throw new InvalidOperationException();
        }

        return new Cue(ordinal, Label, Targets.Select(o => o.ToCueTarget(Writers)));
    }
}
