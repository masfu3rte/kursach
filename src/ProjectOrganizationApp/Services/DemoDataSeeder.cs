using System;
using System.Collections.Generic;
using System.Linq;
using ProjectOrganizationApp.Models;

namespace ProjectOrganizationApp.Services
{
    public static class DemoDataSeeder
    {
        public static void EnsureSeeded(DataContext context)
        {
            if (context.Departments.Any())
            {
                return;
            }

            var design = new Department { Name = "Конструкторский отдел" };
            var engineering = new Department { Name = "Инженерный отдел" };
            var lab = new Department { Name = "Лаборатория" };

            var employees = new List<Employee>
            {
                new Constructor
                {
                    FirstName = "Анна",
                    LastName = "Иванова",
                    BirthDate = new DateTime(1990, 4, 12),
                    HourlyRate = 20,
                    PatentCount = 3,
                    DepartmentId = design.Id,
                    Specializations = { "Механика", "Детали машин" },
                    IsDepartmentHead = true
                },
                new Engineer
                {
                    FirstName = "Петр",
                    LastName = "Петров",
                    BirthDate = new DateTime(1988, 9, 3),
                    HourlyRate = 18,
                    Certificates = { "Промышленная автоматизация" },
                    DepartmentId = engineering.Id
                },
                new Technician
                {
                    FirstName = "Елена",
                    LastName = "Сидорова",
                    BirthDate = new DateTime(1995, 2, 1),
                    HourlyRate = 12,
                    SupportedEquipmentModels = { "3D-принтер", "Станок ЧПУ" },
                    DepartmentId = engineering.Id
                },
                new LaboratoryAssistant
                {
                    FirstName = "Игорь",
                    LastName = "Федоров",
                    BirthDate = new DateTime(1997, 7, 22),
                    HourlyRate = 10,
                    Competencies = { "Испытания материалов" },
                    DepartmentId = lab.Id
                },
                new SupportStaff
                {
                    FirstName = "Мария",
                    LastName = "Кузнецова",
                    BirthDate = new DateTime(1992, 11, 8),
                    HourlyRate = 9,
                    ResponsibilityArea = "Документооборот",
                    DepartmentId = design.Id
                }
            };

            design.EmployeeIds = employees.Where(e => e.DepartmentId == design.Id).Select(e => e.Id).ToList();
            engineering.EmployeeIds = employees.Where(e => e.DepartmentId == engineering.Id).Select(e => e.Id).ToList();
            lab.EmployeeIds = employees.Where(e => e.DepartmentId == lab.Id).Select(e => e.Id).ToList();
            design.ManagerId = employees.First(e => e is Constructor c && c.IsDepartmentHead).Id;

            var equipment = new List<Equipment>
            {
                new Equipment { Name = "3D-принтер", Type = "Прототипирование", IsShared = true, DepartmentOwnerId = design.Id },
                new Equipment { Name = "Лазерный резак", Type = "Резка", IsShared = false, DepartmentOwnerId = engineering.Id },
                new Equipment { Name = "Климатическая камера", Type = "Испытания", IsShared = true, DepartmentOwnerId = lab.Id }
            };

            design.EquipmentIds = equipment.Where(e => e.DepartmentOwnerId == design.Id).Select(e => e.Id).ToList();
            engineering.EquipmentIds = equipment.Where(e => e.DepartmentOwnerId == engineering.Id).Select(e => e.Id).ToList();
            lab.EquipmentIds = equipment.Where(e => e.DepartmentOwnerId == lab.Id).Select(e => e.Id).ToList();

            var project = new Project
            {
                Name = "Проект А",
                Code = "PRJ-001",
                ManagerId = employees.OfType<Constructor>().First().Id,
                EmployeeIds = new List<Guid> { employees[0].Id, employees[1].Id, employees[2].Id },
                EquipmentIds = new List<Guid> { equipment[0].Id, equipment[1].Id },
                StartDate = DateTime.Today.AddDays(-30),
                EndDate = DateTime.Today.AddDays(60),
                Budget = 50000,
                Subcontracts = new List<Subcontract>
                {
                    new Subcontract
                    {
                        Contractor = "ООО Подряд",
                        Scope = "Разработка электроники",
                        Cost = 10000,
                        StartDate = DateTime.Today.AddDays(-15),
                        EndDate = DateTime.Today.AddDays(30)
                    }
                }
            };

            var contract = new Contract
            {
                Code = "CNT-2024-01",
                Customer = "АО Заказчик",
                ManagerId = employees.OfType<Engineer>().First().Id,
                ProjectIds = new List<Guid> { project.Id },
                EmployeeIds = new List<Guid> { employees[0].Id, employees[1].Id },
                SignedAt = DateTime.Today.AddDays(-45),
                CompletedAt = null,
                TotalCost = 80000
            };

            project.ContractIds.Add(contract.Id);

            context.Departments = new List<Department> { design, engineering, lab };
            context.Employees = employees;
            context.Projects = new List<Project> { project };
            context.Contracts = new List<Contract> { contract };
            context.EquipmentPool = equipment;
        }
    }
}
