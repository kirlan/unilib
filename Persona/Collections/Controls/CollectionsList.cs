using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Persona.Collections.Controls
{
    public partial class CollectionsList : UserControl
    {
        public CollectionsList()
        {
            InitializeComponent();
        }

        private Module m_pModule = null;

        public void UpdateInfo(Module pModule)
        {
            m_pModule = pModule;

            CollectionsListBox.Items.Clear();
            CollectionsListBox.Items.AddRange(m_pModule.m_cCollections.ToArray());
            if (CollectionsListBox.Items.Count > 0)
                CollectionsListBox.SelectedIndex = 0;
        }

        public ObjectsCollection SelectedCollection
        {
            get
            {
                if (CollectionsListBox.SelectedItems.Count == 0)
                    return null;

                return CollectionsListBox.SelectedItem as ObjectsCollection;
            }
        }

        public event EventHandler CollectionSelected;

        private void CollectionsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            EventHandler handler = CollectionSelected;
            if (handler != null)
            {
                // Invokes the delegates.
                handler(this, e);
            }
        }

        private void CollectionAdd_Click(object sender, EventArgs e)
        {
            ObjectsCollection pCollection = new ObjectsCollection();

            EditCollection pForm = new EditCollection(pCollection);
            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                m_pModule.m_cCollections.Add(pCollection);
                CollectionsListBox.SelectedIndex = CollectionsListBox.Items.Add(pCollection);
            }
        }

        private void CollectionEdit_Click(object sender, EventArgs e)
        {
            ObjectsCollection pCollection = CollectionsListBox.SelectedItem as ObjectsCollection;

            EditCollection pForm = new EditCollection(pCollection);
            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                LISTBOX.Refresh(CollectionsListBox);

                foreach (var pItem in pCollection.m_cObjects)
                    pItem.Value.Sinchronize(pCollection.m_cNumericParameters, pCollection.m_cBoolParameters, pCollection.m_cStringParameters);

                CollectionsListBox_SelectedIndexChanged(sender, e);
            }
        }
    }
}
