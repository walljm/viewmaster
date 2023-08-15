using System.ComponentModel;
using System.Diagnostics;
using ViewMaster.Core.Models.Operations;
using ViewMaster.Core.Models.Writers;

namespace ViewMaster.Core.Models.Sequences;

public record Cue
{
    public Cue() { }

    public Cue(int ordinal, string label, IEnumerable<CueOperation> actions)
    {
        this.Ordinal = ordinal;
        this.Label = label;
        this.Operations = actions;
    }

    [DisplayName("Order")]
    public int Ordinal { get; init; }

    public string Label { get; init; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public IEnumerable<CueOperation> Operations { get; init; } = Array.Empty<CueOperation>();

    public async Task Execute(CancellationToken cancellationToken = default)
    {
        try
        {
            Log($"Cue> {Label} is executing");

            // run each operation sequentially. don't try and run them all at the same time.
            foreach (var operation in Operations)
            {
                this.Status = $"Executing {operation.Operation.GetType().Name}...";
                Log($"  {nameof(CueOperation)}> {operation.Operation.GetType().Name} is executing");
                await operation.Execute(cancellationToken);
            }
        }
        catch (TaskCanceledException)
        {
            this.Status = "Cancelled";
            Log($"Cue> {Label} was cancelled.");
        }
        finally
        {
            this.Status = "Complete";
        }
    }

    private void Log(string message)
    {
        Debug.WriteLine($"{DateTimeOffset.Now:HH:mm:ss}: ({Ordinal}) {message}");
    }
};

public record CueOperation(IOperation Operation, IEnumerable<IWriter> Writers)
{
    public async Task Execute(CancellationToken cancellationToken = default)
    {
        var psTasks = new List<Task>();
        foreach (var writer in Writers)
        {
            Debug.WriteLine($"    Writer ({writer.Id})> {Operation.GetType().Name} is executing");
            psTasks.Add(Operation.Execute(writer, cancellationToken));
        }
        await Task.WhenAll(psTasks);
    }
};