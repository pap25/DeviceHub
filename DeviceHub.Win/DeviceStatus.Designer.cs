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
            lblConfig = new Label();
            lblErrorMsg = new Label();
            SuspendLayout();
            // 
            // lblConfig
            // 
            lblConfig.AutoSize = true;
            lblConfig.Location = new Point(47, 35);
            lblConfig.Name = "lblConfig";
            lblConfig.Size = new Size(53, 17);
            lblConfig.TabIndex = 2;
            lblConfig.Text = "加载中...";
            // 
            // lblErrorMsg
            // 
            lblErrorMsg.AutoSize = true;
            lblErrorMsg.ForeColor = Color.Red;
            lblErrorMsg.Location = new Point(244, 35);
            lblErrorMsg.Name = "lblErrorMsg";
            lblErrorMsg.Size = new Size(0, 17);
            lblErrorMsg.TabIndex = 3;
            // 
            // DeviceStatus
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(lblErrorMsg);
            Controls.Add(lblConfig);
            Name = "DeviceStatus";
            Text = "DeviceStatus";
            Shown += DeviceStatus_Shown;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblConfig;
        private Button button1;
        private Label label1;
        private Label lbllblMessage;
        private Label lblErrorMsg;
    }
}