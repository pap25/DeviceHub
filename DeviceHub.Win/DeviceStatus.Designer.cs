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
            grpLisConfigAuthInfo = new GroupBox();
            lblLisConfigExpireTime = new Label();
            lblLisConfigExpireTimeTitle = new Label();
            lblLisConfigAuthStatus = new Label();
            lblLisConfigAuthStatusTitle = new Label();
            lblLisConfigAuthCode = new Label();
            lblLisConfigAuthCodeTitle = new Label();
            dgvInstrumentItemMapping = new DataGridView();
            colInstrumentItemMappingInstrumentItemCode = new DataGridViewTextBoxColumn();
            colInstrumentItemMappingInstrumentItemName = new DataGridViewTextBoxColumn();
            colInstrumentItemMappingLisItemCode = new DataGridViewTextBoxColumn();
            colInstrumentItemMappingLisItemName = new DataGridViewTextBoxColumn();
            colInstrumentItemMappingUnit = new DataGridViewTextBoxColumn();
            pagerInstrumentItemMapping = new DeviceHub.Win.DeviceHubControl.PagerControl();
            tabControl1 = new TabControl();
            tabPageLisConfig = new TabPage();
            pnlLisConfigLeft = new Panel();
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
            tabPageSendMessage = new TabPage();
            tabPageOperationLog = new TabPage();
            button1 = new Button();
            grpLisConfigAuthInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvInstrumentItemMapping).BeginInit();
            tabControl1.SuspendLayout();
            tabPageLisConfig.SuspendLayout();
            pnlLisConfigLeft.SuspendLayout();
            tabPageReceiveMessage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvReceiveMessage).BeginInit();
            pnlReceiveMessageQuery.SuspendLayout();
            SuspendLayout();
            // 
            // grpLisConfigAuthInfo
            // 
            grpLisConfigAuthInfo.Controls.Add(lblLisConfigExpireTime);
            grpLisConfigAuthInfo.Controls.Add(lblLisConfigExpireTimeTitle);
            grpLisConfigAuthInfo.Controls.Add(lblLisConfigAuthStatus);
            grpLisConfigAuthInfo.Controls.Add(lblLisConfigAuthStatusTitle);
            grpLisConfigAuthInfo.Controls.Add(lblLisConfigAuthCode);
            grpLisConfigAuthInfo.Controls.Add(lblLisConfigAuthCodeTitle);
            grpLisConfigAuthInfo.ForeColor = Color.DarkBlue;
            grpLisConfigAuthInfo.Location = new Point(3, 6);
            grpLisConfigAuthInfo.Name = "grpLisConfigAuthInfo";
            grpLisConfigAuthInfo.Size = new Size(244, 145);
            grpLisConfigAuthInfo.TabIndex = 0;
            grpLisConfigAuthInfo.TabStop = false;
            grpLisConfigAuthInfo.Text = "授权信息";
            // 
            // lblLisConfigExpireTime
            // 
            lblLisConfigExpireTime.AutoSize = true;
            lblLisConfigExpireTime.ForeColor = SystemColors.ControlText;
            lblLisConfigExpireTime.Location = new Point(100, 113);
            lblLisConfigExpireTime.Name = "lblLisConfigExpireTime";
            lblLisConfigExpireTime.Size = new Size(0, 17);
            lblLisConfigExpireTime.TabIndex = 5;
            // 
            // lblLisConfigExpireTimeTitle
            // 
            lblLisConfigExpireTimeTitle.AutoSize = true;
            lblLisConfigExpireTimeTitle.ForeColor = SystemColors.ControlText;
            lblLisConfigExpireTimeTitle.Location = new Point(20, 113);
            lblLisConfigExpireTimeTitle.Name = "lblLisConfigExpireTimeTitle";
            lblLisConfigExpireTimeTitle.Size = new Size(68, 17);
            lblLisConfigExpireTimeTitle.TabIndex = 4;
            lblLisConfigExpireTimeTitle.Text = "过期时间：";
            // 
            // lblLisConfigAuthStatus
            // 
            lblLisConfigAuthStatus.AutoSize = true;
            lblLisConfigAuthStatus.ForeColor = Color.Green;
            lblLisConfigAuthStatus.Location = new Point(100, 78);
            lblLisConfigAuthStatus.Name = "lblLisConfigAuthStatus";
            lblLisConfigAuthStatus.Size = new Size(0, 17);
            lblLisConfigAuthStatus.TabIndex = 3;
            // 
            // lblLisConfigAuthStatusTitle
            // 
            lblLisConfigAuthStatusTitle.AutoSize = true;
            lblLisConfigAuthStatusTitle.ForeColor = SystemColors.ControlText;
            lblLisConfigAuthStatusTitle.Location = new Point(20, 78);
            lblLisConfigAuthStatusTitle.Name = "lblLisConfigAuthStatusTitle";
            lblLisConfigAuthStatusTitle.Size = new Size(44, 17);
            lblLisConfigAuthStatusTitle.TabIndex = 2;
            lblLisConfigAuthStatusTitle.Text = "状态：";
            // 
            // lblLisConfigAuthCode
            // 
            lblLisConfigAuthCode.AutoSize = true;
            lblLisConfigAuthCode.ForeColor = Color.Red;
            lblLisConfigAuthCode.Location = new Point(100, 43);
            lblLisConfigAuthCode.Name = "lblLisConfigAuthCode";
            lblLisConfigAuthCode.Size = new Size(0, 17);
            lblLisConfigAuthCode.TabIndex = 1;
            // 
            // lblLisConfigAuthCodeTitle
            // 
            lblLisConfigAuthCodeTitle.AutoSize = true;
            lblLisConfigAuthCodeTitle.ForeColor = SystemColors.ControlText;
            lblLisConfigAuthCodeTitle.Location = new Point(20, 43);
            lblLisConfigAuthCodeTitle.Name = "lblLisConfigAuthCodeTitle";
            lblLisConfigAuthCodeTitle.Size = new Size(56, 17);
            lblLisConfigAuthCodeTitle.TabIndex = 0;
            lblLisConfigAuthCodeTitle.Text = "授权码：";
            // 
            // dgvInstrumentItemMapping
            // 
            dgvInstrumentItemMapping.AllowUserToAddRows = false;
            dgvInstrumentItemMapping.AllowUserToDeleteRows = false;
            dgvInstrumentItemMapping.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvInstrumentItemMapping.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvInstrumentItemMapping.Columns.AddRange(new DataGridViewColumn[] { colInstrumentItemMappingInstrumentItemCode, colInstrumentItemMappingInstrumentItemName, colInstrumentItemMappingLisItemCode, colInstrumentItemMappingLisItemName, colInstrumentItemMappingUnit });
            dgvInstrumentItemMapping.Dock = DockStyle.Fill;
            dgvInstrumentItemMapping.Location = new Point(259, 3);
            dgvInstrumentItemMapping.Name = "dgvInstrumentItemMapping";
            dgvInstrumentItemMapping.ReadOnly = true;
            dgvInstrumentItemMapping.RowHeadersVisible = false;
            dgvInstrumentItemMapping.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvInstrumentItemMapping.Size = new Size(1080, 552);
            dgvInstrumentItemMapping.TabIndex = 1;
            // 
            // colInstrumentItemMappingInstrumentItemCode
            // 
            colInstrumentItemMappingInstrumentItemCode.DataPropertyName = "InstrumentItemCode";
            colInstrumentItemMappingInstrumentItemCode.HeaderText = "仪器项目编号";
            colInstrumentItemMappingInstrumentItemCode.Name = "colInstrumentItemMappingInstrumentItemCode";
            colInstrumentItemMappingInstrumentItemCode.ReadOnly = true;
            // 
            // colInstrumentItemMappingInstrumentItemName
            // 
            colInstrumentItemMappingInstrumentItemName.DataPropertyName = "InstrumentItemName";
            colInstrumentItemMappingInstrumentItemName.HeaderText = "仪器项目名";
            colInstrumentItemMappingInstrumentItemName.Name = "colInstrumentItemMappingInstrumentItemName";
            colInstrumentItemMappingInstrumentItemName.ReadOnly = true;
            // 
            // colInstrumentItemMappingLisItemCode
            // 
            colInstrumentItemMappingLisItemCode.DataPropertyName = "LisItemCode";
            colInstrumentItemMappingLisItemCode.HeaderText = "LIS项目编号";
            colInstrumentItemMappingLisItemCode.Name = "colInstrumentItemMappingLisItemCode";
            colInstrumentItemMappingLisItemCode.ReadOnly = true;
            // 
            // colInstrumentItemMappingLisItemName
            // 
            colInstrumentItemMappingLisItemName.DataPropertyName = "LisItemName";
            colInstrumentItemMappingLisItemName.HeaderText = "LIS项目名";
            colInstrumentItemMappingLisItemName.Name = "colInstrumentItemMappingLisItemName";
            colInstrumentItemMappingLisItemName.ReadOnly = true;
            // 
            // colInstrumentItemMappingUnit
            // 
            colInstrumentItemMappingUnit.DataPropertyName = "Unit";
            colInstrumentItemMappingUnit.HeaderText = "单位";
            colInstrumentItemMappingUnit.Name = "colInstrumentItemMappingUnit";
            colInstrumentItemMappingUnit.ReadOnly = true;
            // 
            // pagerInstrumentItemMapping
            // 
            pagerInstrumentItemMapping.AutoSize = true;
            pagerInstrumentItemMapping.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            pagerInstrumentItemMapping.Dock = DockStyle.Bottom;
            pagerInstrumentItemMapping.Location = new Point(259, 555);
            pagerInstrumentItemMapping.MinimumSize = new Size(640, 32);
            pagerInstrumentItemMapping.Name = "pagerInstrumentItemMapping";
            pagerInstrumentItemMapping.PageSize = 20;
            pagerInstrumentItemMapping.Size = new Size(1080, 33);
            pagerInstrumentItemMapping.TabIndex = 2;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPageLisConfig);
            tabControl1.Controls.Add(tabPageReceiveMessage);
            tabControl1.Controls.Add(tabPageSendMessage);
            tabControl1.Controls.Add(tabPageOperationLog);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1350, 621);
            tabControl1.TabIndex = 3;
            tabControl1.SelectedIndexChanged += TabControl1_SelectedIndexChanged;
            // 
            // tabPageLisConfig
            // 
            tabPageLisConfig.Controls.Add(dgvInstrumentItemMapping);
            tabPageLisConfig.Controls.Add(pagerInstrumentItemMapping);
            tabPageLisConfig.Controls.Add(pnlLisConfigLeft);
            tabPageLisConfig.Location = new Point(4, 26);
            tabPageLisConfig.Name = "tabPageLisConfig";
            tabPageLisConfig.Padding = new Padding(3);
            tabPageLisConfig.Size = new Size(1342, 591);
            tabPageLisConfig.TabIndex = 0;
            tabPageLisConfig.Text = "LIS参数配置";
            tabPageLisConfig.UseVisualStyleBackColor = true;
            // 
            // pnlLisConfigLeft
            // 
            pnlLisConfigLeft.Controls.Add(grpLisConfigAuthInfo);
            pnlLisConfigLeft.Dock = DockStyle.Left;
            pnlLisConfigLeft.Location = new Point(3, 3);
            pnlLisConfigLeft.Name = "pnlLisConfigLeft";
            pnlLisConfigLeft.Size = new Size(256, 585);
            pnlLisConfigLeft.TabIndex = 0;
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
            dgvReceiveMessage.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvReceiveMessage.Size = new Size(1336, 508);
            dgvReceiveMessage.TabIndex = 1;
            // 
            // colReceiveMessageSelect
            // 
            colReceiveMessageSelect.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colReceiveMessageSelect.FillWeight = 51F;
            colReceiveMessageSelect.HeaderText = "全选";
            colReceiveMessageSelect.Name = "colReceiveMessageSelect";
            colReceiveMessageSelect.Width = 51;
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
            colReceiveMessageType.Width = 80;
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
            pnlReceiveMessageQuery.Controls.Add(button1);
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
            // tabPageSendMessage
            // 
            tabPageSendMessage.Location = new Point(4, 26);
            tabPageSendMessage.Name = "tabPageSendMessage";
            tabPageSendMessage.Padding = new Padding(3);
            tabPageSendMessage.Size = new Size(1342, 591);
            tabPageSendMessage.TabIndex = 2;
            tabPageSendMessage.Text = "发送仪器数据队列";
            tabPageSendMessage.UseVisualStyleBackColor = true;
            // 
            // tabPageOperationLog
            // 
            tabPageOperationLog.Location = new Point(4, 26);
            tabPageOperationLog.Name = "tabPageOperationLog";
            tabPageOperationLog.Padding = new Padding(3);
            tabPageOperationLog.Size = new Size(1342, 591);
            tabPageOperationLog.TabIndex = 3;
            tabPageOperationLog.Text = "操作日志";
            tabPageOperationLog.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            button1.Location = new Point(1169, 10);
            button1.Name = "button1";
            button1.Size = new Size(132, 26);
            button1.TabIndex = 13;
            button1.Text = "重新解码并同步LIS";
            button1.UseVisualStyleBackColor = true;
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
            grpLisConfigAuthInfo.ResumeLayout(false);
            grpLisConfigAuthInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvInstrumentItemMapping).EndInit();
            tabControl1.ResumeLayout(false);
            tabPageLisConfig.ResumeLayout(false);
            tabPageLisConfig.PerformLayout();
            pnlLisConfigLeft.ResumeLayout(false);
            tabPageReceiveMessage.ResumeLayout(false);
            tabPageReceiveMessage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvReceiveMessage).EndInit();
            pnlReceiveMessageQuery.ResumeLayout(false);
            pnlReceiveMessageQuery.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private GroupBox grpLisConfigAuthInfo;
        private Label lblLisConfigExpireTime;
        private Label lblLisConfigExpireTimeTitle;
        private Label lblLisConfigAuthStatus;
        private Label lblLisConfigAuthStatusTitle;
        private Label lblLisConfigAuthCode;
        private Label lblLisConfigAuthCodeTitle;
        private Panel pnlLisConfigLeft;
        private DataGridView dgvInstrumentItemMapping;
        private DeviceHubControl.PagerControl pagerInstrumentItemMapping;
        private DataGridViewTextBoxColumn colInstrumentItemMappingInstrumentItemCode;
        private DataGridViewTextBoxColumn colInstrumentItemMappingInstrumentItemName;
        private DataGridViewTextBoxColumn colInstrumentItemMappingLisItemCode;
        private DataGridViewTextBoxColumn colInstrumentItemMappingLisItemName;
        private DataGridViewTextBoxColumn colInstrumentItemMappingUnit;
        private TabControl tabControl1;
        private TabPage tabPageLisConfig;
        private TabPage tabPageReceiveMessage;
        private TabPage tabPageSendMessage;
        private TabPage tabPageOperationLog;
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
        private Button button1;
    }
}
