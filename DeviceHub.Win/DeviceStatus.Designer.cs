namespace DeviceHub.Win
{
    partial class DeviceStatus
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            grpAuthInfo = new GroupBox();
            lblExpireTime = new Label();
            label3 = new Label();
            lblAuthStatus = new Label();
            label2 = new Label();
            lblAuthCode = new Label();
            label1 = new Label();
            dgvInstrumentItemMapping = new DataGridView();
            Column1 = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            Column3 = new DataGridViewTextBoxColumn();
            Column4 = new DataGridViewTextBoxColumn();
            Column5 = new DataGridViewTextBoxColumn();
            pagerInstrumentItemMapping = new DeviceHub.Win.DeviceHubControl.PagerControl();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            tabPage2 = new TabPage();
            tabPage3 = new TabPage();
            tabPage4 = new TabPage();
            grpAuthInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvInstrumentItemMapping).BeginInit();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            SuspendLayout();
            // 
            // grpAuthInfo
            // 
            grpAuthInfo.Controls.Add(lblExpireTime);
            grpAuthInfo.Controls.Add(label3);
            grpAuthInfo.Controls.Add(lblAuthStatus);
            grpAuthInfo.Controls.Add(label2);
            grpAuthInfo.Controls.Add(lblAuthCode);
            grpAuthInfo.Controls.Add(label1);
            grpAuthInfo.ForeColor = Color.DarkBlue;
            grpAuthInfo.Location = new Point(3, 6);
            grpAuthInfo.Name = "grpAuthInfo";
            grpAuthInfo.Size = new Size(250, 145);
            grpAuthInfo.TabIndex = 0;
            grpAuthInfo.TabStop = false;
            grpAuthInfo.Text = "授权信息";
            // 
            // lblExpireTime
            // 
            lblExpireTime.AutoSize = true;
            lblExpireTime.ForeColor = SystemColors.ControlText;
            lblExpireTime.Location = new Point(100, 113);
            lblExpireTime.Name = "lblExpireTime";
            lblExpireTime.Size = new Size(0, 17);
            lblExpireTime.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ForeColor = SystemColors.ControlText;
            label3.Location = new Point(20, 113);
            label3.Name = "label3";
            label3.Size = new Size(68, 17);
            label3.TabIndex = 4;
            label3.Text = "过期时间：";
            // 
            // lblAuthStatus
            // 
            lblAuthStatus.AutoSize = true;
            lblAuthStatus.ForeColor = Color.Green;
            lblAuthStatus.Location = new Point(100, 78);
            lblAuthStatus.Name = "lblAuthStatus";
            lblAuthStatus.Size = new Size(0, 17);
            lblAuthStatus.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = SystemColors.ControlText;
            label2.Location = new Point(20, 78);
            label2.Name = "label2";
            label2.Size = new Size(44, 17);
            label2.TabIndex = 2;
            label2.Text = "状态：";
            // 
            // lblAuthCode
            // 
            lblAuthCode.AutoSize = true;
            lblAuthCode.ForeColor = Color.Red;
            lblAuthCode.Location = new Point(100, 43);
            lblAuthCode.Name = "lblAuthCode";
            lblAuthCode.Size = new Size(0, 17);
            lblAuthCode.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = SystemColors.ControlText;
            label1.Location = new Point(20, 43);
            label1.Name = "label1";
            label1.Size = new Size(56, 17);
            label1.TabIndex = 0;
            label1.Text = "授权码：";
            // 
            // dgvInstrumentItemMapping
            // 
            dgvInstrumentItemMapping.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvInstrumentItemMapping.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvInstrumentItemMapping.Columns.AddRange(new DataGridViewColumn[] { Column1, Column2, Column3, Column4, Column5 });
            dgvInstrumentItemMapping.Location = new Point(259, 16);
            dgvInstrumentItemMapping.Name = "dgvInstrumentItemMapping";
            dgvInstrumentItemMapping.Size = new Size(1080, 527);
            dgvInstrumentItemMapping.TabIndex = 1;
            // 
            // Column1
            // 
            Column1.DataPropertyName = "InstrumentItemCode";
            Column1.HeaderText = "仪器项目编号";
            Column1.Name = "Column1";
            // 
            // Column2
            // 
            Column2.DataPropertyName = "InstrumentItemName";
            Column2.HeaderText = "仪器项目名";
            Column2.Name = "Column2";
            // 
            // Column3
            // 
            Column3.DataPropertyName = "LisItemCode";
            Column3.HeaderText = "LIS项目编号";
            Column3.Name = "Column3";
            // 
            // Column4
            // 
            Column4.DataPropertyName = "LisItemName";
            Column4.HeaderText = "LIS项目名";
            Column4.Name = "Column4";
            // 
            // Column5
            // 
            Column5.DataPropertyName = "Unit";
            Column5.HeaderText = "单位";
            Column5.Name = "Column5";
            // 
            // pagerInstrumentItemMapping
            // 
            pagerInstrumentItemMapping.AutoSize = true;
            pagerInstrumentItemMapping.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            pagerInstrumentItemMapping.Location = new Point(259, 549);
            pagerInstrumentItemMapping.MinimumSize = new Size(640, 32);
            pagerInstrumentItemMapping.Name = "pagerInstrumentItemMapping";
            pagerInstrumentItemMapping.Size = new Size(670, 33);
            pagerInstrumentItemMapping.TabIndex = 2;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(tabPage4);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1350, 621);
            tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(dgvInstrumentItemMapping);
            tabPage1.Controls.Add(grpAuthInfo);
            tabPage1.Controls.Add(pagerInstrumentItemMapping);
            tabPage1.Location = new Point(4, 26);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(1342, 591);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "LIS参数配置";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Location = new Point(4, 26);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(1342, 585);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "接收仪器数据队列";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            tabPage3.Location = new Point(4, 26);
            tabPage3.Name = "tabPage3";
            tabPage3.Size = new Size(1342, 585);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "发送仪器数据队列";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            tabPage4.Location = new Point(4, 26);
            tabPage4.Name = "tabPage4";
            tabPage4.Size = new Size(1342, 585);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "操作日志";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // DeviceStatus
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1350, 621);
            Controls.Add(tabControl1);
            Name = "DeviceStatus";
            Text = "DeviceStatus";
            Shown += DeviceStatus_Shown;
            grpAuthInfo.ResumeLayout(false);
            grpAuthInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvInstrumentItemMapping).EndInit();
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private Button button1;
        private Label label1;
        private Label lbllblMessage;
        private GroupBox grpAuthInfo;
        private DataGridView dgvInstrumentItemMapping;
        private DeviceHubControl.PagerControl pagerInstrumentItemMapping;
        private Label lblExpireTime;
        private Label label3;
        private Label lblAuthStatus;
        private Label label2;
        private Label lblAuthCode;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column3;
        private DataGridViewTextBoxColumn Column4;
        private DataGridViewTextBoxColumn Column5;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private TabPage tabPage4;
    }
}