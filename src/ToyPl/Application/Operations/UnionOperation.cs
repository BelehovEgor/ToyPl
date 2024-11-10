using ToyPl.Application.Models;

namespace ToyPl.Application.Operations;

public class UnionOperation(IOperation first, IOperation second) : IOperation
{
    public IReadOnlyCollection<State> Do(IReadOnlyCollection<State> states)
    {
        return first.Do(states)
            .Union(second.Do(states))
            .Distinct(new StateComparer())
            .ToArray();
    }
}