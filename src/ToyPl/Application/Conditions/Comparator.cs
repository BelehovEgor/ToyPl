using OneOf;
using ToyPl.Application.Expressions;
using ToyPl.Application.Models;

namespace ToyPl.Application.Conditions;

public abstract class Comparator(Func<UnsignedIntModType, UnsignedIntModType, bool> predicate)
{
    public bool Invoke(State state, params OneOf<string, UnsignedIntModType, Expression>[] expressions)
    {
        if (expressions.Length != 2) throw new InvalidOperationException();

        return predicate(Calc(expressions[0], state), Calc(expressions[1], state));
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