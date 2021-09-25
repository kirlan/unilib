using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Persona.Parameters;
using Persona.Conditions;
using Persona.Collections;
using Persona.Consequences;

namespace Persona
{
    public partial class EditRule : Form
    {
        public Function.Rule m_pRule;
        public Function m_pFunction;

        public EditRule(Function pFunction, Function.Rule pRule, Module pModule)
        {
            InitializeComponent();

            tableLayoutPanelNumeric.Location = new Point(0, 0);
            tableLayoutPanelNumeric.Size = panel1.ClientSize;

            tableLayoutPanelBool.Location = new Point(0, 0);
            tableLayoutPanelBool.Size = panel1.ClientSize;

            tableLayoutPanelString.Location = new Point(0, 0);
            tableLayoutPanelString.Size = panel1.ClientSize;

            m_pFunction = pFunction;
            m_pRule = pRule;

            textBox1.Text = m_pFunction.m_pParam.m_sName;

            conditionsList1.Bind(pModule);
            conditionsList1.Conditions = m_pRule.m_cConditions;

            if (m_pFunction.m_pParam is NumericParameter)
            {
                tableLayoutPanelBool.Visible = false;
                tableLayoutPanelString.Visible = false;

                float fNewValue = 0;
                float.TryParse((m_pRule.m_pConsequence as ParameterSet).m_sNewValue, out fNewValue);
                numericUpDown1.Value = (decimal)fNewValue;

                NumericParameter pParam = pFunction.m_pParam as NumericParameter;

                comboBox2.Items.Clear();
                comboBox2.Items.AddRange(pParam.m_cRanges.ToArray());

                foreach (var pRange in pParam.m_cRanges)
                {
                    if (fNewValue >= pRange.m_fMin && fNewValue <= pRange.m_fMax)
                    {
                        comboBox2.SelectedItem = pRange;
                        break;
                    }
                }
            }
            if (m_pFunction.m_pParam is BoolParameter)
            {
                tableLayoutPanelNumeric.Visible = false;
                tableLayoutPanelString.Visible = false;

                bool bNewValue = true;
                if (!bool.TryParse((m_pRule.m_pConsequence as ParameterSet).m_sNewValue, out bNewValue))
                {
                    float fNewValue = 0;
                    float.TryParse((m_pRule.m_pConsequence as ParameterSet).m_sNewValue, out fNewValue);
                    bNewValue = (fNewValue > 0);
                }

                if (bNewValue)
                    radioButton1.Checked = true;
                else
                    radioButton2.Checked = true;
            }
            if (m_pFunction.m_pParam is StringParameter)
            {
                tableLayoutPanelNumeric.Visible = false;
                tableLayoutPanelBool.Visible = false;

                descriptionEdit1.Bind(pModule);
                descriptionEdit1.Text = (m_pRule.m_pConsequence as ParameterSet).m_sNewValue;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem == null)
                return;

            NumericParameter.Range pRange = comboBox2.SelectedItem as NumericParameter.Range;
            if ((float)numericUpDown1.Value < pRange.m_fMin || (float)numericUpDown1.Value > pRange.m_fMax)
                numericUpDown1.Value = (decimal)(pRange.m_fMin + pRange.m_fMax) / 2;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            NumericParameter pParam = m_pFunction.m_pParam as NumericParameter;
            float fNewValue = (float)numericUpDown1.Value;

            foreach (var pRange in pParam.m_cRanges)
            {
                if (fNewValue >= pRange.m_fMin && fNewValue <= pRange.m_fMax)
                {
                    comboBox2.SelectedItem = pRange;
                    break;
                }
            }
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            m_pRule.m_cConditions = conditionsList1.Conditions;

            if (m_pFunction.m_pParam is NumericParameter)
            {
                (m_pRule.m_pConsequence as ParameterSet).m_sNewValue = numericUpDown1.Value.ToString();
            }
            if (m_pFunction.m_pParam is BoolParameter)
            {
                (m_pRule.m_pConsequence as ParameterSet).m_sNewValue = radioButton1.Checked.ToString();
            }
            if (m_pFunction.m_pParam is StringParameter)
            {
                (m_pRule.m_pConsequence as ParameterSet).m_sNewValue = descriptionEdit1.Text;
            } 

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
