using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Random;
using System.Drawing.Drawing2D;
using System.Reflection;
using RB.Socium.Psichology;
using RB.Socium;
using RB.Geography;

namespace RB
{
    public partial class Form1 : Form
    {
        CWorld m_pWorld = null;
        CLocation m_pCurrentLocation = null;
        CLocation m_pNextLocation = null;
        CLink m_pWalking = null;
        double m_fDaysPassed;

        /// <summary>
        /// Сколько дней требуется, чтобы пересечь половину карты пешком по равнине
        /// </summary>
        const double m_fTimeModifer = 14;

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

            numericUpDown1.Maximum = int.MaxValue;
            numericUpDown1.Value = Properties.Settings.Default.Seed;
        }


        int ScaleX(double x)
        {
            if (m_pWorld == null)
                return 0;

            return ActualMap.Width / 10 + ActualMap.Width * 4 * (m_pWorld.WorldScale + (int)x) / (5 * 2 * m_pWorld.WorldScale);
        }

        int ScaleY(double y)
        {
            if (m_pWorld == null)
                return 0;

            return ActualMap.Height / 10 + ActualMap.Height * 4 * (m_pWorld.WorldScale + (int)y) / (5 * 2 * m_pWorld.WorldScale);
        }

        private Dictionary<CState, Color> m_cColors = new Dictionary<CState, Color>();
        
        Bitmap ActualMap;

        private List<CLink> m_cTraversed = new List<CLink>();
        private List<CLink> m_cVisibleLinks = new List<CLink>();
        private List<CLocation> m_cVisibleLocations = new List<CLocation>();

