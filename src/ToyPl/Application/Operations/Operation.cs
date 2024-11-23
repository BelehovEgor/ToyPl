using ToyPl.Application.Commands;
using ToyPl.Application.Conditions;

namespace ToyPl.Application.Operations;

public interface IOperation
{
    public CommandBase Translate(ICommand? next);
}

public record AssignOperation(string LeftValue, PossibleValue RightValue) : IOperation
{
    public CommandBase Translate(ICommand? next)
    {
        return new ChangeCommand(LeftValue, RightValue, next);
    }
}

public record ClosureOperation(IOperation Body) : IOperation
{
    public CommandBase Translate(ICommand? next)
    {
        var closure = new ClosureCommand(Body.Translate(null));
        
        if (next is not null) closure.SetNext(next);

        return closure;
    }
}

public record CompositionOperation(IOperation First, IOperation Second) : IOperation
{
    public CommandBase Translate(ICommand? next)
    {
        var secondTranslated = Second.Translate(next);
        var firstTranslated = First.Translate(secondTranslated);

        return firstTranslated;
    }
}

public record TestOperation(ICondition Condition) : IOperation
{
    public CommandBase Translate(ICommand? next)
    {
        return new IfCommand(Condition, next, new ExitCommand());
    }
}

public record UnionOperation(IOperation First, IOperation Second) : IOperation
{
    public CommandBase Translate(ICommand? next)
    {
        var firstTranslate = First.Translate(next);
        var secondTranslate = Second.Translate(next);

        return new ForkCommand(firstTranslate, secondTranslate);
    }
}