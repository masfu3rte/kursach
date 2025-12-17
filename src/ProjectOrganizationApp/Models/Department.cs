using System;
using System.Collections.Generic;

namespace ProjectOrganizationApp.Models
{
    public class Department
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public Guid? ManagerId { get; set; }
        public ICollection<Guid> EmployeeIds { get; set; } = new List<Guid>();
        public ICollection<Guid> EquipmentIds { get; set; } = new List<Guid>();
    }
}
