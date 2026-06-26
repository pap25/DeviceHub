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
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            dgvInstrumentItemMapping = new DataGridView();
            Column1 = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            Column3 = new DataGridViewTextBoxColumn();
            Column4 = new DataGridViewTextBoxColumn();
            Column5 = new DataGridViewTextBoxColumn();
            grpAuthInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvInstrumentItemMapping).BeginInit();
            SuspendLayout();
            // 
            // grpAuthInfo
            // 
            grpAuthInfo.Controls.Add(label3);
            grpAuthInfo.Controls.Add(label2);
            grpAuthInfo.Controls.Add(label1);
            grpAuthInfo.Location = new Point(12, 26);
            grpAuthInfo.Name = "grpAuthInfo";
            grpAuthInfo.Size = new Size(270, 246);
            grpAuthInfo.TabIndex = 0;
            grpAuthInfo.TabStop = false;
            grpAuthInfo.Text = "授权信息";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(34, 177);
            label3.Name = "label3";
            label3.Size = new Size(43, 17);
            label3.TabIndex = 2;
            label3.Text = "label3";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(34, 113);
            label2.Name = "label2";
            label2.Size = new Size(43, 17);
            label2.TabIndex = 1;
            label2.Text = "label2";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(34, 51);
            label1.Name = "label1";
            label1.Size = new Size(44, 17);
            label1.TabIndex = 0;
            label1.Text = "授权码";
            // 
            // dgvInstrumentItemMapping
            // 
            dgvInstrumentItemMapping.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvInstrumentItemMapping.Columns.AddRange(new DataGridViewColumn[] { Column1, Column2, Column3, Column4, Column5 });
            dgvInstrumentItemMapping.Location = new Point(414, 52);
            dgvInstrumentItemMapping.Name = "dgvInstrumentItemMapping";
            dgvInstrumentItemMapping.Size = new Size(739, 150);
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
            // DeviceStatus
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1192, 551);
            Controls.Add(dgvInstrumentItemMapping);
            Controls.Add(grpAuthInfo);
            Name = "DeviceStatus";
            Text = "DeviceStatus";
            Shown += DeviceStatus_Shown;
            grpAuthInfo.ResumeLayout(false);
            grpAuthInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvInstrumentItemMapping).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private Button button1;
        private Label label1;
        private Label lbllblMessage;
        private GroupBox grpAuthInfo;
        private DataGridView dgvInstrumentItemMapping;
        private Label label3;
        private Label label2;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column3;
        private DataGridViewTextBoxColumn Column4;
        private DataGridViewTextBoxColumn Column5;
    }
}