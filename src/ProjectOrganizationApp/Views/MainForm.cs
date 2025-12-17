using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ProjectOrganizationApp.Models;
using ProjectOrganizationApp.Services;

namespace ProjectOrganizationApp.Views
{
    public partial class MainForm : Form
    {
        private readonly DataContext _context;
        private readonly ReportsService _reports;

        public MainForm()
        {
            InitializeComponent();
            _context = DataContext.Load();
            DemoDataSeeder.EnsureSeeded(_context);
            _reports = new ReportsService(_context);
            RefreshLists();
        }

        private void RefreshLists()
        {
            employeeList.Items.Clear();
            foreach (var employee in _context.Employees.OrderBy(e => e.LastName))
            {
                employeeList.Items.Add($"{employee.Position}: {employee.LastName} {employee.FirstName}, отдел: {GetDepartmentName(employee.DepartmentId)}, возраст: {employee.GetAge()}");
            }

            departmentList.Items.Clear();
            foreach (var department in _context.Departments.OrderBy(d => d.Name))
            {
                var head = department.ManagerId is not null ? GetEmployeeName(department.ManagerId.Value) : "не назначен";
                departmentList.Items.Add($"{department.Name} (руководитель: {head}), сотрудников: {department.EmployeeIds.Count}");
            }

            contractList.Items.Clear();
            foreach (var contract in _context.Contracts)
            {
                var manager = contract.ManagerId is not null ? GetEmployeeName(contract.ManagerId.Value) : "не назначен";
                contractList.Items.Add($"{contract.Code} — {contract.Customer}, руководитель: {manager}, стоимость: {contract.TotalCost:C}");
            }

            projectList.Items.Clear();
            foreach (var project in _context.Projects)
            {
                var manager = project.ManagerId is not null ? GetEmployeeName(project.ManagerId.Value) : "не назначен";
                projectList.Items.Add($"{project.Code} — {project.Name}, руководитель: {manager}, бюджет: {project.Budget:C}");
            }

            equipmentList.Items.Clear();
            foreach (var equipment in _context.EquipmentPool)
            {
                var owner = equipment.DepartmentOwnerId is not null ? GetDepartmentName(equipment.DepartmentOwnerId.Value) : "Общий фонд";
                var allocated = equipment.AllocatedProjectId is not null ? GetProjectName(equipment.AllocatedProjectId.Value) : "свободно";
                equipmentList.Items.Add($"{equipment.Name} ({equipment.Type}) — владелец: {owner}, используется: {allocated}");
            }

            _context.Save();
            BuildReport();
        }

        private string GetDepartmentName(Guid id) => _context.Departments.FirstOrDefault(d => d.Id == id)?.Name ?? "неизвестно";
        private string GetEmployeeName(Guid id)
        {
            var employee = _context.Employees.FirstOrDefault(e => e.Id == id);
            return employee is null ? "неизвестно" : $"{employee.LastName} {employee.FirstName}";
        }

        private string GetProjectName(Guid id)
        {
            return _context.Projects.FirstOrDefault(p => p.Id == id)?.Name ?? "неизвестно";
        }

        private void BuildReport()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Руководители отделов:");
            foreach (var department in _reports.GetDepartmentsWithHeads())
            {
                sb.AppendLine($" - {department.Name}: {GetEmployeeName(department.ManagerId!.Value)}");
            }

            sb.AppendLine();
            sb.AppendLine("Активные договоры в текущем месяце:");
            foreach (var contract in _reports.GetActiveContracts(DateTime.Today.AddMonths(-1), DateTime.Today.AddMonths(1)))
            {
                sb.AppendLine($" - {contract.Code}: {contract.Customer}, стоимость {contract.TotalCost:C}");
            }

            sb.AppendLine();
            sb.AppendLine("Активные проекты в текущем месяце:");
            foreach (var project in _reports.GetActiveProjects(DateTime.Today.AddMonths(-1), DateTime.Today.AddMonths(1)))
            {
                sb.AppendLine($" - {project.Code}: {project.Name}, бюджет {project.Budget:C}");
            }

            sb.AppendLine();
            sb.AppendLine("Использование оборудования:");
            foreach (var entry in _reports.GetEquipmentEfficiency())
            {
                sb.AppendLine($" - {entry.Equipment.Name}: объем работ {entry.Efficiency:C}");
            }

            sb.AppendLine();
            sb.AppendLine("Эффективность договоров:");
            foreach (var entry in _reports.GetContractEfficiency())
            {
                sb.AppendLine($" - {entry.Contract.Code}: {entry.Efficiency:C} в день");
            }

            reportOutput.Text = sb.ToString();
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            RefreshLists();
        }
    }
}
