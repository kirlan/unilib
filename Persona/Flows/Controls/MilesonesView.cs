using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Persona.Flows;

namespace Persona.Core.Controls
{
    public partial class MilesonesView : UserControl
    {
        public MilesonesView()
        {
            InitializeComponent();

            foreach (ColumnHeader pColumn in MilestonesListView.Columns)
                m_cMilestonesColumnWidths[pColumn.Index] = (float)pColumn.Width / MilestonesListView.ClientSize.Width;
        }

        private Dictionary<int, float> m_cMilestonesColumnWidths = new Dictionary<int, float>();

        private void MilestonesListView_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            m_cMilestonesColumnWidths[e.ColumnIndex] = (float)MilestonesListView.Columns[e.ColumnIndex].Width / MilestonesListView.ClientSize.Width;
        }

        private void MilestonesListView_SizeChanged(object sender, EventArgs e)
        {
            foreach (ColumnHeader pColumn in MilestonesListView.Columns)
                pColumn.Width = Math.Max(25, (int)(m_cMilestonesColumnWidths[pColumn.Index] * MilestonesListView.ClientSize.Width));
        }

        private Module m_pModule = null;
        private Flow m_pFlow = null;

        public void UpdateInfo(Module pModule, Flow pFlow)
        {
            m_pModule = pModule;
            m_pFlow = pFlow;

            MilestonesListView.Items.Clear();

            if (m_pModule == null || m_pFlow == null)
                return;

            foreach (var pItem in m_pFlow.m_cMilestones)
            {
                AddMilestoneInfo(pItem);
            }
        }

        private void AddMilestoneInfo(Milestone pMilestone)
        {
            ListViewItem pNewLine = new ListViewItem(pMilestone.m_fPosition.ToString("00000.00"));
            pNewLine.SubItems.Add(pMilestone.m_sName);
            pNewLine.SubItems.Add(pMilestone.m_bRepeatable ? "+" : "-");

            string conditions = "";
            foreach (var pCondition in pMilestone.m_cConditions)
            {
                if (conditions.Length > 0)
                    conditions += " И ";
                conditions += pCondition.ToString();
            }
            pNewLine.SubItems.Add(conditions);

            string commands = "";
            foreach (var pCommand in pMilestone.m_cConsequences)
            {
                if (commands.Length > 0)
                    commands += ", ";
                commands += pCommand.ToString();
            }
            pNewLine.SubItems.Add(commands);

            pNewLine.Tag = pMilestone;
            MilestonesListView.Items.Add(pNewLine);
        }

        private void UpdateMilestoneInfo(ListViewItem pSelected)
        {
            Milestone pMilestone = pSelected.Tag as Milestone;

            pSelected.Text = pMilestone.m_fPosition.ToString("00000.00");
            pSelected.SubItems[1].Text = pMilestone.m_sName;
            pSelected.SubItems[2].Text = pMilestone.m_bRepeatable ? "+" : "-";

            string conditions = "";
            foreach (var pCondition in pMilestone.m_cConditions)
            {
                if (conditions.Length > 0)
                    conditions += " И ";
                conditions += pCondition.ToString();
            }
            pSelected.SubItems[3].Text = conditions;

            string commands = "";
            foreach (var pCommand in pMilestone.m_cConsequences)
            {
                if (commands.Length > 0)
                    commands += ", ";
                commands += pCommand.ToString();
            }
            pSelected.SubItems[4].Text = commands;
        }

        private void AddMilestone_Click(object sender, EventArgs e)
        {
            if (m_pFlow == null)
                return;

            Milestone pStone = new Milestone();

            EditMilestone pForm = new EditMilestone(pStone, m_pModule);
            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                m_pFlow.m_cMilestones.Add(pStone);
                AddMilestoneInfo(pStone);
            }

        }

        private void EditMilestone_Click(object sender, EventArgs e)
        {
            if (MilestonesListView.SelectedItems.Count == 0)
                return;

            Milestone pStone = MilestonesListView.SelectedItems[0].Tag as Milestone;

            EditMilestone pForm = new EditMilestone(pStone, m_pModule);
            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                UpdateMilestoneInfo(MilestonesListView.SelectedItems[0]);
            }
        }

        private void DeleteMilestone_Click(object sender, EventArgs e)
        {
            if (m_pFlow == null)
                return;

            if (MilestonesListView.SelectedItems.Count == 0)
                return;

            Milestone pStone = MilestonesListView.SelectedItems[0].Tag as Milestone;
            m_pFlow.m_cMilestones.Remove(pStone);

            MilestonesListView.Items.Remove(MilestonesListView.SelectedItems[0]);
        }

        private void CloneMilestone_Click(object sender, EventArgs e)
        {
            if (m_pFlow == null)
                return;

            if (MilestonesListView.SelectedItems.Count == 0)
                return;

            Milestone pStone = MilestonesListView.SelectedItems[0].Tag as Milestone;

            Milestone pNewStone = new Milestone(pStone);

            EditMilestone pForm = new EditMilestone(pNewStone, m_pModule);
            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                m_pFlow.m_cMilestones.Add(pNewStone);
                AddMilestoneInfo(pNewStone);
            }
        }
    }
}
