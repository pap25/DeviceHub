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
            dateTimePicker1 = new DateTimePicker();
            pagerControl1 = new DeviceHub.Win.DeviceHubControl.PagerControl();
            label9 = new Label();
            label8 = new Label();
            textBox5 = new TextBox();
            textBox3 = new TextBox();
            label5 = new Label();
            comboBox4 = new ComboBox();
            textBox1 = new TextBox();
            label4 = new Label();
            button2 = new Button();
            label6 = new Label();
            comboBox1 = new ComboBox();
            dataGridView1 = new DataGridView();
            Column8 = new DataGridViewCheckBoxColumn();
            状态 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
            Column6 = new DataGridViewTextBoxColumn();
            tabPage3 = new TabPage();
            tabPage4 = new TabPage();
            grpAuthInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvInstrumentItemMapping).BeginInit();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
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
            pagerInstrumentItemMapping.PageSize = 20;
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
            tabPage2.Controls.Add(dateTimePicker1);
            tabPage2.Controls.Add(pagerControl1);
            tabPage2.Controls.Add(label9);
            tabPage2.Controls.Add(label8);
            tabPage2.Controls.Add(textBox5);
            tabPage2.Controls.Add(textBox3);
            tabPage2.Controls.Add(label5);
            tabPage2.Controls.Add(comboBox4);
            tabPage2.Controls.Add(textBox1);
            tabPage2.Controls.Add(label4);
            tabPage2.Controls.Add(button2);
            tabPage2.Controls.Add(label6);
            tabPage2.Controls.Add(comboBox1);
            tabPage2.Controls.Add(dataGridView1);
            tabPage2.Location = new Point(4, 26);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(1342, 591);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "接收仪器数据队列";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.Location = new Point(615, 15);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(200, 23);
            dateTimePicker1.TabIndex = 26;
            // 
            // pagerControl1
            // 
            pagerControl1.AutoSize = true;
            pagerControl1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            pagerControl1.Location = new Point(3, 555);
            pagerControl1.MinimumSize = new Size(640, 32);
            pagerControl1.Name = "pagerControl1";
            pagerControl1.Size = new Size(670, 33);
            pagerControl1.TabIndex = 25;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(817, 44);
            label9.Name = "label9";
            label9.Size = new Size(20, 17);
            label9.TabIndex = 24;
            label9.Text = "至";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(655, 44);
            label8.Name = "label8";
            label8.Size = new Size(56, 17);
            label8.TabIndex = 23;
            label8.Text = "创建时间";
            // 
            // textBox5
            // 
            textBox5.Location = new Point(837, 41);
            textBox5.Name = "textBox5";
            textBox5.Size = new Size(96, 23);
            textBox5.TabIndex = 22;
            textBox5.Text = "2016-06-22";
            // 
            // textBox3
            // 
            textBox3.Location = new Point(716, 41);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(100, 23);
            textBox3.TabIndex = 21;
            textBox3.Text = "2016-06-22";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(309, 43);
            label5.Name = "label5";
            label5.Size = new Size(32, 17);
            label5.TabIndex = 20;
            label5.Text = "类型";
            // 
            // comboBox4
            // 
            comboBox4.FormattingEnabled = true;
            comboBox4.Items.AddRange(new object[] { "全部", "检验结果", "查询样本申请信息" });
            comboBox4.Location = new Point(347, 38);
            comboBox4.Name = "comboBox4";
            comboBox4.Size = new Size(91, 25);
            comboBox4.TabIndex = 19;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(501, 40);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(148, 23);
            textBox1.TabIndex = 18;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(451, 43);
            label4.Name = "label4";
            label4.Size = new Size(44, 17);
            label4.TabIndex = 17;
            label4.Text = "条形码";
            // 
            // button2
            // 
            button2.Location = new Point(969, 38);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 16;
            button2.Text = "查询";
            button2.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(161, 40);
            label6.Name = "label6";
            label6.Size = new Size(32, 17);
            label6.TabIndex = 15;
            label6.Text = "状态";
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "全部", "待处理", "已处理", "处理失败" });
            comboBox1.Location = new Point(199, 38);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(89, 25);
            comboBox1.TabIndex = 14;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Column8, 状态, dataGridViewTextBoxColumn1, dataGridViewTextBoxColumn2, dataGridViewTextBoxColumn3, dataGridViewTextBoxColumn4, dataGridViewTextBoxColumn5, Column6 });
            dataGridView1.Location = new Point(3, 102);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(1336, 454);
            dataGridView1.TabIndex = 13;
            // 
            // Column8
            // 
            Column8.HeaderText = "全选";
            Column8.Name = "Column8";
            Column8.Width = 40;
            // 
            // 状态
            // 
            状态.HeaderText = "状态";
            状态.Name = "状态";
            状态.Width = 80;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewTextBoxColumn1.HeaderText = "类型";
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewTextBoxColumn2.HeaderText = "原始报文";
            dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            dataGridViewTextBoxColumn2.Width = 240;
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewTextBoxColumn3.HeaderText = "解码结果";
            dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            dataGridViewTextBoxColumn3.Width = 240;
            // 
            // dataGridViewTextBoxColumn4
            // 
            dataGridViewTextBoxColumn4.HeaderText = "条形码";
            dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // dataGridViewTextBoxColumn5
            // 
            dataGridViewTextBoxColumn5.HeaderText = "创建时间";
            dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            // 
            // Column6
            // 
            Column6.HeaderText = "失败原因";
            Column6.Name = "Column6";
            // 
            // tabPage3
            // 
            tabPage3.Location = new Point(4, 26);
            tabPage3.Name = "tabPage3";
            tabPage3.Size = new Size(1342, 591);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "发送仪器数据队列";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            tabPage4.Location = new Point(4, 26);
            tabPage4.Name = "tabPage4";
            tabPage4.Size = new Size(1342, 591);
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
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
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
        private Label label9;
        private Label label8;
        private TextBox textBox5;
        private TextBox textBox3;
        private Label label5;
        private ComboBox comboBox4;
        private TextBox textBox1;
        private Label label4;
        private Button button2;
        private Label label6;
        private ComboBox comboBox1;
        private DataGridView dataGridView1;
        private DataGridViewCheckBoxColumn Column8;
        private DataGridViewTextBoxColumn 状态;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private DataGridViewTextBoxColumn Column6;
        private DeviceHubControl.PagerControl pagerControl1;
        private DateTimePicker dateTimePicker1;
    }
}