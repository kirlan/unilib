using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Persona.Events.Controls
{
    public partial class ActionsList : UserControl
    {
        public ActionsList()
        {
            InitializeComponent();
        }

        private Module m_pModule = null;

        public void UpdateInfo(Module pModule)
        {
            m_pModule = pModule;

            ActionsListBox.Items.Clear();
            ActionsListBox.Items.AddRange(m_pModule.m_cActions.ToArray());

        }

        //public ObjectsCollection SelectedCollection
        //{
        //    get
        //    {
        //        if (CollectionsListBox.SelectedItems.Count == 0)
        //            return null;

        //        return CollectionsListBox.SelectedItem as ObjectsCollection;
        //    }
        //}

        //public event EventHandler CollectionSelected;

        //private void CollectionsListBox_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    EventHandler handler = CollectionSelected;
        //    if (handler != null)
        //    {
        //        // Invokes the delegates.
        //        handler(this, e);
        //    }
        //}

        private void AddCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (EditAction pForm = new EditAction(new Action("Новое действие")))
            {
                if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    m_pModule.m_cActions.Add(pForm.m_pAction);
                    ActionsListBox.Items.Add(pForm.m_pAction);
                }
            }
        }

        private void EditCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActionsListBox.SelectedItem == null)
                return;

            using (EditAction pForm = new EditAction(ActionsListBox.SelectedItem as Action))
            {
                if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    LISTBOX.Refresh(ActionsListBox);
                }
            }
        }

        private void RemoveCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActionsListBox.SelectedItem == null)
                return;

            m_pModule.m_cActions.Remove(ActionsListBox.SelectedItem as Action);
            ActionsListBox.Items.Remove(ActionsListBox.SelectedItem);
        }

        private int m_iFocusedAction = -1;

        private void ActionsListBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (m_iFocusedAction != -1 && m_iFocusedAction < ActionsListBox.Items.Count)
                if (ActionsListBox.GetItemRectangle(m_iFocusedAction).Contains(e.Location))
                    return;

            m_iFocusedAction = -1;

            for (int i = 0; i < ActionsListBox.Items.Count; i++)
            {
                if (ActionsListBox.GetItemRectangle(i).Contains(e.Location))
                {
                    m_iFocusedAction = i;
                    break;
                }
            }

            ActionsListBox.SelectedIndex = m_iFocusedAction;
        }
    }
}
