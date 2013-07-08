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
    public partial class EditConditionRange : Form
    {
        ConditionRange m_pCondition;

        public EditConditionRange(ConditionRange pCondition, List<NumericParameter> cParameters)
        {
            InitializeComponent();

            m_pCondition = pCondition;

            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(cParameters.ToArray());

            comboBox1.SelectedItem = m_pCondition.m_pParam1;
            comboBox1_SelectedIndexChanged(this, new EventArgs());

            numericUpDown1.Minimum = (decimal)(m_pCondition.m_pParam1 as NumericParameter).m_fMin;
            numericUpDown2.Minimum = (decimal)(m_pCondition.m_pParam1 as NumericParameter).m_fMin;
            numericUpDown1.Maximum = (decimal)(m_pCondition.m_pParam1 as NumericParameter).m_fMax;
            numericUpDown2.Maximum = (decimal)(m_pCondition.m_pParam1 as NumericParameter).m_fMax;

            numericUpDown1.Value = (decimal)m_pCondition.m_fMinValue;
            numericUpDown2.Value = (decimal)m_pCondition.m_fMaxValue;

            numericUpDown1_ValueChanged(this, new EventArgs());
            numericUpDown2_ValueChanged(this, new EventArgs());

            if (m_pCondition.m_bNot)
                radioButton6.Checked = true;
            else
                radioButton7.Checked = true;
        }

        private void SelectRange()
        {
            if (comboBox1.SelectedItem == null)
                return;
            
            comboBox2.SelectedItem = null;
            
            NumericParameter pParam = comboBox1.SelectedItem as NumericParameter;

            foreach (var pRange in pParam.m_cRanges)
                if (pRange.m_fMin == (float)numericUpDown1.Value && pRange.m_fMax == (float)numericUpDown2.Value)
                {
                    comboBox2.SelectedItem = pRange;
                    break;
                }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button2.Enabled = (comboBox1.SelectedItem != null);

            comboBox2.Items.Clear();
            if (comboBox1.SelectedItem != null)
            {
                NumericParameter pParam = comboBox1.SelectedItem as NumericParameter;

                numericUpDown1.Minimum = (decimal)pParam.m_fMin;
                numericUpDown2.Minimum = (decimal)pParam.m_fMin;
                numericUpDown1.Maximum = (decimal)pParam.m_fMax;
                numericUpDown2.Maximum = (decimal)pParam.m_fMax;

                comboBox2.Items.AddRange(pParam.m_cRanges.ToArray());
                SelectRange();
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown1.Value > numericUpDown2.Value)
                numericUpDown2.Value = numericUpDown1.Value;

            numericUpDown2.Minimum = numericUpDown1.Value;
            SelectRange();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown2.Value < numericUpDown1.Value)
                numericUpDown1.Value = numericUpDown2.Value;

            numericUpDown1.Maximum = numericUpDown2.Value;
            SelectRange();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            m_pCondition.m_pParam1 = comboBox1.SelectedItem as Parameter;

            m_pCondition.m_fMinValue = (float)numericUpDown1.Value;
            m_pCondition.m_fMaxValue = (float)numericUpDown2.Value;

            m_pCondition.m_bNot = radioButton6.Checked;

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem == null)
                return;

            NumericParameter.Range pRange = comboBox2.SelectedItem as NumericParameter.Range;

            numericUpDown1.Minimum = decimal.MinValue;
            numericUpDown1.Maximum = decimal.MaxValue;

            numericUpDown2.Minimum = decimal.MinValue;
            numericUpDown2.Maximum = decimal.MaxValue;
            
            numericUpDown1.Value = (decimal)pRange.m_fMin;
            //numericUpDown1_ValueChanged(sender, e);
            
            numericUpDown2.Value = (decimal)pRange.m_fMax;
            //numericUpDown2_ValueChanged(sender, e);
        }
    }
}
