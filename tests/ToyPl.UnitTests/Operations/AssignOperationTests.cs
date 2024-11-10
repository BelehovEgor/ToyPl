using ToyPl.Application.Models;
using ToyPl.Application.Operations;
using ToyPl.UnitTests.Builders;

namespace ToyPl.UnitTests.Operations;

public class AssignOperationTests
{
    [Fact]
    public void Do_EmptyStates_ShouldReturnEmpty()
    {
        // Arrange
        var states = Array.Empty<State>();
        var assignOperation = new AssignOperation("Name", new UnsignedIntModType(4));

        // Act
        var result = assignOperation.Do(states);
        
        // Assert
        result.Should().BeEmpty();
    }
    
    [Fact]
    public void Do_NotExistsVariableName_ShouldThrow()
    {
        // Arrange
        var states = new [] { StateBuilder.Build(1) };
        var assignOperation = new AssignOperation("Name", new UnsignedIntModType(4));

        // Act
        var act = () => assignOperation.Do(states);
        
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
    
    [Fact]
    public void Do_OneVariableInState_ShouldReturnNewStateWithNewVariable()
    {
        // Arrange
        var name = "Name";
        var oldValue = new UnsignedIntModType(1);
        var newValue = new UnsignedIntModType(2);
        var oldVariable = new Variable(name, oldValue);
        var state = StateBuilder.Build(oldVariable);
        var states = new [] { state };
        var newVariable = oldVariable with { Value = newValue};
        var assignOperation = new AssignOperation(name, newValue);

        // Act
        var result = assignOperation.Do(states);
        
        // Assert
        result.Should().HaveCount(1);
        
        var resultState = result.Single();
        resultState.Variables.Should().HaveCount(1);

        var resultVariable = resultState.Variables.Single().Value;
        resultVariable.Should().Be(newVariable);
    }
    
    [Fact]
    public void Do_FewVariableInState_ShouldReturnNewStateWithNewVariable()
    {
        // Arrange
        var state = StateBuilder.Build(3);
        
        var states = new [] { state };
        var oldVariable = state.Variables.First().Value;
        var newVariable = oldVariable with { Value = new UnsignedIntModType(2) };
        var assignOperation = new AssignOperation(newVariable.Name, newVariable.Value);

        // Act
        var result = assignOperation.Do(states);
        
        // Assert
        result.Should().HaveCount(1);
        
        var resultState = result.Single();
        var expectedStateVariables = new[]
        {
            newVariable, state.Variables.Values.ElementAt(1), state.Variables.Values.ElementAt(2)
        }.ToDictionary(x => x.Name);
        var expectedState = new State(expectedStateVariables);
        resultState.IsEqual(expectedState).Should().BeTrue();
    }
    
    // TODO test for many states
}