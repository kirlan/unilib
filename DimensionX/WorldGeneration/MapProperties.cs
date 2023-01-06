﻿using System;
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
            new MapPreset("Continents", "Earth-like world. 5 big continents ocuppies 33% of world surface. Polar regions are lands-free in this template.", true, true, 15, 25, 5, 66, 50, 45),
            new MapPreset("Continents 2", "Earth-like world. 5 big continents ocuppies 33% of world surface. There could be a continents in polar regions in this template.", true, false, 15, 25, 5, 66, 50, 50),
            new MapPreset("Gondwana", "One big continent ocuppies 33% of world surface. Polar regions are lands-free in this template.", true, true, 15, 25, 1, 66, 50, 45),
            new MapPreset("Archipelago", "About 30 big islands are evenly dispersed over the map and totally ocuppies 33% of world surface. Polar regions are lands-free in this template.", true, true, 100, 100, 30, 66, 50, 45),
        };

        MapPreset[] m_aPartialMaps = new MapPreset[] 
        {
            new MapPreset("Coast", "Traditional adventure map of big coastral region, like Europe, Middlearth, Hyperborea or Faerun. This is a part of a one big continent from arctic to tropics, with a long coastral line.", false, false, 15, 25, 1, 50, 95, 95),
            new MapPreset("Mediterranean", "Mediterranean-like region - there are parts of 3 big continents, divided by a sea. Continents extends from arctic to tropics.", false, false, 15, 25, 3, 50, 95, 95),
            new MapPreset("Atlantis", "One big continent in middle latitudes, surrounded by ocean.", false, true, 15, 25, 1, 50, 120, 130),
            new MapPreset("Tropical Paradise", "Archipelago of about 15 tropical islands.", false, true, 100, 100, 15, 90, 50, 100),
        };

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

        private void CalculateLimits(int iLocationsCount)
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

            int iNotOcean = iLocationsCount * (100 - (int)WaterPercentBar.Value) / 100;

            //if (StatesCountBar.Minimum > Math.Min(1, iNotOcean / 40))
                StatesCountBar.Minimum = Math.Min(1, iNotOcean / 40);

            //if (StatesCountBar.Maximum < Math.Min(100, iNotOcean / 20))
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
            LandsCountBar.Value = pPreset.m_iLandsCountPercent;
            LandMassesCountBar.Value = pPreset.m_iLandMassesPercent;
            ContinentsCountEdit.Value = pPreset.m_iContinentsCount;

            WaterPercentBar.Value = pPreset.m_iWaterCoverage;
            EquatorBar.Value = pPreset.m_iEquatorPosition;
            PoleBar.Value = pPreset.m_iPoleDistance;

            //считаем количество государств на отображаемом участке карты из рассчёта что на полной карте имеем 60 государств (т.е. на четвертинке карты, такой как средиземье или хайбория, будет 15 государств...)
            StatesCountBar.Value = Math.Min(StatesCountBar.Maximum, Math.Max(StatesCountBar.Minimum, 15 * 100 * 100 / (PoleBar.Value * PoleBar.Value)));
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

        public bool PartialMap
        {
            get { return PartialMapBox.Checked; }
            set { PartialMapBox.Checked = value; }
        }

        public int EquatorPosition
        {
            get { return EquatorBar.Value; }
            set { EquatorBar.Value = value; }
        }

        public int PoleDistance
        {
            get { return PoleBar.Value; }
            set { PoleBar.Value = value; }
        }

        public int StatesCount
        {
            get { return StatesCountBar.Value; }
            set { StatesCountBar.Value = value; }
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
