namespace ViewMaster.Core.Models.Sequences;

public record Sequence(string Label, IList<Cue> Cues);
