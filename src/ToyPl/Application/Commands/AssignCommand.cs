using ToyPl.Application.Models;

namespace ToyPl.Application.Commands;

public class AssignCommand(string left, PossibleValue right, ICommand? nextCommand) : CommandBase(nextCommand)
{
    public const string TypeStr = "assign";

    protected override State?[] ExecuteInternal(State?[] states)
    {
        return states.Select(x => x?.Update(left, right.Calc(x))).ToArray();
    }

    protected override string Type => TypeStr;
    protected override string Content => $"{left}={right}";
    protected override ICommand[] Goto => Next is null ? [] : [Next];
}