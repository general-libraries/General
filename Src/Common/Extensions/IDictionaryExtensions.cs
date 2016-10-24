using System.Collections.Generic;

namespace Common.Extensions
{
    public static class IDictionaryExtensions
    {
        public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IDictionary<TKey, TValue> dictionaryToAdd)
        {
            if (dictionaryToAdd == null) { return; }

            foreach (var keyValue in dictionaryToAdd)
            {
                dictionary.AddOrUpdate(keyValue.Key, keyValue.Value);
            }
        }

        public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, KeyValuePair<TKey, TValue> keyValue)
        {
            dictionary.AddOrUpdate(keyValue.Key, keyValue.Value);
        }

        public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
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
    }
}
