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
        return new AssignCommand(LeftValue, RightValue, next);
    }
}

public record ClosureOperation(IOperation Body) : IOperation
{
    public CommandBase Translate(ICommand? next)
    {
        return new ClosureCommand(Body.Translate(null), next);
    }
}

public record CompositionOperation(IOperation First, IOperation Second) : IOperation
{
    public CommandBase Translate(ICommand? next)
    {
        var secondTranslated = Second.Translate(next);
        return First.Translate(secondTranslated);
    }
}

public record TestOperation(ICondition Condition) : IOperation
{
    public CommandBase Translate(ICommand? next)
    {
        return new IfCommand(Condition, next ?? new EmptyCommand(next), new ExitCommand(), null);
    }
}

public record UnionOperation(IOperation First, IOperation Second) : IOperation
{
    public CommandBase Translate(ICommand? next)
    {
        var firstTranslate = First.Translate(null);
        var secondTranslate = Second.Translate(null);

        return new ForkCommand(firstTranslate, secondTranslate, next);
    }
}