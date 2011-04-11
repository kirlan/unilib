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

namespace SS30Editor
{
    public partial class LeisureEditForm : Form
    {
        private CLeisure m_pLeisure;

        public LeisureEditForm(CLeisure pLeisure)
        {
            InitializeComponent();

            m_pLeisure = pLeisure;

            textBox1.Text = pLeisure.Value;
            textBox2.Text = pLeisure.Name;
            textBox3.Text = pLeisure.Description;

            comboBox1.Items.Clear();
            comboBox1.Items.Add("none");
            foreach (CAction action in MainForm.m_pWorld.Actions)
            {
                comboBox1.Items.Add(action.Value);
            }

            if (pLeisure.Action == null)
                comboBox1.SelectedItem = "none";
            else
                comboBox1.SelectedItem = pLeisure.Action.Value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m_pLeisure.Value = textBox1.Text;
            m_pLeisure.Name = textBox2.Text;
            m_pLeisure.Description = textBox3.Text;

            if (comboBox1.SelectedItem as string == "none")
                m_pLeisure.Action = null;
            else
            {
                foreach (CAction action in MainForm.m_pWorld.Actions)
                {
                    if (comboBox1.SelectedItem as string == action.Value)
                    {
                        m_pLeisure.Action = action;
                        break;
                    }
                }
            }

            DialogResult = DialogResult.OK;
        }
    }
}
