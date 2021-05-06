using Skillbox.App.Tools;
using System.Collections.ObjectModel;

namespace Skillbox.App.ViewModel
{
    public class OrganizationVM : Observable
    {
        public ObservableCollection<DepartmentVM> Departments { get; } = new ObservableCollection<DepartmentVM>();
        public ObservableCollection<EmployeeVM> Employees { get; } = new ObservableCollection<EmployeeVM>();
    }
}
