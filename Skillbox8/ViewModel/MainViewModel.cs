using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Skillbox.App.Model;
using Skillbox.App.Tools;

namespace Skillbox.App.ViewModel
{
    class MainViewModel : Observable
    {
        public CancelEventHandler RequestClose { get; internal set; }
        public OrganizationVM Organization => EntityManager.Organization;
        public ICommand NewDepartmentCommand { get; set; }
        public ICommand NewEmployeeCommand { get; set; }

        public MainViewModel()
        {
            CreateMockOrg();

            EntityManager.Load();
            OnPropertyChanged(nameof(Organization));
            NewDepartmentCommand = new RelayCommand(CreateDepartment);
            NewEmployeeCommand = new RelayCommand(CreateEmployee);

            RequestClose = PromptOnClose;
        }

        private void CreateDepartment()
        {
            Organization.AddDepartment(new DepartmentVM(new Department()));
        }

        private void CreateEmployee()
        {
            Organization.Employees.Add(new EmployeeVM(new Employee()));
        }

        private void CreateMockOrg()
        {
            var dep2 = new Department { Name = "Actual reports for inner usage", Created = DateTime.Now };
            var dep1 = new Department { Name = "Tax Avoidance", Created = DateTime.Now };
            var dep3 = new Department { Name = "FinDep", Created = DateTime.Now };
            dep3.Departments.Add(dep1.Id);
            dep3.Departments.Add(dep2.Id);
            var pd1 = new InternPayment { Salary = 600M };
            var pd2 = new WorkerPayment { HoursLastMonth = 250M, PayRate = 400M };
            var pd3 = new ManagerPayment();
            var ivan = new Employee { Birthday = new DateTime(1970, 9, 9), Name = "Ivan Snow", PaymentData= pd1, DepartmentId = dep1.Id };
            var petr = new Employee { Birthday = new DateTime(1999, 9, 9), Name = "Petr Snow", PaymentData = pd2, DepartmentId = dep2.Id };
            var max = new Employee { Birthday = new DateTime(1999, 9, 9), Name = "Max Snow", PaymentData= pd3, DepartmentId = dep3.Id };
            dep1.Employees.Add(ivan.Id);
            dep2.Employees.Add(petr.Id);
            dep3.Employees.Add(max.Id);
            Organization.AddDepartment(new DepartmentVM(dep3));
            EntityManager.Save();
        }

        private void PromptOnClose(object sender, CancelEventArgs e)
        {
            var res = MessageBox.Show("Save?", "", MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.Yes);
            switch (res)
            {
                case MessageBoxResult.Yes:
                    EntityManager.Save();
                    break;
                case MessageBoxResult.No:
                    break;
                default:
                    e.Cancel = true;
                    break;
            }
        }
    }
}
