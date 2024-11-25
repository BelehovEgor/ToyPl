using ToyPl.Application.Models;

namespace ToyPl.Application.Conditions;

public abstract class Comparator(Func<UnsignedIntModType, UnsignedIntModType, bool> predicate)
{
    public bool Invoke(State state, params PossibleValue[] expressions)
    {
        if (expressions.Length != 2) throw new InvalidOperationException();

        return predicate(expressions[0].Calc(state), expressions[1].Calc(state));
    }

    public static Comparator FromString(string line)
    {
        return line switch
        {
            "=" => Equal.Create,
            "/=" => NotEqual.Create,
            ">" => Greater.Create,
            ">=" => GreaterOrEqual.Create,
            "<" => Less.Create,
            "<=" => LessOrEqual.Create,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}

public class LessOrEqual() : Comparator((l, r) => l.Value <= r.Value)
{
    public static Comparator Create => new LessOrEqual();

    public override string ToString() => "<=";
}

public class NotEqual() : Comparator((l, r) => l.Value != r.Value)
{
    public static Comparator Create => new NotEqual();
    
    public override string ToString() => "!=";
}

public class Less() : Comparator((l, r) => l.Value < r.Value)
{
    public static Comparator Create => new Less();
    
    public override string ToString() => "<";
}

public class GreaterOrEqual() : Comparator((l, r) => l.Value >= r.Value)
{
    public static Comparator Create => new GreaterOrEqual();
    
    
    public override string ToString() => ">=";
}

public class Greater() : Comparator((l, r) => l.Value > r.Value)
{
    public static Comparator Create => new Greater();
    
    public override string ToString() => ">";
}


public class Equal() : Comparator((l, r) => l.Value == r.Value)
{
    public static Comparator Create => new Equal();
    
    public override string ToString() => "=";
}