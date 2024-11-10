using ToyPl.Application.Models;

namespace ToyPl.Application.Operations;

public class ClosureOperation(IOperation body) : IOperation
{
    public IReadOnlyCollection<State> Do(IReadOnlyCollection<State> states)
    {
        if (states.Count == 0) return [];
        
        var rk = body;
        var r0 = body.Do(states);
        
        var unionStates = new HashSet<State>(r0, new StateComparer());
        
        int prevCount;
        do
        {
            prevCount = unionStates.Count;
            
            rk = new CompositionOperation(body, rk);
            var currentState = rk.Do(states);
            
            foreach (var state in currentState)
            {
                unionStates.Add(state);
            }
        } while (prevCount != unionStates.Count);

        return unionStates;
    }
}