using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Persona.Consequences;
using Persona.Parameters;

namespace Persona.Core.Controls
{
    public partial class ParameterRulesView : UserControl
    {
        public ParameterRulesView()
        {
            InitializeComponent();

            foreach (ColumnHeader pColumn in RulesListView.Columns)
                m_cRulesColumnWidths[pColumn.Index] = (float)pColumn.Width / RulesListView.ClientSize.Width;
        }

        private Module m_pModule = null;
        private Parameter m_pSelectedParameter = null;

        public void UpdateInfo(Module pModule, Parameter pParam)
        {
            m_pModule = pModule;
            m_pSelectedParameter = pParam;

            RulesListView.Items.Clear();

            if (m_pModule == null || m_pSelectedParameter == null)
                return;

            foreach (Function pFunction in m_pModule.m_cFunctions)
                if (pFunction.m_pParam == m_pSelectedParameter)
                {
                    foreach (Function.Rule pRule in pFunction.m_cRules)
                    {
                        AddRuleInfo(pFunction, pRule);
                    }
                    return;
                }
        }

        private string GetRuleValue(Function pFunction, Function.Rule pRule)
        {
            string sValue = "";
            if (pRule.m_pConsequence is ParameterSet)
            {
                sValue = (pRule.m_pConsequence as ParameterSet).m_sNewValue;
                if (pFunction.m_pParam != null)
                    sValue = pFunction.m_pParam.GetDisplayValue(sValue);
            }
            if (pRule.m_pConsequence is ParameterChangeVariable)
            {
                switch ((pRule.m_pConsequence as ParameterChangeVariable).m_eOperation)
                {
                    case NumericParameter.Operation.ADD:
                        return string.Format("[{0}] + [{1}]", (pRule.m_pConsequence as ParameterChangeVariable).m_pParam1 != null ? (pRule.m_pConsequence as ParameterChangeVariable).m_pParam1.FullName : "НЕВЕРНЫЙ ПАРАМЕТР", (pRule.m_pConsequence as ParameterChangeVariable).m_pParam2 != null ? (pRule.m_pConsequence as ParameterChangeVariable).m_pParam2.FullName : "НЕВЕРНЫЙ ПАРАМЕТР");
                    case NumericParameter.Operation.SUB:
                        return string.Format("[{0}] - [{1}]", (pRule.m_pConsequence as ParameterChangeVariable).m_pParam1 != null ? (pRule.m_pConsequence as ParameterChangeVariable).m_pParam1.FullName : "НЕВЕРНЫЙ ПАРАМЕТР", (pRule.m_pConsequence as ParameterChangeVariable).m_pParam2 != null ? (pRule.m_pConsequence as ParameterChangeVariable).m_pParam2.FullName : "НЕВЕРНЫЙ ПАРАМЕТР");
                    case NumericParameter.Operation.SET:
                        return string.Format("[{1}]", (pRule.m_pConsequence as ParameterChangeVariable).m_pParam1 != null ? (pRule.m_pConsequence as ParameterChangeVariable).m_pParam1.FullName : "НЕВЕРНЫЙ ПАРАМЕТР", (pRule.m_pConsequence as ParameterChangeVariable).m_pParam2 != null ? (pRule.m_pConsequence as ParameterChangeVariable).m_pParam2.FullName : "НЕВЕРНЫЙ ПАРАМЕТР");
                    case NumericParameter.Operation.AVG:
                        return string.Format("[{0}] -> [{1}]", (pRule.m_pConsequence as ParameterChangeVariable).m_pParam1 != null ? (pRule.m_pConsequence as ParameterChangeVariable).m_pParam1.FullName : "НЕВЕРНЫЙ ПАРАМЕТР", (pRule.m_pConsequence as ParameterChangeVariable).m_pParam2 != null ? (pRule.m_pConsequence as ParameterChangeVariable).m_pParam2.FullName : "НЕВЕРНЫЙ ПАРАМЕТР");
                    default:
                        return string.Format("[{0}] ?? [{1}]", (pRule.m_pConsequence as ParameterChangeVariable).m_pParam1 != null ? (pRule.m_pConsequence as ParameterChangeVariable).m_pParam1.FullName : "НЕВЕРНЫЙ ПАРАМЕТР", (pRule.m_pConsequence as ParameterChangeVariable).m_pParam2 != null ? (pRule.m_pConsequence as ParameterChangeVariable).m_pParam2.FullName : "НЕВЕРНЫЙ ПАРАМЕТР");
                }
            }
            return sValue;
        }

