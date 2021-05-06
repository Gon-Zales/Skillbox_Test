using Newtonsoft.Json;
using Skillbox.App.Model;
using Skillbox.App.ViewModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Skillbox.App.Tools
{
    public static class EntityManager
    {
        private const string fileName = @".\organization.json";
        public static int Count => AllEntities.Employees.Count + AllEntities.Departments.Count + 1;

        public static OrganizationVM Organization { get; } = new OrganizationVM();
        public static DataBase AllEntities { get; private set; } = new DataBase();
        public static Dictionary<int, DepartmentVM> AllDepartmentVMs { get; } = new Dictionary<int, DepartmentVM>();
        public static Dictionary<int, EmployeeVM> AllEmployeeVMs { get; } = new Dictionary<int, EmployeeVM>();

        public static EmployeeVM GetEmployeeVM(int id)
        {
            if (!AllEmployeeVMs.ContainsKey(id))
            {
                Employee model = AllEntities.Employees.Single(x => x.Id == id);
                Organization.Employees.Add(new EmployeeVM(model));
            }
            return AllEmployeeVMs[id];
        }
        public static DepartmentVM GetDepartmentVM(int id)
        {
            if (!AllDepartmentVMs.ContainsKey(id))
            {
                Department model = AllEntities.Departments.Single(x => x.Id == id);
                Organization.AddDepartment(new DepartmentVM(model));
            }
            return AllDepartmentVMs[id];
        }

        public static IEnumerable<DepartmentVM> GetAllSubDepartments(DepartmentVM department)
        {
            return department.Departments.SelectMany(d => GetAllSubDepartments(d));
        }
        public static IEnumerable<DepartmentVM> GetAllDepartments()
        {
            return Organization.Departments.SelectMany(d => GetAllSubDepartments(d));
        }
        public static IEnumerable<EmployeeVM> GetAllEmployees()
        {
            var employees = Organization.Employees.ToList();
            var workers = GetAllDepartments().SelectMany(d => d.Employees);
            employees.AddRange(workers);
            return employees;
        }

        public static void Save()
        {
            foreach (var employee in AllEmployeeVMs.Values)
            {
                employee.Save();
            }
            foreach (var department in AllDepartmentVMs.Values)
            {
                department.Save();
            }
            using (var stream = File.CreateText(fileName))
            {
                string json = JsonConvert.SerializeObject(AllEntities);
                stream.Write(json);
            }
        }

        public static void Load()
        {
            ClearAllData();

            using (var r = new StreamReader(fileName))
            {
                string json = r.ReadToEnd();
                AllEntities = JsonConvert.DeserializeObject<DataBase>(json);
            }
            var subdeps = AllEntities.Departments.SelectMany(d => d.Departments);
            foreach (var e in AllEntities.Departments.Where(d => !subdeps.Contains(d.Id)))
                GetDepartmentVM(e.Id);

            foreach (var e in AllEntities.Employees.Where(d => d.DepartmentId == 0))
                GetEmployeeVM(e.Id);
        }

        private static void ClearAllData()
        {
            Organization.Employees.Clear();
            Organization.Departments.Clear();
            AllEntities.Employees.Clear();
            AllEntities.Departments.Clear();
            AllDepartmentVMs.Clear();
            AllEmployeeVMs.Clear();
        }
    }
}
