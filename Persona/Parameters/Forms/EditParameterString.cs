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
    public partial class EditParameterString : Form
    {
        private StringParameter m_pParam;

        public EditParameterString(StringParameter pParam)
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

            textBox3.Text = m_pParam.m_sDefaultValue;
        }

        public EditParameterString(StringParameter pParam, string sCollection)
        {
            InitializeComponent();

            m_pParam = pParam;

            textBox1.Text = m_pParam.m_sName;

            comboBox1.Items.Clear();
            comboBox1.Items.Add(sCollection);
            comboBox1.Text = sCollection;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;

            radioButton7.Checked = true;
            radioButton6.Enabled = false;
            radioButton7.Enabled = false;

            textBox2.Text = m_pParam.m_sComment;

            textBox3.Text = m_pParam.m_sDefaultValue;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            m_pParam.m_sName = textBox1.Text;
            m_pParam.m_sGroup = comboBox1.Text;
            m_pParam.m_bHidden = radioButton7.Checked;
            m_pParam.m_sComment = textBox2.Text;
            m_pParam.m_sDefaultValue = textBox3.Text;

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
