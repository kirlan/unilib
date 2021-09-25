using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Persona.Parameters;
using Persona.Collections;
using Persona.Consequences;
using Persona.Conditions;

namespace Persona
{
    public partial class EditRuleNumericFormula : Form
    {
        private ParameterChangeVariable m_pConsequence;
        public Function.Rule m_pRule;
        public Function m_pFunction;

        public EditRuleNumericFormula(Function pFunction, Function.Rule pRule, Module pModule)
        {
            InitializeComponent();

            m_pFunction = pFunction;
            m_pRule = pRule;
            m_pConsequence = m_pRule.m_pConsequence as ParameterChangeVariable;

            textBox1.Text = m_pFunction.m_pParam.m_sName;

            conditionsList1.Bind(pModule);
            conditionsList1.Conditions = m_pRule.m_cConditions;

            numericParameterComboBox1.Bind(pModule);
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
                //case NumericParameter.Operation.SET:
                //    radioButton3.Checked = true;
                //    break;
                case NumericParameter.Operation.AVG:
                    radioButton4.Checked = true;
                    break;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            m_pRule.m_cConditions = conditionsList1.Conditions;

            m_pConsequence.m_pParam1 = numericParameterComboBox1.Parameter;
            m_pConsequence.m_pParam2 = numericParameterComboBox2.Parameter;
            if (radioButton1.Checked)
                m_pConsequence.m_eOperation = NumericParameter.Operation.ADD;
            if (radioButton2.Checked)
                m_pConsequence.m_eOperation = NumericParameter.Operation.SUB;
            //if (radioButton3.Checked)
            //    m_pConsequence.m_eOperation = NumericParameter.Operation.SET;
            if (radioButton4.Checked)
                m_pConsequence.m_eOperation = NumericParameter.Operation.AVG;

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
