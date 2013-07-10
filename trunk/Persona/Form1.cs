using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Persona.Parameters;

namespace Persona
{
    public partial class Form1 : Form
    {
        private Module m_pModule = new Module();

        public Form1()
        {
            InitializeComponent();

            foreach (ColumnHeader pColumn in EventsListView.Columns)
                m_cEventsColumnWidths[pColumn.Index] = (float)pColumn.Width / EventsListView.ClientSize.Width;

            foreach (ColumnHeader pColumn in ReactionsListView.Columns)
                m_cReactionsColumnWidths[pColumn.Index] = (float)pColumn.Width / ReactionsListView.ClientSize.Width;

            foreach (ColumnHeader pColumn in TriggersListView.Columns)
                m_cTriggersColumnWidths[pColumn.Index] = (float)pColumn.Width / TriggersListView.ClientSize.Width;

            foreach (ColumnHeader pColumn in ParametersListView.Columns)
                m_cParametersColumnWidths[pColumn.Index] = (float)pColumn.Width / ParametersListView.ClientSize.Width;

            UpdateModuleInfo();
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

        private void UpdateModuleInfo()
        {
            ModuleNameBox.Text = m_pModule.m_sName;
            ModuleDescBox.Text = m_pModule.m_sDescription;

            ActionsListBox.Items.Clear();
            ActionsListBox.Items.AddRange(m_pModule.m_cActions.ToArray());

            EventsListView.Items.Clear();
            foreach (var pEvent in m_pModule.m_cEvents)
            {
                AddEventInfo(pEvent);
            }

            if (EventsListView.Items.Count > 0)
                EventsListView.Items[0].Selected = true;

            TriggersListView.Items.Clear();
            foreach (var pTrigger in m_pModule.m_cTriggers)
            {
                AddTriggerInfo(pTrigger);
            }

            ParametersTypesListBox.SelectedIndex = 0;
            ParametersTypesListBox_SelectedIndexChanged(this, new EventArgs());
        }

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
                    ActionsListBox.Refresh();
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

        private void EventsListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (EventsListView.SelectedItems.Count == 0)
                return;

            Event pEvent = EventsListView.SelectedItems[0].Tag as Event;

            ReactionsListView.Items.Clear();
            foreach (Reaction pReaction in pEvent.m_cReactions)
                AddReactionInfo(pReaction);
        }

        private void AddEventToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_pModule.m_cActions.Count == 0)
                return;

            Event pEvent = new Event(m_pModule.m_cActions[0]);

            EditEvent pForm = new EditEvent(pEvent, m_pModule.m_cActions, m_pModule.m_cNumericParameters, m_pModule.m_cBoolParameters, m_pModule.m_cStringParameters);
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

            EditEvent pForm = new EditEvent(EventsListView.SelectedItems[0].Tag as Event, m_pModule.m_cActions, m_pModule.m_cNumericParameters, m_pModule.m_cBoolParameters, m_pModule.m_cStringParameters);
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

        private void AddParameterInfo(Parameter pParam)
        {
            ListViewItem pItem = new ListViewItem(pParam.m_sName);
            pItem.SubItems.Add(pParam.m_sGroup);
            pItem.SubItems.Add(pParam.m_bHidden ? "#" : "-");
            pItem.SubItems.Add(pParam.m_sComment);

            pItem.Tag = pParam;
            ParametersListView.Items.Add(pItem);
        }

        private void ParametersTypesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ParametersListView.Items.Clear();

