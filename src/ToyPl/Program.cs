﻿using ToyPl.Application.Models;
using ToyPl.Translation;

if (args.Length == 0)
{
    Console.WriteLine("Укажите программный файл");
    Environment.Exit(1);
}

try
{
    var (program, programVariables)  = new ProgramReader().GetProgram(args[0]);

    var sortedVariables = programVariables.Order().ToArray();
    Console.WriteLine($"Введите переменные в порядке ({string.Join(", ", sortedVariables)}):");
    var readVars = Console.ReadLine()?.Split()
        .Where(x => uint.TryParse(x, out _))
        .Select(uint.Parse)
        .ToArray();

    if (readVars?.Length != sortedVariables.Length)
    {
        Console.WriteLine("Неверное число значений переменных");
        Environment.Exit(1);
    }

    var variablesDict = sortedVariables
        .Zip(readVars)
        .Select(x => new Variable(x.First, new UnsignedIntModType(x.Second)))
        .ToDictionary(x => x.Name);
    var state = new State(variablesDict);

    var result = program.Do([state]);
    foreach (var finalState in result)
    {
        Console.WriteLine(finalState.ToBeautyString());
    }
}
catch (Exception ex)
{
    Console.Error.WriteLine(ex.Message);
    Environment.Exit(1);
}

