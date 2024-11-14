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
    
    [Theory]
    [InlineData(5, new[] { 1u })]
    [InlineData(6, new[] { 0u })]
    public void Do_Odd_Success(uint a, uint[] actualA)
    {
        // Arrange
        var fileName = "../../../ToyPlExamples/odd.tpl";
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
    
    [Fact]
    public void Do_to10_Success()
    {
        // Arrange
        var fileName = "../../../ToyPlExamples/to-10.tpl";
        var programReader = new ProgramReader();
        var (program, _) = programReader.GetProgram(fileName);

        var state = new StateBuilder()
            .WithVariable("a", 0)
            .Build();

        // Act
        var result = program.Do([state]);

        // Assert
        result.Should().HaveCount(1);
        var actualResult = result.Single();
        actualResult.Variables["a"].Value.Should().Be(new UnsignedIntModType(10));
    }
    
    [Theory]
    [InlineData(0, new[] { 0u })]
    [InlineData(1, new[] { 1u })]
    [InlineData(2, new[] { 1u })]
    [InlineData(3, new[] { 2u })]
    [InlineData(4, new[] { 3u })]
    [InlineData(5, new[] { 5u })]
    public void Do_Fibbonachi_Success(uint n, uint[] actualA)
    {
        // Arrange
        var fileName = "../../../ToyPlExamples/fibbonachi.tpl";
        var programReader = new ProgramReader();
        var (program, _) = programReader.GetProgram(fileName);

        var state = new StateBuilder()
            .WithVariable("a", 0)
            .WithVariable("b", 0)
            .WithVariable("n", n)
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
    [InlineData(1, 1, new[] { 1u })]
    [InlineData(2, 1, new[] { 1u })]
    [InlineData(1, 2, new[] { 1u })]
    [InlineData(2, 2, new[] { 2u })]
    [InlineData(6, 3, new[] { 3u })]
    [InlineData(9, 3, new[] { 3u })]
    [InlineData(21, 15, new[] { 3u })]
    public void Do_Euclidean_Success(uint a, uint b, uint[] actualA)
    {
        // Arrange
        var fileName = "../../../ToyPlExamples/euclidean.tpl";
        var programReader = new ProgramReader();
        var (program, _) = programReader.GetProgram(fileName);

        var state = new StateBuilder()
            .WithVariable("a", a)
            .WithVariable("b", b)
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
    [InlineData(1, 1, new[] { 1u })]
    [InlineData(2, 1, new[] { 2u })]
    [InlineData(1, 2, new[] { 2u })]
    [InlineData(2, 2, new[] { 4u })]
    [InlineData(2, 3, new[] { 6u })]
    [InlineData(4, 3, new[] { 12u })]
    public void Do_FastTimes_Success(uint a, uint b, uint[] actualZ)
    {
        // Arrange
        var fileName = "../../../ToyPlExamples/fast-times.tpl";
        var programReader = new ProgramReader();
        var (program, _) = programReader.GetProgram(fileName);

        var state = new StateBuilder()
            .WithVariable("a", a)
            .WithVariable("b", b)
            .WithVariable("z", 0)
            .Build();

        // Act
        var result = program.Do([state]);

        // Assert
        result.Should().HaveCount(actualZ.Length);
        foreach (var (actualState, expectedValue) in result.Zip(actualZ))
        {
            actualState.Variables["z"].Value.Should().Be(new UnsignedIntModType(expectedValue));
        }
    }
    
    [Theory]
    [InlineData(1, new[] { 1u })]
    [InlineData(2, new[] { 1u, 2u })]
    [InlineData(3, new[] { 1u, 3u })]
    [InlineData(4, new[] { 1u, 2u, 4u })]
    [InlineData(6, new[] { 1u, 2u, 3u, 6u })]
    public void Do_Mults_Success(uint a, uint[] actualC)
    {
        // Arrange
        var fileName = "../../../ToyPlExamples/mults.tpl";
        var programReader = new ProgramReader();
        var (program, _) = programReader.GetProgram(fileName);

        var state = new StateBuilder()
            .WithVariable("a", a)
            .WithVariable("b", 0)
            .WithVariable("c", 0)
            .Build();

        // Act
        var result = program.Do([state]);

        // Assert
        result.Should().HaveCount(actualC.Length);
        foreach (var (actualState, expectedValue) in result.Zip(actualC))
        {
            actualState.Variables["c"].Value.Should().Be(new UnsignedIntModType(expectedValue));
        }
    }
    
    [Theory]
    [InlineData(1, 1, 1, new[] { 1u })]
    [InlineData(2, 1, 3, new[] { 6u })]
    [InlineData(2, 3, 3, new[] { 18u })]
    public void Do_Volume_Success(uint a, uint b, uint c, uint[] actualS)
    {
        // Arrange
        var fileName = "../../../ToyPlExamples/volume.tpl";
        var programReader = new ProgramReader();
        var (program, _) = programReader.GetProgram(fileName);

        var state = new StateBuilder()
            .WithVariable("a", a)
            .WithVariable("b", b)
            .WithVariable("c", c)
            .WithVariable("s", 0)
            .Build();

        // Act
        var result = program.Do([state]);

        // Assert
        result.Should().HaveCount(actualS.Length);
        foreach (var (actualState, expectedValue) in result.Zip(actualS))
        {
            actualState.Variables["s"].Value.Should().Be(new UnsignedIntModType(expectedValue));
        }
    }
}