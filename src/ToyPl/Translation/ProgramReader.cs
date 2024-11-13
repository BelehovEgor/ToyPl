using Antlr4.Runtime;
using ToyPl.Application.Operations;

namespace ToyPl.Translation;

public class ProgramReader
{
    public (IOperation program, ICollection<string> variables) GetProgram(string filePath)
    {
        using StreamReader reader = new(filePath);

        var code = reader.ReadToEnd();
        
        var stream = CharStreams.fromString(code);
        var lexer = new toyPlLexer(stream);
        var tokens = new CommonTokenStream(lexer);
        var parser = new toyPlParser(tokens);
        
        var visitor = new ToyPlTranslator();
        var program = parser.program().Accept(visitor);

        return (program, visitor.Variables.ToArray());
    }
}