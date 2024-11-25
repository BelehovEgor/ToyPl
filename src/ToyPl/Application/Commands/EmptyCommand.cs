using ToyPl.Application.Models;

namespace ToyPl.Application.Commands;

public class EmptyCommand(ICommand? next) : CommandBase(next)
{
    public const string TypeStr = "empty";

    protected override State?[] ExecuteInternal(State?[] states)
    {
        return states;
    }

    protected override string Type => TypeStr;
    protected override string Content => "";
    protected override ICommand[] Goto => Next is null ? [] : [Next];
}