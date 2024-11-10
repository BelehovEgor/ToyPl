using ToyPl.Application.Models;

namespace ToyPl.Application.Operations;

public class TestOperation(Predicate<State> predicate) : IOperation
{
    public IReadOnlyCollection<State> Do(IReadOnlyCollection<State> states)
    {
        return states.Where(predicate.Invoke).ToArray();
    }
}