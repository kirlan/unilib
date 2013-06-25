using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Persona.Conditions;
using Persona.Parameters;

namespace Persona
{
    public partial class EditConditionComparsion : Form
    {
        public ConditionComparsion m_pCondition;

        public EditConditionComparsion(ConditionComparsion pCondition, List<NumericParameter> cParameters)
        {
            InitializeComponent();

            m_pCondition = pCondition;

            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(cParameters.ToArray());

            comboBox2.Items.Clear();
            comboBox2.Items.AddRange(cParameters.ToArray());

            comboBox1.SelectedItem = m_pCondition.m_pParam1;
            comboBox2.SelectedItem = m_pCondition.m_pParam2;

            comboBox1_SelectedIndexChanged(this, new EventArgs());

            switch (m_pCondition.m_eType)
            {
                case ConditionComparsion.ComparsionType.LOLO:
                    radioButton1.Checked = true;
                    break;
                case ConditionComparsion.ComparsionType.HIHI:
                    radioButton2.Checked = true;
                    break;
                case ConditionComparsion.ComparsionType.LO:
                    radioButton3.Checked = true;
                    break;
                case ConditionComparsion.ComparsionType.HI:
                    radioButton4.Checked = true;
                    break;
                case ConditionComparsion.ComparsionType.EQ:
                    radioButton5.Checked = true;
                    break;
            }

            if (m_pCondition.m_bNot)
                radioButton6.Checked = true;
            else
                radioButton7.Checked = true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button2.Enabled = (comboBox1.SelectedItem != comboBox2.SelectedItem) && (comboBox1.SelectedItem != null) && (comboBox2.SelectedItem != null);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            m_pCondition.m_pParam1 = comboBox1.SelectedItem as Parameter;
            m_pCondition.m_pParam1 = comboBox1.SelectedItem as Parameter;

            if (radioButton1.Checked)
                m_pCondition.m_eType = ConditionComparsion.ComparsionType.LOLO;
            if (radioButton2.Checked)
                m_pCondition.m_eType = ConditionComparsion.ComparsionType.HIHI;
            if (radioButton3.Checked)
                m_pCondition.m_eType = ConditionComparsion.ComparsionType.LO;
            if (radioButton4.Checked)
                m_pCondition.m_eType = ConditionComparsion.ComparsionType.HI;
            if (radioButton5.Checked)
                m_pCondition.m_eType = ConditionComparsion.ComparsionType.EQ;

            m_pCondition.m_bNot = radioButton6.Checked;

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
