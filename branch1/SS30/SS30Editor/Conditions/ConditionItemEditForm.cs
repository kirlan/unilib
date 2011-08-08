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
using SS30Conf.Items;

namespace SS30Editor
{
    public partial class ConditionItemEditForm : Form
    {
        private CConditionItem m_pCondition;

        public ConditionItemEditForm(CConditionItem pCondition)
        {
            InitializeComponent();

            m_pCondition = pCondition;

            textBox1.Text = pCondition.Value;

            if (pCondition.Not)
                NotRadioButton.Checked = true;
            else
                YesRadioButton.Checked = true;

            comboBox1.Items.Clear();
            foreach (CItem item in MainForm.m_pWorld.Items)
            {
                comboBox1.Items.Add(item.Value);
            }

            if (pCondition.Item == null)
                comboBox1.SelectedItem = null;
            else
                comboBox1.SelectedItem = pCondition.Item.Value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m_pCondition.Value = textBox1.Text;

            m_pCondition.Not = NotRadioButton.Checked;

            if (comboBox1.SelectedItem == null)
                m_pCondition.Item = null;
            else
            {
                foreach (CItem item in MainForm.m_pWorld.Items)
                {
                    if (comboBox1.SelectedItem as string == item.Value)
                    {
                        m_pCondition.Item = item;
                        break;
                    }
                }
            }

            DialogResult = DialogResult.OK;
        }
    }
}
