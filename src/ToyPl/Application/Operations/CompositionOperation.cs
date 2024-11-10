using ToyPl.Application.Models;

namespace ToyPl.Application.Operations;

public class CompositionOperation(IOperation first, IOperation second) : IOperation
{
    public IReadOnlyCollection<State> Do(IReadOnlyCollection<State> states)
    {
        var firstDoResult = first.Do(states);
        
        return second.Do(firstDoResult);
    }
}