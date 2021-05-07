using System;
using System.Runtime.Serialization;

namespace Skillbox.App.Model
{
    public enum PaymentType
    {
        PerHour, Contract, Percentage
    }
    public interface IPaymentData
    {
        PaymentType Type { get; }
        decimal TotalCompensation { get; }
    }
    public class InternPayment : IPaymentData
    {
        public decimal Salary { get; set; }

        public decimal TotalCompensation => Salary;

        public PaymentType Type => PaymentType.Contract;
    }
    public class WorkerPayment : IPaymentData
    {
        public decimal HoursLastMonth { get; set; }
        public decimal PayRate { get; set; }
        public decimal TotalCompensation => HoursLastMonth * PayRate;
        public PaymentType Type => PaymentType.PerHour;
    }
    public class ManagerPayment : IPaymentData
    {
        public const decimal ManagersMinSalary = 1300M;
        [IgnoreDataMember]
        public Func<decimal> SumSubordinatesSal = () => 0M;
        public decimal TotalCompensation => Salary;
        public PaymentType Type => PaymentType.Percentage;
        public decimal Salary
        {
            get
            {
                var sum = SumSubordinatesSal();
                return sum < ManagersMinSalary ? ManagersMinSalary : sum;
            }
        }
    }
}
