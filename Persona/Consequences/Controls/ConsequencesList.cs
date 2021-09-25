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
    public partial class ConsequencesList : UserControl
    {
        public ConsequencesList()
        {
            InitializeComponent();
        }

        //http://stackoverflow.com/questions/8158419/cannot-add-control-to-form
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<Consequence> Consequences
        {
            set
            {
                listBox2.Items.Clear();
                listBox2.Items.AddRange(value.ToArray());
            }
            get
            {
                List<Consequence> cConsequences = new List<Consequence>();
                foreach (Consequence pConsequence in listBox2.Items)
                    cConsequences.Add(pConsequence);

                return cConsequences;
            }
        }

        public Module m_pModule = null;

        public void Bind(Module pModule)
        {
            m_pModule = pModule;
        }

        private void EditConsequenceToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem == null)
                return;

            if (listBox2.SelectedItem is ParameterSet)
            {
                ParameterSet pConsequence = listBox2.SelectedItem as ParameterSet;
                EditConsequenceSet pForm = new EditConsequenceSet(pConsequence, m_pModule);
                if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    LISTBOX.Refresh(listBox2);
                    s_pLastConsequence = pConsequence;
                }
            }
            if (listBox2.SelectedItem is ParameterChange)
            {
                ParameterChange pConsequence = listBox2.SelectedItem as ParameterChange;
                EditConsequenceChange pForm = new EditConsequenceChange(pConsequence, m_pModule);
                if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    LISTBOX.Refresh(listBox2);
            }
            if (listBox2.SelectedItem is ParameterChangeVariable)
            {
                ParameterChangeVariable pConsequence = listBox2.SelectedItem as ParameterChangeVariable;
                EditConsequenceChangeVariable pForm = new EditConsequenceChangeVariable(pConsequence, m_pModule);
                if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    LISTBOX.Refresh(listBox2);
            }
            if (listBox2.SelectedItem is CollectionShuffle)
            {
                CollectionShuffle pConsequence = listBox2.SelectedItem as CollectionShuffle;
                EditConsequenceShuffle pForm = new EditConsequenceShuffle(pConsequence, m_pModule.m_cCollections);
                if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    LISTBOX.Refresh(listBox2);
            }
            if (listBox2.SelectedItem is CollectionSelect)
            {
                CollectionSelect pConsequence = listBox2.SelectedItem as CollectionSelect;
                EditConsequenceSelect pForm = new EditConsequenceSelect(pConsequence, m_pModule.m_cCollections);
                if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    LISTBOX.Refresh(listBox2);
            }
            if (listBox2.SelectedItem is FlowProgress)
            {
                FlowProgress pConsequence = listBox2.SelectedItem as FlowProgress;
                EditConsequenceFlowProgress pForm = new EditConsequenceFlowProgress(pConsequence, m_pModule.m_cFlows);
                if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    LISTBOX.Refresh(listBox2);
            }
            if (listBox2.SelectedItem is FlowProgressVariable)
            {
                FlowProgressVariable pConsequence = listBox2.SelectedItem as FlowProgressVariable;
                EditConsequenceFlowProgressVariable pForm = new EditConsequenceFlowProgressVariable(pConsequence, m_pModule);
                if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    LISTBOX.Refresh(listBox2);
            }
            if (listBox2.SelectedItem is SystemCommand)
            {
                SystemCommand pConsequence = listBox2.SelectedItem as SystemCommand;
                EditConsequenceCommand pForm = new EditConsequenceCommand(pConsequence);
                if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    LISTBOX.Refresh(listBox2);
            }
        }

        private static ParameterSet s_pLastConsequence = null;

        private void AddConsequenceSetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Parameter pParam = null;
            string sValue = "";

            if (s_pLastConsequence == null)
            {
                if (m_pModule.m_cNumericParameters.Count > 0)
                {
                    pParam = m_pModule.m_cNumericParameters[0];
                    sValue = "0";
                }
                else if (m_pModule.m_cBoolParameters.Count > 0)
                {
                    pParam = m_pModule.m_cBoolParameters[0];
                    sValue = true.ToString();
                }
                else if (m_pModule.m_cStringParameters.Count > 0)
                    pParam = m_pModule.m_cStringParameters[0];
            }
            else
            {
                pParam = s_pLastConsequence.m_pParam;
                sValue = s_pLastConsequence.m_sNewValue;
            }

            if (pParam == null)
                return;

            ParameterSet pConsequence = new ParameterSet(pParam, sValue);
            EditConsequenceSet pForm = new EditConsequenceSet(pConsequence, m_pModule);
            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                listBox2.Items.Add(pConsequence);
                s_pLastConsequence = pConsequence;
            }
        }

        private void DeleteConsequenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem == null)
                return;

            listBox2.Items.Remove(listBox2.SelectedItem);
        }

        private void AddConsequenceChangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_pModule.m_cNumericParameters.Count == 0)
                return;

            ParameterChange pConsequence = new ParameterChange(m_pModule.m_cNumericParameters[0], 1);
            EditConsequenceChange pForm = new EditConsequenceChange(pConsequence, m_pModule);
            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                listBox2.Items.Add(pConsequence);
            }
        }

        private void AddConsequenceChangeVariableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_pModule.m_cNumericParameters.Count == 0)
                return;

            ParameterChangeVariable pConsequence = new ParameterChangeVariable(m_pModule.m_cNumericParameters[0], m_pModule.m_cNumericParameters[0], NumericParameter.Operation.ADD);
            EditConsequenceChangeVariable pForm = new EditConsequenceChangeVariable(pConsequence, m_pModule);
            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                listBox2.Items.Add(pConsequence);
            }
        }

        private void AddConsequenceCommandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SystemCommand pConsequence = new SystemCommand(SystemCommand.ActionType.GameOver);
            EditConsequenceCommand pForm = new EditConsequenceCommand(pConsequence);
            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                listBox2.Items.Add(pConsequence);
            }
        }

        private void AddConsequenceShuffleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_pModule.m_cCollections.Count == 0)
                return;

            CollectionShuffle pConsequence = new CollectionShuffle(m_pModule.m_cCollections[0]);
            EditConsequenceShuffle pForm = new EditConsequenceShuffle(pConsequence, m_pModule.m_cCollections);
            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                listBox2.Items.Add(pConsequence);
            }
        }

        private void AddConsequenceSelectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_pModule.m_cCollections.Count == 0)
                return;

            if (m_pModule.m_cCollections[0].m_cObjects.Count == 0)
                return;

            CollectionSelect pConsequence = new CollectionSelect(m_pModule.m_cCollections[0], m_pModule.m_cCollections[0].m_cObjects.ElementAt(0).Value.m_iID);
            EditConsequenceSelect pForm = new EditConsequenceSelect(pConsequence, m_pModule.m_cCollections);
            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                listBox2.Items.Add(pConsequence);
            }
        }

        private void AddConsequenceFlowProgressToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_pModule.m_cFlows.Count == 0)
                return;

            FlowProgress pConsequence = new FlowProgress(m_pModule.m_cFlows[0], 1);
            EditConsequenceFlowProgress pForm = new EditConsequenceFlowProgress(pConsequence, m_pModule.m_cFlows);
            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                listBox2.Items.Add(pConsequence);
            }
        }

        private void AddConsequenceFlowProgressVariableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_pModule.m_cFlows.Count == 0)
                return;

            if (m_pModule.m_cNumericParameters.Count == 0)
                return;

            FlowProgressVariable pConsequence = new FlowProgressVariable(m_pModule.m_cFlows[0], m_pModule.m_cNumericParameters[0]);
            EditConsequenceFlowProgressVariable pForm = new EditConsequenceFlowProgressVariable(pConsequence, m_pModule);
            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                listBox2.Items.Add(pConsequence);
            }
        }

        private int m_iFocusedConsequence = -1;

        private void listBox2_MouseDown(object sender, MouseEventArgs e)
        {
            if (m_iFocusedConsequence != -1 && m_iFocusedConsequence < listBox2.Items.Count)
                if (listBox2.GetItemRectangle(m_iFocusedConsequence).Contains(e.Location))
                    return;

            m_iFocusedConsequence = -1;

            for (int i = 0; i < listBox2.Items.Count; i++)
            {
                if (listBox2.GetItemRectangle(i).Contains(e.Location))
                {
                    m_iFocusedConsequence = i;
                    break;
                }
            }

            listBox2.SelectedIndex = m_iFocusedConsequence;
        }
    }
}