        public void DrawMap()
        {
            Graphics gr = Graphics.FromImage(ActualMap);

            Brush fill = new SolidBrush(Color.FromArgb(0xd3, 0xfa, 0x5f));//Brushes.Black;
            gr.FillRectangle(fill, 0, 0, ActualMap.Width, ActualMap.Height);

            if (m_pWorld == null)
                return;

            Pen pDarkBlack = new Pen(Color.FromArgb(30,30,30), 2);

            for (int i = 0; i < m_pWorld.Locations.Length; i++)
            {
                CLocation pLoc = m_pWorld.Locations[i];
                //long r = CWorld.m_iMinDist/2;
                //gr.DrawEllipse(pDarkBlack, pWorld.WorldScale + pLoc.X - r, pWorld.WorldScale + pLoc.Y - r, 2 * r, 2 * r);

                GraphicsPath pPath = new GraphicsPath();
                foreach (var pEdge in pLoc.m_cEdges)
                {
                    pPath.AddPolygon(new Point[] {new Point(ScaleX(pEdge.Value.m_pFrom.Circumcenter.X), ScaleY(pEdge.Value.m_pFrom.Circumcenter.Y)),
                                                                                        new Point(ScaleX(pEdge.Value.m_pMidPoint.Circumcenter.X), ScaleY(pEdge.Value.m_pMidPoint.Circumcenter.Y)),
                                                                                        new Point(ScaleX(pEdge.Value.m_pTo.Circumcenter.X), ScaleY(pEdge.Value.m_pTo.Circumcenter.Y)),
                                                                                        new Point(ScaleX(pLoc.X), ScaleY(pLoc.Y))});
                }
                if (pLoc.Territory == Territory.DeadSea)
                {
                    gr.FillPath(new HatchBrush(HatchStyle.ZigZag, Color.Black, Color.FromArgb(0x1e, 0x5e, 0x69)), pPath);
                }
            }

            for (int i = 0; i < m_pWorld.Locations.Length; i++)
            {
                CLocation pLoc = m_pWorld.Locations[i];

                Brush pBrush = new HatchBrush(HatchStyle.DiagonalCross, Color.Black, Color.Red);
                switch (pLoc.Territory.Type)
                {
                    case Territory.TerritoryType.DeadMountains:
                        pBrush = new HatchBrush(HatchStyle.DiagonalCross, Color.Black, Color.FromArgb(0xbd, 0x6d, 0x46));
                        break;
                    case Territory.TerritoryType.DeadDesert:
                        pBrush = new HatchBrush(HatchStyle.SmallConfetti, Color.FromArgb(0x8a, 0x6c, 0x16), Color.FromArgb(0xfa, 0xdc, 0x36));
                        break;
                    case Territory.TerritoryType.DeadSea:
                        pBrush = new HatchBrush(HatchStyle.ZigZag, Color.FromArgb(0x0e, 0x2e, 0x39), Color.FromArgb(0x1e, 0x5e, 0x69));
                        break;
                    case Territory.TerritoryType.DeadSwamp:
                        pBrush = new HatchBrush(HatchStyle.DashedHorizontal, Color.FromArgb(0x57, 0x5d, 0x3b), Color.FromArgb(0xa7, 0xbd, 0x6b));
                        break;
                    case Territory.TerritoryType.Plains:
                        pBrush = new SolidBrush(Color.FromArgb(0xd3, 0xfa, 0x5f));
                        break;
                    case Territory.TerritoryType.Forest:
                        pBrush = new HatchBrush(HatchStyle.LargeConfetti, Color.FromArgb(0x26, 0x38, 0x14), Color.FromArgb(0x56, 0x78, 0x34));
                        break;
                    case Territory.TerritoryType.Hills:
                        pBrush = new HatchBrush(HatchStyle.DashedVertical, Color.FromArgb(0x6d, 0x6d, 0x26), Color.FromArgb(0xcd, 0xdd, 0x56));
                        break;
                    case Territory.TerritoryType.Wastes:
                        pBrush = new HatchBrush(HatchStyle.Divot, Color.FromArgb(0x80, 0x8f, 0x4a), Color.FromArgb(0xf0, 0xff, 0x8a));
                        break;
                }

                if (pLoc.Territory != Territory.Undefined && pLoc.Territory != Territory.DeadSea)
                {
                    int rX = ActualMap.Width * 4 * CWorld.m_iMinDist / (5 * 4 * m_pWorld.WorldScale);
                    int rY = ActualMap.Height * 4 * CWorld.m_iMinDist / (5 * 4 * m_pWorld.WorldScale);
                    gr.FillEllipse(pBrush, ScaleX(pLoc.X) - rX, ScaleY(pLoc.Y) - rY, 2 * rX, 2 * rY);
                }
            }

            for (int i = 0; i < m_cVisibleLinks.Count; i++)
            {
                CLocation pLoc1 = m_cVisibleLinks[i].m_pLocation1;
                CLocation pLoc2 = m_cVisibleLinks[i].m_pLocation2;

                Pen pen = new Pen(Color.DimGray, 2);
                if (!m_cTraversed.Contains(m_cVisibleLinks[i]))
                {
                    pen.Color = Color.LightGray;
                    pen.DashStyle = DashStyle.Dash;
                }
                //if (pLoc1.Owner != null && pLoc1.Owner == pLoc2.Owner)
                //    pen = m_cColors[pLoc1.Owner];
                gr.DrawLine(pen, ScaleX(pLoc1.X), ScaleY(pLoc1.Y),
                    ScaleX(pLoc2.X), ScaleY(pLoc2.Y));
            }

            for (int i = 0; i < m_cVisibleLocations.Count; i++)
            {
                CLocation pLoc = m_cVisibleLocations[i];
                long r = 1;
                if (pLoc.Settlement != null)
                {
                    switch (pLoc.Settlement.m_pInfo.m_eSize)
                    {
                        case SettlementSize.Hamlet:
                            r = 2;
                            break;
                        case SettlementSize.Village:
                            r = 3;
                            break;
                        case SettlementSize.Town:
                            r = 4;
                            break;
                        case SettlementSize.City:
                            r = 5;
                            break;
                        case SettlementSize.Capital:
                            r = 6;
                            break;
                    }
                }
                if (pLoc.Owner != null)
                    gr.FillEllipse(new SolidBrush(m_cColors[pLoc.Owner]), ScaleX(pLoc.X) - r, ScaleY(pLoc.Y) - r, 2 * r, 2 * r);
                //else
                //    gr.FillEllipse(Brushes.DimGray, ScaleX(pLoc.X) - r, ScaleY(pLoc.Y) - r, 2 * r, 2 * r);
            }

            panel1.Refresh();
        }

