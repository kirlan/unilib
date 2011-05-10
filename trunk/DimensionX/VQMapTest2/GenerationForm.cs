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
        private World m_pWorld = null;

        public World World
        {
            get { return m_pWorld; }
        }

        public GenerationForm()
        {
            InitializeComponent();
        
            m_pWorld = null;

            groupBox2.Enabled = false;
            groupBox3.Enabled = false;
            groupBox4.Enabled = false;
            tableLayoutPanel2.Enabled = false;
            button8.Enabled = false;
        }

        private void RndEquator_Click(object sender, EventArgs e)
        {
            Equator.Value = Rnd.Get(200) - 50;
        }

        private void RndPole_Click(object sender, EventArgs e)
        {
            Pole.Value = Math.Max(100 - Equator.Value, Equator.Value);
            if (Rnd.OneChanceFrom(2))
                Pole.Value = Pole.Value + Rnd.Get((int)(Pole.Value / 2));
        }

        private void RndWater_Click(object sender, EventArgs e)
        {
            WaterPercent.Value = WaterPercent.Minimum + Rnd.Get((int)WaterPercent.Maximum - (int)WaterPercent.Minimum);
        }

        private void RndRaces_Click(object sender, EventArgs e)
        {
            RacesCount.Value = RacesCount.Minimum + Rnd.Get((int)RacesCount.Maximum - (int)RacesCount.Minimum);
        }

        private void RndStates_Click(object sender, EventArgs e)
        {
            StatesCount.Value = StatesCount.Minimum + Rnd.Get((int)StatesCount.Maximum - (int)StatesCount.Minimum);
        }

        private void RndProvincies_Click(object sender, EventArgs e)
        {
            ProvinciesCount.Value = ProvinciesCount.Minimum + Rnd.Get((int)ProvinciesCount.Maximum - (int)ProvinciesCount.Minimum);
        }

        private void RndContinents_Click(object sender, EventArgs e)
        {
            ContinentsCount.Value = ContinentsCount.Minimum + Rnd.Get((int)ContinentsCount.Maximum - (int)ContinentsCount.Minimum);
        }

        private void RndAll_Click(object sender, EventArgs e)
        {
            RndEquator_Click(sender, e);
            RndPole_Click(sender, e);
            RndWater_Click(sender, e);
            RndRaces_Click(sender, e);
            RndStates_Click(sender, e);
            //RndProvincies_Click(sender, e);
            RndContinents_Click(sender, e);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            groupBox1.Enabled = false;
            groupBox2.Enabled = false;
            groupBox3.Enabled = false;
            groupBox4.Enabled = false;

            tableLayoutPanel2.Enabled = false;
            button8.Enabled = false;

            button8.Text = "... Please wait ...";

            Refresh();

            m_cLocations.Reset();
            m_pWorld = new World(m_cLocations, (int)ContinentsCount.Value, !PartialMap.Checked, (int)LandsCount.Value, (int)ProvinciesCount.Value, (int)StatesCount.Value, (int)LandMassesCount.Value, (int)WaterPercent.Value, (int)Equator.Value, (int)Pole.Value, (int)RacesCount.Value);
            
            Cursor = Cursors.Arrow;
            groupBox1.Enabled = true;
            groupBox2.Enabled = true;
            groupBox3.Enabled = true;
            groupBox4.Enabled = true;

            tableLayoutPanel2.Enabled = true;
            button8.Enabled = true;

            button8.Text = "Start generation!"; 

            DialogResult = DialogResult.OK;
        }

        private void GenerationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !groupBox1.Enabled;
        }

        private void CalculateLimits(int iLocationsCount)
        {
            if (LandsCount.Minimum > Math.Min(600, iLocationsCount / 4))
                LandsCount.Minimum = Math.Min(600, iLocationsCount / 4);

            if (LandsCount.Maximum < iLocationsCount / 2)
                LandsCount.Maximum = iLocationsCount / 2;

            if (LandsCount.Value > iLocationsCount / 2)
                LandsCount.Value = iLocationsCount / 2;

            if (LandsCount.Value < Math.Min(600, iLocationsCount / 4))
                LandsCount.Value = Math.Min(600, iLocationsCount / 4);

            LandsCount.Maximum = iLocationsCount / 2 + LandsCount.LargeChange - 1;
            LandsCount.Minimum = Math.Min(600, iLocationsCount / 4);
            if (LandsCount.Maximum < 6000)
                LandsCount.Value = LandsCount.Maximum;
            else
                if (LandsCount.Minimum > 6000)
                    LandsCount.Value = LandsCount.Minimum;
                else
                    LandsCount.Value = 6000;

            if (LandMassesCount.Minimum > Math.Min(30, iLocationsCount / 20))
                LandMassesCount.Minimum = Math.Min(30, iLocationsCount / 20);

            if (LandMassesCount.Maximum < Math.Min(300, iLocationsCount / 4))
                LandMassesCount.Maximum = Math.Min(300, iLocationsCount / 4);

            if (LandMassesCount.Value > Math.Min(300, iLocationsCount / 4))
                LandMassesCount.Value = Math.Min(300, iLocationsCount / 4);

            if (LandMassesCount.Value < Math.Min(30, iLocationsCount / 20))
                LandMassesCount.Value = Math.Min(30, iLocationsCount / 20);

            LandMassesCount.Maximum = Math.Min(300, iLocationsCount / 4) + LandMassesCount.LargeChange - 1;
            LandMassesCount.Minimum = Math.Min(30, iLocationsCount / 20);
            if (LandMassesCount.Maximum < 90)
                LandMassesCount.Value = LandMassesCount.Maximum;
            else
                if (LandMassesCount.Minimum > 90)
                    LandMassesCount.Value = LandMassesCount.Minimum;
                else
                    LandMassesCount.Value = 90;

            int iNotOcean = iLocationsCount * (100 - (int)WaterPercent.Value) / 100;

            if (RacesCount.Minimum > Math.Min(6, iNotOcean / 20))
                RacesCount.Minimum = Math.Min(6, iNotOcean / 20);

            if (RacesCount.Maximum < Math.Min(40, iNotOcean / 10))
                RacesCount.Maximum = Math.Min(40, iNotOcean / 10);

            if (RacesCount.Value > Math.Min(40, iNotOcean / 10))
                RacesCount.Value = Math.Min(40, iNotOcean / 10);

            if (RacesCount.Value < Math.Min(6, iNotOcean / 20))
                RacesCount.Value = Math.Min(6, iNotOcean / 20);

            RacesCount.Maximum = Math.Min(40, iNotOcean / 10) + RacesCount.LargeChange - 1;
            RacesCount.Minimum = Math.Min(6, iNotOcean / 20);

            if (ProvinciesCount.Minimum > Math.Min(100, iNotOcean / 20))
                ProvinciesCount.Minimum = Math.Min(100, iNotOcean / 20);

            if (ProvinciesCount.Maximum < Math.Min(500, iNotOcean / 10))
                ProvinciesCount.Maximum = Math.Min(500, iNotOcean / 10);

            if (ProvinciesCount.Value > Math.Min(500, iNotOcean / 10))
                ProvinciesCount.Value = Math.Min(500, iNotOcean / 10);

            if (ProvinciesCount.Value < Math.Min(100, iNotOcean / 20))
                ProvinciesCount.Value = Math.Min(100, iNotOcean / 20);

            ProvinciesCount.Maximum = Math.Min(500, iNotOcean / 10) + ProvinciesCount.LargeChange - 1;
            ProvinciesCount.Minimum = Math.Min(100, iNotOcean / 20);
            if (ProvinciesCount.Maximum < 250)
                ProvinciesCount.Value = ProvinciesCount.Maximum;
            else
                if (ProvinciesCount.Minimum > 250)
                    ProvinciesCount.Value = ProvinciesCount.Minimum;
                else
                    ProvinciesCount.Value = 250;

            if (StatesCount.Minimum > Math.Min(5, iNotOcean / 40))
                StatesCount.Minimum = Math.Min(5, iNotOcean / 40);

            if (StatesCount.Maximum < Math.Min(100, iNotOcean / 20))
                StatesCount.Maximum = Math.Min(100, iNotOcean / 20);

            if (StatesCount.Value > Math.Min(100, iNotOcean / 20))
                StatesCount.Value = Math.Min(100, iNotOcean / 20);

            if (StatesCount.Value < Math.Min(5, iNotOcean / 40))
                StatesCount.Value = Math.Min(5, iNotOcean / 40);

            StatesCount.Maximum = Math.Min(100, iNotOcean / 20) + StatesCount.LargeChange-1;
            StatesCount.Minimum = Math.Min(5, iNotOcean / 40);
            if (StatesCount.Maximum < 100)
                StatesCount.Value = StatesCount.Maximum;
            else
                if (StatesCount.Minimum > 100)
                    StatesCount.Value = StatesCount.Minimum;
                else
                    StatesCount.Value = 100;
        }

        private void PartialMap_CheckedChanged(object sender, EventArgs e)
        {
            //if (PartialMap.Checked)
            //{
            //    if (LandMassesCount.Value / 2 < LandMassesCount.Minimum)
            //        LandMassesCount.Value = LandMassesCount.Minimum;
            //    else
            //        LandMassesCount.Value /= 2;
            //}
            //else
            //{
            //    if (LandMassesCount.Value * 2 > LandMassesCount.Maximum)
            //        LandMassesCount.Value = LandMassesCount.Maximum;
            //    else
            //        LandMassesCount.Value *= 2;
            //}
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ContinentsCount.Value = 5;
            Equator.Value = 50;
            Pole.Value = 45;
            WaterPercent.Value = 66;

            if (RacesCount.Maximum > 24)
                RacesCount.Value = 24;
            else
                RacesCount.Value = RacesCount.Maximum;

            if (StatesCount.Maximum > 20)
                StatesCount.Value = 20;
            else
                StatesCount.Value = StatesCount.Maximum;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            GridBuildingForm pForm = new GridBuildingForm(m_sWorkingDir);

            if (pForm.ShowDialog() == DialogResult.OK)
            {
                m_cLocations = pForm.m_cLocations;
            
                groupBox2.Enabled = true;
                groupBox3.Enabled = true;
                groupBox4.Enabled = true;
                tableLayoutPanel2.Enabled = true;
                button8.Enabled = true;

                comboBox1.Items.Add(m_cLocations);
                comboBox1.SelectedItem = m_cLocations;

                //CalculateLimits(m_cLocations.m_iLocationsCount);
            }
        }

        //private void button9_Click(object sender, EventArgs e)
        //{
        //    if (openFileDialog1.ShowDialog() == DialogResult.OK)
        //    {
        //        Cursor = Cursors.WaitCursor;
        //        groupBox1.Enabled = false;
        //        groupBox2.Enabled = false;
        //        groupBox3.Enabled = false;
        //        groupBox4.Enabled = false;

        //        tableLayoutPanel2.Enabled = false;
        //        button8.Enabled = false; 
        //        Refresh();

        //        m_cLocations = new LocationsGrid<LocationX>(openFileDialog1.FileName);
            
        //        label1.Text = m_cLocations.m_sDescription;

        //        CalculateLimits(m_cLocations.m_iLocationsCount);

        //        Cursor = Cursors.Arrow;
        //        groupBox1.Enabled = true;
        //        groupBox2.Enabled = true;
        //        groupBox3.Enabled = true;
        //        groupBox4.Enabled = true;

        //        tableLayoutPanel2.Enabled = true;
        //        button8.Enabled = true;
        //    }
        //}

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

            FileInfo[] fileNames = dirInfo.GetFiles("*.*");

            foreach (FileInfo fi in fileNames)
            {
                string sDescription = "";
                if (fi.Extension == ".dxg" && LocationsGrid<LocationX>.CheckFile(fi.FullName, out sDescription))
                {
                    comboBox1.Items.Add(new LocationsGrid<LocationX>(fi.FullName));
                }
            }

            if (comboBox1.Items.Count > 0)
                comboBox1.SelectedIndex = 0;

            return true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1)
                return;

            m_cLocations = (LocationsGrid<LocationX>)comboBox1.SelectedItem;

            groupBox2.Enabled = true;
            groupBox3.Enabled = true;
            groupBox4.Enabled = true;
            tableLayoutPanel2.Enabled = true;
            button8.Enabled = true;

            CalculateLimits(m_cLocations.m_iLocationsCount);
        }

        private void hScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(sender as HScrollBar, (sender as HScrollBar).Value.ToString());
        }
    }
}
