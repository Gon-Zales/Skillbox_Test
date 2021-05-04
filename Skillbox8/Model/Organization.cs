using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Skillbox.App.Model
{
    public class Organization
    {
        public ObservableHashSet<Department> Departments { get; set; }
        public ObservableHashSet<Employee> Employees { get; set; }
    }
}
