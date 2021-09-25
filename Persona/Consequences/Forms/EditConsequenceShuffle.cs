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
    public partial class EditConsequenceShuffle : Form
    {
        public CollectionShuffle m_pConsequence;

        public EditConsequenceShuffle(CollectionShuffle pConsequence, List<ObjectsCollection> cCollections)
        {
            InitializeComponent();

            m_pConsequence = pConsequence;

            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(cCollections.ToArray());
            comboBox1.SelectedItem = m_pConsequence.m_pCollection;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            m_pConsequence.m_pCollection = comboBox1.SelectedItem as ObjectsCollection;

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
