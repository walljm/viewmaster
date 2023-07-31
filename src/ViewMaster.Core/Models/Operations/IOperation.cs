using ViewMaster.Core.Models.Writers;

namespace ViewMaster.Core.Models.Operations;

public interface IOperation
{
    Task Execute(IWriter writer);
}