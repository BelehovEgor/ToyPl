using OneOf;
using ToyPl.Application.Expressions;
using ToyPl.Application.Models;
using ToyPl.Extensions;

namespace ToyPl.Application.Operations;

public class AssignOperation(string leftValue, OneOf<string, UnsignedIntModType, Expression> rightValue) : IOperation
{
    public AssignOperation(Variable variable) : this(variable.Name, variable.Value)
    {
        
    }
    
    public IReadOnlyCollection<State> Do(IReadOnlyCollection<State> states)
    {
        return states.Select(Do).ToArray();
    }

    private State Do(State state)
    {
        var calcRightValue = rightValue.Value switch
        {
            string name => state.Variables[name].Value,
            UnsignedIntModType value => value,
            Expression expression => expression.Calc(state),
            _ => throw new ArgumentOutOfRangeException()
        };
        
        if (!state.Variables.ContainsKey(leftValue))
        {
            throw new InvalidOperationException("Trying assign not exists variable");
        }

        var newVariables = state.Variables.DeepClone();
        newVariables[leftValue] = new Variable(leftValue, calcRightValue);

        return new State(newVariables);
    }
}