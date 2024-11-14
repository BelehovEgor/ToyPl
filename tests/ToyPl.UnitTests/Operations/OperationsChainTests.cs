using ToyPl.Application.Conditions;
using ToyPl.Application.Expressions;
using ToyPl.Application.Models;
using ToyPl.Application.Operations;
using ToyPl.UnitTests.Builders;

namespace ToyPl.UnitTests.Operations;

public class OperationsChainTests
{
    [Fact]
    public void Do_TwoAssigns_ShouldReturnChangedIncomingState()
    {
        // Arrange
        var state = StateBuilder.Build(3);

        var newVariable1 = state.Variables["1"] with
        {
            Value = new UnsignedIntModType(state.Variables["1"].Value.Value + 1)
        };
        var newVariable2 = state.Variables["2"] with
        {
            Value = new UnsignedIntModType(state.Variables["2"].Value.Value + 1)
        };

        var assignOperation1 = new AssignOperation(newVariable1);
        var assignOperation2 = new AssignOperation(newVariable2);

        var compositionOperation = new CompositionOperation(assignOperation1, assignOperation2);

        // Act
        var result = compositionOperation.Do([state]);

        // Assert
        result.Should().HaveCount(1);
        var resultState = result.Single();

        var expectedState = new StateBuilder()
            .WithVariable(state.Variables["0"])
            .WithVariable(newVariable1)
            .WithVariable(newVariable2)
            .Build();
        resultState.IsEqual(expectedState).Should().BeTrue();
    }
    
    [Fact]
    public void Do_FewAssigns_ShouldReturnChangedIncomingState()
    {
        // Arrange
        var state = new StateBuilder()
            .WithVariable("A", 1)
            .WithVariable("B", 1)
            .Build();

        var newVariable1 = VariableBuilder.Build("A", 2);
        var newVariable2 = VariableBuilder.Build("A", 3);
        var newVariable3 = VariableBuilder.Build("B", 2);

        var assignOperation1 = new AssignOperation(newVariable1);
        var assignOperation2 = new AssignOperation(newVariable2);
        var assignOperation3 = new AssignOperation(newVariable3);

        var compositionOperation1 = new CompositionOperation(assignOperation1, assignOperation2);
        var compositionOperation2 = new CompositionOperation(compositionOperation1, assignOperation3);

        // Act
        var result = compositionOperation2.Do([state]);

        // Assert
        result.Should().HaveCount(1);
        var resultState = result.Single();

        var expectedState = new StateBuilder()
            .WithVariable("A", 3)
            .WithVariable("B", 2)
            .Build();
        resultState.IsEqual(expectedState).Should().BeTrue();
    }
    
    [Fact]
    public void Do_AssignsNonDeterminism_ShouldReturnAllPossibleValues()
    {
        // Arrange
        var state = new StateBuilder()
            .WithVariable("A", 1)
            .Build();

        var newVariable1 = VariableBuilder.Build("A", 2);
        var newVariable2 = VariableBuilder.Build("A", 3);
        var newVariable3 = VariableBuilder.Build("A", 4);

        var assignOperation1 = new AssignOperation(newVariable1);
        var assignOperation2 = new AssignOperation(newVariable2);
        var assignOperation3 = new AssignOperation(newVariable3);

        var unionOperation1 = new UnionOperation(assignOperation1, assignOperation2);
        var unionOperation2 = new UnionOperation(unionOperation1, assignOperation3);

        // Act
        var result = unionOperation2.Do([state]);

        // Assert
        result.Should().HaveCount(3);

        var expectedStates = new[]
        {
            StateBuilder.Build("A", 2),
            StateBuilder.Build("A", 3),
            StateBuilder.Build("A", 4)
        };
        result.ShouldDeepEqual(expectedStates);
    }
    
    [Fact]
    public void Do_FewAssignsWithNonDeterminism_ShouldReturnAllPossibleValues()
    {
        // Arrange
        var state = new StateBuilder()
            .WithVariable("A", 1)
            .Build();

        var newVariable1 = VariableBuilder.Build("A", 2);
        var newVariable2 = VariableBuilder.Build("A", 3);
        var newVariable3 = VariableBuilder.Build("A", 4);

        var assignOperation1 = new AssignOperation(newVariable1);
        var assignOperation2 = new AssignOperation(newVariable2);
        var assignOperation3 = new AssignOperation(newVariable3);

        var unionOperation = new UnionOperation(
            assignOperation1, 
            new CompositionOperation(assignOperation2, assignOperation3));

        // Act
        var result = unionOperation.Do([state]);

        // Assert
        result.Should().HaveCount(2);

        var expectedStates = new[]
        {
            StateBuilder.Build("A", 2),
            StateBuilder.Build("A", 4)
        };
        result.ShouldDeepEqual(expectedStates);
    }
    
    [Fact]
    public void Do_SimpleClosure_ShouldReturnAllPossibleValues()
    {
        // Arrange
        var state = new StateBuilder()
            .WithVariable("A", 1)
            .Build();

        var newVariable1 = VariableBuilder.Build("A", 2);
        var newVariable2 = VariableBuilder.Build("A", 3);

        var assignOperation1 = new AssignOperation(newVariable1);
        var assignOperation2 = new AssignOperation(newVariable2);

        var unionOperation = new UnionOperation(assignOperation1, assignOperation2);

        var closureOperation = new ClosureOperation(unionOperation);

        // Act
        var result = closureOperation.Do([state]);

        // Assert
        result.Should().HaveCount(2);

        var expectedStates = new[]
        {
            StateBuilder.Build("A", 2),
            StateBuilder.Build("A", 3)
        };
        result.ShouldDeepEqual(expectedStates);
    }

    [Fact]
    public void Do_If_True_ShouldReturnThen()
    {
        // Arrange
        var state = new StateBuilder()
            .WithVariable("A", 1)
            .Build();

        var newVariable1 = VariableBuilder.Build("A", 2);
        var newVariable2 = VariableBuilder.Build("A", 3);

        var assignOperation1 = new AssignOperation(newVariable1);
        var assignOperation2 = new AssignOperation(newVariable2);

        var condition = new Condition(
            [
                new Expression("A", new UnsignedIntModType(2), ModOperation.Create),
                new UnsignedIntModType(1)
            ], 
            Equal.Create);
        var notCondition = new Condition(
            [
                new Expression("A", new UnsignedIntModType(2), ModOperation.Create),
                new UnsignedIntModType(1)
            ], 
            NotEqual.Create);
        
        var ifOperation = new UnionOperation(
            new CompositionOperation(
                new TestOperation(condition),
                assignOperation1),
            new CompositionOperation(
                new TestOperation(notCondition),
                assignOperation2));

        // Act
        var result = ifOperation.Do([state]);

        // Assert
        result.Should().HaveCount(1);
        var resultState = result.Single();

        resultState.ShouldDeepEqual(StateBuilder.Build(newVariable1));
    }

    [Fact]
    public void Do_Closure_ReturnStates()
    {
        // Arrange
        var state = new StateBuilder()
            .WithVariable("A", 0)
            .Build();

        var left = new AssignOperation("A", new UnsignedIntModType(1));
        var right = new AssignOperation("A", new Expression("A", new UnsignedIntModType(2), PlusOperation.Create));
        var union = new UnionOperation(left, right);
        var closure = new ClosureOperation(union);

        // Act
        var result = closure.Do([state]);

        // Assert
        result.Should().HaveCount(8); // 0..7
    }
}