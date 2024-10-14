using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University
{
    public interface IVisitorUniversity
    {
        void Accept(IVisitor visitor);
    }

    public class University : IVisitorUniversity
    {
        public string Name { get; }
        public List<Faculty> Faculties { get; } = new List<Faculty>();

        public University(string name)
        {
            Name = name;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.VisitUniversity(this);
        }
    }

    public class Faculty : IVisitorUniversity
    {
        public string Name { get; }
        public List<Department> Departments { get; } = new List<Department>();

        public Faculty(string name)
        {
            Name = name;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.VisitFaculty(this);
        }
    }

    public class Department : IVisitorUniversity
    {
        public string Name { get; }
        public List<Employee> Employees { get; } = new List<Employee>();

        public Department(string name)
        {
            Name = name;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.VisitDepartment(this);
        }
    }

    public class Employee : IVisitorUniversity
    {
        public string Name { get; }
        public int Experience { get; }
        public decimal Salary { get; set; }

        public Employee(string name, int experience, decimal salary)
        {
            Name = name;
            Experience = experience;
            Salary = salary;
        }
        public void Accept(IVisitor visitor)
        {
            visitor.VisitEmployee(this);
        }
    }

    public interface IVisitor
    {
        void VisitUniversity(University university);
        void VisitFaculty(Faculty faculty);
        void VisitDepartment(Department department);
        void VisitEmployee(Employee employee);
    }

    public class SalaryCalculatorVisitor : IVisitor
    {
        private decimal totalSalary;

        public decimal TotalSalary => totalSalary;

        public void VisitUniversity(University university)
        {
            totalSalary = 0;
            foreach (var faculty in university.Faculties)
            {
                faculty.Accept(this);
            }
            Console.WriteLine($"Загальна зарплата для університету {university.Name}: {totalSalary}");
        }

        public void VisitFaculty(Faculty faculty)
        {
            foreach (var department in faculty.Departments)
            {
                department.Accept(this);
            }
            Console.WriteLine($"Загальна зарпалата для факультету {faculty.Name}: {totalSalary}");
        }

        public void VisitDepartment(Department department)
        {
            foreach (var employee in department.Employees)
            {
                 employee.Accept(this);
            }
            Console.WriteLine($"Загальна зарпалата для кафедри {department.Name}: {totalSalary}");
        }
        public void VisitEmployee(Employee employee)
        {
            totalSalary += employee.Salary;
        }
        
    }

    public class SalaryIncreaseVisitor : IVisitor
    {
        private readonly int yearsThreshold;
        private readonly decimal increasePercentage;

        public SalaryIncreaseVisitor(int yearsThreshold, decimal increasePercentage)
        {
            this.yearsThreshold = yearsThreshold;
            this.increasePercentage = increasePercentage;
        }
        public void VisitUniversity(University university)
        {
            foreach (var faculty in university.Faculties)
            {
                faculty.Accept(this);
            }
        }

        public void VisitFaculty(Faculty faculty)
        {
            foreach (var deparment in faculty.Departments)
            {
                deparment.Accept(this);
            }
        }

        public void VisitDepartment(Department department)
        {
            foreach (var employee in department.Employees)
            {
                employee.Accept(this);
            }
        }
        public void VisitEmployee(Employee employee)
        {
            if (employee.Experience > yearsThreshold)
            {
                decimal oldSalary = employee.Salary;
                employee.Salary += employee.Salary * (increasePercentage / 100);
                Console.WriteLine($"Заробітна плата для {employee.Name} до підвищення: {oldSalary}, після підвищення: {employee.Salary}");
            }
           else
           {
                Console.WriteLine($"{employee.Name} не відповідає критерію стажу для підвищення.");
           }
       }
        
    }

    class Program
    {
        static void Main(string[] args)
        {
            var university = new University("Uzhnu");

            var mathFaculty = new Faculty("Math");
            var systemDepartment = new Department("System Analisis");
            systemDepartment.Employees.Add(new Employee("Alice", 6, 50000));
            systemDepartment.Employees.Add(new Employee("Max", 3, 45000));

            var AlgebraDepartment = new Department("Algebra");
            AlgebraDepartment.Employees.Add(new Employee("Sophia", 7, 55000));

            mathFaculty.Departments.Add(systemDepartment);
            mathFaculty.Departments.Add(AlgebraDepartment);

            university.Faculties.Add(mathFaculty);

            
            var salaryCalculator = new SalaryCalculatorVisitor();
            university.Accept(salaryCalculator);

            
            var salaryIncreaseVisitor = new SalaryIncreaseVisitor(4, 10); 
            university.Accept(salaryIncreaseVisitor);

            Console.WriteLine("Змінення зарпалати завершено.");
            Console.ReadLine();
        }
    }
}
