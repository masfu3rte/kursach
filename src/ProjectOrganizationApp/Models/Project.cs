using System;
using System.Collections.Generic;

namespace ProjectOrganizationApp.Models
{
    public class Project
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public Guid? ManagerId { get; set; }
        public ICollection<Guid> ContractIds { get; set; } = new List<Guid>();
        public ICollection<Guid> EmployeeIds { get; set; } = new List<Guid>();
        public ICollection<Guid> EquipmentIds { get; set; } = new List<Guid>();
        public ICollection<Subcontract> Subcontracts { get; set; } = new List<Subcontract>();
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal Budget { get; set; }

        public bool IsActive(DateTime? at = null)
        {
            var today = at ?? DateTime.Today;
            if (StartDate is null && EndDate is null)
            {
                return true;
            }

            if (StartDate is not null && StartDate > today)
            {
                return false;
            }

            if (EndDate is not null && EndDate < today)
            {
                return false;
            }

            return true;
        }
    }
}
