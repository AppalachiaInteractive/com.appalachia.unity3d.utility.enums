#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Internal.Core.Collections.Generic
{
    /// <summary>
    ///     Iterable collection of an enums values.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    public struct EnumValuesCollection<T> : IEnumerable<T>
        where T : Enum
    {
        public EnumValuesCollection(bool x = false)
        {
            _exclusions = new HashSet<T>();
            var exc = _exclusions;
            _values = EnumValueManager.GetAllValues<T>().Where(v => !exc.Contains(v)).ToArray();
        }

        public EnumValuesCollection(params T[] args)
        {
            _exclusions = ToHashSet(args);
            var exc = _exclusions;
            _values = EnumValueManager.GetAllValues<T>().Where(v => !exc.Contains(v)).ToArray();
        }

        private static HashSet<T> ToHashSet(IEnumerable<T> source)
        {
            return new HashSet<T>(source);
        }

        private HashSet<T> _exclusions;

        private T[] _values;

        public T[] Values
        {
            get
            {
                if ((_values == null) || (_values.Length == 0))
                {
                    _exclusions = new HashSet<T>();
                    var exc = _exclusions;
                    _values = EnumValueManager.GetAllValues<T>().Where(v => !exc.Contains(v)).ToArray();
                }

                return _values;
            }
        }

        public int Length => Values.Length;

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>) Values).GetEnumerator(); // <-- 1 non-local call
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
