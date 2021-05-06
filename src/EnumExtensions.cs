using System;
using System.Linq;

namespace Appalachia.Utility.Enums
{
    public static class EnumExtensions
    {
        public static T[] GetValuesAsInstances<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToArray();
        }

        public static T Parse<T>(this string name)
            where T : struct, IConvertible
        {
            try
            {
                return (T) Enum.Parse(typeof(T), name);
            }
            catch
            {
                return default;
            }
        }
    }
}
