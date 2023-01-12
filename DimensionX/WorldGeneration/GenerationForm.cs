using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Random;
using LandscapeGeneration;
using Microsoft.Win32;
using System.IO;
using Socium;

namespace WorldGeneration
{
    public partial class GenerationForm : Form
    {
        public class Settings
        {
            public List<string> m_cLastUsedPresets = new List<string>();
        }

        public Settings m_pSettings = null;

        private World m_pWorld = null;

        public World World
        {
            get { return m_pWorld; }
        }

        public GenerationForm()
        {
            InitializeComponent();
        
            m_pWorld = null;

            StartGenerationButton.Enabled = false;
            groupBox5.Enabled = false;
            //groupBox6.Enabled = false;
            mapProperties1.Enabled = false;
        }

        private void StartGeneration_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            StartGenerationButton.Enabled = false;
            groupBox5.Enabled = false;
            mapProperties1.Enabled = false;

            StartGenerationButton.Text = "... Please wait ...";

            Refresh();

            GenerateWorld(this);

            Cursor = Cursors.Arrow;

            StartGenerationButton.Enabled = true;
            groupBox5.Enabled = true;
            mapProperties1.Enabled = true;

            StartGenerationButton.Text = "Start generation!"; 

            DialogResult = DialogResult.OK;
        }

        public void GenerateWorld(IWin32Window pOwnerWindow)
        {
            List<Epoch> cEpoches = new List<Epoch>();

            foreach (ListViewItem pItem in AgesView.Items)
                cEpoches.Add((pItem.Tag as EpochWrapper).GetEpochInfo());

            WaitForm.StartWait(pOwnerWindow, "World generation...");

            m_pWorld = new World(800,
                5,
                mapProperties1.ContinentsCount,
                !mapProperties1.PartialMap,
                mapProperties1.LandsDiversity,
                Math.Max(10, Math.Min(mapProperties1.StatesCount * 3, 300)),
                mapProperties1.StatesCount,
                mapProperties1.LandMassesDiversity,
                mapProperties1.WaterPercent,
                mapProperties1.EquatorPosition,
                mapProperties1.PoleDistance,
                cEpoches.ToArray(),
                WaitForm.BeginStep,
                WaitForm.ProgressStep);
            WaitForm.CloseWait();
        }

