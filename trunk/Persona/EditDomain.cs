using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Persona
{
    public partial class EditDomain : Form
    {
        public Domain m_pDomain;

        public EditDomain(Domain pDomain)
        {
            InitializeComponent();

            m_pDomain = pDomain;

            textBox1.Text = pDomain.m_sName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            m_pDomain.m_sName = textBox1.Text;

            DialogResult = DialogResult.OK;
        }
    }
}
