using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Day5_6
{
    internal class Range<T>
    {
        public T min, max;
        public Range(T min,T max)
        {
            this.min = min;
            this.max = max;
        }
        public bool IsInRange(T Value)
        {
            if (Comparer<T>.Default.Compare(Value, min) > 0 && (Comparer<T>.Default.Compare(Value, max) < 0))
                return true;
            else return false;

        }
        public double Length()
        {
            return Convert.ToDouble(max) - Convert.ToDouble(min);
        }
    }
}
