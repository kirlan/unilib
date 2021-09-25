using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Persona.Parameters;
using Persona.Consequences;
using Persona.Collections;
using Persona.Flows;

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

        private void UpdateModuleInfo()
        {
            ModuleNameBox.Text = m_pModule.m_sName;
            ModuleDescBox.Text = m_pModule.m_sDescription;

            actionsList1.UpdateInfo(m_pModule);
            eventsView1.UpdateInfo(m_pModule);
            parametersView1.UpdateInfo(m_pModule);
            triggersView1.UpdateInfo(m_pModule);
            collectionsList1.UpdateInfo(m_pModule);
            flowsList1.UpdateInfo(m_pModule);
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

        private void проверитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TestPlay pForm = new TestPlay(m_pModule);
            pForm.ShowDialog();
        }

        private void EventsListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            reactionsView1.UpdateInfo(m_pModule, eventsView1.SelectedEvent);
        }

        private void ParametersListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            parameterRulesView1.UpdateInfo(m_pModule, parametersView1.SelectedParameter);
        }

        private void FlowsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            milesonesView1.UpdateInfo(m_pModule, flowsList1.SelectedFlow);
        }

        private void CollectionsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            collectedItemsView1.UpdateInfo(m_pModule, collectionsList1.SelectedCollection);
        }
    }

    public static class LISTBOX
    {
        public static void Refresh(ListBox pListBox)
        {
            pListBox.DisplayMember = "";
            pListBox.DisplayMember = "Name";
        }
    }
}
