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
            this.employeeList = new System.Windows.Forms.ListBox();
            this.tabDepartments = new System.Windows.Forms.TabPage();
            this.departmentList = new System.Windows.Forms.ListBox();
            this.tabContracts = new System.Windows.Forms.TabPage();
            this.contractList = new System.Windows.Forms.ListBox();
            this.tabProjects = new System.Windows.Forms.TabPage();
            this.projectList = new System.Windows.Forms.ListBox();
            this.tabEquipment = new System.Windows.Forms.TabPage();
            this.equipmentList = new System.Windows.Forms.ListBox();
            this.tabReports = new System.Windows.Forms.TabPage();
            this.reportOutput = new System.Windows.Forms.TextBox();
            this.refreshButton = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tabEmployees.SuspendLayout();
            this.tabDepartments.SuspendLayout();
            this.tabContracts.SuspendLayout();
            this.tabProjects.SuspendLayout();
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
            this.tabEmployees.Location = new System.Drawing.Point(4, 29);
            this.tabEmployees.Name = "tabEmployees";
            this.tabEmployees.Padding = new System.Windows.Forms.Padding(3);
            this.tabEmployees.Size = new System.Drawing.Size(1176, 628);
            this.tabEmployees.TabIndex = 0;
            this.tabEmployees.Text = "Сотрудники";
            this.tabEmployees.UseVisualStyleBackColor = true;
            // 
            // employeeList
            // 
            this.employeeList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.employeeList.FormattingEnabled = true;
            this.employeeList.ItemHeight = 20;
            this.employeeList.Location = new System.Drawing.Point(3, 3);
            this.employeeList.Name = "employeeList";
            this.employeeList.Size = new System.Drawing.Size(1170, 622);
            this.employeeList.TabIndex = 0;
            // 
            // tabDepartments
            // 
            this.tabDepartments.Controls.Add(this.departmentList);
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
            this.departmentList.Location = new System.Drawing.Point(3, 3);
            this.departmentList.Name = "departmentList";
            this.departmentList.Size = new System.Drawing.Size(1170, 622);
            this.departmentList.TabIndex = 0;
            // 
            // tabContracts
            // 
            this.tabContracts.Controls.Add(this.contractList);
            this.tabContracts.Location = new System.Drawing.Point(4, 29);
            this.tabContracts.Name = "tabContracts";
            this.tabContracts.Padding = new System.Windows.Forms.Padding(3);
            this.tabContracts.Size = new System.Drawing.Size(1176, 628);
            this.tabContracts.TabIndex = 2;
            this.tabContracts.Text = "Договоры";
            this.tabContracts.UseVisualStyleBackColor = true;
            // 
            // contractList
            // 
            this.contractList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contractList.FormattingEnabled = true;
            this.contractList.ItemHeight = 20;
            this.contractList.Location = new System.Drawing.Point(3, 3);
            this.contractList.Name = "contractList";
            this.contractList.Size = new System.Drawing.Size(1170, 622);
            this.contractList.TabIndex = 0;
            // 
            // tabProjects
            // 
            this.tabProjects.Controls.Add(this.projectList);
            this.tabProjects.Location = new System.Drawing.Point(4, 29);
            this.tabProjects.Name = "tabProjects";
            this.tabProjects.Padding = new System.Windows.Forms.Padding(3);
            this.tabProjects.Size = new System.Drawing.Size(1176, 628);
            this.tabProjects.TabIndex = 3;
            this.tabProjects.Text = "Проекты";
            this.tabProjects.UseVisualStyleBackColor = true;
            // 
            // projectList
            // 
            this.projectList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.projectList.FormattingEnabled = true;
            this.projectList.ItemHeight = 20;
            this.projectList.Location = new System.Drawing.Point(3, 3);
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
            this.tabDepartments.ResumeLayout(false);
            this.tabContracts.ResumeLayout(false);
            this.tabProjects.ResumeLayout(false);
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
        private System.Windows.Forms.TabPage tabContracts;
        private System.Windows.Forms.ListBox contractList;
        private System.Windows.Forms.TabPage tabProjects;
        private System.Windows.Forms.ListBox projectList;
        private System.Windows.Forms.TabPage tabEquipment;
        private System.Windows.Forms.ListBox equipmentList;
        private System.Windows.Forms.TabPage tabReports;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.TextBox reportOutput;
    }
}
