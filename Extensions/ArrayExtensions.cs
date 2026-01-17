using System;
using NetworkMessages.FromClient;

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

		public static void Swap<T>(this T[] array, int a, int b)
		{
			T tmp = array[a];
			array[a] = array[b];
			array[b] = tmp;
		}

		private static int QsPartition<T>(T[] array, Func<T, T, int> compare, int low, int high)
		{
			T pivot = array[high];
			int i = low - 1;

			for(int j = low; j <= high - 1; ++j)
			{
				if(compare(array[j], pivot) >= 0)
					continue;
				i++;
				array.Swap(i, j);
			}

			array.Swap(i + 1, high);
			return i + 1;
		}

		public static void QuickSort<T>(this T[] array, Func<T, T, int> compare, int low = -1, int high = -1)
		{
			if(low < 0)
			{
				low = 0;
				high = array.Length - 1;
			}

			if(high >= low)
				return;
			
			int pi = QsPartition(array, compare, low, high);
			array.QuickSort(compare, low, pi - 1);
			array.QuickSort(compare, pi + 1, high);
		}
	}
}