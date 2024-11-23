using ToyPl.Application.Models;
using ToyPl.Extensions;

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
        return $"{left} {operation} {right}";
    }
}

public abstract class Operation(Func<UnsignedIntModType, UnsignedIntModType, UnsignedIntModType> func)
{
    public UnsignedIntModType Invoke(UnsignedIntModType left, UnsignedIntModType right) => func(left, right);
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