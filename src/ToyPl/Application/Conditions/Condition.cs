using ToyPl.Application.Models;

namespace ToyPl.Application.Conditions;

public interface ICondition
{
    bool Check(State state);
}

public abstract class ConditionBase : ICondition
{
    public abstract bool Check(State state);
}

public class Condition(PossibleValue[] expressions, Comparator comparator) : ConditionBase
{
    public override bool Check(State state)
    {
        return comparator.Invoke(state, expressions);
    }
    
    public override string ToString() => $"({expressions[0]} {comparator} {expressions[1]})";
}

public class NotCondition(ICondition condition) : ConditionBase
{
    public override bool Check(State state)
    {
        return !condition.Check(state);
    }

    public override string ToString() => $"(! {condition})";
}

public class AndCondition(ICondition left, ICondition right) : ConditionBase
{
    public override bool Check(State state)
    {
        return left.Check(state) && right.Check(state);
    }
    
    public override string ToString() => $"({left} && {right})";
}

public class OrCondition(ICondition left, ICondition right) : ConditionBase
{
    public override bool Check(State state)
    {
        return left.Check(state) || right.Check(state);
    }
    
    public override string ToString() => $"({left} || {right})";
}

