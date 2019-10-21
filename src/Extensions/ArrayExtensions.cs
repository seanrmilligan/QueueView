using System;

namespace Extensions
{
    public static class ArrayExtensions
    {
        public static T[] SubArray<T>(this T[] array, int index, int length)
        {
            T[] subArray = new T[length];
            Array.Copy(array, index, subArray, 0, length);
            return subArray;
        }

        public static T[] Concat<T>(this T[] array, T[] tail)
        {
            T[] concatArray = new T[array.Length + tail.Length];
            Array.Copy(array, 0, concatArray, 0, array.Length);
            Array.Copy(tail, 0, concatArray, array.Length, tail.Length);
            return concatArray;
        }
    }
}
