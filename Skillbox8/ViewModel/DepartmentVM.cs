using Skillbox.App.Model;
using Skillbox.App.Tools;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Skillbox.App.ViewModel
{
    public class DepartmentVM : Observable, IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public ObservableCollection<EmployeeVM> Employees { get; } = new ObservableCollection<EmployeeVM>();
        public ObservableCollection<DepartmentVM> Departments { get; } = new ObservableCollection<DepartmentVM>();

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
    }
}
