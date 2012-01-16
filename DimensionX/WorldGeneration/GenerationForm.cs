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
        private string m_sWorkingDir = "";

        private LocationsGrid<LocationX> m_cLocationsGrid = null;
        private LocationsGrid<LocationX> m_cLastUsedGrid = null;
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
            groupBox6.Enabled = false;
            mapProperties1.Enabled = false;
        }

        private void StartGeneration_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            StartGenerationButton.Enabled = false;
            groupBox5.Enabled = false;
            groupBox6.Enabled = false;
            mapProperties1.Enabled = false;

            StartGenerationButton.Text = "... Please wait ...";

            Refresh();

            GenerateWorld(this);

            Cursor = Cursors.Arrow;

            StartGenerationButton.Enabled = true;
            groupBox5.Enabled = true;
            groupBox6.Enabled = true;
            mapProperties1.Enabled = true;

            StartGenerationButton.Text = "Start generation!"; 

            DialogResult = DialogResult.OK;
        }

        public void GenerateWorld(IWin32Window pOwnerWindow)
        {
            if (m_cLastUsedGrid == m_cLocationsGrid)
                m_cLastUsedGrid.Reset();
            else
            {
                if (m_cLastUsedGrid != null)
                    m_cLastUsedGrid.Unload();
                m_cLastUsedGrid = m_cLocationsGrid;
            }

            List<Epoch> cEpoches = new List<Epoch>();

            foreach (ListViewItem pItem in AgesView.Items)
                cEpoches.Add((pItem.Tag as EpochWrapper).GetEpochInfo());

            WaitForm.StartWait(pOwnerWindow);

            m_pWorld = new World(m_cLastUsedGrid,
                mapProperties1.ContinentsCount,
                !mapProperties1.PartialMap,
                mapProperties1.LandsCount,
                Math.Max(10, Math.Min((int)mapProperties1.StatesCount * 7, 300)),
                mapProperties1.StatesCount,
                mapProperties1.LandMassesCount,
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

        private void GridsManagerButton_Click(object sender, EventArgs e)
        {
            GridsManager pForm = new GridsManager(m_sWorkingDir);

            pForm.ShowDialog();
            ScanWorkingDir();

            //GridBuildingForm pForm = new GridBuildingForm(m_sWorkingDir);

            //if (pForm.ShowDialog() == DialogResult.OK)
            //{
            //    StartGenerationButton.Enabled = true;
            //    groupBox5.Enabled = true;
            //    groupBox6.Enabled = true;
            //    mapProperties1.Enabled = true;

            //    GridsComboBox.Items.Add(pForm.m_pLocationsGrid);
            //    GridsComboBox.SelectedItem = pForm.m_pLocationsGrid;
            //}
        }

        private bool GetNewWorkingDir(RegistryKey key)
        {
            folderBrowserDialog1.SelectedPath = Application.CommonAppDataPath;
            if (folderBrowserDialog1.ShowDialog() != DialogResult.OK)
                return false;

            m_sWorkingDir = folderBrowserDialog1.SelectedPath;
            key.SetValue("WorkingPath", m_sWorkingDir);

            return true;
        }

        public bool Preload()
        {
            RegistryKey key;
            key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\DimensionX", true);
            if (key == null)
                key = Registry.LocalMachine.CreateSubKey("SOFTWARE\\DimensionX");

            string sPreset = (string)key.GetValue("preset1", "");
            if (sPreset != "" && !m_cLastUsedPresets.Contains(sPreset))
                m_cLastUsedPresets.Add(sPreset);

            sPreset = (string)key.GetValue("preset2", "");
            if (sPreset != "" && !m_cLastUsedPresets.Contains(sPreset))
                m_cLastUsedPresets.Add(sPreset);

            sPreset = (string)key.GetValue("preset3", "");
            if (sPreset != "" && !m_cLastUsedPresets.Contains(sPreset))
                m_cLastUsedPresets.Add(sPreset);

            sPreset = (string)key.GetValue("preset4", "");
            if (sPreset != "" && !m_cLastUsedPresets.Contains(sPreset))
                m_cLastUsedPresets.Add(sPreset);

            sPreset = (string)key.GetValue("preset5", "");
            if (sPreset != "" && !m_cLastUsedPresets.Contains(sPreset))
                m_cLastUsedPresets.Add(sPreset);

            foreach (string sPrset in m_cLastUsedPresets.ToArray())
                if (!File.Exists(sPrset))
                    m_cLastUsedPresets.Remove(sPrset);

            m_sWorkingDir = (string)key.GetValue("WorkingPath", "");

            if (m_sWorkingDir == "" && !GetNewWorkingDir(key))
            {
                key.Close();
                return false;
            }

            DirectoryInfo dirInfo = new DirectoryInfo(m_sWorkingDir);
            if(!dirInfo.Exists && !GetNewWorkingDir(key))
            {
                key.Close();
                return false;
            }

            key.Close();

            ScanWorkingDir();

            return true;
        }

        private void ScanWorkingDir()
        {
            GridsComboBox.Items.Clear();

            DirectoryInfo dirInfo = new DirectoryInfo(m_sWorkingDir);
            FileInfo[] fileNames = dirInfo.GetFiles("*.*");

            foreach (FileInfo fi in fileNames)
            {
                if (fi.Extension == ".dxg")
                {
                    try
                    {
                        LocationsGrid<LocationX> pGrid = new LocationsGrid<LocationX>(fi.FullName);
                        GridsComboBox.Items.Add(pGrid);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }

            if (GridsComboBox.Items.Count > 0)
                GridsComboBox.SelectedIndex = 0;
        }

        private void GridsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GridsComboBox.SelectedIndex == -1)
                return;

            m_cLocationsGrid = (LocationsGrid<LocationX>)GridsComboBox.SelectedItem;

            groupBox5.Enabled = true;
            groupBox6.Enabled = true;
            mapProperties1.Enabled = true;
            StartGenerationButton.Enabled = true;

            mapProperties1.LocationsGrid = m_cLocationsGrid;
            //CalculateLimits(m_cLocations.m_iLocationsCount);

            if (AgesView.Items.Count == 0)
                AddNewAge_Click(sender, e);
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
                if (m_cLastUsedGrid == m_cLocationsGrid)
                    m_cLastUsedGrid.Reset();
                else
                {
                    if (m_cLastUsedGrid != null)
                        m_cLastUsedGrid.Unload();
                    m_cLastUsedGrid = m_cLocationsGrid;
                }

                List<Epoch> cEpoches = new List<Epoch>();

                foreach (ListViewItem pItem in AgesView.Items)
                    cEpoches.Add((pItem.Tag as EpochWrapper).GetEpochInfo());

                WorldPreset pWorldPreset = new WorldPreset(m_cLastUsedGrid,
                    mapProperties1.ContinentsCount,
                    !mapProperties1.PartialMap,
                    mapProperties1.LandsCount,
                    mapProperties1.StatesCount,
                    mapProperties1.LandMassesCount,
                    mapProperties1.WaterPercent,
                    mapProperties1.EquatorPosition,
                    mapProperties1.PoleDistance,
                    cEpoches.ToArray());

                pWorldPreset.Save(saveFileDialog1.FileName);

                if (m_cLastUsedPresets.Contains(saveFileDialog1.FileName))
                    m_cLastUsedPresets.Remove(saveFileDialog1.FileName);

                m_cLastUsedPresets.Insert(0, saveFileDialog1.FileName);

                while (m_cLastUsedPresets.Count > 5)
                    m_cLastUsedPresets.RemoveAt(5);

                RegistryKey key;
                key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\DimensionX", true);
                if (key != null)
                {
                    for (int i = 1; i <= m_cLastUsedPresets.Count; i++)
                    {
                        key.SetValue("preset" + i.ToString(), m_cLastUsedPresets[i - 1]);
                    }
                    key.Close();
                }
            }
        }

        public List<string> m_cLastUsedPresets = new List<string>();

        private void LoadWorldPreset_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                LoadPreset(openFileDialog1.FileName);
        }

        public void LoadPreset(string sFileName)
        {
            WorldPreset pWorldPreset = new WorldPreset(sFileName, GridsComboBox.Items);
            if (pWorldPreset.m_pLocationsGrid != null)
                GridsComboBox.SelectedItem = pWorldPreset.m_pLocationsGrid;

            if (m_cLastUsedPresets.Contains(sFileName))
                m_cLastUsedPresets.Remove(sFileName);

            m_cLastUsedPresets.Insert(0, sFileName);

            while (m_cLastUsedPresets.Count > 5)
                m_cLastUsedPresets.RemoveAt(5);

            RegistryKey key;
            key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\DimensionX", true);
            if (key != null)
            {
                for (int i = 1; i <= m_cLastUsedPresets.Count; i++)
                {
                    key.SetValue("preset" + i.ToString(), m_cLastUsedPresets[i - 1]);
                }
                key.Close();
            }

            checkBox1.Checked = true;

            mapProperties1.LandsCount = pWorldPreset.m_iLands;
            mapProperties1.LandMassesCount = pWorldPreset.m_iLandMasses;
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
