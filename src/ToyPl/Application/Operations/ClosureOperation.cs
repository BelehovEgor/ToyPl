using ToyPl.Application.Models;

namespace ToyPl.Application.Operations;

public class ClosureOperation(IOperation body) : IOperation
{
    public IReadOnlyCollection<State> Do(IReadOnlyCollection<State> states)
    {
        if (states.Count == 0) return [];
        
        var lastDoStates = states;
        var newStates = new HashSet<State>(states, new StateComparer());
        var prevCount = -1;
        while (prevCount != newStates.Count)
        {
            prevCount = newStates.Count;
            lastDoStates = body.Do(lastDoStates);

            foreach (var state in lastDoStates)
            {
                newStates.Add(state);
            }
        }

        return newStates;
    }
}