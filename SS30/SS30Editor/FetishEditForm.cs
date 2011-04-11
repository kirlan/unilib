using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SS30Conf;

namespace SS30Editor
{
    public partial class FetishEditForm : Form
    {
        private CFetish m_pFetish;

        public FetishEditForm(CFetish pFetish)
        {
            InitializeComponent();

            m_pFetish = pFetish;

            textBox1.Text = pFetish.Value;
            textBox2.Text = pFetish.Name;
            textBox3.Text = pFetish.Description;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m_pFetish.Value = textBox1.Text;
            m_pFetish.Name = textBox2.Text;
            m_pFetish.Description = textBox3.Text;

            DialogResult = DialogResult.OK;
        }
    }
}
