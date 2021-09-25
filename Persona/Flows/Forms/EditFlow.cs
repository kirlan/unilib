using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Persona.Flows;

namespace Persona
{
    public partial class EditFlow : Form
    {
        private Flow m_pFlow;

        public EditFlow(Flow pFlow)
        {
            InitializeComponent();

            m_pFlow = pFlow;

            textBox1.Text = m_pFlow.m_sName;

            textBox2.Text = m_pFlow.m_sComment;

            numericUpDown1.Value = (decimal)m_pFlow.m_fMajorProgress;
            numericUpDown2.Value = (decimal)m_pFlow.m_fStartPosition;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            m_pFlow.m_sName = textBox1.Text;
            m_pFlow.m_sComment = textBox2.Text;
            m_pFlow.m_fMajorProgress = (float)numericUpDown1.Value;
            m_pFlow.m_fStartPosition = (float)numericUpDown2.Value;

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
