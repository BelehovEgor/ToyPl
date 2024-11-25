using ToyPl.Application.Models;

namespace ToyPl.Application.Commands;

public class ExitCommand() : CommandBase(null)
{
    public const string TypeStr = "exit";

    public ExitCommand(int id) : this()
    {
        _id = id;
    }
    
    protected  override State?[] ExecuteInternal(State?[] states)
    {
        return states.Select(_ => null as State).ToArray();
    }

    protected override string Type => TypeStr;
    protected override string Content => "";
    protected override ICommand[] Goto => [];
}