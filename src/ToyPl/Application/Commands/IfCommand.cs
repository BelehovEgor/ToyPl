using ToyPl.Application.Conditions;
using ToyPl.Application.Models;

namespace ToyPl.Application.Commands;

public class IfCommand(ICondition condition, ICommand then, ICommand @else) : CommandBase(null)
{
    public const string TypeStr = "if";

    protected override State?[] ExecuteInternal(State?[] states)
    {
        return states
            .SelectMany(state => 
                state is null
                    ? [state]                
                    : condition.Check(state)
                        ? DoIfCan(then, state)
                        : DoIfCan(@else, state))
            .ToArray();
    }

    protected override string Type => TypeStr;
    protected override string Content => condition.ToString()!;
    protected override ICommand[] Goto => [then, @else];

    private State?[] DoIfCan(ICommand? command, State state)
    {
        return command is null 
            ? [state] 
            : command.Execute([state]);
    }
}