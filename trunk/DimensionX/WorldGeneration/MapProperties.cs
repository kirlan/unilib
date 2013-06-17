using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LandscapeGeneration;
using Socium;
using Random;
using LandscapeGeneration.PlanetBuilder;

namespace WorldGeneration
{
    public partial class MapProperties : UserControl
    {
        MapPreset[] m_aWorldMaps = new MapPreset[] 
        {
            new MapPreset("Gondwana", "One big continent ocuppies 33% of world surface.", 15, 25, 1, 66),
            new MapPreset("Continents", "Earth-like world. 5 big continents ocuppies 33% of world surface.", 15, 25, 5, 66),
            new MapPreset("Archipelago", "About 30 big islands are evenly dispersed over the map and totally ocuppies 33% of world surface.", 100, 100, 30, 66),
        };

        MapPreset[] m_aPartialMaps = new MapPreset[] 
        {
            new MapPreset("Atlantis", "Traditional adventure map of 1 one big continent, with a complex coastral line.", 15, 25, 1, 50),
            new MapPreset("Duality", "2 big continents, divided by a wide ocean.", 15, 25, 2, 75),
            new MapPreset("Mediterranean", "Mediterranean-like region - there are 3 big continents, divided by a narrow sea.", 15, 25, 3, 50),
            new MapPreset("Islands Paradise", "Archipelago of about 15 small islands.", 100, 100, 15, 90),
        };

        private WorkingArea m_eWorkingArea = WorkingArea.WholeSphere;

        public WorkingArea WorkingArea
        {
            get { return m_eWorkingArea; }
            set 
            {
                m_eWorkingArea = value;

                Enabled = true;

                if (MapPresets.Items.Count == 0 || MapPresets.Items.Contains(m_aWorldMaps[0]) != (m_eWorkingArea == WorkingArea.WholeSphere))
                {
                    MapPresets.Items.Clear();

                    if (m_eWorkingArea == WorkingArea.WholeSphere)
                        MapPresets.Items.AddRange(m_aWorldMaps);
                    else
                        MapPresets.Items.AddRange(m_aPartialMaps);
                }

                CalculateLimits();

                if (MapPresets.Items.Count > 0)
                    MapPresets.SelectedIndex = 0;
            }
        }

        private int m_iChunkSize = 800;

        public int ChunkSize
        {
            get { return m_iChunkSize; }
            set 
            { 
                m_iChunkSize = value;

                CalculateLimits();
            }
        }

        private int m_iChunksCount = 5;

        public int ChunksCount
        {
            get { return m_iChunksCount; }
            set 
            { 
                m_iChunksCount = value;

                CalculateLimits();
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

        public MapProperties()
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

            MapPresets.Items.Clear();

            Enabled = false;
        }

        private void CalculateLimits()
        {
            //if (LandsCountBar.Minimum > Math.Min(600, iLocationsCount / 4))
            //    LandsCountBar.Minimum = Math.Min(600, iLocationsCount / 4);

            //if (LandsCountBar.Maximum < iLocationsCount / 2)
            //    LandsCountBar.Maximum = iLocationsCount / 2;

            //if (LandsCountBar.Value > iLocationsCount / 2)
            //    LandsCountBar.Value = iLocationsCount / 2;

            //if (LandsCountBar.Value < Math.Min(600, iLocationsCount / 4))
            //    LandsCountBar.Value = Math.Min(600, iLocationsCount / 4);

            //LandsCountBar.Maximum = iLocationsCount / 2 + LandsCountBar.LargeChange - 1;
            //LandsCountBar.Minimum = Math.Min(600, iLocationsCount / 4);

            //if (LandMassesCountBar.Minimum > Math.Min(30, iLocationsCount / 20))
            //    LandMassesCountBar.Minimum = Math.Min(30, iLocationsCount / 20);

            //if (LandMassesCountBar.Maximum < Math.Min(300, iLocationsCount / 4))
            //    LandMassesCountBar.Maximum = Math.Min(300, iLocationsCount / 4);

            //if (LandMassesCountBar.Value > Math.Min(300, iLocationsCount / 4))
            //    LandMassesCountBar.Value = Math.Min(300, iLocationsCount / 4);

            //if (LandMassesCountBar.Value < Math.Min(30, iLocationsCount / 20))
            //    LandMassesCountBar.Value = Math.Min(30, iLocationsCount / 20);

            //LandMassesCountBar.Maximum = Math.Min(300, iLocationsCount / 4) + LandMassesCountBar.LargeChange - 1;
            //LandMassesCountBar.Minimum = Math.Min(30, iLocationsCount / 20);

            int iLocations = m_iChunkSize * m_iChunksCount * m_iChunksCount;
            if (m_eWorkingArea == WorkingArea.WholeSphere)
                iLocations *= 6;
            if (m_eWorkingArea == WorkingArea.HalfSphereEquatorial || m_eWorkingArea == WorkingArea.HalfSpherePolar)
                iLocations *= 3;

            int iNotOcean = iLocations * (100 - (int)WaterPercentBar.Value) / 100;

            //if (StatesCountBar.Minimum > Math.Min(1, iNotOcean / 40))
                StatesCountBar.Minimum = Math.Min(1, iNotOcean / 40);

            //f (StatesCountBar.Maximum < Math.Min(100, iNotOcean / 20))
                StatesCountBar.Maximum = Math.Min(100, iNotOcean / 20);

            if (StatesCountBar.Value > Math.Min(100, iNotOcean / 20))
                StatesCountBar.Value = Math.Min(100, iNotOcean / 20);

            if (StatesCountBar.Value < Math.Min(1, iNotOcean / 40))
                StatesCountBar.Value = Math.Min(1, iNotOcean / 40);

            StatesCountBar.Maximum = Math.Min(100, iNotOcean / 20) + StatesCountBar.LargeChange - 1;
            StatesCountBar.Minimum = Math.Min(1, iNotOcean / 40);
        }

        private void PresetsPanel_Resize(object sender, EventArgs e)
        {
            MapPresets.Width = PresetsPanel.Width / 3;
        }

        private void MapProperties_Resize(object sender, EventArgs e)
        {
            PresetsPanel.Width = ClientRectangle.Width;
            PresetsPanel.Height = ClientRectangle.Height;

            AdvancedPanel.Width = ClientRectangle.Width;
            AdvancedPanel.Height = ClientRectangle.Height;
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void MapPresets_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MapPresets.SelectedIndex != -1)
            {
                MapPreset pPreset = MapPresets.SelectedItem as MapPreset;
                MapPresetDescription.Text = pPreset.m_sDescription;

                ApplyPreset(pPreset);
            }
        }

