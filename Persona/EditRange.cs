using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Persona.Parameters;

namespace Persona
{
    public partial class EditRange : Form
    {
        NumericParameter.Range m_pRange;

        public EditRange(NumericParameter.Range pRange, float fMin, float fMax)
        {
            InitializeComponent();

            m_pRange = pRange;

            textBox1.Text = m_pRange.m_sDescription;

            numericUpDown1.Minimum = (decimal)fMin;
            numericUpDown2.Minimum = (decimal)fMin;
            numericUpDown1.Maximum = (decimal)fMax;
            numericUpDown2.Maximum = (decimal)fMax;

            numericUpDown1.Value = (decimal)m_pRange.m_fMin;
            numericUpDown2.Value = (decimal)m_pRange.m_fMax;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown1.Value > numericUpDown2.Value)
                numericUpDown2.Value = numericUpDown1.Value;

            numericUpDown2.Minimum = numericUpDown1.Value;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown2.Value < numericUpDown1.Value)
                numericUpDown1.Value = numericUpDown2.Value;

            numericUpDown1.Maximum = numericUpDown2.Value;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            m_pRange.m_sDescription = textBox1.Text;

            m_pRange.m_fMin = (float)numericUpDown1.Value;
            m_pRange.m_fMax = (float)numericUpDown2.Value;

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
