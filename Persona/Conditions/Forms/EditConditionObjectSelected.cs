using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Persona.Conditions;
using Persona.Collections;

namespace Persona
{
    public partial class EditConditionObjectSelected : Form
    {
        public ConditionObjectSelected m_pCondition;

        public EditConditionObjectSelected(ConditionObjectSelected pCondition, List<ObjectsCollection> cCollections)
        {
            InitializeComponent();

            m_pCondition = pCondition;

            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(cCollections.ToArray());
            comboBox1.SelectedItem = m_pCondition.m_pCollection;

            comboBox2.SelectedItem = m_pCondition.m_pCollection.m_cObjects[m_pCondition.m_iSelectID];

            if (m_pCondition.m_bNot)
                radioButton6.Checked = true;
            else
                radioButton7.Checked = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            m_pCondition.m_pCollection = comboBox1.SelectedItem as ObjectsCollection;
            m_pCondition.m_iSelectID = (comboBox2.SelectedItem as CollectedObject).m_iID;

            m_pCondition.m_bNot = radioButton6.Checked;

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1)
                return;

            ObjectsCollection pCollection = comboBox1.SelectedItem as ObjectsCollection;

            comboBox2.Items.Clear();
            comboBox2.Items.AddRange(pCollection.m_cObjects.Values.ToArray());
            if (comboBox2.Items.Count > 0)
                comboBox2.SelectedIndex = 0;
        }
    }
}
