using ViewMaster.Core.Models.Export;
using ViewMaster.Core.Models.Writers;

namespace ViewMaster.Core.Models.Operations;

public interface IOperation
{
    string? Label { get; }

    OperationType Kind { get; }

    Task Execute(IWriter writer, CancellationToken cancellationToken);
}

public abstract class Operation : IOperation
{
    public Operation(string? label, OperationType kind)
    {
        this.Label = label;
        this.Kind = kind;
    }

    public string? Label { get; set; }

    public OperationType Kind { get; }

    public abstract Task Execute(IWriter writer, CancellationToken cancellationToken);

    public override string ToString()
    {
        return $"{this.GetType().Name}: {Label}";
    }
}
