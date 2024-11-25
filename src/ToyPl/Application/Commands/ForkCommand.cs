using ToyPl.Application.Models;
using ToyPl.Extensions;

namespace ToyPl.Application.Commands;

public class ForkCommand(ICommand left, ICommand right, ICommand? next) : CommandBase(next)
{
    public const string TypeStr = "fork";

    public ForkCommand(int id, ICommand left, ICommand right, ICommand? next) : this(left, right, next)
    {
        _id = id;
    }
    
    protected override State?[] ExecuteInternal(State?[] states)
    {
        var command = new[] { left, right }.GetRandom();

        if (command is null) return [];

        return left.Execute(states)
            .Union(right.Execute(states))
            .Where(x => x is not null)
            .ToArray();
    }

    protected override string Type => TypeStr;
    protected override string Content => string.Empty;
    protected override ICommand[] Goto => Next is null ? [left, right] : [left, right, Next];
}