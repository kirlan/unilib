using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Persona.Consequences;
using Persona.Parameters;

namespace Persona
{
    public partial class EditConsequenceSet : Form
    {
        public ParameterSet m_pConsequence;

        public EditConsequenceSet(ParameterSet pConsequence, List<NumericParameter> cNumeric, List<BoolParameter> cBool, List<StringParameter> cString)
        {
            InitializeComponent();

            numericUpDown1.Minimum = decimal.MinValue;
            numericUpDown1.Maximum = decimal.MaxValue;

            m_pConsequence = pConsequence;

            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(cNumeric.ToArray());

            comboBox3.Items.Clear();
            comboBox3.Items.AddRange(cBool.ToArray());

            comboBox5.Items.Clear();
            comboBox5.Items.AddRange(cString.ToArray());

            if (m_pConsequence.m_pParam is NumericParameter)
            {
                tabControl1.SelectedIndex = 0;
                comboBox1.SelectedItem = m_pConsequence.m_pParam;
                
                float fNewValue = 0;
                float.TryParse(m_pConsequence.m_sNewValue, out fNewValue);
                numericUpDown1.Value = (decimal)fNewValue;

                NumericParameter pParam = m_pConsequence.m_pParam as NumericParameter;

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
            if (m_pConsequence.m_pParam is BoolParameter)
            {
                tabControl1.SelectedIndex = 1;
                comboBox3.SelectedItem = m_pConsequence.m_pParam;

                bool bNewValue = true;
                if (!bool.TryParse(m_pConsequence.m_sNewValue, out bNewValue))
                {
                    float fNewValue = 0;
                    float.TryParse(m_pConsequence.m_sNewValue, out fNewValue);
                    bNewValue = (fNewValue > 0);
                }

                if (bNewValue)
                    radioButton1.Checked = true;
                else
                    radioButton2.Checked = true;
            }
            if (m_pConsequence.m_pParam is StringParameter)
            {
                tabControl1.SelectedIndex = 2;
                comboBox5.SelectedItem = m_pConsequence.m_pParam;

                textBox1.Text = m_pConsequence.m_sNewValue;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem == null)
                return;

            NumericParameter.Range pRange = comboBox2.SelectedItem as NumericParameter.Range;
            if((float)numericUpDown1.Value < pRange.m_fMin || (float)numericUpDown1.Value > pRange.m_fMax)
                numericUpDown1.Value = (decimal)(pRange.m_fMin + pRange.m_fMax)/2;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            NumericParameter pParam = m_pConsequence.m_pParam as NumericParameter;
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
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    m_pConsequence.m_pParam = comboBox1.SelectedItem as NumericParameter;
                    m_pConsequence.m_sNewValue = numericUpDown1.Value.ToString();
                    break;
                case 1:
                    m_pConsequence.m_pParam = comboBox3.SelectedItem as BoolParameter;
                    m_pConsequence.m_sNewValue = radioButton1.Checked.ToString();
                    break;
                case 2:
                    m_pConsequence.m_pParam = comboBox5.SelectedItem as BoolParameter;
                    m_pConsequence.m_sNewValue = textBox1.Text;
                    break;
            }

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            NumericParameter pParam = comboBox1.SelectedItem as NumericParameter;

            comboBox2.Items.Clear();
            comboBox2.Items.AddRange(pParam.m_cRanges.ToArray());

            foreach (var pRange in pParam.m_cRanges)
            {
                if ((float)numericUpDown1.Value >= pRange.m_fMin && (float)numericUpDown1.Value <= pRange.m_fMax)
                {
                    comboBox2.SelectedItem = pRange;
                    break;
                }
            }
        }
    }
}
