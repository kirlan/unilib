using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Persona.Consequences;
using Persona.Flows;

namespace Persona
{
    public partial class EditConsequenceFlowProgress : Form
    {
        public FlowProgress m_pConsequence;

        public EditConsequenceFlowProgress(FlowProgress pConsequence, List<Flow> cFlows)
        {
            InitializeComponent();

            m_pConsequence = pConsequence;

            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(cFlows.ToArray());
            comboBox1.SelectedItem = m_pConsequence.m_pFlow;

            numericUpDown1.Value = (decimal)m_pConsequence.m_fProgress;

            if (pConsequence.m_pFlow != null)
                radioButton7.Text = "x " + pConsequence.m_pFlow.m_fMajorProgress.ToString();

            if (m_pConsequence.m_bMajor)
                radioButton7.Checked = true;
            else
                radioButton6.Checked = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            m_pConsequence.m_pFlow = comboBox1.SelectedItem as Flow;
            m_pConsequence.m_fProgress = (float)numericUpDown1.Value;

            m_pConsequence.m_bMajor = radioButton7.Checked;

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1)
                return;

            radioButton7.Text = "x " + (comboBox1.SelectedItem as Flow).m_fMajorProgress.ToString();
        }
    }
}
