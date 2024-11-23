namespace ToyPl.Extensions;

public static class CollectionExtensions
{
    public static T? GetRandom<T>(this IReadOnlyCollection<T> collection)
    {
        var i = new Random().Next(collection.Count);

        return collection.Skip(i).FirstOrDefault();
    }
}