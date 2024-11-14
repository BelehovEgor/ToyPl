using OneOf;
using ToyPl.Application.Models;

namespace ToyPl.Application.Expressions;

public class Expression(
    PossibleValue left, 
    PossibleValue right, 
    Operation operation)
{
    public UnsignedIntModType Calc(State state)
    {
        return operation.Invoke(Calc(left, state), Calc(right, state));
    }

    private UnsignedIntModType Calc(PossibleValue value, State state)
    {
        return value.Value switch
        {
            string name => state.Variables[name].Value,
            UnsignedIntModType intModTypeValue => intModTypeValue,
            Expression expression => expression.Calc(state),
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }
}