using Skillbox.App.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Skillbox.App.Model
{
    public class Department : Observable, IEntity
    {
        public int Id { get; set; } = EntityManager.Count;
        public string Name { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public ISet<int> Employees { get; set; } = new HashSet<int>();
        public ISet<int> Departments { get; set; } = new HashSet<int>();

        public Department()
        {
            Debug.WriteLine($"Department '{Name}' is created {Id}");
            EntityManager.AllEntities.Departments.Add(this);
        }
    }
}
