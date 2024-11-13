namespace ToyPl.Application.Conditions;

public class Less() : Comparator((l, r) => l.Value < r.Value)
{
    public static Comparator Create => new Less();
}