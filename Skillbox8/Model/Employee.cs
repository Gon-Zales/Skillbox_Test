using Skillbox.App.Tools;
using System;
using System.Diagnostics;

namespace Skillbox.App.Model
{
    public class Employee : IEntity
    {
        public int Id { get; set; } = EntityManager.Count;
        public string Name { get; set; }
        public int Age => DateTime.Today.Year - Birthday.Year;
        public DateTime Birthday { get; set; }
        public decimal Salary { get; set; }
        public int DepartmentId { get; set; }

        public Employee()
        {
            Debug.WriteLine($"Employee '{Name}' is created {Id}");
            EntityManager.AllEntities.Employees.Add(this);
        }
    }
}
