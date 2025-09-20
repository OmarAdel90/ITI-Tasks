using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day5_6
{
    internal class Helper
    {
        public static void Reverse(ArrayList arrayList)
        {
            Stack<object> temp = new Stack<object>();
            int size = arrayList.Count;
            foreach(object element in arrayList)
            {
                temp.Push(element);
            }
            arrayList.Clear();
            for(int i = 0;i < size; i++) { arrayList.Add(temp.Pop()); }
        }
        public static List<int> ReturnEven(List<int> ints)
        {
            List<int> EvenList = new List<int>();
            foreach(int num in ints)
            {
                if (num % 2 == 0)
                    EvenList.Add(num);
            }
            return EvenList;
        }
        public static int ReturnUnique(string InputString)
        {
            Dictionary<string, int> InputDict = new Dictionary<string, int>();
            foreach(char letter in InputString)
            {
                if (InputDict.ContainsKey(letter.ToString())){
                    InputDict[letter.ToString()]++;
                }
                else InputDict.Add(letter.ToString(), 1);
            }
            var minEntry = InputDict.OrderBy(kv => kv.Value).First();
            if(minEntry.Value == 1)
            {
                for (int i = 0; i < InputString.Length; i++)
                {
                    if (InputString[i].ToString() == minEntry.Key) return i;
                }
            }
            return -1;
        }
    }
}
