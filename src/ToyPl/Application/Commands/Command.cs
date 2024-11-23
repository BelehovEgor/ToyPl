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
    void Print(TextWriter writer);
}

public abstract class CommandBase : ICommand
{
    private static readonly HashSet<int> AlreadyPrinted = new();
    private static int _commonNumber;
    protected const int Out = -1;

    private int? _id;
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

    public void Print(TextWriter writer)
    {
        var id = Id;
        if (!AlreadyPrinted.Add(id)) return;
        
        TryPrint(writer);
    }

    public abstract void TryPrint(TextWriter writer);
}

public class IfCommand(ICondition condition, ICommand? then, ICommand? @else) : CommandBase
{ 
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

    public override void TryPrint(TextWriter writer)
    {
        writer.WriteLine(
            $"{Id, 3}: if ({condition}) then {then?.Id ?? Out} else {@else?.Id ?? Out}");
        then?.Print(writer);
        @else?.Print(writer);
    }

    private State?[] DoIfCan(ICommand? command, State state)
    {
        return command is null 
            ? [state] 
            : command.Execute(state);
    }
}

public class ForkCommand(params ICommand?[] possibleNextCommands) : CommandBase
{
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

    public override void TryPrint(TextWriter writer)
    {
        writer.WriteLine($"{Id, 3}: fork {string.Join(", ", possibleNextCommands.Select(x => x?.Id ?? Out))}");
        foreach (var possibleNextCommand in possibleNextCommands)
        {
            possibleNextCommand?.Print(writer);
        }
    }
}

public class ChangeCommand(string left, PossibleValue right, ICommand? nextCommand) : CommandBase
{
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

    public override void TryPrint(TextWriter writer)
    {
        writer.WriteLine($"{Id, 3}: {left} := {right} goto {nextCommand?.Id ?? Out}");
        nextCommand?.Print(writer);
    }
}

public class ExitCommand : CommandBase
{
    public override void SetNext(ICommand command)
    {
        
    }

    public override State?[] Execute(State?[] states)
    {
        return [null];
    }

    public override void TryPrint(TextWriter writer)
    {
        writer.WriteLine($"{Id, 3}: exit {Out}");
    }
}

public class ClosureCommand(CommandBase body) : CommandBase
{
    private ICommand? _next;
    
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

    public override void TryPrint(TextWriter writer)
    {
        writer.WriteLine($"{Id, 3}: closure body {body.Id} out {_next?.Id ?? Out}");
        body.Print(writer);
        _next?.Print(writer);
    }

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
