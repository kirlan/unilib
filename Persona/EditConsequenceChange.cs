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

        public EditConsequenceChange(ParameterChange pConsequence, List<NumericParameter> cNumeric)
        {
            InitializeComponent();

            numericUpDown1.Minimum = decimal.MinValue;
            numericUpDown1.Maximum = decimal.MaxValue;

            m_pConsequence = pConsequence;

            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(cNumeric.ToArray());
            comboBox1.SelectedItem = m_pConsequence.m_pParam;

            numericUpDown1.Value = (decimal)m_pConsequence.m_fDelta;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            m_pConsequence.m_pParam = comboBox1.SelectedItem as NumericParameter;
            m_pConsequence.m_fDelta = (float)numericUpDown1.Value;

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
