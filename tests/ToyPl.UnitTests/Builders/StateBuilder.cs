using ToyPl.Application.Models;

namespace ToyPl.UnitTests.Builders;

public static class StateBuilder
{
    private static uint _value;
    private static uint Next => _value++ % UnsignedIntModType.MaxValue;
    
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
    
    public static State Build(string variableName) => new(
        new Dictionary<string, Variable>
        {
            {variableName, new Variable(variableName, new UnsignedIntModType(Next))}
        });
}