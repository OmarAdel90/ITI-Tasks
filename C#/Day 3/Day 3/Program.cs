using System;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Security.Cryptography;

namespace Day3
{
    class Program
    {
        static void Main()
        {
            Calc c1 = new Calc();
            Console.WriteLine(c1.Sum(2, 3));
            Console.WriteLine(c1.Sum(2.5f, 3.8f));

            MCQQuestion q = new MCQQuestion();
            q.show();

            // Create array of MCQ questions and take input from user
            Console.Write("How many questions do you want to make: ");
            int qCount = Convert.ToInt32(Console.ReadLine());
            MCQQuestion[] qs = new MCQQuestion[qCount];
            // Input Questions and their data
            for(int i = 0; i < qCount; i++)
            {
                Console.WriteLine("Enter Question " + i);
                Console.Write("Enter Head: ");
                string head = Console.ReadLine();
                Console.Write("Enter Body: ");
                string body = Console.ReadLine();
                Console.Write("Enter Mark: ");
                float mark = Convert.ToSingle(Console.ReadLine());
                Console.Write("Enter the correct answer: ");
                string answer = Console.ReadLine();
                string[] qsChoices = new string[3];
                for(int j = 0; j < 3; j++)
                {
                    Console.WriteLine("Enter choice " + j);
                    qsChoices[j] = Console.ReadLine();
                }
                qs[i] = new MCQQuestion(head, body, mark, qsChoices,answer);
                Console.WriteLine($"Question {i} object created and stored in array");
            }
            // Display questions and their choices
            for(int i = 0; i < qCount; i++) { qs[i].show(); }

            // Get answers from users and calculate result
            string[] Answers = new string[qCount];
            float TotalMark = 0;
            Console.WriteLine("Input your answers");
            for(int i = 0; i < qCount; i++)
            {
                Answers[i] = Console.ReadLine();
            }
            for(int i = 0; i < qCount; i++)
            {
                if (qs[i].GetAnswer() == Answers[i]) TotalMark+=qs[i].mark;
            }
            Console.WriteLine("Your total marks: " + TotalMark);
        }
        class Calc
        {
            // I dont understand if i should have members when its a calculator class
            

            // Methods
            
            public int Sum(int x, int y)
            {
                return x + y;
            }

            // Overloaded sum to accept floats and return them
            public float Sum(float x, float y)
            {
                return x + y;
            }

            // The rest can be overloaded the same way as Sum() method
            public int Sub(int x, int y)
            {
                return x - y;
            }
            public int Multiply(int x, int y)
            {
                return x * y;
            }
            public float div(int x, int y)
            {
                return x / y;
            }
        }
        class Question
        {
            public string head;
            public string body;
            public float mark;

            public Question()
            {
                head = "Default Question";
                body = "Default Body";
                mark = 10;
            }
            public Question(string InputQuestion,string body,float mark)
            {
                this.head = InputQuestion;
                this.body = body;
                this.mark = mark;
            }
            public void show()
            {
                Console.Write("Title: " + head);
                Console.Write("Body: " + body);
                Console.Write("Mark: " + mark);
            }

        }
        class MCQQuestion : Question
        {
            public string[] choices;
            private string answer { get; set; }

            public MCQQuestion() : base()
            { 
                choices = new string[]{ "Option A","Option B","Option C" };
            }
            public MCQQuestion(string Head,string body,float mark, string[] choices,string answer) : base(Head,body,mark)
            {
                this.choices = choices;
                this.answer = answer;
            }
            public string GetAnswer() { return answer; }
            public void show()
            {
                base.show();
                Console.WriteLine("Choices:");
                Console.WriteLine(string.Join("\n", choices));
            }


        }


    }
}
