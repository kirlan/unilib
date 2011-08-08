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
    public partial class ConditionRndEditForm : Form
    {
        private CConditionRnd m_pCondition;

        public ConditionRndEditForm(CConditionRnd pCondition)
        {
            InitializeComponent();

            m_pCondition = pCondition;

            textBox1.Text = pCondition.Value;

            if (pCondition.Not)
                NotRadioButton.Checked = true;
            else
                YesRadioButton.Checked = true;

            numericUpDown1.Value = pCondition.Chances;
            numericUpDown2.Value = pCondition.Total;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m_pCondition.Value = textBox1.Text;

            m_pCondition.Not = NotRadioButton.Checked;

            m_pCondition.Total = (int)numericUpDown2.Value;
            m_pCondition.Chances = (int)numericUpDown1.Value;

            DialogResult = DialogResult.OK;
        }
    }
}
