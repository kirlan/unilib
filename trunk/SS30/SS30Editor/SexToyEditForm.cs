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
    public partial class SexToyEditForm : Form
    {
        private CSexToy m_pSexToy;

        public SexToyEditForm(CSexToy pSexToy)
        {
            InitializeComponent();

            m_pSexToy = pSexToy;

            textBox1.Text = pSexToy.Value;
            textBox2.Text = pSexToy.Name;
            textBox3.Text = pSexToy.Description;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m_pSexToy.Value = textBox1.Text;
            m_pSexToy.Name = textBox2.Text;
            m_pSexToy.Description = textBox3.Text;

            DialogResult = DialogResult.OK;
        }
    }
}
