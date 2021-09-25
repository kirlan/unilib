using SimpleWorld.Geography;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Random;

namespace LostIsland
{
    public partial class Form1 : Form
    {
        CLostWorld m_pWorld = null;

        private readonly Dictionary<CLostFaction, Color> m_cColors = new Dictionary<CLostFaction, Color>();

        CLostLocation m_pCurrentLocation = null;
        CLostLocation m_pNextLocation = null;
        CLink m_pWalking = null;
        double m_fDaysPassed;

        /// <summary>
        /// Сколько дней требуется, чтобы пересечь половину карты пешком по равнине
        /// </summary>
        const double m_fTimeModifer = 14;

        const int m_iMapScale = 5;

        Bitmap ActualMap;

        public Form1()
        {
            Application.EnableVisualStyles();
            Application.DoEvents();

            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.UserPaint, true);

            typeof(Panel).InvokeMember("DoubleBuffered",
               BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
               null, panel1, new object[] { true });

            ActualMap = new Bitmap(panel1.Width, panel1.Height);
        }

        int ScaleX(double x)
        {
            if (m_pWorld == null)
                return 0;

            return ActualMap.Width * 4 * (m_pWorld.WorldScale + (int)x) / (m_iMapScale * 2 * m_pWorld.WorldScale);
            //return ActualMap.Width * (m_pWorld.WorldScale + (int)x) / (5 * 2 * m_pWorld.WorldScale);
        }

        int ToScreenX(double x)
        {
            return ActualMap.Width / 10 + ScaleX(x);
        }

        int ScaleY(double y)
        {
            if (m_pWorld == null)
                return 0;

            return ActualMap.Height * 4 * (m_pWorld.WorldScale + (int)y) / (m_iMapScale * 2 * m_pWorld.WorldScale);
            //return ActualMap.Height * (m_pWorld.WorldScale + (int)y) / (5 * 2 * m_pWorld.WorldScale);
        }

        int ToScreenY(double y)
        {
            return ActualMap.Height / 10 + ScaleY(y);
        }

        private readonly List<CLink> m_cTraversed = new List<CLink>();
        private readonly List<CLink> m_cVisibleLinks = new List<CLink>();
        private readonly List<CLostLocation> m_cVisibleLocations = new List<CLostLocation>();

