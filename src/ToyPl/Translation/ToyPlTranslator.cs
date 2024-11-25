using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using ToyPl.Application.Conditions;
using ToyPl.Application.Expressions;
using ToyPl.Application.Models;
using ToyPl.Application.Operations;

namespace ToyPl.Translation;

public static class ToyPlTranslator
{
    public static (IOperation program, HashSet<string> variables) GetProgram(string code)
    {
        var stream = CharStreams.fromString(code);
        var lexer = new toyPlLexer(stream);
        var tokens = new CommonTokenStream(lexer);
        var parser = new toyPlParser(tokens);
        
        var vars = new HashSet<string>();
        return (VisitStatement(parser.program().statement(), vars), vars);
    }

    public static PossibleValue GetPossibleValue(string code, HashSet<string> vars)
    {
        if (code.All(char.IsLetter))
        {
            vars.Add(code);
            return new PossibleValue(code);
        }

        if (code.All(char.IsDigit))
        {
            return new PossibleValue(new UnsignedIntModType(uint.Parse(code)));
        }
        
        var stream = CharStreams.fromString(code);
        var lexer = new toyPlLexer(stream);
        var tokens = new CommonTokenStream(lexer);
        var parser = new toyPlParser(tokens);
        
        return GetExpression(parser.expr(), vars);
    }
    
    public static ICondition GetCondition(string code, HashSet<string> vars)
    {
        var stream = CharStreams.fromString(code);
        var lexer = new toyPlLexer(stream);
        var tokens = new CommonTokenStream(lexer);
        var parser = new toyPlParser(tokens);
        
        return GetPredicate(parser.cond(), vars);
    }
    
    private static IOperation VisitStatement(toyPlParser.StatementContext context, HashSet<string> vars)
    {
        return context.children switch
        {
            [
                toyPlParser.VarContext varContext, 
                TerminalNodeImpl { Symbol.Text: ":=" }, 
                toyPlParser.ExprContext exprContext
            ] => new AssignOperation(GetVar(varContext, vars), GetExpression(exprContext, vars)),
            
            [
                toyPlParser.CondContext condContext, 
                TerminalNodeImpl { Symbol.Text: "?" }
            ] => new TestOperation(GetPredicate(condContext, vars)),
            
            [
                TerminalNodeImpl { Symbol.Text: "(" }, 
                toyPlParser.StatementContext left, 
                TerminalNodeImpl { Symbol.Text: ";" }, 
                toyPlParser.StatementContext right, 
                TerminalNodeImpl { Symbol.Text: ")" }
            ] => new CompositionOperation(VisitStatement(left, vars), VisitStatement(right, vars)),
            
            [
                TerminalNodeImpl { Symbol.Text: "(" }, 
                toyPlParser.StatementContext left, 
                TerminalNodeImpl { Symbol.Text: "U" }, 
                toyPlParser.StatementContext right, 
                TerminalNodeImpl { Symbol.Text: ")" }
            ] => new UnionOperation(VisitStatement(left, vars), VisitStatement(right, vars)),
            
            [
                toyPlParser.StatementContext statement, 
                TerminalNodeImpl { Symbol.Text: "*" }
            ] => new ClosureOperation(VisitStatement(statement, vars)),
            
            [
                TerminalNodeImpl { Symbol.Text: "(" }, 
                toyPlParser.StatementContext statement, 
                TerminalNodeImpl { Symbol.Text: ")" }
            ] => VisitStatement(statement, vars),
            
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static string GetVar(toyPlParser.VarContext varContext, HashSet<string> vars)
    {
        var name = varContext.GetText();

        vars.Add(name);
        
        return name;
    }

    private static PossibleValue GetExpression(toyPlParser.ExprContext exprContext, HashSet<string> vars)
    {
        return exprContext.children switch
        {
            [ toyPlParser.VarContext varContext ]
                => new PossibleValue(GetVar(varContext, vars)),
            
            [ TerminalNodeImpl terminalNodeImpl ] 
                => new PossibleValue(new UnsignedIntModType(uint.Parse(terminalNodeImpl.GetText()))),
            
            [
                TerminalNodeImpl { Symbol.Text: "(" }, 
                toyPlParser.ExprContext left, 
                toyPlParser.Int_opContext opContext, 
                toyPlParser.ExprContext right, 
                TerminalNodeImpl { Symbol.Text: ")" }
            ] => new PossibleValue(
                    new Expression(
                        GetExpression(left, vars), 
                        GetExpression(right, vars), 
                        GetOperation(opContext))),
            
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static ICondition GetPredicate(toyPlParser.CondContext condContext, HashSet<string> vars)
    {
        return condContext.children switch
        {
            [
                TerminalNodeImpl { Symbol.Text: "(" }, 
                toyPlParser.ExprContext left, 
                toyPlParser.Cond_int_opContext opContext, 
                toyPlParser.ExprContext right, 
                TerminalNodeImpl { Symbol.Text: ")" }
            ] => new Condition([GetExpression(left, vars), GetExpression(right, vars)], GetComparator(opContext)),
            
            [
                TerminalNodeImpl { Symbol.Text: "(" }, 
                TerminalNodeImpl { Symbol.Text: "!" }, 
                toyPlParser.CondContext cond,
                TerminalNodeImpl { Symbol.Text: ")" }
            ] => new NotCondition(GetPredicate(cond, vars)),
            
            [
                TerminalNodeImpl { Symbol.Text: "(" }, 
                toyPlParser.CondContext left, 
                toyPlParser.Cond_bool_opContext opContext, 
                toyPlParser.CondContext right, 
                TerminalNodeImpl { Symbol.Text: ")" }
            ] when opContext.GetText() == "&&"
                => new AndCondition(GetPredicate(left, vars), GetPredicate(right, vars)),
            
            [
                TerminalNodeImpl { Symbol.Text: "(" }, 
                toyPlParser.CondContext left, 
                toyPlParser.Cond_bool_opContext opContext, 
                toyPlParser.CondContext right, 
                TerminalNodeImpl { Symbol.Text: ")" }
            ] when opContext.GetText() == "||"
                => new OrCondition(GetPredicate(left, vars), GetPredicate(right, vars)),
            
            [
                TerminalNodeImpl { Symbol.Text: "(" }, 
                toyPlParser.CondContext cond, 
                TerminalNodeImpl { Symbol.Text: ")" }
            ] => GetPredicate(cond, vars),
            
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static Comparator GetComparator(toyPlParser.Cond_int_opContext opContext)
    {
        return Comparator.FromString(opContext.GetText());
    }

    private static Operation GetOperation(toyPlParser.Int_opContext opContext)
    {
        return Operation.FromString(opContext.GetText());
    }
}