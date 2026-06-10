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
            lblMessage = new Label();
            SuspendLayout();
            // 
            // lblMessage
            // 
            lblMessage.AutoSize = true;
            lblMessage.Location = new Point(47, 35);
            lblMessage.Name = "lblMessage";
            lblMessage.Size = new Size(53, 17);
            lblMessage.TabIndex = 2;
            lblMessage.Text = "加载中...";
            // 
            // DeviceStatus
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(lblMessage);
            Name = "DeviceStatus";
            Text = "DeviceStatus";
            Shown += DeviceStatus_Shown;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblMessage;
        private Button button1;
        private Label label1;
    }
}