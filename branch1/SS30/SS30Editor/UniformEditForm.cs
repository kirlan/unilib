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
    public partial class UniformEditForm : Form
    {
        private CUniform m_pUniform;

        public UniformEditForm(CUniform pUniform)
        {
            InitializeComponent();

            m_pUniform = pUniform;

            textBox1.Text = pUniform.Value;
            textBox2.Text = pUniform.Name;
            textBox3.Text = pUniform.Description;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m_pUniform.Value = textBox1.Text;
            m_pUniform.Name = textBox2.Text;
            m_pUniform.Description = textBox3.Text;

            DialogResult = DialogResult.OK;
        }
    }
}
