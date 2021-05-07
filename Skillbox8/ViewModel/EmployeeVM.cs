using Microsoft.EntityFrameworkCore.ChangeTracking;
using Skillbox.App.Model;
using Skillbox.App.Tools;
using System;
using System.Diagnostics;
using System.Linq;

namespace Skillbox.App.ViewModel
{
    public class EmployeeVM : Observable, IEntity
    {
        private DepartmentVM department;
        private readonly Employee model;

        public int Id { get; set; }
        public string Name { get; set; }
        public int Age => DateTime.Today.Year - Birthday.Year;
        public DateTime Birthday { get; set; }
        public decimal Salary { get; set; }
        public int DepartmentId
        {
            get => department.Id;
            set => Department = EntityManager.GetDepartmentVM(value);
        }
        public string Position { get; set; }
        public IPaymentData PaymentData { get; set; }
        public ObservableHashSet<EmployeeVM> Subordinates { get; set; } = new ObservableHashSet<EmployeeVM>();
        public DepartmentVM Department
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
            }
        }

        public EmployeeVM(Employee model)
        {
            this.model = model;
            Id = model.Id;
            Name = model.Name;
            Birthday = model.Birthday;
            PaymentData = model.PaymentData;
            EntityManager.AllEmployeeVMs[Id] = this;
            DepartmentId = model.DepartmentId;
            Subordinates = new ObservableHashSet<EmployeeVM>(model.Subordinates.Select(x => EntityManager.GetEmployeeVM(x)));
            Debug.WriteLine($"Employee '{Name}' is created {Id}");
        }
        public void Save()
        {
            model.Id = Id;
            model.Name = Name;
            model.Birthday = Birthday;
            model.PaymentData = PaymentData;
            model.DepartmentId = DepartmentId;
            model.Subordinates = Subordinates.Select(x => x.Id).ToHashSet();
        }
        public override bool Equals(object obj)
        {
            return obj is EmployeeVM vM && Id == vM.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
    class WorkerVM : EmployeeVM
    {
        public WorkerVM(Employee model) : base(model)
        {
            if (model.PaymentData == null)
                PaymentData = new WorkerPayment();
        }
    }
    class InternVM : EmployeeVM
    {
        public InternVM(Employee model) : base(model)
        {
            if (model.PaymentData == null)
                PaymentData = new InternPayment();
        }
    }
    class ManagerVM : EmployeeVM
    {

        public ManagerPayment Payment
        {
            get => (ManagerPayment)PaymentData;
            set
            {
                value.SumSubordinatesSal = () => Subordinates.Sum(s => s.Salary);
                PaymentData = value;
                OnPropertyChanged();
            }
        }
        public ManagerVM(Employee model) : base(model)
        {
            if (model.PaymentData == null)
                PaymentData = new ManagerPayment();
        }
    }
}
