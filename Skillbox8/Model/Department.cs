using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.Serialization;

namespace Skillbox.App.Model
{
    public class Department
    {
        private readonly ObservableHashSet<Employee> employees = new ObservableHashSet<Employee>();
        public Department()
        {
            employees.CollectionChanged += Employees_CollectionChanged;
        }

        public string Name { get; set; }
        public DateTime Created { get; set; }
        public int EmployeesCount => Employees.Count;

        [IgnoreDataMember]
        public ISet<Employee> Employees { get => employees; }
        public ISet<Department> Departments { get; } = new HashSet<Department>();
        private void Employees_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (Employee newbie in e.NewItems)
                    newbie.Department = this;
            if (e.OldItems != null)
                foreach (Employee retiree in e.OldItems)
                    retiree.Department = null;
        }
    }
}
