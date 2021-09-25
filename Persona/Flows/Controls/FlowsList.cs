using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Persona.Flows.Controls
{
    public partial class FlowsList : UserControl
    {
        public FlowsList()
        {
            InitializeComponent();
        }

        private Module m_pModule = null;

        public void UpdateInfo(Module pModule)
        {
            m_pModule = pModule;

            FlowsListBox.Items.Clear();
            FlowsListBox.Items.AddRange(m_pModule.m_cFlows.ToArray());
            if (FlowsListBox.Items.Count > 0)
                FlowsListBox.SelectedIndex = 0;
        }

        public Flow SelectedFlow
        {
            get
            {
                if (FlowsListBox.SelectedItems.Count == 0)
                    return null;

                return FlowsListBox.SelectedItem as Flow;
            }
        }

        public event EventHandler FlowSelected;

        private void FlowsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            EventHandler handler = FlowSelected;
            if (handler != null)
            {
                // Invokes the delegates.
                handler(this, e);
            }
        }

        private void FlowAdd_Click(object sender, EventArgs e)
        {
            Flow pFlow = new Flow();

            EditFlow pForm = new EditFlow(pFlow);
            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                m_pModule.m_cFlows.Add(pFlow);
                FlowsListBox.SelectedIndex = FlowsListBox.Items.Add(pFlow);
            }
        }

        private void FlowEdit_Click(object sender, EventArgs e)
        {
            Flow pFlow = FlowsListBox.SelectedItem as Flow;

            EditFlow pForm = new EditFlow(pFlow);
            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                LISTBOX.Refresh(FlowsListBox);

                FlowsListBox_SelectedIndexChanged(sender, e);
            }
        }
    }
}