        public void DrawMap()
        {
            Graphics gr = Graphics.FromImage(ActualMap);

            Brush fill = new SolidBrush(Color.FromArgb(0xd3, 0xfa, 0x5f));//Brushes.Black;
            gr.FillRectangle(fill, 0, 0, ActualMap.Width, ActualMap.Height);

            if (m_pWorld == null)
                return;

            Pen pDarkBlack = new Pen(Color.FromArgb(30, 30, 30), 2);

            for (int i = 0; i < m_pWorld.Locations.Length; i++)
            {
                CLostLocation pLoc = m_pWorld.Locations[i];
                //long r = CWorld.m_iMinDist/2;
                //gr.DrawEllipse(pDarkBlack, pWorld.WorldScale + pLoc.X - r, pWorld.WorldScale + pLoc.Y - r, 2 * r, 2 * r);

                using (GraphicsPath pPath = new GraphicsPath())
                {
                    foreach (var pEdge in pLoc.m_cEdges)
                    {
                        pPath.AddPolygon(new Point[] {new Point(ToScreenX(pEdge.Value.m_pFrom.Circumcenter.X), ToScreenY(pEdge.Value.m_pFrom.Circumcenter.Y)),
                                                                                        new Point(ToScreenX(pEdge.Value.m_pMidPoint.Circumcenter.X), ToScreenY(pEdge.Value.m_pMidPoint.Circumcenter.Y)),
                                                                                        new Point(ToScreenX(pEdge.Value.m_pTo.Circumcenter.X), ToScreenY(pEdge.Value.m_pTo.Circumcenter.Y)),
                                                                                        new Point(ToScreenX(pLoc.X), ToScreenY(pLoc.Y))});
                    }
                    if (pLoc.Territory == CLostTerritory.DeepSea)
                    {
                        gr.FillPath(new HatchBrush(HatchStyle.ZigZag, Color.Black, Color.FromArgb(0x1e, 0x5e, 0x69)), pPath);
                    }
                    else if (pLoc.Territory == CLostTerritory.Shallows)
                    {
                        gr.FillPath(new HatchBrush(HatchStyle.ZigZag, Color.FromArgb(0x0e, 0x2e, 0x39), Color.FromArgb(0x2e, 0x6e, 0x79)), pPath);
                    }
                }
            }

            pDarkBlack.Dispose();

            for (int i = 0; i < m_pWorld.Locations.Length; i++)
            {
                CLostLocation pLoc = m_pWorld.Locations[i];

                if (pLoc.Territory == null)
                    continue;
//                    throw new Exception("null Territory!");

                Brush pBrush;
                switch (pLoc.Territory.Type)
                {
                    case CLostTerritory.TerritoryType.Mountains:
                        pBrush = new HatchBrush(HatchStyle.DiagonalCross, Color.Black, Color.FromArgb(0xbd, 0x6d, 0x46));
                        break;
                    case CLostTerritory.TerritoryType.Desert:
                        pBrush = new HatchBrush(HatchStyle.SmallConfetti, Color.FromArgb(0x8a, 0x6c, 0x16), Color.FromArgb(0xfa, 0xdc, 0x36));
                        break;
                    //case CLostTerritory.TerritoryType.DeepSea:
                    //    pBrush = new HatchBrush(HatchStyle.ZigZag, Color.FromArgb(0x0e, 0x2e, 0x39), Color.FromArgb(0x1e, 0x5e, 0x69));
                    //    break;
                    case CLostTerritory.TerritoryType.Shallows:
                        pBrush = new HatchBrush(HatchStyle.ZigZag, Color.FromArgb(0x0e, 0x2e, 0x39), Color.FromArgb(0x2e, 0x6e, 0x79));
                        break;
                    case CLostTerritory.TerritoryType.Swamp:
                        pBrush = new HatchBrush(HatchStyle.DashedHorizontal, Color.FromArgb(0x57, 0x5d, 0x3b), Color.FromArgb(0xa7, 0xbd, 0x6b));
                        break;
                    case CLostTerritory.TerritoryType.Plains:
                        pBrush = new SolidBrush(Color.FromArgb(0xd3, 0xfa, 0x5f));
                        break;
                    case CLostTerritory.TerritoryType.Forest:
                        pBrush = new HatchBrush(HatchStyle.LargeConfetti, Color.FromArgb(0x26, 0x38, 0x14), Color.FromArgb(0x56, 0x78, 0x34));
                        break;
                    case CLostTerritory.TerritoryType.Hills:
                        pBrush = new HatchBrush(HatchStyle.DashedVertical, Color.FromArgb(0x6d, 0x6d, 0x26), Color.FromArgb(0xcd, 0xdd, 0x56));
                        break;
                    case CLostTerritory.TerritoryType.Wastes:
                        pBrush = new HatchBrush(HatchStyle.Divot, Color.FromArgb(0x80, 0x8f, 0x4a), Color.FromArgb(0xf0, 0xff, 0x8a));
                        break;
                    default:
                        pBrush = new HatchBrush(HatchStyle.DiagonalCross, Color.Black, Color.Red);
                        break;
                }

                if (pLoc.Territory != CLostTerritory.DeepSea)
                {
                    int rX = ActualMap.Width * 4 * CLostWorld.m_iMinDist / (m_iMapScale * 4 * m_pWorld.WorldScale);
                    int rY = ActualMap.Height * 4 * CLostWorld.m_iMinDist / (m_iMapScale * 4 * m_pWorld.WorldScale);
                    gr.FillEllipse(pBrush, ToScreenX(pLoc.X) - rX, ToScreenY(pLoc.Y) - rY, 2 * rX, 2 * rY);
                }

                pBrush.Dispose();
            }

            for (int i = 0; i < m_cVisibleLinks.Count; i++)
            {
                CLostLocation pLoc1 = m_cVisibleLinks[i].m_pLocation1 as CLostLocation;
                CLostLocation pLoc2 = m_cVisibleLinks[i].m_pLocation2 as CLostLocation;

                Pen pen = new Pen(Color.DimGray, 2);
                if (!m_cTraversed.Contains(m_cVisibleLinks[i]))
                {
                    pen.Color = Color.LightGray;
                    pen.DashStyle = DashStyle.Dash;
                }
                //if (pLoc1.Owner != null && pLoc1.Owner == pLoc2.Owner)
                //    pen = m_cColors[pLoc1.Owner];
                gr.DrawLine(pen, ToScreenX(pLoc1.X), ToScreenY(pLoc1.Y),
                    ToScreenX(pLoc2.X), ToScreenY(pLoc2.Y));
            }

            for (int i = 0; i < m_cVisibleLocations.Count; i++)
            {
                CLostLocation pLoc = m_cVisibleLocations[i];
                long r = 1;
                if (pLoc.Settlement != null)
                {
                    switch (pLoc.Settlement.Size)
                    {
                        case SettlementSize.Small:
                            r = 2;
                            break;
                        case SettlementSize.Big:
                            r = 6;
                            break;
                    }
                }
                if (pLoc.Settlement != null)
                    gr.FillEllipse(new SolidBrush(m_cColors[pLoc.Settlement.Owner]), ToScreenX(pLoc.X) - r, ToScreenY(pLoc.Y) - r, 2 * r, 2 * r);
                else
                    gr.FillEllipse(Brushes.DimGray, ToScreenX(pLoc.X) - r, ToScreenY(pLoc.Y) - r, 2 * r, 2 * r);
            }

            panel1.Refresh();
        }

