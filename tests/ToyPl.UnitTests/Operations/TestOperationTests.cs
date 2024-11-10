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
        bool Predicate(State _) => false;
        var operation = new TestOperation(Predicate);

        var state = StateBuilder.Build(2);
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
        bool Predicate(State _) => true;
        var operation = new TestOperation(Predicate);

        var state = StateBuilder.Build(2);
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
        bool Predicate(State state) => state.Variables.Count % 2 == 1;
        var operation = new TestOperation(Predicate);

        var oddState = StateBuilder.Build(1);
        var notOddState = StateBuilder.Build(2);

        var states = new[] { oddState, notOddState };

        // Act
        var newStates = operation.Do(states);

        // Assert
        newStates.Should().HaveCount(1);
        var newState = newStates.Single();
        newState.IsEqual(oddState).Should().BeTrue();
    }
}