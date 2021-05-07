using Newtonsoft.Json;
using Skillbox.App.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Skillbox.App.Model
{
    public class Employee : IEntity
    {
        public int Id { get; set; } = EntityManager.Count;
        public string Name { get; set; }
        public int Age => DateTime.Today.Year - Birthday.Year;
        public DateTime Birthday { get; set; }
        public int DepartmentId { get; set; }
        public string Position { get; set; }
        [JsonConverter(typeof(PaymentTypeConverter))]
        public IPaymentData PaymentData { get; set; }
        public ISet<int> Subordinates { get; set; } = new HashSet<int>();
        public Employee()
        {
            Debug.WriteLine($"Employee '{Name}' is created {Id}");
            EntityManager.AllEntities.Employees.Add(this);
        }
    }
}