        private void Generate_Click(object sender, EventArgs e)
        {
            //if (!checkBox1.Checked)
            //    numericUpDown1.Value = Rnd.GetNewSeed();

            //Rnd.SetSeed((int)numericUpDown1.Value);
            Rnd.SetSeed(Rnd.GetNewSeed());

            //Properties.Settings.Default.Seed = (int)numericUpDown1.Value;
            //Properties.Settings.Default.Save();

            m_pWorld = new CLostWorld(250);

            m_fDaysPassed = 0;

            m_cColors.Clear();
            m_cColors[m_pWorld.FactionExplorers] = Color.Navy;
            m_cColors[m_pWorld.FactionOthers] = Color.Azure;
            m_cColors[m_pWorld.FactionShadows] = Color.Red;

            m_cTraversed.Clear();
            m_cVisibleLinks.Clear();
            m_cVisibleLocations.Clear();

            m_pCurrentLocation = null;
            Dictionary<CLostLocation, float> cPossible = new Dictionary<CLostLocation, float>();
            foreach (CLostLocation pLoc in m_pWorld.Locations)
            {
                if (pLoc.Type == LocationType.Forbidden)
                    continue;

                int iDist = 1;// (pLoc.X - m_pTargetLocation.X) * (pLoc.X - m_pTargetLocation.X) + (pLoc.Y - m_pTargetLocation.Y) * (pLoc.Y - m_pTargetLocation.Y);
                //iDist = (int)Math.Sqrt(iDist);
                //iDist /= pLoc.Owner.m_iInfrastructureLevel + 1;
                cPossible[pLoc] = iDist;
            }

            if (cPossible.Count > 0)
            {
                int iIndex = Rnd.ChooseOne(cPossible.Values, 3);
                SetCurrentLocation(cPossible.Keys.ElementAt(iIndex));
            }
        }
        private void SetCurrentLocation(CLostLocation pLocation)
        {
            m_pCurrentLocation = pLocation;

            if (!m_cVisibleLocations.Contains(m_pCurrentLocation))
                m_cVisibleLocations.Add(m_pCurrentLocation);

            foreach (var pLink in m_pCurrentLocation.Links)
            {
                if (!m_cVisibleLocations.Contains(pLink.Key))
                    m_cVisibleLocations.Add(pLink.Key as CLostLocation);

                if (!m_cVisibleLinks.Contains(pLink.Value))
                    m_cVisibleLinks.Add(pLink.Value);

                //foreach (var pLink2 in pLink.Key.Links)
                //{
                //    if (!m_cVisibleLinks.Contains(pLink2.Value))
                //        m_cVisibleLinks.Add(pLink2.Value);
                //}
            }

            label1.Text = string.Format("Days passed: {0}", (int)m_fDaysPassed);

            //ShowLocationInfo(m_pCurrentLocation);
            //ShowStateInfo(m_pCurrentLocation.Owner);

            DrawMap();
        }
        private readonly Pen m_pPointerPen = new Pen(Color.Gray, 20);
        private readonly Pen m_pCompassBorderPen = new Pen(Color.DarkGray, 3);

