using ToyPl.Application.Conditions;
using ToyPl.Application.Models;
using ToyPl.Extensions;

namespace ToyPl.Application.Commands;

public interface ICommand
{
    int Id { get; }
    void SetNext(ICommand command);
    State?[] Execute(State? states);
    State?[] Execute(State?[] states);
    void FillLines(Dictionary<int, string> lines);
}

public abstract class CommandBase : ICommand
{
    private const int Out = -1;
    
    private static int _commonNumber;
    protected int? _id;
    public int Id
    {
        get
        {
            _id ??= _commonNumber++;
            return _id.Value;
        }
    }

    public abstract void SetNext(ICommand command);

    public virtual State?[] Execute(State? state)
    {
        if (state is null) return [null];
        
        return Execute([state]);
    }
    
    public abstract State?[] Execute(State?[] states);

    public void FillLines(Dictionary<int, string> lines)
    {
        if (lines.ContainsKey(Id)) return;
        
        lines[Id] = GetLine();

        foreach (var command in Goto)
        {
            command?.FillLines(lines);
        }
    }

    public static ICommand Translate(string code)
    {
        var lines = code.Split(Environment.NewLine);
        
        var createdCommands = new Dictionary<int, CommandBase>();
        foreach (var (id, type, content, gotoIds) in lines.Select(GetInfo).Reverse())
        {
            createdCommands[id] = type switch
            {
                IfCommand.TypeStr => 
                    IfCommand.Create(
                        id, 
                        content, 
                        createdCommands.GetValueOrDefault(gotoIds[0]), 
                        createdCommands.GetValueOrDefault(gotoIds[1])),
                ForkCommand.TypeStr => 
                    new ForkCommand(
                        id, 
                        gotoIds.Select(gotoId => (ICommand?) createdCommands.GetValueOrDefault(gotoId)).ToArray()),
                ChangeCommand.TypeStr => 
                    ChangeCommand.Create(
                        id, 
                        content, 
                        createdCommands.GetValueOrDefault(gotoIds[0])),
                ExitCommand.TypeStr => new ExitCommand(),
                ClosureCommand.TypeStr => 
                    new ClosureCommand(
                        id, 
                        createdCommands[gotoIds[0]], 
                        createdCommands.GetValueOrDefault(gotoIds[1])),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        return null!;
    }

    protected abstract string Type { get; }
    protected abstract string Content { get; }
    protected abstract ICommand?[] Goto { get; }
    
    private string GetLine() => $"{Id}:{Type}:{Content}:{string.Join(",", Goto.Select(x => x?.Id ?? Out))}";

    private static (int Id, string Type, string Content, int[] GotoIds) GetInfo(string line)
    {
        var info = line.Split(':');
        var id = int.Parse(info[0]);
        var type = info[1];
        var content = info[2];
        var gotoIds = info[3].Split(',').Select(int.Parse).ToArray();

        return (id, type, content, gotoIds);
    }
}

public class IfCommand(ICondition condition, ICommand? then, ICommand? @else) : CommandBase
{
    public const string TypeStr = "if";

    public static IfCommand Create(int id, string content, ICommand? then, ICommand? @else)
    {
        throw new ArgumentOutOfRangeException();
    }
    
    public IfCommand(int id, ICondition condition, ICommand? then, ICommand? @else) 
        : this(condition, then, @else)
    {
        _id = id;
    }
    
    public override void SetNext(ICommand command)
    {
        then?.SetNext(command);
        @else?.SetNext(command);
    }

    public override State?[] Execute(State?[] states)
    {
        return states
            .SelectMany(state => 
                state is null
                    ? [null]                
                    : condition.Check(state)
                        ? DoIfCan(then, state)
                        : DoIfCan(@else, state))
            .ToArray();
    }

    protected override string Type => TypeStr;
    protected override string Content => condition.ToString()!;
    protected override ICommand?[] Goto => [then, @else];

    private State?[] DoIfCan(ICommand? command, State state)
    {
        return command is null 
            ? [state] 
            : command.Execute(state);
    }
}

public class ForkCommand(params ICommand?[] possibleNextCommands) : CommandBase
{
    public const string TypeStr = "fork";

    public ForkCommand(int id, params ICommand?[] possibleNextCommands) : this(possibleNextCommands)
    {
        _id = id;
    }
    
    public override void SetNext(ICommand command)
    {
        foreach (var possibleNextCommand in possibleNextCommands)
        {
            possibleNextCommand?.SetNext(command);
        }
    }
    
    public override State?[] Execute(State?[] states)
    {
        var command = possibleNextCommands.GetRandom();

        if (command is null) return [];
        
        return possibleNextCommands
            .SelectMany(c => states.SelectMany(s => 
                s is null || c is null 
                    ? [null] 
                    : c.Execute(s)))
            .ToArray();
    }

    protected override string Type => TypeStr;
    protected override string Content => string.Empty;
    protected override ICommand?[] Goto => possibleNextCommands;
}

public class ChangeCommand(string left, PossibleValue right, ICommand? nextCommand) : CommandBase
{
    public const string TypeStr = "assign";

    public static ChangeCommand Create(int id, string content, ICommand? nextCommand)
    {
        var contentParts = content.Split(" := ");
        var left = contentParts[0];
        var right = PossibleValue.FromString(contentParts[1]);

        return new ChangeCommand(id, left, right, nextCommand);
    }
    
    public ChangeCommand(int id, string left, PossibleValue right, ICommand? nextCommand)
        : this(left, right, nextCommand)
    {
        _id = id;
    }
    
    public override void SetNext(ICommand command)
    {
        nextCommand?.SetNext(command);
    }

    public override State?[] Execute(State?[] states)
    {
        var newStates = states.Select(x => x?.Update(left, right.Calc(x))).ToArray();
        
        return nextCommand is not null 
            ? newStates.SelectMany(nextCommand.Execute).ToArray() 
            : newStates;
    }

    protected override string Type => TypeStr;
    protected override string Content => $"{left} := {right}";
    protected override ICommand?[] Goto => [nextCommand];
}

public class ExitCommand : CommandBase
{
    public const string TypeStr = "exit";
    
    public override void SetNext(ICommand command)
    {
        
    }

    public override State?[] Execute(State?[] states)
    {
        return [null];
    }

    protected override string Type => TypeStr;
    protected override string Content => "";
    protected override ICommand?[] Goto => [null];
}

public class ClosureCommand(CommandBase body) : CommandBase
{
    public const string TypeStr = "closure";
    
    private ICommand? _next;

    public ClosureCommand(int id, CommandBase body, ICommand? next) : this(body)
    {
        _id = id;
        _next = next;
    }
    
    public override void SetNext(ICommand command)
    {
        _next = command;
    }

    public override State?[] Execute(State? state)
    {
        var possibleStates = Execute([state]);

        return _next is not null 
            ? _next.Execute(possibleStates) 
            : possibleStates;
    }

    public override State?[] Execute(State?[] states)
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

        return unionStates.ToArray();
    }

    protected override string Type => TypeStr;
    protected override string Content => string.Empty;
    protected override ICommand?[] Goto => [body, _next];

    private State?[] DoN(State?[] states, int count)
    {
        var currentState = states;
        for (var i = 0; i < count; i++)
        {
            currentState = body.Execute(currentState);
        }

        return currentState;
    }
}
