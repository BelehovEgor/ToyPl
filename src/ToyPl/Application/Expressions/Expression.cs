using OneOf;
using ToyPl.Application.Models;

namespace ToyPl.Application.Expressions;

public class Expression(OneOf<string, UnsignedIntModType, Expression> left, OneOf<string, UnsignedIntModType, Expression> right, Operation operation)
{
    public UnsignedIntModType Calc(State state)
    {
        return operation.Invoke(Calc(left, state), Calc(right, state));
    }

    private UnsignedIntModType Calc(OneOf<string, UnsignedIntModType, Expression> value, State state)
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