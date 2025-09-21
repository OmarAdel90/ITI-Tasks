using System;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
namespace Day7
{
    class Program
    {
        static void Main(string[] args)
        {
            // Q1
            Product MyProduct = new Product("Tape", 30);
            var AnonObj = CreateProduct(MyProduct);
            Console.WriteLine(AnonObj);

            // Q2
            string MyString = "Hello,World Test";
            Console.WriteLine(MyString.ReturnWordCount());

            // Q3
            int MyInt = 4;
            int MyInt2 = 5;
            Console.WriteLine(MyInt.IsEven());
            Console.WriteLine(MyInt2.IsEven());

            // Q4
            DateTime BirthDay = new DateTime(2013, 5, 20);
            Console.WriteLine(BirthDay.ReturnAge());

            // Q5
            string MyString2 = "Hello";
            Console.WriteLine(MyString2.Reverse());
        }

        class Product
        {
            public string Name;
            public int Price;
            public Product(string Name,int Price)
            {
                this.Name = Name;
                this.Price = Price;
            }
        }
        static object CreateProduct(Product product)
        {
            return new
            {
                Name = product.Name,
                Price = product.Price
            };
        }
    }
    static class StringExtension
    {
        public static int ReturnWordCount(this string InputString)
        {
            string[] SplitWords = InputString.Split(' ', ',');
            return SplitWords.Count();
        }
        public static string Reverse(this string InputString)
        {
            string Reversed = "";
            for(int i = InputString.Length - 1; i >= 0; i--)
            {
                Reversed += InputString[i];
            }
            return Reversed;
        }
    }
    static class IntExtension
    {
        public static bool IsEven(this int X)
        {
            return X % 2 == 0;
        }
    }
    static class DateTimeExtension
    {
        public static int ReturnAge(this DateTime birthDate)
        {
            return DateTime.Now.Year - birthDate.Year;
        }
    }
}