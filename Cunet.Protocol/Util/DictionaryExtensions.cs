namespace Cunet.Protocol.Util;

internal static class DictionaryExtensions {
    /// <summary>
    ///     Tries to retrieve a value from a dictionary and calculates and sets the value on demand if the key is not present
    ///     in the dictionary.
    /// </summary>
    /// <param name="dictionary">The dictionary to retrieve the value from.</param>
    /// <param name="key">The key to retrieve.</param>
    /// <param name="creator">The function creating the value if none could be found.</param>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <returns>The found or created type.</returns>
    internal static TValue GetOrSet<TKey, TValue>(
        this IDictionary<TKey, TValue> dictionary,
        TKey key,
        Func<TKey, TValue> creator
    ) {
        bool existing = dictionary.TryGetValue(key, out TValue? value);
        if (existing) {
            return value!;
        }

        value = creator(key);
        dictionary[key] = value;
        return value;
    }
}
