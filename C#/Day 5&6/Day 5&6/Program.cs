using Day5_6;
using System;
using System.Collections;
namespace Day5_6
{
    class Program
    {
        static void Main(string[] args)
        {
            // Q1
            int[] Array = { 3, 5, 2, 9, 10 };
            BetterBubbleSort<int>.Sort(ref Array);
            foreach (int var in Array)
                Console.WriteLine(var);

            // Q2
            Range<int> MyRange = new Range<int>(5, 10);
            Console.WriteLine(MyRange.IsInRange(7));
            Console.WriteLine(MyRange.Length());

            // Q3 Function in Helper.cs
            ArrayList arrayList = new ArrayList(5) { 1, 2, 3, 4, 5 };
            Helper.Reverse(arrayList);
            foreach (object element in arrayList) { Console.WriteLine(element); }

            // Q4 Function in Helper.cs
            List<int> ints = new List<int>(5) { 1, 2, 3, 4, 5 };
            List<int> EvenList = Helper.ReturnEven(ints);
            foreach (int num in EvenList) { Console.WriteLine(num); }

            // Q5
            FixedSizeList<int> FList = new FixedSizeList<int>(2);
            FList.Add(10);
            FList.Add(20);
            // This will throw an exception
            //FList.Add(30);
            //Console.WriteLine(FList.Get(0));
            // This will throw an exception
            //Console.WriteLine(FList.Get(3));

            // Q6
            string s = "eerrnnfffz";
            Console.WriteLine(Helper.ReturnUnique(s));
            string x = "eerrnnffff";
            Console.WriteLine(Helper.ReturnUnique(x));

        }
    }
}