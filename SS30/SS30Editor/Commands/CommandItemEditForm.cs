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
    public partial class CommandItemEditForm : Form
    {
        private CCommandItem m_pCommand;

        public CommandItemEditForm(CCommandItem pCommand)
        {
            InitializeComponent();

            m_pCommand = pCommand;

            textBox1.Text = pCommand.Value;

            comboBox1.Items.Clear();
            foreach (CItem item in MainForm.m_pWorld.Items)
            {
                comboBox1.Items.Add(item.Value);
            }

            if (pCommand.Item == null)
                comboBox1.SelectedItem = null;
            else
                comboBox1.SelectedItem = pCommand.Item.Value;

            numericUpDown1.Value = pCommand.AmountChange;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m_pCommand.Value = textBox1.Text;

            if (comboBox1.SelectedItem == null)
                m_pCommand.Item = null;
            else
            {
                foreach (CItem item in MainForm.m_pWorld.Items)
                {
                    if (comboBox1.SelectedItem as string == item.Value)
                    {
                        m_pCommand.Item = item;
                        break;
                    }
                }
            }

            m_pCommand.AmountChange = (int)numericUpDown1.Value;

            DialogResult = DialogResult.OK;
        }
    }
}
