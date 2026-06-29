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
            tabPageReceiveMessage = new TabPage();
            dgvReceiveMessage = new DataGridView();
            colReceiveMessageSelect = new DataGridViewCheckBoxColumn();
            colReceiveMessageStatus = new DataGridViewTextBoxColumn();
            colReceiveMessageType = new DataGridViewTextBoxColumn();
            colReceiveMessageRawMessage = new DataGridViewTextBoxColumn();
            colReceiveMessageDecodeResult = new DataGridViewTextBoxColumn();
            colReceiveMessageBarcode = new DataGridViewTextBoxColumn();
            colReceiveMessageSampleNo = new DataGridViewTextBoxColumn();
            colReceiveMessageCreateTime = new DataGridViewTextBoxColumn();
            colReceiveMessageErrorMessage = new DataGridViewTextBoxColumn();
            pagerReceiveMessage = new DeviceHub.Win.DeviceHubControl.PagerControl();
            pnlReceiveMessageQuery = new Panel();
            lblReceiveMessageStatus = new Label();
            cboReceiveMessageStatus = new ComboBox();
            lblReceiveMessageType = new Label();
            cboReceiveMessageType = new ComboBox();
            lblReceiveMessageBarcode = new Label();
            txtReceiveMessageBarcode = new TextBox();
            lblReceiveMessageSampleNo = new Label();
            txtReceiveMessageSampleNo = new TextBox();
            lblReceiveMessageCreateTime = new Label();
            dtpReceiveMessageCreateTimeStart = new DateTimePicker();
            lblReceiveMessageCreateTimeTo = new Label();
            dtpReceiveMessageCreateTimeEnd = new DateTimePicker();
            btnReceiveMessageQuery = new Button();
            tabPage3 = new TabPage();
            tabPage4 = new TabPage();
            grpAuthInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvInstrumentItemMapping).BeginInit();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPageReceiveMessage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvReceiveMessage).BeginInit();
            pnlReceiveMessageQuery.SuspendLayout();
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
            tabControl1.Controls.Add(tabPageReceiveMessage);
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
            // tabPageReceiveMessage
            // 
            tabPageReceiveMessage.Controls.Add(dgvReceiveMessage);
            tabPageReceiveMessage.Controls.Add(pagerReceiveMessage);
            tabPageReceiveMessage.Controls.Add(pnlReceiveMessageQuery);
            tabPageReceiveMessage.Location = new Point(4, 26);
            tabPageReceiveMessage.Name = "tabPageReceiveMessage";
            tabPageReceiveMessage.Padding = new Padding(3);
            tabPageReceiveMessage.Size = new Size(1342, 591);
            tabPageReceiveMessage.TabIndex = 1;
            tabPageReceiveMessage.Text = "接收仪器数据队列";
            tabPageReceiveMessage.UseVisualStyleBackColor = true;
            // 
            // dgvReceiveMessage
            // 
            dgvReceiveMessage.AllowUserToAddRows = false;
            dgvReceiveMessage.AllowUserToDeleteRows = false;
            dgvReceiveMessage.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvReceiveMessage.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvReceiveMessage.Columns.AddRange(new DataGridViewColumn[] { colReceiveMessageSelect, colReceiveMessageStatus, colReceiveMessageType, colReceiveMessageRawMessage, colReceiveMessageDecodeResult, colReceiveMessageBarcode, colReceiveMessageSampleNo, colReceiveMessageCreateTime, colReceiveMessageErrorMessage });
            dgvReceiveMessage.Dock = DockStyle.Fill;
            dgvReceiveMessage.Location = new Point(3, 47);
            dgvReceiveMessage.Name = "dgvReceiveMessage";
            dgvReceiveMessage.RowHeadersVisible = false;
            dgvReceiveMessage.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvReceiveMessage.Size = new Size(1336, 508);
            dgvReceiveMessage.TabIndex = 1;
            // 
            // colReceiveMessageSelect
            // 
            colReceiveMessageSelect.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colReceiveMessageSelect.FillWeight = 40F;
            colReceiveMessageSelect.HeaderText = "全选";
            colReceiveMessageSelect.Name = "colReceiveMessageSelect";
            colReceiveMessageSelect.Width = 40;
            // 
            // colReceiveMessageStatus
            // 
            colReceiveMessageStatus.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colReceiveMessageStatus.DataPropertyName = "Status";
            colReceiveMessageStatus.FillWeight = 80F;
            colReceiveMessageStatus.HeaderText = "状态";
            colReceiveMessageStatus.Name = "colReceiveMessageStatus";
            colReceiveMessageStatus.ReadOnly = true;
            colReceiveMessageStatus.Width = 80;
            // 
            // colReceiveMessageType
            // 
            colReceiveMessageType.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colReceiveMessageType.DataPropertyName = "Type";
            colReceiveMessageType.HeaderText = "类型";
            colReceiveMessageType.Name = "colReceiveMessageType";
            colReceiveMessageType.ReadOnly = true;
            // 
            // colReceiveMessageRawMessage
            // 
            colReceiveMessageRawMessage.DataPropertyName = "RawMessage";
            colReceiveMessageRawMessage.FillWeight = 200F;
            colReceiveMessageRawMessage.HeaderText = "原始报文";
            colReceiveMessageRawMessage.Name = "colReceiveMessageRawMessage";
            colReceiveMessageRawMessage.ReadOnly = true;
            // 
            // colReceiveMessageDecodeResult
            // 
            colReceiveMessageDecodeResult.DataPropertyName = "DecodeResult";
            colReceiveMessageDecodeResult.FillWeight = 200F;
            colReceiveMessageDecodeResult.HeaderText = "解码结果";
            colReceiveMessageDecodeResult.Name = "colReceiveMessageDecodeResult";
            colReceiveMessageDecodeResult.ReadOnly = true;
            // 
            // colReceiveMessageBarcode
            // 
            colReceiveMessageBarcode.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colReceiveMessageBarcode.DataPropertyName = "Barcode";
            colReceiveMessageBarcode.FillWeight = 120F;
            colReceiveMessageBarcode.HeaderText = "条形码";
            colReceiveMessageBarcode.Name = "colReceiveMessageBarcode";
            colReceiveMessageBarcode.ReadOnly = true;
            colReceiveMessageBarcode.Width = 120;
            // 
            // colReceiveMessageSampleNo
            // 
            colReceiveMessageSampleNo.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colReceiveMessageSampleNo.DataPropertyName = "SampleNo";
            colReceiveMessageSampleNo.HeaderText = "样本号";
            colReceiveMessageSampleNo.Name = "colReceiveMessageSampleNo";
            colReceiveMessageSampleNo.ReadOnly = true;
            // 
            // colReceiveMessageCreateTime
            // 
            colReceiveMessageCreateTime.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colReceiveMessageCreateTime.DataPropertyName = "CreateTime";
            colReceiveMessageCreateTime.FillWeight = 150F;
            colReceiveMessageCreateTime.HeaderText = "创建时间";
            colReceiveMessageCreateTime.Name = "colReceiveMessageCreateTime";
            colReceiveMessageCreateTime.ReadOnly = true;
            colReceiveMessageCreateTime.Width = 150;
            // 
            // colReceiveMessageErrorMessage
            // 
            colReceiveMessageErrorMessage.DataPropertyName = "ErrorMessage";
            colReceiveMessageErrorMessage.FillWeight = 150F;
            colReceiveMessageErrorMessage.HeaderText = "失败原因";
            colReceiveMessageErrorMessage.Name = "colReceiveMessageErrorMessage";
            colReceiveMessageErrorMessage.ReadOnly = true;
            // 
            // pagerReceiveMessage
            // 
            pagerReceiveMessage.AutoSize = true;
            pagerReceiveMessage.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            pagerReceiveMessage.Dock = DockStyle.Bottom;
            pagerReceiveMessage.Location = new Point(3, 555);
            pagerReceiveMessage.MinimumSize = new Size(640, 32);
            pagerReceiveMessage.Name = "pagerReceiveMessage";
            pagerReceiveMessage.PageSize = 20;
            pagerReceiveMessage.Size = new Size(1336, 33);
            pagerReceiveMessage.TabIndex = 2;
            // 
            // pnlReceiveMessageQuery
            // 
            pnlReceiveMessageQuery.Controls.Add(lblReceiveMessageStatus);
            pnlReceiveMessageQuery.Controls.Add(cboReceiveMessageStatus);
            pnlReceiveMessageQuery.Controls.Add(lblReceiveMessageType);
            pnlReceiveMessageQuery.Controls.Add(cboReceiveMessageType);
            pnlReceiveMessageQuery.Controls.Add(lblReceiveMessageBarcode);
            pnlReceiveMessageQuery.Controls.Add(txtReceiveMessageBarcode);
            pnlReceiveMessageQuery.Controls.Add(lblReceiveMessageSampleNo);
            pnlReceiveMessageQuery.Controls.Add(txtReceiveMessageSampleNo);
            pnlReceiveMessageQuery.Controls.Add(lblReceiveMessageCreateTime);
            pnlReceiveMessageQuery.Controls.Add(dtpReceiveMessageCreateTimeStart);
            pnlReceiveMessageQuery.Controls.Add(lblReceiveMessageCreateTimeTo);
            pnlReceiveMessageQuery.Controls.Add(dtpReceiveMessageCreateTimeEnd);
            pnlReceiveMessageQuery.Controls.Add(btnReceiveMessageQuery);
            pnlReceiveMessageQuery.Dock = DockStyle.Top;
            pnlReceiveMessageQuery.Location = new Point(3, 3);
            pnlReceiveMessageQuery.Name = "pnlReceiveMessageQuery";
            pnlReceiveMessageQuery.Size = new Size(1336, 44);
            pnlReceiveMessageQuery.TabIndex = 0;
            // 
            // lblReceiveMessageStatus
            // 
            lblReceiveMessageStatus.AutoSize = true;
            lblReceiveMessageStatus.Location = new Point(8, 14);
            lblReceiveMessageStatus.Name = "lblReceiveMessageStatus";
            lblReceiveMessageStatus.Size = new Size(32, 17);
            lblReceiveMessageStatus.TabIndex = 0;
            lblReceiveMessageStatus.Text = "状态";
            // 
            // cboReceiveMessageStatus
            // 
            cboReceiveMessageStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cboReceiveMessageStatus.FormattingEnabled = true;
            cboReceiveMessageStatus.Items.AddRange(new object[] { "全部", "待处理", "处理成功", "处理失败" });
            cboReceiveMessageStatus.Location = new Point(46, 10);
            cboReceiveMessageStatus.Name = "cboReceiveMessageStatus";
            cboReceiveMessageStatus.Size = new Size(90, 25);
            cboReceiveMessageStatus.TabIndex = 1;
            // 
            // lblReceiveMessageType
            // 
            lblReceiveMessageType.AutoSize = true;
            lblReceiveMessageType.Location = new Point(148, 14);
            lblReceiveMessageType.Name = "lblReceiveMessageType";
            lblReceiveMessageType.Size = new Size(32, 17);
            lblReceiveMessageType.TabIndex = 2;
            lblReceiveMessageType.Text = "类型";
            // 
            // cboReceiveMessageType
            // 
            cboReceiveMessageType.DropDownStyle = ComboBoxStyle.DropDownList;
            cboReceiveMessageType.FormattingEnabled = true;
            cboReceiveMessageType.Items.AddRange(new object[] { "全部", "检验结果", "查询样本申请信息" });
            cboReceiveMessageType.Location = new Point(186, 10);
            cboReceiveMessageType.Name = "cboReceiveMessageType";
            cboReceiveMessageType.Size = new Size(130, 25);
            cboReceiveMessageType.TabIndex = 3;
            // 
            // lblReceiveMessageBarcode
            // 
            lblReceiveMessageBarcode.AutoSize = true;
            lblReceiveMessageBarcode.Location = new Point(328, 14);
            lblReceiveMessageBarcode.Name = "lblReceiveMessageBarcode";
            lblReceiveMessageBarcode.Size = new Size(44, 17);
            lblReceiveMessageBarcode.TabIndex = 4;
            lblReceiveMessageBarcode.Text = "条形码";
            // 
            // txtReceiveMessageBarcode
            // 
            txtReceiveMessageBarcode.Location = new Point(378, 10);
            txtReceiveMessageBarcode.Name = "txtReceiveMessageBarcode";
            txtReceiveMessageBarcode.PlaceholderText = "请输入条形码";
            txtReceiveMessageBarcode.Size = new Size(130, 23);
            txtReceiveMessageBarcode.TabIndex = 5;
            // 
            // lblReceiveMessageSampleNo
            // 
            lblReceiveMessageSampleNo.AutoSize = true;
            lblReceiveMessageSampleNo.Location = new Point(520, 14);
            lblReceiveMessageSampleNo.Name = "lblReceiveMessageSampleNo";
            lblReceiveMessageSampleNo.Size = new Size(44, 17);
            lblReceiveMessageSampleNo.TabIndex = 6;
            lblReceiveMessageSampleNo.Text = "样本号";
            // 
            // txtReceiveMessageSampleNo
            // 
            txtReceiveMessageSampleNo.Location = new Point(570, 10);
            txtReceiveMessageSampleNo.Name = "txtReceiveMessageSampleNo";
            txtReceiveMessageSampleNo.PlaceholderText = "请输入样本号";
            txtReceiveMessageSampleNo.Size = new Size(110, 23);
            txtReceiveMessageSampleNo.TabIndex = 7;
            // 
            // lblReceiveMessageCreateTime
            // 
            lblReceiveMessageCreateTime.AutoSize = true;
            lblReceiveMessageCreateTime.Location = new Point(692, 14);
            lblReceiveMessageCreateTime.Name = "lblReceiveMessageCreateTime";
            lblReceiveMessageCreateTime.Size = new Size(56, 17);
            lblReceiveMessageCreateTime.TabIndex = 8;
            lblReceiveMessageCreateTime.Text = "创建时间";
            // 
            // dtpReceiveMessageCreateTimeStart
            // 
            dtpReceiveMessageCreateTimeStart.Format = DateTimePickerFormat.Short;
            dtpReceiveMessageCreateTimeStart.Location = new Point(754, 10);
            dtpReceiveMessageCreateTimeStart.Name = "dtpReceiveMessageCreateTimeStart";
            dtpReceiveMessageCreateTimeStart.Size = new Size(110, 23);
            dtpReceiveMessageCreateTimeStart.TabIndex = 9;
            // 
            // lblReceiveMessageCreateTimeTo
            // 
            lblReceiveMessageCreateTimeTo.AutoSize = true;
            lblReceiveMessageCreateTimeTo.Location = new Point(872, 14);
            lblReceiveMessageCreateTimeTo.Name = "lblReceiveMessageCreateTimeTo";
            lblReceiveMessageCreateTimeTo.Size = new Size(20, 17);
            lblReceiveMessageCreateTimeTo.TabIndex = 10;
            lblReceiveMessageCreateTimeTo.Text = "至";
            // 
            // dtpReceiveMessageCreateTimeEnd
            // 
            dtpReceiveMessageCreateTimeEnd.Format = DateTimePickerFormat.Short;
            dtpReceiveMessageCreateTimeEnd.Location = new Point(898, 10);
            dtpReceiveMessageCreateTimeEnd.Name = "dtpReceiveMessageCreateTimeEnd";
            dtpReceiveMessageCreateTimeEnd.Size = new Size(110, 23);
            dtpReceiveMessageCreateTimeEnd.TabIndex = 11;
            // 
            // btnReceiveMessageQuery
            // 
            btnReceiveMessageQuery.Location = new Point(1020, 9);
            btnReceiveMessageQuery.Name = "btnReceiveMessageQuery";
            btnReceiveMessageQuery.Size = new Size(75, 26);
            btnReceiveMessageQuery.TabIndex = 12;
            btnReceiveMessageQuery.Text = "查询";
            btnReceiveMessageQuery.UseVisualStyleBackColor = true;
            btnReceiveMessageQuery.Click += btnReceiveMessageQuery_Click;
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
            tabPageReceiveMessage.ResumeLayout(false);
            tabPageReceiveMessage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvReceiveMessage).EndInit();
            pnlReceiveMessageQuery.ResumeLayout(false);
            pnlReceiveMessageQuery.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private GroupBox grpAuthInfo;
        private Label lblExpireTime;
        private Label label3;
        private Label lblAuthStatus;
        private Label label2;
        private Label lblAuthCode;
        private Label label1;
        private DataGridView dgvInstrumentItemMapping;
        private DeviceHubControl.PagerControl pagerInstrumentItemMapping;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column3;
        private DataGridViewTextBoxColumn Column4;
        private DataGridViewTextBoxColumn Column5;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPageReceiveMessage;
        private TabPage tabPage3;
        private TabPage tabPage4;
        private Panel pnlReceiveMessageQuery;
        private Label lblReceiveMessageStatus;
        private ComboBox cboReceiveMessageStatus;
        private Label lblReceiveMessageType;
        private ComboBox cboReceiveMessageType;
        private Label lblReceiveMessageBarcode;
        private TextBox txtReceiveMessageBarcode;
        private Label lblReceiveMessageSampleNo;
        private TextBox txtReceiveMessageSampleNo;
        private Label lblReceiveMessageCreateTime;
        private DateTimePicker dtpReceiveMessageCreateTimeStart;
        private Label lblReceiveMessageCreateTimeTo;
        private DateTimePicker dtpReceiveMessageCreateTimeEnd;
        private Button btnReceiveMessageQuery;
        private DataGridView dgvReceiveMessage;
        private DeviceHubControl.PagerControl pagerReceiveMessage;
        private DataGridViewCheckBoxColumn colReceiveMessageSelect;
        private DataGridViewTextBoxColumn colReceiveMessageStatus;
        private DataGridViewTextBoxColumn colReceiveMessageType;
        private DataGridViewTextBoxColumn colReceiveMessageRawMessage;
        private DataGridViewTextBoxColumn colReceiveMessageDecodeResult;
        private DataGridViewTextBoxColumn colReceiveMessageBarcode;
        private DataGridViewTextBoxColumn colReceiveMessageSampleNo;
        private DataGridViewTextBoxColumn colReceiveMessageCreateTime;
        private DataGridViewTextBoxColumn colReceiveMessageErrorMessage;
    }
}
