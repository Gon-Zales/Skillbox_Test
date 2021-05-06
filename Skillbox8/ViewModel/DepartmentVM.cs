using Microsoft.EntityFrameworkCore.ChangeTracking;
using Skillbox.App.Model;
using Skillbox.App.Tools;
using System;
using System.Diagnostics;
using System.Linq;

namespace Skillbox.App.ViewModel
{
    public class DepartmentVM : Observable, IEntity
    {
        private readonly Department model;

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public ObservableHashSet<EmployeeVM> Employees { get; private set; } = new ObservableHashSet<EmployeeVM>();
        public ObservableHashSet<DepartmentVM> Departments { get; } = new ObservableHashSet<DepartmentVM>();

        public DepartmentVM(Department model)
        {
            this.model = model;
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
        public void Save()
        {
            model.Id = Id;
            model.Name = Name;
            model.Created = Created;
            model.Employees = Employees.Select(x => x.Id).ToHashSet();
            model.Departments = Departments.Select(x => x.Id).ToHashSet();
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
