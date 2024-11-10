using ToyPl.Application.Operations;
using ToyPl.UnitTests.Builders;

namespace ToyPl.UnitTests.Operations;

public class UnionOperationTests
{
    [Fact]
    public void Do_ValidOperations_ShouldReturnAllStates()
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
            statesAfterFirstOperations[0],
            StateBuilder.Build(3),
            StateBuilder.Build(3),
            StateBuilder.Build(3),
        };
        var secondOperationMock = new Mock<IOperation>(MockBehavior.Strict);
        secondOperationMock
            .Setup(x => x.Do(states))
            .Returns(statesAfterSecondOperations);

        var unionOperation = new UnionOperation(firstOperationMock.Object, secondOperationMock.Object);

        // Act
        var result = unionOperation.Do(states);

        // Assert
        var expected = new[]
            {
                statesAfterFirstOperations,
                statesAfterSecondOperations[1..]
            }
            .SelectMany(x => x)
            .ToArray();
        
        result.ShouldDeepEqual(expected);
        
        firstOperationMock.VerifyAll();
        secondOperationMock.VerifyAll();
    }
}