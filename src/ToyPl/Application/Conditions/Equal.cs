namespace ToyPl.Application.Conditions;

public class Equal() : Comparator((l, r) => l.Value == r.Value)
{
    public static Comparator Create => new Equal();
}