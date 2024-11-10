using ToyPl.Application.Operations;
using ToyPl.UnitTests.Builders;

namespace ToyPl.UnitTests.Operations;

public class CompositionOperationTests
{
    [Fact]
    public void Do_ValidOperations_ShouldReturnCompositionResult()
    {
        // Arrange
        var states = new[]
        {
            StateBuilder.Build(3),
            StateBuilder.Build(3),
            StateBuilder.Build(3)
        };

        var statesAfterFirstOperations = new[]
        {
            StateBuilder.Build(3),
            StateBuilder.Build(3)
        };
        var firstOperationMock = new Mock<IOperation>(MockBehavior.Strict);
        firstOperationMock
            .Setup(x => x.Do(states))
            .Returns(statesAfterFirstOperations);
        
        var statesAfterSecondOperations = new[]
        {
            StateBuilder.Build(3),
            StateBuilder.Build(3),
            StateBuilder.Build(3),
            StateBuilder.Build(3),
            StateBuilder.Build(3),
        };
        var secondOperationMock = new Mock<IOperation>(MockBehavior.Strict);
        secondOperationMock
            .Setup(x => x.Do(statesAfterFirstOperations))
            .Returns(statesAfterSecondOperations);

        var compositionOperation = new CompositionOperation(firstOperationMock.Object, secondOperationMock.Object);

        // Act
        var result = compositionOperation.Do(states);

        // Assert
        result.ShouldDeepEqual(statesAfterSecondOperations);
        
        firstOperationMock.VerifyAll();
        secondOperationMock.VerifyAll();
    }
}