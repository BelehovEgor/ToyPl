using ToyPl;
using ToyPl.Application.Commands;
using ToyPl.Application.Models;
using ToyPl.Extensions;
using ToyPl.Translation;

if (args.Length == 0)
{
    Console.WriteLine("First argument should be path to program file");
    Environment.Exit(1);
}

try
{
    if (args.Length > 1 && int.TryParse(args[1], out var n) && n > 3)
    {
        Constants.N = n;
    }

    string? outputMlFile = null;
    if (args.Length > 2)
    {
        outputMlFile = args[2];
    }

    var (command, programVariables) = GetCommand(args[0], outputMlFile);

    var variablesDict = programVariables
        .Select(Variable.Create)
        .ToDictionary(x => x.Name, x => x.Value);
    var state = new State(variablesDict);
    
    var result = command.Execute([state]).GetRandom();
    Console.WriteLine(result?.ToBeautyString() ?? "Failed");
}
catch (Exception ex)
{
    Console.Error.WriteLine(ex.Message);
    Environment.Exit(1);
}

(ICommand, HashSet<string>) GetCommand(string fileName, string? outputMlFile)
{
    var file = new FileInfo(fileName);
    var extension = file.Extension;

    return extension switch
    {
        ".tpl" => GetTplCommand(fileName, outputMlFile),
        ".vm" => GetVmProgram(fileName),
        _ => throw new ArgumentOutOfRangeException()
    };
}

(ICommand, HashSet<string>) GetTplCommand(string fileName, string? outputMlFile)
{
    var programReader = new ProgramReader();
    var (program, programVariables) = programReader.GetProgram(fileName);
    
    var command = programReader.Translate(program, outputMlFile);

    return (command, programVariables);
}

(ICommand, HashSet<string>) GetVmProgram(string fileName)
{
    return new ProgramReader().GetVm(fileName);
}

