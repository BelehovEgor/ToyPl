namespace ToyPl.Application.Models;

public record UnsignedIntModType
{
    public static uint MaxValue => (uint) Math.Pow(2, Constants.N) - 1; 
    private static uint MinValue => 0;
    public uint Value { get; }

    public UnsignedIntModType(uint value)
    {
        Value = value % MaxValue;
    }
}