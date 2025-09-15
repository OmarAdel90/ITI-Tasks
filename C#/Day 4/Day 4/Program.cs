using System;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
namespace Day4;

class Program
{
    static void Main(string[] args)
    {
        // First Requirment
        Company MyCompany = new Company();
        Department IT = new Department("IT");
        Department HR = new Department("HR");
        MyCompany.Departments.AddRange([IT, HR]);
        Course Back = new Course("Backened", CourseLevel.Beginner,50);
        Course SQL = new Course("SQL", CourseLevel.Intermediate,60);
        Course MVC = new Course("Model View Controller", CourseLevel.Advanced,70);
        Employee EMP1 = new Employee("Ahmed", [Back, MVC], IT);
        Employee EMP2 = new Employee("Hussien", [], HR);
        List<Employee> Employees = new List<Employee>();
        Employees.AddRange([EMP1, EMP2]);
        foreach (Employee emp in Employees)
        {
            Console.WriteLine("Employee Name: " + emp.Name);
            Console.WriteLine("Employee Department: " + emp.Dept.DeptName);
            Console.WriteLine("Employee Courses: " + string.Join(", ", emp.Courses.Select(c => c.Name)));
        }

        // Second Requirment
        Instructor I1 = new Instructor("Ahmed", 30);
        Student S1 = new Student("Salah", 17);
        I1.Introduce();
        S1.Introduce();

        // Third Requirment, Shape class only has an abstract function, do you mean to creat rectangle and a circle to test Draw() and Area()?
        Rectangle R1 = new Rectangle(3f, 4.5f);
        Console.WriteLine("Rectangle Area: " + R1.Area());
        R1.Draw();
        Circle C1 = new Circle(5f);
        Console.WriteLine("Circle Area: " + C1.Area());
        C1.Draw();

        // Fourth Requirment
        Grade Math = new Grade(20);
        Grade Geography = new Grade(30);
        List<Grade> MohsenGrades = new List<Grade> { Math, Geography };
        Student S2 = new Student("Mohsen", 23, MohsenGrades);
        // (Math+Geography) returns an object that i access its value field
        Console.WriteLine("Mohsen's Total Grades: " + (Math + Geography).Value);
        Console.WriteLine(Math == Geography);

        // System Sim
        Company Uni = new Company();
        Department CS = new Department("Computer Science");
        Department IS = new Department("Information System");
        Uni.Departments.AddRange([CS, IS]);
        Course Algorithm = new Course("Algorithm", CourseLevel.Advanced,100);
        Course DS = new Course("Data Structures", CourseLevel.Beginner,50);
        Employee HR1 = new Employee("Hoda", [], CS);
        Instructor Prof1 = new Instructor("Prof. Salma", 40);
        Instructor Prof2 = new Instructor("Prof. Ziad", 50);
        Student S3 = new Student("Nasser", 21);
        Student S4 = new Student("Tarek", 23);
        S3.RegisterCourse(Algorithm);
        S3.RegisterCourse(DS);
        S4.RegisterCourse(Algorithm);
        Prof1.TeachCourse(Algorithm);
        Prof2.TeachCourse(DS);
        // Grouping 
        List<Department> Departments = new List<Department>();
        Departments.AddRange([CS, IS]);
        List<Instructor> Instructors = new List<Instructor>();
        Instructors.AddRange([Prof1, Prof2]);
        List<Course> Courses = new List<Course>();
        Courses.AddRange([Algorithm, DS]);
        List<Student> Students = new List<Student>();
        Students.AddRange([S3, S4]);
        // Report
        foreach(Department d in Departments)
        {
            Console.WriteLine("Department name: " + d.DeptName + " and it has "+d.EmpNum+ " Employee(s)");
        }
        foreach(Student S in Students)
        {
            Console.WriteLine("Student name: " + S.Name);
            Console.WriteLine("Student courses: " + string.Join(", ", S.RegisteredCourse.Select(c => c.Name)));
            Console.WriteLine("Student total marks: " + S.Grades.Sum(g => g.Value));
        }
        foreach(Instructor I in Instructors)
        {
            Console.WriteLine("Instructor: " + I.Name + " and they teach " + I.ActiveCourse.Name);
        }








    }

