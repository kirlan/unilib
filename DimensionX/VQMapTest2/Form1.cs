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

            LandTypes.Coastral.AddLayer(new LandTypeDrawInfo(Color.FromArgb(0x27, 0x67, 0x71)));//(0x2a, 0x83, 0x93);//(0x36, 0xa9, 0xbd);//FromArgb(0xa2, 0xed, 0xfa);//LightSkyBlue;//LightCyan;

            LandTypes.Ocean.AddLayer(new LandTypeDrawInfo(Color.FromArgb(0x1e, 0x5e, 0x69)));//(0x2a, 0x83, 0x93);//(0x36, 0xa9, 0xbd);//FromArgb(0xa2, 0xed, 0xfa);//LightSkyBlue;//LightCyan;

            LandTypes.Plains.AddLayer(new LandTypeDrawInfo(Color.FromArgb(0xd3, 0xfa, 0x5f)));//(0xdc, 0xfa, 0x83);//LightGreen;

            LandTypes.Savanna.AddLayer(new LandTypeDrawInfo(Color.FromArgb(0xf0, 0xff, 0x8a)));//(0xbd, 0xb0, 0x6b);//PaleGreen;

            LandTypes.Tundra.AddLayer(new LandTypeDrawInfo(Color.FromArgb(0xc9, 0xff, 0xff)));//(0xc9, 0xe0, 0xff);//PaleGreen;

            LandTypes.Desert.AddLayer(new LandTypeDrawInfo(Color.FromArgb(0xfa, 0xdc, 0x36)));//(0xf9, 0xfa, 0x8a);//LightYellow;

            LandTypes.Forest.AddLayer(new LandTypeDrawInfo(Color.FromArgb(0x56, 0x78, 0x34)));//(0x63, 0x78, 0x4e);//LightGreen;//ForestGreen;

            LandTypes.Taiga.AddLayer(new LandTypeDrawInfo(Color.FromArgb(0x63, 0x78, 0x4e)));//LightGreen;//ForestGreen;

            LandTypes.Swamp.AddLayer(new LandTypeDrawInfo(Color.FromArgb(0xa7, 0xbd, 0x6b)));// DarkKhaki;

            LandTypes.Mountains.AddLayer(new LandTypeDrawInfo(Color.FromArgb(0xbd, 0x6d, 0x46)));//Tan;

            LandTypes.Jungle.AddLayer(new LandTypeDrawInfo(Color.FromArgb(0x8d, 0xb7, 0x31)));//(0x72, 0x94, 0x28);//PaleGreen;

            mapDraw1.Assign(m_pWorld);
            mapDraw1.ScaleMultiplier = fScale;

            label7.Text = string.Format("Avrg. tech level: {0} [T{1}]", Society.GetTechString(m_pWorld.m_aEpoches.Last().NativesMaxTechLevel, Socium.Psychology.Customs.Science.Moderate_Science), m_pWorld.m_aEpoches.Last().NativesMaxTechLevel);
            if (m_pWorld.m_aEpoches.Last().NativesMaxMagicLevel > 0)
            {
                label8.Text = string.Format("Magic users: up to {0} [M{1}]", Society.GetMagicString(m_pWorld.m_aEpoches.Last().NativesMaxMagicLevel), m_pWorld.m_aEpoches.Last().NativesMaxMagicLevel);
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

        private void AddNationInfo(Nation pNation)
        {
            List<Nation> cKnownNations = new List<Nation>();
            foreach (State pState in m_pWorld.m_aStates)
            {
                foreach (var pEstate in pState.m_pSociety.Estates)
                {
                    if (pEstate.Value.TitularNation.Race == pNation.Race && !cKnownNations.Contains(pEstate.Value.TitularNation))
                    {
                        cKnownNations.Add(pEstate.Value.TitularNation);
                    }
                }
            }

            string sRaceName = pNation.Race.Name;
            sRaceName = sRaceName.Substring(0, 1).ToUpper() + sRaceName.Substring(1);
            var pPhenotypeM = pNation.Race.PhenotypeMale;
            var pPhenotypeF = pNation.Race.PhenotypeFemale;
            if (cKnownNations.Count == 1)
            {
                richTextBox1.AppendText(sRaceName + "s are mostly known in this world as " + pNation.ProtoSociety.Name + "s. ");
                sRaceName = pNation.ProtoSociety.Name.Substring(0, 1).ToUpper() + pNation.ProtoSociety.Name.Substring(1);
                pPhenotypeM = pNation.PhenotypeMale;
                pPhenotypeF = pNation.PhenotypeFemale;
            }

            richTextBox1.AppendText(sRaceName + (pNation.IsAncient ? " is an ancient race." : " is a young race. "));
            if (pNation.IsInvader)
                richTextBox1.AppendText("They are not from this world. ");
            richTextBox1.AppendText("\n");

            richTextBox1.AppendText(sRaceName + " males " + pPhenotypeM.GetDescription());
            richTextBox1.AppendText("\n");
            richTextBox1.AppendText(sRaceName + " females " + pPhenotypeF.GetComparsion(pPhenotypeM.m_pValues));
            
            if (cKnownNations.Count > 1)
            {
                richTextBox1.AppendText("\n");
                richTextBox1.AppendText("Known " + pNation.Race.Name + " nations are: ");
                bool bFirst = true;
                foreach (Nation pKnownNation in cKnownNations)
                {
                    if (!bFirst)
                        richTextBox1.AppendText(", ");

                    bFirst = false;

                    richTextBox1.AppendText(pKnownNation.ProtoSociety.Name);
                }
                richTextBox1.AppendText(".\n\n");

                string sFenotypeNationM = pNation.PhenotypeMale.GetComparsion(pNation.Race.PhenotypeMale.m_pValues);
                //var expectedFenotypeF = GeneLab.Phenotype<LandTypeInfo>.ApplyDifferences(pSociety.m_pTitularNation.m_pPhenotypeM, pSociety.m_pTitularNation.m_pRace.m_pPhenotypeM, pSociety.m_pTitularNation.m_pRace.m_pPhenotypeF);
                string sFenotypeNationF = pNation.PhenotypeFemale.GetComparsion(pNation.Race.PhenotypeFemale.m_pValues);

                if (sFenotypeNationM == sFenotypeNationF)
                {
                    if (!sFenotypeNationM.StartsWith("are"))
                        sFenotypeNationM = "are common " + pNation.Race.Name + "s, however they " + sFenotypeNationM.Substring(0, 1).ToLower() + sFenotypeNationM.Substring(1);
                    richTextBox1.AppendText(pNation.ProtoSociety.Name + " " + sFenotypeNationM);
                }
                else
                {
                    if (sFenotypeNationM != "")
                    {
                        if (!sFenotypeNationM.StartsWith("are"))
                            sFenotypeNationM = "are common " + pNation.Race.Name + "s, however they " + sFenotypeNationM.Substring(0, 1).ToLower() + sFenotypeNationM.Substring(1);
                        richTextBox1.AppendText(pNation.ProtoSociety.Name + " males " + sFenotypeNationM);
                    }

                    if (sFenotypeNationF != "")
                    {
                        if (sFenotypeNationM != "")
                            richTextBox1.AppendText("\n");

                        if (!sFenotypeNationF.StartsWith("are"))
                            sFenotypeNationF = "are common " + pNation.Race.Name + "s, however they " + sFenotypeNationF.Substring(0, 1).ToLower() + sFenotypeNationF.Substring(1);
                        richTextBox1.AppendText(pNation.ProtoSociety.Name + " females " + sFenotypeNationF);
                    }
                }
            }
            richTextBox1.AppendText("\n\n");
        }

        private void worldMap1_StateSelectedEvent(object sender, MapDraw.SelectedStateChangedEventArgs e)
        {
            comboBox1.SelectedItem = e.State;

            var pSociety = e.State.m_pSociety;

            richTextBox1.Clear();

            richTextBox1.AppendText(string.Format("{0} {1}\n\n", e.State.m_pSociety.Name, pSociety.Polity.Name));

            richTextBox1.AppendText(string.Format("Major race: {2} [T{0}M{1}]\n\n", pSociety.TechLevel, pSociety.MagicLimit, pSociety.TitularNation));

            richTextBox1.AppendText(string.Format("Social order : {0} [C{1}]\n\n", Society.GetControlString(pSociety.Control), pSociety.DominantCulture.ProgressLevel));

            richTextBox1.AppendText(string.Format("Economic system : {0}\n\n", StateSociety.GetEqualityString(pSociety.SocialEquality))); 

            richTextBox1.AppendText(string.Format("Culture:\n"));
            foreach (Trait eMentality in Mentality.AllTraits)
            {
                richTextBox1.AppendText("   ");
                //richTextBox1.AppendText(string.Format("   {0}: \t", eMorale));
                //if (eMorale.ToString().Length < 6)
                //    richTextBox1.AppendText("\t");
                richTextBox1.AppendText(pSociety.DominantCulture.Mentality.GetTraitString(eMentality, pSociety.DominantCulture.ProgressLevel));
                //richTextBox1.AppendText(string.Format("{0:0.00}\n", e.State.m_pCulture.Moral[eMorale]));
                richTextBox1.AppendText("\n");
            }
            richTextBox1.AppendText("\n");

            AddNationInfo(pSociety.TitularNation);

            List<Nation> cDisplayedNations = new List<Nation>();
            cDisplayedNations.Add(pSociety.TitularNation);
            foreach (var pEstate in pSociety.Estates)
            {
                if (cDisplayedNations.Contains(pEstate.Value.TitularNation))
                    continue;

                AddNationInfo(pEstate.Value.TitularNation);
                cDisplayedNations.Add(pEstate.Value.TitularNation);
            }

            if (pSociety.GetImportedTech() == -1)
                richTextBox1.AppendText(string.Format("Available tech: {0} [T{1}]\n\n", Society.GetTechString(pSociety.TechLevel, pSociety.DominantCulture.Customs.ValueOf<Customs.Science>()), pSociety.GetEffectiveTech()));
            else
                richTextBox1.AppendText(string.Format("Available tech: {0} [T{1}]\n\n", pSociety.GetImportedTechString(), pSociety.GetImportedTech()));
            richTextBox1.AppendText(string.Format("Industrial base: {0} [T{1}]\n\n", Society.GetTechString(pSociety.TechLevel, pSociety.DominantCulture.Customs.ValueOf<Customs.Science>()), pSociety.GetEffectiveTech()));

            if (pSociety.MagicLimit > 0)
            {
                string sMagicAttitude = "regulated";
                if (pSociety.DominantCulture.Customs.Has(Customs.Magic.Magic_Feared))
                    sMagicAttitude = "outlawed";
                if (pSociety.DominantCulture.Customs.Has(Customs.Magic.Magic_Praised))
                    sMagicAttitude = "unlimited";
                richTextBox1.AppendText(string.Format("Magic users: {0}, ", sMagicAttitude));
                richTextBox1.AppendText(string.Format("{2}, up to {0} [M{1}]\n\n", Society.GetMagicString(pSociety.MagicLimit), pSociety.MagicLimit, pSociety.DominantCulture.MagicAbilityDistribution.ToString().Replace('_', ' ')));
            }
            else
            {
                string sMagicAttitude = "but allowed";
                if (pSociety.DominantCulture.Customs.Has(Customs.Magic.Magic_Feared))
                    sMagicAttitude = "and outlawed";
                if (pSociety.DominantCulture.Customs.Has(Customs.Magic.Magic_Praised))
                    sMagicAttitude = "but praised";
                richTextBox1.AppendText(string.Format("Magic users: none, {0} [M0]\n\n", sMagicAttitude));
            }

            richTextBox1.AppendText(string.Format("Resources: F:{0}, W:{1}, I:{2} / P:{3}\n\n", e.State.m_iFood, e.State.m_cResources[LandResource.Wood], e.State.m_cResources[LandResource.Ore], e.State.m_iLocationsCount));

            richTextBox1.AppendText("Estates: \n");

            int iTotalPop = 0;
            foreach (var pEstate in pSociety.Estates)
            {
                iTotalPop += pSociety.EstatesCounts[pEstate.Key];
            }

            {
                Estate pEstate = pSociety.Estates[Estate.SocialRank.Commoners];

                richTextBox1.AppendText("  - " + Estate.SocialRank.Commoners.ToString() + " aka " + pEstate.Name + " [" + string.Format("{0:D}", (int)(pSociety.EstatesCounts[Estate.SocialRank.Commoners] * 100.0f / iTotalPop)) + "%]: ");
                if (pEstate.TitularNation != pSociety.TitularNation)
                    richTextBox1.AppendText("Estate consists of enslaved " + pEstate.TitularNation + ".\n");

                richTextBox1.AppendText(pEstate.DominantCulture.Customs.GetCustomsDescription());

                string sMinorsCulture = pEstate.InferiorCulture.Mentality.GetDiffString(pEstate.InferiorCulture.ProgressLevel, pEstate.DominantCulture.Mentality, pEstate.DominantCulture.ProgressLevel);
                string sMinorsCustoms = pEstate.InferiorCulture.Customs.GetCustomsDiffString2(pEstate.DominantCulture.Customs);

                if (pEstate.IsSegregated() && (sMinorsCustoms != "" || sMinorsCulture != ""))
                {
                    string sMajors = pEstate.DominantCulture.Customs.Has(Customs.GenderPriority.Patriarchy) ? "males" : "females";
                    string sMinors = pEstate.DominantCulture.Customs.Has(Customs.GenderPriority.Patriarchy) ? "females" : "males";
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

                if (pEstate.GenderProfessionPreferences.Count > 0)
                {
                    richTextBox1.AppendText("\n    Prevalent professions:");
                    foreach (var pGenderPreference in pEstate.GenderProfessionPreferences)
                        richTextBox1.AppendText("\n         - " + (pGenderPreference.Value == Customs.GenderPriority.Matriarchy ? pGenderPreference.Key.NameFeminine : pGenderPreference.Key.NameMasculine));
                }
                richTextBox1.AppendText("\n\n");
            }

            foreach (var pEstate in pSociety.Estates)
            {
                if (pEstate.Key == Estate.SocialRank.Commoners)
                    continue;

                richTextBox1.AppendText("  - " + pEstate.Key.ToString() + " aka " + pEstate.Value.Name + " [" + string.Format("{0:D}", (int)(pSociety.EstatesCounts[pEstate.Key] * 100.0f / iTotalPop)) + "%]: ");
                if (pEstate.Value.TitularNation != pSociety.TitularNation)
                    richTextBox1.AppendText("Estate consists of " + (pEstate.Key == Estate.SocialRank.Lowlifes ? "enslaved " : "rebel ") + pEstate.Value.TitularNation + ".\n");

                string sCulture = pEstate.Value.DominantCulture.Mentality.GetDiffString(pEstate.Value.DominantCulture.ProgressLevel, pSociety.DominantCulture.Mentality, pSociety.DominantCulture.ProgressLevel);
                string sCustoms = pEstate.Value.DominantCulture.Customs.GetCustomsDiffString2(pSociety.DominantCulture.Customs);

                string sMinorsCulture = pEstate.Value.InferiorCulture.Mentality.GetDiffString(pEstate.Value.InferiorCulture.ProgressLevel, pEstate.Value.DominantCulture.Mentality, pEstate.Value.DominantCulture.ProgressLevel);
                string sMinorsCustoms = pEstate.Value.InferiorCulture.Customs.GetCustomsDiffString2(pEstate.Value.DominantCulture.Customs);

                string sGenderPriority = "";

                if (pEstate.Value.DominantCulture.Customs.Has(GenderPriority.Patriarchy))
                {
                    sGenderPriority = "patriarchal";
                }
                else if (pEstate.Value.DominantCulture.Customs.Has(GenderPriority.Matriarchy))
                {
                    sGenderPriority = "matriarchal";
                }
                else if (pEstate.Value.DominantCulture.Customs.Has(GenderPriority.Genders_equality))
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
                    string sMajors = pEstate.Value.DominantCulture.Customs.Has(Customs.GenderPriority.Patriarchy) ? "males" : "females";
                    string sMinors = pEstate.Value.DominantCulture.Customs.Has(Customs.GenderPriority.Patriarchy) ? "females" : "males";
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
                foreach (var pGenderPreference in pEstate.Value.GenderProfessionPreferences)
                    richTextBox1.AppendText("\n         - " + (pGenderPreference.Value == Customs.GenderPriority.Matriarchy ? pGenderPreference.Key.NameFeminine : pGenderPreference.Key.NameMasculine));
                richTextBox1.AppendText("\n\n");
            }

            State[] cEnemies = e.State.GetEnemiesList();
            if(cEnemies.Length > 0)
            {
                richTextBox1.AppendText("Enemies: ");
                bool bFirst = true;
                foreach (State pState in cEnemies)
                {
                    if(!bFirst)
                        richTextBox1.AppendText(", ");
                    richTextBox1.InsertLink(string.Format("{0} {1}", pState.m_pSociety.Name, pState.m_pSociety.Polity.Name), comboBox1.Items.IndexOf(pState).ToString());
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
                    richTextBox1.InsertLink(string.Format("{0} {1}", pState.m_pSociety.Name, pState.m_pSociety.Polity.Name), comboBox1.Items.IndexOf(pState).ToString());
                    bFirst = false;
                }
                richTextBox1.AppendText("\n\n");
            }
            else
                richTextBox1.AppendText("Allies: none\n\n");

            listBox1.Items.Clear();

            Dictionary<string, int> cPop = new Dictionary<string, int>();

            foreach(LocationX pLoc in e.State.m_pSociety.Settlements)
            {
                listBox1.Items.Add(pLoc);

                foreach (Building pBuilding in pLoc.Settlement.Buildings)
                {
                    if (pBuilding.Info.Size == BuildingSize.Unique)
                        continue;

                    int iPop = 0;
                    cPop.TryGetValue(pBuilding.Info.OwnerProfession.NameMasculine, out iPop);
                    cPop[pBuilding.Info.OwnerProfession.NameMasculine] = iPop + pBuilding.Info.OwnersCount;

                    iPop = 0;
                    cPop.TryGetValue(pBuilding.Info.WorkersProfession.NameMasculine, out iPop);
                    cPop[pBuilding.Info.WorkersProfession.NameMasculine] = iPop + pBuilding.Info.WorkersCount;
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

            if (m_pGenerationForm.Preload(settings))
            {
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

                        string sTip = string.Format("{0} {1} to {2} {3}:\n", mapDraw1.SelectedState.m_pSociety.Name, mapDraw1.SelectedState.m_pSociety.Polity.Name, pLinkState.m_pSociety.Name, pLinkState.m_pSociety.Polity.Name);
                        string sRelation;
                        mapDraw1.SelectedState.m_pSociety.CalcHostility(pLinkState, out sRelation);

                        sTip += sRelation;
                        sTip += "\n";

                        sTip += string.Format("{2} {3} to {0} {1}:\n", mapDraw1.SelectedState.m_pSociety.Name, mapDraw1.SelectedState.m_pSociety.Polity.Name, pLinkState.m_pSociety.Name, pLinkState.m_pSociety.Polity.Name);
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

        Location m_pTPFStart = null;
        Location m_pTPFFinish = null;

        private void ToolStripMenuItem_TestPathFinding1_Click(object sender, EventArgs e)
        {
            do
            {
                m_pTPFStart = m_pWorld.LocationsGrid.Locations[Rnd.Get(m_pWorld.LocationsGrid.Locations.Length)];
            }
            while (m_pTPFStart.Forbidden || 
                   m_pTPFStart.GetOwner().IsWater || 
                   m_pTPFStart.As<LocationX>().Settlement == null || 
                   m_pTPFStart.As<LocationX>().Settlement.RuinsAge > 0);

            do
            {
                m_pTPFFinish = m_pWorld.LocationsGrid.Locations[Rnd.Get(m_pWorld.LocationsGrid.Locations.Length)];
            }
            while (m_pTPFFinish.Forbidden || 
                   m_pTPFFinish == m_pTPFStart || 
                   m_pTPFFinish.GetOwner().IsWater || 
                   ((m_pTPFFinish.As<LocationX>().Building == null || 
                     (m_pTPFFinish.As<LocationX>().Building.Type != BuildingType.Hideout && 
                      m_pTPFFinish.As<LocationX>().Building.Type != BuildingType.Lair)) && 
                    m_pTPFFinish.As<LocationX>().Settlement == null));

            mapDraw1.ClearPath();

            ShortestPath pPath1 = World.FindReallyBestPath(m_pTPFStart, m_pTPFFinish, m_pWorld.LocationsGrid.CycleShift, false);
            mapDraw1.AddPath(pPath1.Nodes, Color.Fuchsia);
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

        private void hideMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mapDraw1.Visible = !mapDraw1.Visible;
            miniMapDraw1.Visible = !miniMapDraw1.Visible;

            hideMapToolStripMenuItem.Checked = !hideMapToolStripMenuItem.Checked;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            //==========================================DEBUG=================================================
            hideMapToolStripMenuItem_Click(this, null);
            //==========================================DEBUG=================================================
        }
    }
}
