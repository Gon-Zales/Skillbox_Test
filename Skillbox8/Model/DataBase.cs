using System.Collections.Generic;

namespace Skillbox.App.Model
{
    public class DataBase
    {
        public IList<Employee> Employees { get; } = new List<Employee>();
        public IList<Department> Departments { get; } = new List<Department>();
    }
}
