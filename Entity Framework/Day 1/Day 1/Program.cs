using System;
using System.Xml.Linq;
namespace Day1
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> numbers = new List<int>() { 2, 4, 6, 7, 1, 4, 2, 9, 1 };
            // Q1
            var OrderedDistinct = numbers.Distinct().Order();
            Console.WriteLine(string.Join(", ", OrderedDistinct));

            // Q2
            var Multiplied = OrderedDistinct.Select(x => x * x);
            Console.WriteLine(string.Join(", ", Multiplied));

            string[] names = { "Tom", "Dick", "Harry", "MARY", "Jay" };
            // Q3
            var NameLength3 = names.Where(x => x.Length == 3);
            Console.WriteLine(string.Join(", ", NameLength3));

            // Q4
            var ContainsA = names.Where(x => x.ToLower().Contains('a')).OrderBy(x => x.Length);
            Console.WriteLine(string.Join(", ", ContainsA));

            // Q5
            var FirstTwo = names.Take(2);
            Console.WriteLine(string.Join(", ", FirstTwo));

            List<Student> students = new List<Student>()
            {
                new Student(){
                    ID = 1,
                    FirstName = "Ali",
                    LastName = "Mohammed",
                    subjects = new Subject[] {
                        new Subject(){ Code = 22, Name = "EF" },
                        new Subject(){ Code = 33, Name = "UML" }
                    }
                },
                new Student(){
                    ID = 2,
                    FirstName = "Mona",
                    LastName = "Gala",
                    subjects = new Subject[] {
                        new Subject(){ Code = 22, Name = "EF" },
                        new Subject(){ Code = 34, Name = "XML" },
                        new Subject(){ Code = 25, Name = "JS" }
                    }
                },
                new Student(){
                    ID = 3,
                    FirstName = "Yara",
                    LastName = "Yousf",
                    subjects = new Subject[] {
                        new Subject(){ Code = 22, Name = "EF" },
                        new Subject(){ Code = 25, Name = "JS" }
                    }
                },
                new Student(){
                    ID = 1,
                    FirstName = "Ali",
                    LastName = "Ali",
                    subjects = new Subject[] {
                        new Subject(){ Code = 33, Name = "UML" }
                    }
                }
            };
            // Q6
            var StudentDetails = students.Select(x => new
            {
                FullName = x.FirstName +" "+ x.LastName,
                NoOfSubjects = x.subjects.Length
            });
            Console.WriteLine(string.Join("\n ", StudentDetails));

            // Q7
            var StudentOrder = students.OrderByDescending(x => x.FirstName).ThenBy(x => x.LastName).Select(x =>  x.FirstName + " " + x.LastName );
            Console.WriteLine(string.Join("\n ", StudentOrder));

            // Q8
            var StudentSubjects = students.SelectMany(
                x => x.subjects,
                (student, subject) => new
                {
                    FullName = student.FirstName + " " + student.LastName,
                    subject = subject.Name
                }
             );
            Console.WriteLine(string.Join("\n ", StudentSubjects));

        }
        public class Student
        {
            public int ID { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public Subject[] subjects { get; set; }
        }

        public class Subject
        {
            public int Code { get; set; }
            public string Name { get; set; }
        }
    }
}