            switch (ParametersTypesListBox.SelectedIndex)
            {
                case 0:
                    foreach (var pParam in m_pModule.m_cNumericParameters)
                        AddParameterInfo(pParam);
                    break;
                case 1:
                    foreach (var pParam in m_pModule.m_cBoolParameters)
                        AddParameterInfo(pParam);
                    break;
                case 2:
                    foreach (var pParam in m_pModule.m_cStringParameters)
                        AddParameterInfo(pParam);
                    break;
            }
        }

        private void AddParameterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switch (ParametersTypesListBox.SelectedIndex)
            {
                case 0:
                    {
                        NumericParameter pParam = new NumericParameter();
                        EditParameterNumeric pForm = new EditParameterNumeric(pParam);
                        if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            m_pModule.m_cNumericParameters.Add(pParam);
                            AddParameterInfo(pParam);
                        }
                    }
                    break;
                case 1:
                    {
                        BoolParameter pParam = new BoolParameter();
                        EditParameterBool pForm = new EditParameterBool(pParam);
                        if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            m_pModule.m_cBoolParameters.Add(pParam);
                            AddParameterInfo(pParam);
                        }
                    }
                    break;
                case 2:
                    break;
            }
        }

        private void EditParameterToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (ParametersListView.SelectedItems.Count == 0)
                return;

            switch (ParametersTypesListBox.SelectedIndex)
            {
                case 0:
                    {
                        NumericParameter pParam = ParametersListView.SelectedItems[0].Tag as NumericParameter;
                        EditParameterNumeric pForm = new EditParameterNumeric(pParam);
                        if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            ParametersListView.SelectedItems[0].Text = pParam.m_sName;
                            ParametersListView.SelectedItems[0].SubItems[1].Text = pParam.m_sGroup;
                            ParametersListView.SelectedItems[0].SubItems[2].Text = pParam.m_bHidden ? "#" : "-";
                            ParametersListView.SelectedItems[0].SubItems[3].Text = pParam.m_sComment;
                        }
                    }
                    break;
                case 1:
                    {
                        BoolParameter pParam = ParametersListView.SelectedItems[0].Tag as BoolParameter;
                        EditParameterBool pForm = new EditParameterBool(pParam);
                        if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            ParametersListView.SelectedItems[0].Text = pParam.m_sName;
                            ParametersListView.SelectedItems[0].SubItems[1].Text = pParam.m_sGroup;
                            ParametersListView.SelectedItems[0].SubItems[2].Text = pParam.m_bHidden ? "#" : "-";
                            ParametersListView.SelectedItems[0].SubItems[3].Text = pParam.m_sComment;
                        }
                    }
                    break;
                case 2:
                    break;
            }
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                m_pModule.SaveXML(saveFileDialog1.FileName);
            }
        }

        private void загрузитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                m_pModule.LoadXML(openFileDialog1.FileName);
                UpdateModuleInfo();
            }
        }

        private void AddReactionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (EventsListView.SelectedItems.Count == 0)
                return;

            Reaction pReaction = new Reaction();
            EditReaction pForm = new EditReaction(pReaction, m_pModule.m_cNumericParameters, m_pModule.m_cBoolParameters, m_pModule.m_cStringParameters);
            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Event pEvent = EventsListView.SelectedItems[0].Tag as Event;
                pEvent.m_cReactions.Add(pReaction);

                AddReactionInfo(pReaction);
            }
        }

        private void EditReactionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (EventsListView.SelectedItems.Count == 0)
                return;

            if (ReactionsListView.SelectedItems.Count == 0)
                return;

            EditReaction pForm = new EditReaction(ReactionsListView.SelectedItems[0].Tag as Reaction, m_pModule.m_cNumericParameters, m_pModule.m_cBoolParameters, m_pModule.m_cStringParameters);
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
            if (EventsListView.SelectedItems.Count == 0)
                return;

            Event pEvent = EventsListView.SelectedItems[0].Tag as Event;

            List<ListViewItem> cKills = new List<ListViewItem>();
            foreach (ListViewItem pItem in ReactionsListView.SelectedItems)
                cKills.Add(pItem);

            foreach (var pItem in cKills)
            {
                ReactionsListView.Items.Remove(pItem);
                pEvent.m_cReactions.Remove(pItem.Tag as Reaction);
            }
        }

        private void CopyEventToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (EventsListView.SelectedItems.Count == 0)
                return;

            Event pEvent = new Event(EventsListView.SelectedItems[0].Tag as Event);

            EditEvent pForm = new EditEvent(pEvent, m_pModule.m_cActions, m_pModule.m_cNumericParameters, m_pModule.m_cBoolParameters, m_pModule.m_cStringParameters);
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
                pColumn.Width = (int)(m_cEventsColumnWidths[pColumn.Index] * EventsListView.ClientSize.Width);
        }

        private Dictionary<int, float> m_cReactionsColumnWidths = new Dictionary<int, float>();

        private void ReactionsListView_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            m_cReactionsColumnWidths[e.ColumnIndex] = (float)ReactionsListView.Columns[e.ColumnIndex].Width / ReactionsListView.ClientSize.Width;
        }

        private void ReactionsListView_SizeChanged(object sender, EventArgs e)
        {
            foreach (ColumnHeader pColumn in ReactionsListView.Columns)
                pColumn.Width = (int)(m_cReactionsColumnWidths[pColumn.Index] * ReactionsListView.ClientSize.Width);
        }

        private Dictionary<int, float> m_cParametersColumnWidths = new Dictionary<int, float>();

        private void ParametersListView_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            m_cParametersColumnWidths[e.ColumnIndex] = (float)ParametersListView.Columns[e.ColumnIndex].Width / ParametersListView.ClientSize.Width;
        }

        private void ParametersListView_SizeChanged(object sender, EventArgs e)
        {
            foreach (ColumnHeader pColumn in ParametersListView.Columns)
                pColumn.Width = (int)(m_cParametersColumnWidths[pColumn.Index] * ParametersListView.ClientSize.Width);
        }

        private void CopyReactionToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (EventsListView.SelectedItems.Count == 0)
                return;

            if (ReactionsListView.SelectedItems.Count == 0)
                return;

            Reaction pReaction = new Reaction(ReactionsListView.SelectedItems[0].Tag as Reaction);
            EditReaction pForm = new EditReaction(pReaction, m_pModule.m_cNumericParameters, m_pModule.m_cBoolParameters, m_pModule.m_cStringParameters);
            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Event pEvent = EventsListView.SelectedItems[0].Tag as Event;
                pEvent.m_cReactions.Add(pReaction);

                AddReactionInfo(pReaction);
            }
        }

        private Dictionary<int, float> m_cTriggersColumnWidths = new Dictionary<int, float>();

        private void TriggersListView_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            m_cTriggersColumnWidths[e.ColumnIndex] = (float)TriggersListView.Columns[e.ColumnIndex].Width / TriggersListView.ClientSize.Width;
        }

        private void TriggersListView_SizeChanged(object sender, EventArgs e)
        {
            foreach (ColumnHeader pColumn in TriggersListView.Columns)
                pColumn.Width = (int)(m_cTriggersColumnWidths[pColumn.Index] * TriggersListView.ClientSize.Width);
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            Trigger pTrigger = new Trigger();

            EditTrigger pForm = new EditTrigger(pTrigger, m_pModule.m_cNumericParameters, m_pModule.m_cBoolParameters, m_pModule.m_cStringParameters);
            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                m_pModule.m_cTriggers.Add(pTrigger);
                AddTriggerInfo(pTrigger);
            }
        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            if (TriggersListView.SelectedItems.Count == 0)
                return;

            EditTrigger pForm = new EditTrigger(TriggersListView.SelectedItems[0].Tag as Trigger, m_pModule.m_cNumericParameters, m_pModule.m_cBoolParameters, m_pModule.m_cStringParameters);
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
            EditTrigger pForm = new EditTrigger(pTrigger, m_pModule.m_cNumericParameters, m_pModule.m_cBoolParameters, m_pModule.m_cStringParameters);
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

        private void DeleteParameterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ParametersListView.SelectedItems.Count == 0)
                return;

            switch (ParametersTypesListBox.SelectedIndex)
            {
                case 0:
                    {
                        NumericParameter pParam = ParametersListView.SelectedItems[0].Tag as NumericParameter;
                        ParametersListView.Items.Remove(ParametersListView.SelectedItems[0]);
                        m_pModule.m_cNumericParameters.Remove(pParam);
                    }
                    break;
                case 1:
                    {
                        BoolParameter pParam = ParametersListView.SelectedItems[0].Tag as BoolParameter;
                        ParametersListView.Items.Remove(ParametersListView.SelectedItems[0]);
                        m_pModule.m_cBoolParameters.Remove(pParam);
                    }
                    break;
                case 2:
                    {
                        StringParameter pParam = ParametersListView.SelectedItems[0].Tag as StringParameter;
                        ParametersListView.Items.Remove(ParametersListView.SelectedItems[0]);
                        m_pModule.m_cStringParameters.Remove(pParam);
                    }
                    break;
            }
        }
    }
}
