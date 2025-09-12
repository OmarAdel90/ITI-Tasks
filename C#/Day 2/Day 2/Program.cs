using System;
using System.Linq;
using System.Xml.Linq;

namespace Day1
{
    class Program
    {
        static void Main(string[] args)
        {
            //Q1
            StudentInput();

            //Q2
            TrackAvg();

            //Q3
            Time t1 = new Time(3, 40, 11);
            t1.Print();
        }

        static void StudentInput()
        {
            Console.WriteLine("Enter Number of students: ");
            int n = Convert.ToInt32(Console.ReadLine());
            string[] names = new string[100]; // Unused memory if input names < 100
            for(int i = 0; i < n; i++)
            {
                Console.WriteLine("Enter name: " + i);
                names[i] = Console.ReadLine();
            }
            for(int i = 0; i < n; i++)
            {
                Console.WriteLine("Student Name: " + names[i]);
            }

        }
        static void TrackAvg()
        {
            int[] FrontAges = new int[100];
            int[] BackAges = new int[100];
            int[] StudentNums = new int[100];
            Console.WriteLine("Enter Number of students: ");
            int n = Convert.ToInt32(Console.ReadLine());
            // Filling in data
            int BackCounter = 0, Frontcounter = 0;
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine("Enter student number: ");
                StudentNums[i] = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Enter student track: ");
                string temp = Console.ReadLine();
                
                if(temp == "Front")
                {
                    Console.WriteLine("Enter student age: ");
                    FrontAges[i]= Convert.ToInt32(Console.ReadLine());
                    Frontcounter++;
                }
                else
                {
                    Console.WriteLine("Enter student age: ");
                    BackAges[i] = Convert.ToInt32(Console.ReadLine());
                    BackCounter++;
                }
            }
            
            // Printing average per track and ages for each track
            Console.WriteLine("FrontEnd ages: ");
            for(int i = 0; i < n; i++) { Console.Write(FrontAges[i] + ","); }
            Console.WriteLine("BackEnd ages: ");
            for (int i = 0; i < n; i++) { Console.Write(BackAges[i] + ","); }
            Console.WriteLine("FrontEnd average ages: " + (FrontAges.Sum() / Frontcounter));
            Console.WriteLine("BackEnd average ages: " + (BackAges.Sum() / BackCounter));
        }
        struct Time
        {
            public int hours, minutes, seconds;
            public Time(int hour,int minute, int second)
            {
                hours = hour;
                minutes = minute;
                seconds = second;
            }
            public void Print()
            {
                Console.WriteLine(hours + "H:" + minutes + "M:" + seconds + "S");
            }
        }
    }
}
