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

namespace SS30Editor
{
    public partial class CommandChangeStatEditForm : Form
    {
        private CCommandChangeStat m_pCommand;

        public CommandChangeStatEditForm(CCommandChangeStat pCommand)
        {
            InitializeComponent();

            m_pCommand = pCommand;

            textBox1.Text = pCommand.Value;

            if (pCommand.Subject == Subject.ACTOR)
                ActorRadioButton.Checked = true;
            else
                TargetRadioButton.Checked = true;

            comboBox1.Items.Clear();
            foreach (CPersonStat stat in MainForm.m_pWorld.Stats.Values)
            {
                comboBox1.Items.Add(stat.Value);
            }

            if (pCommand.Stat == null)
                comboBox1.SelectedItem = null;
            else
                comboBox1.SelectedItem = pCommand.Stat.Value;

            numericUpDown1.Value = pCommand.ValueChange;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m_pCommand.Value = textBox1.Text;

            if (ActorRadioButton.Checked)
                m_pCommand.Subject = Subject.ACTOR;

            if (TargetRadioButton.Checked)
                m_pCommand.Subject = Subject.TARGET;

            if (comboBox1.SelectedItem == null)
                m_pCommand.Stat = null;
            else
            {
                foreach (CPersonStat stat in MainForm.m_pWorld.Stats.Values)
                {
                    if (comboBox1.SelectedItem as string == stat.Value)
                    {
                        m_pCommand.Stat = stat;
                        break;
                    }
                }
            }

            m_pCommand.ValueChange = (int)numericUpDown1.Value;

            DialogResult = DialogResult.OK;
        }
    }
}
