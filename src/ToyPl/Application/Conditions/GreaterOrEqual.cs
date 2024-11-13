namespace ToyPl.Application.Conditions;

public class GreaterOrEqual() : Comparator((l, r) => l.Value >= r.Value)
{
    public static Comparator Create => new GreaterOrEqual();
}