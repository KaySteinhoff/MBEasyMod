using System;

namespace MBEasyMod.Extensions
{
    public static class ArrayExtensions
    {
        public static G[] ToArray<T, G>(this T[] arr, Func<T, G> func)
        {
            G[] res = new G[arr.Length];
            for(int i = 0; i < arr.Length; ++i)
                res[i] = func(arr[i]);
            return res;
        }
    }
}