        public CLocation m_pTargetLocation = null;

        private void Generate_Click(object sender, EventArgs e)
        {
            if (!checkBox1.Checked)
                numericUpDown1.Value = Rnd.GetNewSeed();

            Rnd.SetSeed((int)numericUpDown1.Value);

            Properties.Settings.Default.Seed = (int)numericUpDown1.Value;
            Properties.Settings.Default.Save();

            m_pWorld = new CWorld(250, 3 + Rnd.Get(3));

            m_fDaysPassed = 0;

            m_cColors.Clear();
            for (int i = 0; i < m_pWorld.States.Count; i++)
            {
                switch (Rnd.Get(3))
                {
                    case 0:
                        m_cColors[m_pWorld.States[i]] = Color.FromArgb(255, Rnd.Get(168), Rnd.Get(168));
                        break;
                    case 1:
                        m_cColors[m_pWorld.States[i]] = Color.FromArgb(Rnd.Get(168), 255, Rnd.Get(168));
                        break;
                    case 2:
                        m_cColors[m_pWorld.States[i]] = Color.FromArgb(Rnd.Get(168), Rnd.Get(168), 255);
                        break;
                }
            }

            m_cTraversed.Clear();
            m_cVisibleLinks.Clear();
            m_cVisibleLocations.Clear();

            CPerson pVillain = m_pWorld.m_cFactions[Rnd.Get(m_pWorld.m_cFactions.Count)];
            if (pVillain != null)
                m_pTargetLocation = pVillain.m_pHomeLocation;

            m_pCurrentLocation = null;
            Dictionary<CLocation, float> cPossible = new Dictionary<CLocation, float>();
            foreach(CLocation pLoc in m_pWorld.Locations)
            {
                if(pLoc.Type == LocationType.Forbidden)
                    continue;

                int iDist = (pLoc.X - m_pTargetLocation.X)*(pLoc.X - m_pTargetLocation.X) + (pLoc.Y - m_pTargetLocation.Y)*(pLoc.Y - m_pTargetLocation.Y);
                iDist = (int)Math.Sqrt(iDist);
                iDist /= pLoc.Owner.m_iInfrastructureLevel + 1;
                cPossible[pLoc] = iDist;
            }

            if (cPossible.Count > 0)
            {
                int iIndex = Rnd.ChooseOne(cPossible.Values, 3);
                SetCurrentLocation(cPossible.Keys.ElementAt(iIndex));
            }
        }

        private void SetCurrentLocation(CLocation pLocation)
        {
            m_pCurrentLocation = pLocation;

            if (!m_cVisibleLocations.Contains(m_pCurrentLocation))
                m_cVisibleLocations.Add(m_pCurrentLocation);

            foreach (var pLink in m_pCurrentLocation.Links)
            {
                if (!m_cVisibleLocations.Contains(pLink.Key))
                    m_cVisibleLocations.Add(pLink.Key);

                if (!m_cVisibleLinks.Contains(pLink.Value))
                    m_cVisibleLinks.Add(pLink.Value);

                //foreach (var pLink2 in pLink.Key.Links)
                //{
                //    if (!m_cVisibleLinks.Contains(pLink2.Value))
                //        m_cVisibleLinks.Add(pLink2.Value);
                //}
            }

            label1.Text = string.Format("Days passed: {0}", (int)m_fDaysPassed);

            ShowLocationInfo(m_pCurrentLocation);
            ShowStateInfo(m_pCurrentLocation.Owner);

            DrawMap();
        }

        private Pen m_pPointerPen = new Pen(Color.Gray, 20);
        private Pen m_pCompassBorderPen = new Pen(Color.DarkGray, 3);

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                if (m_pWorld == null || m_pCurrentLocation == null)
                    return;

                int iX = ScaleX(m_pCurrentLocation.X);
                int iY = ScaleY(m_pCurrentLocation.Y);

