using ToyPl.Application.Models;

namespace ToyPl.Application.Expressions;

public abstract class Operation(Func<UnsignedIntModType, UnsignedIntModType, UnsignedIntModType> func)
{
    public UnsignedIntModType Invoke(UnsignedIntModType left, UnsignedIntModType right) => func(left, right);
}