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
            tabLisConfig = new TabPage();
            pnlLisConfigLeft = new Panel();
            tabReceiveMessage = new TabPage();
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
            button1 = new Button();
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
            tabSendMessage = new TabPage();
            dgvSendMessage = new DataGridView();
            colSendMessageStatus = new DataGridViewTextBoxColumn();
            colSendMessageSendJson = new DataGridViewTextBoxColumn();
            colSendMessageSendContent = new DataGridViewTextBoxColumn();
            colSendMessageBarcode = new DataGridViewTextBoxColumn();
            colSendMessageSampleNo = new DataGridViewTextBoxColumn();
            colSendMessageCreateTime = new DataGridViewTextBoxColumn();
            colSendMessageErrorMessage = new DataGridViewTextBoxColumn();
            pagerSendMessage = new DeviceHub.Win.DeviceHubControl.PagerControl();
            pnlSendMessageQuery = new Panel();
            lblSendMessageStatus = new Label();
            cboSendMessageStatus = new ComboBox();
            lblSendMessageBarcode = new Label();
            txtSendMessageBarcode = new TextBox();
            lblSendMessageSampleNo = new Label();
            txtSendMessageSampleNo = new TextBox();
            lblSendMessageCreateTime = new Label();
            dtpSendMessageCreateTimeStart = new DateTimePicker();
            lblSendMessageCreateTimeTo = new Label();
            dtpSendMessageCreateTimeEnd = new DateTimePicker();
            btnSendMessageQuery = new Button();
            tabLog = new TabPage();
            grpLisConfigAuthInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvInstrumentItemMapping).BeginInit();
            tabControl1.SuspendLayout();
            tabLisConfig.SuspendLayout();
            pnlLisConfigLeft.SuspendLayout();
            tabReceiveMessage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvReceiveMessage).BeginInit();
            pnlReceiveMessageQuery.SuspendLayout();
            tabSendMessage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSendMessage).BeginInit();
            pnlSendMessageQuery.SuspendLayout();
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
            pagerInstrumentItemMapping.Size = new Size(1080, 33);
            pagerInstrumentItemMapping.TabIndex = 2;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabLisConfig);
            tabControl1.Controls.Add(tabReceiveMessage);
            tabControl1.Controls.Add(tabSendMessage);
            tabControl1.Controls.Add(tabLog);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1350, 621);
            tabControl1.TabIndex = 3;
            tabControl1.SelectedIndexChanged += TabControl1_SelectedIndexChanged;
            // 
            // tabLisConfig
            // 
            tabLisConfig.Controls.Add(dgvInstrumentItemMapping);
            tabLisConfig.Controls.Add(pagerInstrumentItemMapping);
            tabLisConfig.Controls.Add(pnlLisConfigLeft);
            tabLisConfig.Location = new Point(4, 26);
            tabLisConfig.Name = "tabLisConfig";
            tabLisConfig.Padding = new Padding(3);
            tabLisConfig.Size = new Size(1342, 591);
            tabLisConfig.TabIndex = 0;
            tabLisConfig.Text = "LIS参数配置";
            tabLisConfig.UseVisualStyleBackColor = true;
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
            // tabReceiveMessage
            // 
            tabReceiveMessage.Controls.Add(dgvReceiveMessage);
            tabReceiveMessage.Controls.Add(pagerReceiveMessage);
            tabReceiveMessage.Controls.Add(pnlReceiveMessageQuery);
            tabReceiveMessage.Location = new Point(4, 26);
            tabReceiveMessage.Name = "tabReceiveMessage";
            tabReceiveMessage.Padding = new Padding(3);
            tabReceiveMessage.Size = new Size(1342, 591);
            tabReceiveMessage.TabIndex = 1;
            tabReceiveMessage.Text = "接收仪器数据队列";
            tabReceiveMessage.UseVisualStyleBackColor = true;
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
            colReceiveMessageStatus.DataPropertyName = "StatusName";
            colReceiveMessageStatus.FillWeight = 80F;
            colReceiveMessageStatus.HeaderText = "状态";
            colReceiveMessageStatus.Name = "colReceiveMessageStatus";
            colReceiveMessageStatus.ReadOnly = true;
            colReceiveMessageStatus.Width = 80;
            // 
            // colReceiveMessageType
            // 
            colReceiveMessageType.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colReceiveMessageType.DataPropertyName = "TypeName";
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
            // button1
            // 
            button1.Location = new Point(1169, 10);
            button1.Name = "button1";
            button1.Size = new Size(132, 26);
            button1.TabIndex = 13;
            button1.Text = "重新解码并同步LIS";
            button1.UseVisualStyleBackColor = true;
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
            // tabSendMessage
            // 
            tabSendMessage.Controls.Add(dgvSendMessage);
            tabSendMessage.Controls.Add(pagerSendMessage);
            tabSendMessage.Controls.Add(pnlSendMessageQuery);
            tabSendMessage.Location = new Point(4, 26);
            tabSendMessage.Name = "tabSendMessage";
            tabSendMessage.Padding = new Padding(3);
            tabSendMessage.Size = new Size(1342, 591);
            tabSendMessage.TabIndex = 2;
            tabSendMessage.Text = "发送仪器数据队列";
            tabSendMessage.UseVisualStyleBackColor = true;
            // 
            // dgvSendMessage
            // 
            dgvSendMessage.AllowUserToAddRows = false;
            dgvSendMessage.AllowUserToDeleteRows = false;
            dgvSendMessage.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSendMessage.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSendMessage.Columns.AddRange(new DataGridViewColumn[] { colSendMessageStatus, colSendMessageSendJson, colSendMessageSendContent, colSendMessageBarcode, colSendMessageSampleNo, colSendMessageCreateTime, colSendMessageErrorMessage });
            dgvSendMessage.Dock = DockStyle.Fill;
            dgvSendMessage.Location = new Point(3, 47);
            dgvSendMessage.Name = "dgvSendMessage";
            dgvSendMessage.RowHeadersVisible = false;
            dgvSendMessage.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvSendMessage.Size = new Size(1336, 508);
            dgvSendMessage.TabIndex = 1;
            // 
            // colSendMessageStatus
            // 
            colSendMessageStatus.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colSendMessageStatus.DataPropertyName = "StatusName";
            colSendMessageStatus.FillWeight = 80F;
            colSendMessageStatus.HeaderText = "状态";
            colSendMessageStatus.Name = "colSendMessageStatus";
            colSendMessageStatus.ReadOnly = true;
            colSendMessageStatus.Width = 80;
            // 
            // colSendMessageSendJson
            // 
            colSendMessageSendJson.DataPropertyName = "SendJson";
            colSendMessageSendJson.FillWeight = 200F;
            colSendMessageSendJson.HeaderText = "发送内容JSON";
            colSendMessageSendJson.Name = "colSendMessageSendJson";
            colSendMessageSendJson.ReadOnly = true;
            // 
            // colSendMessageSendContent
            // 
            colSendMessageSendContent.DataPropertyName = "SendContent";
            colSendMessageSendContent.FillWeight = 200F;
            colSendMessageSendContent.HeaderText = "发送报文";
            colSendMessageSendContent.Name = "colSendMessageSendContent";
            colSendMessageSendContent.ReadOnly = true;
            // 
            // colSendMessageBarcode
            // 
            colSendMessageBarcode.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colSendMessageBarcode.DataPropertyName = "Barcode";
            colSendMessageBarcode.FillWeight = 120F;
            colSendMessageBarcode.HeaderText = "条形码";
            colSendMessageBarcode.Name = "colSendMessageBarcode";
            colSendMessageBarcode.ReadOnly = true;
            colSendMessageBarcode.Width = 120;
            // 
            // colSendMessageSampleNo
            // 
            colSendMessageSampleNo.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colSendMessageSampleNo.DataPropertyName = "SampleNo";
            colSendMessageSampleNo.HeaderText = "样本号";
            colSendMessageSampleNo.Name = "colSendMessageSampleNo";
            colSendMessageSampleNo.ReadOnly = true;
            // 
            // colSendMessageCreateTime
            // 
            colSendMessageCreateTime.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colSendMessageCreateTime.DataPropertyName = "CreateTime";
            colSendMessageCreateTime.FillWeight = 150F;
            colSendMessageCreateTime.HeaderText = "创建时间";
            colSendMessageCreateTime.Name = "colSendMessageCreateTime";
            colSendMessageCreateTime.ReadOnly = true;
            colSendMessageCreateTime.Width = 150;
            // 
            // colSendMessageErrorMessage
            // 
            colSendMessageErrorMessage.DataPropertyName = "ErrorMessage";
            colSendMessageErrorMessage.HeaderText = "失败原因";
            colSendMessageErrorMessage.Name = "colSendMessageErrorMessage";
            colSendMessageErrorMessage.ReadOnly = true;
            // 
            // pagerSendMessage
            // 
            pagerSendMessage.AutoSize = true;
            pagerSendMessage.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            pagerSendMessage.Dock = DockStyle.Bottom;
            pagerSendMessage.Location = new Point(3, 555);
            pagerSendMessage.MinimumSize = new Size(640, 32);
            pagerSendMessage.Name = "pagerSendMessage";
            pagerSendMessage.Size = new Size(1336, 33);
            pagerSendMessage.TabIndex = 2;
            // 
            // pnlSendMessageQuery
            // 
            pnlSendMessageQuery.Controls.Add(lblSendMessageStatus);
            pnlSendMessageQuery.Controls.Add(cboSendMessageStatus);
            pnlSendMessageQuery.Controls.Add(lblSendMessageBarcode);
            pnlSendMessageQuery.Controls.Add(txtSendMessageBarcode);
            pnlSendMessageQuery.Controls.Add(lblSendMessageSampleNo);
            pnlSendMessageQuery.Controls.Add(txtSendMessageSampleNo);
            pnlSendMessageQuery.Controls.Add(lblSendMessageCreateTime);
            pnlSendMessageQuery.Controls.Add(dtpSendMessageCreateTimeStart);
            pnlSendMessageQuery.Controls.Add(lblSendMessageCreateTimeTo);
            pnlSendMessageQuery.Controls.Add(dtpSendMessageCreateTimeEnd);
            pnlSendMessageQuery.Controls.Add(btnSendMessageQuery);
            pnlSendMessageQuery.Dock = DockStyle.Top;
            pnlSendMessageQuery.Location = new Point(3, 3);
            pnlSendMessageQuery.Name = "pnlSendMessageQuery";
            pnlSendMessageQuery.Size = new Size(1336, 44);
            pnlSendMessageQuery.TabIndex = 0;
            // 
            // lblSendMessageStatus
            // 
            lblSendMessageStatus.AutoSize = true;
            lblSendMessageStatus.Location = new Point(8, 14);
            lblSendMessageStatus.Name = "lblSendMessageStatus";
            lblSendMessageStatus.Size = new Size(32, 17);
            lblSendMessageStatus.TabIndex = 0;
            lblSendMessageStatus.Text = "状态";
            // 
            // cboSendMessageStatus
            // 
            cboSendMessageStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cboSendMessageStatus.FormattingEnabled = true;
            cboSendMessageStatus.Location = new Point(46, 10);
            cboSendMessageStatus.Name = "cboSendMessageStatus";
            cboSendMessageStatus.Size = new Size(90, 25);
            cboSendMessageStatus.TabIndex = 1;
            // 
            // lblSendMessageBarcode
            // 
            lblSendMessageBarcode.AutoSize = true;
            lblSendMessageBarcode.Location = new Point(148, 14);
            lblSendMessageBarcode.Name = "lblSendMessageBarcode";
            lblSendMessageBarcode.Size = new Size(44, 17);
            lblSendMessageBarcode.TabIndex = 2;
            lblSendMessageBarcode.Text = "条形码";
            // 
            // txtSendMessageBarcode
            // 
            txtSendMessageBarcode.Location = new Point(198, 10);
            txtSendMessageBarcode.Name = "txtSendMessageBarcode";
            txtSendMessageBarcode.PlaceholderText = "请输入条形码";
            txtSendMessageBarcode.Size = new Size(130, 23);
            txtSendMessageBarcode.TabIndex = 3;
            // 
            // lblSendMessageSampleNo
            // 
            lblSendMessageSampleNo.AutoSize = true;
            lblSendMessageSampleNo.Location = new Point(340, 14);
            lblSendMessageSampleNo.Name = "lblSendMessageSampleNo";
            lblSendMessageSampleNo.Size = new Size(44, 17);
            lblSendMessageSampleNo.TabIndex = 4;
            lblSendMessageSampleNo.Text = "样本号";
            // 
            // txtSendMessageSampleNo
            // 
            txtSendMessageSampleNo.Location = new Point(390, 10);
            txtSendMessageSampleNo.Name = "txtSendMessageSampleNo";
            txtSendMessageSampleNo.PlaceholderText = "请输入样本号";
            txtSendMessageSampleNo.Size = new Size(110, 23);
            txtSendMessageSampleNo.TabIndex = 5;
            // 
            // lblSendMessageCreateTime
            // 
            lblSendMessageCreateTime.AutoSize = true;
            lblSendMessageCreateTime.Location = new Point(512, 14);
            lblSendMessageCreateTime.Name = "lblSendMessageCreateTime";
            lblSendMessageCreateTime.Size = new Size(56, 17);
            lblSendMessageCreateTime.TabIndex = 6;
            lblSendMessageCreateTime.Text = "创建时间";
            // 
            // dtpSendMessageCreateTimeStart
            // 
            dtpSendMessageCreateTimeStart.Format = DateTimePickerFormat.Short;
            dtpSendMessageCreateTimeStart.Location = new Point(574, 10);
            dtpSendMessageCreateTimeStart.Name = "dtpSendMessageCreateTimeStart";
            dtpSendMessageCreateTimeStart.Size = new Size(110, 23);
            dtpSendMessageCreateTimeStart.TabIndex = 7;
            // 
            // lblSendMessageCreateTimeTo
            // 
            lblSendMessageCreateTimeTo.AutoSize = true;
            lblSendMessageCreateTimeTo.Location = new Point(692, 14);
            lblSendMessageCreateTimeTo.Name = "lblSendMessageCreateTimeTo";
            lblSendMessageCreateTimeTo.Size = new Size(20, 17);
            lblSendMessageCreateTimeTo.TabIndex = 8;
            lblSendMessageCreateTimeTo.Text = "至";
            // 
            // dtpSendMessageCreateTimeEnd
            // 
            dtpSendMessageCreateTimeEnd.Format = DateTimePickerFormat.Short;
            dtpSendMessageCreateTimeEnd.Location = new Point(718, 10);
            dtpSendMessageCreateTimeEnd.Name = "dtpSendMessageCreateTimeEnd";
            dtpSendMessageCreateTimeEnd.Size = new Size(110, 23);
            dtpSendMessageCreateTimeEnd.TabIndex = 9;
            // 
            // btnSendMessageQuery
            // 
            btnSendMessageQuery.Location = new Point(840, 9);
            btnSendMessageQuery.Name = "btnSendMessageQuery";
            btnSendMessageQuery.Size = new Size(75, 26);
            btnSendMessageQuery.TabIndex = 10;
            btnSendMessageQuery.Text = "查询";
            btnSendMessageQuery.UseVisualStyleBackColor = true;
            btnSendMessageQuery.Click += btnSendMessageQuery_Click;
            // 
            // tabLog
            // 
            tabLog.Location = new Point(4, 26);
            tabLog.Name = "tabLog";
            tabLog.Padding = new Padding(3);
            tabLog.Size = new Size(1342, 591);
            tabLog.TabIndex = 3;
            tabLog.Text = "操作日志";
            tabLog.UseVisualStyleBackColor = true;
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
            tabLisConfig.ResumeLayout(false);
            tabLisConfig.PerformLayout();
            pnlLisConfigLeft.ResumeLayout(false);
            tabReceiveMessage.ResumeLayout(false);
            tabReceiveMessage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvReceiveMessage).EndInit();
            pnlReceiveMessageQuery.ResumeLayout(false);
            pnlReceiveMessageQuery.PerformLayout();
            tabSendMessage.ResumeLayout(false);
            tabSendMessage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSendMessage).EndInit();
            pnlSendMessageQuery.ResumeLayout(false);
            pnlSendMessageQuery.PerformLayout();
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
        private TabPage tabLisConfig;
        private TabPage tabReceiveMessage;
        private TabPage tabSendMessage;
        private TabPage tabLog;
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
        private Button button1;
        private DataGridViewCheckBoxColumn colReceiveMessageSelect;
        private DataGridViewTextBoxColumn colReceiveMessageStatus;
        private DataGridViewTextBoxColumn colReceiveMessageType;
        private DataGridViewTextBoxColumn colReceiveMessageRawMessage;
        private DataGridViewTextBoxColumn colReceiveMessageDecodeResult;
        private DataGridViewTextBoxColumn colReceiveMessageBarcode;
        private DataGridViewTextBoxColumn colReceiveMessageSampleNo;
        private DataGridViewTextBoxColumn colReceiveMessageCreateTime;
        private DataGridViewTextBoxColumn colReceiveMessageErrorMessage;
        private Panel pnlSendMessageQuery;
        private Label lblSendMessageStatus;
        private ComboBox cboSendMessageStatus;
        private Label lblSendMessageBarcode;
        private TextBox txtSendMessageBarcode;
        private Label lblSendMessageSampleNo;
        private TextBox txtSendMessageSampleNo;
        private Label lblSendMessageCreateTime;
        private DateTimePicker dtpSendMessageCreateTimeStart;
        private Label lblSendMessageCreateTimeTo;
        private DateTimePicker dtpSendMessageCreateTimeEnd;
        private Button btnSendMessageQuery;
        private DataGridView dgvSendMessage;
        private DeviceHubControl.PagerControl pagerSendMessage;
        private DataGridViewTextBoxColumn colSendMessageStatus;
        private DataGridViewTextBoxColumn colSendMessageSendJson;
        private DataGridViewTextBoxColumn colSendMessageSendContent;
        private DataGridViewTextBoxColumn colSendMessageBarcode;
        private DataGridViewTextBoxColumn colSendMessageSampleNo;
        private DataGridViewTextBoxColumn colSendMessageCreateTime;
        private DataGridViewTextBoxColumn colSendMessageErrorMessage;
    }
}
