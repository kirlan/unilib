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

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
