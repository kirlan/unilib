using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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
            textBox1.Text = m_pModule.m_sName;
            textBox2.Text = m_pModule.m_sDescription;

            listBox1.Items.Clear();
            listBox1.Items.AddRange(m_pModule.m_cDomains.ToArray());

            listView1.Items.Clear();
            foreach (var pEvent in m_pModule.m_cEvents)
            {
                ListViewItem pItem = new ListViewItem(pEvent.m_sID.ToString());
                pItem.SubItems.Add(pEvent.m_pDomain.m_sName);
                pItem.SubItems.Add(pEvent.m_iPriority.ToString());
                pItem.SubItems.Add(pEvent.m_iProbability.ToString());
                pItem.SubItems.Add(pEvent.m_bRepeatable ? "+":"-");
            
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
                listView1.Items.Add(pItem);
            }

            if (listView1.Items.Count > 0)
                listView1.Items[0].Selected = true;

            listView2.Items.Clear();
            foreach (var pParam in m_pModule.m_cParameters)
            {
                ListViewItem pItem = new ListViewItem(pParam.m_sName);
                pItem.SubItems.Add(pParam.m_eType.ToString());
                pItem.SubItems.Add(pParam.m_sDescription);

                pItem.Tag = pParam;
                listView2.Items.Add(pItem);
            }
        }

        private void добавитьНовуюМастьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (EditDomain pForm = new EditDomain(new Domain("Новая категория")))
            {
                if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    m_pModule.m_cDomains.Add(pForm.m_pDomain);
                    listBox1.Items.Add(pForm.m_pDomain);
                }
            }
        }

        private void переименоватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
                return;

            using (EditDomain pForm = new EditDomain(listBox1.SelectedItem as Domain))
            {
                if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    listBox1.Refresh();
                }
            }
        }

        private void удалитьМастьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
                return;

            m_pModule.m_cDomains.Remove(listBox1.SelectedItem as Domain);
            listBox1.Items.Remove(listBox1.SelectedItem);
        }

        private int m_iFocusedDomain = -1;
        private void listBox1_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (m_iFocusedDomain != -1)
                if (listBox1.GetItemRectangle(m_iFocusedDomain).Contains(e.Location))
                    return;

            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                if (listBox1.GetItemRectangle(i).Contains(e.Location))
                {
                    m_iFocusedDomain = i;
                    break;
                }
            } 
            
            listBox1.SelectedIndex = m_iFocusedDomain;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            Event pEvent = listView1.SelectedItems[0].Tag as Event;

            listView3.Items.Clear();
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

                listView3.Items.Add(pItem);
            }
        }

        private void listView1_ItemActivate(object sender, EventArgs e)
        {

        }

        private void добавитьНовуюКартуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_pModule.m_cDomains.Count == 0)
                return;

            Event pEvent = new Event(m_pModule.m_cDomains[0]);

            EditEvent pForm = new EditEvent(pEvent, m_pModule.m_cDomains);
            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                m_pModule.m_cEvents.Add(pEvent);
            }
        }
    }
}
