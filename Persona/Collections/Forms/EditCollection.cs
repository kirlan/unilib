using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Persona.Parameters;
using Persona.Collections;

namespace Persona
{
    public partial class EditCollection : Form
    {
        public ObjectsCollection m_pCollection;

        public EditCollection(ObjectsCollection pCollection)
        {
            InitializeComponent();

            m_pCollection = pCollection;

            textBox1.Text = m_pCollection.m_sName;

            listView1.Items.Clear();
            foreach (var pParam in m_pCollection.m_cNumericParameters)
                AddParameterInfo(pParam);
            foreach (var pParam in m_pCollection.m_cBoolParameters)
                AddParameterInfo(pParam);
            foreach (var pParam in m_pCollection.m_cStringParameters)
                AddParameterInfo(pParam);
        }

        private void AddParameterInfo(Parameter pParam)
        {
            ListViewItem pItem = new ListViewItem(pParam.m_sName);
            if (pParam is NumericParameter)
                pItem.SubItems.Add("Числовой");
            if (pParam is BoolParameter)
                pItem.SubItems.Add("Логический");
            if (pParam is StringParameter)
                pItem.SubItems.Add("Строковый");

            pItem.Tag = pParam;
            listView1.Items.Add(pItem);
        }

        private void UpdateParameterInfo(ListViewItem pItem)
        {
            Parameter pParam = pItem.Tag as Parameter;

            pItem.Text = pParam.m_sName;
            if (pParam is NumericParameter)
                pItem.SubItems[1].Text = "Числовой";
            if (pParam is BoolParameter)
                pItem.SubItems[1].Text = "Логический";
            if (pParam is StringParameter)
                pItem.SubItems[1].Text = "Строковый";
         }

        private void button2_Click(object sender, EventArgs e)
        {
            m_pCollection.m_sName = textBox1.Text;

            m_pCollection.m_cNumericParameters.Clear();
            m_pCollection.m_cBoolParameters.Clear();
            m_pCollection.m_cStringParameters.Clear();
            foreach (ListViewItem pItem in listView1.Items)
            {
                Parameter pParam = pItem.Tag as Parameter;
                pParam.m_sCollection = m_pCollection.m_sName;
                if (pParam is NumericParameter)
                    m_pCollection.m_cNumericParameters.Add((NumericParameter)pParam);
                if (pParam is BoolParameter)
                    m_pCollection.m_cBoolParameters.Add((BoolParameter)pParam);
                if (pParam is StringParameter)
                    m_pCollection.m_cStringParameters.Add((StringParameter)pParam);
            }

            DialogResult = DialogResult.OK;
        }

        private void AddNumericToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NumericParameter pParam = new NumericParameter();

            EditParameterNumeric pForm = new EditParameterNumeric(pParam, textBox1.Text);
            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                AddParameterInfo(pParam);
            }
        }

        private void AddBoolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BoolParameter pParam = new BoolParameter();

            EditParameterBool pForm = new EditParameterBool(pParam, textBox1.Text);
            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                AddParameterInfo(pParam);
            }
        }


        private void AddStringToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringParameter pParam = new StringParameter();

            EditParameterString pForm = new EditParameterString(pParam, textBox1.Text);
            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                AddParameterInfo(pParam);
            }
        }

        private void EditParamToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            Parameter pPar = listView1.SelectedItems[0].Tag as Parameter;

            if (pPar is NumericParameter)
            {
                NumericParameter pParam = listView1.SelectedItems[0].Tag as NumericParameter;
                EditParameterNumeric pForm = new EditParameterNumeric(pParam, textBox1.Text);
                if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    UpdateParameterInfo(listView1.SelectedItems[0]);
                }
            }
            if (pPar is BoolParameter)
            {
                BoolParameter pParam = listView1.SelectedItems[0].Tag as BoolParameter;
                EditParameterBool pForm = new EditParameterBool(pParam, textBox1.Text);
                if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    UpdateParameterInfo(listView1.SelectedItems[0]);
                }
            }
            if (pPar is StringParameter)
            {
                StringParameter pParam = listView1.SelectedItems[0].Tag as StringParameter;
                EditParameterString pForm = new EditParameterString(pParam, textBox1.Text);
                if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    UpdateParameterInfo(listView1.SelectedItems[0]);
                }
            }
        }

        private void RemoveParamToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            listView1.Items.Remove(listView1.SelectedItems[0]);
        }
    }
}
