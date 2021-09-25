using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Persona.Core.Controls
{
    public partial class EventsView : UserControl
    {
        public EventsView()
        {
            InitializeComponent();

            foreach (ColumnHeader pColumn in EventsListView.Columns)
                m_cEventsColumnWidths[pColumn.Index] = (float)pColumn.Width / EventsListView.ClientSize.Width;
        }

        private Module m_pModule = null;

        public void UpdateInfo(Module pModule)
        {
            m_pModule = pModule;

            EventsListView.Items.Clear();

            if (m_pModule == null)
                return;

            foreach (var pEvent in m_pModule.m_cEvents)
                AddEventInfo(pEvent);

            if (EventsListView.Items.Count > 0)
                EventsListView.Items[0].Selected = true;
        }

        private void AddEventInfo(Event pEvent)
        {
            ListViewItem pItem = new ListViewItem(pEvent.m_sID.ToString());
            pItem.SubItems.Add(pEvent.m_pAction.m_sName);
            pItem.SubItems.Add(pEvent.m_iPriority.ToString());
            pItem.SubItems.Add(pEvent.m_iProbability.ToString());
            pItem.SubItems.Add(pEvent.m_bRepeatable ? "+" : "-");

            string conditions = "";
            foreach (var pCondition in pEvent.m_pDescription.m_cConditions)
            {
                if (conditions.Length > 0)
                    conditions += " И ";
                conditions += pCondition.ToString();
            }
            pItem.SubItems.Add(conditions);
            pItem.SubItems.Add(pEvent.m_pDescription.m_sText);

            pItem.Tag = pEvent;
            EventsListView.Items.Add(pItem);
        }

        private void AddEventToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_pModule.m_cActions.Count == 0)
                return;

            Event pEvent = new Event(m_pModule.m_cActions[0]);

            EditEvent pForm = new EditEvent(pEvent, m_pModule);
            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                m_pModule.m_cEvents.Add(pEvent);
                AddEventInfo(pEvent);
            }
        }

        private void RemoveEventToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<ListViewItem> cKills = new List<ListViewItem>();
            foreach (ListViewItem pItem in EventsListView.SelectedItems)
                cKills.Add(pItem);

            foreach (var pItem in cKills)
            {
                EventsListView.Items.Remove(pItem);
                m_pModule.m_cEvents.Remove(pItem.Tag as Event);
            }
        }

        private void EditEventToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (EventsListView.SelectedItems.Count == 0)
                return;

            EditEvent pForm = new EditEvent(EventsListView.SelectedItems[0].Tag as Event, m_pModule);
            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ListViewItem pItem = EventsListView.SelectedItems[0];
                pItem.SubItems[0].Text = pForm.m_pEvent.m_sID.ToString();
                pItem.SubItems[1].Text = pForm.m_pEvent.m_pAction.m_sName;
                pItem.SubItems[2].Text = pForm.m_pEvent.m_iPriority.ToString();
                pItem.SubItems[3].Text = pForm.m_pEvent.m_iProbability.ToString();
                pItem.SubItems[4].Text = pForm.m_pEvent.m_bRepeatable ? "+" : "-";

                string conditions = "";
                foreach (var pCondition in pForm.m_pEvent.m_pDescription.m_cConditions)
                {
                    if (conditions.Length > 0)
                        conditions += " И ";
                    conditions += pCondition.ToString();
                }
                pItem.SubItems[5].Text = conditions;
                pItem.SubItems[6].Text = pForm.m_pEvent.m_pDescription.m_sText;
            }
        }

        private void CopyEventToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (EventsListView.SelectedItems.Count == 0)
                return;

            Event pEvent = new Event(EventsListView.SelectedItems[0].Tag as Event);

            EditEvent pForm = new EditEvent(pEvent, m_pModule);
            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                m_pModule.m_cEvents.Add(pEvent);
                AddEventInfo(pEvent);
            }
        }

        private Dictionary<int, float> m_cEventsColumnWidths = new Dictionary<int, float>();

        private void EventsListView_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            m_cEventsColumnWidths[e.ColumnIndex] = (float)EventsListView.Columns[e.ColumnIndex].Width / EventsListView.ClientSize.Width;
        }

        private void EventsListView_SizeChanged(object sender, EventArgs e)
        {
            foreach (ColumnHeader pColumn in EventsListView.Columns)
                pColumn.Width = Math.Max(25, (int)(m_cEventsColumnWidths[pColumn.Index] * EventsListView.ClientSize.Width));
        }

        public Event SelectedEvent
        {
            get
            {
                if (EventsListView.SelectedItems.Count == 0)
                    return null;

                return EventsListView.SelectedItems[0].Tag as Event;
            }
        }

        public event EventHandler EventSelected;

        private void EventsListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            EventHandler handler = EventSelected;
            if (handler != null)
            {
                // Invokes the delegates.
                handler(this, e);
            }
        }
    }
}
