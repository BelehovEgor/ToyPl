using ToyPl.Application.Models;

namespace ToyPl.Application.Expressions;

public class MinusOperation() : Operation((left, right) => new UnsignedIntModType(left.Value - right.Value))
{
    public static Operation Create => new MinusOperation();
}