using Skillbox.App.Tools;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Skillbox.App.ViewModel
{
    public class OrganizationVM : Observable
    {
        public ObservableCollection<DepartmentVM> Departments { get; } = new ObservableCollection<DepartmentVM>();
        public ObservableCollection<EmployeeVM> Employees { get; } = new ObservableCollection<EmployeeVM>();

        internal void AddDepartment(DepartmentVM departmentVM)
        {
            var subdeps = EntityManager.AllEntities.Departments.SelectMany(d => d.Departments);
            if (!subdeps.Contains(departmentVM.Id))
                Departments.Add(departmentVM);
        }
    }
}
