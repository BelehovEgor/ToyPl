using ToyPl.Application.Commands;
using ToyPl.Application.Operations;

namespace ToyPl.Translation;

public class ProgramReader
{
    public (IOperation, HashSet<string>) GetProgram(string filePath)
    {
        using StreamReader reader = new(filePath);

        var code = reader.ReadToEnd();

        return ToyPlTranslator.GetProgram(code);
    }

    public ICommand Translate(IOperation operation, string? exportTo = null)
    {
        var stringWriter = new StreamWriter(exportTo ?? "code.vm");
        
        var command = operation.Translate(null);

        var lines = new Dictionary<int, string>();
        command.FillLines(lines);
        foreach (var (_, line) in lines.OrderBy(x => x.Key))
        {
            stringWriter.WriteLine(line);
        }
        stringWriter.Close();
        
        return command;
    }

    public (ICommand, HashSet<string>) GetVm(string filePath)
    {
        using StreamReader reader = new(filePath);

        var code = reader.ReadToEnd();
        var commands = new Dictionary<int, CommandBase>();
        var vars = new HashSet<string>();
        var lines = code.Split(Environment.NewLine)
            .Where(x => x != string.Empty)
            .Select(GetInfo)
            .OrderByDescending(x => x.Id)
            .ToArray();
        var command = lines
            .Select(line =>
            {
                var (id, type, content, gotoIds) = line;
                return CreateCommand(id, type, content, gotoIds, commands, vars);
            })
            .MinBy(x => x.Id)!;

        return (command, vars);
    }

    private static ICommand CreateCommand(
        int id, string type, string content, int[] gotoIds, Dictionary<int, CommandBase> commands, HashSet<string> vars)
    {
        CommandBase command = type switch
        {
            AssignCommand.TypeStr => new AssignCommand(
                id,
                ToyPlTranslator.GetPossibleValue(content.Split('=')[0], vars).Value.AsT0, 
                ToyPlTranslator.GetPossibleValue(content.Split('=')[1], vars),
                gotoIds.Length == 1
                    ? commands.GetValueOrDefault(gotoIds[0])
                    : null),
            ClosureCommand.TypeStr => new ClosureCommand(
                id,
                commands[gotoIds[0]],
                gotoIds.Length == 2
                    ? commands.GetValueOrDefault(gotoIds[1])
                    : null),
            EmptyCommand.TypeStr => new EmptyCommand(
                id, 
                gotoIds.Length == 1
                    ? commands.GetValueOrDefault(gotoIds[0])
                    : null),
            ExitCommand.TypeStr => new ExitCommand(id),
            ForkCommand.TypeStr => new ForkCommand(
                id,
                commands[gotoIds[0]],
                commands[gotoIds[1]],
                gotoIds.Length == 3
                    ? commands.GetValueOrDefault(gotoIds[2])
                    : null),
            IfCommand.TypeStr => new IfCommand(
                id, 
                ToyPlTranslator.GetCondition(content, vars), 
                commands[gotoIds[0]],
                commands[gotoIds[1]]),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };

        commands[id] = command;

        return command;
    }
    
    private static (int Id, string Type, string Content, int[] GotoIds) GetInfo(string line)
    {
        var info = line.Split(':');
        var id = int.Parse(info[0]);
        var type = info[1];
        var content = info[2];
        var gotoIds = info[3] == string.Empty
            ? []
            : info[3].Split(',').Select(int.Parse).ToArray();

        return (id, type, content, gotoIds);
    }
}