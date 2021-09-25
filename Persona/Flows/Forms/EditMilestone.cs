using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Persona.Flows;
using Persona.Parameters;
using Persona.Collections;
using Persona.Conditions;
using Persona.Consequences;

namespace Persona
{
    public partial class EditMilestone : Form
    {
        public Milestone m_pMilestone;

        public EditMilestone(Milestone pMilestone, Module pModule)
        {
            InitializeComponent();
        
            m_pMilestone = pMilestone;

            textBox1.Text = m_pMilestone.m_sName;
            numericUpDown1.Value = (decimal)m_pMilestone.m_fPosition;

            conditionsList1.Bind(pModule);
            conditionsList1.Conditions = m_pMilestone.m_cConditions;

            consequencesList1.Bind(pModule);
            consequencesList1.Consequences = m_pMilestone.m_cConsequences;

            if (m_pMilestone.m_bRepeatable)
                radioButton1.Checked = true;
            else
                radioButton2.Checked = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            m_pMilestone.m_sName = textBox1.Text;
            m_pMilestone.m_fPosition = (float)numericUpDown1.Value;

            m_pMilestone.m_cConditions = conditionsList1.Conditions;

            m_pMilestone.m_cConsequences = consequencesList1.Consequences;

            m_pMilestone.m_bRepeatable = radioButton1.Checked;

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
