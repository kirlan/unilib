using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Random;
using LandscapeGeneration.PathFind;
using LandscapeGeneration;
using Socium;

namespace VQMapTest2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            MapScaleChanged(this, null);
            MapModeChanged(this, null);
            MapLayersChanged(this, null);

            showLandmarksToolStripMenuItem.Checked = true;
            showRoadsToolStripMenuItem.Checked = true;

            worldMap1.AddMiniMapView(worldMap2);
        }

        private World m_pWorld;

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    ///TODO:
        //    ///4. StateInfo
        //    BuildWorld(5000);
        //}

        //private void button2_Click(object sender, EventArgs e)
        //{
        //    BuildWorld(10000);
        //}

        //private void button3_Click(object sender, EventArgs e)
        //{
        //    BuildWorld(25000);
        //}

        //private void button4_Click(object sender, EventArgs e)
        //{
        //    BuildWorld(50000);
        //}

        //private void BuildWorld(int iLocations)
        //{
        //    button1.Enabled = false;
        //    button2.Enabled = false;
        //    button3.Enabled = false;
        //    button4.Enabled = false;

        //    int iEquator = Rnd.Get(200) - 50;
        //    int iPole = Math.Max(100 - iEquator, iEquator);
        //    if(Rnd.OneChanceFrom(2))
        //        iPole = iPole + Rnd.Get(iPole / 2);

        //    //m_pWorld = new World(iLocations, 2000, 100, 20, 30, 40 + Rnd.Get(20), iEquator, iPole);
        //    m_pWorld = new World(iLocations, 3000, 100, 20, 100, 60, 50, 45, 24);
        //    ShowWorld();

        //    button1.Enabled = true;
        //    button2.Enabled = true;
        //    button3.Enabled = true;
        //    button4.Enabled = true;
        //}

        private void ShowWorld()
        {
            float fScale = 1.0f;

            if (radioButton1.Checked)
                fScale = 1.0f;
            if (radioButton2.Checked)
                fScale = 2.0f;
            if (radioButton3.Checked)
                fScale = 4.0f;
            if (radioButton4.Checked)
                fScale = 8.0f;

            worldMap1.ClearPath();
            worldMap1.DrawMap(m_pWorld, fScale);

            label7.Text = string.Format("Avrg. tech level: {0} [T{1}]", State.GetTechString(m_pWorld.m_iTechLevel), m_pWorld.m_iTechLevel);
            label8.Text = string.Format("Psi users: {0}, ", m_pWorld.m_eMagicAbilityPrevalence);
            label2.Text = string.Format("          {2}, up to {0} [B{1}]", State.GetMagicString(m_pWorld.m_iMagicLimit), m_pWorld.m_iMagicLimit, m_pWorld.m_eMagicAbilityDistribution);

            comboBox1.Items.Clear();
            foreach (State pState in m_pWorld.m_aStates)
            {
                comboBox1.Items.Add(pState);
            }
            comboBox1.SelectedIndex = 0;
        }

        private void MapScaleChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
                worldMap1.MapScale = 1;
            if (radioButton2.Checked)
                worldMap1.MapScale = 4;
            if (radioButton3.Checked)
                worldMap1.MapScale = 8;
            if (radioButton4.Checked)
                worldMap1.MapScale = 16;

            comboBox1.Focus();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            MapScaleChanged(sender, e);
        }

        private void MapModeChanged(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    worldMap1.Mode = WorldMap.VisType.LandType;
                    break;
                case 1:
                    worldMap1.Mode = WorldMap.VisType.Humidity;
                    break;
                case 2:
                    worldMap1.Mode = WorldMap.VisType.RacesNative;
                    break;
                case 3:
                    worldMap1.Mode = WorldMap.VisType.RacesStates;
                    break;
                case 4:
                    break;
                case 5:
                    break;
            }

            comboBox1.Focus();
        }

        private void MapLayersChanged(object sender, EventArgs e)
        {
            worldMap1.ShowLocations = showLandmarksToolStripMenuItem.Checked;
            worldMap1.ShowRoads = showRoadsToolStripMenuItem.Checked;
            worldMap1.ShowStates = showStateBordersToolStripMenuItem.Checked;
            worldMap1.ShowProvincies = showProvinciesBordersToolStripMenuItem.Checked;
            worldMap1.ShowLocationsBorders = showLocationsToolStripMenuItem.Checked;
            worldMap1.ShowLands = showLandsToolStripMenuItem.Checked;
            worldMap1.ShowLandMasses = showLandMassesToolStripMenuItem.Checked;

            if (toolStripMenuItem3.Checked != showLandmarksToolStripMenuItem.Checked)
                toolStripMenuItem3.Checked = showLandmarksToolStripMenuItem.Checked;
            if (toolStripMenuItem4.Checked != showRoadsToolStripMenuItem.Checked)
                toolStripMenuItem4.Checked = showRoadsToolStripMenuItem.Checked;
            if (toolStripMenuItem5.Checked != showStateBordersToolStripMenuItem.Checked)
                toolStripMenuItem5.Checked = showStateBordersToolStripMenuItem.Checked;
            if (toolStripMenuItem6.Checked != showProvinciesBordersToolStripMenuItem.Checked)
                toolStripMenuItem6.Checked = showProvinciesBordersToolStripMenuItem.Checked;
            if (toolStripMenuItem8.Checked != showLocationsToolStripMenuItem.Checked)
                toolStripMenuItem8.Checked = showLocationsToolStripMenuItem.Checked;
            if (toolStripMenuItem9.Checked != showLandsToolStripMenuItem.Checked)
                toolStripMenuItem9.Checked = showLandsToolStripMenuItem.Checked;
            if (toolStripMenuItem10.Checked != showLandMassesToolStripMenuItem.Checked)
                toolStripMenuItem10.Checked = showLandMassesToolStripMenuItem.Checked;

            comboBox1.Focus();
        }

        private void MapLayersChanged2(object sender, EventArgs e)
        {
            worldMap1.ShowLocations = toolStripMenuItem3.Checked;
            worldMap1.ShowRoads = toolStripMenuItem4.Checked;
            worldMap1.ShowStates = toolStripMenuItem5.Checked;
            worldMap1.ShowProvincies = toolStripMenuItem6.Checked;
            worldMap1.ShowLocationsBorders = toolStripMenuItem8.Checked;
            worldMap1.ShowLands = toolStripMenuItem9.Checked;
            worldMap1.ShowLandMasses = toolStripMenuItem10.Checked;

            if (showLandmarksToolStripMenuItem.Checked != toolStripMenuItem3.Checked)
                showLandmarksToolStripMenuItem.Checked = toolStripMenuItem3.Checked;
            if (showRoadsToolStripMenuItem.Checked != toolStripMenuItem4.Checked)
                showRoadsToolStripMenuItem.Checked = toolStripMenuItem4.Checked;
            if (showStateBordersToolStripMenuItem.Checked != toolStripMenuItem5.Checked)
                showStateBordersToolStripMenuItem.Checked = toolStripMenuItem5.Checked;
            if (showProvinciesBordersToolStripMenuItem.Checked != toolStripMenuItem6.Checked)
                showProvinciesBordersToolStripMenuItem.Checked = toolStripMenuItem6.Checked;
            if (showLocationsToolStripMenuItem.Checked != toolStripMenuItem8.Checked)
                showLocationsToolStripMenuItem.Checked = toolStripMenuItem8.Checked;
            if (showLandsToolStripMenuItem.Checked != toolStripMenuItem9.Checked)
                showLandsToolStripMenuItem.Checked = toolStripMenuItem9.Checked;
            if (showLandMassesToolStripMenuItem.Checked != toolStripMenuItem10.Checked)
                showLandMassesToolStripMenuItem.Checked = toolStripMenuItem10.Checked;

            comboBox1.Focus();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            worldMap1.SelectedState = comboBox1.SelectedItem as State;
        }

        private void worldMap1_StateSelectedEvent(object sender, WorldMap.StateSelectedEventArgs e)
        {
            comboBox1.SelectedItem = e.State;

            richTextBox1.Clear();

            richTextBox1.AppendText(string.Format("Social order : {0} {1} [C{2}]\n\n", State.GetControlString(e.State.m_iControl), e.State.m_pInfo.m_sName, e.State.m_iInfrastructureLevel));

            richTextBox1.AppendText(string.Format("Major race: {2} [T{0}B{1}]\n\n", e.State.m_pRace.m_iTechLevel, e.State.m_pRace.m_iMagicLimit, e.State.m_pRace.m_sName));

            richTextBox1.AppendText(string.Format("Culture:\n"));
            foreach (Culture.Mentality eMorale in Culture.Mentalities)
            {
                richTextBox1.AppendText("   ");
                //richTextBox1.AppendText(string.Format("   {0}: \t", eMorale));
                //if (eMorale.ToString().Length < 6)
                //    richTextBox1.AppendText("\t");
                if (e.State.m_pCulture.MentalityValues[eMorale] < 0.2)
                    richTextBox1.AppendText("(+5) " + e.State.m_pCulture.GetMentalityString(eMorale));
                else
                    if (e.State.m_pCulture.MentalityValues[eMorale] < 0.4)
                        richTextBox1.AppendText("(+4) " + e.State.m_pCulture.GetMentalityString(eMorale));
                    else
                        if (e.State.m_pCulture.MentalityValues[eMorale] < 0.6)
                            richTextBox1.AppendText("(+3) " + e.State.m_pCulture.GetMentalityString(eMorale));
                        else
                            if (e.State.m_pCulture.MentalityValues[eMorale] < 0.8)
                                richTextBox1.AppendText("(+2) " + e.State.m_pCulture.GetMentalityString(eMorale));
                            else
                                if (e.State.m_pCulture.MentalityValues[eMorale] < 1)
                                    richTextBox1.AppendText("(+1) " + e.State.m_pCulture.GetMentalityString(eMorale));
                                else
                                    if (e.State.m_pCulture.MentalityValues[eMorale] < 1.2)
                                        richTextBox1.AppendText("(-1) " + e.State.m_pCulture.GetMentalityString(eMorale));
                                    else
                                        if (e.State.m_pCulture.MentalityValues[eMorale] < 1.4)
                                            richTextBox1.AppendText("(-2) " + e.State.m_pCulture.GetMentalityString(eMorale));
                                        else
                                            if (e.State.m_pCulture.MentalityValues[eMorale] < 1.6)
                                                richTextBox1.AppendText("(-3) " + e.State.m_pCulture.GetMentalityString(eMorale));
                                            else
                                                if (e.State.m_pCulture.MentalityValues[eMorale] < 1.8)
                                                    richTextBox1.AppendText("(-4) " + e.State.m_pCulture.GetMentalityString(eMorale));
                                                else
                                                    richTextBox1.AppendText("(-5) " + e.State.m_pCulture.GetMentalityString(eMorale));
                //richTextBox1.AppendText(string.Format("{0:0.00}\n", e.State.m_pCulture.Moral[eMorale]));
                richTextBox1.AppendText("\n");
            }
            richTextBox1.AppendText("\n");

            richTextBox1.AppendText(string.Format("Pariahs:\n"));
            richTextBox1.AppendText(string.Format("   {0}\n", State.GetGendersPariahString(e.State.m_eGenderPriority)));
            richTextBox1.AppendText("\n");

            if (e.State.GetImportedTech() == -1)
                richTextBox1.AppendText(string.Format("Used tech: {0} [T{1}]\n\n", State.GetTechString(e.State.m_iTechLevel), e.State.m_iTechLevel));
            else
                richTextBox1.AppendText(string.Format("Used tech: {0} [T{1}]\n\n", State.GetTechString(e.State.GetImportedTech()), e.State.GetImportedTech()));
            richTextBox1.AppendText(string.Format("Industrial base: {0} [T{1}]\n\n", State.GetTechString(e.State.m_iTechLevel), e.State.m_iTechLevel));
            
            richTextBox1.AppendText(string.Format("Psi users: {0}, ", e.State.m_eMagicAbilityPrevalence));
            richTextBox1.AppendText(string.Format("{2}, up to {0} [B{1}]\n\n", State.GetMagicString(e.State.m_iMagicLimit), e.State.m_iMagicLimit, e.State.m_eMagicAbilityDistribution));
            
            richTextBox1.AppendText(string.Format("Resources: F:{0}, W:{1}, I:{2} / P:{3}\n\n", e.State.m_iFood, e.State.m_iWood, e.State.m_iOre, e.State.m_iPopulation));

            State[] cEnemies = e.State.GetEnemiesList();
            if(cEnemies.Length > 0)
            {
                richTextBox1.AppendText("Enemies: ");
                bool bFirst = true;
                foreach (State pState in cEnemies)
                {
                    if(!bFirst)
                        richTextBox1.AppendText(", ");
                    richTextBox1.InsertLink(string.Format("{0} {1}", pState.m_sName, pState.m_pInfo.m_sName), comboBox1.Items.IndexOf(pState).ToString());
                    bFirst = false;
                }
                richTextBox1.AppendText("\n\n");
            }
            else
                richTextBox1.AppendText("Enemies: none\n\n");

            State[] cAllies = e.State.GetAlliesList();
            if (cAllies.Length > 0)
            {
                richTextBox1.AppendText("Allies: ");
                bool bFirst = true;
                foreach (State pState in cAllies)
                {
                    if (!bFirst)
                        richTextBox1.AppendText(", ");
                    richTextBox1.InsertLink(string.Format("{0} {1}", pState.m_sName, pState.m_pInfo.m_sName), comboBox1.Items.IndexOf(pState).ToString());
                    bFirst = false;
                }
                richTextBox1.AppendText("\n\n");
            }
            else
                richTextBox1.AppendText("Allies: none\n\n");

            listBox1.Items.Clear();

            foreach (LocationX pLoc in e.State.m_cSettlements)
                listBox1.Items.Add(pLoc);

            comboBox1.Focus();
            //worldMap1.SetPan(e.m_iX - worldMap1.ClientRectangle.Width / 2, e.m_iY - worldMap1.ClientRectangle.Height / 2);
        }
        
        //public delegate void AutoScrollPositionDelegate(ScrollableControl ctrl, Point p);

        ///// <summary>
        ///// Специальный хитрый механизм для того, чтобы получив фокус, карта мира не скроллилась к своему началу.
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void worldMap1_Enter(object sender, EventArgs e)
        //{
        //    Point p = panel1.AutoScrollPosition;
        //    AutoScrollPositionDelegate del = new AutoScrollPositionDelegate(SetAutoScrollPosition);
        //    Object[] args = { panel1, p };
        //    BeginInvoke(del, args);
        //}
        //private void SetAutoScrollPosition(ScrollableControl sender, Point p)
        //{
        //    p.X = Math.Abs(p.X);
        //    p.Y = Math.Abs(p.Y);
        //    sender.AutoScrollPosition = p;
        //}

        private bool m_bScrolling1 = false;
        private Point m_pLastMouseLocation1 = new Point(0, 0);

        private bool m_bScrolling2 = false;
        private Point m_pLastMouseLocation2 = new Point(0, 0);

        private void worldMap1_MouseDown(object sender, MouseEventArgs e)
        {
            m_bScrolling1 = true;
            m_pLastMouseLocation1 = new Point(-1, -1);//new Point(e.X + panel1.AutoScrollPosition.X,
                //e.Y + panel1.AutoScrollPosition.Y);
        }

        private void worldMap1_MouseUp(object sender, MouseEventArgs e)
        {
            m_bScrolling1 = false;
            comboBox1.Focus();
        }

        private void worldMap1_MouseLeave(object sender, EventArgs e)
        {
            m_bScrolling1 = false;
            comboBox1.Focus();
        }

        private void worldMap1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!m_bScrolling1)
                return;

            Point p = worldMap1.DrawFrame.Location;

            //Point realPos = new Point(e.X + panel1.AutoScrollPosition.X,
            //    e.Y + panel1.AutoScrollPosition.Y);

            if (m_pLastMouseLocation1.X > 0)
            {
                p.X -= e.X - m_pLastMouseLocation1.X;
                p.Y -= e.Y - m_pLastMouseLocation1.Y;
            }
            m_pLastMouseLocation1 = e.Location;

            worldMap1.SetPan(p.X, p.Y);
            //panel1.Refresh();
        }

        private GenerationForm m_pGenerationForm = new GenerationForm();

        private void ToolStripMenuItem_New_Click(object sender, EventArgs e)
        {
            if (m_pGenerationForm.ShowDialog() == DialogResult.OK)
            {
                m_pWorld = m_pGenerationForm.World;
                label1.Visible = false;
                ShowWorld();
            }
        }

        private void ToolStripMenuItem_Exit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void worldMap2_MouseDown(object sender, MouseEventArgs e)
        {
            m_bScrolling2 = true;
            m_pLastMouseLocation2 = new Point(-1, -1);//new Point(e.X + panel1.AutoScrollPosition.X,
        }

        private void worldMap2_MouseLeave(object sender, EventArgs e)
        {
            m_bScrolling2 = false;
            comboBox1.Focus();
        }

        private void worldMap2_MouseMove(object sender, MouseEventArgs e)
        {
            if (!m_bScrolling2)
                return;

            Point p = worldMap2.DrawFrame.Location;

            //Point realPos = new Point(e.X + panel1.AutoScrollPosition.X,
            //    e.Y + panel1.AutoScrollPosition.Y);

            if (m_pLastMouseLocation2.X > 0)
            {
                p.X += e.X - m_pLastMouseLocation2.X;
                p.Y += e.Y - m_pLastMouseLocation2.Y;
            }
            m_pLastMouseLocation2 = e.Location;

            worldMap2.SetPan(p.X, p.Y);
        }

        private void worldMap2_MouseUp(object sender, MouseEventArgs e)
        {
            m_bScrolling2 = false;
            comboBox1.Focus();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!m_pGenerationForm.Preload())
                Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Point pPoint = worldMap1.GetCentralPoint(worldMap1.SelectedState);

            //Получаем новые координаты для DrawFrame, чтобы выбранное государство было в центре
            worldMap1.SetPan(pPoint.X - worldMap1.DrawFrame.Width / 2, pPoint.Y - worldMap1.DrawFrame.Height / 2);

            comboBox1.Focus();
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
                button1_Click(sender, null);
        }

        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox1.Focus();
        }

        private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            int iIndex = int.Parse(e.LinkText.Substring(e.LinkText.IndexOf('#')+1));
            worldMap1.SelectedState = comboBox1.Items[iIndex] as State;
        }

        private int Str2Int(string sStr)
        {
            int iRes = -1;

            while (sStr.Length > 0 && !char.IsDigit(sStr[0]))
            {
                sStr = sStr.Remove(0, 1);
            }


            while (!int.TryParse(sStr, out iRes) && sStr.Length > 1)
            {
                sStr = sStr.Substring(0, sStr.Length - 1);
            }

            return iRes;
        }

        int iToolTipedState = -1;

        private void richTextBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (timer1.Enabled)
                return;

            if (worldMap1.SelectedState == null)
                return;

            string sLink = richTextBox1.GetLink(e.Location);
            if (sLink != "")
            {
                int iPos = sLink.IndexOf('#');
                if (iPos != -1)
                {
                    int iIndex = Str2Int(sLink.Substring(iPos + 1));
                    if (iIndex < 0)
                        return;

                    if (iIndex != iToolTipedState)
                    {
                        iToolTipedState = iIndex;

                        State pLinkState = comboBox1.Items[iIndex] as State;

                        string sTip = string.Format("{0} {1} to {2} {3}:\n", worldMap1.SelectedState.m_sName, worldMap1.SelectedState.m_pInfo.m_sName, pLinkState.m_sName, pLinkState.m_pInfo.m_sName);
                        string sRelation;
                        worldMap1.SelectedState.CalcHostility(pLinkState, out sRelation);

                        sTip += sRelation;
                        sTip += "\n";

                        sTip += string.Format("{2} {3} to {0} {1}:\n", worldMap1.SelectedState.m_sName, worldMap1.SelectedState.m_pInfo.m_sName, pLinkState.m_sName, pLinkState.m_pInfo.m_sName);
                        pLinkState.CalcHostility(worldMap1.SelectedState, out sRelation);
                        sTip += sRelation;

                        toolTip1.SetToolTip(richTextBox1, sTip);
                        toolTip1.Active = true;
                    }
                }
            }
            else
            {
                toolTip1.Active = false;
                iToolTipedState = -1;
            }

            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
        }

        LocationX m_pTPFStart = null;
        LocationX m_pTPFFinish = null;

        private void ToolStripMenuItem_TestPathFinding1_Click(object sender, EventArgs e)
        {
            do
            {
                m_pTPFStart = m_pWorld.m_cGrid.m_aLocations[Rnd.Get(m_pWorld.m_cGrid.m_aLocations.Length)];
            }
            while (m_pTPFStart.Forbidden || (m_pTPFStart.Owner as LandX).IsWater);

            do
            {
                m_pTPFFinish = m_pWorld.m_cGrid.m_aLocations[Rnd.Get(m_pWorld.m_cGrid.m_aLocations.Length)];
            }
            while (m_pTPFFinish.Forbidden || m_pTPFFinish == m_pTPFStart || (m_pTPFFinish.Owner as LandX).IsWater);

            worldMap1.ClearPath();

            //DateTime pTime1 = DateTime.Now;
            //PathFinder pAltPath = new PathFinder(m_pTPFStart, m_pTPFFinish, m_pWorld.m_cGrid.CycleShift, -1, false);
            //worldMap1.AddPath(pAltPath.m_cPath, Color.Purple);
            //DateTime pTime2 = DateTime.Now;

            //m_iPassword++;
            //PathFinder pLMPath = new PathFinder((m_pTPFStart.Owner as LandX).Owner as LandMass<LandX>, (m_pTPFFinish.Owner as LandX).Owner as LandMass<LandX>, m_pWorld.m_cGrid.CycleShift, -1, false);
            //foreach (TransportationNode pNode in pLMPath.m_cPath)
            //{
            //    LandMass<LandX> pLandMass = pNode as LandMass<LandX>;
            //    foreach (LandX pLand in pLandMass.m_cContents)
            //    {
            //        pLand.m_iPassword = m_iPassword;
            //    }
            //    foreach (ITerritory pTerr in pLandMass.m_aBorderWith)
            //    {
            //        if (!pTerr.Forbidden)
            //        {
            //            LandMass<LandX> pLinkedLandMass = pTerr as LandMass<LandX>;
            //            foreach (LandX pLand in pLinkedLandMass.m_cContents)
            //            {
            //                pLand.m_iPassword = m_iPassword;
            //            }
            //        }
            //    }
            //}

            //PathFinder pLandsPath = new PathFinder(m_pTPFStart.Owner as LandX, m_pTPFFinish.Owner as LandX, m_pWorld.m_cGrid.CycleShift, m_iPassword, false);
            //foreach (TransportationNode pNode in pLandsPath.m_cPath)
            //{
            //    LandX pLand = pNode as LandX;
            //    foreach (LocationX pLoc in pLand.m_cContents)
            //    {
            //        pLoc.m_iPassword = m_iPassword;
            //    }
            //    foreach (ITerritory pTerr in pLand.m_aBorderWith)
            //    {
            //        if (!pTerr.Forbidden)
            //        {
            //            LandX pLinkedLand = pTerr as LandX;
            //            foreach (LocationX pLoc in pLinkedLand.m_cContents)
            //            {
            //                pLoc.m_iPassword = m_iPassword;
            //            }
            //        }
            //    }
            //}

            //PathFinder pBestPath = new PathFinder(m_pTPFStart, m_pTPFFinish, m_pWorld.m_cGrid.CycleShift, m_iPassword, false);
            ////DateTime pTime3 = DateTime.Now;

            //List<TransportationNode> cSucceedPath = new List<TransportationNode>();
            //if (pBestPath.m_cPath.Length == 0)
            //{
            //    foreach (TransportationNode pNode in pLandsPath.m_cPath)
            //    {
            //        if (!pBestPath.visited.Contains(pNode))
            //            break;
            //        cSucceedPath.Add(pNode);
            //    }
            //    pBestPath.m_cPath = cSucceedPath.ToArray();
            //}

            //worldMap1.AddPath(pBestPath.m_cPath, Color.Fuchsia);
            ShortestPath pPath1 = World.FindReallyBestPath(m_pTPFStart, m_pTPFFinish, m_pWorld.m_cGrid.CycleShift, false);
            //ShortestPath pPath2 = World.FindBestPath(m_pTPFFinish, m_pTPFStart, m_pWorld.m_cGrid.CycleShift, false);

            //if (pPath1.m_fLength < pPath2.m_fLength)
            //{
            //    worldMap1.AddPath(pPath1.m_aNodes, Color.Purple);
            //    worldMap1.AddPath(pPath2.m_aNodes, Color.Fuchsia);
            //}
            //else
            //{
            //    worldMap1.AddPath(pPath2.m_aNodes, Color.Purple);
            //    worldMap1.AddPath(pPath1.m_aNodes, Color.Fuchsia);
            //}
            worldMap1.AddPath(pPath1.m_aNodes, Color.Fuchsia);
        }

        private int m_iPassword = 0;

        private void ToolStripMenuItem_TestPathFinding2_Click(object sender, EventArgs e)
        {
            worldMap1.ClearPath();

            m_iPassword++;
            ShortestPath pLMPath = new ShortestPath((m_pTPFStart.Owner as LandX).Owner as LandMass<LandX>, (m_pTPFFinish.Owner as LandX).Owner as LandMass<LandX>, m_pWorld.m_cGrid.CycleShift, -1, false);
            foreach (TransportationNode pNode in pLMPath.m_aNodes)
            {
                LandMass<LandX> pLandMass = pNode as LandMass<LandX>;
                foreach (LandX pLand in pLandMass.m_cContents)
                {
                    pLand.m_iPassword = m_iPassword;
                }
                foreach (ITerritory pTerr in pLandMass.m_aBorderWith)
                {
                    if (!pTerr.Forbidden)
                    {
                        LandMass<LandX> pLinkedLandMass = pTerr as LandMass<LandX>;
                        foreach (LandX pLand in pLinkedLandMass.m_cContents)
                        {
                            pLand.m_iPassword = m_iPassword;
                        }
                    }
                }
            }
            worldMap1.AddPath(pLMPath.m_aNodes, Color.Purple);

            ShortestPath pLandsPath = new ShortestPath(m_pTPFStart.Owner as LandX, m_pTPFFinish.Owner as LandX, m_pWorld.m_cGrid.CycleShift, m_iPassword, false);
            worldMap1.AddPath(pLandsPath.m_aNodes, Color.Fuchsia);
        }

        private void ToolStripMenuItem_TestPathFinding3_Click(object sender, EventArgs e)
        {
            worldMap1.ClearPath();

            m_iPassword++;
            ShortestPath pLMPath = new ShortestPath((m_pTPFStart.Owner as LandX).Owner as LandMass<LandX>, (m_pTPFFinish.Owner as LandX).Owner as LandMass<LandX>, m_pWorld.m_cGrid.CycleShift, -1, false);
            foreach (TransportationNode pNode in pLMPath.m_aNodes)
            {
                LandMass<LandX> pLandMass = pNode as LandMass<LandX>;
                foreach (LandX pLand in pLandMass.m_cContents)
                {
                    pLand.m_iPassword = m_iPassword;
                }
                foreach (ITerritory pTerr in pLandMass.m_aBorderWith)
                {
                    if (!pTerr.Forbidden)
                    {
                        LandMass<LandX> pLinkedLandMass = pTerr as LandMass<LandX>;
                        foreach (LandX pLand in pLinkedLandMass.m_cContents)
                        {
                            pLand.m_iPassword = m_iPassword;
                        }
                    }
                }
            }
            worldMap1.AddPath(pLMPath.m_aNodes, Color.Purple);

            ShortestPath pLandsPath = new ShortestPath(m_pTPFStart.Owner as LandX, m_pTPFFinish.Owner as LandX, m_pWorld.m_cGrid.CycleShift, m_iPassword, false);
            foreach (TransportationNode pNode in pLandsPath.m_aNodes)
            {
                LandX pLand = pNode as LandX;
                foreach (LocationX pLoc in pLand.m_cContents)
                {
                    pLoc.m_iPassword = m_iPassword;
                }
                foreach (ITerritory pTerr in pLand.m_aBorderWith)
                {
                    if (!pTerr.Forbidden)
                    {
                        LandX pLinkedLand = pTerr as LandX;
                        foreach (LocationX pLoc in pLinkedLand.m_cContents)
                        {
                            pLoc.m_iPassword = m_iPassword;
                        }
                    }
                }
            }

            ShortestPath pBestPath = new ShortestPath(m_pTPFStart, m_pTPFFinish, m_pWorld.m_cGrid.CycleShift, m_iPassword, false);

            List<TransportationNode> cSucceedPath = new List<TransportationNode>();
            if (pBestPath.m_aNodes.Length == 0)
            {
                foreach (TransportationNode pNode in pLandsPath.m_aNodes)
                {
                    if (!pBestPath.visited.Contains(pNode))
                        break;
                    cSucceedPath.Add(pNode);
                }
                pBestPath.m_aNodes = cSucceedPath.ToArray();
            }

            worldMap1.AddPath(pBestPath.m_aNodes, Color.Fuchsia);
        }
    }
}
