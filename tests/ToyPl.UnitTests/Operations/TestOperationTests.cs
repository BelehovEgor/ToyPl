using ToyPl.Application.Conditions;
using ToyPl.Application.Expressions;
using ToyPl.Application.Models;
using ToyPl.Application.Operations;
using ToyPl.UnitTests.Builders;

namespace ToyPl.UnitTests.Operations;

public class TestOperationTests
{
    [Fact]
    public void Do_PredicateReturnFalse_ShouldReturnEmptyStates()
    {
        // Arrange
        var state = new StateBuilder().WithVariable("a", 1).Build();
        var condition = new Condition(
            [
                "a",
                new UnsignedIntModType(1)
            ], 
            NotEqual.Create);
        var operation = new TestOperation(condition);

        var states = new[] { state };

        // Act
        var newStates = operation.Do(states);

        // Assert
        newStates.Should().BeEmpty();
    }
    
    [Fact]
    public void Do_PredicateReturnTrue_ShouldReturnSameStates()
    {
        // Arrange
        var state = new StateBuilder().WithVariable("a", 1).Build();
        var condition = new Condition(
            [
                "a",
                new UnsignedIntModType(1)
            ], 
            Equal.Create);
        var operation = new TestOperation(condition);

        var states = new[] { state };

        // Act
        var newStates = operation.Do(states);

        // Assert
        newStates.Should().HaveCount(1);
        var newState = newStates.Single();
        newState.IsEqual(state).Should().BeTrue();
    }
    
    [Fact]
    public void Do_PredicateByOddVariableCount_ShouldReturnStatesWithOddVariableCount()
    {
        // Arrange
        var oddState = new StateBuilder().WithVariable("a", 1).Build();
        var notOddState = new StateBuilder().WithVariable("a", 2).Build();
        var condition = new Condition(
            [
                new Expression("a", new UnsignedIntModType(2), ModOperation.Create),
                new UnsignedIntModType(1)
            ], 
            Equal.Create);
        var operation = new TestOperation(condition);

        var states = new[] { oddState, notOddState };

        // Act
        var newStates = operation.Do(states);

        // Assert
        newStates.Should().HaveCount(1);
        var newState = newStates.Single();
        newState.IsEqual(oddState).Should().BeTrue();
    }
}