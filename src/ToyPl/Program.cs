// See https://aka.ms/new-console-template for more information

using Antlr4.Runtime;
using ToyPl.Translation;

var toyPlProgram = 
    """
    (a = a) ?
    """;

var stream = CharStreams.fromString(toyPlProgram);
var lexer = new toyPlLexer(stream);
var tokens = new CommonTokenStream(lexer);
var parser = new toyPlParser(tokens);

try
{
    var visitor = new ToyPlTranslator();
    parser.program().Accept(visitor);
}
catch (Exception ex)
{
    Console.Error.WriteLine(ex.Message);
    Environment.Exit(1);
}

