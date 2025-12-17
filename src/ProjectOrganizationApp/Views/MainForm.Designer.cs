namespace ProjectOrganizationApp.Views
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabEmployees = new System.Windows.Forms.TabPage();
            this.employeeToolbar = new System.Windows.Forms.FlowLayoutPanel();
            this.addEmployeeButton = new System.Windows.Forms.Button();
            this.editEmployeeButton = new System.Windows.Forms.Button();
            this.deleteEmployeeButton = new System.Windows.Forms.Button();
            this.employeeSortLabel = new System.Windows.Forms.Label();
            this.employeeSort = new System.Windows.Forms.ComboBox();
            this.employeeList = new System.Windows.Forms.ListBox();
            this.tabDepartments = new System.Windows.Forms.TabPage();
            this.departmentToolbar = new System.Windows.Forms.FlowLayoutPanel();
            this.addDepartmentButton = new System.Windows.Forms.Button();
            this.editDepartmentButton = new System.Windows.Forms.Button();
            this.deleteDepartmentButton = new System.Windows.Forms.Button();
            this.departmentList = new System.Windows.Forms.ListBox();
            this.tabContracts = new System.Windows.Forms.TabPage();
            this.contractToolbar = new System.Windows.Forms.FlowLayoutPanel();
            this.addContractButton = new System.Windows.Forms.Button();
            this.editContractButton = new System.Windows.Forms.Button();
            this.deleteContractButton = new System.Windows.Forms.Button();
            this.contractSortLabel = new System.Windows.Forms.Label();
            this.contractSort = new System.Windows.Forms.ComboBox();
            this.contractList = new System.Windows.Forms.ListBox();
            this.tabProjects = new System.Windows.Forms.TabPage();
            this.projectToolbar = new System.Windows.Forms.FlowLayoutPanel();
            this.addProjectButton = new System.Windows.Forms.Button();
            this.editProjectButton = new System.Windows.Forms.Button();
            this.deleteProjectButton = new System.Windows.Forms.Button();
            this.projectSortLabel = new System.Windows.Forms.Label();
            this.projectSort = new System.Windows.Forms.ComboBox();
            this.projectList = new System.Windows.Forms.ListBox();
            this.tabEquipment = new System.Windows.Forms.TabPage();
            this.equipmentList = new System.Windows.Forms.ListBox();
            this.tabReports = new System.Windows.Forms.TabPage();
            this.reportOutput = new System.Windows.Forms.TextBox();
            this.refreshButton = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tabEmployees.SuspendLayout();
            this.employeeToolbar.SuspendLayout();
            this.tabDepartments.SuspendLayout();
            this.departmentToolbar.SuspendLayout();
            this.tabContracts.SuspendLayout();
            this.contractToolbar.SuspendLayout();
            this.tabProjects.SuspendLayout();
            this.projectToolbar.SuspendLayout();
            this.tabEquipment.SuspendLayout();
            this.tabReports.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabEmployees);
            this.tabControl.Controls.Add(this.tabDepartments);
            this.tabControl.Controls.Add(this.tabContracts);
            this.tabControl.Controls.Add(this.tabProjects);
            this.tabControl.Controls.Add(this.tabEquipment);
            this.tabControl.Controls.Add(this.tabReports);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1184, 661);
            this.tabControl.TabIndex = 0;
            // 
            // tabEmployees
            // 
            this.tabEmployees.Controls.Add(this.employeeList);
            this.tabEmployees.Controls.Add(this.employeeToolbar);
            this.tabEmployees.Location = new System.Drawing.Point(4, 29);
            this.tabEmployees.Name = "tabEmployees";
            this.tabEmployees.Padding = new System.Windows.Forms.Padding(3);
            this.tabEmployees.Size = new System.Drawing.Size(1176, 628);
            this.tabEmployees.TabIndex = 0;
            this.tabEmployees.Text = "Сотрудники";
            this.tabEmployees.UseVisualStyleBackColor = true;
            // 
            // employeeToolbar
            // 
            this.employeeToolbar.Controls.Add(this.addEmployeeButton);
            this.employeeToolbar.Controls.Add(this.editEmployeeButton);
            this.employeeToolbar.Controls.Add(this.deleteEmployeeButton);
            this.employeeToolbar.Controls.Add(this.employeeSortLabel);
            this.employeeToolbar.Controls.Add(this.employeeSort);
            this.employeeToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.employeeToolbar.Location = new System.Drawing.Point(3, 3);
            this.employeeToolbar.Name = "employeeToolbar";
            this.employeeToolbar.Size = new System.Drawing.Size(1170, 43);
            this.employeeToolbar.TabIndex = 1;
            // 
            // addEmployeeButton
            // 
            this.addEmployeeButton.Location = new System.Drawing.Point(3, 3);
            this.addEmployeeButton.Name = "addEmployeeButton";
            this.addEmployeeButton.Size = new System.Drawing.Size(118, 34);
            this.addEmployeeButton.TabIndex = 0;
            this.addEmployeeButton.Text = "Добавить";
            this.addEmployeeButton.UseVisualStyleBackColor = true;
            this.addEmployeeButton.Click += new System.EventHandler(this.AddEmployeeButton_Click);
            // 
            // editEmployeeButton
            // 
            this.editEmployeeButton.Location = new System.Drawing.Point(127, 3);
            this.editEmployeeButton.Name = "editEmployeeButton";
            this.editEmployeeButton.Size = new System.Drawing.Size(118, 34);
            this.editEmployeeButton.TabIndex = 1;
            this.editEmployeeButton.Text = "Изменить";
            this.editEmployeeButton.UseVisualStyleBackColor = true;
            this.editEmployeeButton.Click += new System.EventHandler(this.EditEmployeeButton_Click);
            // 
            // deleteEmployeeButton
            // 
            this.deleteEmployeeButton.Location = new System.Drawing.Point(251, 3);
            this.deleteEmployeeButton.Name = "deleteEmployeeButton";
            this.deleteEmployeeButton.Size = new System.Drawing.Size(118, 34);
            this.deleteEmployeeButton.TabIndex = 2;
            this.deleteEmployeeButton.Text = "Удалить";
            this.deleteEmployeeButton.UseVisualStyleBackColor = true;
            this.deleteEmployeeButton.Click += new System.EventHandler(this.DeleteEmployeeButton_Click);
            // 
            // employeeSortLabel
            // 
            this.employeeSortLabel.AutoSize = true;
            this.employeeSortLabel.Location = new System.Drawing.Point(375, 9);
            this.employeeSortLabel.Margin = new System.Windows.Forms.Padding(3, 9, 3, 0);
            this.employeeSortLabel.Name = "employeeSortLabel";
            this.employeeSortLabel.Size = new System.Drawing.Size(99, 20);
            this.employeeSortLabel.TabIndex = 3;
            this.employeeSortLabel.Text = "Сортировка:";
            // 
            // employeeSort
            // 
            this.employeeSort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.employeeSort.FormattingEnabled = true;
            this.employeeSort.Items.AddRange(new object[] {
            "По фамилии",
            "По дате рождения"});
            this.employeeSort.Location = new System.Drawing.Point(480, 6);
            this.employeeSort.Name = "employeeSort";
            this.employeeSort.Size = new System.Drawing.Size(181, 28);
            this.employeeSort.TabIndex = 4;
            this.employeeSort.SelectedIndexChanged += new System.EventHandler(this.EmployeeSort_SelectedIndexChanged);
            // 
            // employeeList
            // 
            this.employeeList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.employeeList.FormattingEnabled = true;
            this.employeeList.ItemHeight = 20;
            this.employeeList.Location = new System.Drawing.Point(3, 46);
            this.employeeList.Name = "employeeList";
            this.employeeList.Size = new System.Drawing.Size(1170, 622);
            this.employeeList.TabIndex = 0;
            // 
            // tabDepartments
            // 
            this.tabDepartments.Controls.Add(this.departmentList);
            this.tabDepartments.Controls.Add(this.departmentToolbar);
            this.tabDepartments.Location = new System.Drawing.Point(4, 29);
            this.tabDepartments.Name = "tabDepartments";
            this.tabDepartments.Padding = new System.Windows.Forms.Padding(3);
            this.tabDepartments.Size = new System.Drawing.Size(1176, 628);
            this.tabDepartments.TabIndex = 1;
            this.tabDepartments.Text = "Отделы";
            this.tabDepartments.UseVisualStyleBackColor = true;
            //
            // departmentList
            //
            this.departmentList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.departmentList.FormattingEnabled = true;
            this.departmentList.ItemHeight = 20;
            this.departmentList.Location = new System.Drawing.Point(3, 46);
            this.departmentList.Name = "departmentList";
            this.departmentList.Size = new System.Drawing.Size(1170, 579);
            this.departmentList.TabIndex = 1;
            //
            // departmentToolbar
            //
            this.departmentToolbar.Controls.Add(this.addDepartmentButton);
            this.departmentToolbar.Controls.Add(this.editDepartmentButton);
            this.departmentToolbar.Controls.Add(this.deleteDepartmentButton);
            this.departmentToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.departmentToolbar.Location = new System.Drawing.Point(3, 3);
            this.departmentToolbar.Name = "departmentToolbar";
            this.departmentToolbar.Size = new System.Drawing.Size(1170, 43);
            this.departmentToolbar.TabIndex = 0;
            //
            // addDepartmentButton
            //
            this.addDepartmentButton.Location = new System.Drawing.Point(3, 3);
            this.addDepartmentButton.Name = "addDepartmentButton";
            this.addDepartmentButton.Size = new System.Drawing.Size(118, 34);
            this.addDepartmentButton.TabIndex = 0;
            this.addDepartmentButton.Text = "Добавить";
            this.addDepartmentButton.UseVisualStyleBackColor = true;
            this.addDepartmentButton.Click += new System.EventHandler(this.AddDepartmentButton_Click);
            //
            // editDepartmentButton
            //
            this.editDepartmentButton.Location = new System.Drawing.Point(127, 3);
            this.editDepartmentButton.Name = "editDepartmentButton";
            this.editDepartmentButton.Size = new System.Drawing.Size(118, 34);
            this.editDepartmentButton.TabIndex = 1;
            this.editDepartmentButton.Text = "Изменить";
            this.editDepartmentButton.UseVisualStyleBackColor = true;
            this.editDepartmentButton.Click += new System.EventHandler(this.EditDepartmentButton_Click);
            //
            // deleteDepartmentButton
            //
            this.deleteDepartmentButton.Location = new System.Drawing.Point(251, 3);
            this.deleteDepartmentButton.Name = "deleteDepartmentButton";
            this.deleteDepartmentButton.Size = new System.Drawing.Size(118, 34);
            this.deleteDepartmentButton.TabIndex = 2;
            this.deleteDepartmentButton.Text = "Удалить";
            this.deleteDepartmentButton.UseVisualStyleBackColor = true;
            this.deleteDepartmentButton.Click += new System.EventHandler(this.DeleteDepartmentButton_Click);
            //
            // tabContracts
            //
            this.tabContracts.Controls.Add(this.contractList);
            this.tabContracts.Controls.Add(this.contractToolbar);
            this.tabContracts.Location = new System.Drawing.Point(4, 29);
            this.tabContracts.Name = "tabContracts";
            this.tabContracts.Padding = new System.Windows.Forms.Padding(3);
            this.tabContracts.Size = new System.Drawing.Size(1176, 628);
            this.tabContracts.TabIndex = 2;
            this.tabContracts.Text = "Договоры";
            this.tabContracts.UseVisualStyleBackColor = true;
            // 
            // contractToolbar
            // 
            this.contractToolbar.Controls.Add(this.addContractButton);
            this.contractToolbar.Controls.Add(this.editContractButton);
            this.contractToolbar.Controls.Add(this.deleteContractButton);
            this.contractToolbar.Controls.Add(this.contractSortLabel);
            this.contractToolbar.Controls.Add(this.contractSort);
            this.contractToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.contractToolbar.Location = new System.Drawing.Point(3, 3);
            this.contractToolbar.Name = "contractToolbar";
            this.contractToolbar.Size = new System.Drawing.Size(1170, 43);
            this.contractToolbar.TabIndex = 1;
            // 
            // addContractButton
            // 
            this.addContractButton.Location = new System.Drawing.Point(3, 3);
            this.addContractButton.Name = "addContractButton";
            this.addContractButton.Size = new System.Drawing.Size(118, 34);
            this.addContractButton.TabIndex = 0;
            this.addContractButton.Text = "Добавить";
            this.addContractButton.UseVisualStyleBackColor = true;
            this.addContractButton.Click += new System.EventHandler(this.AddContractButton_Click);
            // 
            // editContractButton
            // 
            this.editContractButton.Location = new System.Drawing.Point(127, 3);
            this.editContractButton.Name = "editContractButton";
            this.editContractButton.Size = new System.Drawing.Size(118, 34);
            this.editContractButton.TabIndex = 1;
            this.editContractButton.Text = "Изменить";
            this.editContractButton.UseVisualStyleBackColor = true;
            this.editContractButton.Click += new System.EventHandler(this.EditContractButton_Click);
            // 
            // deleteContractButton
            // 
            this.deleteContractButton.Location = new System.Drawing.Point(251, 3);
            this.deleteContractButton.Name = "deleteContractButton";
            this.deleteContractButton.Size = new System.Drawing.Size(118, 34);
            this.deleteContractButton.TabIndex = 2;
            this.deleteContractButton.Text = "Удалить";
            this.deleteContractButton.UseVisualStyleBackColor = true;
            this.deleteContractButton.Click += new System.EventHandler(this.DeleteContractButton_Click);
            // 
            // contractSortLabel
            // 
            this.contractSortLabel.AutoSize = true;
            this.contractSortLabel.Location = new System.Drawing.Point(375, 9);
            this.contractSortLabel.Margin = new System.Windows.Forms.Padding(3, 9, 3, 0);
            this.contractSortLabel.Name = "contractSortLabel";
            this.contractSortLabel.Size = new System.Drawing.Size(99, 20);
            this.contractSortLabel.TabIndex = 3;
            this.contractSortLabel.Text = "Сортировка:";
            // 
            // contractSort
            // 
            this.contractSort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.contractSort.FormattingEnabled = true;
            this.contractSort.Items.AddRange(new object[] {
            "По коду",
            "По дате подписания"});
            this.contractSort.Location = new System.Drawing.Point(480, 6);
            this.contractSort.Name = "contractSort";
            this.contractSort.Size = new System.Drawing.Size(181, 28);
            this.contractSort.TabIndex = 4;
            this.contractSort.SelectedIndexChanged += new System.EventHandler(this.ContractSort_SelectedIndexChanged);
            // 
            // contractList
            // 
            this.contractList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contractList.FormattingEnabled = true;
            this.contractList.ItemHeight = 20;
            this.contractList.Location = new System.Drawing.Point(3, 46);
            this.contractList.Name = "contractList";
            this.contractList.Size = new System.Drawing.Size(1170, 622);
            this.contractList.TabIndex = 0;
            // 
            // tabProjects
            // 
            this.tabProjects.Controls.Add(this.projectList);
            this.tabProjects.Controls.Add(this.projectToolbar);
            this.tabProjects.Location = new System.Drawing.Point(4, 29);
            this.tabProjects.Name = "tabProjects";
            this.tabProjects.Padding = new System.Windows.Forms.Padding(3);
            this.tabProjects.Size = new System.Drawing.Size(1176, 628);
            this.tabProjects.TabIndex = 3;
            this.tabProjects.Text = "Проекты";
            this.tabProjects.UseVisualStyleBackColor = true;
            // 
            // projectToolbar
            // 
            this.projectToolbar.Controls.Add(this.addProjectButton);
            this.projectToolbar.Controls.Add(this.editProjectButton);
            this.projectToolbar.Controls.Add(this.deleteProjectButton);
            this.projectToolbar.Controls.Add(this.projectSortLabel);
            this.projectToolbar.Controls.Add(this.projectSort);
            this.projectToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.projectToolbar.Location = new System.Drawing.Point(3, 3);
            this.projectToolbar.Name = "projectToolbar";
            this.projectToolbar.Size = new System.Drawing.Size(1170, 43);
            this.projectToolbar.TabIndex = 1;
            // 
            // addProjectButton
            // 
            this.addProjectButton.Location = new System.Drawing.Point(3, 3);
            this.addProjectButton.Name = "addProjectButton";
            this.addProjectButton.Size = new System.Drawing.Size(118, 34);
            this.addProjectButton.TabIndex = 0;
            this.addProjectButton.Text = "Добавить";
            this.addProjectButton.UseVisualStyleBackColor = true;
            this.addProjectButton.Click += new System.EventHandler(this.AddProjectButton_Click);
            // 
            // editProjectButton
            // 
            this.editProjectButton.Location = new System.Drawing.Point(127, 3);
            this.editProjectButton.Name = "editProjectButton";
            this.editProjectButton.Size = new System.Drawing.Size(118, 34);
            this.editProjectButton.TabIndex = 1;
            this.editProjectButton.Text = "Изменить";
            this.editProjectButton.UseVisualStyleBackColor = true;
            this.editProjectButton.Click += new System.EventHandler(this.EditProjectButton_Click);
            // 
            // deleteProjectButton
            // 
            this.deleteProjectButton.Location = new System.Drawing.Point(251, 3);
            this.deleteProjectButton.Name = "deleteProjectButton";
            this.deleteProjectButton.Size = new System.Drawing.Size(118, 34);
            this.deleteProjectButton.TabIndex = 2;
            this.deleteProjectButton.Text = "Удалить";
            this.deleteProjectButton.UseVisualStyleBackColor = true;
            this.deleteProjectButton.Click += new System.EventHandler(this.DeleteProjectButton_Click);
            // 
            // projectSortLabel
            // 
            this.projectSortLabel.AutoSize = true;
            this.projectSortLabel.Location = new System.Drawing.Point(375, 9);
            this.projectSortLabel.Margin = new System.Windows.Forms.Padding(3, 9, 3, 0);
            this.projectSortLabel.Name = "projectSortLabel";
            this.projectSortLabel.Size = new System.Drawing.Size(99, 20);
            this.projectSortLabel.TabIndex = 3;
            this.projectSortLabel.Text = "Сортировка:";
            // 
            // projectSort
            // 
            this.projectSort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.projectSort.FormattingEnabled = true;
            this.projectSort.Items.AddRange(new object[] {
            "По названию",
            "По дате начала"});
            this.projectSort.Location = new System.Drawing.Point(480, 6);
            this.projectSort.Name = "projectSort";
            this.projectSort.Size = new System.Drawing.Size(181, 28);
            this.projectSort.TabIndex = 4;
            this.projectSort.SelectedIndexChanged += new System.EventHandler(this.ProjectSort_SelectedIndexChanged);
            // 
            // projectList
            // 
            this.projectList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.projectList.FormattingEnabled = true;
            this.projectList.ItemHeight = 20;
            this.projectList.Location = new System.Drawing.Point(3, 46);
            this.projectList.Name = "projectList";
            this.projectList.Size = new System.Drawing.Size(1170, 622);
            this.projectList.TabIndex = 0;
            // 
            // tabEquipment
            // 
            this.tabEquipment.Controls.Add(this.equipmentList);
            this.tabEquipment.Location = new System.Drawing.Point(4, 29);
            this.tabEquipment.Name = "tabEquipment";
            this.tabEquipment.Padding = new System.Windows.Forms.Padding(3);
            this.tabEquipment.Size = new System.Drawing.Size(1176, 628);
            this.tabEquipment.TabIndex = 4;
            this.tabEquipment.Text = "Оборудование";
            this.tabEquipment.UseVisualStyleBackColor = true;
            // 
            // equipmentList
            // 
            this.equipmentList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.equipmentList.FormattingEnabled = true;
            this.equipmentList.ItemHeight = 20;
            this.equipmentList.Location = new System.Drawing.Point(3, 3);
            this.equipmentList.Name = "equipmentList";
            this.equipmentList.Size = new System.Drawing.Size(1170, 622);
            this.equipmentList.TabIndex = 0;
            // 
            // tabReports
            // 
            this.tabReports.Controls.Add(this.reportOutput);
            this.tabReports.Controls.Add(this.refreshButton);
            this.tabReports.Location = new System.Drawing.Point(4, 29);
            this.tabReports.Name = "tabReports";
            this.tabReports.Padding = new System.Windows.Forms.Padding(3);
            this.tabReports.Size = new System.Drawing.Size(1176, 628);
            this.tabReports.TabIndex = 5;
            this.tabReports.Text = "Отчеты";
            this.tabReports.UseVisualStyleBackColor = true;
            // 
            // reportOutput
            // 
            this.reportOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reportOutput.Location = new System.Drawing.Point(3, 43);
            this.reportOutput.Multiline = true;
            this.reportOutput.Name = "reportOutput";
            this.reportOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.reportOutput.Size = new System.Drawing.Size(1170, 582);
            this.reportOutput.TabIndex = 1;
            // 
            // refreshButton
            // 
            this.refreshButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.refreshButton.Location = new System.Drawing.Point(3, 3);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(1170, 40);
            this.refreshButton.TabIndex = 0;
            this.refreshButton.Text = "Обновить отчеты";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 661);
            this.Controls.Add(this.tabControl);
            this.Name = "MainForm";
            this.Text = "Проектная организация";
            this.tabControl.ResumeLayout(false);
            this.tabEmployees.ResumeLayout(false);
            this.employeeToolbar.ResumeLayout(false);
            this.employeeToolbar.PerformLayout();
            this.tabDepartments.ResumeLayout(false);
            this.departmentToolbar.ResumeLayout(false);
            this.tabDepartments.PerformLayout();
            this.tabContracts.ResumeLayout(false);
            this.contractToolbar.ResumeLayout(false);
            this.contractToolbar.PerformLayout();
            this.tabProjects.ResumeLayout(false);
            this.projectToolbar.ResumeLayout(false);
            this.projectToolbar.PerformLayout();
            this.tabEquipment.ResumeLayout(false);
            this.tabReports.ResumeLayout(false);
            this.tabReports.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabEmployees;
        private System.Windows.Forms.ListBox employeeList;
        private System.Windows.Forms.TabPage tabDepartments;
        private System.Windows.Forms.ListBox departmentList;
        private System.Windows.Forms.FlowLayoutPanel departmentToolbar;
        private System.Windows.Forms.Button addDepartmentButton;
        private System.Windows.Forms.Button editDepartmentButton;
        private System.Windows.Forms.Button deleteDepartmentButton;
        private System.Windows.Forms.TabPage tabContracts;
        private System.Windows.Forms.ListBox contractList;
        private System.Windows.Forms.TabPage tabProjects;
        private System.Windows.Forms.ListBox projectList;
        private System.Windows.Forms.TabPage tabEquipment;
        private System.Windows.Forms.ListBox equipmentList;
        private System.Windows.Forms.TabPage tabReports;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.TextBox reportOutput;
        private System.Windows.Forms.FlowLayoutPanel employeeToolbar;
        private System.Windows.Forms.Button addEmployeeButton;
        private System.Windows.Forms.Button editEmployeeButton;
        private System.Windows.Forms.Button deleteEmployeeButton;
        private System.Windows.Forms.Label employeeSortLabel;
        private System.Windows.Forms.ComboBox employeeSort;
        private System.Windows.Forms.FlowLayoutPanel contractToolbar;
        private System.Windows.Forms.Button addContractButton;
        private System.Windows.Forms.Button editContractButton;
        private System.Windows.Forms.Button deleteContractButton;
        private System.Windows.Forms.Label contractSortLabel;
        private System.Windows.Forms.ComboBox contractSort;
        private System.Windows.Forms.FlowLayoutPanel projectToolbar;
        private System.Windows.Forms.Button addProjectButton;
        private System.Windows.Forms.Button editProjectButton;
        private System.Windows.Forms.Button deleteProjectButton;
        private System.Windows.Forms.Label projectSortLabel;
        private System.Windows.Forms.ComboBox projectSort;
    }
}
