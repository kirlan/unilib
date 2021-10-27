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
using Socium.Psychology;
using Socium.Settlements;
using Socium.Nations;
using Socium.Population;
using static Socium.Psychology.Customs;

namespace VQMapTest2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            //try
            {
                InitializeComponent();

                MapScaleChanged(this, null);
                MapModeChanged(this, null);
                MapLayersChanged(this, null);

                showLandmarksToolStripMenuItem.Checked = true;
                showRoadsToolStripMenuItem.Checked = true;

                mapDraw1.BindMiniMap(miniMapDraw1);
            }
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message + "/r/n----/r/n" + ex.InnerException.Message);
            //}
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

            //==========================================DEBUG=================================================
            mapDraw1.Visible = false;
            //==========================================DEBUG=================================================

            mapDraw1.Assign(m_pWorld);
            mapDraw1.ScaleMultiplier = fScale;

            label7.Text = string.Format("Avrg. tech level: {0} [T{1}]", Society.GetTechString(m_pWorld.m_aEpoches.Last().m_iNativesMaxTechLevel, Socium.Psychology.Customs.Science.Moderate_Science), m_pWorld.m_aEpoches.Last().m_iNativesMaxTechLevel);
            if (m_pWorld.m_aEpoches.Last().m_iNativesMaxMagicLevel > 0)
            {
                label8.Text = string.Format("Magic users: up to {0} [M{1}]", Society.GetMagicString(m_pWorld.m_aEpoches.Last().m_iNativesMaxMagicLevel), m_pWorld.m_aEpoches.Last().m_iNativesMaxMagicLevel);
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
                    mapDraw1.Mode = MapMode.Elevation;
                    break;
                case 3:
                    mapDraw1.Mode = MapMode.Natives;
                    break;
                case 4:
                    mapDraw1.Mode = MapMode.Nations;
                    break;
                case 5:
                    mapDraw1.Mode = MapMode.TechLevel;
                    break;
                case 6:
                    mapDraw1.Mode = MapMode.PsiLevel;
                    break;
                case 7:
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

            var pSociety = e.m_pState.m_pSociety;

            richTextBox1.Clear();

            richTextBox1.AppendText(string.Format("{0} {1}\n\n", e.m_pState.m_pSociety.m_sName, pSociety.m_pStateModel.m_sName));

            richTextBox1.AppendText(string.Format("Major race: {2} [T{0}M{1}]\n\n", pSociety.m_iTechLevel, pSociety.m_iMagicLimit, pSociety.m_pTitularNation));

            richTextBox1.AppendText(string.Format("Social order : {0} [C{1}]\n\n", Society.GetControlString(pSociety.m_iControl), pSociety.DominantCulture.m_iProgressLevel));

            richTextBox1.AppendText(string.Format("Economic system : {0}\n\n", StateSociety.GetEqualityString(pSociety.m_iSocialEquality))); 
            
            richTextBox1.AppendText(string.Format("Culture:\n"));
            foreach (Trait eMentality in Mentality.AllTraits)
            {
                richTextBox1.AppendText("   ");
                //richTextBox1.AppendText(string.Format("   {0}: \t", eMorale));
                //if (eMorale.ToString().Length < 6)
                //    richTextBox1.AppendText("\t");
                richTextBox1.AppendText(pSociety.DominantCulture.m_pMentality.GetTraitString(eMentality, pSociety.DominantCulture.m_iProgressLevel));
                //richTextBox1.AppendText(string.Format("{0:0.00}\n", e.State.m_pCulture.Moral[eMorale]));
                richTextBox1.AppendText("\n");
            }
            richTextBox1.AppendText("\n");

            string sRaceName = pSociety.m_pTitularNation.m_pRace.m_sName;
            sRaceName = sRaceName.Substring(0, 1).ToUpper() + sRaceName.Substring(1);
            richTextBox1.AppendText(sRaceName + " males " + pSociety.m_pTitularNation.m_pRace.m_pPhenotypeM.GetDescription());
            richTextBox1.AppendText("\n");
            richTextBox1.AppendText(sRaceName + " females " + pSociety.m_pTitularNation.m_pRace.m_pPhenotypeF.GetComparsion(pSociety.m_pTitularNation.m_pRace.m_pPhenotypeM));
            List<Nation> cKnownNations = new List<Nation>();
            foreach (State pState in m_pWorld.m_aStates)
            {
                if (pState.m_pSociety.m_pTitularNation.m_pRace == pSociety.m_pTitularNation.m_pRace && !cKnownNations.Contains(pState.m_pSociety.m_pTitularNation))
                {
                    cKnownNations.Add(pState.m_pSociety.m_pTitularNation);
                }
            }
            if (cKnownNations.Count > 1)
            {
                richTextBox1.AppendText("\n");
                richTextBox1.AppendText("Known " + pSociety.m_pTitularNation.m_pRace.m_sName + " nations are: ");
                bool bFirst = true;
                foreach (Nation pNation in cKnownNations)
                {
                    if (!bFirst)
                        richTextBox1.AppendText(", ");

                    bFirst = false;

                    richTextBox1.AppendText(pNation.m_pProtoSociety.m_sName);
                }
                richTextBox1.AppendText(".\n\n");

                string sFenotypeNationM = pSociety.m_pTitularNation.m_pPhenotypeM.GetComparsion(pSociety.m_pTitularNation.m_pRace.m_pPhenotypeM);
                //var expectedFenotypeF = GeneLab.Phenotype<LandTypeInfo>.ApplyDifferences(pSociety.m_pTitularNation.m_pPhenotypeM, pSociety.m_pTitularNation.m_pRace.m_pPhenotypeM, pSociety.m_pTitularNation.m_pRace.m_pPhenotypeF);
                string sFenotypeNationF = pSociety.m_pTitularNation.m_pPhenotypeF.GetComparsion(pSociety.m_pTitularNation.m_pRace.m_pPhenotypeF);

                if (sFenotypeNationM == sFenotypeNationF)
                {
                    if (!sFenotypeNationM.StartsWith("are"))
                        sFenotypeNationM = "are common " + pSociety.m_pTitularNation.m_pRace.m_sName + "s, however " + sFenotypeNationM.Substring(0, 1).ToLower() + sFenotypeNationM.Substring(1);
                    richTextBox1.AppendText(pSociety.m_pTitularNation.m_pProtoSociety.m_sName + " " + sFenotypeNationM);
                }
                else
                {
                    if (sFenotypeNationM != "")
                    {
                        if (!sFenotypeNationM.StartsWith("are"))
                            sFenotypeNationM = "are common " + pSociety.m_pTitularNation.m_pRace.m_sName + "s, however " + sFenotypeNationM.Substring(0, 1).ToLower() + sFenotypeNationM.Substring(1);
                        richTextBox1.AppendText(pSociety.m_pTitularNation.m_pProtoSociety.m_sName + " males " + sFenotypeNationM);
                    }

                    if (sFenotypeNationF != "")
                    {
                        if (sFenotypeNationM != "")
                            richTextBox1.AppendText("\n");

                        if (!sFenotypeNationF.StartsWith("are"))
                            sFenotypeNationF = "are common " + pSociety.m_pTitularNation.m_pRace.m_sName + "s, however " + sFenotypeNationF.Substring(0, 1).ToLower() + sFenotypeNationF.Substring(1);
                        richTextBox1.AppendText(pSociety.m_pTitularNation.m_pProtoSociety.m_sName + " females " + sFenotypeNationF);
                    }
                }
            }
            richTextBox1.AppendText("\n\n");

            if (pSociety.GetImportedTech() == -1)
                richTextBox1.AppendText(string.Format("Available tech: {0} [T{1}]\n\n", Society.GetTechString(pSociety.m_iTechLevel, pSociety.DominantCulture.m_pCustoms.ValueOf<Customs.Science>()), pSociety.GetEffectiveTech()));
            else
                richTextBox1.AppendText(string.Format("Available tech: {0} [T{1}]\n\n", pSociety.GetImportedTechString(), pSociety.GetImportedTech()));
            richTextBox1.AppendText(string.Format("Industrial base: {0} [T{1}]\n\n", Society.GetTechString(pSociety.m_iTechLevel, pSociety.DominantCulture.m_pCustoms.ValueOf<Customs.Science>()), pSociety.GetEffectiveTech()));

            if (pSociety.m_iMagicLimit > 0)
            {
                string sMagicAttitude = "regulated";
                if (pSociety.DominantCulture.m_pCustoms.Has(Customs.Magic.Magic_Feared))
                    sMagicAttitude = "outlawed";
                if (pSociety.DominantCulture.m_pCustoms.Has(Customs.Magic.Magic_Praised))
                    sMagicAttitude = "unlimited";
                richTextBox1.AppendText(string.Format("Magic users: {0}, ", sMagicAttitude));
                richTextBox1.AppendText(string.Format("{2}, up to {0} [M{1}]\n\n", Society.GetMagicString(pSociety.m_iMagicLimit), pSociety.m_iMagicLimit, pSociety.DominantCulture.m_eMagicAbilityDistribution.ToString().Replace('_', ' ')));
            }
            else
            {
                string sMagicAttitude = "but allowed";
                if (pSociety.DominantCulture.m_pCustoms.Has(Customs.Magic.Magic_Feared))
                    sMagicAttitude = "and outlawed";
                if (pSociety.DominantCulture.m_pCustoms.Has(Customs.Magic.Magic_Praised))
                    sMagicAttitude = "but praised";
                richTextBox1.AppendText(string.Format("Magic users: none, {0} [M0]\n\n", sMagicAttitude));
            }

            richTextBox1.AppendText(string.Format("Resources: F:{0}, W:{1}, I:{2} / P:{3}\n\n", e.m_pState.m_iFood, e.m_pState.m_iWood, e.m_pState.m_iOre, e.m_pState.m_iPopulation));

            richTextBox1.AppendText("Estates: \n");

            int iTotalPop = 0;
            foreach (var pEstate in pSociety.m_cEstates)
            {
                iTotalPop += pSociety.m_cEstatesCounts[pEstate.Key];
            }

            {
                Estate pEstate = pSociety.m_cEstates[Estate.Position.Commoners];

                richTextBox1.AppendText("  - " + Estate.Position.Commoners.ToString() + " aka " + pEstate.m_sName + " [" + string.Format("{0:D}", (int)(pSociety.m_cEstatesCounts[Estate.Position.Commoners] * 100.0f / iTotalPop)) + "%]: ");
                if (pEstate.m_pTitularNation != pSociety.m_pTitularNation)
                    richTextBox1.AppendText("Estate consists of enslaved " + pEstate.m_pTitularNation + ".\n");

                richTextBox1.AppendText(pEstate.DominantCulture.m_pCustoms.GetCustomsDescription());

                string sMinorsCulture = pEstate.InferiorCulture.m_pMentality.GetDiffString(pEstate.InferiorCulture.m_iProgressLevel, pEstate.DominantCulture.m_pMentality, pEstate.DominantCulture.m_iProgressLevel);
                string sMinorsCustoms = pEstate.InferiorCulture.m_pCustoms.GetCustomsDiffString2(pEstate.DominantCulture.m_pCustoms);

                if (pEstate.IsSegregated() && (sMinorsCustoms != "" || sMinorsCulture != ""))
                {
                    string sMajors = pEstate.DominantCulture.m_pCustoms.Has(Customs.GenderPriority.Patriarchy) ? "males" : "females";
                    string sMinors = pEstate.DominantCulture.m_pCustoms.Has(Customs.GenderPriority.Patriarchy) ? "females" : "males";
                    if (sMinorsCustoms != "")
                        richTextBox1.AppendText(" Their " + sMinors + " commonly " + sMinorsCustoms + ".");
                    if (sMinorsCulture != "")
                    {
                        if (sMinorsCustoms == "")
                            richTextBox1.AppendText(" Their " + sMinors + " usually " + sMinorsCulture + " then " + sMajors + ".");
                        else
                            richTextBox1.AppendText(" They are usually " + sMinorsCulture + " then " + sMajors + ".");
                    }
                }

                richTextBox1.AppendText("\n    Prevalent professions:");
                foreach (var pGenderPreference in pEstate.m_cGenderProfessionPreferences)
                    richTextBox1.AppendText("\n         - " + (pGenderPreference.Value == Customs.GenderPriority.Matriarchy ? pGenderPreference.Key.m_sNameF : pGenderPreference.Key.m_sNameM));
                richTextBox1.AppendText("\n\n");
            }

            foreach (var pEstate in pSociety.m_cEstates)
            {
                if (pEstate.Key == Estate.Position.Commoners)
                    continue;

                richTextBox1.AppendText("  - " + pEstate.Key.ToString() + " aka " + pEstate.Value.m_sName + " [" + string.Format("{0:D}", (int)(pSociety.m_cEstatesCounts[pEstate.Key] * 100.0f / iTotalPop)) + "%]: ");
                if (pEstate.Value.m_pTitularNation != pSociety.m_pTitularNation)
                    richTextBox1.AppendText("Estate consists of " + (pEstate.Key == Estate.Position.Lowlifes ? "enslaved " : "rebel ") + pEstate.Value.m_pTitularNation + ".\n");

                string sCulture = pEstate.Value.DominantCulture.m_pMentality.GetDiffString(pEstate.Value.DominantCulture.m_iProgressLevel, pSociety.DominantCulture.m_pMentality, pSociety.DominantCulture.m_iProgressLevel);
                string sCustoms = pEstate.Value.DominantCulture.m_pCustoms.GetCustomsDiffString2(pSociety.DominantCulture.m_pCustoms);

                string sMinorsCulture = pEstate.Value.InferiorCulture.m_pMentality.GetDiffString(pEstate.Value.InferiorCulture.m_iProgressLevel, pEstate.Value.DominantCulture.m_pMentality, pEstate.Value.DominantCulture.m_iProgressLevel);
                string sMinorsCustoms = pEstate.Value.InferiorCulture.m_pCustoms.GetCustomsDiffString2(pEstate.Value.DominantCulture.m_pCustoms);

                string sGenderPriority = "";

                if (pEstate.Value.DominantCulture.m_pCustoms.Has(GenderPriority.Patriarchy))
                {
                    sGenderPriority = "patriarchal";
                }
                else if (pEstate.Value.DominantCulture.m_pCustoms.Has(GenderPriority.Matriarchy))
                {
                    sGenderPriority = "matriarchal";
                }
                else if (pEstate.Value.DominantCulture.m_pCustoms.Has(GenderPriority.Genders_equality))
                {
                    sGenderPriority = "gender equality";
                }

                if (sCustoms != "")
                    richTextBox1.AppendText("This is a " + sGenderPriority + " society, which members " + sCustoms + ".");
                if (sCulture != "")
                {
                    if (sCustoms == "")
                        richTextBox1.AppendText("This is a " + sGenderPriority + " society, which members " + sCulture + " then common citizens.");
                    else
                        richTextBox1.AppendText(" They are usually " + sCulture + " then common citizens.");
                }

                if (pEstate.Value.IsSegregated() && (sMinorsCustoms != "" || sMinorsCulture != ""))
                {
                    string sMajors = pEstate.Value.DominantCulture.m_pCustoms.Has(Customs.GenderPriority.Patriarchy) ? "males" : "females";
                    string sMinors = pEstate.Value.DominantCulture.m_pCustoms.Has(Customs.GenderPriority.Patriarchy) ? "females" : "males";
                    if (sCustoms == "" && sCulture == "")
                    {
                        richTextBox1.AppendText("This is a " + sGenderPriority + " society, which members are just a common citizens.");
                    }
                    if (sMinorsCustoms != "")
                        richTextBox1.AppendText(" Their " + sMinors + " commonly " + sMinorsCustoms + ".");
                    if (sMinorsCulture != "")
                    {
                        if (sMinorsCustoms == "")
                            richTextBox1.AppendText(" Their " + sMinors + " usually " + sMinorsCulture + " then " + sMajors + ".");
                        else
                            richTextBox1.AppendText(" They are usually " + sMinorsCulture + " then " + sMajors + ".");
                    }
                }

                if (sCulture == "" && sCustoms == "" && sMinorsCulture == "" && sMinorsCustoms == "")
                    richTextBox1.AppendText("Members of this estate are just a common citizens.");

                richTextBox1.AppendText("\n    Prevalent professions:");
                foreach (var pGenderPreference in pEstate.Value.m_cGenderProfessionPreferences)
                    richTextBox1.AppendText("\n         - " + (pGenderPreference.Value == Customs.GenderPriority.Matriarchy ? pGenderPreference.Key.m_sNameF : pGenderPreference.Key.m_sNameM));
                richTextBox1.AppendText("\n\n");
            }

            State[] cEnemies = e.m_pState.GetEnemiesList();
            if(cEnemies.Length > 0)
            {
                richTextBox1.AppendText("Enemies: ");
                bool bFirst = true;
                foreach (State pState in cEnemies)
                {
                    if(!bFirst)
                        richTextBox1.AppendText(", ");
                    richTextBox1.InsertLink(string.Format("{0} {1}", pState.m_pSociety.m_sName, pState.m_pSociety.m_pStateModel.m_sName), comboBox1.Items.IndexOf(pState).ToString());
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
                bool bFirst = true;
                foreach (State pState in cAllies)
                {
                    if (!bFirst)
                        richTextBox1.AppendText(", ");
                    richTextBox1.InsertLink(string.Format("{0} {1}", pState.m_pSociety.m_sName, pState.m_pSociety.m_pStateModel.m_sName), comboBox1.Items.IndexOf(pState).ToString());
                    bFirst = false;
                }
                richTextBox1.AppendText("\n\n");
            }
            else
                richTextBox1.AppendText("Allies: none\n\n");

            listBox1.Items.Clear();

            Dictionary<string, int> cPop = new Dictionary<string, int>();

            foreach(LocationX pLoc in e.m_pState.m_pSociety.Settlements)
            {
                listBox1.Items.Add(pLoc);

                foreach (Building pBuilding in pLoc.m_pSettlement.m_cBuildings)
                {
                    if (pBuilding.m_pInfo.m_eSize == BuildingSize.Unique)
                        continue;

                    int iPop = 0;
                    cPop.TryGetValue(pBuilding.m_pInfo.m_pOwnerProfession.m_sNameM, out iPop);
                    cPop[pBuilding.m_pInfo.m_pOwnerProfession.m_sNameM] = iPop + pBuilding.m_pInfo.OwnersCount;

                    iPop = 0;
                    cPop.TryGetValue(pBuilding.m_pInfo.m_pWorkersProfession.m_sNameM, out iPop);
                    cPop[pBuilding.m_pInfo.m_pWorkersProfession.m_sNameM] = iPop + pBuilding.m_pInfo.WorkersCount;
                }
            }

            richTextBox2.Clear();

            int iTotal = 0;
            
            Dictionary<string, int> cPopSorted = new Dictionary<string, int>();

            while (cPopSorted.Count < cPop.Count)
            {
                int iMax = 0;
                string sMax = "";
                foreach (var vPop in cPop)
                {
                    if (cPopSorted.ContainsKey(vPop.Key))
                        continue;

                    if (vPop.Value > iMax)
                    {
                        iMax = vPop.Value;
                        sMax = vPop.Key;
                    }
                }

                cPopSorted[sMax] = iMax;
                iTotal += iMax;
            }

            foreach (var vPop in cPopSorted)
            {
                richTextBox2.AppendText(vPop.Key + "s : " + vPop.Value*100/iTotal + "%\r\n");
            }
            //richTextBox2.AppendText("Total : " + iTotal + "\r\n");

            comboBox1.Focus();
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

            if (m_pGenerationForm.m_pSettings.m_cLastUsedPresets.Count > 0)
                Properties.Settings.Default.preset1 = m_pGenerationForm.m_pSettings.m_cLastUsedPresets[0];
            if (m_pGenerationForm.m_pSettings.m_cLastUsedPresets.Count > 1)
                Properties.Settings.Default.preset2 = m_pGenerationForm.m_pSettings.m_cLastUsedPresets[1];
            if (m_pGenerationForm.m_pSettings.m_cLastUsedPresets.Count > 2)
                Properties.Settings.Default.preset3 = m_pGenerationForm.m_pSettings.m_cLastUsedPresets[2];
            if (m_pGenerationForm.m_pSettings.m_cLastUsedPresets.Count > 3)
                Properties.Settings.Default.preset4 = m_pGenerationForm.m_pSettings.m_cLastUsedPresets[3];
            if (m_pGenerationForm.m_pSettings.m_cLastUsedPresets.Count > 4)
                Properties.Settings.Default.preset5 = m_pGenerationForm.m_pSettings.m_cLastUsedPresets[4];
            Properties.Settings.Default.WorkingPath = m_pGenerationForm.m_pSettings.m_sWorkingDir;

            Properties.Settings.Default.Save();
        }

        private void ToolStripMenuItem_Exit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GenerationForm.Settings settings = new GenerationForm.Settings();

            string sPreset = Properties.Settings.Default.preset1;
            if (sPreset != "" && !settings.m_cLastUsedPresets.Contains(sPreset))
                settings.m_cLastUsedPresets.Add(sPreset);

            sPreset = Properties.Settings.Default.preset2;
            if (sPreset != "" && !settings.m_cLastUsedPresets.Contains(sPreset))
                settings.m_cLastUsedPresets.Add(sPreset);

            sPreset = Properties.Settings.Default.preset3;
            if (sPreset != "" && !settings.m_cLastUsedPresets.Contains(sPreset))
                settings.m_cLastUsedPresets.Add(sPreset);

            sPreset = Properties.Settings.Default.preset4;
            if (sPreset != "" && !settings.m_cLastUsedPresets.Contains(sPreset))
                settings.m_cLastUsedPresets.Add(sPreset);

            sPreset = Properties.Settings.Default.preset5;
            if (sPreset != "" && !settings.m_cLastUsedPresets.Contains(sPreset))
                settings.m_cLastUsedPresets.Add(sPreset);

            settings.m_sWorkingDir = Properties.Settings.Default.WorkingPath;

            if (m_pGenerationForm.Preload(settings))
            {
                Properties.Settings.Default.WorkingPath = m_pGenerationForm.m_pSettings.m_sWorkingDir;
                Properties.Settings.Default.Save();
            }
            else
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

                        string sTip = string.Format("{0} {1} to {2} {3}:\n", mapDraw1.SelectedState.m_pSociety.m_sName, mapDraw1.SelectedState.m_pSociety.m_pStateModel.m_sName, pLinkState.m_pSociety.m_sName, pLinkState.m_pSociety.m_pStateModel.m_sName);
                        string sRelation;
                        mapDraw1.SelectedState.m_pSociety.CalcHostility(pLinkState, out sRelation);

                        sTip += sRelation;
                        sTip += "\n";

                        sTip += string.Format("{2} {3} to {0} {1}:\n", mapDraw1.SelectedState.m_pSociety.m_sName, mapDraw1.SelectedState.m_pSociety.m_pStateModel.m_sName, pLinkState.m_pSociety.m_sName, pLinkState.m_pSociety.m_pStateModel.m_sName);
                        pLinkState.m_pSociety.CalcHostility(mapDraw1.SelectedState, out sRelation);
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
                m_pTPFStart = m_pWorld.m_pGrid.Locations[Rnd.Get(m_pWorld.m_pGrid.Locations.Length)];
            }
            while (m_pTPFStart.Forbidden || 
                   (m_pTPFStart.Owner as LandX).IsWater || 
                   m_pTPFStart.m_pSettlement == null || 
                   m_pTPFStart.m_pSettlement.m_iRuinsAge > 0);

            do
            {
                m_pTPFFinish = m_pWorld.m_pGrid.Locations[Rnd.Get(m_pWorld.m_pGrid.Locations.Length)];
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

            foreach (string sPreset in m_pGenerationForm.m_pSettings.m_cLastUsedPresets)
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

                if (m_pWorld != null)
                    ShowWorld();
            }
        }
    }
}
