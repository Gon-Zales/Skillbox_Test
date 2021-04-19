namespace Skillbox.App.Model
{
    public class Employee
    {
        private Department department;

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public Department Department
        {
            get => department;
            set
            {
                if (department == value) return;
                department?.Employees.Remove(this);
                department = value;
                department.Employees.Add(this);
            }
        }
        public int Id { get; set; }
        public decimal Salary { get; set; }
    }
}
