using ToyPl.Application.Expressions;
using ToyPl.Application.Models;
using ToyPl.UnitTests.Builders;

namespace ToyPl.UnitTests.Expressions;

public class ExpressionsTests
{
    [Fact]
    public void Calc_Values_ShouldReturnValidAnswer()
    {
        // Arrange
        var state = StateBuilder.Build(2);
        
        var left = new PossibleValue(new UnsignedIntModType(1));
        var right = new PossibleValue(new UnsignedIntModType(2));

        var expr = new Expression(left, right, PlusOperation.Create);

        // Act
        var result = expr.Calc(state);

        // Assert
        result.Value.Should().Be(3);
    }
    
    [Fact]
    public void Calc_Variables_ShouldReturnValidAnswer()
    {
        // Arrange
        var state = new StateBuilder()
            .WithVariable("A", 1)
            .WithVariable("B", 2)
            .Build();
        
        var left = new PossibleValue("A");
        var right = new PossibleValue("B");

        var expr = new Expression(left, right, PlusOperation.Create);

        // Act
        var result = expr.Calc(state);

        // Assert
        result.Value.Should().Be(3);
    }
    
    [Fact]
    public void Calc_ComplexExpression_ShouldReturnValidAnswer()
    {
        // Arrange
        var state = new StateBuilder()
            .WithVariable("A", 1)
            .WithVariable("B", 2)
            .Build();
        
        var plusExpr = new Expression(new PossibleValue("A"), new PossibleValue("B"), PlusOperation.Create);
        var timesExpr = new Expression(
            new PossibleValue(plusExpr), 
            new PossibleValue(new UnsignedIntModType(3)), 
            TimesOperation.Create);
        var minusExpr = new Expression(
            new PossibleValue(timesExpr), 
            new PossibleValue(plusExpr), 
            MinusOperation.Create);

        // Act
        var result = minusExpr.Calc(state);

        // Assert
        result.Value.Should().Be(6);
    }
}