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
                ListViewItem pItem = new ListViewItem(pEvent.m_iID.ToString());
                pItem.SubItems.Add(pEvent.m_cDescriptions[0].m_sText);
            
                string conditions = "";
                foreach (var pCondition in pEvent.m_cDescriptions[0].m_cConditions)
                {
                    if (conditions.Length > 0)
                        conditions += " И ";
                    conditions += pCondition.ToString();
                }
                pItem.SubItems.Add(conditions);

                pItem.Tag = pEvent;
                listView1.Items.Add(pItem);
            }

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
            //if (m_iFocusedDomain != -1)
            //    if (listBox1.GetItemRectangle(m_iFocusedDomain).Contains(e.Location))
            //        return;

            //for(int i=0; i<listBox1.Items.Count; i++)
            //{
            //    if (listBox1.GetItemRectangle(i).Contains(e.Location))
            //    {
            //        m_iFocusedDomain = i;
            //        break;
            //    }
            //}

            //listBox1.SelectedIndex = m_iFocusedDomain;
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
    }
}
