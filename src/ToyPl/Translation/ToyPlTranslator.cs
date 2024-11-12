using Antlr4.Runtime.Tree;
using ToyPl.Application.Expressions;
using ToyPl.Application.Models;
using ToyPl.Application.Operations;

namespace ToyPl.Translation;

public class ToyPlTranslator : ItoyPlParserVisitor<IOperation>
{
    private readonly HashSet<string> _variables = new();

    public IReadOnlyCollection<string> Variables => _variables;
    
    public IOperation Visit(IParseTree tree)
    {
        throw new NotImplementedException();
    }

    public IOperation VisitChildren(IRuleNode node)
    {
        throw new NotImplementedException();
    }

    public IOperation VisitTerminal(ITerminalNode node)
    {
        throw new NotImplementedException();
    }

    public IOperation VisitErrorNode(IErrorNode node)
    {
        throw new NotImplementedException();
    }

    public IOperation VisitVar(toyPlParser.VarContext context)
    {
        throw new NotImplementedException();
    }

    public IOperation VisitExpr(toyPlParser.ExprContext context)
    {
        throw new NotImplementedException();
    }

    public IOperation VisitInt_op(toyPlParser.Int_opContext context)
    {
        throw new NotImplementedException();
    }

    public IOperation VisitCond_int_op(toyPlParser.Cond_int_opContext context)
    {
        throw new NotImplementedException();
    }

    public IOperation VisitCond_bool_op(toyPlParser.Cond_bool_opContext context)
    {
        throw new NotImplementedException();
    }

    public IOperation VisitCond(toyPlParser.CondContext context)
    {
        throw new NotImplementedException();
    }

    public IOperation VisitStatement(toyPlParser.StatementContext context)
    {
        return context.children switch
        {
            [
                toyPlParser.VarContext varContext, 
                TerminalNodeImpl { Symbol.Text: ":=" }, 
                toyPlParser.ExprContext exprContext
            ] => new AssignOperation(GetVar(varContext), GetExpression(exprContext)),
            
            [toyPlParser.CondContext condContext, TerminalNodeImpl { Symbol.Text: "?" }]
                => new TestOperation(GetPredicate(condContext)),
            
            [
                TerminalNodeImpl { Symbol.Text: "(" }, 
                toyPlParser.StatementContext left, 
                TerminalNodeImpl { Symbol.Text: ";" }, 
                toyPlParser.StatementContext right, 
                TerminalNodeImpl { Symbol.Text: ")" }
            ] => new CompositionOperation(left.Accept(this), right.Accept(this)),
            
            [
                TerminalNodeImpl { Symbol.Text: "(" }, 
                toyPlParser.StatementContext left, 
                TerminalNodeImpl { Symbol.Text: "U" }, 
                toyPlParser.StatementContext right, 
                TerminalNodeImpl { Symbol.Text: ")" }
            ] => new UnionOperation(left.Accept(this), right.Accept(this)),
            
            [
                toyPlParser.StatementContext statement, 
                TerminalNodeImpl { Symbol.Text: "*" }
            ] => new ClosureOperation(statement.Accept(this)),
            
            [
                TerminalNodeImpl { Symbol.Text: "(" }, 
                toyPlParser.StatementContext statement, 
                TerminalNodeImpl { Symbol.Text: ")" }
            ] => statement.Accept(this),
            
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public IOperation VisitProgram(toyPlParser.ProgramContext context)
    {
        return context.statement().Accept(this);
    }
    
    private string GetVar(toyPlParser.VarContext varContext)
    {
        var varName = varContext.GetText();

        _variables.Add(varName);

        return varName;
    }

    private Expression GetExpression(toyPlParser.ExprContext exprContext)
    {
        return new Expression("a", "a", new PlusOperation());
    }
    
    private Predicate<State> GetPredicate(toyPlParser.CondContext condContext)
    {
        return _ => true;
    }
}