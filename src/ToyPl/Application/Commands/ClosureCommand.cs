using ToyPl.Application.Models;

namespace ToyPl.Application.Commands;

public class ClosureCommand : CommandBase
{
    public const string TypeStr = "closure";

    private readonly CommandBase _body;
    
    public ClosureCommand(CommandBase body, ICommand? next) : base(next)
    {
        _body = body;
    }
    
    public ClosureCommand(int id, CommandBase body, ICommand? next) : this(body, next)
    {
        _id = id;
    }

    protected override State?[] ExecuteInternal(State?[] states)
    {
        if (states.Length == 0) return [];
        
        var k = 1;
        var unionStates = new HashSet<State?>(new StateComparer());
        
        int prevCount;
        do
        {
            prevCount = unionStates.Count;
            
            var newStates = DoN(states, k++);
            
            foreach (var state in newStates)
            {
                unionStates.Add(state);
            }
        } while (prevCount != unionStates.Count);

        return unionStates.Where(s => s is not null).ToArray();
    }

    protected override string Type => TypeStr;
    protected override string Content => string.Empty;
    protected override ICommand[] Goto => Next is null ? [_body] : [_body, Next];

    private State?[] DoN(State?[] states, int count)
    {
        var currentState = states;
        for (var i = 0; i < count; i++)
        {
            currentState = _body.Execute(currentState);
        }

        return currentState;
    }
}