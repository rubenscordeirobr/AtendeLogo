using System.Collections;

public static class DictionaryExtensions
{
    public static TValue GetOrAdd<TKey, TValue>(
        this Dictionary<TKey, TValue> dictionary,
        TKey key,
        Func<TValue> valueFactory)
        where TKey : notnull
    {
        if (dictionary.TryGetValue(key, out var value))
        {
            return value;
        }
        value = valueFactory();
        dictionary.Add(key, value);
        return value;
    }

    public static void AddOrUpdate<TKey, TValue>(
        this Dictionary<TKey, TValue> dictionary,
        TKey key,
        TValue value)
        where TKey : notnull
    {
        if (dictionary.ContainsKey(key))
        {
            dictionary[key] = value;
        }
        else
        {
            dictionary.Add(key, value);
        }
    }

    public static void AddRangeOrUpdate<TKey, TValue>(
        this Dictionary<TKey, TValue> dictionary,
        IEnumerable<TKey> keys,
        IEnumerable<TValue> values)
        where TKey : notnull
    {
        if (keys.Count() != values.Count())
        {
            throw new ArgumentException("Keys and values must have the same length.");
        }

        for (int i = 0; i < keys.Count(); i++)
        {
            dictionary.AddOrUpdate(keys.ElementAt(i), values.ElementAt(i));
        }
    }

    public static TValue GetOrAddThreadSafe<TKey, TValue>(
       this Dictionary<TKey, TValue> dictionary,
       TKey key,
       Func<TValue> valueFactory)
       where TKey : notnull
    {
        lock (((IDictionary)dictionary).SyncRoot)
        {
            return GetOrAdd(dictionary, key, valueFactory);
        }
    }

    public static void AddOrUpdateThreadSafe<TKey, TValue>(
        this Dictionary<TKey, TValue> dictionary,
        TKey key,
        TValue value)
        where TKey : notnull
    {
        lock(((IDictionary)dictionary).SyncRoot)
        {
             AddOrUpdate(dictionary, key, value);
        }
    }

    public static void AddRangeOrUpdateThreadSafe<TKey, TValue>(
        this Dictionary<TKey, TValue> dictionary,
        IEnumerable<TKey> keys,
        IEnumerable<TValue> values)
        where TKey : notnull
    {
        lock (((IDictionary)dictionary).SyncRoot)
        {
            AddRangeOrUpdate(dictionary, keys, values);
        }
    }
}
