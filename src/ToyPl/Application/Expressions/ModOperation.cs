using ToyPl.Application.Models;

namespace ToyPl.Application.Expressions;

public class ModOperation() : Operation((left, right) => new UnsignedIntModType(left.Value % right.Value))
{
    public static Operation Create => new ModOperation();
}