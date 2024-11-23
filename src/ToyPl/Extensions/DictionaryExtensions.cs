namespace ToyPl.Extensions;

public static class DictionaryExtensions
{
    public static IDictionary<TKey, TValue> Clone<TKey, TValue>(
        this IDictionary<TKey, TValue> original) 
        where TValue : notnull 
        where TKey : notnull
    {
        var ret = new Dictionary<TKey, TValue>(original.Count);
        foreach (var entry in original)
        {
            ret.Add(entry.Key, entry.Value);
        }
        
        return ret;
    }
}