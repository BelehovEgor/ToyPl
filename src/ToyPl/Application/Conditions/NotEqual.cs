namespace ToyPl.Application.Conditions;

public class NotEqual() : Comparator((l, r) => l.Value != r.Value)
{
    public static Comparator Create => new NotEqual();
}