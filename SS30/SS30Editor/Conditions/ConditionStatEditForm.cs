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

namespace SS30Editor
{
    public partial class ConditionStatEditForm : Form
    {
        private CConditionStatLevel m_pCondition;

        public ConditionStatEditForm(CConditionStatLevel pCondition)
        {
            InitializeComponent();

            m_pCondition = pCondition;

            textBox1.Text = pCondition.Value;

            if (pCondition.Not)
                NotRadioButton.Checked = true;
            else
                YesRadioButton.Checked = true;

            if (pCondition.Subject == Subject.ACTOR)
                ActorRadioButton.Checked = true;
            else
                TargetRadioButton.Checked = true;

            comboBox1.Items.Clear();
            foreach (CPersonStat stat in MainForm.m_pWorld.Stats.Values)
            {
                comboBox1.Items.Add(stat.Value);
            }

            if (pCondition.Stat == null)
                comboBox1.SelectedItem = null;
            else
                comboBox1.SelectedItem = pCondition.Stat.Value;

            numericUpDown1.Value = pCondition.LowTreshold;
            numericUpDown2.Value = pCondition.HiTreshold;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m_pCondition.Value = textBox1.Text;

            m_pCondition.Not = NotRadioButton.Checked;

            if (ActorRadioButton.Checked)
                m_pCondition.Subject = Subject.ACTOR;

            if (TargetRadioButton.Checked)
                m_pCondition.Subject = Subject.TARGET;

            if (comboBox1.SelectedItem == null)
                m_pCondition.Stat = null;
            else
            {
                foreach (CPersonStat stat in MainForm.m_pWorld.Stats.Values)
                {
                    if (comboBox1.SelectedItem as string == stat.Value)
                    {
                        m_pCondition.Stat = stat;
                        break;
                    }
                }
            }

            m_pCondition.LowTreshold = (int)numericUpDown1.Value;
            m_pCondition.HiTreshold = (int)numericUpDown2.Value;

            DialogResult = DialogResult.OK;
        }
    }
}
