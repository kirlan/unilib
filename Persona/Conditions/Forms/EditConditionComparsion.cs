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
using Persona.Collections;

namespace Persona
{
    public partial class EditConditionComparsion : Form
    {
        public ConditionComparsion m_pCondition;

        public EditConditionComparsion(ConditionComparsion pCondition, Module pModule)
        {
            InitializeComponent();

            m_pCondition = pCondition;

            numericParameterComboBox1.Bind(pModule);
            numericParameterComboBox1.Parameter = m_pCondition.m_pParam1;

            numericParameterComboBox2.Bind(pModule);
            numericParameterComboBox2.Parameter = m_pCondition.m_pParam2;

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
            button2.Enabled = (numericParameterComboBox1.Parameter != numericParameterComboBox2.Parameter) &&
                (numericParameterComboBox1.Parameter != null) && 
                (numericParameterComboBox2.Parameter != null);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            m_pCondition.m_pParam1 = numericParameterComboBox1.Parameter;
            m_pCondition.m_pParam2 = numericParameterComboBox2.Parameter;

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
