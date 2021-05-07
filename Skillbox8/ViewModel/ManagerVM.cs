using Microsoft.EntityFrameworkCore.ChangeTracking;
using Skillbox.App.Model;
using Skillbox.App.Tools;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Skillbox.App.ViewModel
{
    public class ManagerVM : EmployeeVM
    {
        public ObservableHashSet<EmployeeVM> Subordinates { get; } = new ObservableHashSet<EmployeeVM>();
        public ManagerPayment Payment
        {
            get => (ManagerPayment)PaymentData;
            set
            {
                value.SumSubordinatesSal = () => GetAllSubs(this).Sum(s => s.PaymentData.TotalCompensation);
                PaymentData = value;
                OnPropertyChanged();
            }
        }
        private IEnumerable<EmployeeVM> GetAllSubs(EmployeeVM employee)
        {
            if (employee is ManagerVM manager)
                return manager.Subordinates.SelectMany(x => GetAllSubs(x));
            return new List<EmployeeVM> { employee };
        }
        public ManagerVM(Employee model) : base(model)
        {
            if (model.PaymentData == null)
                Payment = new ManagerPayment();
            Payment = (ManagerPayment)PaymentData;
            Subordinates.CollectionChanged += Subordinates_CollectionChanged;
            foreach (var sub in model.Subordinates)
            {
                Subordinates.Add(EntityManager.GetEmployeeVM(sub));
            }
        }

        private void Subordinates_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
                foreach (EmployeeVM old in e.OldItems)
                {
                    if (old.Boss == this)
                        old.Boss = null;
                }
            if (e.NewItems != null)
                foreach (EmployeeVM new_ in e.NewItems)
                {
                    if (new_.Boss != this)
                        new_.Boss = this;
                }
            OnPropertyChanged(nameof(PaymentData));
        }

        public override void Save()
        {
            base.Save();
            model.Subordinates = Subordinates.Select(x => x.Id).ToHashSet();
        }
    }
}
