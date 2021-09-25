using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Persona.Consequences;

namespace Persona.Core.Controls
{
    public partial class TriggersView : UserControl
    {
        public TriggersView()
        {
            InitializeComponent();
            
            foreach (ColumnHeader pColumn in TriggersListView.Columns)
                m_cTriggersColumnWidths[pColumn.Index] = (float)pColumn.Width / TriggersListView.ClientSize.Width;
        }

        private Module m_pModule = null;

        public void UpdateInfo(Module pModule)
        {
            m_pModule = pModule;

            TriggersListView.Items.Clear();

            if (m_pModule == null)
                return;

            foreach (var pTrigger in m_pModule.m_cTriggers)
                AddTriggerInfo(pTrigger);
        }

        private void AddTriggerInfo(Trigger pTrigger)
        {
            ListViewItem pItem = new ListViewItem(pTrigger.m_sID);
            pItem.SubItems.Add(pTrigger.m_bRepeatable ? "+" : "-");

            string conditions = "";
            foreach (var pCondition in pTrigger.m_cConditions)
            {
                if (conditions.Length > 0)
                    conditions += " И ";
                conditions += pCondition.ToString();
            }
            pItem.SubItems.Add(conditions);

            string commands = "";
            foreach (var pCommand in pTrigger.m_cConsequences)
            {
                if (commands.Length > 0)
                    commands += ", ";
                commands += pCommand.ToString();
            }
            pItem.SubItems.Add(commands);

            pItem.Tag = pTrigger;

            TriggersListView.Items.Add(pItem);
        }

        private Dictionary<int, float> m_cTriggersColumnWidths = new Dictionary<int, float>();

        private void TriggersListView_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            if (TriggersListView.ClientSize.Width != 0)
                m_cTriggersColumnWidths[e.ColumnIndex] = (float)TriggersListView.Columns[e.ColumnIndex].Width / TriggersListView.ClientSize.Width;
        }

        private void TriggersListView_SizeChanged(object sender, EventArgs e)
        {
            foreach (ColumnHeader pColumn in TriggersListView.Columns)
                pColumn.Width = Math.Max(25, (int)(m_cTriggersColumnWidths[pColumn.Index] * TriggersListView.ClientSize.Width));
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            Trigger pTrigger = new Trigger();

            EditTrigger pForm = new EditTrigger(pTrigger, m_pModule);
            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                m_pModule.m_cTriggers.Add(pTrigger);
                AddTriggerInfo(pTrigger);
            }
        }

        private void EditTriggerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TriggersListView.SelectedItems.Count == 0)
                return;

            EditTrigger pForm = new EditTrigger(TriggersListView.SelectedItems[0].Tag as Trigger, m_pModule);
            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ListViewItem pItem = TriggersListView.SelectedItems[0];
                pItem.SubItems[0].Text = pForm.m_pTrigger.m_sID.ToString();
                pItem.SubItems[1].Text = pForm.m_pTrigger.m_bRepeatable ? "+" : "-";

                string conditions = "";
                foreach (var pCondition in pForm.m_pTrigger.m_cConditions)
                {
                    if (conditions.Length > 0)
                        conditions += " И ";
                    conditions += pCondition.ToString();
                }
                pItem.SubItems[2].Text = conditions;

                string commands = "";
                foreach (var pCommand in pForm.m_pTrigger.m_cConsequences)
                {
                    if (commands.Length > 0)
                        commands += ", ";
                    commands += pCommand.ToString();
                }
                pItem.SubItems[3].Text = commands;
            }
        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            if (TriggersListView.SelectedItems.Count == 0)
                return;

            Trigger pTrigger = new Trigger(TriggersListView.SelectedItems[0].Tag as Trigger);
            EditTrigger pForm = new EditTrigger(pTrigger, m_pModule);
            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                m_pModule.m_cTriggers.Add(pTrigger);

                AddTriggerInfo(pTrigger);
            }
        }

        private void toolStripMenuItem11_Click(object sender, EventArgs e)
        {
            List<ListViewItem> cKills = new List<ListViewItem>();
            foreach (ListViewItem pItem in TriggersListView.SelectedItems)
                cKills.Add(pItem);

            foreach (var pItem in cKills)
            {
                TriggersListView.Items.Remove(pItem);
                m_pModule.m_cTriggers.Remove(pItem.Tag as Trigger);
            }
        }

        private void Trigger2FunctionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<ListViewItem> cMutations = new List<ListViewItem>();
            foreach (ListViewItem pItem in TriggersListView.SelectedItems)
                cMutations.Add(pItem);

            foreach (var pItem in cMutations)
            {
                Trigger pTrigger = pItem.Tag as Trigger;

                if (pTrigger.m_cConditions.Count == 0 || pTrigger.m_cConsequences.Count != 1)
                    continue;

                Consequence pConsequence = pTrigger.m_cConsequences[0];

                if (!(pConsequence is ParameterSet))
                    continue;

                ParameterSet pParamSet = pConsequence as ParameterSet;

                Function pFunction = null;
                foreach (Function pPretender in m_pModule.m_cFunctions)
                    if (pPretender.m_pParam.FullName == pParamSet.m_pParam.FullName)
                    {
                        pFunction = pPretender;
                        break;
                    }

                if (pFunction == null)
                {
                    pFunction = new Function(pParamSet.m_pParam);
                    m_pModule.m_cFunctions.Add(pFunction);
                    pParamSet.m_pParam.m_pFunction = pFunction;
                }

                Function.Rule pRule = new Function.Rule(pParamSet);

                foreach (var pCondition in pTrigger.m_cConditions)
                    pRule.m_cConditions.Add(pCondition.Clone());

                pFunction.m_cRules.Add(pRule);

                TriggersListView.Items.Remove(pItem);
                m_pModule.m_cTriggers.Remove(pTrigger);
            }
        }
    }
}
