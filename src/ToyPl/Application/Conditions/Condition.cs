using ToyPl.Application.Models;

namespace ToyPl.Application.Conditions;

public interface ICondition
{
    bool Check(State state);
}

public class Condition(PossibleValue[] expressions, Comparator comparator)
    : ICondition
{
    public bool Check(State state)
    {
        return comparator.Invoke(state, expressions);
    }
}

public class NotCondition(ICondition condition) : ICondition
{
    public bool Check(State state)
    {
        return !condition.Check(state);
    }
}

public class AndCondition(ICondition left, ICondition right) : ICondition
{
    public bool Check(State state)
    {
        return left.Check(state) && right.Check(state);
    }
}

public class OrCondition(ICondition left, ICondition right) : ICondition
{
    public bool Check(State state)
    {
        return left.Check(state) || right.Check(state);
    }
}

