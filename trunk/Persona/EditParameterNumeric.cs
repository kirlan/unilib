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
    public partial class EditParameterNumeric : Form
    {
        private NumericParameter m_pParam;

        private void AddRangeInfo(NumericParameter.Range pRange)
        {
            ListViewItem pItem = new ListViewItem(string.Format("{0} .. {1}", pRange.m_fMin, pRange.m_fMax));
            pItem.SubItems.Add(pRange.m_sDescription);

            pItem.Tag = pRange;

            listView1.Items.Add(pItem);
        }

        public EditParameterNumeric(NumericParameter pParam)
        {
            InitializeComponent();

            m_pParam = pParam;

            textBox1.Text = m_pParam.m_sName;
            
            comboBox1.Items.Clear();
            comboBox1.Text = m_pParam.m_sGroup;

            if (m_pParam.m_bHidden)
                radioButton7.Checked = true;
            else
                radioButton6.Checked = true;

            textBox2.Text = m_pParam.m_sComment;

            numericUpDown1.Value = (decimal)m_pParam.m_fDefaultValue;
            numericUpDown2.Value = (decimal)m_pParam.m_fMin;
            numericUpDown3.Value = (decimal)m_pParam.m_fMax;

            listView1.Items.Clear();
            foreach (var pRange in m_pParam.m_cRanges)
                AddRangeInfo(pRange);
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void добавитьДиапазонToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NumericParameter.Range pRange = new NumericParameter.Range();

            EditRange pForm = new EditRange(pRange, (float)numericUpDown2.Value, (float)numericUpDown3.Value);
            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //m_pParam.m_cRanges.Add(pRange);
                AddRangeInfo(pRange);
            }
        }

        private void редактироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            NumericParameter.Range pRange = listView1.SelectedItems[0].Tag as NumericParameter.Range;

            EditRange pForm = new EditRange(pRange, (float)numericUpDown2.Value, (float)numericUpDown3.Value);
            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                listView1.SelectedItems[0].Text = string.Format("{0} .. {1}", pRange.m_fMin, pRange.m_fMax);
                listView1.SelectedItems[0].SubItems[1].Text = pRange.m_sDescription;
            }
        }

        private void удалитьДиапазонToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<ListViewItem> cKills = new List<ListViewItem>();
            foreach (ListViewItem pItem in listView1.SelectedItems)
                cKills.Add(pItem);

            foreach (var pItem in cKills)
            {
                listView1.Items.Remove(pItem);
                //m_pParam.m_cRanges.Remove(pItem.Tag as NumericParameter.Range);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            m_pParam.m_sName = textBox1.Text;
            m_pParam.m_sGroup = comboBox1.Text;
            m_pParam.m_bHidden = radioButton7.Checked;
            m_pParam.m_sComment = textBox2.Text;
            m_pParam.m_fDefaultValue = (float)numericUpDown1.Value;
            m_pParam.m_fMin = (float)numericUpDown2.Value;
            m_pParam.m_fMax = (float)numericUpDown3.Value;
            m_pParam.m_cRanges.Clear();
            foreach (ListViewItem pItem in listView1.Items)
                m_pParam.m_cRanges.Add(pItem.Tag as NumericParameter.Range);

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