        private void AddRuleInfo(Function pFunction, Function.Rule pRule)
        {
            string conditions = "";
            foreach (var pCondition in pRule.m_cConditions)
            {
                if (conditions.Length > 0)
                    conditions += " И ";
                conditions += pCondition.ToString();
            }

            ListViewItem pItem = new ListViewItem(conditions);

            pItem.SubItems.Add(GetRuleValue(pFunction, pRule));

            pItem.Tag = pRule;

            RulesListView.Items.Add(pItem);
        }

        private Dictionary<int, float> m_cRulesColumnWidths = new Dictionary<int, float>();

        private void RulesListView_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            m_cRulesColumnWidths[e.ColumnIndex] = (float)RulesListView.Columns[e.ColumnIndex].Width / RulesListView.ClientSize.Width;
        }

        private void RulesListView_SizeChanged(object sender, EventArgs e)
        {
            foreach (ColumnHeader pColumn in RulesListView.Columns)
                pColumn.Width = Math.Max(25, (int)(m_cRulesColumnWidths[pColumn.Index] * RulesListView.ClientSize.Width));
        }

        private void AddRuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_pSelectedParameter == null)
                return;

            //RulesListView.Items.Clear();

            Function pFunction = m_pSelectedParameter.m_pFunction;

            bool bNewFunction = false;
            if (pFunction == null)
            {
                pFunction = new Function(m_pSelectedParameter);
                bNewFunction = true;
            }

            Function.Rule pRule = new Function.Rule(m_pSelectedParameter);

            EditRule pForm = new EditRule(pFunction, pRule, m_pModule);
            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (bNewFunction)
                {
                    m_pModule.m_cFunctions.Add(pFunction);
                    m_pSelectedParameter.m_pFunction = pFunction;
                    //UpdateParameterInfo(ParametersListView.SelectedItems[0]);
                }

                pFunction.m_cRules.Add(pRule);

                AddRuleInfo(pFunction, pRule);
            }
        }

        private void EditRuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_pSelectedParameter == null)
                return;

            if (RulesListView.SelectedItems.Count == 0)
                return;

            if (m_pSelectedParameter.m_pFunction == null)
                return;

            Function.Rule pRule = RulesListView.SelectedItems[0].Tag as Function.Rule;

            Form pForm;
            if (pRule.m_pConsequence is ParameterChangeVariable)
                pForm = new EditRuleNumericFormula(m_pSelectedParameter.m_pFunction, pRule, m_pModule);
            else
                pForm = new EditRule(m_pSelectedParameter.m_pFunction, pRule, m_pModule);
            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ListViewItem pItem = RulesListView.SelectedItems[0];
                string conditions = "";
                foreach (var pCondition in pRule.m_cConditions)
                {
                    if (conditions.Length > 0)
                        conditions += " И ";
                    conditions += pCondition.ToString();
                }
                pItem.SubItems[0].Text = conditions;
                pItem.SubItems[1].Text = GetRuleValue(m_pSelectedParameter.m_pFunction, pRule);
            }
        }

        private void CopyRuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_pSelectedParameter == null)
                return;

            if (RulesListView.SelectedItems.Count == 0)
                return;

            if (m_pSelectedParameter.m_pFunction == null)
                return;

            Function.Rule pRule = new Function.Rule(RulesListView.SelectedItems[0].Tag as Function.Rule);

            Form pForm;
            if (pRule.m_pConsequence is ParameterChangeVariable)
                pForm = new EditRuleNumericFormula(m_pSelectedParameter.m_pFunction, pRule, m_pModule);
            else
                pForm = new EditRule(m_pSelectedParameter.m_pFunction, pRule, m_pModule);
            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                m_pSelectedParameter.m_pFunction.m_cRules.Add(pRule);

                AddRuleInfo(m_pSelectedParameter.m_pFunction, pRule);
            }
        }

        private void DeleteRuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_pSelectedParameter == null)
                return;

            if (m_pSelectedParameter.m_pFunction == null)
                return;

            List<ListViewItem> cKills = new List<ListViewItem>();
            foreach (ListViewItem pItem in RulesListView.SelectedItems)
                cKills.Add(pItem);

            foreach (var pItem in cKills)
            {
                RulesListView.Items.Remove(pItem);
                m_pSelectedParameter.m_pFunction.m_cRules.Remove(pItem.Tag as Function.Rule);
            }

            if (m_pSelectedParameter.m_pFunction.m_cRules.Count == 0)
            {
                m_pModule.m_cFunctions.Remove(m_pSelectedParameter.m_pFunction);
                m_pSelectedParameter.m_pFunction = null;
                //UpdateParameterInfo(ParametersListView.SelectedItems[0]);
            }
        }

        private void AddRuleFormulaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_pSelectedParameter == null)
                return;

            if (m_pModule.m_cNumericParameters.Count == 0)
                return;

            //RulesListView.Items.Clear();

            Function pFunction = m_pSelectedParameter.m_pFunction;

            bool bNewFunction = false;
            if (pFunction == null)
            {
                pFunction = new Function(m_pSelectedParameter);
                bNewFunction = true;
            }

            ParameterChangeVariable pConsequence = new ParameterChangeVariable(m_pModule.m_cNumericParameters[0], m_pModule.m_cNumericParameters[0], NumericParameter.Operation.ADD);
            Function.Rule pRule = new Function.Rule(pConsequence);

            EditRuleNumericFormula pForm = new EditRuleNumericFormula(pFunction, pRule, m_pModule);
            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (bNewFunction)
                {
                    m_pModule.m_cFunctions.Add(pFunction);
                    m_pSelectedParameter.m_pFunction = pFunction;
                    //UpdateParameterInfo(ParametersListView.SelectedItems[0]);
                }

                pFunction.m_cRules.Add(pRule);

                AddRuleInfo(pFunction, pRule);
            }
        }

        private void contextMenuStrip_Rules_Opening(object sender, CancelEventArgs e)
        {
            if (m_pSelectedParameter == null)
                return;

            addNewRuleFormula_toolstripItem.Enabled = m_pSelectedParameter is NumericParameter;

            if (RulesListView.SelectedItems.Count == 0)
                return;

            if (m_pSelectedParameter.m_pFunction == null)
                return;

            Function.Rule pRule = RulesListView.SelectedItems[0].Tag as Function.Rule;

            transformToFormula_toolstripItem.Enabled = pRule.m_pConsequence is ParameterSet;
        }

        private void transformToFormula_toolstripItem_Click(object sender, EventArgs e)
        {
            if (m_pSelectedParameter == null)
                return;

            if (RulesListView.SelectedItems.Count == 0)
                return;

            if (m_pSelectedParameter.m_pFunction == null)
                return;

            if (m_pModule.m_cNumericParameters.Count == 0)
                return;

            Function.Rule pRule = RulesListView.SelectedItems[0].Tag as Function.Rule;
            pRule.m_pConsequence = new ParameterChangeVariable(m_pModule.m_cNumericParameters[0], m_pModule.m_cNumericParameters[0], NumericParameter.Operation.ADD);

            Form pForm = new EditRuleNumericFormula(m_pSelectedParameter.m_pFunction, pRule, m_pModule);
            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ListViewItem pItem = RulesListView.SelectedItems[0];
                string conditions = "";
                foreach (var pCondition in pRule.m_cConditions)
                {
                    if (conditions.Length > 0)
                        conditions += " И ";
                    conditions += pCondition.ToString();
                }
                pItem.SubItems[0].Text = conditions;
                pItem.SubItems[1].Text = GetRuleValue(m_pSelectedParameter.m_pFunction, pRule);
            }
        }
    }
}
