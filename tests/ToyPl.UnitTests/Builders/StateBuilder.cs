using ToyPl.Application.Models;

namespace ToyPl.UnitTests.Builders;

public class StateBuilder
{

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
        return new State(_variables.ToDictionary(x => x.Name, x => x.Value));
    }
    
    public static State Build(int variablesCount) => new(
        Enumerable
            .Range(0, variablesCount)
            .Select(x => Variable.Create(x.ToString()))
            .ToDictionary(x => x.Name, x => x.Value));
    
    public static State Build(Variable variable) => new(
        new Dictionary<string, UnsignedIntModType>
        {
            {variable.Name, variable.Value}
        });
    
    public static State Build(string variableName, uint variableValue) => new(
        new Dictionary<string, UnsignedIntModType>
        {
            {variableName, new UnsignedIntModType(variableValue)}
        });
}

public static class VariableBuilder
{
    public static Variable Build(string name, uint value = 1) => new(name, new UnsignedIntModType(value));
}