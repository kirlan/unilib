using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SS30Conf;
using SS30Conf.Actions;
using SS30Conf.Actions.Conditions;
using SS30Conf.Actions.Commands;
using SS30Conf.Items;

namespace SS30Editor
{
    public partial class CommandStatusEditForm : Form
    {
        private CCommandStatus m_pCommand;

        public CommandStatusEditForm(CCommandStatus pCommand)
        {
            InitializeComponent();

            m_pCommand = pCommand;

            textBox1.Text = pCommand.Value;

            if (pCommand.Subject == Subject.ACTOR)
                ActorRadioButton.Checked = true;
            else
                TargetRadioButton.Checked = true;

            comboBox1.Items.Clear();
            foreach (string status in CConfigRepository.Instance.Statuses)
            {
                comboBox1.Items.Add(status);
            }

            if (pCommand.Status == "")
                comboBox1.SelectedItem = null;
            else
                comboBox1.SelectedItem = pCommand.Status;

            if (pCommand.Enable)
                SetRadioButton.Checked = true;
            else
                ClearRadioButton.Checked = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m_pCommand.Value = textBox1.Text;

            if (ActorRadioButton.Checked)
                m_pCommand.Subject = Subject.ACTOR;

            if (TargetRadioButton.Checked)
                m_pCommand.Subject = Subject.TARGET;

            m_pCommand.Status = comboBox1.Text;

            m_pCommand.Enable = SetRadioButton.Checked;

            DialogResult = DialogResult.OK;
        }
    }
}
