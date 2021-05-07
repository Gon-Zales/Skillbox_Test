using Skillbox.App.Model;
using Skillbox.App.Tools;
using System;
using System.Diagnostics;

namespace Skillbox.App.ViewModel
{
    public abstract class EmployeeVM : Observable, IEntity
    {
        protected DepartmentVM department;
        protected readonly Employee model;
        protected ManagerVM boss;

        public int Id { get; set; }
        public string Name { get; set; }
        public int Age => DateTime.Today.Year - Birthday.Year;
        public DateTime Birthday { get; set; }
        public int? DepartmentId
        {
            get => department?.Id;
            set => Department = EntityManager.GetDepartmentVM(value.Value);
        }
        public string Position { get; set; }
        public IPaymentData PaymentData { get; set; }
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
        public int? BossId
        {
            get => boss?.Id;
            set
            {
                if (value.Value == 0)
                    Boss = null;
                else if (value.Value == Id)
                    return;
                else
                    Boss = (ManagerVM)EntityManager.GetEmployeeVM(value.Value);
            }
        }
        public ManagerVM Boss
        {
            get => boss;
            set
            {
                if (boss == value)
                    return;
                Debug.WriteLine($"Employee {Id}.Boss is assigned {value?.Id}");
                boss?.Subordinates.Remove(this);
                boss = value;
                boss?.Subordinates.Add(this);
                OnPropertyChanged();
                OnPropertyChanged(nameof(BossId));
            }
        }
        public static EmployeeVM CreateEmployee(Employee model)
        {
            switch (model.PaymentData.Type)
            {
                case PaymentType.Contract:
                    return new InternVM(model);
                case PaymentType.Percentage:
                    return new ManagerVM(model);
                case PaymentType.PerHour:
                    return new WorkerVM(model);
                default:
                    throw new ArgumentOutOfRangeException("Invalid PaymentType value");
            }
        }
        protected EmployeeVM(Employee model)
        {
            this.model = model;
            Id = model.Id;
            Name = model.Name;
            Birthday = model.Birthday;
            PaymentData = model.PaymentData;
            EntityManager.AllEmployeeVMs[Id] = this;
            DepartmentId = model.DepartmentId;
            Debug.WriteLine($"Employee '{Name}' is created {Id}");
        }
        public virtual void Save()
        {
            model.Id = Id;
            model.Name = Name;
            model.Birthday = Birthday;
            model.PaymentData = PaymentData;
            model.DepartmentId = DepartmentId.Value;
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
    public class WorkerVM : EmployeeVM
    {
        public WorkerVM(Employee model) : base(model)
        {
            if (model.PaymentData == null)
                PaymentData = new WorkerPayment();
        }
    }
    public class InternVM : EmployeeVM
    {
        public InternVM(Employee model) : base(model)
        {
            if (model.PaymentData == null)
                PaymentData = new InternPayment();
        }
    }
}
