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
        private static readonly (string Key, string Label)[] RoleOptions =
        {
            (nameof(Constructor), "Конструктор"),
            (nameof(Engineer), "Инженер"),
            (nameof(Technician), "Техник"),
            (nameof(LaboratoryAssistant), "Лаборант"),
            (nameof(SupportStaff), "Вспомогательный персонал")
        };

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
                    Text = $"{GetRoleLabel(employee.Position)}: {employee.LastName} {employee.FirstName}, отдел: {GetDepartmentName(employee.DepartmentId)}, возраст: {employee.GetAge()}",
                    Value = employee
                });
            }

            departmentList.Items.Clear();
            foreach (var department in _context.Departments.OrderBy(d => d.Name))
            {
                var head = department.ManagerId is not null ? GetEmployeeName(department.ManagerId.Value) : "не назначен";
                departmentList.Items.Add(new ListItem<Department>
                {
                    Text = $"{department.Name} (руководитель: {head}), сотрудников: {department.EmployeeIds.Count}",
                    Value = department
                });
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
                equipmentList.Items.Add(new ListItem<Equipment>
                {
                    Text = $"{equipment.Name} ({equipment.Type}) — владелец: {owner}, используется: {allocated}",
                    Value = equipment
                });
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

        private string GetRoleLabel(string roleKey)
        {
            return RoleOptions.FirstOrDefault(r => r.Key == roleKey).Label ?? roleKey;
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

            var original = selected.Value;
            var previousDepartmentId = original.DepartmentId;

            var updated = PromptForEmployee(original);
            if (updated is null)
            {
                return;
            }

            if (updated.DepartmentId != previousDepartmentId)
            {
                var previous = _context.Departments.First(d => d.Id == previousDepartmentId);
                previous.EmployeeIds.Remove(original.Id);
                var next = _context.Departments.First(d => d.Id == updated.DepartmentId);
                next.EmployeeIds.Add(original.Id);
            }

            var managedDepartment = _context.Departments.FirstOrDefault(d => d.ManagerId == original.Id);
            if (managedDepartment is not null && managedDepartment.Id != updated.DepartmentId)
            {
                managedDepartment.ManagerId = null;
            }

            if (!ReferenceEquals(updated, original))
            {
                ReplaceEmployee(original, updated);
                selected.Value = updated;
            }

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

            foreach (var department in _context.Departments)
            {
                if (department.ManagerId == selected.Value.Id)
                {
                    department.ManagerId = null;
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
            selected.Value.ManagerId = updated.ManagerId;
            EnsureManagerLinked(selected.Value.EmployeeIds, updated.ManagerId);
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
            selected.Value.ManagerId = updated.ManagerId;
            EnsureManagerLinked(selected.Value.EmployeeIds, updated.ManagerId);
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

        private void AddDepartmentButton_Click(object sender, EventArgs e)
        {
            var result = PromptForDepartment();
            if (result is null)
            {
                return;
            }

            _context.Departments.Add(result);

            if (result.ManagerId is Guid managerId)
            {
                var manager = _context.Employees.FirstOrDefault(e => e.Id == managerId);
                if (manager is not null)
                {
                    MoveEmployeeToDepartment(manager, result.Id);
                    result.EmployeeIds.Add(manager.Id);
                }
            }

            _context.Save();
            RefreshLists();
        }

        private void EditDepartmentButton_Click(object sender, EventArgs e)
        {
            if (departmentList.SelectedItem is not ListItem<Department> selected)
            {
                MessageBox.Show("Выберите отдел для редактирования");
                return;
            }

            var updated = PromptForDepartment(selected.Value);
            if (updated is null)
            {
                return;
            }

            var previousManagerId = selected.Value.ManagerId;
            selected.Value.Name = updated.Name;
            selected.Value.ManagerId = updated.ManagerId;

            if (updated.ManagerId is Guid managerId)
            {
                var manager = _context.Employees.FirstOrDefault(e => e.Id == managerId);
                if (manager is not null)
                {
                    MoveEmployeeToDepartment(manager, selected.Value.Id);
                    if (!selected.Value.EmployeeIds.Contains(manager.Id))
                    {
                        selected.Value.EmployeeIds.Add(manager.Id);
                    }
                }
            }

            if (previousManagerId is Guid previousId && updated.ManagerId != previousId)
            {
                var previous = _context.Employees.FirstOrDefault(e => e.Id == previousId);
                if (previous is not null && previous.DepartmentId != selected.Value.Id)
                {
                    selected.Value.EmployeeIds.Remove(previous.Id);
                }
            }

            _context.Save();
            RefreshLists();
        }

        private void DeleteDepartmentButton_Click(object sender, EventArgs e)
        {
            if (departmentList.SelectedItem is not ListItem<Department> selected)
            {
                MessageBox.Show("Выберите отдел для удаления");
                return;
            }

            if (selected.Value.EmployeeIds.Any())
            {
                MessageBox.Show("Нельзя удалить отдел с сотрудниками. Переведите сотрудников в другие отделы.");
                return;
            }

            if (_context.EquipmentPool.Any(e => e.DepartmentOwnerId == selected.Value.Id))
            {
                MessageBox.Show("Нельзя удалить отдел, за которым закреплено оборудование.");
                return;
            }

            if (MessageBox.Show("Удалить выбранный отдел?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            _context.Departments.Remove(selected.Value);
            _context.Save();
            RefreshLists();
        }

        private void AddEquipmentButton_Click(object sender, EventArgs e)
        {
            var equipment = PromptForEquipment();
            if (equipment is null)
            {
                return;
            }

            _context.EquipmentPool.Add(equipment);
            UpdateDepartmentEquipmentLinks(null, equipment);
            UpdateProjectEquipmentLinks(null, equipment);
            _context.Save();
            RefreshLists();
        }

        private void EditEquipmentButton_Click(object sender, EventArgs e)
        {
            if (equipmentList.SelectedItem is not ListItem<Equipment> selected)
            {
                MessageBox.Show("Выберите оборудование для редактирования");
                return;
            }

            var previousDepartmentId = selected.Value.DepartmentOwnerId;
            var previousProjectId = selected.Value.AllocatedProjectId;

            var updated = PromptForEquipment(selected.Value);
            if (updated is null)
            {
                return;
            }

            UpdateDepartmentEquipmentLinks(previousDepartmentId, selected.Value);
            UpdateProjectEquipmentLinks(previousProjectId, selected.Value);
            _context.Save();
            RefreshLists();
        }

        private void DeleteEquipmentButton_Click(object sender, EventArgs e)
        {
            if (equipmentList.SelectedItem is not ListItem<Equipment> selected)
            {
                MessageBox.Show("Выберите оборудование для удаления");
                return;
            }

            if (MessageBox.Show("Удалить выбранное оборудование?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            UpdateDepartmentEquipmentLinks(selected.Value.DepartmentOwnerId, selected.Value);
            UpdateProjectEquipmentLinks(selected.Value.AllocatedProjectId, selected.Value);
            _context.EquipmentPool.Remove(selected.Value);
            _context.Save();
            RefreshLists();
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
            using var form = CreateDialog("Сотрудник", 520, 340);

            var table = BuildTableLayout(5);
            var roleLabel = new Label { Text = "Категория", AutoSize = true };
            var roleCombo = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            foreach (var option in RoleOptions)
            {
                var item = new ListItem<string> { Text = option.Label, Value = option.Key };
                roleCombo.Items.Add(item);
                if (existing?.Position == option.Key)
                {
                    roleCombo.SelectedItem = item;
                }
            }

            if (roleCombo.SelectedIndex < 0 && roleCombo.Items.Count > 0)
            {
                roleCombo.SelectedIndex = 0;
            }

            var firstNameBox = new TextBox { Text = existing?.FirstName ?? string.Empty };
            var lastNameBox = new TextBox { Text = existing?.LastName ?? string.Empty };
            var birthPicker = new DateTimePicker { Value = existing?.BirthDate ?? DateTime.Today.AddYears(-25) };

            var departmentCombo = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            foreach (var department in _context.Departments.OrderBy(d => d.Name))
            {
                var listItem = new ListItem<Department> { Text = department.Name, Value = department };
                departmentCombo.Items.Add(listItem);
                if (existing?.DepartmentId == department.Id)
                {
                    departmentCombo.SelectedItem = listItem;
                }
            }

            if (departmentCombo.SelectedIndex < 0 && departmentCombo.Items.Count > 0)
            {
                departmentCombo.SelectedIndex = 0;
            }

            table.Controls.Add(roleLabel, 0, 0);
            table.Controls.Add(roleCombo, 1, 0);
            table.Controls.Add(new Label { Text = "Имя", AutoSize = true }, 0, 1);
            table.Controls.Add(firstNameBox, 1, 1);
            table.Controls.Add(new Label { Text = "Фамилия", AutoSize = true }, 0, 2);
            table.Controls.Add(lastNameBox, 1, 2);
            table.Controls.Add(new Label { Text = "Дата рождения", AutoSize = true }, 0, 3);
            table.Controls.Add(birthPicker, 1, 3);
            table.Controls.Add(new Label { Text = "Отдел", AutoSize = true }, 0, 4);
            table.Controls.Add(departmentCombo, 1, 4);

            var (ok, cancel) = AddDialogButtons(form);
            ok.Click += (_, _) =>
            {
                if (string.IsNullOrWhiteSpace(firstNameBox.Text) || string.IsNullOrWhiteSpace(lastNameBox.Text) || departmentCombo.SelectedItem is null)
                {
                    MessageBox.Show("Заполните все поля");
                    return;
                }

                form.DialogResult = DialogResult.OK;
                form.Close();
            };

            form.Controls.Add(table);
            form.AcceptButton = ok;
            form.CancelButton = cancel;

            if (form.ShowDialog(this) != DialogResult.OK)
            {
                return null;
            }

            var role = roleCombo.SelectedItem is ListItem<string> selectedRole ? selectedRole.Value : existing?.Position ?? RoleOptions.First().Key;
            var target = existing ?? CreateEmployee(role);
            if (existing is not null && existing.Position != role)
            {
                target = CreateEmployee(role, existing.Id);
                target.HourlyRate = existing.HourlyRate;
                target.IsDepartmentHead = existing.IsDepartmentHead;
            }

            target.FirstName = firstNameBox.Text.Trim();
            target.LastName = lastNameBox.Text.Trim();
            target.BirthDate = birthPicker.Value.Date;
            if (departmentCombo.SelectedItem is ListItem<Department> selectedDepartment)
            {
                target.DepartmentId = selectedDepartment.Value.Id;
            }

            return target;
        }

        private Contract? PromptForContract(Contract? existing = null)
        {
            using var form = CreateDialog("Договор", 520, 360);
            var table = BuildTableLayout(6);

            var codeBox = new TextBox { Text = existing?.Code ?? string.Empty };
            var customerBox = new TextBox { Text = existing?.Customer ?? string.Empty };
            var costBox = new TextBox { Text = existing?.TotalCost.ToString() ?? "0" };
            var signedPicker = new DateTimePicker { Value = existing?.SignedAt ?? DateTime.Today, ShowCheckBox = true, Checked = existing?.SignedAt is not null };
            var completedPicker = new DateTimePicker { Value = existing?.CompletedAt ?? DateTime.Today.AddMonths(1), ShowCheckBox = true, Checked = existing?.CompletedAt is not null };
            var managerCombo = BuildManagerCombo(existing?.ManagerId);

            table.Controls.Add(new Label { Text = "Код", AutoSize = true }, 0, 0);
            table.Controls.Add(codeBox, 1, 0);
            table.Controls.Add(new Label { Text = "Заказчик", AutoSize = true }, 0, 1);
            table.Controls.Add(customerBox, 1, 1);
            table.Controls.Add(new Label { Text = "Стоимость", AutoSize = true }, 0, 2);
            table.Controls.Add(costBox, 1, 2);
            table.Controls.Add(new Label { Text = "Дата подписания", AutoSize = true }, 0, 3);
            table.Controls.Add(signedPicker, 1, 3);
            table.Controls.Add(new Label { Text = "Дата завершения", AutoSize = true }, 0, 4);
            table.Controls.Add(completedPicker, 1, 4);
            table.Controls.Add(new Label { Text = "Руководитель", AutoSize = true }, 0, 5);
            table.Controls.Add(managerCombo, 1, 5);

            var (ok, cancel) = AddDialogButtons(form);
            ok.Click += (_, _) =>
            {
                if (string.IsNullOrWhiteSpace(codeBox.Text) || string.IsNullOrWhiteSpace(customerBox.Text) || !decimal.TryParse(costBox.Text, out _))
                {
                    MessageBox.Show("Проверьте код, заказчика и сумму");
                    return;
                }

                form.DialogResult = DialogResult.OK;
                form.Close();
            };

            form.Controls.Add(table);
            form.AcceptButton = ok;
            form.CancelButton = cancel;

            if (form.ShowDialog(this) != DialogResult.OK)
            {
                return null;
            }

            var target = existing ?? new Contract();
            target.Code = codeBox.Text.Trim();
            target.Customer = customerBox.Text.Trim();
            target.TotalCost = decimal.Parse(costBox.Text);
            target.SignedAt = signedPicker.Checked ? signedPicker.Value.Date : null;
            target.CompletedAt = completedPicker.Checked ? completedPicker.Value.Date : null;
            target.ManagerId = GetSelectedManagerId(managerCombo);
            EnsureManagerLinked(target.EmployeeIds, target.ManagerId);
            return target;
        }

        private Project? PromptForProject(Project? existing = null)
        {
            using var form = CreateDialog("Проект", 520, 360);
            var table = BuildTableLayout(6);

            var codeBox = new TextBox { Text = existing?.Code ?? string.Empty };
            var nameBox = new TextBox { Text = existing?.Name ?? string.Empty };
            var budgetBox = new TextBox { Text = existing?.Budget.ToString() ?? "0" };
            var startPicker = new DateTimePicker { Value = existing?.StartDate ?? DateTime.Today, ShowCheckBox = true, Checked = existing?.StartDate is not null };
            var endPicker = new DateTimePicker { Value = existing?.EndDate ?? DateTime.Today.AddMonths(1), ShowCheckBox = true, Checked = existing?.EndDate is not null };
            var managerCombo = BuildManagerCombo(existing?.ManagerId);

            table.Controls.Add(new Label { Text = "Код", AutoSize = true }, 0, 0);
            table.Controls.Add(codeBox, 1, 0);
            table.Controls.Add(new Label { Text = "Название", AutoSize = true }, 0, 1);
            table.Controls.Add(nameBox, 1, 1);
            table.Controls.Add(new Label { Text = "Бюджет", AutoSize = true }, 0, 2);
            table.Controls.Add(budgetBox, 1, 2);
            table.Controls.Add(new Label { Text = "Дата начала", AutoSize = true }, 0, 3);
            table.Controls.Add(startPicker, 1, 3);
            table.Controls.Add(new Label { Text = "Дата завершения", AutoSize = true }, 0, 4);
            table.Controls.Add(endPicker, 1, 4);
            table.Controls.Add(new Label { Text = "Руководитель", AutoSize = true }, 0, 5);
            table.Controls.Add(managerCombo, 1, 5);

            var (ok, cancel) = AddDialogButtons(form);
            ok.Click += (_, _) =>
            {
                if (string.IsNullOrWhiteSpace(codeBox.Text) || string.IsNullOrWhiteSpace(nameBox.Text) || !decimal.TryParse(budgetBox.Text, out _))
                {
                    MessageBox.Show("Проверьте код, название и бюджет");
                    return;
                }

                form.DialogResult = DialogResult.OK;
                form.Close();
            };

            form.Controls.Add(table);
            form.AcceptButton = ok;
            form.CancelButton = cancel;

            if (form.ShowDialog(this) != DialogResult.OK)
            {
                return null;
            }

            var target = existing ?? new Project();
            target.Code = codeBox.Text.Trim();
            target.Name = nameBox.Text.Trim();
            target.Budget = decimal.Parse(budgetBox.Text);
            target.StartDate = startPicker.Checked ? startPicker.Value.Date : null;
            target.EndDate = endPicker.Checked ? endPicker.Value.Date : null;
            target.ManagerId = GetSelectedManagerId(managerCombo);
            EnsureManagerLinked(target.EmployeeIds, target.ManagerId);
            return target;
        }

        private Department? PromptForDepartment(Department? existing = null)
        {
            using var form = CreateDialog("Отдел", 520, 260);
            var table = BuildTableLayout(3);

            var nameBox = new TextBox { Text = existing?.Name ?? string.Empty };
            var managerCombo = BuildManagerCombo(existing?.ManagerId, allowEmpty: true, "Без руководителя");

            table.Controls.Add(new Label { Text = "Название", AutoSize = true }, 0, 0);
            table.Controls.Add(nameBox, 1, 0);
            table.Controls.Add(new Label { Text = "Руководитель", AutoSize = true }, 0, 1);
            table.Controls.Add(managerCombo, 1, 1);

            var (ok, cancel) = AddDialogButtons(form);
            ok.Click += (_, _) =>
            {
                if (string.IsNullOrWhiteSpace(nameBox.Text))
                {
                    MessageBox.Show("Введите название отдела");
                    return;
                }

                form.DialogResult = DialogResult.OK;
                form.Close();
            };

            form.Controls.Add(table);
            form.AcceptButton = ok;
            form.CancelButton = cancel;

            if (form.ShowDialog(this) != DialogResult.OK)
            {
                return null;
            }

            var target = existing ?? new Department();
            target.Name = nameBox.Text.Trim();
            target.ManagerId = GetSelectedManagerId(managerCombo);
            return target;
        }

        private Equipment? PromptForEquipment(Equipment? existing = null)
        {
            using var form = CreateDialog("Оборудование", 520, 320);
            var table = BuildTableLayout(5);

            var nameBox = new TextBox { Text = existing?.Name ?? string.Empty };
            var typeBox = new TextBox { Text = existing?.Type ?? string.Empty };
            var sharedCheck = new CheckBox { Text = "Общее оборудование", Checked = existing?.IsShared ?? true };

            var departmentCombo = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            departmentCombo.Items.Add(new ListItem<Department> { Text = "Общий фонд", Value = null });
            foreach (var department in _context.Departments.OrderBy(d => d.Name))
            {
                var item = new ListItem<Department> { Text = department.Name, Value = department };
                departmentCombo.Items.Add(item);
                if (existing?.DepartmentOwnerId == department.Id)
                {
                    departmentCombo.SelectedItem = item;
                }
            }

            if (departmentCombo.SelectedIndex < 0 && departmentCombo.Items.Count > 0)
            {
                departmentCombo.SelectedIndex = 0;
            }

            var projectCombo = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            projectCombo.Items.Add(new ListItem<Project> { Text = "Не назначено", Value = null });
            foreach (var project in _context.Projects.OrderBy(p => p.Name))
            {
                var item = new ListItem<Project> { Text = $"{project.Code} — {project.Name}", Value = project };
                projectCombo.Items.Add(item);
                if (existing?.AllocatedProjectId == project.Id)
                {
                    projectCombo.SelectedItem = item;
                }
            }

            if (projectCombo.SelectedIndex < 0 && projectCombo.Items.Count > 0)
            {
                projectCombo.SelectedIndex = 0;
            }

            table.Controls.Add(new Label { Text = "Название", AutoSize = true }, 0, 0);
            table.Controls.Add(nameBox, 1, 0);
            table.Controls.Add(new Label { Text = "Тип", AutoSize = true }, 0, 1);
            table.Controls.Add(typeBox, 1, 1);
            table.Controls.Add(new Label { Text = "Закреплено за", AutoSize = true }, 0, 2);
            table.Controls.Add(departmentCombo, 1, 2);
            table.Controls.Add(new Label { Text = "Назначено на проект", AutoSize = true }, 0, 3);
            table.Controls.Add(projectCombo, 1, 3);
            table.Controls.Add(sharedCheck, 1, 4);

            var (ok, cancel) = AddDialogButtons(form);
            ok.Click += (_, _) =>
            {
                if (string.IsNullOrWhiteSpace(nameBox.Text) || string.IsNullOrWhiteSpace(typeBox.Text))
                {
                    MessageBox.Show("Укажите название и тип оборудования");
                    return;
                }

                form.DialogResult = DialogResult.OK;
                form.Close();
            };

            form.Controls.Add(table);
            form.AcceptButton = ok;
            form.CancelButton = cancel;

            if (form.ShowDialog(this) != DialogResult.OK)
            {
                return null;
            }

            var target = existing ?? new Equipment();
            target.Name = nameBox.Text.Trim();
            target.Type = typeBox.Text.Trim();
            target.IsShared = sharedCheck.Checked;
            target.DepartmentOwnerId = departmentCombo.SelectedItem is ListItem<Department> department && department.Value is not null ? department.Value.Id : null;
            target.AllocatedProjectId = projectCombo.SelectedItem is ListItem<Project> project && project.Value is not null ? project.Value.Id : null;
            return target;
        }

        private Guid? GetSelectedManagerId(ComboBox combo)
        {
            if (combo.SelectedItem is ListItem<Employee> manager && manager.Value is not null)
            {
                return manager.Value.Id;
            }

            return null;
        }

        private ComboBox BuildManagerCombo(Guid? current, bool allowEmpty = true, string emptyLabel = "Не выбран")
        {
            var combo = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            if (allowEmpty)
            {
                combo.Items.Add(new ListItem<Employee> { Text = emptyLabel, Value = null! });
            }

            foreach (var employee in _context.Employees.OrderBy(e => e.LastName).ThenBy(e => e.FirstName))
            {
                var roleLabel = GetRoleLabel(employee.Position);
                var listItem = new ListItem<Employee> { Text = $"{employee.LastName} {employee.FirstName} ({roleLabel})", Value = employee };
                combo.Items.Add(listItem);
                if (employee.Id == current)
                {
                    combo.SelectedItem = listItem;
                }
            }

            if (combo.SelectedIndex < 0 && combo.Items.Count > 0)
            {
                combo.SelectedIndex = 0;
            }

            return combo;
        }

        private static (Button ok, Button cancel) AddDialogButtons(Form form)
        {
            var ok = new Button { Text = "ОК", DialogResult = DialogResult.OK, Anchor = AnchorStyles.Bottom | AnchorStyles.Right, Width = 90, Height = 32 };
            var cancel = new Button { Text = "Отмена", DialogResult = DialogResult.Cancel, Anchor = AnchorStyles.Bottom | AnchorStyles.Right, Width = 90, Height = 32 };

            ok.Top = form.ClientSize.Height - 50;
            cancel.Top = form.ClientSize.Height - 50;
            cancel.Left = form.ClientSize.Width - 110;
            ok.Left = form.ClientSize.Width - 210;

            form.Controls.Add(ok);
            form.Controls.Add(cancel);
            return (ok, cancel);
        }

        private static Form CreateDialog(string title, int width, int height)
        {
            return new Form
            {
                Width = width,
                Height = height,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = title,
                StartPosition = FormStartPosition.CenterParent,
                MinimizeBox = false,
                MaximizeBox = false
            };
        }

        private static TableLayoutPanel BuildTableLayout(int rows)
        {
            var table = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                ColumnCount = 2,
                RowCount = rows,
                AutoSize = true,
                Padding = new Padding(10),
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));
            for (var i = 0; i < rows; i++)
            {
                table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            }

            return table;
        }

        private void MoveEmployeeToDepartment(Employee employee, Guid departmentId)
        {
            if (employee.DepartmentId == departmentId)
            {
                return;
            }

            var current = _context.Departments.FirstOrDefault(d => d.Id == employee.DepartmentId);
            current?.EmployeeIds.Remove(employee.Id);
            var next = _context.Departments.FirstOrDefault(d => d.Id == departmentId);
            if (next is not null && !next.EmployeeIds.Contains(employee.Id))
            {
                next.EmployeeIds.Add(employee.Id);
            }

            employee.DepartmentId = departmentId;
        }

        private void EnsureManagerLinked(ICollection<Guid> employeeIds, Guid? managerId)
        {
            if (managerId is Guid id && !employeeIds.Contains(id))
            {
                employeeIds.Add(id);
            }
        }

        private void UpdateDepartmentEquipmentLinks(Guid? previousDepartmentId, Equipment equipment)
        {
            if (previousDepartmentId is Guid previousId)
            {
                var previousDepartment = _context.Departments.FirstOrDefault(d => d.Id == previousId);
                previousDepartment?.EquipmentIds.Remove(equipment.Id);
            }

            if (equipment.DepartmentOwnerId is Guid currentId)
            {
                var currentDepartment = _context.Departments.FirstOrDefault(d => d.Id == currentId);
                if (currentDepartment is not null && !currentDepartment.EquipmentIds.Contains(equipment.Id))
                {
                    currentDepartment.EquipmentIds.Add(equipment.Id);
                }
            }
        }

        private void UpdateProjectEquipmentLinks(Guid? previousProjectId, Equipment equipment)
        {
            if (previousProjectId is Guid previousId)
            {
                var previousProject = _context.Projects.FirstOrDefault(p => p.Id == previousId);
                previousProject?.EquipmentIds.Remove(equipment.Id);
            }

            if (equipment.AllocatedProjectId is Guid currentId)
            {
                var currentProject = _context.Projects.FirstOrDefault(p => p.Id == currentId);
                if (currentProject is not null && !currentProject.EquipmentIds.Contains(equipment.Id))
                {
                    currentProject.EquipmentIds.Add(equipment.Id);
                }
            }
        }

        private void ReplaceEmployee(Employee original, Employee replacement)
        {
            if (_context.Employees is IList<Employee> list)
            {
                var index = list.IndexOf(original);
                if (index >= 0)
                {
                    list[index] = replacement;
                    return;
                }
            }

            _context.Employees.Remove(original);
            _context.Employees.Add(replacement);
        }

        private static Employee CreateEmployee(string role, Guid? id = null)
        {
            return role switch
            {
                nameof(Constructor) => new Constructor { Id = id ?? Guid.NewGuid() },
                nameof(Engineer) => new Engineer { Id = id ?? Guid.NewGuid() },
                nameof(Technician) => new Technician { Id = id ?? Guid.NewGuid() },
                nameof(LaboratoryAssistant) => new LaboratoryAssistant { Id = id ?? Guid.NewGuid() },
                _ => new SupportStaff { Id = id ?? Guid.NewGuid() }
            };
        }

        private class ListItem<T>
        {
            public string Text { get; set; } = string.Empty;
            public T? Value { get; set; }
            public override string ToString() => Text;
        }
    }
}
