using DeviceHub.Utils;
using DeviceHub.Lis;
using DeviceHub.Lis.Dto;
using DeviceHub.Lis.Impl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DeviceHub.Win
{
    public partial class Form1 : Form
    {
        private readonly ILisClient lisClient = LisClient.Instance;
        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox4.SelectedIndex = 1;

            
            dataGridView4.DataSource = lisClient.GetInstrumentItemMappingPage(111, 10, 1).Result.Data;
        }

        private void 从LIS同步ToolStripMenuItem_Click(object sender, EventArgs e)
        {
               
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
