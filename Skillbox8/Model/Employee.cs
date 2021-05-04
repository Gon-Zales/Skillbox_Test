using Skillbox.App.Tools;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;

namespace Skillbox.App.Model
{
    public class Employee : Observable
    {
        private Department department;

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public decimal Salary { get; set; }
        public int DepartmentId
        {
            get => department?.Id??0;
            set
            {
                Debug.WriteLine($"Employee {Id}.DepartmentId is assigned {value}");
                Department = Department.AllDepartments.Single(x => x.Key == value).Value;
            }
        }

        [IgnoreDataMember]
        public Department Department
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
                OnPropertyChanged(nameof(DepartmentId));
            }
        }
    }
}