        double m_fScroll = 0;
        readonly double m_fScrollSpeed = 5;//1.5;

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                if (m_pWorld == null || m_pCurrentLocation == null)
                    return;

                int iX = ToScreenX(m_pCurrentLocation.X);
                int iY = ToScreenY(m_pCurrentLocation.Y);

                if (m_pNextLocation != null)
                {
                    iX += (int)((ToScreenX(m_pNextLocation.X) - ToScreenX(m_pCurrentLocation.X)) * m_fScroll);
                    iY += (int)((ToScreenY(m_pNextLocation.Y) - ToScreenY(m_pCurrentLocation.Y)) * m_fScroll);
                }

                GraphicsPath excludePath2 = new GraphicsPath();
                excludePath2.AddEllipse(10, 10, ActualMap.Width - 20, ActualMap.Height - 20);
                excludePath2.Reverse();
                excludePath2.AddRectangle(new Rectangle(0, 0, ActualMap.Width, ActualMap.Height));

                Region excludeRegion2 = new Region(excludePath2);

                // Set clipping region to exclude region.
                e.Graphics.ExcludeClip(excludeRegion2);
                //if (m_pTargetLocation == m_pCurrentLocation)
                //    e.Graphics.FillRectangle(new SolidBrush(m_pPointerPen.Color), panel1.ClientRectangle);
                //else
                    e.Graphics.FillRectangle(Brushes.Silver, panel1.ClientRectangle);

                //if (m_pTargetLocation != null &&
                //    m_pTargetLocation != m_pCurrentLocation)
                //{
                //    int iXTarget = ScaleX(m_pTargetLocation.X) - iX;
                //    int iYTarget = ScaleY(m_pTargetLocation.Y) - iY;

                //    double fDist = Math.Sqrt(iXTarget * iXTarget + iYTarget * iYTarget);

                //    iXTarget = (int)(iXTarget * panel1.Width / fDist);
                //    iYTarget = (int)(iYTarget * panel1.Height / fDist);

                //    e.Graphics.DrawLine(m_pPointerPen, panel1.Width / 2, panel1.Height / 2, panel1.Width / 2 + iXTarget, panel1.Height / 2 + iYTarget);
                //}
                e.Graphics.DrawEllipse(m_pCompassBorderPen, 11, 11, ActualMap.Width - 22, ActualMap.Height - 22);
                e.Graphics.DrawEllipse(m_pCompassBorderPen, 19, 19, ActualMap.Width - 38, ActualMap.Height - 38);

                GraphicsPath excludePath = new GraphicsPath();
                excludePath.AddEllipse(20, 20, ActualMap.Width - 40, ActualMap.Height - 40);
                excludePath.Reverse();
                excludePath.AddRectangle(new Rectangle(0, 0, ActualMap.Width, ActualMap.Height));

                Region excludeRegion = new Region(excludePath);

                // Set clipping region to exclude region.
                e.Graphics.ExcludeClip(excludeRegion);
                //e.Graphics.DrawImage(ActualMap, 0, 0);

