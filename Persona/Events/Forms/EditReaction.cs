using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Persona.Consequences;
using Persona.Conditions;
using Persona.Parameters;
using Persona.Collections;
using Persona.Flows;

namespace Persona
{
    public partial class EditReaction : Form
    {
        public Reaction m_pReaction;

        public EditReaction(Reaction pReaction, Module pModule)
        {
            InitializeComponent();

            m_pReaction = pReaction;

            conditionsList1.Bind(pModule);
            consequencesList1.Bind(pModule);
            descriptionEdit1.Bind(pModule);

            conditionsList1.Conditions = pReaction.m_cConditions;
            consequencesList1.Consequences = pReaction.m_cConsequences;

            textBox1.Text = m_pReaction.m_sName;

            descriptionEdit1.Text = pReaction.m_sText;

            if (m_pReaction.m_bAlwaysVisible)
                radioButton1.Checked = true;
            else
                radioButton2.Checked = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            m_pReaction.m_sName = textBox1.Text;

            m_pReaction.m_cConditions = conditionsList1.Conditions;
            m_pReaction.m_sText = descriptionEdit1.Text;
            m_pReaction.m_cConsequences = consequencesList1.Consequences;

            m_pReaction.m_bAlwaysVisible = radioButton1.Checked;

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
