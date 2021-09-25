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

namespace Persona
{
    public partial class EditRule : Form
    {
        public Function.Rule m_pRule;
        public Function m_pFunction;

        public List<NumericParameter> m_cNumeric = new List<NumericParameter>();
        public List<BoolParameter> m_cBool = new List<BoolParameter>();
        public List<StringParameter> m_cString = new List<StringParameter>();

        public EditRule(Function pFunction, Function.Rule pRule, List<NumericParameter> cNumeric, List<BoolParameter> cBool, List<StringParameter> cString)
        {
            InitializeComponent();

            tableLayoutPanelNumeric.Location = new Point(0, 0);
            tableLayoutPanelNumeric.Size = panel1.ClientSize;

            tableLayoutPanelBool.Location = new Point(0, 0);
            tableLayoutPanelBool.Size = panel1.ClientSize;

            tableLayoutPanelString.Location = new Point(0, 0);
            tableLayoutPanelString.Size = panel1.ClientSize;

            m_cNumeric = cNumeric;
            m_cBool = cBool;
            m_cString = cString;

            m_pFunction = pFunction;
            m_pRule = pRule;

            textBox1.Text = m_pFunction.m_pParam.m_sName;

            listBox1.Items.Clear();
            listBox1.Items.AddRange(m_pRule.m_cConditions.ToArray());

            if (m_pFunction.m_pParam is NumericParameter)
            {
                tableLayoutPanelBool.Visible = false;
                tableLayoutPanelString.Visible = false;

                float fNewValue = 0;
                float.TryParse(m_pRule.m_sNewValue, out fNewValue);
                numericUpDown1.Value = (decimal)fNewValue;

                NumericParameter pParam = pFunction.m_pParam as NumericParameter;

                comboBox2.Items.Clear();
                comboBox2.Items.AddRange(pParam.m_cRanges.ToArray());

                foreach (var pRange in pParam.m_cRanges)
                {
                    if (fNewValue >= pRange.m_fMin && fNewValue <= pRange.m_fMax)
                    {
                        comboBox2.SelectedItem = pRange;
                        break;
                    }
                }
            }
            if (m_pFunction.m_pParam is BoolParameter)
            {
                tableLayoutPanelNumeric.Visible = false;
                tableLayoutPanelString.Visible = false;

                bool bNewValue = true;
                if (!bool.TryParse(m_pRule.m_sNewValue, out bNewValue))
                {
                    float fNewValue = 0;
                    float.TryParse(m_pRule.m_sNewValue, out fNewValue);
                    bNewValue = (fNewValue > 0);
                }

                if (bNewValue)
                    radioButton1.Checked = true;
                else
                    radioButton2.Checked = true;
            }
            if (m_pFunction.m_pParam is StringParameter)
            {
                tableLayoutPanelNumeric.Visible = false;
                tableLayoutPanelBool.Visible = false;

                textBox2.Text = m_pRule.m_sNewValue;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem == null)
                return;

            NumericParameter.Range pRange = comboBox2.SelectedItem as NumericParameter.Range;
            if ((float)numericUpDown1.Value < pRange.m_fMin || (float)numericUpDown1.Value > pRange.m_fMax)
                numericUpDown1.Value = (decimal)(pRange.m_fMin + pRange.m_fMax) / 2;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            NumericParameter pParam = m_pFunction.m_pParam as NumericParameter;
            float fNewValue = (float)numericUpDown1.Value;

            foreach (var pRange in pParam.m_cRanges)
            {
                if (fNewValue >= pRange.m_fMin && fNewValue <= pRange.m_fMax)
                {
                    comboBox2.SelectedItem = pRange;
                    break;
                }
            }
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            m_pRule.m_cConditions.Clear();
            foreach (Condition pCondition in listBox1.Items)
                m_pRule.m_cConditions.Add(pCondition);

            if (m_pFunction.m_pParam is NumericParameter)
            {
                m_pRule.m_sNewValue = numericUpDown1.Value.ToString();
            }
            if (m_pFunction.m_pParam is BoolParameter)
            {
                m_pRule.m_sNewValue = radioButton1.Checked.ToString();
            }
            if (m_pFunction.m_pParam is StringParameter)
            {
                m_pRule.m_sNewValue = textBox1.Text;
            } 

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
    }
}
