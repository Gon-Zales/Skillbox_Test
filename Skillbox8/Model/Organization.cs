using System.Collections.Generic;

namespace Skillbox.App.Model
{
    public class Organization
    {
        public IEnumerable<Department> Departments { get; set; }
        public IEnumerable<Employee> Employees { get; set; }
    }
}
