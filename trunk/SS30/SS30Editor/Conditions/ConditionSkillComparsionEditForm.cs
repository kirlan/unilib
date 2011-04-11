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
    public partial class ConditionSkillComparsionEditForm : Form
    {
        private CConditionSkillComparsion m_pCondition;

        public ConditionSkillComparsionEditForm(CConditionSkillComparsion pCondition)
        {
            InitializeComponent();

            m_pCondition = pCondition;

            textBox1.Text = pCondition.Value;

            if (pCondition.Not)
                NotRadioButton.Checked = true;
            else
                YesRadioButton.Checked = true;

            comboBox1.Items.Clear();
            foreach (CSkill skill in MainForm.m_pWorld.Skills)
            {
                comboBox1.Items.Add(skill.Value);
            }

            if (pCondition.Skill == null)
                comboBox1.SelectedItem = null;
            else
                comboBox1.SelectedItem = pCondition.Skill.Value;

            switch (pCondition.ComparsionType)
            {
                case ComparsionType.LOLO:
                    lolo.Checked = true;
                    break;
                case ComparsionType.LO:
                    lo.Checked = true;
                    break;
                case ComparsionType.EQ:
                    eq.Checked = true;
                    break;
                case ComparsionType.HI:
                    hi.Checked = true;
                    break;
                case ComparsionType.HIHI:
                    hihi.Checked = true;
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m_pCondition.Value = textBox1.Text;

            m_pCondition.Not = NotRadioButton.Checked;

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

            if (lolo.Checked)
                m_pCondition.ComparsionType = ComparsionType.LOLO;
            if (lo.Checked)
                m_pCondition.ComparsionType = ComparsionType.LO;
            if (eq.Checked)
                m_pCondition.ComparsionType = ComparsionType.EQ;
            if (hi.Checked)
                m_pCondition.ComparsionType = ComparsionType.HI;
            if (hihi.Checked)
                m_pCondition.ComparsionType = ComparsionType.HIHI;

            DialogResult = DialogResult.OK;
        }
    }
}