                if (m_pNextLocation != null)
                {
                    iX += (int)((ScaleX(m_pNextLocation.X) - ScaleX(m_pCurrentLocation.X)) * m_fScroll);
                    iY += (int)((ScaleY(m_pNextLocation.Y) - ScaleY(m_pCurrentLocation.Y)) * m_fScroll);
                }

                GraphicsPath excludePath2 = new GraphicsPath();
                excludePath2.AddEllipse(10, 10, ActualMap.Width - 20, ActualMap.Height - 20);
                excludePath2.Reverse();
                excludePath2.AddRectangle(new Rectangle(0, 0, ActualMap.Width, ActualMap.Height));

                Region excludeRegion2 = new Region(excludePath2);

                // Set clipping region to exclude region.
                e.Graphics.ExcludeClip(excludeRegion2);
                if (m_pTargetLocation == m_pCurrentLocation)
                    e.Graphics.FillRectangle(new SolidBrush(m_pPointerPen.Color), panel1.ClientRectangle);
                else
                    e.Graphics.FillRectangle(Brushes.Silver, panel1.ClientRectangle);

                if (m_pTargetLocation != null &&
                    m_pTargetLocation != m_pCurrentLocation)
                {
                    int iXTarget = ScaleX(m_pTargetLocation.X) - iX;
                    int iYTarget = ScaleY(m_pTargetLocation.Y) - iY;

                    double fDist = Math.Sqrt(iXTarget * iXTarget + iYTarget * iYTarget);

                    iXTarget = (int)(iXTarget * panel1.Width / fDist);
                    iYTarget = (int)(iYTarget * panel1.Height / fDist);

                    e.Graphics.DrawLine(m_pPointerPen, panel1.Width / 2, panel1.Height / 2, panel1.Width / 2 + iXTarget, panel1.Height / 2 + iYTarget);
                }
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

        private void panel1_Resize(object sender, EventArgs e)
        {
            ActualMap = new Bitmap(panel1.Width, panel1.Height);
            DrawMap();
        }

        double m_fScroll = 0;

        double m_fScrollSpeed = 5;//1.5;

        private void timer1_Tick(object sender, EventArgs e)
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
        private bool CheckLocation(CLocation pLocation, int iMouseX, int iMouseY)
        {
            GraphicsPath pPath = new GraphicsPath();
            foreach (var pEdge in pLocation.m_cEdges)
            {
                pPath.AddPolygon(new Point[] {new Point(ScaleX(pEdge.Value.m_pFrom.Circumcenter.X), ScaleY(pEdge.Value.m_pFrom.Circumcenter.Y)),
                                                                                        new Point(ScaleX(pEdge.Value.m_pMidPoint.Circumcenter.X), ScaleY(pEdge.Value.m_pMidPoint.Circumcenter.Y)),
                                                                                        new Point(ScaleX(pEdge.Value.m_pTo.Circumcenter.X), ScaleY(pEdge.Value.m_pTo.Circumcenter.Y)),
                                                                                        new Point(ScaleX(pLocation.X), ScaleY(pLocation.Y))});
            }

            int iX = ScaleX(m_pCurrentLocation.X) - ActualMap.Width / 10;
            int iY = ScaleY(m_pCurrentLocation.Y) - ActualMap.Height / 10;

            int iScaledMouseX = iX + iMouseX * ActualMap.Width / (5 * panel1.Width);
            int iScaledMouseY = iY + iMouseY * ActualMap.Height / (5 * panel1.Height);

            Region pRegion = new Region(pPath);

            return pRegion.IsVisible(iScaledMouseX, iScaledMouseY);
        }

        private CLocation m_pFocusedLocation = null;

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if(m_pCurrentLocation == null)
                return;

            if (m_pFocusedLocation != null && CheckLocation(m_pFocusedLocation, e.X, e.Y))
                return;

            if (CheckLocation(m_pCurrentLocation, e.X, e.Y))
            {
                m_pFocusedLocation = m_pCurrentLocation;
                panel1.Cursor = Cursors.No;
                toolTip1.SetToolTip(panel1, string.Format("{0} - you are here", m_pFocusedLocation.ShortName));
                return;
            }

