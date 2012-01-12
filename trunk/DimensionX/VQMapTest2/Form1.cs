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
using MapDrawEngine;
using WorldGeneration;
using Socium.Psichology;
using Socium.Settlements;
using Socium.Nations;

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

            mapDraw1.BindMiniMap(miniMapDraw1);
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
                fScale = 8.0f;
            if (radioButton4.Checked)
                fScale = 32.0f;

            mapDraw1.Assign(m_pWorld);
            mapDraw1.ScaleMultiplier = fScale;

            label7.Text = string.Format("Avrg. tech level: {0} [T{1}]", State.GetTechString(m_pWorld.m_iTechLevel), m_pWorld.m_iTechLevel);
            if (m_pWorld.m_iMagicLimit > 0)
            {
                label8.Text = string.Format("Magic users: up to {0} [M{1}]", State.GetMagicString(m_pWorld.m_iMagicLimit), m_pWorld.m_iMagicLimit);
                label2.Text = "";
            }
            else
            {
                label8.Text = "Magic users: none [M0]";
                label2.Text = "";
            }

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
                mapDraw1.ScaleMultiplier = 1;
            if (radioButton2.Checked)
                mapDraw1.ScaleMultiplier = 2;
            if (radioButton3.Checked)
                mapDraw1.ScaleMultiplier = 8;
            if (radioButton4.Checked)
                mapDraw1.ScaleMultiplier = 32;

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
                    mapDraw1.Mode = MapMode.Areas;
                    break;
                case 1:
                    mapDraw1.Mode = MapMode.Humidity;
                    break;
                case 2:
                    mapDraw1.Mode = MapMode.Natives;
                    break;
                case 3:
                    mapDraw1.Mode = MapMode.Nations;
                    break;
                case 4:
                    mapDraw1.Mode = MapMode.TechLevel;
                    break;
                case 5:
                    mapDraw1.Mode = MapMode.PsiLevel;
                    break;
                case 6:
                    mapDraw1.Mode = MapMode.Infrastructure;
                    break;
            }

            comboBox1.Focus();
        }

        private void MapLayersChanged(object sender, EventArgs e)
        {
            mapDraw1.ShowLocations = showLandmarksToolStripMenuItem.Checked;
            mapDraw1.ShowRoads = showRoadsToolStripMenuItem.Checked;
            mapDraw1.ShowStates = showStateBordersToolStripMenuItem.Checked;
            mapDraw1.ShowProvincies = showProvinciesBordersToolStripMenuItem.Checked;
            mapDraw1.ShowLocationsBorders = showLocationsToolStripMenuItem.Checked;
            mapDraw1.ShowLands = showLandsToolStripMenuItem.Checked;
            mapDraw1.ShowLandMasses = showLandMassesToolStripMenuItem.Checked;

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
            mapDraw1.ShowLocations = toolStripMenuItem3.Checked;
            mapDraw1.ShowRoads = toolStripMenuItem4.Checked;
            mapDraw1.ShowStates = toolStripMenuItem5.Checked;
            mapDraw1.ShowProvincies = toolStripMenuItem6.Checked;
            mapDraw1.ShowLocationsBorders = toolStripMenuItem8.Checked;
            mapDraw1.ShowLands = toolStripMenuItem9.Checked;
            mapDraw1.ShowLandMasses = toolStripMenuItem10.Checked;

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
            mapDraw1.SelectedState = comboBox1.SelectedItem as State;
        }

        private void worldMap1_StateSelectedEvent(object sender, MapDraw.SelectedStateChangedEventArgs e)
        {
            comboBox1.SelectedItem = e.m_pState;

            richTextBox1.Clear();

            richTextBox1.AppendText(string.Format("{0} {1}\n\n", e.m_pState.m_sName, e.m_pState.m_pInfo.m_sName));

            richTextBox1.AppendText(string.Format("Major race: {2} [T{0}M{1}]\n\n", e.m_pState.m_pNation.m_iTechLevel, e.m_pState.m_pNation.m_iMagicLimit, e.m_pState.m_pNation));

            richTextBox1.AppendText(string.Format("Social order : {0} [C{1}]\n\n", State.GetControlString(e.m_pState.m_iControl), e.m_pState.m_iCultureLevel));

            richTextBox1.AppendText(string.Format("Economic system : {0}\n\n", State.GetEqualityString(e.m_pState.m_iSocialEquality))); 
            
            richTextBox1.AppendText(string.Format("Culture:\n"));
            foreach (Mentality eMentality in Culture.Mentalities)
            {
                richTextBox1.AppendText("   ");
                //richTextBox1.AppendText(string.Format("   {0}: \t", eMorale));
                //if (eMorale.ToString().Length < 6)
                //    richTextBox1.AppendText("\t");
                richTextBox1.AppendText(e.m_pState.m_pCulture.GetMentalityString(eMentality, e.m_pState.m_iCultureLevel));
                //richTextBox1.AppendText(string.Format("{0:0.00}\n", e.State.m_pCulture.Moral[eMorale]));
                richTextBox1.AppendText("\n");
            }
            richTextBox1.AppendText("\n");

            string sRaceName = e.m_pState.m_pNation.m_pRace.m_sName;
            sRaceName = sRaceName.Substring(0, 1).ToUpper() + sRaceName.Substring(1);
            richTextBox1.AppendText(sRaceName + "s " + e.m_pState.m_pNation.m_pRace.m_pFenotype.GetDescription());
            richTextBox1.AppendText("\n");
            richTextBox1.AppendText("Known " + e.m_pState.m_pNation.m_pRace.m_sName + " nations are: ");
            bool bFirst = true;
            List<Nation> cKnownNations = new List<Nation>();
            foreach (State pState in m_pWorld.m_aStates)
            {
                if(pState.m_pNation.m_pRace == e.m_pState.m_pNation.m_pRace && !cKnownNations.Contains(pState.m_pNation))
                {
                    if (!bFirst)
                        richTextBox1.AppendText(", ");

                    bFirst = false;

                    richTextBox1.AppendText(pState.m_pNation.m_sName);

                    cKnownNations.Add(pState.m_pNation);
                }
            }
            richTextBox1.AppendText(".\n\n");

            string sFenotypeNation = e.m_pState.m_pNation.m_pFenotype.GetComparsion(e.m_pState.m_pNation.m_pRace.m_pFenotype);
            if (!sFenotypeNation.StartsWith("are"))
                sFenotypeNation = "are common " + e.m_pState.m_pNation.m_pRace.m_sName + "s, however " + sFenotypeNation.Substring(0, 1).ToLower() + sFenotypeNation.Substring(1);
            richTextBox1.AppendText(e.m_pState.m_pNation.m_sName + "s " + sFenotypeNation);
            richTextBox1.AppendText("\n\n");

            richTextBox1.AppendText(e.m_pState.m_pCustoms.GetCustomsString2());
            richTextBox1.AppendText("\n");

            if (e.m_pState.GetImportedTech() == -1)
                richTextBox1.AppendText(string.Format("Used tech: {0} [T{1}]\n\n", State.GetTechString(e.m_pState.m_iTechLevel), e.m_pState.m_iTechLevel));
            else
                richTextBox1.AppendText(string.Format("Used tech: {0} [T{1}]\n\n", State.GetTechString(e.m_pState.GetImportedTech()), e.m_pState.GetImportedTech()));
            richTextBox1.AppendText(string.Format("Industrial base: {0} [T{1}]\n\n", State.GetTechString(e.m_pState.m_iTechLevel), e.m_pState.m_iTechLevel));

            if (e.m_pState.m_iMagicLimit > 0)
            {
                richTextBox1.AppendText(string.Format("Magic users: {0}, ", e.m_pState.m_eMagicAbilityPrevalence.ToString().Replace('_', ' ')));
                richTextBox1.AppendText(string.Format("{2}, up to {0} [M{1}]\n\n", State.GetMagicString(e.m_pState.m_iMagicLimit), e.m_pState.m_iMagicLimit, e.m_pState.m_eMagicAbilityDistribution.ToString().Replace('_', ' ')));
            }
            else
            {
                richTextBox1.AppendText("Magic users: none [M0]\n\n");
            }

            richTextBox1.AppendText(string.Format("Resources: F:{0}, W:{1}, I:{2} / P:{3}\n\n", e.m_pState.m_iFood, e.m_pState.m_iWood, e.m_pState.m_iOre, e.m_pState.m_iPopulation));

            State[] cEnemies = e.m_pState.GetEnemiesList();
            if(cEnemies.Length > 0)
            {
                richTextBox1.AppendText("Enemies: ");
                bFirst = true;
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

            State[] cAllies = e.m_pState.GetAlliesList();
            if (cAllies.Length > 0)
            {
                richTextBox1.AppendText("Allies: ");
                bFirst = true;
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

            foreach(Province pProvince in e.m_pState.m_cContents)
                foreach (LocationX pLoc in pProvince.m_cSettlements)
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

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!m_pGenerationForm.Preload())
                Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Point pPoint = mapDraw1.GetCentralPoint(mapDraw1.SelectedState);

            //Получаем новые координаты для DrawFrame, чтобы выбранное государство было в центре
            mapDraw1.SetPan(pPoint.X - mapDraw1.DrawFrame.Width / 2, pPoint.Y - mapDraw1.DrawFrame.Height / 2);

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
            mapDraw1.SelectedState = comboBox1.Items[iIndex] as State;
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

            if (mapDraw1.SelectedState == null)
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

                        string sTip = string.Format("{0} {1} to {2} {3}:\n", mapDraw1.SelectedState.m_sName, mapDraw1.SelectedState.m_pInfo.m_sName, pLinkState.m_sName, pLinkState.m_pInfo.m_sName);
                        string sRelation;
                        mapDraw1.SelectedState.CalcHostility(pLinkState, out sRelation);

                        sTip += sRelation;
                        sTip += "\n";

                        sTip += string.Format("{2} {3} to {0} {1}:\n", mapDraw1.SelectedState.m_sName, mapDraw1.SelectedState.m_pInfo.m_sName, pLinkState.m_sName, pLinkState.m_pInfo.m_sName);
                        pLinkState.CalcHostility(mapDraw1.SelectedState, out sRelation);
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
                m_pTPFStart = m_pWorld.m_pGrid.m_aLocations[Rnd.Get(m_pWorld.m_pGrid.m_aLocations.Length)];
            }
            while (m_pTPFStart.Forbidden || 
                   (m_pTPFStart.Owner as LandX).IsWater || 
                   m_pTPFStart.m_pSettlement == null || 
                   m_pTPFStart.m_pSettlement.m_iRuinsAge > 0);

            do
            {
                m_pTPFFinish = m_pWorld.m_pGrid.m_aLocations[Rnd.Get(m_pWorld.m_pGrid.m_aLocations.Length)];
            }
            while (m_pTPFFinish.Forbidden || 
                   m_pTPFFinish == m_pTPFStart || 
                   (m_pTPFFinish.Owner as LandX).IsWater || 
                   ((m_pTPFFinish.m_pBuilding == null || 
                     (m_pTPFFinish.m_pBuilding.m_eType != BuildingType.Hideout && 
                      m_pTPFFinish.m_pBuilding.m_eType != BuildingType.Lair)) && 
                    m_pTPFFinish.m_pSettlement == null));

            mapDraw1.ClearPath();

            ShortestPath pPath1 = World.FindReallyBestPath(m_pTPFStart, m_pTPFFinish, m_pWorld.m_pGrid.CycleShift, false);
            mapDraw1.AddPath(pPath1.m_aNodes, Color.Fuchsia);
        }

        private void worldToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            repeatCreationFromPresetToolStripMenuItem.DropDownItems.Clear();

            foreach (string sPreset in m_pGenerationForm.m_cLastUsedPresets)
            {
                repeatCreationFromPresetToolStripMenuItem.DropDownItems.Add(sPreset).Click += new EventHandler(UsedPresetClick);
            }

            repeatCreationFromPresetToolStripMenuItem.Enabled = repeatCreationFromPresetToolStripMenuItem.DropDownItems.Count > 0;
        }

        private void UsedPresetClick(object sender, EventArgs e)
        {
            ToolStripItem pItem = sender as ToolStripItem;

            if (pItem != null)
            {
                m_pGenerationForm.LoadPreset(pItem.Text);
                m_pGenerationForm.GenerateWorld(this);
            
                m_pWorld = m_pGenerationForm.World;
                label1.Visible = false;
                ShowWorld();
            }
        }
    }
}
