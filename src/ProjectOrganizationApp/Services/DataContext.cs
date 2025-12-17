using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using ProjectOrganizationApp.Models;

namespace ProjectOrganizationApp.Services
{
    public class DataContext
    {
        private const string DataFile = "organization-data.json";
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
        public ICollection<Department> Departments { get; set; } = new List<Department>();
        public ICollection<Contract> Contracts { get; set; } = new List<Contract>();
        public ICollection<Project> Projects { get; set; } = new List<Project>();
        public ICollection<Equipment> EquipmentPool { get; set; } = new List<Equipment>();

        public static DataContext Load()
        {
            if (!File.Exists(DataFile))
            {
                return new DataContext();
            }

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true,
                Converters = { new EmployeeJsonConverter() }
            };

            var json = File.ReadAllText(DataFile);
            var context = JsonSerializer.Deserialize<DataContext>(json, options);
            return context ?? new DataContext();
        }

        public void Save()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters = { new EmployeeJsonConverter() }
            };

            var json = JsonSerializer.Serialize(this, options);
            File.WriteAllText(DataFile, json);
        }
    }
}
