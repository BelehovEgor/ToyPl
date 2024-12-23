using ToyPl.Application.Expressions;
using ToyPl.Application.Models;

namespace ToyPl;

using Value = OneOf.OneOf<string, UnsignedIntModType, Expression>;

public record PossibleValue(Value Value)
{
    public override string ToString()
    {
        return Value.Value switch
        {
            string name => name,
            UnsignedIntModType intModTypeValue => intModTypeValue.ToString(),
            Expression expression => expression.ToString(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public static PossibleValue FromString(string value)
    {
        if (value.All(char.IsLetter)) return new PossibleValue(value);
        
        if (uint.TryParse(value, out var intValue)) return new PossibleValue(new UnsignedIntModType(intValue));

        return new PossibleValue(Expression.FromString(value));
    }
    
    public UnsignedIntModType Calc(State state)
    {
        return Value.Value switch
        {
            string name => state.Variables[name],
            UnsignedIntModType intModTypeValue => intModTypeValue,
            Expression expression => expression.Calc(state),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}