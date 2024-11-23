namespace ToyPl.Application.Models;

public record Variable(string Name, UnsignedIntModType Value)
{
    public static Variable Create(string name) => new(
        name,
        new UnsignedIntModType((uint) new Random().Next((int) UnsignedIntModType.MaxValue)));
}