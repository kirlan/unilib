using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SS30Conf;
using SS30Conf.Actions;
using SS30Conf.Actions.Conditions;
using SS30Conf.Actions.Commands;
using SS30Conf.Items;

namespace SS30Editor
{
    public partial class CommodityEditForm : Form
    {
        private CCommodity m_pCommodity;

        public CommodityEditForm(CCommodity pCommodity)
        {
            InitializeComponent();

            m_pCommodity = pCommodity;

            textBox1.Text = pCommodity.Value;

            comboBox1.Items.Clear();
            foreach (CItem item in MainForm.m_pWorld.Items)
            {
                comboBox1.Items.Add(item.Value);
            }

            if (pCommodity.Item == null)
                comboBox1.SelectedItem = null;
            else
                comboBox1.SelectedItem = pCommodity.Item.Value;

            numericUpDown1.Value = pCommodity.Cost;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m_pCommodity.Value = textBox1.Text;

            if (comboBox1.SelectedItem == null)
                m_pCommodity.Item = null;
            else
            {
                foreach (CItem item in MainForm.m_pWorld.Items)
                {
                    if (comboBox1.SelectedItem as string == item.Value)
                    {
                        m_pCommodity.Item = item;
                        break;
                    }
                }
            }

            m_pCommodity.Cost = (int)numericUpDown1.Value;

            DialogResult = DialogResult.OK;
        }
    }
}
