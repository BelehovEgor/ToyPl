using ToyPl;
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
    var programReader = new ProgramReader();
    var (program, programVariables) = programReader.GetProgram(args[0]);

    if (args.Length > 1 && int.TryParse(args[1], out var n) && n > 3)
    {
        Constants.N = n;
    }

    string? outputMlFile = null;
    if (args.Length > 2)
    {
        outputMlFile = args[2];
    }

    var variablesDict = programVariables
        .Select(Variable.Create)
        .ToDictionary(x => x.Name, x => x.Value);
    var state = new State(variablesDict);

    var command = programReader.Translate(program, outputMlFile);
    var result = command.Execute(state).GetRandom();
    Console.WriteLine(result?.ToBeautyString() ?? "Failed");
}
catch (Exception ex)
{
    Console.Error.WriteLine(ex.Message);
    Environment.Exit(1);
}

