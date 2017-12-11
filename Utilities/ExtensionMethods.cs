using System;

namespace Utilities
{
    public static class ExtensionMethods
    {
        public static T[] Initialize<T>(this T[] array, Func<int, T> valueProvider)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = valueProvider(i);
            }

            return array;
        }

    }
}
