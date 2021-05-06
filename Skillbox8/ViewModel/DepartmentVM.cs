using Microsoft.EntityFrameworkCore.ChangeTracking;
using Skillbox.App.Model;
using Skillbox.App.Tools;
using System;
using System.Diagnostics;

namespace Skillbox.App.ViewModel
{
    public class DepartmentVM : Observable, IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public ObservableHashSet<EmployeeVM> Employees { get; private set; } = new ObservableHashSet<EmployeeVM>();
        public ObservableHashSet<DepartmentVM> Departments { get; } = new ObservableHashSet<DepartmentVM>();

        public DepartmentVM(Department model)
        {
            Id = model.Id;
            Name = model.Name;
            Created = model.Created;
            EntityManager.AllDepartmentVMs[Id] = this;
            foreach (var e in model.Employees)
                Employees.Add(EntityManager.GetEmployeeVM(e));
            foreach (var e in model.Departments)
                Departments.Add(EntityManager.GetDepartmentVM(e));
            Debug.WriteLine($"Department '{Name}' is created {Id}");
        }

        public override bool Equals(object obj)
        {
            return obj is DepartmentVM vM && Id == vM.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
