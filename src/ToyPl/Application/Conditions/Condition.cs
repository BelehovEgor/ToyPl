using ToyPl.Application.Models;

namespace ToyPl.Application.Conditions;

public interface ICondition
{
    bool Check(State state);
}

public class Condition(PossibleValue[] expressions, Comparator comparator) : ICondition
{
    public bool Check(State state)
    {
        return comparator.Invoke(state, expressions);
    }
    
    public override string ToString() => $"({expressions[0]} {comparator} {expressions[1]})";
}

public class NotCondition(ICondition condition) : ICondition
{
    public bool Check(State state)
    {
        return !condition.Check(state);
    }

    public override string ToString() => $"(not {condition})";
}

public class AndCondition(ICondition left, ICondition right) : ICondition
{
    public bool Check(State state)
    {
        return left.Check(state) && right.Check(state);
    }
    
    public override string ToString() => $"({left} and {right})";
}

public class OrCondition(ICondition left, ICondition right) : ICondition
{
    public bool Check(State state)
    {
        return left.Check(state) || right.Check(state);
    }
    
    public override string ToString() => $"({left} or {right})";
}

