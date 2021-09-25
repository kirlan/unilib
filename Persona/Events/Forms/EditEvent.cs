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
    public partial class EditEvent : Form
    {
        public Event m_pEvent;

        public EditEvent(Event pEvent, Module pModule)
        {
            InitializeComponent();

            m_pEvent = pEvent;

            textBox1.Text = m_pEvent.m_sID;

            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(pModule.m_cActions.ToArray());
            comboBox1.SelectedItem = m_pEvent.m_pAction;

            conditionsList1.Bind(pModule);
            conditionsList1.Conditions = pEvent.m_pDescription.m_cConditions;

            descriptionEdit1.Bind(pModule);
            descriptionEdit1.Text = pEvent.m_pDescription.m_sText;

            listView1.Items.Clear();
            foreach (var pAlternate in m_pEvent.m_cAlternateDescriptions)
            {
                string conditions = "";
                foreach (var pCondition in pAlternate.m_cConditions)
                {
                    if (conditions.Length > 0)
                        conditions += " И ";
                    conditions += pCondition.ToString();
                }

                ListViewItem pItem = new ListViewItem(conditions);
                pItem.SubItems.Add(pAlternate.m_sText);
                
                pItem.Tag = pAlternate;
                listView1.Items.Add(pItem);
            }

            consequencesList1.Bind(pModule);
            consequencesList1.Consequences = pEvent.m_cConsequences;

            numericUpDown1.Value = m_pEvent.m_iPriority;
            numericUpDown2.Value = m_pEvent.m_iProbability;

            if (m_pEvent.m_bRepeatable)
                radioButton1.Checked = true;
            else
                radioButton2.Checked = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            m_pEvent.m_sID = textBox1.Text;
            
            m_pEvent.m_pAction = comboBox1.SelectedItem as Action;
            
            m_pEvent.m_pDescription.m_cConditions = conditionsList1.Conditions;

            m_pEvent.m_pDescription.m_sText = descriptionEdit1.Text;

            m_pEvent.m_cAlternateDescriptions.Clear();
            foreach (ListViewItem pItem in listView1.Items)
                m_pEvent.m_cAlternateDescriptions.Add(pItem.Tag as Situation);

            m_pEvent.m_cConsequences = consequencesList1.Consequences;

            m_pEvent.m_iPriority = (int)numericUpDown1.Value;

            m_pEvent.m_iProbability = (int)numericUpDown2.Value;

            m_pEvent.m_bRepeatable = radioButton1.Checked;

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
