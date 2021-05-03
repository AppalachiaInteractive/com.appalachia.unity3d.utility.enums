#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

#endregion

namespace Internal.Core.Collections.Generic
{
    /// <summary>
    ///     Helper for accessing the values of an enum.
    /// </summary>
    public static class EnumValueManager
    {
        private static Dictionary<string, object> _lookupWithoutObsolete;
        private static Dictionary<string, object> _lookupWithObsolete;
        private static Dictionary<string, object[]> _boxedLookupWithoutObsolete;
        private static Dictionary<string, object[]> _boxedLookupWithObsolete;

        // ReSharper disable once UnusedParameter.Global
        /// <summary>
        ///     Gets all values of the enum that matches the provided <see cref="value" />.
        /// </summary>
        /// <param name="value">A value in the enum.</param>
        /// <param name="includeObsolete">Should enum entries marked <see cref="ObsoleteAttribute" /> be included?</param>
        /// <typeparam name="T">The enum type - likely inferred.</typeparam>
        /// <returns>An array of the enum values.</returns>
        public static T[] GetAllValues<T>(this T value, bool includeObsolete = false)
            where T : Enum
        {
            return GetAllValues<T>(includeObsolete);
        }

        /// <summary>
        ///     Gets all values of the enum that matches the provided <typeparamref name="T" />.
        /// </summary>
        /// <param name="includeObsolete">Should enum entries marked <see cref="ObsoleteAttribute" /> be included?</param>
        /// <typeparam name="T">The enum type.</typeparam>
        /// <returns>An array of the enum values.</returns>
        public static T[] GetAllValues<T>(bool includeObsolete = false)
            where T : Enum
        {
            if (_lookupWithoutObsolete == null)
            {
                _lookupWithoutObsolete = new Dictionary<string, object>();
            }

            if (_lookupWithObsolete == null)
            {
                _lookupWithObsolete = new Dictionary<string, object>();
            }

            var type = typeof(T);
            var fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Static);

            var typeName = type.FullName;

            Debug.Assert(typeName != null, nameof(typeName) + " != null");

            if (includeObsolete && _lookupWithObsolete.ContainsKey(typeName))
            {
                return (T[]) _lookupWithObsolete[typeName];
            }

            if (!includeObsolete && _lookupWithoutObsolete.ContainsKey(typeName))
            {
                return (T[]) _lookupWithoutObsolete[typeName];
            }

            var allValues = Enum.GetValues(type).Cast<T>().ToArray();

            _lookupWithObsolete.Add(typeName, allValues);

            var values = allValues.Where(
                                       t =>
                                       {
                                           var field = fieldInfos.First(
                                               e => (e.DeclaringType == type) && (e.Name == t.ToString())
                                           );
                                           var attributes = field.GetCustomAttributes(typeof(ObsoleteAttribute), false);

                                           return attributes.Length == 0;
                                       }
                                   )
                                  .ToArray();

            _lookupWithoutObsolete.Add(typeName, values);

            return includeObsolete ? allValues : values;
        }

        /// <summary>
        ///     Gets all values of the enum that matches the provided <paramref name="type" />.
        /// </summary>
        /// <param name="type">The enum type.</param>
        /// <param name="includeObsolete">Should enum entries marked <see cref="ObsoleteAttribute" /> be included?</param>
        /// <returns>An array of the enum values.</returns>
        public static object[] GetAllValues(Type type, bool includeObsolete = false)
        {
            if (_boxedLookupWithoutObsolete == null)
            {
                _boxedLookupWithoutObsolete = new Dictionary<string, object[]>();
            }

            if (_boxedLookupWithObsolete == null)
            {
                _boxedLookupWithObsolete = new Dictionary<string, object[]>();
            }

            var fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Static);

            var typeName = type.FullName;

            Debug.Assert(typeName != null, nameof(typeName) + " != null");

            if (includeObsolete && _boxedLookupWithObsolete.ContainsKey(typeName))
            {
                return _boxedLookupWithObsolete[typeName];
            }

            if (!includeObsolete && _boxedLookupWithoutObsolete.ContainsKey(typeName))
            {
                return _boxedLookupWithoutObsolete[typeName];
            }

            var allValues = Enum.GetValues(type).Cast<object>().ToArray();

            _boxedLookupWithObsolete.Add(typeName, allValues);

            var values = allValues.Where(
                                       t =>
                                       {
                                           var field = fieldInfos.First(
                                               e => (e.DeclaringType == type) && (e.Name == t.ToString())
                                           );
                                           var atts = field.GetCustomAttributes(typeof(ObsoleteAttribute), false);

                                           return atts.Length == 0;
                                       }
                                   )
                                  .ToArray();

            _boxedLookupWithoutObsolete.Add(typeName, values);

            return includeObsolete ? allValues : values;
        }
    }
}
