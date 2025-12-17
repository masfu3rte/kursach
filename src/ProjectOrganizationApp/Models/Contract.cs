using System;
using System.Collections.Generic;

namespace ProjectOrganizationApp.Models
{
    public class Contract
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Code { get; set; } = string.Empty;
        public string Customer { get; set; } = string.Empty;
        public Guid? ManagerId { get; set; }
        public ICollection<Guid> ProjectIds { get; set; } = new List<Guid>();
        public ICollection<Guid> EmployeeIds { get; set; } = new List<Guid>();
        public decimal TotalCost { get; set; }
        public DateTime? SignedAt { get; set; }
        public DateTime? CompletedAt { get; set; }

        public bool IsActive(DateTime? at = null)
        {
            var today = at ?? DateTime.Today;
            if (SignedAt is null)
            {
                return false;
            }

            if (SignedAt > today)
            {
                return false;
            }

            if (CompletedAt is not null && CompletedAt < today)
            {
                return false;
            }

            return true;
        }
    }
}