        private void GenerationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !groupBox5.Enabled;
        }

        public bool Preload(Settings settings)
        {
            m_pSettings = settings;

            foreach (string sPrset in settings.m_cLastUsedPresets.ToArray())
                if (!File.Exists(sPrset))
                    settings.m_cLastUsedPresets.Remove(sPrset);

            groupBox5.Enabled = true;
            mapProperties1.Enabled = true;
            StartGenerationButton.Enabled = true;
            mapProperties1.UpdatePresets(25000, 0);
            
            if (AgesView.Items.Count == 0)
                AddNewAge_Click(this, new EventArgs());

            return true;
        }

        private void AgesView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AgesView.SelectedIndices.Count > 0)
            {
                epochProperties1.Epoch = AgesView.SelectedItems[0].Tag as EpochWrapper;
                button2.Enabled = AgesView.Items.Count > 1;
                button3.Enabled = AgesView.SelectedItems[0].Index > 0;
                button4.Enabled = AgesView.SelectedItems[0].Index < AgesView.Items.Count - 1;
            }
        }

        private void epochProperties1_UpdateEvent(object sender, EventArgs e)
        {
            if (AgesView.SelectedIndices.Count > 0)
                if (epochProperties1.Epoch == AgesView.SelectedItems[0].Tag as EpochWrapper)
                {
                    ListViewItem pItem = AgesView.SelectedItems[0];
                    EpochWrapper pEpoch = epochProperties1.Epoch;
                    pItem.SubItems[0].Text = pEpoch.Name;
                    pItem.SubItems[1].Text = pEpoch.NativesPresetString;
                    pItem.SubItems[2].Text = pEpoch.NativesRacesString;
                    pItem.SubItems[3].Text = pEpoch.Length.ToString();
                }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            mapProperties1.AdvancedMode = checkBox1.Checked;
            epochProperties1.AdvancedMode = checkBox1.Checked;
        }

        private void AddNewAge_Click(object sender, EventArgs e)
        {
            EpochWrapper pEpoch = new EpochWrapper();

            pEpoch.NativesPreset = ProgressPreset.s_aSocietyPresets[1];
            pEpoch.InvadersPreset = ProgressPreset.s_aSocietyPresets[ProgressPreset.s_aSocietyPresets.Length-1];

            pEpoch.NativesRacesSets = new RacesSet[] { RacesSet.s_aSets[0], RacesSet.s_aSets[1], RacesSet.s_aSets[2], RacesSet.s_aSets[3] };
            pEpoch.InvadersRacesSets = new RacesSet[] { RacesSet.s_aSets[RacesSet.s_aSets.Length - 2], RacesSet.s_aSets[RacesSet.s_aSets.Length - 1] };

            ListViewItem pItem = AgesView.Items.Add(pEpoch.Name);
            pItem.SubItems.Add(pEpoch.NativesPresetString);
            pItem.SubItems.Add(pEpoch.NativesRacesString);
            pItem.SubItems.Add(pEpoch.Length.ToString());

            pItem.Tag = pEpoch;

            pItem.Selected = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (AgesView.SelectedIndices.Count > 0)
                if (epochProperties1.Epoch == AgesView.SelectedItems[0].Tag as EpochWrapper)
                {
                    int iIndex = AgesView.SelectedItems[0].Index;
                    AgesView.Items.Remove(AgesView.SelectedItems[0]);
                    if (AgesView.Items.Count < iIndex)
                        AgesView.Items[iIndex].Selected = true;
                    else
                        AgesView.Items[iIndex-1].Selected = true;

                    button2.Enabled = AgesView.Items.Count > 1;
                    button3.Enabled = AgesView.SelectedItems[0].Index > 0;
                    button4.Enabled = AgesView.SelectedItems[0].Index < AgesView.Items.Count - 1;
                }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (AgesView.SelectedIndices.Count > 0)
                if (epochProperties1.Epoch == AgesView.SelectedItems[0].Tag as EpochWrapper)
                {
                    ListViewItem pItem = AgesView.SelectedItems[0];
                    int iIndex = pItem.Index;
                    AgesView.Items.Remove(pItem);
                    AgesView.Items.Insert(iIndex-1, pItem);
                    pItem.Selected = true;

                    button2.Enabled = AgesView.Items.Count > 1;
                    button3.Enabled = AgesView.SelectedItems[0].Index > 0;
                    button4.Enabled = AgesView.SelectedItems[0].Index < AgesView.Items.Count - 1;
                }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (AgesView.SelectedIndices.Count > 0)
                if (epochProperties1.Epoch == AgesView.SelectedItems[0].Tag as EpochWrapper)
                {
                    ListViewItem pItem = AgesView.SelectedItems[0];
                    int iIndex = pItem.Index;
                    AgesView.Items.Remove(pItem);
                    AgesView.Items.Insert(iIndex + 1, pItem);
                    pItem.Selected = true;

                    button2.Enabled = AgesView.Items.Count > 1;
                    button3.Enabled = AgesView.SelectedItems[0].Index > 0;
                    button4.Enabled = AgesView.SelectedItems[0].Index < AgesView.Items.Count - 1;
                }
        }

        private void SaveWorldPreset_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                List<Epoch> cEpoches = new List<Epoch>();

                foreach (ListViewItem pItem in AgesView.Items)
                    cEpoches.Add((pItem.Tag as EpochWrapper).GetEpochInfo());

                WorldPreset pWorldPreset = new WorldPreset(mapProperties1.ContinentsCount,
                    !mapProperties1.PartialMap,
                    mapProperties1.LandsDiversity,
                    mapProperties1.StatesCount,
                    mapProperties1.LandMassesDiversity,
                    mapProperties1.WaterPercent,
                    mapProperties1.EquatorPosition,
                    mapProperties1.PoleDistance,
                    cEpoches.ToArray());

                pWorldPreset.Save(saveFileDialog1.FileName);

                if (m_pSettings.m_cLastUsedPresets.Contains(saveFileDialog1.FileName))
                    m_pSettings.m_cLastUsedPresets.Remove(saveFileDialog1.FileName);

                m_pSettings.m_cLastUsedPresets.Insert(0, saveFileDialog1.FileName);

                while (m_pSettings.m_cLastUsedPresets.Count > 5)
                    m_pSettings.m_cLastUsedPresets.RemoveAt(5);
            }
        }

        private void LoadWorldPreset_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                LoadPreset(openFileDialog1.FileName);
        }

        public void LoadPreset(string sFileName)
        {
            WorldPreset pWorldPreset = new WorldPreset(sFileName);

            if (m_pSettings.m_cLastUsedPresets.Contains(sFileName))
                m_pSettings.m_cLastUsedPresets.Remove(sFileName);

            m_pSettings.m_cLastUsedPresets.Insert(0, sFileName);

            while (m_pSettings.m_cLastUsedPresets.Count > 5)
                m_pSettings.m_cLastUsedPresets.RemoveAt(5);

            checkBox1.Checked = true;

            mapProperties1.LandsDiversity = pWorldPreset.m_iLandsDiversity;
            mapProperties1.LandMassesDiversity = pWorldPreset.m_iLandMassesDiversity;
            mapProperties1.ContinentsCount = pWorldPreset.m_iContinents;
            mapProperties1.WaterPercent = pWorldPreset.m_iOcean;

            mapProperties1.PartialMap = !pWorldPreset.m_bGreatOcean;
            mapProperties1.EquatorPosition = pWorldPreset.m_iEquator;
            mapProperties1.PoleDistance = pWorldPreset.m_iPole;

            mapProperties1.StatesCount = pWorldPreset.m_iStates;

            AgesView.Items.Clear();

            foreach (Epoch pEpoch in pWorldPreset.m_aEpoches)
            {
                EpochWrapper pEpochInfo = new EpochWrapper(pEpoch);

                ListViewItem pItem = AgesView.Items.Add(pEpochInfo.Name);
                pItem.SubItems.Add(pEpochInfo.NativesPresetString);
                pItem.SubItems.Add(pEpochInfo.NativesRacesString);
                pItem.SubItems.Add(pEpochInfo.Length.ToString());

                pItem.Tag = pEpochInfo;
            }

            if (AgesView.Items.Count > 0)
                AgesView.Items[0].Selected = true;
        }
    }
}
