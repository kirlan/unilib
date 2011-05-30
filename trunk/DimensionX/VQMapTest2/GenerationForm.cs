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
        MapPreset[] m_aWorldMaps = new MapPreset[] 
        {
            new MapPreset("Continents", "Earth-like world. 5 big continents ocuppies 33% of world surface. Polar regions are lands-free in this template.", true, true, 0, 90, 5, 66, 50, 45),
            new MapPreset("Continents 2", "Earth-like world. 5 big continents ocuppies 33% of world surface. There could be a continents in polar regions in this template.", true, false, 0, 45, 5, 66, 50, 50),
            new MapPreset("Gondwana", "One big continent ocuppies 33% of world surface. Polar regions are lands-free in this template.", true, true, 0, 90, 1, 66, 50, 45),
            new MapPreset("Archipelago", "About 30 big islands are evenly dispersed over the map and totally ocuppies 33% of world surface. Polar regions are lands-free in this template.", true, true, 0, 300, 30, 66, 50, 45),
        };

        MapPreset[] m_aPartialMaps = new MapPreset[] 
        {
            new MapPreset("Coast", "Traditional adventure map of big coastral region, like Europe, Middlearth, Hyperborea or Faerun. This is a part of a one big continent from arctic to tropics, with a long coastral line.", false, false, 15, 45, 1, 50, 95, 95),
            new MapPreset("Mediterranean", "Mediterranean-like region - there are parts of 3 big continents, divided by a sea. Continents extends from arctic to tropics.", false, false, 15, 45, 3, 50, 95, 95),
            new MapPreset("Atlantis", "One big continent in middle latitudes, surrounded by ocean.", false, true, 15, 45, 1, 50, 120, 130),
            new MapPreset("Tropical Paradise", "Archipelago of about 15 tropical islands.", false, true, 100, 300, 15, 90, 50, 100),
        };

        SocietyPreset[] m_aHistoricalSocietyPresets = new SocietyPreset[] 
        { 
            new SocietyPreset("Antiquity", "An antique world without magic - like Ancient Greece, Rome, Egypt, Assyria, etc.", 0, 1, 0, 0),
            new SocietyPreset("Medieval", "A medieval world without magic - castles, knights, tournaments, etc.", 0, 2, 0, 0),
            new SocietyPreset("Renaissance", "A renaissance world without magic - musketeers, geographic exploration, etc.", 1, 3, 0, 0),
            new SocietyPreset("Modern", "A modern world without magic - railroads, aviation, world wars, etc.", 4, 5, 0, 0),
        };

        SocietyPreset[] m_aFictionalSocietyPresets = new SocietyPreset[] 
        { 
            new SocietyPreset("Antique mythology", "A world of antique mythology - just a usual antique world, but with a bit of magic...", 0, 1, 1, 2),
            new SocietyPreset("Fantasy - low magic", "A medieval world with a bit of magic - like Knights of the Round Table, Lord of the Rings, etc.", 0, 2, 1, 3),
            new SocietyPreset("Fantasy - high magic", "A medieval world with a lot of magic - like Dragonlance, Wheel of Time, etc.", 0, 2, 2, 4),
            new SocietyPreset("Technomagic", "A renaissance world with a lot of magic - like Arcanum, Final Fantasy, etc.", 1, 3, 2, 4),
            new SocietyPreset("Superheroes", "A modern world with a bit of magic (aka supernatural abilities) - like Superman, Fantastic Four, Spiderman, etc.", 4, 5, 1, 3),
            new SocietyPreset("Urban fantasy", "A modern world with a lot of magic - vampires, werewolfs, voodoo, secret societies, etc.", 4, 5, 2, 4),
            new SocietyPreset("Cyberpunk", "Nearest future world without magic - advanced technologies, mega-corporations, industrial espionage, etc.", 4, 6, 0, 0),
            new SocietyPreset("Space opera", "Far future world with a bit of magic (aka psi-abilities) - like Star Wars, Star Trek, etc.", 6, 7, 1, 3),
        };

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

            groupBox2.Enabled = false;
            groupBox3.Enabled = false;
            groupBox4.Enabled = false;
            tableLayoutPanel2.Enabled = false;
            button8.Enabled = false;

            for (int i = 0; i < 9; i++)
            {
                comboBox2.Items.Add(string.Format("{0} [T{1}]", State.GetTechString(i), i));
                comboBox3.Items.Add(string.Format("{0} [T{1}]", State.GetTechString(i), i));

                comboBox4.Items.Add(string.Format("{0} [M{1}]", State.GetMagicString(i), i));
                comboBox5.Items.Add(string.Format("{0} [M{1}]", State.GetMagicString(i), i));
            }

            comboBox2.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;

            comboBox3.SelectedIndex = 8;
            comboBox5.SelectedIndex = 8;

            listBox1.Items.Clear();
            listBox1.Items.AddRange(m_aWorldMaps);
            if (listBox1.Items.Count > 0)
                listBox1.SelectedIndex = 0;

            listBox2.Items.Clear();
            listBox2.Items.AddRange(m_aPartialMaps);
            if (listBox2.Items.Count > 0)
                listBox2.SelectedIndex = 0;

            listBox3.Items.Clear();
            listBox3.Items.AddRange(m_aHistoricalSocietyPresets);
            if (listBox3.Items.Count > 0)
                listBox3.SelectedIndex = 0;

            listBox4.Items.Clear();
            listBox4.Items.AddRange(m_aFictionalSocietyPresets);
            if (listBox4.Items.Count > 0)
                listBox4.SelectedIndex = 0;

            comboBox10.SelectedIndex = 0;
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

        private void RndContinents_Click(object sender, EventArgs e)
        {
            ContinentsCount.Value = ContinentsCount.Minimum + Rnd.Get((int)ContinentsCount.Maximum - (int)ContinentsCount.Minimum);
        }

        private void RndAll_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 2)
            {
                RndEquator_Click(sender, e);
                RndPole_Click(sender, e);
                RndWater_Click(sender, e);
                RndContinents_Click(sender, e);
            }
            RndRaces_Click(sender, e);
            RndStates_Click(sender, e);
            //RndProvincies_Click(sender, e);
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

            if (m_cLastUsedLocations == m_cLocations)
                m_cLastUsedLocations.Reset();
            else
            {
                if (m_cLastUsedLocations != null)
                    m_cLastUsedLocations.Unload();
                m_cLastUsedLocations = m_cLocations;
            }

            int iInvasionProb = 0;
            switch (comboBox10.SelectedIndex)
            {
                case 1:
                    iInvasionProb = 1;
                    break;
                case 2:
                    iInvasionProb = 5;
                    break;
                case 3:
                    iInvasionProb = 20;
                    break;
                case 4:
                    iInvasionProb = 100;
                    break;
            }
            m_pWorld = new World(m_cLastUsedLocations, (int)ContinentsCount.Value, !PartialMap.Checked, (int)LandsCount.Value, Math.Max(10, Math.Min((int)StatesCount.Value * 3, 300)), (int)StatesCount.Value, (int)LandMassesCount.Value, (int)WaterPercent.Value, (int)Equator.Value, (int)Pole.Value, (int)RacesCount.Value, comboBox2.SelectedIndex, m_iTechLevelLimit, comboBox4.SelectedIndex, m_iMagicLimit, iInvasionProb, m_iInvadersTechLevelLimit, m_iInvadersMagicLimit);
            
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

            //LandsCount.Value = (LandsCount.Minimum + LandsCount.Maximum) / 2;
            //if (LandsCount.Maximum < 6000)
            //    LandsCount.Value = LandsCount.Maximum;
            //else
            //    if (LandsCount.Minimum > 6000)
            //        LandsCount.Value = LandsCount.Minimum;
            //    else
            //        LandsCount.Value = 6000;

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

            if (StatesCount.Minimum > Math.Min(1, iNotOcean / 40))
                StatesCount.Minimum = Math.Min(1, iNotOcean / 40);

            if (StatesCount.Maximum < Math.Min(100, iNotOcean / 20))
                StatesCount.Maximum = Math.Min(100, iNotOcean / 20);

            if (StatesCount.Value > Math.Min(100, iNotOcean / 20))
                StatesCount.Value = Math.Min(100, iNotOcean / 20);

            if (StatesCount.Value < Math.Min(1, iNotOcean / 40))
                StatesCount.Value = Math.Min(1, iNotOcean / 40);

            StatesCount.Maximum = Math.Min(100, iNotOcean / 20) + StatesCount.LargeChange-1;
            StatesCount.Minimum = Math.Min(1, iNotOcean / 40);
            //if (StatesCount.Maximum < 100)
            //    StatesCount.Value = StatesCount.Maximum;
            //else
            //    if (StatesCount.Minimum > 100)
            //        StatesCount.Value = StatesCount.Minimum;
            //    else
            //        StatesCount.Value = 100;
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
            if (tabControl1.SelectedIndex == 2)
            {
                ContinentsCount.Value = 5;
                Equator.Value = 50;
                Pole.Value = 45;
                WaterPercent.Value = 66;
            }

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
                groupBox2.Enabled = true;
                groupBox3.Enabled = true;
                groupBox4.Enabled = true;
                tableLayoutPanel2.Enabled = true;
                button8.Enabled = true;

                if (pForm.m_cLocations.m_bCycled)
                {
                    comboBox7.Items.Add(pForm.m_cLocations);
                    comboBox7.SelectedItem = pForm.m_cLocations;
                }
                else
                {
                    comboBox6.Items.Add(pForm.m_cLocations);
                    comboBox6.SelectedItem = pForm.m_cLocations;
                }

                comboBox1.Items.Add(pForm.m_cLocations);
                comboBox1.SelectedItem = pForm.m_cLocations;
                //m_cLocations = pForm.m_cLocations;

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

            dirInfo = new DirectoryInfo(m_sWorkingDir); 
            FileInfo[] fileNames = dirInfo.GetFiles("*.*");

            foreach (FileInfo fi in fileNames)
            {
                if (fi.Extension == ".dxg")
                {
                    try
                    {
                        LocationsGrid<LocationX> pGrid = new LocationsGrid<LocationX>(fi.FullName);
                        if (pGrid.m_bCycled)
                            comboBox7.Items.Add(pGrid);
                        else
                            comboBox6.Items.Add(pGrid);
                        comboBox1.Items.Add(pGrid);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }

            if (comboBox6.Items.Count > 0)
                comboBox6.SelectedIndex = 0;

            if (comboBox7.Items.Count > 0)
                comboBox7.SelectedIndex = 0;

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

            if (comboBox6.Items.Contains(comboBox1.SelectedItem))
                comboBox6.SelectedItem = comboBox1.SelectedItem;

            if (comboBox7.Items.Contains(comboBox1.SelectedItem))
                comboBox7.SelectedItem = comboBox1.SelectedItem;
        }

        private void hScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(sender as HScrollBar, (sender as HScrollBar).Value.ToString());
        }


        private void ApplyPreset(MapPreset pPreset)
        {
            PartialMap.Checked = !pPreset.m_bBordered;
            if (pPreset.m_iLandsCountPercent > 0)
                LandsCount.Value = LandsCount.Minimum + (LandsCount.Maximum - LandsCount.Minimum) * pPreset.m_iLandsCountPercent / 100;
            else
            {
                if (LandsCount.Maximum < 6000)
                    LandsCount.Value = LandsCount.Maximum;
                else
                    if (LandsCount.Minimum > 6000)
                        LandsCount.Value = LandsCount.Minimum;
                    else
                        LandsCount.Value = 6000;
            }

            if (LandMassesCount.Maximum < pPreset.m_iLandMassesCount)
                LandMassesCount.Value = LandMassesCount.Maximum;
            else
                if (LandMassesCount.Minimum > pPreset.m_iLandMassesCount)
                    LandMassesCount.Value = LandMassesCount.Minimum;
                else
                    LandMassesCount.Value = pPreset.m_iLandMassesCount;

            if (ContinentsCount.Maximum < pPreset.m_iContinentsCount)
                ContinentsCount.Value = ContinentsCount.Maximum;
            else
                if (ContinentsCount.Minimum > pPreset.m_iContinentsCount)
                    ContinentsCount.Value = ContinentsCount.Minimum;
                else
                    ContinentsCount.Value = pPreset.m_iContinentsCount;

            WaterPercent.Value = pPreset.m_iWaterCoverage;
            Equator.Value = pPreset.m_iEquatorPosition;
            Pole.Value = pPreset.m_iPoleDistance;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            { 
                MapPreset pPreset = listBox1.SelectedItem as MapPreset;
                label1.Text = pPreset.m_sDescription;

                ApplyPreset(pPreset);
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox3.Items.Clear();
            comboBox8.Items.Clear();
            for (int i = comboBox2.SelectedIndex; i < 9; i++)
            {
                comboBox3.Items.Add(string.Format("{0} [T{1}]", State.GetTechString(i), i));
                comboBox8.Items.Add(string.Format("{0} [T{1}]", State.GetTechString(i), i));
            }

            comboBox3.SelectedIndex = Math.Max(0, m_iTechLevelLimit - comboBox2.SelectedIndex);
            comboBox8.SelectedIndex = Math.Max(0, m_iInvadersTechLevelLimit - comboBox2.SelectedIndex);
        }

        private int m_iTechLevelLimit = 8;

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox3.SelectedIndex != -1)
                m_iTechLevelLimit = comboBox2.SelectedIndex + comboBox3.SelectedIndex;
        }

        private int m_iMagicLimit = 8;

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox5.SelectedIndex != -1)
                m_iMagicLimit = comboBox4.SelectedIndex + comboBox5.SelectedIndex;
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox5.Items.Clear();
            comboBox9.Items.Clear();
            for (int i = comboBox4.SelectedIndex; i < 9; i++)
            {
                comboBox5.Items.Add(string.Format("{0} [M{1}]", State.GetMagicString(i), i));
                comboBox9.Items.Add(string.Format("{0} [M{1}]", State.GetMagicString(i), i));
            }

            comboBox5.SelectedIndex = Math.Max(0, m_iMagicLimit - comboBox4.SelectedIndex);
            comboBox9.SelectedIndex = Math.Max(0, m_iInvadersMagicLimit - comboBox4.SelectedIndex);
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                MapPreset pPreset = listBox2.SelectedItem as MapPreset;
                label19.Text = pPreset.m_sDescription;

                ApplyPreset(pPreset);
            }
        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox7.SelectedIndex == -1)
                return;

            m_cLocations = (LocationsGrid<LocationX>)comboBox7.SelectedItem;

            groupBox2.Enabled = true;
            groupBox3.Enabled = true;
            groupBox4.Enabled = true;
            tableLayoutPanel2.Enabled = true;
            button8.Enabled = true;

            CalculateLimits(m_cLocations.m_iLocationsCount);

            if (comboBox1.Items.Contains(comboBox7.SelectedItem))
                comboBox1.SelectedItem = comboBox7.SelectedItem;
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox6.SelectedIndex == -1)
                return;

            m_cLocations = (LocationsGrid<LocationX>)comboBox6.SelectedItem;

            groupBox2.Enabled = true;
            groupBox3.Enabled = true;
            groupBox4.Enabled = true;
            tableLayoutPanel2.Enabled = true;
            button8.Enabled = true;

            CalculateLimits(m_cLocations.m_iLocationsCount);

            if (comboBox1.Items.Contains(comboBox6.SelectedItem))
                comboBox1.SelectedItem = comboBox6.SelectedItem;
        }

        private int m_iInvadersTechLevelLimit = 8;
        private int m_iInvadersMagicLimit = 8;

        private void comboBox8_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox8.SelectedIndex != -1)
                m_iInvadersTechLevelLimit = comboBox2.SelectedIndex + comboBox8.SelectedIndex;
        }

        private void comboBox9_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox9.SelectedIndex != -1)
                m_iInvadersMagicLimit = comboBox4.SelectedIndex + comboBox9.SelectedIndex;
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex != -1)
            {
                SocietyPreset pPreset = listBox3.SelectedItem as SocietyPreset;
                label20.Text = pPreset.m_sDescription;

                comboBox2.SelectedIndex = pPreset.m_iMinTechLevel;
                comboBox3.SelectedIndex = pPreset.m_iMaxTechLevel - pPreset.m_iMinTechLevel;

                comboBox4.SelectedIndex = pPreset.m_iMinMagicLevel;
                comboBox5.SelectedIndex = pPreset.m_iMaxMagicLevel - pPreset.m_iMinMagicLevel;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
                comboBox7_SelectedIndexChanged(sender, e);
            if (tabControl1.SelectedIndex == 1)
                comboBox6_SelectedIndexChanged(sender, e);
            if (tabControl1.SelectedIndex == 2)
                comboBox1_SelectedIndexChanged(sender, e);
        }

        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox4.SelectedIndex != -1)
            {
                SocietyPreset pPreset = listBox4.SelectedItem as SocietyPreset;
                label23.Text = pPreset.m_sDescription;

                comboBox2.SelectedIndex = pPreset.m_iMinTechLevel;
                comboBox3.SelectedIndex = pPreset.m_iMaxTechLevel - pPreset.m_iMinTechLevel;

                comboBox4.SelectedIndex = pPreset.m_iMinMagicLevel;
                comboBox5.SelectedIndex = pPreset.m_iMaxMagicLevel - pPreset.m_iMinMagicLevel;
            }
        }
    }
}