        private void ApplyPreset(MapPreset pPreset)
        {
            LandsCountBar.Value = pPreset.m_iLandsCountPercent;
            LandMassesCountBar.Value = pPreset.m_iLandMassesPercent;
            ContinentsCountEdit.Value = pPreset.m_iContinentsCount;

            WaterPercentBar.Value = pPreset.m_iWaterCoverage;

            //считаем количество государств на отображаемом участке карты из рассчёта что на полной карте имеем 60 государств (т.е. на четвертинке карты, такой как средиземье или хайбория, будет 15 государств...)
            StatesCountBar.Value = 10;
            if (m_eWorkingArea == WorkingArea.WholeSphere)
                StatesCountBar.Value *= 6;
            if (m_eWorkingArea == WorkingArea.HalfSphereEquatorial || m_eWorkingArea == WorkingArea.HalfSpherePolar)
                StatesCountBar.Value *= 3;
        }

        public int LandsDiversity
        {
            get { return LandsCountBar.Value; }
            set { LandsCountBar.Value = value; }
        }

        public int LandMassesDiversity
        {
            get { return LandMassesCountBar.Value; }
            set { LandMassesCountBar.Value = value; }
        }

        public int ContinentsCount
        {
            get { return (int)ContinentsCountEdit.Value; }
            set { ContinentsCountEdit.Value = value; }
        }

        public int WaterPercent
        {
            get { return WaterPercentBar.Value; }
            set { WaterPercentBar.Value = value; }
        }

        //public bool PartialMap
        //{
        //    get { return PartialMapBox.Checked; }
        //    set { PartialMapBox.Checked = value; }
        //}

        //public int EquatorPosition
        //{
        //    get { return EquatorBar.Value; }
        //    set { EquatorBar.Value = value; }
        //}

        //public int PoleDistance
        //{
        //    get { return PoleBar.Value; }
        //    set { PoleBar.Value = value; }
        //}

        public int StatesCount
        {
            get { return StatesCountBar.Value; }
            set { StatesCountBar.Value = value; }
        }

        public void Randomize()
        {
            ContinentsCountEdit.Value = ContinentsCountEdit.Minimum + Rnd.Get((int)ContinentsCountEdit.Maximum - (int)ContinentsCountEdit.Minimum);

            //EquatorBar.Value = Rnd.Get(200) - 50;

            //PoleBar.Value = Math.Max(100 - EquatorBar.Value, EquatorBar.Value);
            //if (Rnd.OneChanceFrom(2))
            //    PoleBar.Value = PoleBar.Value + Rnd.Get((int)(PoleBar.Value / 2));

            WaterPercentBar.Value = WaterPercentBar.Minimum + Rnd.Get((int)WaterPercentBar.Maximum - (int)WaterPercentBar.Minimum);

            StatesCountBar.Value = StatesCountBar.Minimum + Rnd.Get((int)StatesCountBar.Maximum - (int)StatesCountBar.Minimum);
        }

        private void ScrollBar_ValueChanged(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(sender as HScrollBar, (sender as HScrollBar).Value.ToString());
        }
    }
}
