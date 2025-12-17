using System;
using System.Collections.Generic;

namespace ProjectOrganizationApp.Models
{
    public abstract class Employee
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; } = DateTime.Now.AddYears(-25);
        public string Position => GetType().Name;
        public decimal HourlyRate { get; set; }
        public Guid DepartmentId { get; set; }
        public bool IsDepartmentHead { get; set; }

        public int GetAge()
        {
            var now = DateTime.Today;
            var age = now.Year - BirthDate.Year;
            if (BirthDate.Date > now.AddYears(-age))
            {
                age--;
            }
            return age;
        }
    }

    public class Constructor : Employee
    {
        public int PatentCount { get; set; }
        public ICollection<string> Specializations { get; set; } = new List<string>();
        public Guid? LeadingContractId { get; set; }
        public Guid? LeadingProjectId { get; set; }
    }

    public class Engineer : Employee
    {
        public ICollection<string> Certificates { get; set; } = new List<string>();
        public Guid? LeadingContractId { get; set; }
        public Guid? LeadingProjectId { get; set; }
    }

    public class Technician : Employee
    {
        public ICollection<string> SupportedEquipmentModels { get; set; } = new List<string>();
    }

    public class LaboratoryAssistant : Employee
    {
        public ICollection<string> Competencies { get; set; } = new List<string>();
    }

    public class SupportStaff : Employee
    {
        public string ResponsibilityArea { get; set; } = string.Empty;
    }
}
