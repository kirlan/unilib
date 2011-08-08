using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StarMap;
using Random;

namespace StarMapVis
{
    public partial class UniverseMap : UserControl
    {
        public enum MapDrawType
        {
            mdtRaces,
            mdtWorldType,
            mdtTechLevel,
            mdtSocium,
            mdtWealth
        }

        MapDrawType m_eMapDrawType = MapDrawType.mdtRaces;
        public MapDrawType MapDraw
        {
            get { return m_eMapDrawType; }
            set 
            { 
                m_eMapDrawType = value;
                DrawMap();
            }
        }

        public UniverseMap()
        {
            InitializeComponent();

            ActualMap = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);
        }

        Bitmap ActualMap;

        private void UniverseMap_Resize(object sender, EventArgs e)
        {
            ActualMap = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);

            DrawMap();
        }

        private void UniverseMap_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(ActualMap, 0, 0);
        }

        private Color[] m_aColorsID;

        private CUniverse m_pUniverse = null;

        public void DrawMap(CUniverse pUniverse)
        {
            m_pUniverse = pUniverse;

            m_aColorsID = new Color[m_pUniverse.m_cRaces.Count];
            for (int i = 0; i < m_pUniverse.m_cRaces.Count; i++)
            {
                switch (Rnd.Get(3))
                { 
                    case 0:
                        m_aColorsID[i] = Color.FromArgb(255, Rnd.Get(256), Rnd.Get(256));
                        break;
                    case 1:
                        m_aColorsID[i] = Color.FromArgb(Rnd.Get(256), 255, Rnd.Get(256));
                        break;
                    case 2:
                        m_aColorsID[i] = Color.FromArgb(Rnd.Get(256), Rnd.Get(256), 255);
                        break;
                }
            }

            DrawMap();
        }

        public void DrawMap()
        {
            Graphics gr = Graphics.FromImage(ActualMap);

            Brush fill = Brushes.Black;
            gr.FillRectangle(fill, 0, 0, ClientRectangle.Width, ClientRectangle.Height);

            if (m_pUniverse == null)
                return;

            if (m_pUniverse.m_cRaces == null || m_pUniverse.m_cWorlds == null)
                return;

            if (m_pUniverse.m_cWorlds.Count == 0)
                return;

            //float fkX = ClientRectangle.Width / m_pUniverse.m_iWidth;
            //float fkY = ClientRectangle.Height / m_pUniverse.m_iHeight;

            foreach (CWorld pWorld in m_pUniverse.m_cWorlds)
            {
                //int iDiameter = (int)(fkY / 3);
                Brush marking = GetWorldBrush(pWorld);

                //if (pWorld.m_eWorldType == CWorld.WorldType.PrimitiveWorld ||
                //    pWorld.m_eWorldType == CWorld.WorldType.EmptyWorld)
                //{
                //    if (pWorld.m_pNearestCivilizedWorld != null)
                //    {
                //        gr.FillEllipse(Brushes.Gray, pWorld.m_iX * fkX - 1, pWorld.m_iY * fkY - 1, iDiameter+2, iDiameter+2);
                //    }
                //}
                //gr.FillEllipse(marking, pWorld.m_iX * fkX, pWorld.m_iY * fkY, iDiameter, iDiameter);
                DrawWorld(pWorld, Brushes.Black);
            }

            if (m_pSelectedWorld != null)
            {
                ShowWorld(m_pSelectedWorld, true);
                SelectWorld(m_pSelectedWorld);
            }

            Refresh();
        }

        private Brush GetWorldBrush(CWorld pWorld)
        {
            Brush marking = Brushes.Black;
            switch (m_eMapDrawType)
            {
                case MapDrawType.mdtRaces:
                    {
                        if (pWorld.m_bHaveHabitablePlanets)
                            marking = new SolidBrush(Color.FromArgb(40, 40, 40));

                        if (pWorld.m_pMajorRace != null)
                            marking = new SolidBrush(m_aColorsID[pWorld.m_pMajorRace.m_iID]);

                    }
                    break;
                case MapDrawType.mdtSocium:
                    {
                        if (pWorld.m_pMajorRace != null)
                        {
                            switch (pWorld.m_pMajorRace.m_eSociety)
                            {
                                case CRace.Extraversion.lv0_Closed:
                                    marking = Brushes.Purple;
                                    break;
                                case CRace.Extraversion.lv1_Peaceful:
                                    marking = Brushes.Green;
                                    break;
                                case CRace.Extraversion.lv2_Agressive:
                                    marking = Brushes.Red;
                                    break;
                            }
                        }
                    }
                    break;
                case MapDrawType.mdtTechLevel:
                    {
                        if (pWorld.m_pMajorRace != null)
                        {
                            switch (pWorld.m_pMajorRace.m_eLevel)
                            {
                                case CRace.TechLevel.lv0_Barbarian:
                                    marking = Brushes.DarkRed;
                                    break;
                                case CRace.TechLevel.lv1_Medieval:
                                    marking = Brushes.DarkGoldenrod;
                                    break;
                                case CRace.TechLevel.lv2_Industrial:
                                    marking = Brushes.DarkKhaki;
                                    break;
                                case CRace.TechLevel.lv3_Humans:
                                    marking = Brushes.Green;
                                    break;
                                case CRace.TechLevel.lv4_Heirs:
                                    marking = Brushes.Cyan;
                                    break;
                                case CRace.TechLevel.lv5_AncientEsses:
                                    marking = Brushes.Blue;
                                    break;
                                case CRace.TechLevel.lv6_Magistrate:
                                    marking = Brushes.Purple;
                                    break;
                                case CRace.TechLevel.lv7_AncientAnnan:
                                    marking = Brushes.Gray;
                                    break;
                                case CRace.TechLevel.lv8_ModernEsses:
                                    marking = Brushes.Silver;
                                    break;
                                case CRace.TechLevel.lv9_DarkDragons:
                                    marking = Brushes.White;
                                    break;
                            }
                        }
                    }
                    break;
                case MapDrawType.mdtWorldType:
                    {
                        switch (pWorld.m_eWorldType)
                        {
                            case CWorld.WorldType.PrimitiveWorld:
                                marking = Brushes.Red;
                                break;
                            case CWorld.WorldType.ClosedWorld:
                                marking = Brushes.Orange;
                                break;
                            case CWorld.WorldType.AdvancingWorld:
                                marking = Brushes.Yellow;
                                break;
                            case CWorld.WorldType.Colony:
                                marking = Brushes.Green;
                                break;
                            case CWorld.WorldType.AdvancedWorld:
                                marking = Brushes.Cyan;
                                break;
                            case CWorld.WorldType.EmptyWorld:
                                if (pWorld.m_bHaveHabitablePlanets)
                                    marking = new SolidBrush(Color.FromArgb(40, 40, 40));
                                break;
                        }
                    }
                    break;
            }
            return marking;
        }

        CWorld m_pSelectedWorld = null;

        private void UniverseMap_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_pUniverse == null)
                return;

            if (m_pUniverse.m_cWorlds.Count == 0)
                return;

            if (m_pSelectedWorld != null)
                ShowWorld(m_pSelectedWorld, false);

            float fkX = ClientRectangle.Width / m_pUniverse.m_iWidth;
            float fkY = ClientRectangle.Height / m_pUniverse.m_iHeight;

            int iX = (int)((e.X - fkX) / fkX);
            int iY = (int)((e.Y - fkY) / fkY);

            foreach (CWorld pWorld in m_pUniverse.m_cWorlds)
            {
                if (pWorld.m_iX == iX && pWorld.m_iY == iY)
                    m_pSelectedWorld = pWorld;
            }

            if (m_pSelectedWorld != null)
            {
                ShowWorld(m_pSelectedWorld, true);
                SelectWorld(m_pSelectedWorld);
                Refresh();
            }
        }

        private void ShowWorld(CWorld pSelectedWorld, bool bShowLightUp)
        {
            if (pSelectedWorld == null)
                return;

            if (pSelectedWorld.m_pMajorRace != null)
            {
                foreach (CWorld pWorld in pSelectedWorld.m_pMajorRace.m_cOwnedWorlds)
                {
                    DrawWorld(pWorld, bShowLightUp ? Brushes.Gray : Brushes.Black);
                    foreach (CRoute pRoute in pWorld.m_cOutcoming)
                    {
                        DrawWorld(pRoute.m_pTo, bShowLightUp ? new SolidBrush(Color.FromArgb(40, 40, 40)) : Brushes.Black);
                    }
                }
            }

            DrawWorld(pSelectedWorld, bShowLightUp ? Brushes.White : Brushes.Black);
        }

        private void DrawWorld(CWorld pWorld, Brush lightUp)
        {
            Graphics gr = Graphics.FromImage(ActualMap);

            float fkX = ClientRectangle.Width / m_pUniverse.m_iWidth;
            float fkY = ClientRectangle.Height / m_pUniverse.m_iHeight;

            int iDiameter = 2 + pWorld.m_iPotential/2;

            if (iDiameter > fkX)
                iDiameter = (int)fkX;
            if (iDiameter > fkY)
                iDiameter = (int)fkY;

            gr.FillEllipse(lightUp, fkX + pWorld.m_iX * fkX - iDiameter / 2 - 1, fkY + pWorld.m_iY * fkY - iDiameter / 2 - 1, iDiameter + 2, iDiameter + 2);
            foreach(CRoute pRoute in pWorld.m_cIncoming)
            {
                gr.DrawLine(new Pen(lightUp), fkX + pWorld.m_iX * fkX, fkY + pWorld.m_iY * fkY,
                    fkX + pRoute.m_pFrom.m_iX * fkX, fkY + pRoute.m_pFrom.m_iY * fkY);
            }
            Brush marking = GetWorldBrush(pWorld);
            gr.FillEllipse(marking, fkX + pWorld.m_iX * fkX - iDiameter / 2, fkY + pWorld.m_iY * fkY - iDiameter / 2, iDiameter, iDiameter);
        }

        public class WorldSelectedEventArgs : EventArgs
        {
            private CWorld m_pWorld;
            public CWorld World
            {
                get { return m_pWorld; }
            }

            public WorldSelectedEventArgs(CWorld pWorld)
            {
                m_pWorld = pWorld;
            }
        }

        public event EventHandler<WorldSelectedEventArgs> WorldSelectedEvent;

        private void SelectWorld(CWorld pWorld)
        {
            // Copy to a temporary variable to be thread-safe.
            EventHandler<WorldSelectedEventArgs> temp = WorldSelectedEvent;
            if (temp != null)
                temp(this, new WorldSelectedEventArgs(pWorld));
        }
    }
}
