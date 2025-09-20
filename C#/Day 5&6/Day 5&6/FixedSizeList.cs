using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day5_6
{
    internal class FixedSizeList<T>
    {
        List<T> FixedList = new List<T>();
        int counter;
        public FixedSizeList(int Size) => FixedList.Capacity = Size;
        public void Add(T element)
        {
            if (counter >= FixedList.Capacity) {
                throw new InvalidOperationException("List is Full");
            }
            else
            {
                FixedList.Add(element);
                counter++;
            } 
        }
        public T Get(int index)
        {
            if(index > FixedList.Capacity)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            else { return FixedList[index]; }
        }
    }
}
