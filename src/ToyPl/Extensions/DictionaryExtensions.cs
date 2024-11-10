using ToyPl.Application.Models;

namespace ToyPl.Extensions;

public static class DictionaryExtensions
{
    public static IDictionary<TKey, TValue> DeepClone<TKey, TValue>(
        this IDictionary<TKey, TValue> original) 
        where TValue : Variable 
        where TKey : notnull
    {
        var ret = new Dictionary<TKey, TValue>(original.Count);
        foreach (var entry in original)
        {
            ret.Add(entry.Key, entry.Value with {});
        }
        
        return ret;
    }
}