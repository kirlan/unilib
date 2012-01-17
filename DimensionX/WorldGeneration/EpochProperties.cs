using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Socium;
using Random;
using Socium.Nations;

namespace WorldGeneration
{
    public partial class EpochProperties : UserControl
    {
        private EpochWrapper m_pEpoch = null;

        public EpochWrapper Epoch
        {
            get { return m_pEpoch; }
            set 
            { 
                m_pEpoch = value;

                Enabled = m_pEpoch != null;

                if (m_pEpoch != null)
                {
                    textBox2.Text = m_pEpoch.Name;
                    numericUpDown1.Value = m_pEpoch.Length;

                    RedrawPanels(this, new EventArgs());
                }
            }
        }

        private bool m_bAdvancedMode = false;

        public bool AdvancedMode
        {
            get { return m_bAdvancedMode; }
            set
            {
                m_bAdvancedMode = value;
                PresetsPanel.Visible = !m_bAdvancedMode;
                AdvancedPanel.Visible = m_bAdvancedMode;
            }
        }
        
        public EpochProperties()
        {
            InitializeComponent();

            PresetsPanel.Top = 0;
            PresetsPanel.Left = 0;
            PresetsPanel.Width = ClientRectangle.Width;
            PresetsPanel.Height = ClientRectangle.Height;
            PresetsPanel.Visible = true;

            AdvancedPanel.Top = 0;
            AdvancedPanel.Left = 0;
            AdvancedPanel.Width = ClientRectangle.Width;
            AdvancedPanel.Height = ClientRectangle.Height;
            AdvancedPanel.Visible = false;

            checkedListBox1.Items.Clear();
            foreach (Race pRace in Race.m_cAllRaces)
                checkedListBox1.Items.Add(pRace);

            for (int i = 0; i < 9; i++)
            {
                BaseTechBox.Items.Add(string.Format("{0} [T{1}]", State.GetTechString(i, Socium.Psichology.Customs.Progressiveness.Normal), i));
                MaxTechBox.Items.Add(string.Format("{0} [T{1}]", State.GetTechString(i, Socium.Psichology.Customs.Progressiveness.Normal), i));

                BaseMagicBox.Items.Add(string.Format("{0} [M{1}]", State.GetMagicString(i), i));
                MaxMagicBox.Items.Add(string.Format("{0} [M{1}]", State.GetMagicString(i), i));
            }

            BaseTechBox.SelectedIndex = 0;
            BaseMagicBox.SelectedIndex = 0;

            MaxTechBox.SelectedIndex = 8;
            MaxMagicBox.SelectedIndex = 8;

            NativesSocialPreset.Items.Clear();
            NativesSocialPreset.Items.AddRange(ProgressPreset.s_aSocietyPresets);
            NativesSocialPreset.Items.Add("Custom progress preset");
            if (NativesSocialPreset.Items.Count > 0)
                NativesSocialPreset.SelectedIndex = 0;

            InvadersSocialPreset.Items.Clear();
            InvadersSocialPreset.Items.AddRange(ProgressPreset.s_aSocietyPresets);
            InvadersSocialPreset.Items.Add("Custom progress preset");
            if (InvadersSocialPreset.Items.Count > 0)
                InvadersSocialPreset.SelectedIndex = 0;

            NativesRacesSet.Items.Clear();
            InvadersRacesSet.Items.Clear();
            foreach (RacesSet pSet in RacesSet.s_aSets)
            {
                NativesRacesSet.Items.Add(pSet);
                InvadersRacesSet.Items.Add(pSet);

                NativesRacesSet.CheckBoxItems[pSet.ToString()].Checked = true;
                InvadersRacesSet.CheckBoxItems[pSet.ToString()].Checked = true;
            }

            Enabled = false;
        }

        private void EpochProperties_Resize(object sender, EventArgs e)
        {
            PresetsPanel.Width = ClientRectangle.Width;
            PresetsPanel.Height = ClientRectangle.Height;

            AdvancedPanel.Width = ClientRectangle.Width;
            AdvancedPanel.Height = ClientRectangle.Height;
        }

