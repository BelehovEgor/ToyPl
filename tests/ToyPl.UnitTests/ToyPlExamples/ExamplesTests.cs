using ToyPl.Application.Models;
using ToyPl.Extensions;
using ToyPl.Translation;
using ToyPl.UnitTests.Builders;
using Xunit.Abstractions;

namespace ToyPl.UnitTests.ToyPlExamples;

public class ExamplesTests
{
    private readonly ITestOutputHelper _output;
    
    public ExamplesTests(ITestOutputHelper output)
    {
        Constants.N = 5;
        _output = output;
    }
    
    [Fact]
    public void Do_Assign_Success()
    {
        // Arrange
        var fileName = "../../../ToyPlExamples/assign-1.tpl";
        var programReader = new ProgramReader();
        var (program, _) = programReader.GetProgram(fileName);
        var command = programReader.Translate(program);

        var state = new StateBuilder()
            .WithVariable("a", 0)
            .Build();

        // Act
        var result = command.Execute(state).GetRandom();

        // Assert
        result.Should().NotBeNull();
        result!.Variables["a"].Should().Be(new UnsignedIntModType(1));
        _output.WriteLine($"state: {result.ToBeautyString()}");
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
        var command = programReader.Translate(program);

        var state = new StateBuilder()
            .WithVariable("a", 0)
            .WithVariable("b", b)
            .Build();

        // Act
        var result = command.Execute(state).GetRandom();

        // Assert
        result.Should().NotBeNull();
        result!.Variables["a"].Should().Be(new UnsignedIntModType(2));
        result.Variables["b"].Should().Be(new UnsignedIntModType(actualB));
        _output.WriteLine($"state: {result.ToBeautyString()}");
    }
    
    [Fact]
    public void Do_AssignFew_Success()
    {
        // Arrange
        var fileName = "../../../ToyPlExamples/assign-few.tpl";
        var programReader = new ProgramReader();
        var (program, _) = programReader.GetProgram(fileName);
        var command = programReader.Translate(program);

        var state = new StateBuilder()
            .WithVariable("a", 0)
            .WithVariable("b", 0)
            .WithVariable("c", 0)
            .Build();

        // Act
        var result = command.Execute(state).GetRandom();

        // Assert
        result.Should().NotBeNull();
        result!.Variables["a"].Should().Be(new UnsignedIntModType(1));
        result.Variables["b"].Should().Be(new UnsignedIntModType(2));
        result.Variables["c"].Should().Be(new UnsignedIntModType(3));
        _output.WriteLine($"state: {result.ToBeautyString()}");
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
        var command = programReader.Translate(program);

        var state = new StateBuilder()
            .WithVariable("a", a)
            .Build();

        // Act
        var result = command.Execute(state).GetRandom();

        // Assert
        result.Should().NotBeNull();
        actualA.Should().Contain(result!.Variables["a"].Value);
        _output.WriteLine($"state: {result.ToBeautyString()}");
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
        var command = programReader.Translate(program);

        var state = new StateBuilder()
            .WithVariable("a", a)
            .Build();

        // Act
        var result = command.Execute(state).GetRandom();

        // Assert
        result.Should().NotBeNull();
        actualA.Should().Contain(result!.Variables["a"].Value);
        _output.WriteLine($"state: {result.ToBeautyString()}");
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
        var command = programReader.Translate(program);

        var state = new StateBuilder()
            .WithVariable("a", a)
            .Build();

        // Act
        var result = command.Execute(state).GetRandom();

        // Assert
        result.Should().NotBeNull();
        actualA.Should().Contain(result!.Variables["a"].Value);
        _output.WriteLine($"state: {result.ToBeautyString()}");
    }
    
    [Fact]
    public void Do_to10_Success()
    {
        // Arrange
        var fileName = "../../../ToyPlExamples/to-10.tpl";
        var programReader = new ProgramReader();
        var (program, _) = programReader.GetProgram(fileName);
        var command = programReader.Translate(program);

        var state = new StateBuilder()
            .WithVariable("a", 0)
            .Build();

        // Act
        var result = command.Execute(state).GetRandom();

        // Assert
        if (result is null) return;
        
        result.Variables["a"].Should().Be(new UnsignedIntModType(10));
        _output.WriteLine($"state: {result.ToBeautyString()}");
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
        var command = programReader.Translate(program);

        var state = new StateBuilder()
            .WithVariable("a", 0)
            .WithVariable("b", 0)
            .WithVariable("n", n)
            .Build();

        // Act
        var result = command.Execute(state).GetRandom();

        // Assert
        result.Should().NotBeNull();
        actualA.Should().Contain(result!.Variables["a"].Value);
        _output.WriteLine($"state: {result.ToBeautyString()}");
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
        var command = programReader.Translate(program);

        var state = new StateBuilder()
            .WithVariable("a", a)
            .WithVariable("b", b)
            .Build();

        // Act
        var result = command.Execute(state).GetRandom();

        // Assert
        result.Should().NotBeNull();
        actualA.Should().Contain(result!.Variables["a"].Value);
        _output.WriteLine($"state: {result.ToBeautyString()}");
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
        var command = programReader.Translate(program);

        var state = new StateBuilder()
            .WithVariable("a", a)
            .WithVariable("b", b)
            .WithVariable("z", 0)
            .Build();

        // Act
        var result = command.Execute(state).GetRandom();

        // Assert
        result.Should().NotBeNull();
        actualZ.Should().Contain(result!.Variables["z"].Value);
        _output.WriteLine($"state: {result.ToBeautyString()}");
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
        var command = programReader.Translate(program);

        var state = new StateBuilder()
            .WithVariable("a", a)
            .WithVariable("b", 0)
            .WithVariable("c", 0)
            .Build();

        // Act
        var result = command.Execute(state).GetRandom();

        // Assert
        result.Should().NotBeNull();
        actualC.Should().Contain(result!.Variables["c"].Value);
        _output.WriteLine($"state: {result.ToBeautyString()}");
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
        var command = programReader.Translate(program);

        var state = new StateBuilder()
            .WithVariable("a", a)
            .WithVariable("b", b)
            .WithVariable("c", c)
            .WithVariable("s", 0)
            .Build();

        // Act
        var result = command.Execute(state).GetRandom();

        // Assert
        result.Should().NotBeNull();
        actualS.Should().Contain(result!.Variables["s"].Value);
        _output.WriteLine($"state: {result.ToBeautyString()}");
    }

    
    [Theory]
    [InlineData(1, 4)]   // 0ms
    [InlineData(2, 12)]   // 1ms
    [InlineData(3, 32)]  // 1ms
    //[InlineData(4, 80)]  // 112ms
    //[InlineData(5, 192)] // 340ms
    //[InlineData(6, 192)] // 340ms
    //[InlineData(7, 1024)] // 26s 391ms
    //[InlineData(8, 2304)]   // 4m 34s 
    public void Do_Boom_Success(int n, int stateCount)
    {
        // Arrange
        Constants.N = n;
        
        var fileName = "../../../ToyPlExamples/boom.tpl";
        var programReader = new ProgramReader();
        var (program, _) = programReader.GetProgram(fileName);
        var command = programReader.Translate(program);
        
        var state = new StateBuilder()
            .WithVariable("a", 1)
            .WithVariable("b", 1)
            .WithVariable("c", 1)
            .Build();

        // Act
        _ = command.Execute(state);
    }
}