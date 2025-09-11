using System;

namespace Day1
{
    class program
    {
        static void Main(string[] args)
        {
            // I am using functions for better readability

            //Q1
            ReturnAscii();

            //Q2
            ReturnChar();

            //Q3
            EvenOrOdd();

            //Q4
            NumsOperations();

            //Q5 i don't understand if total course degree should be specified to calculate grade with respect to it
            GradeCalc();

            //Q6
            MultiplicationTable();
        }

        static void ReturnAscii()
        {
            Console.Write("Type character: ");
            char s = Console.ReadLine()[0];
            Console.WriteLine("ASCII code of " + s + " is " + (int)s);
        }
        static void ReturnChar()
        {
            Console.Write("Type ASCII Code: ");
            int s = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Char corresponding to " + s + " is " + (char)s);
        }
        static void EvenOrOdd()
        {
            Console.WriteLine("Enter number to check if it's even or odd");
            int input = Convert.ToInt32(Console.ReadLine());
            if (input % 2 == 0) Console.WriteLine(input + " " + "is even");
            else Console.WriteLine(input + " " + "is odd");
        }
        static void NumsOperations()
        {
            Console.WriteLine("Enter X: ");
            float x = Convert.ToSingle(Console.ReadLine());
            Console.WriteLine("Enter Y: ");
            float y = Convert.ToSingle(Console.ReadLine());
            Console.WriteLine("Sum: " + (x + y));
            Console.WriteLine("Subtraction: " + (x - y));
            Console.WriteLine("Multiplication: " + (x * y));
        }
        static void GradeCalc()
        {
            Console.WriteLine("Enter your mark: ");
            float studentMark = Convert.ToSingle(Console.ReadLine());
            Console.WriteLine("Enter total course marks: ");
            float totalCourseMark = Convert.ToSingle(Console.ReadLine());
            float percentage = (studentMark / totalCourseMark) * 100;
            if (percentage >= (totalCourseMark / 2)) Console.WriteLine("Student Grade: D");
            else Console.WriteLine("Student Grade: F");
        }
        static void MultiplicationTable()
        {
            Console.WriteLine("Enter number to get multiplication table for: ");
            int x = Convert.ToInt32(Console.ReadLine());
            for (int i = 1; i <= 12; i++)
            {
                Console.WriteLine(x + " * " + i + " = " + x * i);
            }
        }
    }
}