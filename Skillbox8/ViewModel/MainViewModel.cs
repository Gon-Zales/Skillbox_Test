using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using Skillbox.App.Model;
using Skillbox.App.Tools;

namespace Skillbox.App.ViewModel
{
    class MainViewModel
    {
        private readonly string fileName = @".\organization.json";

        public CancelEventHandler RequestClose { get; internal set; }
        public Organization Organization { get; set; }
        public ICommand NewDepartmentCommand { get; set; }
        public ICommand NewEmployeeCommand { get; set; }

        public MainViewModel()
        {
            var dep2 = new Department { Name = "Actual reports for inner usage", Created = DateTime.Now };
            var dep1 = new Department { Name = "Tax Avoidance", Created = DateTime.Now };
            var dep3 = new Department { Name = "FinDep", Created = DateTime.Now };
            dep3.Departments.Add(dep1);
            dep3.Departments.Add(dep2);
            var ivan = new Employee { Birthday = new DateTime(1970, 9, 9), Name = "Ivan Snow", Salary = 1543, Department = dep1 };
            var petr = new Employee { Birthday = new DateTime(1999, 9, 9), Name = "Petr Snow", Salary = 100, Department = dep2 };
            Organization = new Organization
            {
                Departments = new ObservableHashSet<Department> { dep3 },
                Employees = new ObservableHashSet<Employee> { ivan, petr }
            };
            Save();

            Load();
            NewDepartmentCommand = new RelayCommand(x => true, x => Organization.Departments.Add(new Department()));
            NewEmployeeCommand = new RelayCommand(x => true, x => Organization.Employees.Add(new Employee()));

            RequestClose = PromptOnClose;
        }

        private void PromptOnClose(object sender, CancelEventArgs e)
        {
            var res = MessageBox.Show("Save?", "", MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.Yes);
            switch (res)
            {
                case MessageBoxResult.Yes:
                    Save();
                    break;
                case MessageBoxResult.No:
                    break;
                default:
                    e.Cancel = true;
                    break;
            }
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

            using (var r = new StreamReader(fileName))
            {
                string json = r.ReadToEnd();
                Organization = JsonConvert.DeserializeObject<Organization>(json);
            }
        }
    }
}