                e.Graphics.DrawImage(ActualMap, new Rectangle(0, 0, panel1.Width, panel1.Height),
                    new Rectangle(iX - ActualMap.Width / 10, iY - ActualMap.Height / 10, ActualMap.Width / 5, ActualMap.Height / 5), GraphicsUnit.Pixel);
            }
            catch (Exception ex)
            {
                Text = ex.Message;
            }
        }

        private void Panel1_Resize(object sender, EventArgs e)
        {
            ActualMap = new Bitmap(panel1.Width, panel1.Height);
            DrawMap();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            m_fScroll += m_fScrollSpeed / m_pWalking.Cost;//0.05;
            m_fDaysPassed += m_fScrollSpeed * m_fTimeModifer / m_pWorld.WorldScale;
            label1.Text = string.Format("Days passed: {0}", (int)m_fDaysPassed);

            if (m_fScroll > 1)
            {
                timer1.Enabled = false;

                if (!m_cTraversed.Contains(m_pWalking))
                    m_cTraversed.Add(m_pWalking);

                //m_fDaysPassed += m_fTimeModifer * m_pWalking.Cost / m_pWorld.WorldScale;

                SetCurrentLocation(m_pNextLocation);

                m_pNextLocation = null;
                m_pWalking = null;
                return;
            }

            panel1.Refresh();
        }

        /// <summary>
        /// Прверяет, попадает ли указатель мыши в указанную локацию на карте
        /// </summary>
        /// <param name="pLocation"></param>
        /// <param name="iMouseX"></param>
        /// <param name="iMouseY"></param>
        /// <returns></returns>
        private bool CheckLocation(CLostLocation pLocation, int iMouseX, int iMouseY)
        {
            GraphicsPath pPath = new GraphicsPath();
            foreach (var pEdge in pLocation.m_cEdges)
            {
                pPath.AddPolygon(new Point[] {new Point(ToScreenX(pEdge.Value.m_pFrom.Circumcenter.X), ToScreenY(pEdge.Value.m_pFrom.Circumcenter.Y)),
                                                                                        new Point(ToScreenX(pEdge.Value.m_pMidPoint.Circumcenter.X), ToScreenY(pEdge.Value.m_pMidPoint.Circumcenter.Y)),
                                                                                        new Point(ToScreenX(pEdge.Value.m_pTo.Circumcenter.X), ToScreenY(pEdge.Value.m_pTo.Circumcenter.Y)),
                                                                                        new Point(ToScreenX(pLocation.X), ToScreenY(pLocation.Y))});
            }

            int iX = ToScreenX(m_pCurrentLocation.X) - ActualMap.Width / 10;
            int iY = ToScreenY(m_pCurrentLocation.Y) - ActualMap.Height / 10;

            int iScaledMouseX = iX + iMouseX / m_iMapScale;
            int iScaledMouseY = iY + iMouseY / m_iMapScale;

            bool bResult;
            using (Region pRegion = new Region(pPath))
            {
                bResult = pRegion.IsVisible(iScaledMouseX, iScaledMouseY);
            }
            return bResult;
        }

        private CLostLocation m_pFocusedLocation = null;

        private void Panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_pCurrentLocation == null)
                return;

            if (m_pFocusedLocation != null && CheckLocation(m_pFocusedLocation, e.X, e.Y))
                return;

            if (CheckLocation(m_pCurrentLocation, e.X, e.Y))
            {
                m_pFocusedLocation = m_pCurrentLocation;
                panel1.Cursor = Cursors.No;
                //toolTip1.SetToolTip(panel1, string.Format("{0} - you are here", m_pFocusedLocation.ShortName));
                return;
            }

            CLostLocation pNewFocused = null;

            foreach (var pLink in m_pCurrentLocation.Links)
            {
                if (CheckLocation(pLink.Key as CLostLocation, e.X, e.Y))
                {
                    pNewFocused = pLink.Key as CLostLocation;
                    break;
                }
            }

            if (pNewFocused != m_pFocusedLocation)
            {
                m_pFocusedLocation = pNewFocused;
                if (m_pFocusedLocation != null)
                {
                    panel1.Cursor = Cursors.Hand;
                    //toolTip1.SetToolTip(panel1, string.Format("{0} - {1} days", m_pFocusedLocation.ShortName,
                    //        (int)(m_fTimeModifer * m_pCurrentLocation.Links[m_pFocusedLocation].Cost / m_pWorld.WorldScale)));
                }
                else
                {
                    panel1.Cursor = Cursors.No;
                    //toolTip1.SetToolTip(panel1, "No way!");
                }
            }
        }

        private void Panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (m_pCurrentLocation == null)
                return;

            foreach (var pLink in m_pCurrentLocation.Links)
            {
                if (CheckLocation(pLink.Key as CLostLocation, e.X, e.Y))
                {
                    m_pNextLocation = pLink.Key as CLostLocation;
                    m_pWalking = pLink.Value;
                    m_fScroll = 0;
                    timer1.Enabled = true;
                    break;
                }
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
    }
}
