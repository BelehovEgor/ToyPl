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
        result.Should().HaveCount(3);

        var expectedStates = new[]
        {
            StateBuilder.Build("A", 1),
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

        bool Predicate(State state) => state.Variables["A"].Value.Value % 2 == 1;
        var ifOperation = new UnionOperation(
            new CompositionOperation(
                new TestOperation(Predicate),
                assignOperation1),
            new CompositionOperation(
                new TestOperation(x => !Predicate(x)),
                assignOperation2));

        // Act
        var result = ifOperation.Do([state]);

        // Assert
        result.Should().HaveCount(1);
        var resultState = result.Single();

        resultState.ShouldDeepEqual(StateBuilder.Build(newVariable1));
    }
    
    [Fact]
    public void Do_While_ShouldReturnThen()
    {
        // Arrange
        var state = new StateBuilder()
            .WithVariable("A", 1)
            .Build();

        var newVariable1 = VariableBuilder.Build("A", 2);

        var assignOperation1 = new AssignOperation(newVariable1);

        bool Predicate(State state) => state.Variables["A"].Value.Value % 2 == 1;
        var whileOperation = new ClosureOperation(
            new CompositionOperation(
                new CompositionOperation(new TestOperation(Predicate), assignOperation1),
                new TestOperation(x => !Predicate(x))));

        // Act
        var result = whileOperation.Do([state]);

        // Assert
        
        // Not true while cause while -> Ui where i > 0, but closure Ui where i >= 0
        result.Should().HaveCount(2);
        result.ShouldDeepEqual(
            new[]
            {
                state,
                StateBuilder.Build(newVariable1)
            });
    }
}