    class Company
    {
        public List<Department> Departments = new List<Department>(); 
    }
    class Department
    {
        public string DeptName;
        public int EmpNum;
        public List<Employee> Employees = new List<Employee>();
        public Department(string DeptName) => this.DeptName = DeptName;
        
    }
    class Employee
    {
        public string Name;
        public Department Dept;
        public List<Course> Courses = new List<Course>();
        public Employee(String Name, Course[] Courses,Department Dept)
        {
            this.Name = Name;
            this.Courses.AddRange(Courses);
            this.Dept = Dept;
            Dept.EmpNum++;
        }
    }
    class Course
    {
        public string Name;
        public CourseLevel Level;
        public int Mark;
        public List<Instructor> Instructors = new List<Instructor>();
        public List<Employee> Employees = new List<Employee>();
        public Course()
        {
            Name = "NO COURSE";
        }
        public Course(string Name, CourseLevel Level, int Mark){
            this.Name = Name;
            this.Level = Level;
            this.Mark = Mark;
       
        }
    }
    class Engine
    {
        public string Model;
    }
    class Car
    {
        public string Brand;
        public Engine engine;
    }
    class Person
    {
        public string Name;
        public int Age;

        public Person()
        {
            Name = "John Doe";
            Age = 0;
        }
        public Person(string Name, int Age)
        {
            this.Name = Name;
            this.Age = Age;
        }
        public virtual void Introduce() => Console.WriteLine("My name is " + Name);
    }
    class Instructor : Person
    {
        public Course ActiveCourse = new Course();
        private int ID;
        public Instructor(string Name, int Age)
        {
            this.Name = Name;
            this.Age = Age;
            ID = IdGenerator.GenerateId();
        }
        public void TeachCourse(Course course)
        {
            ActiveCourse = course;
            Console.WriteLine("Instructor " + Name + " has been assigned to course " + ActiveCourse.Name);
        }
        public override void Introduce() => Console.WriteLine("My name is " + Name + " and i am an instructor");

    }
    class Student : Person
    {
        public List<Course> RegisteredCourse = new List<Course>();
        public List<Grade> Grades = new List<Grade>();
        private int ID;
        public Student(string Name, int Age, List<Grade> Grades)
        {
            this.Name = Name;
            this.Age = Age;
            this.Grades = Grades;
            ID = IdGenerator.GenerateId();
        }
        public Student(string Name, int Age)
        {
            this.Name = Name;
            this.Age = Age;
            ID = IdGenerator.GenerateId();
        }
        public void RegisterCourse(Course course)
        {
            RegisteredCourse.Add(course);
            switch (course.Level)
            {
                case CourseLevel.Beginner:
                    Console.WriteLine("Good Luck starting out");
                    break;
                case CourseLevel.Intermediate:
                    Console.WriteLine("You are getting better");
                    break;
                case CourseLevel.Advanced:
                    Console.WriteLine("This will be challenging");
                    break;
            }
            Grades.Add(new Grade(course.Mark));
        }
        public override void Introduce() => Console.WriteLine("My name is " + Name + " and i am a student");

    }
    abstract class Shape 
    {
        public abstract float Area();
    }
    class Circle : Shape, IDrawable
    {
        public float Radius;
        public Circle(float Radius) => this.Radius = Radius;
        public override float Area()
        {
            return (3.14f * Radius * Radius);
        }
        public void Draw() => Console.WriteLine("A simple Drawn Circle");
        
    }
    class Rectangle : Shape, IDrawable
    {
        public float Length,Width;
        public Rectangle(float Length,float Width)
        {
            this.Length = Length;
            this.Width = Width;
        }
        public override float Area()
        {
            return (Length * Width);
        }
        public void Draw() => Console.WriteLine("A simple Drawn Rectangle");
    }
    interface IDrawable
    {
        void Draw();
    }
    static class IdGenerator
    {
        private static int IdCounter = 0;
        public static int GenerateId() {
            return IdCounter++;
        }
    }
    class Grade
    {
        public int Value;
        public Grade(int Value) => this.Value = Value;
        public static Grade operator +(Grade G1,Grade G2)
        {
            return new Grade(G1.Value + G2.Value);
        }
        public static bool operator ==(Grade G1,Grade G2)
        {
            return (G1.Value == G2.Value);
            
        }
        public static bool operator !=(Grade G1, Grade G2)
        {
            return G1.Value != G2.Value;
        }

    }
    enum CourseLevel
    {
        Beginner, Intermediate, Advanced
    }
    


}
