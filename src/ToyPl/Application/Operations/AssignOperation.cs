using ToyPl.Application.Models;
using ToyPl.Extensions;

namespace ToyPl.Application.Operations;

public class AssignOperation(Variable newVariable) : IOperation
{
    public IReadOnlyCollection<State> Do(IReadOnlyCollection<State> states)
    {
        return states.Select(Do).ToArray();
    }

    private State Do(State state)
    {
        if (!state.Variables.ContainsKey(newVariable.Name))
        {
            throw new InvalidOperationException("Trying assign not exists variable");
        }

        var newVariables = state.Variables.DeepClone();
        newVariables[newVariable.Name] = newVariable;

        return new State(newVariables);
    }
}