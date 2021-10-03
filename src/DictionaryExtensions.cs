using System;
using System.Collections.Generic;

namespace Appalachia.Utility.Enums
{
    public static class DictionaryExtensions
    {

        public static void PopulateEnumKeys<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, Func<TKey, TValue> creator)
            where TKey : Enum
        {
            var keys = EnumValueManager.GetAllValues<TKey>();

            for (var i = 0; i < keys.Length; i++)
            {
                var key = keys[i];
                
                if (!dictionary.ContainsKey(key))
                {
                    dictionary.Add(key, creator(key));
                }
            }
        }

        public static void PopulateEnumKeys<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
            where TKey : Enum
            where TValue : new()
        {
            var keys = EnumValueManager.GetAllValues<TKey>();

            for (var i = 0; i < keys.Length; i++)
            {                
                var key = keys[i];

                if (!dictionary.ContainsKey(key))
                {
                    dictionary.Add(key, new TValue());
                }
            }
        }
    }
}
