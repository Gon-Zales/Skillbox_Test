using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Skillbox.App.Model;

namespace Skillbox8.ViewModel
{
    class MainViewModel
    {
        private readonly string fileName = @".\organization.json";

        public MainViewModel()
        {
            var dep2 = new Department { Name = "Actual reports for inner usage", Created = DateTime.Now };
            var dep1 = new Department { Name = "Tax Avoidance", Created = DateTime.Now };
            var dep3 = new Department { Name = "FinDep", Created = DateTime.Now };
            dep3.Departments.Add(dep1);
            dep3.Departments.Add(dep2);
            var ivan = new Employee { Age = 24, FirstName = "Ivan", LastName = "Snow", Id = 0, Salary = 1, Department = dep1 };
            var petr = new Employee { Age = 54, FirstName = "Petr", LastName = "Snow", Id = 1, Salary = 1, Department = dep2 };
            Organization = new Organization
            {
                Departments = new List<Department> { dep3 },
                Employees = new List<Employee> { ivan, petr }
            };
            Save();
            Load();
        }

        private void Save()
        {
            using (var stream = File.CreateText(fileName))
            {
                string json = JsonConvert.SerializeObject(Organization);
                stream.Write(json);
            }
        }

        private void Load()
        {

            using (var r = new StreamReader(fileName)) //using (var r = new TextWriter(fileName))
            {
                string json = r.ReadToEnd();
                Organization = JsonConvert.DeserializeObject<Organization>(json);
            }
        }

        public Action RequestClose { get; internal set; }
        public Organization Organization { get; set; }
    }
}
