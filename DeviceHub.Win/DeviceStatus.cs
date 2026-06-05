using DeviceHub.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DeviceHub.Win
{
    public partial class DeviceStatus : Form
    {
        public DeviceStatus()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IDeviceDriver d = DriverFactory.create();
            label1.Text = d.Test();
        }
    }
}
