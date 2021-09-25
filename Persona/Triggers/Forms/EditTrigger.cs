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
using Persona.Consequences;
using Persona.Collections;
using Persona.Flows;

namespace Persona
{
    public partial class EditTrigger : Form
    {
        public Trigger m_pTrigger;

        public EditTrigger(Trigger pTrigger, Module pModule)
        {
            InitializeComponent();
        
            m_pTrigger = pTrigger;

            textBox1.Text = m_pTrigger.m_sID;

            conditionsList1.Bind(pModule);
            conditionsList1.Conditions = m_pTrigger.m_cConditions;

            consequencesList1.Bind(pModule);
            consequencesList1.Consequences = m_pTrigger.m_cConsequences;

            if (m_pTrigger.m_bRepeatable)
                radioButton1.Checked = true;
            else
                radioButton2.Checked = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            m_pTrigger.m_sID = textBox1.Text;

            m_pTrigger.m_cConditions = conditionsList1.Conditions;

            m_pTrigger.m_cConsequences = consequencesList1.Consequences;

            m_pTrigger.m_bRepeatable = radioButton1.Checked;

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