        bool m_bInitMode = false;

        private void RedrawPanels(object sender, EventArgs e)
        {
            if (m_pEpoch == null)
                return;

            if (AdvancedPanel.Visible)
                tabControl1_SelectedIndexChanged(sender, e);
            else
            {
                if (m_pEpoch.NativesPreset != null)
                    NativesSocialPreset.SelectedItem = m_pEpoch.NativesPreset;
                else
                    NativesSocialPreset.SelectedItem = "Custom progress preset";

                if (m_pEpoch.InvadersPreset != null)
                    InvadersSocialPreset.SelectedItem = m_pEpoch.InvadersPreset;
                else
                    InvadersSocialPreset.SelectedItem = "Custom progress preset";

                m_bInitMode = true;
                foreach (RacesSet pSet in RacesSet.s_aSets)
                {
                    NativesRacesSet.CheckBoxItems[pSet.ToString()].Checked = m_pEpoch.NativesRacesSets.Contains(pSet);
                    InvadersRacesSet.CheckBoxItems[pSet.ToString()].Checked = m_pEpoch.InvadersRacesSets.Contains(pSet);
                }
                m_bInitMode = false;

                NativesRacesCount.Value = m_pEpoch.NativesCount;
                InvadersRacesCount.Value = m_pEpoch.InvadersCount;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                BaseTechBox.SelectedIndex = m_pEpoch.NativesMinTechLevel;
                MaxTechBox.SelectedIndex = m_pEpoch.NativesMaxTechLevel - m_pEpoch.NativesMinTechLevel;
                BaseMagicBox.SelectedIndex = m_pEpoch.NativesMinMagicLevel;
                MaxMagicBox.SelectedIndex = m_pEpoch.NativesMaxMagicLevel - m_pEpoch.NativesMinMagicLevel;

                RacesCount.Value = m_pEpoch.NativesCount;
                RacesCount.Minimum = 1;

                m_bInitMode = true;
                for (int i = 0; i < checkedListBox1.Items.Count; i++ )
                    checkedListBox1.SetItemChecked(i, m_pEpoch.NativesRaces.Contains(checkedListBox1.Items[i]));
                m_bInitMode = false;
            }
            else
            {
                BaseTechBox.SelectedIndex = m_pEpoch.InvadersMinTechLevel;
                MaxTechBox.SelectedIndex = m_pEpoch.InvadersMaxTechLevel - m_pEpoch.InvadersMinTechLevel;
                BaseMagicBox.SelectedIndex = m_pEpoch.InvadersMinMagicLevel;
                MaxMagicBox.SelectedIndex = m_pEpoch.InvadersMaxMagicLevel - m_pEpoch.InvadersMinMagicLevel;

                RacesCount.Minimum = 0;
                RacesCount.Value = m_pEpoch.InvadersCount;

                m_bInitMode = true;
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                    checkedListBox1.SetItemChecked(i, m_pEpoch.InvadersRaces.Contains(checkedListBox1.Items[i]));
                m_bInitMode = false;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            m_pEpoch.Name = textBox2.Text;
            if (textBox3.Text != textBox2.Text)
                textBox3.Text = textBox2.Text;

            FireUpdate();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            m_pEpoch.Name = textBox3.Text;
            if (textBox2.Text != textBox3.Text)
                textBox2.Text = textBox3.Text;

            FireUpdate();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            m_pEpoch.Length = (int)numericUpDown1.Value;
            if (numericUpDown2.Value != numericUpDown1.Value)
                numericUpDown2.Value = numericUpDown1.Value;

            FireUpdate();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            m_pEpoch.Length = (int)numericUpDown2.Value;
            if (numericUpDown1.Value != numericUpDown2.Value)
                numericUpDown1.Value = numericUpDown2.Value;

            FireUpdate();
        }

        private void BaseTechBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_pEpoch == null)
                return;

