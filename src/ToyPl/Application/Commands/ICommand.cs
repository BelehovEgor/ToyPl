using ToyPl.Application.Models;

namespace ToyPl.Application.Commands;

public interface ICommand
{
    int Id { get; }
    ICommand? Next { get; }
    State?[] Execute(State?[] states);
    void FillLines(Dictionary<int, string> lines);
}