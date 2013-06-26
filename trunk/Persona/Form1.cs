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

            UpdateModuleInfo();
        }

        private void AddEventInfo(Event pEvent)
        {
            ListViewItem pItem = new ListViewItem(pEvent.m_sID.ToString());
            pItem.SubItems.Add(pEvent.m_pDomain.m_sName);
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

            CategoriesListBox.Items.Clear();
            CategoriesListBox.Items.AddRange(m_pModule.m_cDomains.ToArray());

            EventsListView.Items.Clear();
            foreach (var pEvent in m_pModule.m_cEvents)
            {
                AddEventInfo(pEvent);
            }

            if (EventsListView.Items.Count > 0)
                EventsListView.Items[0].Selected = true;

            ParametersTypesListBox.SelectedIndex = 0;
            ParametersTypesListBox_SelectedIndexChanged(this, new EventArgs());
        }

        private void AddCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (EditDomain pForm = new EditDomain(new Domain("Новая категория")))
            {
                if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    m_pModule.m_cDomains.Add(pForm.m_pDomain);
                    CategoriesListBox.Items.Add(pForm.m_pDomain);
                }
            }
        }

        private void EditCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CategoriesListBox.SelectedItem == null)
                return;

            using (EditDomain pForm = new EditDomain(CategoriesListBox.SelectedItem as Domain))
            {
                if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    CategoriesListBox.Refresh();
                }
            }
        }

        private void RemoveCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CategoriesListBox.SelectedItem == null)
                return;

            m_pModule.m_cDomains.Remove(CategoriesListBox.SelectedItem as Domain);
            CategoriesListBox.Items.Remove(CategoriesListBox.SelectedItem);
        }

        private int m_iFocusedDomain = -1;

        private void CategoriesListBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (m_iFocusedDomain != -1)
                if (CategoriesListBox.GetItemRectangle(m_iFocusedDomain).Contains(e.Location))
                    return;

            for (int i = 0; i < CategoriesListBox.Items.Count; i++)
            {
                if (CategoriesListBox.GetItemRectangle(i).Contains(e.Location))
                {
                    m_iFocusedDomain = i;
                    break;
                }
            } 
            
            CategoriesListBox.SelectedIndex = m_iFocusedDomain;
        }

        private void EventsListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (EventsListView.SelectedItems.Count == 0)
                return;

            Event pEvent = EventsListView.SelectedItems[0].Tag as Event;

            ReactionsListView.Items.Clear();
            foreach (Reaction pReaction in pEvent.m_cReactions)
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
        }

        private void AddEventToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_pModule.m_cDomains.Count == 0)
                return;

            Event pEvent = new Event(m_pModule.m_cDomains[0]);

            EditEvent pForm = new EditEvent(pEvent, m_pModule.m_cDomains, m_pModule.m_cNumericParameters, m_pModule.m_cBoolParameters);
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

            EditEvent pForm = new EditEvent(EventsListView.SelectedItems[0].Tag as Event, m_pModule.m_cDomains, m_pModule.m_cNumericParameters, m_pModule.m_cBoolParameters);
            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ListViewItem pItem = EventsListView.SelectedItems[0];
                pItem.SubItems[0].Text = pForm.m_pEvent.m_sID.ToString();
                pItem.SubItems[1].Text = pForm.m_pEvent.m_pDomain.m_sName;
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

        private void добавитьНовыйПараметрToolStripMenuItem_Click(object sender, EventArgs e)
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
                    break;
                case 2:
                    break;
            }
        }

        private void редактироватьToolStripMenuItem1_Click(object sender, EventArgs e)
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
                    break;
                case 2:
                    break;
            }
        }
    }
}
