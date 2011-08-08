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
    public partial class GiftEditForm : Form
    {
        private CGift m_pGift;

        public GiftEditForm(CGift pGift)
        {
            InitializeComponent();

            m_pGift = pGift;

            textBox1.Text = pGift.Value;
            textBox2.Text = pGift.Name;
            textBox3.Text = pGift.Description;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m_pGift.Value = textBox1.Text;
            m_pGift.Name = textBox2.Text;
            m_pGift.Description = textBox3.Text;

            DialogResult = DialogResult.OK;
        }
    }
}
