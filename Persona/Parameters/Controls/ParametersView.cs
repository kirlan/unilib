using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Persona.Parameters;

namespace Persona.Core.Controls
{
    public partial class ParametersView : UserControl
    {
        public ParametersView()
        {
            InitializeComponent();
            
            foreach (ColumnHeader pColumn in ParametersListView.Columns)
                m_cParametersColumnWidths[pColumn.Index] = (float)pColumn.Width / ParametersListView.ClientSize.Width;

        }

        private Dictionary<int, float> m_cParametersColumnWidths = new Dictionary<int, float>();

        private void ParametersListView_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            m_cParametersColumnWidths[e.ColumnIndex] = (float)ParametersListView.Columns[e.ColumnIndex].Width / ParametersListView.ClientSize.Width;
        }

        private void ParametersListView_SizeChanged(object sender, EventArgs e)
        {
            foreach (ColumnHeader pColumn in ParametersListView.Columns)
                pColumn.Width = Math.Max(25, (int)(m_cParametersColumnWidths[pColumn.Index] * ParametersListView.ClientSize.Width));
        }

        private Module m_pModule = null;

        public void UpdateInfo(Module pModule)
        {
            m_pModule = pModule;

            ParametersListView.Items.Clear();

            if (m_pModule == null)
                return;

            foreach (var pParam in m_pModule.m_cNumericParameters)
                AddParameterInfo(pParam);
            foreach (var pParam in m_pModule.m_cBoolParameters)
                AddParameterInfo(pParam);
            foreach (var pParam in m_pModule.m_cStringParameters)
                AddParameterInfo(pParam);
        }

        private void AddParameterInfo(Parameter pParam)
        {
            ListViewItem pItem = new ListViewItem(pParam.m_sName);
            pItem.SubItems.Add(pParam.m_sGroup);
            if (pParam is NumericParameter)
                pItem.SubItems.Add("Числовой");
            if (pParam is BoolParameter)
                pItem.SubItems.Add("Логический");
            if (pParam is StringParameter)
                pItem.SubItems.Add("Строковый");
            pItem.SubItems.Add(pParam.m_bHidden ? "#" : "-");
            pItem.SubItems.Add(pParam.m_pFunction == null ? " " : "F");
            pItem.SubItems.Add(pParam.m_sComment);

            pItem.Tag = pParam;
            ParametersListView.Items.Add(pItem);
        }

        public void UpdateSelectedParameterInfo()
        {
            if (ParametersListView.SelectedItems.Count == 0)
                return;
            
            UpdateParameterInfo(ParametersListView.SelectedItems[0]);
        }

        private void UpdateParameterInfo(ListViewItem pItem)
        {
            Parameter pParam = pItem.Tag as Parameter;

            pItem.Text = pParam.m_sName;
            pItem.SubItems[1].Text = pParam.m_sGroup;
            if (pParam is NumericParameter)
                pItem.SubItems[2].Text = "Числовой";
            if (pParam is BoolParameter)
                pItem.SubItems[2].Text = "Логический";
            if (pParam is StringParameter)
                pItem.SubItems[2].Text = "Строковый";
            pItem.SubItems[3].Text = pParam.m_bHidden ? "#" : "-";
            pItem.SubItems[4].Text = pParam.m_pFunction == null ? " " : "F";
            pItem.SubItems[5].Text = pParam.m_sComment;
        }

        private void EditParameterToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (ParametersListView.SelectedItems.Count == 0)
                return;

            Parameter pPar = ParametersListView.SelectedItems[0].Tag as Parameter;

            if (pPar is NumericParameter)
            {
                NumericParameter pParam = ParametersListView.SelectedItems[0].Tag as NumericParameter;
                EditParameterNumeric pForm = new EditParameterNumeric(pParam);
                if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    UpdateParameterInfo(ParametersListView.SelectedItems[0]);
                }
            }
            if (pPar is BoolParameter)
            {
                BoolParameter pParam = ParametersListView.SelectedItems[0].Tag as BoolParameter;
                EditParameterBool pForm = new EditParameterBool(pParam);
                if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    UpdateParameterInfo(ParametersListView.SelectedItems[0]);
                }
            }
            if (pPar is StringParameter)
            {
                StringParameter pParam = ParametersListView.SelectedItems[0].Tag as StringParameter;
                EditParameterString pForm = new EditParameterString(pParam);
                if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    UpdateParameterInfo(ParametersListView.SelectedItems[0]);
                }
            }
        }

        private void DeleteParameterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ParametersListView.SelectedItems.Count == 0)
                return;

            Parameter pPar = ParametersListView.SelectedItems[0].Tag as Parameter;

            if (pPar is NumericParameter)
            {
                NumericParameter pParam = ParametersListView.SelectedItems[0].Tag as NumericParameter;
                ParametersListView.Items.Remove(ParametersListView.SelectedItems[0]);
                m_pModule.m_cNumericParameters.Remove(pParam);
            }
            if (pPar is BoolParameter)
            {
                BoolParameter pParam = ParametersListView.SelectedItems[0].Tag as BoolParameter;
                ParametersListView.Items.Remove(ParametersListView.SelectedItems[0]);
                m_pModule.m_cBoolParameters.Remove(pParam);
            }
            if (pPar is StringParameter)
            {
                StringParameter pParam = ParametersListView.SelectedItems[0].Tag as StringParameter;
                ParametersListView.Items.Remove(ParametersListView.SelectedItems[0]);
                m_pModule.m_cStringParameters.Remove(pParam);
            }
        }

        private void клонироватьToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (ParametersListView.SelectedItems.Count == 0)
                return;

            Parameter pPar = ParametersListView.SelectedItems[0].Tag as Parameter;

            if (pPar is NumericParameter)
            {
                NumericParameter pParam = new NumericParameter(ParametersListView.SelectedItems[0].Tag as NumericParameter, true);
                EditParameterNumeric pForm = new EditParameterNumeric(pParam);
                if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    m_pModule.m_cNumericParameters.Add(pParam);
                    AddParameterInfo(pParam);
                }
            }
            if (pPar is BoolParameter)
            {
                BoolParameter pParam = new BoolParameter(ParametersListView.SelectedItems[0].Tag as BoolParameter, true);
                EditParameterBool pForm = new EditParameterBool(pParam);
                if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    m_pModule.m_cBoolParameters.Add(pParam);
                    AddParameterInfo(pParam);
                }
            }
        }

        private void числовойToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NumericParameter pParam = new NumericParameter();
            EditParameterNumeric pForm = new EditParameterNumeric(pParam);
            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                m_pModule.m_cNumericParameters.Add(pParam);
                AddParameterInfo(pParam);
            }
        }

        private void логическийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BoolParameter pParam = new BoolParameter();
            EditParameterBool pForm = new EditParameterBool(pParam);
            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                m_pModule.m_cBoolParameters.Add(pParam);
                AddParameterInfo(pParam);
            }
        }

        private void строковыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringParameter pParam = new StringParameter();
            EditParameterString pForm = new EditParameterString(pParam);
            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                m_pModule.m_cStringParameters.Add(pParam);
                AddParameterInfo(pParam);
            }
        }

        public Parameter SelectedParameter
        {
            get 
            {
                if (ParametersListView.SelectedItems.Count == 0)
                    return null;

                return ParametersListView.SelectedItems[0].Tag as Parameter;
            }
        }

        public event EventHandler ParameterSelected;

        private void ParametersListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            EventHandler handler = ParameterSelected;
            if (handler != null)
            {
                // Invokes the delegates.
                handler(this, e);
            }
        }
    }
}
