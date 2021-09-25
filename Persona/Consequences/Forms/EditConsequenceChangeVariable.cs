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
using Persona.Collections;

namespace Persona
{
    public partial class EditConsequenceChangeVariable : Form
    {
        public ParameterChangeVariable m_pConsequence;

        public EditConsequenceChangeVariable(ParameterChangeVariable pConsequence, Module pModule)
        {
            InitializeComponent();

            m_pConsequence = pConsequence;

            numericParameterComboBox1.Bind(pModule, true);
            numericParameterComboBox1.Parameter = m_pConsequence.m_pParam1;

            numericParameterComboBox2.Bind(pModule);
            numericParameterComboBox2.Parameter = m_pConsequence.m_pParam2;

            switch (m_pConsequence.m_eOperation)
            {
                case NumericParameter.Operation.ADD:
                    radioButton1.Checked = true;
                    break;
                case NumericParameter.Operation.SUB:
                    radioButton2.Checked = true;
                    break;
                case NumericParameter.Operation.SET:
                    radioButton3.Checked = true;
                    break;
                case NumericParameter.Operation.AVG:
                    radioButton4.Checked = true;
                    break;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            m_pConsequence.m_pParam1 = numericParameterComboBox1.Parameter as NumericParameter;
            m_pConsequence.m_pParam2 = numericParameterComboBox2.Parameter as NumericParameter;
            if (radioButton1.Checked)
                m_pConsequence.m_eOperation = NumericParameter.Operation.ADD;
            if (radioButton2.Checked)
                m_pConsequence.m_eOperation = NumericParameter.Operation.SUB;
            if (radioButton3.Checked)
                m_pConsequence.m_eOperation = NumericParameter.Operation.SET;
            if (radioButton4.Checked)
                m_pConsequence.m_eOperation = NumericParameter.Operation.AVG;

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
