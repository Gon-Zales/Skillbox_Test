using Microsoft.EntityFrameworkCore.ChangeTracking;
using Skillbox.App.Tools;
using System;
using System.Linq;

namespace Skillbox.App.ViewModel
{
    public class OrganizationVM : Observable
    {
        public ObservableHashSet<DepartmentVM> Departments { get; } = new ObservableHashSet<DepartmentVM>();
        public ObservableHashSet<EmployeeVM> Employees { get; } = new ObservableHashSet<EmployeeVM>();

        internal void AddDepartment(DepartmentVM departmentVM)
        {
            var subdeps = EntityManager.AllEntities.Departments.SelectMany(d => d.Departments);
            if (!subdeps.Contains(departmentVM.Id))
                Departments.Add(departmentVM);
        }
    }
}
