using ToyPl.Application.Conditions;
using ToyPl.Application.Models;

namespace ToyPl.Application.Operations;

public class TestOperation(ICondition condition) : IOperation
{
    public IReadOnlyCollection<State> Do(IReadOnlyCollection<State> states)
    {
        return states.Where(condition.Check).ToArray();
    }
}