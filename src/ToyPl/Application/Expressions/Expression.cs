using ToyPl.Application.Models;

namespace ToyPl.Application.Expressions;

public class Expression(
    PossibleValue left, 
    PossibleValue right, 
    Operation operation)
{
    public UnsignedIntModType Calc(State state)
    {
        return operation.Invoke(left.Calc(state), right.Calc(state));
    }

    public override string ToString()
    {
        return $"({left} {operation} {right})";
    }

    public static Expression FromString(string line)
    {
        if (line.StartsWith('(') && line.EndsWith(')'))
        {
            var parts = line.Substring(1, line.Length - 2).Split(' ');
            var left = PossibleValue.FromString(parts[0]);
            var operation = Operation.FromString(parts[1]);
            var right = PossibleValue.FromString(parts[2]);

            return new Expression(left, right, operation);
        }
        
        throw new ArgumentOutOfRangeException();
    }
}

public abstract class Operation(Func<UnsignedIntModType, UnsignedIntModType, UnsignedIntModType> func)
{
    public UnsignedIntModType Invoke(UnsignedIntModType left, UnsignedIntModType right) => func(left, right);

    public static Operation FromString(string line)
    {
        return line switch
        {
            "+" => PlusOperation.Create,
            "-" => MinusOperation.Create,
            "*" => TimesOperation.Create,
            "/" => DivOperation.Create,
            "%" => ModOperation.Create,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}

public class DivOperation() : Operation((left, right) => new UnsignedIntModType(left.Value / right.Value))
{
    public static Operation Create => new DivOperation();

    public override string ToString() => "div";
}

public class MinusOperation() : Operation((left, right) => new UnsignedIntModType(left.Value - right.Value))
{
    public static Operation Create => new MinusOperation();

    public override string ToString() => "minus";
}

public class ModOperation() : Operation((left, right) => new UnsignedIntModType(left.Value % right.Value))
{
    public static Operation Create => new ModOperation();
    
    public override string ToString() => "mod";
}

public class PlusOperation() : Operation((left, right) => new UnsignedIntModType(left.Value + right.Value))
{
    public static Operation Create => new PlusOperation();
    
    public override string ToString() => "plus";
}

public class TimesOperation() : Operation((left, right) => new UnsignedIntModType(left.Value * right.Value))
{
    public static Operation Create => new TimesOperation();
    
    public override string ToString() => "times";
}