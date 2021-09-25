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
    public partial class EditConsequenceCommand : Form
    {
        public SystemCommand m_pConsequence;

        public EditConsequenceCommand(SystemCommand pConsequence)
        {
            InitializeComponent();

            ParameterStringPanel.Location = new Point(0,0);
            ParameterStringPanel.Size = ParameterPanel.ClientSize;
            ParameterStringPanel.Visible = true;
            ParameterStringPanel.Enabled = false;

            m_pConsequence = pConsequence;

            switch (m_pConsequence.m_eAction)
            {
                case SystemCommand.ActionType.GameOver:
                    radioButton1.Checked = true;
                    break;
                case SystemCommand.ActionType.RandomRound:
                    radioButton2.Checked = true;
                    break;
                case SystemCommand.ActionType.Return:
                    radioButton3.Checked = true;
                    break;
                case SystemCommand.ActionType.SetDescription:
                    radioButton4.Checked = true;
                    ParameterStringPanel.Enabled = true;
                    ParameterTextBox.Text = m_pConsequence.m_pValue as string;
                    break;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
                m_pConsequence.m_eAction = SystemCommand.ActionType.GameOver;

            if (radioButton2.Checked)
                m_pConsequence.m_eAction = SystemCommand.ActionType.RandomRound;

            if (radioButton3.Checked)
                m_pConsequence.m_eAction = SystemCommand.ActionType.Return;

            if (radioButton4.Checked)
            {
                m_pConsequence.m_eAction = SystemCommand.ActionType.Return;
                m_pConsequence.m_pValue = ParameterTextBox.Text;
            }

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            ParameterStringPanel.Enabled = radioButton4.Checked;
        }
    }
}
