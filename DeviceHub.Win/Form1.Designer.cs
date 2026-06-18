namespace DeviceHub.Win
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            dataGridView1 = new DataGridView();
            tabControl1 = new TabControl();
            tabPage5 = new TabPage();
            pictureBox1 = new PictureBox();
            tabPage1 = new TabPage();
            label5 = new Label();
            comboBox4 = new ComboBox();
            button1 = new Button();
            textBox1 = new TextBox();
            label2 = new Label();
            button2 = new Button();
            label1 = new Label();
            comboBox1 = new ComboBox();
            tabPage2 = new TabPage();
            textBox2 = new TextBox();
            label3 = new Label();
            button5 = new Button();
            label4 = new Label();
            comboBox2 = new ComboBox();
            dataGridView3 = new DataGridView();
            dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn6 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn7 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn8 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn9 = new DataGridViewTextBoxColumn();
            tabPage4 = new TabPage();
            textBox4 = new TextBox();
            label7 = new Label();
            comboBox3 = new ComboBox();
            label6 = new Label();
            button4 = new Button();
            dataGridView2 = new DataGridView();
            dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
            Column8 = new DataGridViewCheckBoxColumn();
            状态 = new DataGridViewTextBoxColumn();
            Column3 = new DataGridViewTextBoxColumn();
            Column1 = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            Column4 = new DataGridViewTextBoxColumn();
            Column5 = new DataGridViewTextBoxColumn();
            Column6 = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            tabControl1.SuspendLayout();
            tabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView3).BeginInit();
            tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Column8, 状态, Column3, Column1, Column2, Column4, Column5, Column6 });
            dataGridView1.Location = new Point(3, 90);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(1047, 454);
            dataGridView1.TabIndex = 0;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage5);
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage4);
            tabControl1.Location = new Point(12, 12);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1058, 574);
            tabControl1.TabIndex = 1;
            // 
            // tabPage5
            // 
            tabPage5.Controls.Add(pictureBox1);
            tabPage5.Location = new Point(4, 26);
            tabPage5.Name = "tabPage5";
            tabPage5.Size = new Size(1050, 544);
            tabPage5.TabIndex = 4;
            tabPage5.Text = "LIS参数配置";
            tabPage5.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(-4, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(1054, 541);
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(label5);
            tabPage1.Controls.Add(comboBox4);
            tabPage1.Controls.Add(button1);
            tabPage1.Controls.Add(textBox1);
            tabPage1.Controls.Add(label2);
            tabPage1.Controls.Add(button2);
            tabPage1.Controls.Add(label1);
            tabPage1.Controls.Add(comboBox1);
            tabPage1.Controls.Add(dataGridView1);
            tabPage1.Location = new Point(4, 26);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(1050, 544);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "接收仪器数据队列";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(209, 37);
            label5.Name = "label5";
            label5.Size = new Size(32, 17);
            label5.TabIndex = 8;
            label5.Text = "类型";
            // 
            // comboBox4
            // 
            comboBox4.FormattingEnabled = true;
            comboBox4.Items.AddRange(new object[] { "全部", "检验结果", "查询样本申请信息" });
            comboBox4.Location = new Point(247, 32);
            comboBox4.Name = "comboBox4";
            comboBox4.Size = new Size(121, 25);
            comboBox4.TabIndex = 7;
            // 
            // button1
            // 
            button1.Location = new Point(905, 34);
            button1.Name = "button1";
            button1.Size = new Size(129, 23);
            button1.TabIndex = 6;
            button1.Text = "重新解码并同步LIS";
            button1.TextAlign = ContentAlignment.MiddleRight;
            button1.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(486, 34);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(148, 23);
            textBox1.TabIndex = 5;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(406, 37);
            label2.Name = "label2";
            label2.Size = new Size(44, 17);
            label2.TabIndex = 4;
            label2.Text = "条形码";
            // 
            // button2
            // 
            button2.Location = new Point(769, 31);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 3;
            button2.Text = "查询";
            button2.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(16, 31);
            label1.Name = "label1";
            label1.Size = new Size(32, 17);
            label1.TabIndex = 2;
            label1.Text = "状态";
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "全部", "待处理", "已处理", "处理失败" });
            comboBox1.Location = new Point(54, 29);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(121, 25);
            comboBox1.TabIndex = 1;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(textBox2);
            tabPage2.Controls.Add(label3);
            tabPage2.Controls.Add(button5);
            tabPage2.Controls.Add(label4);
            tabPage2.Controls.Add(comboBox2);
            tabPage2.Controls.Add(dataGridView3);
            tabPage2.Location = new Point(4, 26);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(1050, 544);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "发送仪器数据队列";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(323, 18);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(148, 23);
            textBox2.TabIndex = 12;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(243, 21);
            label3.Name = "label3";
            label3.Size = new Size(44, 17);
            label3.TabIndex = 11;
            label3.Text = "条形码";
            // 
            // button5
            // 
            button5.Location = new Point(606, 15);
            button5.Name = "button5";
            button5.Size = new Size(75, 23);
            button5.TabIndex = 10;
            button5.Text = "查询";
            button5.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(15, 17);
            label4.Name = "label4";
            label4.Size = new Size(32, 17);
            label4.TabIndex = 9;
            label4.Text = "状态";
            // 
            // comboBox2
            // 
            comboBox2.FormattingEnabled = true;
            comboBox2.Items.AddRange(new object[] { "全部", "待处理", "已处理", "处理失败" });
            comboBox2.Location = new Point(79, 17);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(121, 25);
            comboBox2.TabIndex = 8;
            // 
            // dataGridView3
            // 
            dataGridView3.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView3.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn2, dataGridViewTextBoxColumn5, dataGridViewTextBoxColumn6, dataGridViewTextBoxColumn7, dataGridViewTextBoxColumn8, dataGridViewTextBoxColumn9 });
            dataGridView3.Location = new Point(2, 76);
            dataGridView3.Name = "dataGridView3";
            dataGridView3.Size = new Size(1047, 454);
            dataGridView3.TabIndex = 7;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewTextBoxColumn2.HeaderText = "状态";
            dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            dataGridViewTextBoxColumn2.Width = 80;
            // 
            // dataGridViewTextBoxColumn5
            // 
            dataGridViewTextBoxColumn5.HeaderText = "原始报文";
            dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            dataGridViewTextBoxColumn5.Width = 280;
            // 
            // dataGridViewTextBoxColumn6
            // 
            dataGridViewTextBoxColumn6.HeaderText = "编码结果";
            dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            dataGridViewTextBoxColumn6.Width = 300;
            // 
            // dataGridViewTextBoxColumn7
            // 
            dataGridViewTextBoxColumn7.HeaderText = "条形码";
            dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            // 
            // dataGridViewTextBoxColumn8
            // 
            dataGridViewTextBoxColumn8.HeaderText = "创建时间";
            dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            // 
            // dataGridViewTextBoxColumn9
            // 
            dataGridViewTextBoxColumn9.HeaderText = "失败原因";
            dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            // 
            // tabPage4
            // 
            tabPage4.Controls.Add(textBox4);
            tabPage4.Controls.Add(label7);
            tabPage4.Controls.Add(comboBox3);
            tabPage4.Controls.Add(label6);
            tabPage4.Controls.Add(button4);
            tabPage4.Controls.Add(dataGridView2);
            tabPage4.Location = new Point(4, 26);
            tabPage4.Name = "tabPage4";
            tabPage4.Padding = new Padding(3);
            tabPage4.Size = new Size(1050, 544);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "操作日志";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // textBox4
            // 
            textBox4.Location = new Point(270, 18);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(148, 23);
            textBox4.TabIndex = 16;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(220, 21);
            label7.Name = "label7";
            label7.Size = new Size(32, 17);
            label7.TabIndex = 15;
            label7.Text = "内容";
            // 
            // comboBox3
            // 
            comboBox3.FormattingEnabled = true;
            comboBox3.Items.AddRange(new object[] { "DEBUG", "INFO", "WARN", "ERROR" });
            comboBox3.Location = new Point(51, 15);
            comboBox3.Name = "comboBox3";
            comboBox3.Size = new Size(121, 25);
            comboBox3.TabIndex = 14;
            comboBox3.Text = "全部";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(13, 18);
            label6.Name = "label6";
            label6.Size = new Size(32, 17);
            label6.TabIndex = 13;
            label6.Text = "等级";
            // 
            // button4
            // 
            button4.Location = new Point(614, 18);
            button4.Name = "button4";
            button4.Size = new Size(75, 23);
            button4.TabIndex = 10;
            button4.Text = "查询";
            button4.UseVisualStyleBackColor = true;
            // 
            // dataGridView2
            // 
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn1, dataGridViewTextBoxColumn3, dataGridViewTextBoxColumn4 });
            dataGridView2.Location = new Point(2, 76);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.Size = new Size(1047, 454);
            dataGridView2.TabIndex = 7;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewTextBoxColumn1.HeaderText = "等级";
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewTextBoxColumn3.HeaderText = "内容";
            dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // dataGridViewTextBoxColumn4
            // 
            dataGridViewTextBoxColumn4.HeaderText = "时间";
            dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
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
            // Column3
            // 
            Column3.HeaderText = "类型";
            Column3.Name = "Column3";
            // 
            // Column1
            // 
            Column1.HeaderText = "原始报文";
            Column1.Name = "Column1";
            Column1.Width = 240;
            // 
            // Column2
            // 
            Column2.HeaderText = "解码结果";
            Column2.Name = "Column2";
            Column2.Width = 240;
            // 
            // Column4
            // 
            Column4.HeaderText = "条形码";
            Column4.Name = "Column4";
            // 
            // Column5
            // 
            Column5.HeaderText = "创建时间";
            Column5.Name = "Column5";
            // 
            // Column6
            // 
            Column6.HeaderText = "失败原因";
            Column6.Name = "Column6";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1074, 592);
            Controls.Add(tabControl1);
            Name = "Form1";
            Text = "DeviceHub v1  AUTOLAS 流水线Autolas 446";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            tabControl1.ResumeLayout(false);
            tabPage5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView3).EndInit();
            tabPage4.ResumeLayout(false);
            tabPage4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridView1;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private ComboBox comboBox1;
        private Button button2;
        private Label label1;
        private TextBox textBox1;
        private Label label2;
        private Button button1;
        private TabPage tabPage4;
        private Button button4;
        private DataGridView dataGridView2;
        private ComboBox comboBox3;
        private Label label6;
        private TextBox textBox4;
        private Label label7;
        private TabPage tabPage5;
        private PictureBox pictureBox1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridView dataGridView3;
        private Label label5;
        private ComboBox comboBox4;
        private TextBox textBox2;
        private Label label3;
        private Button button5;
        private Label label4;
        private ComboBox comboBox2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private DataGridViewCheckBoxColumn Column8;
        private DataGridViewTextBoxColumn 状态;
        private DataGridViewTextBoxColumn Column3;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column4;
        private DataGridViewTextBoxColumn Column5;
        private DataGridViewTextBoxColumn Column6;
    }
}