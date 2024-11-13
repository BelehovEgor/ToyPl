using ToyPl.Application.Models;
using ToyPl.Translation;
using ToyPl.UnitTests.Builders;

namespace ToyPl.UnitTests.ToyPlExamples;

public class ExamplesTests
{
    [Fact]
    public void Do_Assign_Success()
    {
        // Arrange
        var fileName = "../../../ToyPlExamples/assign-1.tpl";
        var programReader = new ProgramReader();
        var (program, _) = programReader.GetProgram(fileName);

        var state = new StateBuilder()
            .WithVariable("a", 0)
            .Build();

        // Act
        var result = program.Do([state]);

        // Assert
        result.Should().HaveCount(1);
        result.Single().Variables["a"].Value.Should().Be(new UnsignedIntModType(1));
    }
    
    [Theory]
    [InlineData(0, 3)]
    [InlineData(1, 3)]
    [InlineData(4, 9)]
    public void Do_AssignExpression_Success(uint b, uint actualB)
    {
        // Arrange
        var fileName = "../../../ToyPlExamples/assign-expression.tpl";
        var programReader = new ProgramReader();
        var (program, _) = programReader.GetProgram(fileName);

        var state = new StateBuilder()
            .WithVariable("a", 0)
            .WithVariable("b", b)
            .Build();

        // Act
        var result = program.Do([state]);

        // Assert
        result.Should().HaveCount(1);
        var actualResult = result.Single();
        actualResult.Variables["a"].Value.Should().Be(new UnsignedIntModType(2));
        actualResult.Variables["b"].Value.Should().Be(new UnsignedIntModType(actualB));
    }
    
    [Fact]
    public void Do_AssignFew_Success()
    {
        // Arrange
        var fileName = "../../../ToyPlExamples/assign-few.tpl";
        var programReader = new ProgramReader();
        var (program, _) = programReader.GetProgram(fileName);

        var state = new StateBuilder()
            .WithVariable("a", 0)
            .WithVariable("b", 0)
            .WithVariable("c", 0)
            .Build();

        // Act
        var result = program.Do([state]);

        // Assert
        result.Should().HaveCount(1);
        var actualResult = result.Single();
        actualResult.Variables["a"].Value.Should().Be(new UnsignedIntModType(1));
        actualResult.Variables["b"].Value.Should().Be(new UnsignedIntModType(2));
        actualResult.Variables["c"].Value.Should().Be(new UnsignedIntModType(3));
    }
    
    [Theory]
    [InlineData(0, new[] { 0u })]
    [InlineData(1, new[] { 0u })]
    [InlineData(5, new[] { 2u, 1u, 0u })]
    [InlineData(9, new[] { 4u, 2u, 1u, 0u })]
    public void Do_Closure_Success(uint a, uint[] actualA)
    {
        // Arrange
        var fileName = "../../../ToyPlExamples/closure.tpl";
        var programReader = new ProgramReader();
        var (program, _) = programReader.GetProgram(fileName);

        var state = new StateBuilder()
            .WithVariable("a", a)
            .Build();

        // Act
        var result = program.Do([state]);

        // Assert
        result.Should().HaveCount(actualA.Length);
        foreach (var (actualState, expectedValue) in result.Zip(actualA))
        {
            actualState.Variables["a"].Value.Should().Be(new UnsignedIntModType(expectedValue));
        }
    }
    
    [Theory]
    [InlineData(0, new[] { 0u })]
    [InlineData(1, new[] { 0u })]
    [InlineData(5, new[] { 1u, 2u })]
    [InlineData(9, new[] { 3u, 4u })]
    public void Do_Union_Success(uint a, uint[] actualA)
    {
        // Arrange
        var fileName = "../../../ToyPlExamples/union.tpl";
        var programReader = new ProgramReader();
        var (program, _) = programReader.GetProgram(fileName);

        var state = new StateBuilder()
            .WithVariable("a", a)
            .Build();

        // Act
        var result = program.Do([state]);

        // Assert
        result.Should().HaveCount(actualA.Length);
        foreach (var (actualState, expectedValue) in result.Zip(actualA))
        {
            actualState.Variables["a"].Value.Should().Be(new UnsignedIntModType(expectedValue));
        }
    }
}