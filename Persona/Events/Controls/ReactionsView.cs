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
    public partial class ReactionsView : UserControl
    {
        public ReactionsView()
        {
            InitializeComponent();

            foreach (ColumnHeader pColumn in ReactionsListView.Columns)
                m_cReactionsColumnWidths[pColumn.Index] = (float)pColumn.Width / ReactionsListView.ClientSize.Width;
        }

        private Module m_pModule = null;
        private Event m_pSelectedEvent = null;

        public void UpdateInfo(Module pModule, Event pEvent)
        {
            m_pModule = pModule;
            m_pSelectedEvent = pEvent;

            ReactionsListView.Items.Clear();

            if (m_pModule == null || m_pSelectedEvent == null)
                return;

            foreach (Reaction pReaction in m_pSelectedEvent.m_cReactions)
                AddReactionInfo(pReaction);
        }

        private void AddReactionInfo(Reaction pReaction)
        {
            ListViewItem pItem = new ListViewItem(pReaction.m_sName);
            pItem.SubItems.Add(pReaction.m_bAlwaysVisible ? "!" : "?");

            string conditions = "";
            foreach (var pCondition in pReaction.m_cConditions)
            {
                if (conditions.Length > 0)
                    conditions += " И ";
                conditions += pCondition.ToString();
            }
            pItem.SubItems.Add(conditions);

            string commands = "";
            foreach (var pCommand in pReaction.m_cConsequences)
            {
                if (commands.Length > 0)
                    commands += ", ";
                commands += pCommand.ToString();
            }
            pItem.SubItems.Add(commands);

            pItem.Tag = pReaction;

            ReactionsListView.Items.Add(pItem);
        }

        private void AddReactionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_pSelectedEvent == null)
                return;

            Reaction pReaction = new Reaction();
            EditReaction pForm = new EditReaction(pReaction, m_pModule);
            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                m_pSelectedEvent.m_cReactions.Add(pReaction);

                AddReactionInfo(pReaction);
            }
        }

        private void EditReactionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_pSelectedEvent == null)
                return;

            if (ReactionsListView.SelectedItems.Count == 0)
                return;

            EditReaction pForm = new EditReaction(ReactionsListView.SelectedItems[0].Tag as Reaction, m_pModule);
            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ListViewItem pItem = ReactionsListView.SelectedItems[0];
                pItem.SubItems[0].Text = pForm.m_pReaction.m_sName.ToString();
                pItem.SubItems[1].Text = pForm.m_pReaction.m_bAlwaysVisible ? "!" : "?";

                string conditions = "";
                foreach (var pCondition in pForm.m_pReaction.m_cConditions)
                {
                    if (conditions.Length > 0)
                        conditions += " И ";
                    conditions += pCondition.ToString();
                }
                pItem.SubItems[2].Text = conditions;

                string commands = "";
                foreach (var pCommand in pForm.m_pReaction.m_cConsequences)
                {
                    if (commands.Length > 0)
                        commands += ", ";
                    commands += pCommand.ToString();
                }
                pItem.SubItems[3].Text = commands;
            }
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            if (m_pSelectedEvent == null)
                return;

            List<ListViewItem> cKills = new List<ListViewItem>();
            foreach (ListViewItem pItem in ReactionsListView.SelectedItems)
                cKills.Add(pItem);

            foreach (var pItem in cKills)
            {
                ReactionsListView.Items.Remove(pItem);
                m_pSelectedEvent.m_cReactions.Remove(pItem.Tag as Reaction);
            }
        }

        private void CopyReactionToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (m_pSelectedEvent == null)
                return;

            if (ReactionsListView.SelectedItems.Count == 0)
                return;

            Reaction pReaction = new Reaction(ReactionsListView.SelectedItems[0].Tag as Reaction);
            EditReaction pForm = new EditReaction(pReaction, m_pModule);
            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                m_pSelectedEvent.m_cReactions.Add(pReaction);

                AddReactionInfo(pReaction);
            }
        }

        private Dictionary<int, float> m_cReactionsColumnWidths = new Dictionary<int, float>();

        private void ReactionsListView_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            m_cReactionsColumnWidths[e.ColumnIndex] = (float)ReactionsListView.Columns[e.ColumnIndex].Width / ReactionsListView.ClientSize.Width;
        }

        private void ReactionsListView_SizeChanged(object sender, EventArgs e)
        {
            foreach (ColumnHeader pColumn in ReactionsListView.Columns)
                pColumn.Width = Math.Max(25, (int)(m_cReactionsColumnWidths[pColumn.Index] * ReactionsListView.ClientSize.Width));
        }
    }
}
