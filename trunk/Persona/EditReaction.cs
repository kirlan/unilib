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

namespace Persona
{
    public partial class EditReaction : Form
    {
        public Reaction m_pReaction;

        public List<NumericParameter> m_cNumeric = new List<NumericParameter>();
        public List<BoolParameter> m_cBool = new List<BoolParameter>();
        public List<StringParameter> m_cString = new List<StringParameter>();

        public EditReaction(Reaction pReaction, List<NumericParameter> cNumeric, List<BoolParameter> cBool, List<StringParameter> cString)
        {
            InitializeComponent();

            m_pReaction = pReaction;

            m_cNumeric = cNumeric;
            m_cBool = cBool;
            m_cString = cString;

            textBox1.Text = m_pReaction.m_sName;
            
            listBox1.Items.Clear();
            listBox1.Items.AddRange(m_pReaction.m_cConditions.ToArray());

            textBox2.Text = m_pReaction.m_sResult;

            listBox2.Items.Clear();
            listBox2.Items.AddRange(m_pReaction.m_cConsequences.ToArray());

            if (m_pReaction.m_bAlwaysVisible)
                radioButton1.Checked = true;
            else
                radioButton2.Checked = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            m_pReaction.m_sName = textBox1.Text;

            m_pReaction.m_cConditions.Clear();
            foreach (Condition pCondition in listBox1.Items)
                m_pReaction.m_cConditions.Add(pCondition);

            m_pReaction.m_sResult = textBox2.Text;

            m_pReaction.m_cConsequences.Clear();
            foreach (Consequence pConsequence in listBox2.Items)
                m_pReaction.m_cConsequences.Add(pConsequence);

            m_pReaction.m_bAlwaysVisible = radioButton1.Checked;

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void AddConditionComparsionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Parameter pParam1 = null;
            Parameter pParam2 = null;

            if (m_cNumeric.Count > 0)
                pParam1 = m_cNumeric[0];
            if (m_cNumeric.Count > 1)
                pParam2 = m_cNumeric[1];

            ConditionComparsion pCondition = new ConditionComparsion(pParam1, pParam2);

            EditConditionComparsion pForm = new EditConditionComparsion(pCondition, m_cNumeric);
            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                listBox1.Items.Add(pCondition);
            }
        }

        private void AddConditionRangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Parameter pParam = null;

            if (m_cNumeric.Count > 0)
                pParam = m_cNumeric[0];

            ConditionRange pCondition = new ConditionRange(pParam);

            EditConditionRange pForm = new EditConditionRange(pCondition, m_cNumeric);
            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                listBox1.Items.Add(pCondition);
            }
        }

        private void AddConditionStatusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Parameter pParam = null;

            if (m_cBool.Count > 0)
                pParam = m_cBool[0];

            ConditionStatus pCondition = new ConditionStatus(pParam);

            EditConditionStatus pForm = new EditConditionStatus(pCondition, m_cBool);
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
                pForm = new EditConditionComparsion(pCondition as ConditionComparsion, m_cNumeric);
            }
            if (pCondition is ConditionRange)
            {
                pForm = new EditConditionRange(pCondition as ConditionRange, m_cNumeric);
            }
            if (pCondition is ConditionStatus)
            {
                pForm = new EditConditionStatus(pCondition as ConditionStatus, m_cBool);
            }

            if (pForm != null && pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                listBox1.Refresh();
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

        private void EditConsequenceToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem == null)
                return;

            if (listBox2.SelectedItem is ParameterSet)
            {
                ParameterSet pConsequence = listBox2.SelectedItem as ParameterSet;
                EditConsequenceSet pForm = new EditConsequenceSet(pConsequence, m_cNumeric, m_cBool, m_cString);
                if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    listBox2.Refresh();
            }
            if (listBox2.SelectedItem is ParameterChange)
            {
                ParameterChange pConsequence = listBox2.SelectedItem as ParameterChange;
                EditConsequenceChange pForm = new EditConsequenceChange(pConsequence, m_cNumeric);
                if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    listBox2.Refresh();
            }
            if (listBox2.SelectedItem is SystemCommand)
            {
                SystemCommand pConsequence = listBox2.SelectedItem as SystemCommand;
                EditConsequenceCommand pForm = new EditConsequenceCommand(pConsequence);
                if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    listBox2.Refresh();
            }
        }

        private void AddConsequenceSetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Parameter pParam = null;
            string sValue = "";
            if (m_cNumeric.Count > 0)
            {
                pParam = m_cNumeric[0];
                sValue = "0";
            }
            else if (m_cBool.Count > 0)
            {
                pParam = m_cBool[0];
                sValue = true.ToString();
            }
            else if (m_cString.Count > 0)
                pParam = m_cString[0];

            if (pParam == null)
                return;

            ParameterSet pConsequence = new ParameterSet(pParam, sValue);
            EditConsequenceSet pForm = new EditConsequenceSet(pConsequence, m_cNumeric, m_cBool, m_cString);
            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                listBox2.Items.Add(pConsequence);
            }
        }

        private void DeleteConsequenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem == null)
                return;

            listBox2.Items.Remove(listBox2.SelectedItem);
        }

        private void AddConsequenceChangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_cNumeric.Count == 0)
                return;

            ParameterChange pConsequence = new ParameterChange(m_cNumeric[0], 1);
            EditConsequenceChange pForm = new EditConsequenceChange(pConsequence, m_cNumeric);
            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                listBox2.Items.Add(pConsequence);
            }
        }

        private void AddConsequenceCommandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SystemCommand pConsequence = new SystemCommand(SystemCommand.ActionType.GameOver);
            EditConsequenceCommand pForm = new EditConsequenceCommand(pConsequence);
            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                listBox2.Items.Add(pConsequence);
            }
        }

        private int m_iFocusedConsequence = -1;

        private void listBox2_MouseDown(object sender, MouseEventArgs e)
        {
            if (m_iFocusedConsequence != -1 && m_iFocusedConsequence < listBox2.Items.Count)
                if (listBox2.GetItemRectangle(m_iFocusedConsequence).Contains(e.Location))
                    return;
            
            m_iFocusedConsequence = -1;

            for (int i = 0; i < listBox2.Items.Count; i++)
            {
                if (listBox2.GetItemRectangle(i).Contains(e.Location))
                {
                    m_iFocusedConsequence = i;
                    break;
                }
            }

            listBox2.SelectedIndex = m_iFocusedConsequence;
        }
    }
}
