using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EconomyModel;

namespace EconomyModelVis
{
    public partial class EconomyModelView : UserControl
    {
        public enum MapDrawType
        {
            mdtWealth,
            mdtHappyness
        }

        MapDrawType m_eMapDrawType = MapDrawType.mdtHappyness;
        public MapDrawType MapDraw
        {
            get { return m_eMapDrawType; }
            set { m_eMapDrawType = value; }
        }

        public EconomyModelView()
        {
            InitializeComponent();

            ActualMap = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);
        }

        Bitmap ActualMap;

        private void EconomyModelView_Resize(object sender, EventArgs e)
        {
            ActualMap = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);

            DrawMap();
        }

        private void EconomyModelView_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(ActualMap, 0, 0);
        }

        private CUniverse m_pUniverse = null;

        public void DrawMap(CUniverse pUniverse)
        {
            m_pUniverse = pUniverse;
            DrawMap();
        }

        public void DrawMap()
        {
            Graphics gr = Graphics.FromImage(ActualMap);

            Brush fill = Brushes.Black;
            gr.FillRectangle(fill, 0, 0, ClientRectangle.Width, ClientRectangle.Height);

            if (m_pUniverse == null)
                return;

            if (m_pUniverse.m_cWorlds == null)
                return;

            if (m_pUniverse.m_cWorlds.Count == 0)
                return;

            foreach (CWorld pWorld in m_pUniverse.m_cWorlds)
            {
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
                case MapDrawType.mdtHappyness:
                    {
                        marking = new SolidBrush(Color.FromArgb(255 - (int)(255 * pWorld.m_fHappiness), (int)(255 * pWorld.m_fHappiness), 0));
                    }
                    break;
                case MapDrawType.mdtWealth:
                    {
                        marking = Brushes.Gray;
                    }
                    break;
            }
            return marking;
        }

        CWorld m_pSelectedWorld = null;

        private void EconomyModelView_MouseMove(object sender, MouseEventArgs e)
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

            CWorld pSelected = m_pSelectedWorld;

            foreach (CWorld pWorld in m_pUniverse.m_cWorlds)
            {
                if (pWorld.m_iX == iX && pWorld.m_iY == iY)
                    pSelected = pWorld;
            }

            if (pSelected != null && pSelected != m_pSelectedWorld)
            {
                m_pSelectedWorld = pSelected;
                ShowWorld(m_pSelectedWorld, true);
                SelectWorld(m_pSelectedWorld);
                Refresh();
            }
        }

        private void ShowWorld(CWorld pSelectedWorld, bool bShowLightUp)
        {
            if (pSelectedWorld == null)
                return;

            DrawWorld(pSelectedWorld, bShowLightUp ? Brushes.White : Brushes.Black);
        }

        private void DrawWorld(CWorld pWorld, Brush lightUp)
        {
            Graphics gr = Graphics.FromImage(ActualMap);

            float fkX = ClientRectangle.Width / m_pUniverse.m_iWidth;
            float fkY = ClientRectangle.Height / m_pUniverse.m_iHeight;

            int iDiameter = 2 + pWorld.m_iPopulation / 2;

            if (iDiameter > fkX)
                iDiameter = (int)fkX;
            if (iDiameter > fkY)
                iDiameter = (int)fkY;

            gr.FillEllipse(lightUp, fkX + pWorld.m_iX * fkX - iDiameter / 2 - 1, fkY + pWorld.m_iY * fkY - iDiameter / 2 - 1, iDiameter + 2, iDiameter + 2);

            foreach (CTradingRoute pRoute in pWorld.m_cRoutes)
            {
                if (pRoute.m_pByer == pWorld)
                {
                    Pen _ligthUp = new Pen(lightUp);
                    gr.DrawLine(Pens.Aqua, fkX + pWorld.m_iX * fkX, fkY + pWorld.m_iY * fkY,
                        fkX + ((CWorld)pRoute.m_pSeller).m_iX * fkX, fkY + ((CWorld)pRoute.m_pSeller).m_iY * fkY);
                }
            }

            Brush marking = GetWorldBrush(pWorld);
            gr.FillEllipse(marking, fkX + pWorld.m_iX * fkX - iDiameter / 2, fkY + pWorld.m_iY * fkY - iDiameter / 2, iDiameter, iDiameter);

            if (pWorld.UnlimitedCredits)
            {
                int iInnerD = iDiameter / 2;
                if (iInnerD < 2)
                    iInnerD = 2;
                gr.FillEllipse(Brushes.Navy, fkX + pWorld.m_iX * fkX - iInnerD / 2, fkY + pWorld.m_iY * fkY - iInnerD / 2, iInnerD, iInnerD);
            }
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
