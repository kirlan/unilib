using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Persona.Consequences;
using Persona.Collections;

namespace Persona
{
    public partial class EditConsequenceSelect : Form
    {
        public CollectionSelect m_pConsequence;

        public EditConsequenceSelect(CollectionSelect pConsequence, List<ObjectsCollection> cCollections)
        {
            InitializeComponent();

            m_pConsequence = pConsequence;

            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(cCollections.ToArray());
            comboBox1.SelectedItem = m_pConsequence.m_pCollection;

            comboBox2.SelectedItem = m_pConsequence.m_pCollection.m_cObjects[m_pConsequence.m_iSelectID];
        }

        private void button2_Click(object sender, EventArgs e)
        {
            m_pConsequence.m_pCollection = comboBox1.SelectedItem as ObjectsCollection;
            m_pConsequence.m_iSelectID = (comboBox2.SelectedItem as CollectedObject).m_iID;

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
