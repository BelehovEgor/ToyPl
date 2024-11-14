namespace ToyPl.Application.Conditions;

public class Greater() : Comparator((l, r) => l.Value > r.Value)
{
    public static Comparator Create => new Greater();
}