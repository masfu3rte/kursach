using System;

namespace ProjectOrganizationApp.Models
{
    public class Subcontract
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Contractor { get; set; } = string.Empty;
        public string Scope { get; set; } = string.Empty;
        public decimal Cost { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
