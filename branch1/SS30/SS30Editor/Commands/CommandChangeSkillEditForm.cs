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
    public partial class CommandChangeSkillEditForm : Form
    {
        private CCommandChangeSkill m_pCommand;

        public CommandChangeSkillEditForm(CCommandChangeSkill pCommand)
        {
            InitializeComponent();

            m_pCommand = pCommand;

            textBox1.Text = pCommand.Value;

            if (pCommand.Subject == Subject.ACTOR)
                ActorRadioButton.Checked = true;
            else
                TargetRadioButton.Checked = true;

            comboBox1.Items.Clear();
            foreach (CSkill skill in MainForm.m_pWorld.Skills)
            {
                comboBox1.Items.Add(skill.Value);
            }

            if (pCommand.Skill == null)
                comboBox1.SelectedItem = null;
            else
                comboBox1.SelectedItem = pCommand.Skill.Value;

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
                m_pCommand.Skill = null;
            else
            {
                foreach (CSkill skill in MainForm.m_pWorld.Skills)
                {
                    if (comboBox1.SelectedItem as string == skill.Value)
                    {
                        m_pCommand.Skill = skill;
                        break;
                    }
                }
            }

            m_pCommand.ValueChange = (int)numericUpDown1.Value;

            DialogResult = DialogResult.OK;
        }
    }
}
