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
    public partial class EditFunction : Form
    {
        public Function m_pFunction;

        public EditFunction(Function pFunction, List<NumericParameter> cNumeric, List<BoolParameter> cBool, List<StringParameter> cString)
        {
            InitializeComponent();

            m_pFunction = pFunction;

            listBox1.Items.Clear();
            listBox1.Items.AddRange(cNumeric.ToArray());

            listBox2.Items.Clear();
            listBox2.Items.AddRange(cBool.ToArray());

            listBox3.Items.Clear();
            listBox3.Items.AddRange(cString.ToArray());

            if (m_pFunction.m_pParam is NumericParameter)
            {
                tabControl1.SelectedIndex = 0;
                listBox1.SelectedItem = m_pFunction.m_pParam;
            }
            if (m_pFunction.m_pParam is BoolParameter)
            {
                tabControl1.SelectedIndex = 1;
                listBox2.SelectedItem = m_pFunction.m_pParam;
            }
            if (m_pFunction.m_pParam is StringParameter)
            {
                tabControl1.SelectedIndex = 2;
                listBox3.SelectedItem = m_pFunction.m_pParam;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    m_pFunction.m_pParam = listBox1.SelectedItem as NumericParameter;
                    break;
                case 1:
                    m_pFunction.m_pParam = listBox2.SelectedItem as BoolParameter;
                    break;
                case 2:
                    m_pFunction.m_pParam = listBox3.SelectedItem as BoolParameter;
                    break;
            }

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
