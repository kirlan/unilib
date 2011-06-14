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

        private LocationsGrid<LocationX> m_cLocations = null;
        private LocationsGrid<LocationX> m_cLastUsedLocations = null;
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

            if (m_cLastUsedLocations == m_cLocations)
                m_cLastUsedLocations.Reset();
            else
            {
                if (m_cLastUsedLocations != null)
                    m_cLastUsedLocations.Unload();
                m_cLastUsedLocations = m_cLocations;
            }

            List<Epoch> cEpoches = new List<Epoch>();

            foreach (ListViewItem pItem in AgesView.Items)
                cEpoches.Add((pItem.Tag as EpochWrapper).GetEpochInfo());

            m_pWorld = new World(m_cLastUsedLocations,
                mapProperties1.ContinentsCount,
                !mapProperties1.PartialMap,
                mapProperties1.LandsCount,
                Math.Max(10, Math.Min((int)mapProperties1.StatesCount * 7, 300)),
                mapProperties1.StatesCount,
                mapProperties1.LandMassesCount,
                mapProperties1.WaterPercent,
                mapProperties1.EquatorPosition,
                mapProperties1.PoleDistance,
                cEpoches.ToArray());
            
            Cursor = Cursors.Arrow;

            StartGenerationButton.Enabled = true;
            groupBox5.Enabled = true;
            groupBox6.Enabled = true;
            mapProperties1.Enabled = true;

            StartGenerationButton.Text = "Start generation!"; 

            DialogResult = DialogResult.OK;
        }

        private void GenerationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !groupBox5.Enabled;
        }

        private void GridsManagerButton_Click(object sender, EventArgs e)
        {
            GridBuildingForm pForm = new GridBuildingForm(m_sWorkingDir);

            if (pForm.ShowDialog() == DialogResult.OK)
            {
                StartGenerationButton.Enabled = true;
                groupBox5.Enabled = true;
                groupBox6.Enabled = true;
                mapProperties1.Enabled = true;

                GridsComboBox.Items.Add(pForm.m_cLocations);
                GridsComboBox.SelectedItem = pForm.m_cLocations;
            }
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

            dirInfo = new DirectoryInfo(m_sWorkingDir); 
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

            return true;
        }

        private void GridsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GridsComboBox.SelectedIndex == -1)
                return;

            m_cLocations = (LocationsGrid<LocationX>)GridsComboBox.SelectedItem;

            groupBox5.Enabled = true;
            groupBox6.Enabled = true;
            mapProperties1.Enabled = true;
            StartGenerationButton.Enabled = true;

            mapProperties1.LocationsGrid = m_cLocations;
            //CalculateLimits(m_cLocations.m_iLocationsCount);

            if (AgesView.Items.Count == 0)
            {
                EpochWrapper pEpoch = new EpochWrapper();
                ListViewItem pItem = AgesView.Items.Add(pEpoch.Name);
                pItem.SubItems.Add(pEpoch.NativesPresetString);
                pItem.SubItems.Add(pEpoch.NativesRacesString);
                pItem.SubItems.Add(pEpoch.Length.ToString());

                pItem.Tag = pEpoch;

                pItem.Selected = true;
            }
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

        private void button1_Click(object sender, EventArgs e)
        {
            EpochWrapper pEpoch = new EpochWrapper();
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
    }
}
