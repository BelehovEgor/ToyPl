using ToyPl.Application.Models;
using ToyPl.Application.Operations;
using ToyPl.UnitTests.Builders;

namespace ToyPl.UnitTests.Operations;

public class ClosureOperationTests
{
    [Fact]
    public void Do_EmptyStates_ShouldReturnEmpty()
    {
        // Arrange
        var states = Array.Empty<State>();

        var bodyMock = new Mock<IOperation>(MockBehavior.Strict);
        var closureOperation = new ClosureOperation(bodyMock.Object);
        // Act
        var result = closureOperation.Do(states);

        // Assert
        result.Should().BeEmpty();
        bodyMock.VerifyNoOtherCalls();
    }
    
    [Fact]
    public void Do_FewValidStates_ShouldReturnAllCombinations()
    {
        // Arrange
        var state = new StateBuilder()
            .WithVariable("A", 1)
            .WithVariable("B", 2)
            .Build();
        var states = new[] { state };

        var bodyMock = new Mock<IOperation>(MockBehavior.Strict);

        var newStates1 = new[]
        {
            new StateBuilder()
                .WithVariable("A", 3)
                .WithVariable("B", 4)
                .Build(),
            new StateBuilder()
                .WithVariable("A", 5)
                .WithVariable("B", 6)
                .Build()
        };
        bodyMock
            .Setup(x => x.Do(states))
            .Returns(newStates1);
        
        var newStates2 = new[]
        {
            new StateBuilder()
                .WithVariable("A", 7)
                .WithVariable("B", 8)
                .Build()
        };
        bodyMock
            .Setup(x => x.Do(newStates1))
            .Returns(newStates2);
        bodyMock
            .Setup(x => x.Do(newStates2))
            .Returns(newStates2);
        
        var closureOperation = new ClosureOperation(bodyMock.Object);
        
        // Act
        var result = closureOperation.Do(states);

        // Assert
        var expectedStates = newStates1.Union(newStates2).ToArray();
        result.Count.Should().Be(expectedStates.Length);
        result.ShouldDeepEqual(expectedStates);
    }
}