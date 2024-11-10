using ToyPl.Application.Models;

namespace ToyPl.UnitTests.Builders;

public class StateBuilder
{
    private static uint _value;
    private static uint Next => _value++ % UnsignedIntModType.MaxValue;

    private List<Variable> _variables = new();

    public StateBuilder WithVariable(Variable variable)
    {
        _variables.Add(variable);
        return this;
    }
    
    public StateBuilder WithVariable(string name, uint value)
    {
        _variables.Add(new Variable(name, new UnsignedIntModType(value)));
        return this;
    }

    public State Build()
    {
        return new State(_variables.ToDictionary(x => x.Name));
    }
    
    public static State Build(int variablesCount) => new(
        Enumerable
            .Range(0, variablesCount)
            .Select(x => new Variable(x.ToString(), new UnsignedIntModType(Next)))
            .ToDictionary(x => x.Name));
    
    public static State Build(Variable variable) => new(
        new Dictionary<string, Variable>
        {
            {variable.Name, variable}
        });
    
    public static State Build(string variableName, uint variableValue) => new(
        new Dictionary<string, Variable>
        {
            {variableName, new Variable(variableName, new UnsignedIntModType(variableValue))}
        });
}

public static class VariableBuilder
{
    public static Variable Build(string name, uint value = 1) => new(name, new UnsignedIntModType(value));
}