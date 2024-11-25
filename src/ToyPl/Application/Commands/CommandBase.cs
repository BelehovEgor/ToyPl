using ToyPl.Application.Models;

namespace ToyPl.Application.Commands;

public abstract class CommandBase(ICommand? next) : ICommand
{
    private static int _commonNumber;
    private int? _id;
    public int Id
    {
        get
        {
            _id ??= _commonNumber++;
            return _id.Value;
        }
    }

    public ICommand? Next { get; } = next;

    public State?[] Execute(State?[] states)
    {
        var newStates = ExecuteInternal(states);

        return Next is null
            ? newStates
            : Next.Execute(newStates);
    }
    
    public void FillLines(Dictionary<int, string> lines)
    {
        if (lines.ContainsKey(Id)) return;
        
        lines[Id] = GetLine();

        foreach (var command in Goto)
        {
            command.FillLines(lines);
        }
    }

    protected abstract string Type { get; }
    protected abstract string Content { get; }
    protected abstract ICommand[] Goto { get; }
    protected abstract State?[] ExecuteInternal(State?[] states);

    
    private string GetLine() => $"{Id}:{Type}:{Content}:{string.Join(",", Goto.Select(x => x.Id))}";
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