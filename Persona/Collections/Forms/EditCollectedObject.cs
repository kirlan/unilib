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
using Persona.Consequences;
using Persona.Collections;

namespace Persona
{
    public partial class EditCollectedObject : Form
    {
        public CollectedObject m_pObject;

        public EditCollectedObject(CollectedObject pObject, Module pModule)
        {
            InitializeComponent();

            m_pObject = pObject;

            textBox1.Text = m_pObject.m_iID.ToString();

            textBox2.Text = m_pObject.m_sName;

            conditionsList1.Bind(pModule);
            conditionsList1.Conditions = m_pObject.m_cConditions;

            listView1.Items.Clear();
            foreach (var pValue in m_pObject.m_cValues)
            {
                ListViewItem pItem = new ListViewItem(pValue.m_pParam.m_sName);
                pItem.SubItems.Add(pValue.DisplayValue);
                
                pItem.Tag = pValue;
                listView1.Items.Add(pItem);
            }

            numericUpDown2.Value = m_pObject.m_iProbability;

            if (m_pObject.m_bUnique)
                radioButton2.Checked = true;
            else
                radioButton1.Checked = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            m_pObject.m_cConditions = conditionsList1.Conditions;

            m_pObject.m_cValues.Clear();
            foreach (ListViewItem pItem in listView1.Items)
                m_pObject.m_cValues.Add(pItem.Tag as ParameterSet);

            m_pObject.m_iProbability = (int)numericUpDown2.Value;

            m_pObject.m_bUnique = radioButton2.Checked;

            m_pObject.m_sName = textBox2.Text;

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void listView1_ItemActivate(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            ParameterSet pValue = listView1.SelectedItems[0].Tag as ParameterSet;
    
            EditConsequenceSet pForm = new EditConsequenceSet(pValue);
            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                listView1.SelectedItems[0].SubItems[1].Text = pValue.DisplayValue;
        }
    }
}
