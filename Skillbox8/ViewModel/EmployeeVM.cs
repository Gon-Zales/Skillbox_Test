using Skillbox.App.Model;
using Skillbox.App.Tools;
using System;
using System.Diagnostics;

namespace Skillbox.App.ViewModel
{
    public class EmployeeVM : Observable, IEntity
    {
        private DepartmentVM department;

        public int Id { get; set; }
        public string Name { get; set; }
        public int Age => DateTime.Today.Year - Birthday.Year;
        public DateTime Birthday { get; set; }
        public decimal Salary { get; set; }
        public int DepartmentId
        {
            get => department.Id;
            set => Department = EntityManager.GetDepartmentVM(value);
        }
        public DepartmentVM Department
        {
            get => department;
            set
            {
                if (department == value)
                    return;
                Debug.WriteLine($"Employee {Id}.Department is assigned {value.Id}");
                department?.Employees.Remove(this);
                department = value;
                department.Employees.Add(this);
                OnPropertyChanged();
            }
        }

        public EmployeeVM(Employee model)
        {
            Id = model.Id;
            Name = model.Name;
            Birthday = model.Birthday;
            Salary = model.Salary;
            EntityManager.AllEmployeeVMs[Id] = this;
            DepartmentId = model.DepartmentId;
            Debug.WriteLine($"Employee '{Name}' is created {Id}");
        }

        public override bool Equals(object obj)
        {
            return obj is EmployeeVM vM && Id == vM.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
