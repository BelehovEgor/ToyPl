using ToyPl.Extensions;

namespace ToyPl.Application.Models;

public record State(IDictionary<string, UnsignedIntModType> Variables)
{
    public bool IsEqual(State other)
    {
        var comparer = new StateComparer();

        return comparer.Equals(this, other);
    }

    public string ToBeautyString()
    {
        return $"({string.Join(", ", Variables.OrderBy(x => x.Key).Select(x => $"{x.Key}: {x.Value.Value}"))})";
    }

    public State Update(string left, UnsignedIntModType right)
    {
        return new State(Variables.Clone())
        {
            Variables =
            {
                [left] = right
            }
        };
    }
}

public class StateComparer : IEqualityComparer<State?>
{
    public bool Equals(State? x, State? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.Variables.Count != y.Variables.Count) return false;

        foreach (var (name, variable) in x.Variables)
        {
            if (y.Variables.TryGetValue(name, out var otherVar) && otherVar != variable)
            {
                return false;
            }
        }

        return true;
    }

    public int GetHashCode(State obj)
    {
        var hash = 0;
        foreach (var (_, variable) in obj.Variables)
        {
            hash += variable.GetHashCode();
        }

        return hash;
    }
}