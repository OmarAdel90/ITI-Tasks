using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day5_6
{
    internal class BetterBubbleSort<T>
    {
        public static void Sort(ref T[] arr)
        {
            bool isSwaped = false;
            T temp;
            for(int i = 0; i < arr.Length; i++)
            {
                for(int j = 0; j < arr.Length -1; j++)
                {
                    if (Comparer<T>.Default.Compare(arr[j], arr[j + 1]) > 0)
                    {
                        temp = arr[j];
                        arr[j] = arr[j + 1];
                        arr[j + 1] = temp;
                        isSwaped = true;
                    }
                    
                }
                if (!isSwaped)
                    break;
            }
        }
    }
}