            if(tabControl1.SelectedIndex == 0)
                m_pEpoch.NativesMinTechLevel = BaseTechBox.SelectedIndex;
            else
                m_pEpoch.InvadersMinTechLevel = BaseTechBox.SelectedIndex;

            MaxTechBox.Items.Clear();
            for (int i = BaseTechBox.SelectedIndex; i < 9; i++)
                MaxTechBox.Items.Add(string.Format("{0} [T{1}]", State.GetTechString(i, Socium.Psichology.Customs.Progressiveness.Normal), i));

            if (tabControl1.SelectedIndex == 0)
                MaxTechBox.SelectedIndex = Math.Max(0, m_pEpoch.NativesMaxTechLevel - BaseTechBox.SelectedIndex);
            else
                MaxTechBox.SelectedIndex = Math.Max(0, m_pEpoch.InvadersMaxTechLevel - BaseTechBox.SelectedIndex);

            FireUpdate();
        }

        private void MaxTechBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_pEpoch == null)
                return;

            if (MaxTechBox.SelectedIndex != -1)
                if (tabControl1.SelectedIndex == 0)
                    m_pEpoch.NativesMaxTechLevel = BaseTechBox.SelectedIndex + MaxTechBox.SelectedIndex;
                else
                    m_pEpoch.InvadersMaxTechLevel = BaseTechBox.SelectedIndex + MaxTechBox.SelectedIndex;

            FireUpdate();
        }

        private void BaseMagicBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_pEpoch == null)
                return;

            if (tabControl1.SelectedIndex == 0)
                m_pEpoch.NativesMinMagicLevel = BaseMagicBox.SelectedIndex;
            else
                m_pEpoch.InvadersMinMagicLevel = BaseMagicBox.SelectedIndex;

            MaxMagicBox.Items.Clear();
            for (int i = BaseMagicBox.SelectedIndex; i < 9; i++)
                MaxMagicBox.Items.Add(string.Format("{0} [M{1}]", State.GetMagicString(i), i));

            if (tabControl1.SelectedIndex == 0)
                MaxMagicBox.SelectedIndex = Math.Max(0, m_pEpoch.NativesMaxMagicLevel - BaseMagicBox.SelectedIndex);
            else
                MaxMagicBox.SelectedIndex = Math.Max(0, m_pEpoch.InvadersMaxMagicLevel - BaseMagicBox.SelectedIndex);

            FireUpdate();
        }

        private void MaxMagicBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_pEpoch == null)
                return;

            if (MaxMagicBox.SelectedIndex != -1)
                if (tabControl1.SelectedIndex == 0)
                    m_pEpoch.NativesMaxMagicLevel = BaseMagicBox.SelectedIndex + MaxMagicBox.SelectedIndex;
                else
                    m_pEpoch.InvadersMaxMagicLevel = BaseMagicBox.SelectedIndex + MaxMagicBox.SelectedIndex;

            FireUpdate();
        }

        private void NativesSocialPreset_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_pEpoch == null)
                return;

            if (NativesSocialPreset.SelectedIndex != -1)
            {
                m_pEpoch.NativesPreset = NativesSocialPreset.SelectedItem as ProgressPreset;
                if (m_pEpoch.NativesPreset != null)
                    NativesPresetDescription.Text = m_pEpoch.NativesPreset.m_sDescription;
                else
                    NativesPresetDescription.Text = "Custom preset. Use advanced mode to set real values or choose any other progress preset.";
            }

            FireUpdate();
        }

        private void InvadersSocialPreset_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_pEpoch == null)
                return;

            if (InvadersSocialPreset.SelectedIndex != -1)
            {
                m_pEpoch.InvadersPreset = InvadersSocialPreset.SelectedItem as ProgressPreset;
                if (m_pEpoch.InvadersPreset != null)
                    InvadersPresetDescription.Text = m_pEpoch.InvadersPreset.m_sDescription;
                else
                    InvadersPresetDescription.Text = "Custom preset. Use advanced mode to set real values or choose any other progress preset.";
            }

            FireUpdate();
        }

        private void NativesRacesSet_CheckBoxCheckedChanged(object sender, EventArgs e)
        {
            if (m_pEpoch == null || m_bInitMode)
                return;

            List<RacesSet> cRaces = new List<RacesSet>();
            for (int i = 0; i < NativesRacesSet.Items.Count; i++)
            {
                if (NativesRacesSet.CheckBoxItems[i].Checked)
                    cRaces.Add(NativesRacesSet.Items[i] as RacesSet);
            }

            m_pEpoch.NativesRacesSets = cRaces.ToArray();

            FireUpdate();
        }

        private void InvadersRacesSet_CheckBoxCheckedChanged(object sender, EventArgs e)
        {
            if (m_pEpoch == null || m_bInitMode)
                return;

            List<RacesSet> cRaces = new List<RacesSet>();
            for (int i = 0; i < InvadersRacesSet.Items.Count; i++)
            {
                if (InvadersRacesSet.CheckBoxItems[i].Checked)
                    cRaces.Add(InvadersRacesSet.Items[i] as RacesSet);
            }

            m_pEpoch.InvadersRacesSets = cRaces.ToArray();

            FireUpdate();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            NativesRacesCount.Value = NativesRacesCount.Minimum + Rnd.Get(NativesRacesCount.Maximum - NativesRacesCount.Minimum);
        }

        private void InvadersRacesCount_ValueChanged(object sender, EventArgs e)
        {
            m_pEpoch.InvadersCount = InvadersRacesCount.Value;
            InvadersRacesCountNumber.Text = m_pEpoch.InvadersCount.ToString();

            InvadersRacesSet.Enabled = m_pEpoch.InvadersCount > 0;
            InvadersSocialPreset.Enabled = m_pEpoch.InvadersCount > 0;

            FireUpdate();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            InvadersRacesCount.Value = InvadersRacesCount.Minimum + Rnd.Get(InvadersRacesCount.Maximum - InvadersRacesCount.Minimum);
        }

        private void NativesRacesCount_ValueChanged(object sender, EventArgs e)
        {
            m_pEpoch.NativesCount = NativesRacesCount.Value;
            NativesRacesCountNumber.Text = m_pEpoch.NativesCount.ToString();

            FireUpdate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RacesCount.Value = RacesCount.Minimum + Rnd.Get(RacesCount.Maximum - RacesCount.Minimum);
        }

        private void RacesCount_ValueChanged(object sender, EventArgs e)
        {
            if(tabControl1.SelectedIndex == 0)
                m_pEpoch.NativesCount = RacesCount.Value;
            else
                m_pEpoch.InvadersCount = RacesCount.Value;

            RacesCountNumber.Text = RacesCount.Value.ToString();

            BaseTechBox.Enabled = RacesCount.Value > 0;
            MaxTechBox.Enabled = RacesCount.Value > 0;
            BaseMagicBox.Enabled = RacesCount.Value > 0;
            MaxMagicBox.Enabled = RacesCount.Value > 0;
            checkedListBox1.Enabled = RacesCount.Value > 0;

            FireUpdate();
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (m_pEpoch == null || m_bInitMode)
                return;

            List<Race> cRaces = new List<Race>();

            foreach (Race pRace in checkedListBox1.CheckedItems)
                if(e.Index != checkedListBox1.Items.IndexOf(pRace) || e.NewValue == CheckState.Checked)
                    cRaces.Add(pRace);

            if (tabControl1.SelectedIndex == 0)
                m_pEpoch.NativesRaces = cRaces;
            else
                m_pEpoch.InvadersRaces = cRaces;

            FireUpdate();
        }


        public delegate void UpdateEventHandler(object sender, EventArgs e);
        public event UpdateEventHandler UpdateEvent = null;
    
        public void FireUpdate()
        {
            if (UpdateEvent != null)
                UpdateEvent(this, new EventArgs());
        }
    }
}
