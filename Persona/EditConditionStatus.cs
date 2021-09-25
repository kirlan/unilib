using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Persona.Conditions;
using Persona.Parameters;

namespace Persona
{
    public partial class EditConditionStatus : Form
    {
        ConditionStatus m_pCondition;

        public EditConditionStatus(ConditionStatus pCondition, List<BoolParameter> cParameters)
        {
            InitializeComponent();

            m_pCondition = pCondition;

            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(cParameters.ToArray());
            
            comboBox1_SelectedIndexChanged(this, new EventArgs());

            if (m_pCondition.m_bNot)
                radioButton6.Checked = true;
            else
                radioButton7.Checked = true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button2.Enabled = (comboBox1.SelectedItem != null);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            m_pCondition.m_pParam1 = comboBox1.SelectedItem as Parameter;

            m_pCondition.m_bNot = radioButton6.Checked;

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
