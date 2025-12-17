using System;

namespace ProjectOrganizationApp.Models
{
    public class Equipment
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public bool IsShared { get; set; }
        public Guid? DepartmentOwnerId { get; set; }
        public Guid? AllocatedProjectId { get; set; }
    }
}
