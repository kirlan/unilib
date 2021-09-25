using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Persona.Collections;

namespace Persona.Core.Controls
{
    public partial class CollectedItemsView : UserControl
    {
        public CollectedItemsView()
        {
            InitializeComponent();

            foreach (ColumnHeader pColumn in CollectedItemsListView.Columns)
                m_cCollectedItemsColumnWidths[pColumn.Index] = (float)pColumn.Width / CollectedItemsListView.ClientSize.Width;
        }

        private Module m_pModule = null;
        private ObjectsCollection m_pCollection = null;

        public void UpdateInfo(Module pModule, ObjectsCollection pCollection)
        {
            m_pModule = pModule;
            m_pCollection = pCollection;

            CollectedItemsListView.Items.Clear();

            if (m_pCollection == null)
                return;

            foreach (var pItem in m_pCollection.m_cObjects)
            {
                AddCollectedItemInfo(pItem.Value);
            }
        }

        private Dictionary<int, float> m_cCollectedItemsColumnWidths = new Dictionary<int, float>();

        private void CollectedItemsListView_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            m_cCollectedItemsColumnWidths[e.ColumnIndex] = (float)CollectedItemsListView.Columns[e.ColumnIndex].Width / CollectedItemsListView.ClientSize.Width;
        }

        private void CollectedItemsListView_SizeChanged(object sender, EventArgs e)
        {
            foreach (ColumnHeader pColumn in CollectedItemsListView.Columns)
                pColumn.Width = Math.Max(25, (int)(m_cCollectedItemsColumnWidths[pColumn.Index] * CollectedItemsListView.ClientSize.Width));
        }

        private void AddCollectedItemInfo(CollectedObject pObject)
        {
            ListViewItem pNewLine = new ListViewItem(pObject.m_iID.ToString());
            pNewLine.SubItems.Add(pObject.m_sName);
            pNewLine.SubItems.Add(pObject.m_iProbability.ToString());

            string conditions = "";
            foreach (var pCondition in pObject.m_cConditions)
            {
                if (conditions.Length > 0)
                    conditions += " И ";
                conditions += pCondition.ToString();
            }
            pNewLine.SubItems.Add(conditions);

            string sValues = "";
            foreach (var pValue in pObject.m_cValues)
            {
                if (sValues.Length > 0)
                    sValues += ", ";

                if (pValue.m_pParam != null)
                {
                    sValues += pValue.m_pParam.m_sName;
                    sValues += " = ";
                    sValues += pValue.DisplayValue;
                }
                else
                    sValues += pValue.ToString();
            }

            pNewLine.SubItems.Add(sValues);

            pNewLine.Tag = pObject;
            CollectedItemsListView.Items.Add(pNewLine);
        }

        private void UpdateCollectedItemInfo(ListViewItem pSelected)
        {
            CollectedObject pObject = pSelected.Tag as CollectedObject;

            pSelected.SubItems[1].Text = pObject.m_sName;
            pSelected.SubItems[2].Text = pObject.m_iProbability.ToString();

            string conditions = "";
            foreach (var pCondition in pObject.m_cConditions)
            {
                if (conditions.Length > 0)
                    conditions += " И ";
                conditions += pCondition.ToString();
            }
            pSelected.SubItems[3].Text = conditions;

            string sValues = "";
            foreach (var pValue in pObject.m_cValues)
            {
                if (sValues.Length > 0)
                    sValues += ", ";

                sValues += pValue.m_pParam.m_sName;
                sValues += " = ";
                sValues += pValue.DisplayValue;
            }
            pSelected.SubItems[4].Text = sValues;
        }

        private void AddCollectedObject_Click(object sender, EventArgs e)
        {
            if (m_pCollection == null)
                return;

            CollectedObject pObject = new CollectedObject(m_pCollection.GetNewID(), m_pCollection);

            EditCollectedObject pForm = new EditCollectedObject(pObject, m_pModule);
            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                m_pCollection.m_cObjects[pObject.m_iID] = pObject;
                AddCollectedItemInfo(pObject);
            }

        }

        private void EditCollectedObject_Click(object sender, EventArgs e)
        {
            if (CollectedItemsListView.SelectedItems.Count == 0)
                return;

            CollectedObject pObject = CollectedItemsListView.SelectedItems[0].Tag as CollectedObject;

            EditCollectedObject pForm = new EditCollectedObject(pObject, m_pModule);
            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                UpdateCollectedItemInfo(CollectedItemsListView.SelectedItems[0]);
            }
        }

        private void DeleteCollectedObject_Click(object sender, EventArgs e)
        {
            if (m_pCollection == null)
                return;

            if (CollectedItemsListView.SelectedItems.Count == 0)
                return;

            CollectedObject pObject = CollectedItemsListView.SelectedItems[0].Tag as CollectedObject;
            m_pCollection.m_cObjects.Remove(pObject.m_iID);

            CollectedItemsListView.Items.Remove(CollectedItemsListView.SelectedItems[0]);
        }

        private void CloneCollectedObject_Click(object sender, EventArgs e)
        {
            if (m_pCollection == null)
                return;

            if (CollectedItemsListView.SelectedItems.Count == 0)
                return;

            CollectedObject pObject = CollectedItemsListView.SelectedItems[0].Tag as CollectedObject;

            CollectedObject pNewObject = new CollectedObject(m_pCollection.GetNewID(), pObject);

            EditCollectedObject pForm = new EditCollectedObject(pNewObject, m_pModule);
            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                m_pCollection.m_cObjects[pNewObject.m_iID] = pNewObject;
                AddCollectedItemInfo(pNewObject);
            }
        }
    }
}
