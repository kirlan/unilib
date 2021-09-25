using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Persona.Conditions;
using Persona.Parameters;
using Persona.Collections;

namespace Persona.Core.Controls
{
    public partial class ConditionsList : UserControl
    {
        public ConditionsList()
        {
            InitializeComponent();
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<Condition> Conditions
        {
            set
            {
                listBox1.Items.Clear();
                listBox1.Items.AddRange(value.ToArray());
            }
            get
            {
                List<Condition> cConditions = new List<Condition>();
                foreach (Condition pCondition in listBox1.Items)
                    cConditions.Add(pCondition);

                return cConditions;
            }
        }

        public Module m_pModule = null;

        public void Bind(Module pModule)
        {
            m_pModule = pModule;
        }

        private void AddConditionComparsionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Parameter pParam1 = null;
            Parameter pParam2 = null;

            if (m_pModule.m_cNumericParameters.Count > 0)
                pParam1 = m_pModule.m_cNumericParameters[0];
            if (m_pModule.m_cNumericParameters.Count > 1)
                pParam2 = m_pModule.m_cNumericParameters[1];

            ConditionComparsion pCondition = new ConditionComparsion(pParam1, pParam2);

            EditConditionComparsion pForm = new EditConditionComparsion(pCondition, m_pModule);
            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                listBox1.Items.Add(pCondition);
            }
        }

        private void AddConditionRangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Parameter pParam = null;

            if (m_pModule.m_cNumericParameters.Count > 0)
                pParam = m_pModule.m_cNumericParameters[0];

            ConditionRange pCondition = new ConditionRange(pParam);

            EditConditionRange pForm = new EditConditionRange(pCondition, m_pModule);
            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                listBox1.Items.Add(pCondition);
            }
        }

        private void AddConditionStatusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Parameter pParam = null;

            if (m_pModule.m_cBoolParameters.Count > 0)
                pParam = m_pModule.m_cBoolParameters[0];

            ConditionStatus pCondition = new ConditionStatus(pParam);

            EditConditionStatus pForm = new EditConditionStatus(pCondition, m_pModule);
            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                listBox1.Items.Add(pCondition);
            }
        }

        private void AddConditionObjectSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ObjectsCollection pColl = null;

            if (m_pModule.m_cCollections.Count > 0)
                pColl = m_pModule.m_cCollections[0];

            ConditionObjectSelected pCondition = new ConditionObjectSelected(pColl, pColl.m_cObjects.ElementAt(0).Value.m_iID);

            EditConditionObjectSelected pForm = new EditConditionObjectSelected(pCondition, m_pModule.m_cCollections);
            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                listBox1.Items.Add(pCondition);
            }
        }

        private void EditConditionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
                return;

            Form pForm = null;

            Condition pCondition = listBox1.SelectedItem as Condition;

            if (pCondition is ConditionComparsion)
            {
                pForm = new EditConditionComparsion(pCondition as ConditionComparsion, m_pModule);
            }
            if (pCondition is ConditionRange)
            {
                pForm = new EditConditionRange(pCondition as ConditionRange, m_pModule);
            }
            if (pCondition is ConditionStatus)
            {
                pForm = new EditConditionStatus(pCondition as ConditionStatus, m_pModule);
            }
            if (pCondition is ConditionObjectSelected)
            {
                pForm = new EditConditionObjectSelected(pCondition as ConditionObjectSelected, m_pModule.m_cCollections);
            }

            if (pForm != null && pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                LISTBOX.Refresh(listBox1);
            }
        }

        private int m_iFocusedCondition = -1;

        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (m_iFocusedCondition != -1 && m_iFocusedCondition < listBox1.Items.Count)
                if (listBox1.GetItemRectangle(m_iFocusedCondition).Contains(e.Location))
                    return;

            m_iFocusedCondition = -1;

            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                if (listBox1.GetItemRectangle(i).Contains(e.Location))
                {
                    m_iFocusedCondition = i;
                    break;
                }
            }

            listBox1.SelectedIndex = m_iFocusedCondition;
        }

        private void RemoveConditionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
                return;

            listBox1.Items.Remove(listBox1.SelectedItem);
        }
    }
}
