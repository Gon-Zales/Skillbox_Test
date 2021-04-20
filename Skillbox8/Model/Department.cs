using Microsoft.EntityFrameworkCore.ChangeTracking;
using Skillbox.App.Tools;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;

namespace Skillbox.App.Model
{
    public class Department : Observable
    {
        private readonly ObservableHashSet<Employee> employees = new ObservableHashSet<Employee>();
        public static Dictionary<int, Department> AllDepartments = new Dictionary<int, Department>();

        public int Id { get; internal set; } = AllDepartments.Count;
        public string Name { get; set; }
        public DateTime Created { get; set; }
        [IgnoreDataMember]
        public ISet<Employee> Employees { get => employees; }
        public ISet<int> EmployeeIDs => new HashSet<int>(employees.Select(x => x.Id));
        public ISet<Department> Departments { get; } = new HashSet<Department>();

        public Department()
        {
            employees.CollectionChanged += Employees_CollectionChanged;
            AllDepartments[Id] = this;
        }
        private void Employees_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Employees));
            OnPropertyChanged(nameof(EmployeeIDs));
        }
    }
}
