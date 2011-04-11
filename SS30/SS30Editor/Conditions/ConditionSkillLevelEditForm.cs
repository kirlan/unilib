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
    public partial class ConditionSkillEditForm : Form
    {
        private CConditionSkillLevel m_pCondition;

        public ConditionSkillEditForm(CConditionSkillLevel pCondition)
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
            foreach (CSkill skill in MainForm.m_pWorld.Skills)
            {
                comboBox1.Items.Add(skill.Value);
            }

            if (pCondition.Skill == null)
                comboBox1.SelectedItem = null;
            else
                comboBox1.SelectedItem = pCondition.Skill.Value;

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
                m_pCondition.Skill = null;
            else
            {
                foreach (CSkill skill in MainForm.m_pWorld.Skills)
                {
                    if (comboBox1.SelectedItem as string == skill.Value)
                    {
                        m_pCondition.Skill = skill;
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
