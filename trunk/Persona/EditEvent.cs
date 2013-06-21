using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Persona.Consequences;

namespace Persona
{
    public partial class EditEvent : Form
    {
        public Event m_pEvent;

        public EditEvent(Event pEvent, List<Domain> cDomains)
        {
            InitializeComponent();

            m_pEvent = pEvent;

            textBox1.Text = m_pEvent.m_sID;

            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(cDomains.ToArray());
            comboBox1.SelectedItem = m_pEvent.m_pDomain;

            listBox1.Items.Clear();
            listBox1.Items.AddRange(m_pEvent.m_pDescription.m_cConditions.ToArray());

            textBox2.Text = m_pEvent.m_pDescription.m_sText;

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

            listBox2.Items.Clear();
            listBox2.Items.AddRange(m_pEvent.m_cConsequences.ToArray());

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
            
            m_pEvent.m_pDomain = comboBox1.SelectedItem as Domain;
            
            m_pEvent.m_pDescription.m_cConditions.Clear();
            foreach (Condition pCondition in listBox1.Items)
                m_pEvent.m_pDescription.m_cConditions.Add(pCondition);

            m_pEvent.m_pDescription.m_sText = textBox2.Text;

            m_pEvent.m_cAlternateDescriptions.Clear();
            foreach (ListViewItem pItem in listView1.Items)
                m_pEvent.m_cAlternateDescriptions.Add(pItem.Tag as Situation);

            m_pEvent.m_cConsequences.Clear();
            foreach (Consequence pConsequence in listBox2.Items)
                m_pEvent.m_cConsequences.Add(pConsequence);

            m_pEvent.m_iPriority = (int)numericUpDown1.Value;

            m_pEvent.m_iProbability = (int)numericUpDown2.Value;

            m_pEvent.m_bRepeatable = radioButton1.Checked;

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
