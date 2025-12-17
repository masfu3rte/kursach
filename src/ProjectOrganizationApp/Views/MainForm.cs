using System;
using System.Collections.Generic;
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
        private bool _isInitialized;

        public MainForm()
        {
            InitializeComponent();
            _context = DataContext.Load();
            DemoDataSeeder.EnsureSeeded(_context);
            _reports = new ReportsService(_context);
            employeeSort.SelectedIndex = 0;
            contractSort.SelectedIndex = 0;
            projectSort.SelectedIndex = 0;
            _isInitialized = true;
            RefreshLists();
        }

        private void RefreshLists()
        {
            employeeList.Items.Clear();
            foreach (var employee in SortEmployees())
            {
                employeeList.Items.Add(new ListItem<Employee>
                {
                    Text = $"{employee.Position}: {employee.LastName} {employee.FirstName}, отдел: {GetDepartmentName(employee.DepartmentId)}, возраст: {employee.GetAge()}",
                    Value = employee
                });
            }

            departmentList.Items.Clear();
            foreach (var department in _context.Departments.OrderBy(d => d.Name))
            {
                var head = department.ManagerId is not null ? GetEmployeeName(department.ManagerId.Value) : "не назначен";
                departmentList.Items.Add($"{department.Name} (руководитель: {head}), сотрудников: {department.EmployeeIds.Count}");
            }

            contractList.Items.Clear();
            foreach (var contract in SortContracts())
            {
                var manager = contract.ManagerId is not null ? GetEmployeeName(contract.ManagerId.Value) : "не назначен";
                contractList.Items.Add(new ListItem<Contract>
                {
                    Text = $"{contract.Code} — {contract.Customer}, руководитель: {manager}, стоимость: {contract.TotalCost:C}",
                    Value = contract
                });
            }

            projectList.Items.Clear();
            foreach (var project in SortProjects())
            {
                var manager = project.ManagerId is not null ? GetEmployeeName(project.ManagerId.Value) : "не назначен";
                projectList.Items.Add(new ListItem<Project>
                {
                    Text = $"{project.Code} — {project.Name}, руководитель: {manager}, бюджет: {project.Budget:C}",
                    Value = project
                });
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

        private void AddEmployeeButton_Click(object sender, EventArgs e)
        {
            var employee = PromptForEmployee();
            if (employee is null)
            {
                return;
            }

            _context.Employees.Add(employee);
            _context.Departments.First(d => d.Id == employee.DepartmentId).EmployeeIds.Add(employee.Id);
            _context.Save();
            RefreshLists();
        }

        private void EditEmployeeButton_Click(object sender, EventArgs e)
        {
            if (employeeList.SelectedItem is not ListItem<Employee> selected)
            {
                MessageBox.Show("Выберите сотрудника для редактирования");
                return;
            }

            var updated = PromptForEmployee(selected.Value);
            if (updated is null)
            {
                return;
            }

            if (updated.DepartmentId != selected.Value.DepartmentId)
            {
                var previous = _context.Departments.First(d => d.Id == selected.Value.DepartmentId);
                previous.EmployeeIds.Remove(selected.Value.Id);
                var next = _context.Departments.First(d => d.Id == updated.DepartmentId);
                next.EmployeeIds.Add(selected.Value.Id);
            }

            selected.Value.FirstName = updated.FirstName;
            selected.Value.LastName = updated.LastName;
            selected.Value.BirthDate = updated.BirthDate;
            selected.Value.DepartmentId = updated.DepartmentId;
            _context.Save();
            RefreshLists();
        }

        private void DeleteEmployeeButton_Click(object sender, EventArgs e)
        {
            if (employeeList.SelectedItem is not ListItem<Employee> selected)
            {
                MessageBox.Show("Выберите сотрудника для удаления");
                return;
            }

            if (MessageBox.Show("Удалить выбранного сотрудника?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            _context.Departments.First(d => d.Id == selected.Value.DepartmentId).EmployeeIds.Remove(selected.Value.Id);
            foreach (var contract in _context.Contracts)
            {
                contract.EmployeeIds.Remove(selected.Value.Id);
                if (contract.ManagerId == selected.Value.Id)
                {
                    contract.ManagerId = null;
                }
            }

            foreach (var project in _context.Projects)
            {
                project.EmployeeIds.Remove(selected.Value.Id);
                if (project.ManagerId == selected.Value.Id)
                {
                    project.ManagerId = null;
                }
            }

            _context.Employees.Remove(selected.Value);
            _context.Save();
            RefreshLists();
        }

        private void AddContractButton_Click(object sender, EventArgs e)
        {
            var contract = PromptForContract();
            if (contract is null)
            {
                return;
            }

            _context.Contracts.Add(contract);
            _context.Save();
            RefreshLists();
        }

        private void EditContractButton_Click(object sender, EventArgs e)
        {
            if (contractList.SelectedItem is not ListItem<Contract> selected)
            {
                MessageBox.Show("Выберите договор для редактирования");
                return;
            }

            var updated = PromptForContract(selected.Value);
            if (updated is null)
            {
                return;
            }

            selected.Value.Code = updated.Code;
            selected.Value.Customer = updated.Customer;
            selected.Value.TotalCost = updated.TotalCost;
            selected.Value.SignedAt = updated.SignedAt;
            selected.Value.CompletedAt = updated.CompletedAt;
            _context.Save();
            RefreshLists();
        }

        private void DeleteContractButton_Click(object sender, EventArgs e)
        {
            if (contractList.SelectedItem is not ListItem<Contract> selected)
            {
                MessageBox.Show("Выберите договор для удаления");
                return;
            }

            if (MessageBox.Show("Удалить выбранный договор?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            foreach (var project in _context.Projects)
            {
                project.ContractIds.Remove(selected.Value.Id);
            }

            _context.Contracts.Remove(selected.Value);
            _context.Save();
            RefreshLists();
        }

        private void AddProjectButton_Click(object sender, EventArgs e)
        {
            var project = PromptForProject();
            if (project is null)
            {
                return;
            }

            _context.Projects.Add(project);
            _context.Save();
            RefreshLists();
        }

        private void EditProjectButton_Click(object sender, EventArgs e)
        {
            if (projectList.SelectedItem is not ListItem<Project> selected)
            {
                MessageBox.Show("Выберите проект для редактирования");
                return;
            }

            var updated = PromptForProject(selected.Value);
            if (updated is null)
            {
                return;
            }

            selected.Value.Code = updated.Code;
            selected.Value.Name = updated.Name;
            selected.Value.Budget = updated.Budget;
            selected.Value.StartDate = updated.StartDate;
            selected.Value.EndDate = updated.EndDate;
            _context.Save();
            RefreshLists();
        }

        private void DeleteProjectButton_Click(object sender, EventArgs e)
        {
            if (projectList.SelectedItem is not ListItem<Project> selected)
            {
                MessageBox.Show("Выберите проект для удаления");
                return;
            }

            if (MessageBox.Show("Удалить выбранный проект?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            foreach (var contract in _context.Contracts)
            {
                contract.ProjectIds.Remove(selected.Value.Id);
            }

            foreach (var equipment in _context.EquipmentPool)
            {
                if (equipment.AllocatedProjectId == selected.Value.Id)
                {
                    equipment.AllocatedProjectId = null;
                }
            }

            _context.Projects.Remove(selected.Value);
            _context.Save();
            RefreshLists();
        }

        private void EmployeeSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isInitialized)
            {
                RefreshLists();
            }
        }

        private void ContractSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isInitialized)
            {
                RefreshLists();
            }
        }

        private void ProjectSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isInitialized)
            {
                RefreshLists();
            }
        }

        private IEnumerable<Employee> SortEmployees()
        {
            return employeeSort.SelectedIndex switch
            {
                1 => _context.Employees.OrderBy(e => e.BirthDate),
                _ => _context.Employees.OrderBy(e => e.LastName).ThenBy(e => e.FirstName)
            };
        }

        private IEnumerable<Contract> SortContracts()
        {
            return contractSort.SelectedIndex switch
            {
                1 => _context.Contracts.OrderBy(c => c.SignedAt ?? DateTime.MaxValue),
                _ => _context.Contracts.OrderBy(c => c.Code)
            };
        }

        private IEnumerable<Project> SortProjects()
        {
            return projectSort.SelectedIndex switch
            {
                1 => _context.Projects.OrderBy(p => p.StartDate ?? DateTime.MaxValue),
                _ => _context.Projects.OrderBy(p => p.Name)
            };
        }

        private Employee? PromptForEmployee(Employee? existing = null)
        {
            var role = existing?.Position ?? ChooseRole();
            if (role is null)
            {
                return null;
            }

            var firstName = Prompt("Имя", existing?.FirstName ?? "");
            if (firstName is null)
            {
                return null;
            }

            var lastName = Prompt("Фамилия", existing?.LastName ?? "");
            if (lastName is null)
            {
                return null;
            }

            var birth = PromptDate("Дата рождения (гггг-мм-дд)", existing?.BirthDate ?? DateTime.Today.AddYears(-25));
            if (birth is null)
            {
                return null;
            }

            var department = ChooseFromList("Отдел", _context.Departments, d => d.Name, existing?.DepartmentId);
            if (department is null)
            {
                return null;
            }

            Employee target = existing ?? CreateEmployee(role);
            target.FirstName = firstName;
            target.LastName = lastName;
            target.BirthDate = birth.Value;
            target.DepartmentId = department.Id;
            return target;
        }

        private string? ChooseRole()
        {
            var roles = new[]
            {
                "Constructor",
                "Engineer",
                "Technician",
                "LaboratoryAssistant",
                "SupportStaff"
            };

            using var form = new Form
            {
                Width = 420,
                Height = 180,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Категория сотрудника",
                StartPosition = FormStartPosition.CenterParent,
                MinimizeBox = false,
                MaximizeBox = false
            };

            var combo = new ComboBox { Left = 10, Top = 20, Width = 380, DropDownStyle = ComboBoxStyle.DropDownList };
            combo.Items.AddRange(roles);
            combo.SelectedIndex = 0;
            var ok = new Button { Text = "ОК", Left = 230, Width = 75, Top = 100, DialogResult = DialogResult.OK };
            var cancel = new Button { Text = "Отмена", Left = 315, Width = 75, Top = 100, DialogResult = DialogResult.Cancel };
            form.Controls.AddRange(new Control[] { combo, ok, cancel });
            form.AcceptButton = ok;
            form.CancelButton = cancel;

            var result = form.ShowDialog(this);
            return result == DialogResult.OK ? combo.SelectedItem?.ToString() : null;
        }

        private static Employee CreateEmployee(string role)
        {
            return role switch
            {
                nameof(Constructor) => new Constructor(),
                nameof(Engineer) => new Engineer(),
                nameof(Technician) => new Technician(),
                nameof(LaboratoryAssistant) => new LaboratoryAssistant(),
                _ => new SupportStaff()
            };
        }

        private Contract? PromptForContract(Contract? existing = null)
        {
            var code = Prompt("Код договора", existing?.Code ?? "");
            if (code is null)
            {
                return null;
            }

            var customer = Prompt("Заказчик", existing?.Customer ?? "");
            if (customer is null)
            {
                return null;
            }

            var costText = Prompt("Стоимость", existing?.TotalCost.ToString() ?? "0");
            if (costText is null || !decimal.TryParse(costText, out var cost))
            {
                return null;
            }

            var signed = PromptDate("Дата подписания (гггг-мм-дд)", existing?.SignedAt ?? DateTime.Today);
            var completed = PromptDate("Дата завершения (гггг-мм-дд)", existing?.CompletedAt ?? DateTime.Today.AddMonths(1), true);

            var target = existing ?? new Contract();
            target.Code = code;
            target.Customer = customer;
            target.TotalCost = cost;
            target.SignedAt = signed;
            target.CompletedAt = completed;
            return target;
        }

        private Project? PromptForProject(Project? existing = null)
        {
            var code = Prompt("Код проекта", existing?.Code ?? "");
            if (code is null)
            {
                return null;
            }

            var name = Prompt("Название проекта", existing?.Name ?? "");
            if (name is null)
            {
                return null;
            }

            var budgetText = Prompt("Бюджет", existing?.Budget.ToString() ?? "0");
            if (budgetText is null || !decimal.TryParse(budgetText, out var budget))
            {
                return null;
            }

            var start = PromptDate("Дата начала (гггг-мм-дд)", existing?.StartDate ?? DateTime.Today, true);
            var end = PromptDate("Дата завершения (гггг-мм-дд)", existing?.EndDate ?? DateTime.Today.AddMonths(1), true);

            var target = existing ?? new Project();
            target.Code = code;
            target.Name = name;
            target.Budget = budget;
            target.StartDate = start;
            target.EndDate = end;
            return target;
        }

        private string? Prompt(string label, string initialValue)
        {
            using var form = new Form
            {
                Width = 420,
                Height = 160,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = label,
                StartPosition = FormStartPosition.CenterParent,
                MinimizeBox = false,
                MaximizeBox = false
            };

            var labelControl = new Label { Left = 10, Top = 10, Width = 380, Text = label };
            var inputBox = new TextBox { Left = 10, Top = 35, Width = 380, Text = initialValue };
            var confirmation = new Button { Text = "ОК", Left = 230, Width = 75, Top = 70, DialogResult = DialogResult.OK };
            var cancel = new Button { Text = "Отмена", Left = 315, Width = 75, Top = 70, DialogResult = DialogResult.Cancel };
            confirmation.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            cancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            form.Controls.AddRange(new Control[] { labelControl, inputBox, confirmation, cancel });
            form.AcceptButton = confirmation;
            form.CancelButton = cancel;

            var dialogResult = form.ShowDialog(this);
            return dialogResult == DialogResult.OK ? inputBox.Text : null;
        }

        private DateTime? PromptDate(string label, DateTime initialValue, bool allowEmpty = false)
        {
            var value = Prompt(label, initialValue.ToString("yyyy-MM-dd"));
            if (value is null)
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(value) && allowEmpty)
            {
                return null;
            }

            if (DateTime.TryParse(value, out var date))
            {
                return date;
            }

            MessageBox.Show("Не удалось разобрать дату");
            return null;
        }

        private T? ChooseFromList<T>(string label, IEnumerable<T> items, Func<T, string> selector, Guid? current = null) where T : class
        {
            using var form = new Form
            {
                Width = 420,
                Height = 200,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = label,
                StartPosition = FormStartPosition.CenterParent,
                MinimizeBox = false,
                MaximizeBox = false
            };

            var combo = new ComboBox { Left = 10, Top = 20, Width = 380, DropDownStyle = ComboBoxStyle.DropDownList };
            foreach (var item in items)
            {
                var listItem = new ListItem<T> { Text = selector(item), Value = item };
                combo.Items.Add(listItem);
                if (current is Guid guid)
                {
                    if (item is Department dep && dep.Id == guid)
                    {
                        combo.SelectedItem = listItem;
                    }
                }
            }

            if (combo.SelectedIndex < 0 && combo.Items.Count > 0)
            {
                combo.SelectedIndex = 0;
            }

            var ok = new Button { Text = "ОК", Left = 230, Width = 75, Top = 120, DialogResult = DialogResult.OK };
            var cancel = new Button { Text = "Отмена", Left = 315, Width = 75, Top = 120, DialogResult = DialogResult.Cancel };
            form.Controls.AddRange(new Control[] { combo, ok, cancel });
            form.AcceptButton = ok;
            form.CancelButton = cancel;

            var result = form.ShowDialog(this);
            return result == DialogResult.OK && combo.SelectedItem is ListItem<T> chosen ? chosen.Value : null;
        }

        private class ListItem<T>
        {
            public string Text { get; set; } = string.Empty;
            public T Value { get; set; } = default!;
            public override string ToString() => Text;
        }
    }
}
