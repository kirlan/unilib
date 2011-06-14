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

namespace VQMapTest2
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
        }

        private void StartGeneration_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            StartGenerationButton.Enabled = false;

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

            //m_pWorld = new World(m_cLastUsedLocations, 
            //    (int)ContinentsCount.Value, 
            //    !PartialMap.Checked, 
            //    (int)LandsCount.Value,
            //    Math.Max(10, Math.Min((int)StatesCount.Value * 7, 300)), 
            //    (int)StatesCount.Value,
            //    (int)LandMassesCount.Value, 
            //    (int)WaterPercent.Value, 
            //    (int)Equator.Value, 
            //    (int)Pole.Value, 
            //    (int)RacesCount.Value, 
            //    comboBox2.SelectedIndex, 
            //    m_iTechLevelLimit, 
            //    comboBox4.SelectedIndex, 
            //    m_iMagicLimit, 
            //    cGroups.ToArray(),
            //    comboBox16.SelectedIndex,
            //    m_iAncientTechLevelLimit,
            //    comboBox14.SelectedIndex,
            //    m_iAncientMagicLimit,
            //    cInvadersGroups.ToArray(),
            //    iInvasionProb, 
            //    m_iInvadersTechLevelLimit, 
            //    m_iInvadersMagicLimit);
            
            Cursor = Cursors.Arrow;

            StartGenerationButton.Enabled = true;

            StartGenerationButton.Text = "Start generation!"; 

            DialogResult = DialogResult.OK;
        }

        private void GenerationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !StartGenerationButton.Enabled;
        }

        private void GridsManagerButton_Click(object sender, EventArgs e)
        {
            GridBuildingForm pForm = new GridBuildingForm(m_sWorkingDir);

            if (pForm.ShowDialog() == DialogResult.OK)
            {
                StartGenerationButton.Enabled = true;

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

            StartGenerationButton.Enabled = true;

            //CalculateLimits(m_cLocations.m_iLocationsCount);
        }
    }
}
