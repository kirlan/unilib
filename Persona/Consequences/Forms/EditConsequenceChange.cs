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
    public partial class EditConsequenceChange : Form
    {
        public ParameterChange m_pConsequence;

        public EditConsequenceChange(ParameterChange pConsequence, Module pModule)
        {
            InitializeComponent();

            numericUpDown1.Minimum = decimal.MinValue;
            numericUpDown1.Maximum = decimal.MaxValue;

            m_pConsequence = pConsequence;

            numericParameterComboBox1.Bind(pModule, true);
            numericParameterComboBox1.Parameter = m_pConsequence.m_pParam;

            numericUpDown1.Value = (decimal)m_pConsequence.m_fDelta;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            m_pConsequence.m_pParam = numericParameterComboBox1.Parameter as NumericParameter;
            m_pConsequence.m_fDelta = (float)numericUpDown1.Value;

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
