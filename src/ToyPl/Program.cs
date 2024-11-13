// See https://aka.ms/new-console-template for more information

using Antlr4.Runtime;
using ToyPl.Application.Models;
using ToyPl.Translation;

var toyPlProgram = 
    """
    (
        ((a := (a + 2)) U (a := (a + 4)) * ) ; ((a % 4) = 1) ?
    )
    """;

var stream = CharStreams.fromString(toyPlProgram);
var lexer = new toyPlLexer(stream);
var tokens = new CommonTokenStream(lexer);
var parser = new toyPlParser(tokens);

try
{
    var visitor = new ToyPlTranslator();
    var program = parser.program().Accept(visitor);

    var variable = new Variable("a", new UnsignedIntModType(1));
    var variables = new Dictionary<string, Variable>
    {
        { "a", variable }
    };
    var state = new State(variables);

    var result = program.Do([state]);
}
catch (Exception ex)
{
    Console.Error.WriteLine(ex.Message);
    Environment.Exit(1);
}

