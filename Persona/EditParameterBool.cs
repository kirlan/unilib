using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Persona.Parameters;

namespace Persona
{
    public partial class EditParameterBool : Form
    {
        private BoolParameter m_pParam;

        public EditParameterBool(BoolParameter pParam)
        {
            InitializeComponent();

            m_pParam = pParam;

            textBox1.Text = m_pParam.m_sName;

            comboBox1.Items.Clear();
            comboBox1.Text = m_pParam.m_sGroup;

            if (m_pParam.m_bHidden)
                radioButton7.Checked = true;
            else
                radioButton6.Checked = true;

            textBox2.Text = m_pParam.m_sComment;

            if (m_pParam.m_bDefaultValue)
                radioButton1.Checked = true;
            else
                radioButton2.Checked = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            m_pParam.m_sName = textBox1.Text;
            m_pParam.m_sGroup = comboBox1.Text;
            m_pParam.m_bHidden = radioButton7.Checked;
            m_pParam.m_sComment = textBox2.Text;
            m_pParam.m_bDefaultValue = radioButton1.Checked;

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
