namespace ToyPl.Application.Conditions;

public class LessOrEqual() : Comparator((l, r) => l.Value <= r.Value)
{
    public static Comparator Create => new LessOrEqual();
}