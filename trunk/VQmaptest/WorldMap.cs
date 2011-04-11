using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VQmaptest;
using Random;

namespace VixenQuest
{
    public partial class WorldMap : UserControl
    {
        Bitmap ActualMap;

        public WorldMap()
        {
            InitializeComponent();

            ActualMap = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);
        }

        private void WorldMap_Resize(object sender, EventArgs e)
        {
            ActualMap = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);

            DrawMap();
        }

        private void WorldMap_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(ActualMap, 0, 0);
        }

        private Dictionary<Land, Color> m_aColorsID = new Dictionary<Land,Color>();

        private World m_pWorld = null;

        public void DrawMap(World pWorld)
        {
            m_pWorld = pWorld;

            m_aColorsID.Clear();
            foreach (Land pLand in m_pWorld.m_cLands)
            {
                switch (Rnd.Get(3))
                {
                    case 0:
                        m_aColorsID[pLand] = Color.FromArgb(255, Rnd.Get(256), Rnd.Get(256));
                        break;
                    case 1:
                        m_aColorsID[pLand] = Color.FromArgb(Rnd.Get(256), 255, Rnd.Get(256));
                        break;
                    case 2:
                        m_aColorsID[pLand] = Color.FromArgb(Rnd.Get(256), Rnd.Get(256), 255);
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

            if (m_pWorld == null)
                return;

            if (m_pWorld.m_cLands.Count == 0)
                return;

            foreach (int x in m_pWorld.m_cMap.Keys)
            {
                foreach (int y in m_pWorld.m_cMap[x].Keys)
                {
                    //int iDiameter = (int)(fkY / 3);

                    //if (pWorld.m_eWorldType == CWorld.WorldType.PrimitiveWorld ||
                    //    pWorld.m_eWorldType == CWorld.WorldType.EmptyWorld)
                    //{
                    //    if (pWorld.m_pNearestCivilizedWorld != null)
                    //    {
                    //        gr.FillEllipse(Brushes.Gray, pWorld.m_iX * fkX - 1, pWorld.m_iY * fkY - 1, iDiameter+2, iDiameter+2);
                    //    }
                    //}
                    //gr.FillEllipse(marking, pWorld.m_iX * fkX, pWorld.m_iY * fkY, iDiameter, iDiameter);
                    DrawLandPoint(x, y);//, Brushes.Black);
                }
            }

            foreach (Land pLand in m_pWorld.m_cLands)
            {
                DrawLandCenter(pLand);
            }

            //if (m_pSelectedWorld != null)
            //{
            //    ShowWorld(m_pSelectedWorld, true);
            //    SelectWorld(m_pSelectedWorld);
            //}

            Refresh();
        }

        private Brush GetWorldBrush(Land pLand)
        {
            if (pLand != null)
                return new SolidBrush(m_aColorsID[pLand]);
            else
                return Brushes.Black;
        }

        private void DrawLandPoint(int x, int y)//, Brush lightUp)
        {
            Graphics gr = Graphics.FromImage(ActualMap);

            float fkX = (float)ClientRectangle.Width / (m_pWorld.WorldScale * 2);
            float fkY = (float)ClientRectangle.Height / (m_pWorld.WorldScale * 2);

            Brush marking = GetWorldBrush(m_pWorld.m_cMap[x][y].m_pLand);
            gr.FillRectangle(marking, fkX + (m_pWorld.WorldScale + x) * fkX - fkX / 2, fkY + (m_pWorld.WorldScale + y) * fkY - fkY / 2, fkX, fkY);
        }

        private void DrawLandCenter(Land pLand)
        {
            Graphics gr = Graphics.FromImage(ActualMap);

            float fkX = (float)ClientRectangle.Width / (m_pWorld.WorldScale * 2);
            float fkY = (float)ClientRectangle.Height / (m_pWorld.WorldScale * 2);

            int iDiameter = (int)Math.Sqrt(fkX * fkX + fkY * fkY);

            //gr.FillEllipse(lightUp, fkX + (m_pWorld.WorldScale + pLand.X) * fkX - iDiameter / 2 - 1, fkY + (m_pWorld.WorldScale + pLand.Y) * fkY - iDiameter / 2 - 1, iDiameter + 2, iDiameter + 2);

            Brush marking = Brushes.Black;
            gr.FillEllipse(marking, fkX + (m_pWorld.WorldScale + pLand.X) * fkX - iDiameter / 2, fkY + (m_pWorld.WorldScale + pLand.Y) * fkY - iDiameter / 2, iDiameter, iDiameter);

            foreach (Land pLink in pLand.Links)
            {
                gr.DrawLine(new Pen(marking), fkX + (m_pWorld.WorldScale + pLand.X) * fkX, fkY + (m_pWorld.WorldScale + pLand.Y) * fkY,
                    fkX + (m_pWorld.WorldScale + pLink.X) * fkX, fkY + (m_pWorld.WorldScale + pLink.Y) * fkY);
            }
        }
    }
}
