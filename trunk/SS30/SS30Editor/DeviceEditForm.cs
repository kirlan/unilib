using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SS30Conf;
using SS30Conf.Items;

namespace SS30Editor
{
    public partial class DeviceEditForm : Form
    {
        private CHouseholdMachinery m_pDevice;

        public DeviceEditForm(CHouseholdMachinery pDevice)
        {
            InitializeComponent();

            m_pDevice = pDevice;

            textBox1.Text = pDevice.Value;
            textBox2.Text = pDevice.Name;
            textBox3.Text = pDevice.Description;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m_pDevice.Value = textBox1.Text;
            m_pDevice.Name = textBox2.Text;
            m_pDevice.Description = textBox3.Text;

            DialogResult = DialogResult.OK;
        }
    }
}
