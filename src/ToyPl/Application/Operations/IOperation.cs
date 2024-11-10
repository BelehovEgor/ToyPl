using ToyPl.Application.Models;

namespace ToyPl.Application.Operations;

public interface IOperation
{
    IReadOnlyCollection<State> Do(IReadOnlyCollection<State> states);
}