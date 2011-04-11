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
    public partial class ConditionPersonalAppealEditForm : Form
    {
        private CConditionPersonalAppeal m_pCondition;

        public ConditionPersonalAppealEditForm(CConditionPersonalAppeal pCondition)
        {
            InitializeComponent();

            m_pCondition = pCondition;

            textBox1.Text = pCondition.Value;

            if (pCondition.Not)
                NotRadioButton.Checked = true;
            else
                YesRadioButton.Checked = true;

            switch (pCondition.PersonalAppeal)
            {
                case PersonalAppeal.HATE:
                    disp1.Checked = true;
                    break;
                case PersonalAppeal.DISLIKE:
                    disp2.Checked = true;
                    break;
                case PersonalAppeal.FLAT:
                    disp3.Checked = true;
                    break;
                case PersonalAppeal.LIKE:
                    disp4.Checked = true;
                    break;
                case PersonalAppeal.ADMIRE:
                    disp5.Checked = true;
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m_pCondition.Value = textBox1.Text;

            m_pCondition.Not = NotRadioButton.Checked;

            if (disp1.Checked)
                m_pCondition.PersonalAppeal = PersonalAppeal.HATE;
            if (disp2.Checked)
                m_pCondition.PersonalAppeal = PersonalAppeal.DISLIKE;
            if (disp3.Checked)
                m_pCondition.PersonalAppeal = PersonalAppeal.FLAT;
            if (disp4.Checked)
                m_pCondition.PersonalAppeal = PersonalAppeal.LIKE;
            if (disp5.Checked)
                m_pCondition.PersonalAppeal = PersonalAppeal.ADMIRE;

            DialogResult = DialogResult.OK;
        }
    }
}
