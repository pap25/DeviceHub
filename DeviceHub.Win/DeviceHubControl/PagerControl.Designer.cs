namespace DeviceHub.Win.DeviceHubControl
{
    partial class PagerControl
    {
        private System.ComponentModel.IContainer? components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTotal = new Label();
            lblPageSize = new Label();
            cboPageSize = new ComboBox();
            btnFirst = new Button();
            btnPrev = new Button();
            flpPages = new FlowLayoutPanel();
            btnNext = new Button();
            btnLast = new Button();
            lblPagePrefix = new Label();
            txtPageJump = new TextBox();
            lblPageTotal = new Label();
            btnJump = new Button();
            SuspendLayout();
            //
            // lblTotal
            //
            lblTotal.AutoSize = true;
            lblTotal.Location = new Point(0, 8);
            lblTotal.Name = "lblTotal";
            lblTotal.Text = "共 0 条";
            //
            // lblPageSize
            //
            lblPageSize.AutoSize = true;
            lblPageSize.Location = new Point(90, 8);
            lblPageSize.Name = "lblPageSize";
            lblPageSize.Text = "每页";
            //
            // cboPageSize
            //
            cboPageSize.DropDownStyle = ComboBoxStyle.DropDownList;
            cboPageSize.Location = new Point(130, 4);
            cboPageSize.Name = "cboPageSize";
            cboPageSize.Size = new Size(60, 25);
            cboPageSize.TabIndex = 0;
            //
            // btnFirst
            //
            btnFirst.AutoSize = true;
            btnFirst.Location = new Point(210, 3);
            btnFirst.Name = "btnFirst";
            btnFirst.Padding = new Padding(4, 0, 4, 0);
            btnFirst.Text = "首页";
            btnFirst.UseVisualStyleBackColor = true;
            //
            // btnPrev
            //
            btnPrev.AutoSize = true;
            btnPrev.Location = new Point(260, 3);
            btnPrev.Name = "btnPrev";
            btnPrev.Padding = new Padding(4, 0, 4, 0);
            btnPrev.Text = "上一页";
            btnPrev.UseVisualStyleBackColor = true;
            //
            // flpPages
            //
            flpPages.AutoSize = true;
            flpPages.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpPages.Location = new Point(330, 3);
            flpPages.Name = "flpPages";
            flpPages.Size = new Size(0, 28);
            flpPages.WrapContents = false;
            //
            // btnNext
            //
            btnNext.AutoSize = true;
            btnNext.Location = new Point(340, 3);
            btnNext.Name = "btnNext";
            btnNext.Padding = new Padding(4, 0, 4, 0);
            btnNext.Text = "下一页";
            btnNext.UseVisualStyleBackColor = true;
            //
            // btnLast
            //
            btnLast.AutoSize = true;
            btnLast.Location = new Point(400, 3);
            btnLast.Name = "btnLast";
            btnLast.Padding = new Padding(4, 0, 4, 0);
            btnLast.Text = "末页";
            btnLast.UseVisualStyleBackColor = true;
            //
            // lblPagePrefix
            //
            lblPagePrefix.AutoSize = true;
            lblPagePrefix.Location = new Point(460, 8);
            lblPagePrefix.Name = "lblPagePrefix";
            lblPagePrefix.Text = "第";
            //
            // txtPageJump
            //
            txtPageJump.Location = new Point(485, 4);
            txtPageJump.Name = "txtPageJump";
            txtPageJump.Size = new Size(45, 23);
            txtPageJump.TextAlign = HorizontalAlignment.Center;
            txtPageJump.TabIndex = 1;
            //
            // lblPageTotal
            //
            lblPageTotal.AutoSize = true;
            lblPageTotal.Location = new Point(535, 8);
            lblPageTotal.Name = "lblPageTotal";
            lblPageTotal.Text = "/0";
            //
            // btnJump
            //
            btnJump.AutoSize = true;
            btnJump.Location = new Point(575, 3);
            btnJump.Name = "btnJump";
            btnJump.Padding = new Padding(4, 0, 4, 0);
            btnJump.Text = "跳转";
            btnJump.UseVisualStyleBackColor = true;
            //
            // PagerControl
            //
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Controls.Add(lblTotal);
            Controls.Add(lblPageSize);
            Controls.Add(cboPageSize);
            Controls.Add(btnFirst);
            Controls.Add(btnPrev);
            Controls.Add(flpPages);
            Controls.Add(btnNext);
            Controls.Add(btnLast);
            Controls.Add(lblPagePrefix);
            Controls.Add(txtPageJump);
            Controls.Add(lblPageTotal);
            Controls.Add(btnJump);
            MinimumSize = new Size(640, 32);
            Name = "PagerControl";
            Size = new Size(640, 32);
            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblTotal;
        private Label lblPageSize;
        private ComboBox cboPageSize;
        private Button btnFirst;
        private Button btnPrev;
        private FlowLayoutPanel flpPages;
        private Button btnNext;
        private Button btnLast;
        private Label lblPagePrefix;
        private TextBox txtPageJump;
        private Label lblPageTotal;
        private Button btnJump;
    }
}
