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

namespace WorldGeneration
{
    public partial class MapProperties : UserControl
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

        private LocationsGrid<LocationX> m_cLocationsGrid = null;

        [Browsable(false)]
        public LocationsGrid<LocationX> LocationsGrid
        {
            get { return m_cLocationsGrid; }
            set 
            {
                m_cLocationsGrid = value;

                Enabled = m_cLocationsGrid != null;

                if (m_cLocationsGrid == null)
                    return;

                if (MapPresets.Items.Contains(m_aWorldMaps[0]) != m_cLocationsGrid.m_bCycled)
                {
                    MapPresets.Items.Clear();

                    if (m_cLocationsGrid.m_bCycled)
                        MapPresets.Items.AddRange(m_aWorldMaps);
                    else
                        MapPresets.Items.AddRange(m_aPartialMaps);
                }

                CalculateLimits(m_cLocationsGrid.m_iLocationsCount);

                if (MapPresets.Items.Count > 0)
                    MapPresets.SelectedIndex = 0;
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

            Enabled = false;
        }

        private void CalculateLimits(int iLocationsCount)
        {
            if (LandsCountBar.Minimum > Math.Min(600, iLocationsCount / 4))
                LandsCountBar.Minimum = Math.Min(600, iLocationsCount / 4);

            if (LandsCountBar.Maximum < iLocationsCount / 2)
                LandsCountBar.Maximum = iLocationsCount / 2;

            if (LandsCountBar.Value > iLocationsCount / 2)
                LandsCountBar.Value = iLocationsCount / 2;

            if (LandsCountBar.Value < Math.Min(600, iLocationsCount / 4))
                LandsCountBar.Value = Math.Min(600, iLocationsCount / 4);

            LandsCountBar.Maximum = iLocationsCount / 2 + LandsCountBar.LargeChange - 1;
            LandsCountBar.Minimum = Math.Min(600, iLocationsCount / 4);

            if (LandMassesCountBar.Minimum > Math.Min(30, iLocationsCount / 20))
                LandMassesCountBar.Minimum = Math.Min(30, iLocationsCount / 20);

            if (LandMassesCountBar.Maximum < Math.Min(300, iLocationsCount / 4))
                LandMassesCountBar.Maximum = Math.Min(300, iLocationsCount / 4);

            if (LandMassesCountBar.Value > Math.Min(300, iLocationsCount / 4))
                LandMassesCountBar.Value = Math.Min(300, iLocationsCount / 4);

            if (LandMassesCountBar.Value < Math.Min(30, iLocationsCount / 20))
                LandMassesCountBar.Value = Math.Min(30, iLocationsCount / 20);

            LandMassesCountBar.Maximum = Math.Min(300, iLocationsCount / 4) + LandMassesCountBar.LargeChange - 1;
            LandMassesCountBar.Minimum = Math.Min(30, iLocationsCount / 20);

            int iNotOcean = iLocationsCount * (100 - (int)WaterPercentBar.Value) / 100;

            if (StatesCountBar.Minimum > Math.Min(1, iNotOcean / 40))
                StatesCountBar.Minimum = Math.Min(1, iNotOcean / 40);

            if (StatesCountBar.Maximum < Math.Min(100, iNotOcean / 20))
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
            PartialMapBox.Checked = !pPreset.m_bBordered;
            if (pPreset.m_iLandsCountPercent > 0)
                LandsCountBar.Value = LandsCountBar.Minimum + (LandsCountBar.Maximum - LandsCountBar.Minimum) * pPreset.m_iLandsCountPercent / 100;
            else
            {
                if (LandsCountBar.Maximum < 6000)
                    LandsCountBar.Value = LandsCountBar.Maximum;
                else
                    if (LandsCountBar.Minimum > 6000)
                        LandsCountBar.Value = LandsCountBar.Minimum;
                    else
                        LandsCountBar.Value = 6000;
            }

            if (LandMassesCountBar.Maximum < pPreset.m_iLandMassesCount)
                LandMassesCountBar.Value = LandMassesCountBar.Maximum;
            else
                if (LandMassesCountBar.Minimum > pPreset.m_iLandMassesCount)
                    LandMassesCountBar.Value = LandMassesCountBar.Minimum;
                else
                    LandMassesCountBar.Value = pPreset.m_iLandMassesCount;

            if (ContinentsCountEdit.Maximum < pPreset.m_iContinentsCount)
                ContinentsCountEdit.Value = ContinentsCountEdit.Maximum;
            else
                if (ContinentsCountEdit.Minimum > pPreset.m_iContinentsCount)
                    ContinentsCountEdit.Value = ContinentsCountEdit.Minimum;
                else
                    ContinentsCountEdit.Value = pPreset.m_iContinentsCount;

            WaterPercentBar.Value = pPreset.m_iWaterCoverage;
            EquatorBar.Value = pPreset.m_iEquatorPosition;
            PoleBar.Value = pPreset.m_iPoleDistance;

            //считаем количество государств на отображаемом участке карты из рассчёта что на полной карте имеем 60 государств (т.е. на четвертинке карты, такой как средиземье или хайбория, будет 15 государств...)
            StatesCountBar.Value = Math.Min(StatesCountBar.Maximum, Math.Max(StatesCountBar.Minimum, 150000 / (PoleBar.Value * PoleBar.Value)));
        }

        public int LandsCount
        {
            get { return LandsCountBar.Value; }
        }

        public int LandMassesCount
        {
            get { return LandMassesCountBar.Value; }
        }

        public int ContinentsCount
        {
            get { return (int)ContinentsCountEdit.Value; }
        }

        public int WaterPercent
        {
            get { return WaterPercentBar.Value; }
        }

        public bool PartialMap
        {
            get { return PartialMapBox.Checked; }
        }

        public int EquatorPosition
        {
            get { return EquatorBar.Value; }
        }

        public int PoleDistance
        {
            get { return PoleBar.Value; }
        }

        public int StatesCount
        {
            get { return StatesCountBar.Value; }
        }

        public void Randomize()
        {
            ContinentsCountEdit.Value = ContinentsCountEdit.Minimum + Rnd.Get((int)ContinentsCountEdit.Maximum - (int)ContinentsCountEdit.Minimum);

            EquatorBar.Value = Rnd.Get(200) - 50;

            PoleBar.Value = Math.Max(100 - EquatorBar.Value, EquatorBar.Value);
            if (Rnd.OneChanceFrom(2))
                PoleBar.Value = PoleBar.Value + Rnd.Get((int)(PoleBar.Value / 2));

            WaterPercentBar.Value = WaterPercentBar.Minimum + Rnd.Get((int)WaterPercentBar.Maximum - (int)WaterPercentBar.Minimum);

            StatesCountBar.Value = StatesCountBar.Minimum + Rnd.Get((int)StatesCountBar.Maximum - (int)StatesCountBar.Minimum);
        }

        private void ScrollBar_ValueChanged(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(sender as HScrollBar, (sender as HScrollBar).Value.ToString());
        }
    }
}
