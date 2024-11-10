using ToyPl.Application.Models;

namespace ToyPl.Application.Operations;

public class UnionOperation(IOperation first, IOperation second) : IOperation
{
    public IReadOnlyCollection<State> Do(IReadOnlyCollection<State> states)
    {
        var firstResult = first.Do(states);
        var secondResult = second.Do(states);
        
        return firstResult
            .Union(secondResult)
            .Distinct(new StateComparer())
            .ToArray();
    }
}