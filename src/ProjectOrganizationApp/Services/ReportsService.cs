using System;
using System.Collections.Generic;
using System.Linq;
using ProjectOrganizationApp.Models;

namespace ProjectOrganizationApp.Services
{
    public class ReportsService
    {
        private readonly DataContext _context;

        public ReportsService(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<Employee> GetEmployeesByDepartment(Guid? departmentId = null)
        {
            return _context.Employees.Where(e => departmentId is null || e.DepartmentId == departmentId);
        }

        public IEnumerable<Employee> GetEmployeesByCategory<TEmployee>() where TEmployee : Employee
        {
            return _context.Employees.OfType<TEmployee>();
        }

        public IEnumerable<Employee> GetEmployeesByAge(int minAge, int maxAge)
        {
            return _context.Employees.Where(e => e.GetAge() >= minAge && e.GetAge() <= maxAge);
        }

        public IEnumerable<Department> GetDepartmentsWithHeads()
        {
            return _context.Departments.Where(d => d.ManagerId is not null);
        }

        public IEnumerable<Contract> GetActiveContracts(DateTime? from, DateTime? to)
        {
            return _context.Contracts.Where(c => IsInRange(c.SignedAt, c.CompletedAt, from, to));
        }

        public IEnumerable<Project> GetActiveProjects(DateTime? from, DateTime? to)
        {
            return _context.Projects.Where(p => IsInRange(p.StartDate, p.EndDate, from, to));
        }

        public IEnumerable<Project> GetProjectsForContract(Guid contractId)
        {
            return _context.Projects.Where(p => p.ContractIds.Contains(contractId));
        }

        public IEnumerable<Contract> GetContractsForProject(Guid projectId)
        {
            return _context.Contracts.Where(c => c.ProjectIds.Contains(projectId));
        }

        public decimal GetContractCost(Guid contractId, DateTime? from, DateTime? to)
        {
            var contract = _context.Contracts.FirstOrDefault(c => c.Id == contractId);
            if (contract is null)
            {
                return 0;
            }

            if (!IsInRange(contract.SignedAt, contract.CompletedAt, from, to))
            {
                return 0;
            }

            return contract.TotalCost;
        }

        public decimal GetProjectCost(Guid projectId, DateTime? from, DateTime? to)
        {
            var project = _context.Projects.FirstOrDefault(p => p.Id == projectId);
            if (project is null)
            {
                return 0;
            }

            if (!IsInRange(project.StartDate, project.EndDate, from, to))
            {
                return 0;
            }

            return project.Budget;
        }

        public IEnumerable<Equipment> GetEquipmentDistribution(DateTime? asOf = null)
        {
            return _context.EquipmentPool;
        }

        public IEnumerable<Project> GetProjectsUsingEquipment(Guid equipmentId)
        {
            return _context.Projects.Where(p => p.EquipmentIds.Contains(equipmentId));
        }

        public IEnumerable<(Project Project, Employee Employee)> GetEmployeeParticipation(Guid employeeId, DateTime? from, DateTime? to)
        {
            var projects = _context.Projects.Where(p => p.EmployeeIds.Contains(employeeId) && IsInRange(p.StartDate, p.EndDate, from, to));
            foreach (var project in projects)
            {
                var employee = _context.Employees.First(e => e.Id == employeeId);
                yield return (project, employee);
            }
        }

        public IEnumerable<Subcontract> GetSubcontractWork()
        {
            return _context.Projects.SelectMany(p => p.Subcontracts);
        }

        public IEnumerable<Employee> GetProjectTeam(Guid projectId)
        {
            return _context.Employees.Where(e => _context.Projects.Any(p => p.Id == projectId && p.EmployeeIds.Contains(e.Id)));
        }

        public IEnumerable<(Equipment Equipment, decimal Efficiency)> GetEquipmentEfficiency()
        {
            var efficiencies = new List<(Equipment, decimal)>();
            foreach (var equipment in _context.EquipmentPool)
            {
                var volume = _context.Projects.Where(p => p.EquipmentIds.Contains(equipment.Id)).Sum(p => p.Budget);
                efficiencies.Add((equipment, volume));
            }
            return efficiencies;
        }

        public IEnumerable<(Contract Contract, decimal Efficiency)> GetContractEfficiency()
        {
            var efficiencies = new List<(Contract, decimal)>();
            foreach (var contract in _context.Contracts)
            {
                var duration = (contract.CompletedAt ?? DateTime.Today) - (contract.SignedAt ?? DateTime.Today);
                var cost = contract.TotalCost;
                var efficiency = duration.TotalDays > 0 ? cost / (decimal)duration.TotalDays : cost;
                efficiencies.Add((contract, efficiency));
            }
            return efficiencies;
        }

        private static bool IsInRange(DateTime? start, DateTime? end, DateTime? from, DateTime? to)
        {
            if (from is null && to is null)
            {
                return true;
            }

            if (start is null && end is null)
            {
                return true;
            }

            var effectiveStart = start ?? DateTime.MinValue;
            var effectiveEnd = end ?? DateTime.MaxValue;

            if (from is not null && effectiveEnd < from)
            {
                return false;
            }

            if (to is not null && effectiveStart > to)
            {
                return false;
            }

            return true;
        }
    }
}