            CLocation pNewFocused = null;

            foreach(var pLink in m_pCurrentLocation.Links)
            {
                if (CheckLocation(pLink.Key, e.X, e.Y))
                {
                    pNewFocused = pLink.Key;
                    break;
                }
            }

            if (pNewFocused != m_pFocusedLocation)
            {
                m_pFocusedLocation = pNewFocused;
                if(m_pFocusedLocation != null)
                {
                    panel1.Cursor = Cursors.Hand;
                    toolTip1.SetToolTip(panel1, string.Format("{0} - {1} days", m_pFocusedLocation.ShortName,
                            (int)(m_fTimeModifer * m_pCurrentLocation.Links[m_pFocusedLocation].Cost / m_pWorld.WorldScale)));
                }
                else
                {
                    panel1.Cursor = Cursors.No;
                    toolTip1.SetToolTip(panel1, "No way!");
                }
            }
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (m_pCurrentLocation == null)
                return;

            foreach (var pLink in m_pCurrentLocation.Links)
            {
                if (CheckLocation(pLink.Key, e.X, e.Y))
                {
                    m_pNextLocation = pLink.Key;
                    m_pWalking = pLink.Value;
                    m_fScroll = 0;
                    timer1.Enabled = true;
                    break;
                }
            }
        }

        private void ShowLocationInfo(CLocation pLocation)
        {
            treeView1.Nodes.Clear();
            
            richTextBox2.Clear();

            richTextBox2.AppendText(string.Format("{0} ({1})\n\n", pLocation.Territory.m_sName, pLocation.Type));

            if (pLocation.Owner != null)
            {
                richTextBox2.AppendText(string.Format("{0} {1}\n\n", pLocation.Owner.m_sName, pLocation.Owner.m_pInfo.m_sName));
                richTextBox2.AppendText(string.Format("Population: {0} ({1})\n\n", pLocation.Owner.m_pNation.m_sName, 100 / pLocation.GetClaimingCost(pLocation.Owner.m_pNation)));
            }
            else
            {
                richTextBox2.AppendText("Population: none\n\n");
            }

            if (pLocation.Settlement != null)
            {
                richTextBox2.AppendText(string.Format("{0}: {1}\n\n", pLocation.Settlement.m_pInfo.m_sName, pLocation.Settlement.m_sName));

                Dictionary<string, int> cBuildings = new Dictionary<string, int>();
                Dictionary<CStratum, int> cPeople = new Dictionary<CStratum, int>();

                richTextBox2.AppendText("Buildings:");

                if (pLocation.Settlement.m_cBuildings.Count > 0)
                {
                    foreach (Building pBuilding in pLocation.Settlement.m_cBuildings)
                    {
                        int iCount = 0;
                        cBuildings.TryGetValue(pBuilding.ToString(), out iCount);
                        cBuildings[pBuilding.ToString()] = iCount + 1;

                        int iOwnersCount = 1;
                        int iWorkersCount = 0;
                        switch (pBuilding.m_pInfo.m_eSize)
                        {
                            case BuildingSize.Small:
                                iOwnersCount = 1;
                                iWorkersCount = 3;
                                break;
                            case BuildingSize.Medium:
                                iOwnersCount = 3;
                                iWorkersCount = 15;
                                break;
                            case BuildingSize.Large:
                                iOwnersCount = 5;
                                iWorkersCount = 50;
                                break;
                            case BuildingSize.Huge:
                                iOwnersCount = 10;
                                iWorkersCount = 200;
                                break;
                        }

                        CStratum pOwner = pLocation.Owner.GetStratum(pBuilding.m_pInfo.m_pOwner);
                        cPeople.TryGetValue(pOwner, out iCount);
                        cPeople[pOwner] = iCount + iOwnersCount;

                        CStratum pWorkers = pLocation.Owner.GetStratum(pBuilding.m_pInfo.m_pWorkers);
                        cPeople.TryGetValue(pWorkers, out iCount);
                        cPeople[pWorkers] = iCount + iWorkersCount;

                        if (pBuilding.m_cPersons.Count > 0)
                        {
                            TreeNode pBuildingNode = new TreeNode(pBuilding.ToString());
                            foreach (CPerson pPerson in pBuilding.m_cPersons)
                            {
                                TreeNode pPersonNode = new TreeNode(pPerson.ToString());

                                foreach (var pRelative in pPerson.m_cRelations)
                                {
                                    string sRelation = pRelative.Key.WhoAmITo(pPerson);

                                    if (pRelative.Key.m_pHomeLocation == pLocation)
                                        pPersonNode.Nodes.Add(new TreeNode(string.Format("{0} : {1}", sRelation, pRelative.Key.ToString())));
                                    else
                                        pPersonNode.Nodes.Add(new TreeNode(string.Format("({0}) : {1}", sRelation, pRelative.Key.ToString())));
                                }

                                pBuildingNode.Nodes.Add(pPersonNode);
                            }
                            treeView1.Nodes.Add(pBuildingNode);
                        }
                    }
                    treeView1.ExpandAll();

                    foreach (var vBuilding in cBuildings)
                        richTextBox2.AppendText("\n         - " + vBuilding.Key + "  x" + vBuilding.Value.ToString());
                    
                    richTextBox2.AppendText("\n");
                }
                else
                    richTextBox2.AppendText("\n         - none\n");

                richTextBox2.AppendText("\nPeople:");
                if (cPeople.Count > 0)
                {
                    foreach (var pEstate in pLocation.Owner.m_cEstates)
                    {
                        richTextBox2.AppendText("\n     " + pEstate.Value.m_sName + ":");
                        bool bGotIt = false;
                        foreach (CStratum pStratum in pEstate.Value.m_cStratums)
                        {
                            if (cPeople.ContainsKey(pStratum))
                            {
                                richTextBox2.AppendText("\n         - " + pStratum + "  x" + cPeople[pStratum].ToString());
                                bGotIt = true;
                            }
                        }
                        if(!bGotIt)
                            richTextBox2.AppendText("\n         - none");
                    }

                    richTextBox2.AppendText("\n");
                }
                else
                    richTextBox2.AppendText("\n         - none\n");
            }
        }

        private void ShowStateInfo(CState pState)
        {
            richTextBox1.Clear();

            richTextBox1.AppendText(string.Format("{0} {1}\n\n", pState.m_sName, pState.m_pInfo.m_sName));

            richTextBox1.AppendText(string.Format("Major race: {2} [T{0}M{1}]\n\n", pState.m_pNation.m_iTechLevel, pState.m_pNation.m_iMagicLimit, pState.m_pNation));

            richTextBox1.AppendText(string.Format("Social order : {0} [C{1}]\n\n", CState.GetControlString(pState.m_iControl), pState.m_iCultureLevel));

            richTextBox1.AppendText(string.Format("Economic system : {0}\n\n", CState.GetEqualityString(pState.m_iSocialEquality)));

            richTextBox1.AppendText(string.Format("Culture:\n"));
            foreach (Mentality eMentality in Culture.Mentalities)
            {
                richTextBox1.AppendText("   ");
                //richTextBox1.AppendText(string.Format("   {0}: \t", eMorale));
                //if (eMorale.ToString().Length < 6)
                //    richTextBox1.AppendText("\t");
                richTextBox1.AppendText(pState.m_pCulture.GetMentalityString(eMentality, pState.m_iCultureLevel));
                //richTextBox1.AppendText(string.Format("{0:0.00}\n", e.State.m_pCulture.Moral[eMorale]));
                richTextBox1.AppendText("\n");
            }
            richTextBox1.AppendText("\n");

            string sRaceName = pState.m_pNation.m_pRace.m_sName;
            sRaceName = sRaceName.Substring(0, 1).ToUpper() + sRaceName.Substring(1);
            richTextBox1.AppendText(sRaceName + "s " + pState.m_pNation.m_pRace.m_pFenotype.GetDescription());
            richTextBox1.AppendText("\n");
            richTextBox1.AppendText("Known " + pState.m_pNation.m_pRace.m_sName + " nations are: ");
            bool bFirst = true;
            List<CNation> cKnownNations = new List<CNation>();
            foreach (CState pState2 in m_pWorld.States)
            {
                if (pState.m_pNation.m_pRace == pState2.m_pNation.m_pRace && !cKnownNations.Contains(pState2.m_pNation))
                {
                    if (!bFirst)
                        richTextBox1.AppendText(", ");

                    bFirst = false;

                    richTextBox1.AppendText(pState2.m_pNation.m_sName);

                    cKnownNations.Add(pState2.m_pNation);
                }
            }
            richTextBox1.AppendText(".\n\n");

            string sFenotypeNation = pState.m_pNation.m_pFenotype.GetComparsion(pState.m_pNation.m_pRace.m_pFenotype);
            if (!sFenotypeNation.StartsWith("are"))
                sFenotypeNation = "are common " + pState.m_pNation.m_pRace.m_sName + "s, however " + sFenotypeNation.Substring(0, 1).ToLower() + sFenotypeNation.Substring(1);
            richTextBox1.AppendText(pState.m_pNation.m_sName + "s " + sFenotypeNation);
            richTextBox1.AppendText("\n\n");

            richTextBox1.AppendText(pState.m_pCustoms.GetCustomsString2());
            richTextBox1.AppendText("\n");

            if (pState.GetImportedTech() == -1)
                richTextBox1.AppendText(string.Format("Available tech: {0} [T{1}]\n\n", CState.GetTechString(pState.m_iTechLevel, pState.m_pCustoms.m_eProgress), pState.GetEffectiveTech()));
            else
                richTextBox1.AppendText(string.Format("Available tech: {0} [T{1}]\n\n", pState.GetImportedTechString(), pState.GetImportedTech()));
            richTextBox1.AppendText(string.Format("Industrial base: {0} [T{1}]\n\n", CState.GetTechString(pState.m_iTechLevel, pState.m_pCustoms.m_eProgress), pState.GetEffectiveTech()));

            if (pState.m_iMagicLimit > 0)
            {
                string sMagicAttitude = "regulated";
                if (pState.m_pCustoms.m_eMagic == Customs.Magic.Magic_Feared)
                    sMagicAttitude = "outlawed";
                if (pState.m_pCustoms.m_eMagic == Customs.Magic.Magic_Praised)
                    sMagicAttitude = "unlimited";
                richTextBox1.AppendText(string.Format("Magic users: {0}, ", sMagicAttitude));
                richTextBox1.AppendText(string.Format("{2}, up to {0} [M{1}]\n\n", CState.GetMagicString(pState.m_iMagicLimit), pState.m_iMagicLimit, pState.m_eMagicAbilityDistribution.ToString().Replace('_', ' ')));
            }
            else
            {
                string sMagicAttitude = "but allowed";
                if (pState.m_pCustoms.m_eMagic == Customs.Magic.Magic_Feared)
                    sMagicAttitude = "and outlawed";
                if (pState.m_pCustoms.m_eMagic == Customs.Magic.Magic_Praised)
                    sMagicAttitude = "but praised";
                richTextBox1.AppendText(string.Format("Magic users: none, {0} [M0]\n\n", sMagicAttitude));
            }

            richTextBox1.AppendText(string.Format("Resources: F:{0}, W:{1}, I:{2} / P:{3}\n\n", pState.m_fFood, pState.m_fWood, pState.m_fOre, pState.m_iPopulation));

            richTextBox1.AppendText("Estates: \n");
            foreach (var pEstate in pState.m_cEstates)
            {
                if (pEstate.Value.m_cStratums.Count == 0)
                    continue;

                richTextBox1.AppendText("  - " + pEstate.Value.m_sName + " (" + pEstate.Key.ToString() + "): ");

                string sCustoms = pEstate.Value.m_pCustoms.GetCustomsDiffString2(pState.m_pCustoms);
                string sMinorsCustoms = pEstate.Value.m_pMinorsCustoms.GetCustomsDiffString2(pEstate.Value.m_pCustoms);
                if (sCustoms != "")
                    richTextBox1.AppendText("Members of this estate " + sCustoms + ".");
                if (pEstate.Value.IsSegregated() && sMinorsCustoms != "")
                {
                    string sMinors = pEstate.Value.m_pCustoms.m_eGenderPriority == Customs.GenderPriority.Patriarchy ? "females" : "males";
                    if (sCustoms == "")
                        richTextBox1.AppendText("Members of this estate are just a common citizens. Also, their " + sMinors + " commonly " + sMinorsCustoms + ".");
                    else
                        richTextBox1.AppendText(" Also, their " + sMinors + " commonly " + sMinorsCustoms + ".");
                }

                string sCulture = pEstate.Value.m_pCulture.GetMentalityDiffString(pEstate.Value.m_iCultureLevel, pState.m_pCulture, pState.m_iCultureLevel);
                if (sCulture != "")
                {
                    if (sCustoms == "" && sMinorsCustoms == "")
                        richTextBox1.AppendText("Members of this estate are usually " + sCulture + " then common citizens.");
                    else
                        richTextBox1.AppendText(" They are usually " + sCulture + " then common citizens.");
                }

                if (sCulture == "" && sCustoms == "" && sMinorsCustoms == "")
                    richTextBox1.AppendText("Members of this estate are just a common citizens.");

                richTextBox1.AppendText("\n    Prevalent professions:");
                foreach (CStratum pStratum in pEstate.Value.m_cStratums)
                    richTextBox1.AppendText("\n         - " + (pStratum.m_eGenderPriority == Customs.GenderPriority.Matriarchy ? pStratum.m_pProfession.m_sNameF : pStratum.m_pProfession.m_sNameM));
                richTextBox1.AppendText("\n\n");
            }

            //State[] cEnemies = pState.GetEnemiesList();
            //if (cEnemies.Length > 0)
            //{
            //    richTextBox1.AppendText("Enemies: ");
            //    bFirst = true;
            //    foreach (State pState in cEnemies)
            //    {
            //        if (!bFirst)
            //            richTextBox1.AppendText(", ");
            //        richTextBox1.InsertLink(string.Format("{0} {1}", pState.m_sName, pState.m_pInfo.m_sName), comboBox1.Items.IndexOf(pState).ToString());
            //        bFirst = false;
            //    }
            //    richTextBox1.AppendText("\n\n");
            //}
            //else
            //    richTextBox1.AppendText("Enemies: none\n\n");

            //State[] cAllies = pState.GetAlliesList();
            //if (cAllies.Length > 0)
            //{
            //    richTextBox1.AppendText("Allies: ");
            //    bFirst = true;
            //    foreach (State pState in cAllies)
            //    {
            //        if (!bFirst)
            //            richTextBox1.AppendText(", ");
            //        richTextBox1.InsertLink(string.Format("{0} {1}", pState.m_sName, pState.m_pInfo.m_sName), comboBox1.Items.IndexOf(pState).ToString());
            //        bFirst = false;
            //    }
            //    richTextBox1.AppendText("\n\n");
            //}
            //else
            //    richTextBox1.AppendText("Allies: none\n\n");
        }

        private void personsListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 pForm = new Form2(m_pWorld);
            DialogResult eRes = pForm.ShowDialog();
            if (eRes == DialogResult.Retry && pForm.SelectedPerson != null)
            {
                SetCurrentLocation(pForm.SelectedPerson.m_pHomeLocation);
            }
            if (eRes == DialogResult.Yes && pForm.SelectedPerson != null)
            {
                m_pTargetLocation = pForm.SelectedPerson.m_pHomeLocation;

                DrawMap();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown1.Enabled = checkBox1.Checked;
        }

        private void factionsListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 pForm = new Form3(m_pWorld);
            DialogResult eRes = pForm.ShowDialog();
            //if (eRes == DialogResult.Retry && pForm.SelectedPerson != null)
            //{
            //    SetCurrentLocation(pForm.SelectedPerson.m_pHomeLocation);
            //}
            //if (eRes == DialogResult.Yes && pForm.SelectedPerson != null)
            //{
            //    m_pTargetLocation = pForm.SelectedPerson.m_pHomeLocation;

            //    DrawMap();
            //}
        }
    }
}
