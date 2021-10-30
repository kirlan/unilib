using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GeneLab;
using GeneLab.Genetix;
using LandscapeGeneration;
using nsUniLibControls;
using Socium;


namespace GeneLabTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            LandTypes<LandTypeInfoX>.Coastral.Init(10, 1, EnvironmentType.Water, "sea");
            LandTypes<LandTypeInfoX>.Coastral.SetColor(Color.FromArgb(0x27, 0x67, 0x71));//(0x2a, 0x83, 0x93);//(0x36, 0xa9, 0xbd);//FromArgb(0xa2, 0xed, 0xfa);//LightSkyBlue;//LightCyan;

            LandTypes<LandTypeInfoX>.Ocean.Init(10, 5, EnvironmentType.Water, "ocean");
            LandTypes<LandTypeInfoX>.Ocean.SetColor(Color.FromArgb(0x1e, 0x5e, 0x69));//(0x2a, 0x83, 0x93);//(0x36, 0xa9, 0xbd);//FromArgb(0xa2, 0xed, 0xfa);//LightSkyBlue;//LightCyan;

            LandTypes<LandTypeInfoX>.Plains.Init(1, 1, EnvironmentType.Ground, "plains");
            LandTypes<LandTypeInfoX>.Plains.SetColor(Color.FromArgb(0xd3, 0xfa, 0x5f));//(0xdc, 0xfa, 0x83);//LightGreen;

            LandTypes<LandTypeInfoX>.Savanna.Init(1, 1, EnvironmentType.Ground, "savanna");
            LandTypes<LandTypeInfoX>.Savanna.SetColor(Color.FromArgb(0xf0, 0xff, 0x8a));//(0xbd, 0xb0, 0x6b);//PaleGreen;

            LandTypes<LandTypeInfoX>.Tundra.Init(2, 0.5f, EnvironmentType.Ground, "tundra");
            LandTypes<LandTypeInfoX>.Tundra.SetColor(Color.FromArgb(0xc9, 0xff, 0xff));//(0xc9, 0xe0, 0xff);//PaleGreen;

            LandTypes<LandTypeInfoX>.Desert.Init(2, 0.1f, EnvironmentType.Ground, "desert");
            LandTypes<LandTypeInfoX>.Desert.SetColor(Color.FromArgb(0xfa, 0xdc, 0x36));//(0xf9, 0xfa, 0x8a);//LightYellow;

            LandTypes<LandTypeInfoX>.Forest.Init(3, 2, EnvironmentType.Ground, "forest");
            LandTypes<LandTypeInfoX>.Forest.SetColor(Color.FromArgb(0x56, 0x78, 0x34));//(0x63, 0x78, 0x4e);//LightGreen;//ForestGreen;

            LandTypes<LandTypeInfoX>.Taiga.Init(3, 2, EnvironmentType.Ground, "taiga");
            LandTypes<LandTypeInfoX>.Taiga.SetColor(Color.FromArgb(0x63, 0x78, 0x4e));//LightGreen;//ForestGreen;

            LandTypes<LandTypeInfoX>.Swamp.Init(4, 0.1f, EnvironmentType.Ground, "swamp");
            LandTypes<LandTypeInfoX>.Swamp.SetColor(Color.FromArgb(0xa7, 0xbd, 0x6b));// DarkKhaki;

            LandTypes<LandTypeInfoX>.Mountains.Init(5, 10, EnvironmentType.Mountains, "mountains");
            LandTypes<LandTypeInfoX>.Mountains.SetColor(Color.FromArgb(0xbd, 0x6d, 0x46));//Tan;

            LandTypes<LandTypeInfoX>.Jungle.Init(6, 2, EnvironmentType.Ground, "jungle");
            LandTypes<LandTypeInfoX>.Jungle.SetColor(Color.FromArgb(0x8d, 0xb7, 0x31));//(0x72, 0x94, 0x28);//PaleGreen;
        }

        private void Mutate(Phenotype<LandTypeInfoX> pFenotype, string sName)
        {
            richTextBox1.Clear();

            richTextBox1.AppendText("Original race: " + sName + ".\n\r");
            richTextBox1.AppendText(sName + " " + pFenotype.GetDescription() + "\n\r");

            Phenotype<LandTypeInfoX> pMutant;

            do
            {
                pMutant = (Phenotype<LandTypeInfoX>)pFenotype.MutateRace();
            }
            while (pMutant.IsIdentical(pFenotype));

            richTextBox1.AppendText("Original race mutations:\n\r");
            richTextBox1.AppendText("They " + pMutant.GetComparsion(pFenotype) + "\n\r\n\r");

            pFenotype = pMutant;

            List<Phenotype<LandTypeInfoX>> cMutants = new List<Phenotype<LandTypeInfoX>>();
            for (int i = 0; i < 5; i++)
            {
                bool bNew = false;
                do
                {
                    pMutant = (Phenotype<LandTypeInfoX>)pFenotype.MutateNation();

                    bNew = !pMutant.IsIdentical(pFenotype);

                    foreach (Phenotype<LandTypeInfoX> pOldMutation in cMutants)
                        if (pMutant.IsIdentical(pOldMutation))
                            bNew = false;

                }
                while (!bNew);

                richTextBox1.AppendText("Nation " + (i + 1).ToString() + ":\n\n");
                richTextBox1.AppendText(pMutant.GetComparsion(pFenotype) + "\n\n");

                cMutants.Add(pMutant);

                LandTypeInfoX[] cPreferred;
                LandTypeInfoX[] cHated;

                pMutant.GetTerritoryPreferences(out cPreferred, out cHated);

                bool bSemicolon = false;
                richTextBox1.AppendText("Preferred territories: ");
                foreach (LandTypeInfoX pLand in cPreferred)
                {
                    if (bSemicolon)
                        richTextBox1.AppendText(", ");
                    richTextBox1.AppendText(pLand.m_sName);
                    bSemicolon = true;
                }
                richTextBox1.AppendText("\n");
                
                bSemicolon = false;
                richTextBox1.AppendText("Hated territories: ");
                foreach (LandTypeInfoX pLand in cHated)
                {
                    if (bSemicolon)
                        richTextBox1.AppendText(", ");
                    richTextBox1.AppendText(pLand.m_sName);
                    bSemicolon = true;
                }
                richTextBox1.AppendText("\n\r");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Phenotype<LandTypeInfoX> pFenotype = new Fenotype<LandTypeInfoX>(BodyGenetix.Human,
                                                HeadGenetix.Human,
                                                LegsGenetix.Human,
                                                ArmsGenetix.Human,
                                                WingsGenetix.None,
                                                TailGenetix.None,
                                                HideGenetix.HumanWhite,
                                                BrainGenetix.HumanFantasy,
                                                LifeCycleGenetix.Human,
                                                FaceGenetix.Human,
                                                EarsGenetix.Human,
                                                EyesGenetix.Human,
                                                HairsGenetix.HumanWhite);

            Mutate(pFenotype, "HUMANS");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Phenotype<LandTypeInfoX> pFenotype = new Fenotype<LandTypeInfoX>(BodyGenetix.Human,
                                                HeadGenetix.Human,
                                                LegsGenetix.Human,
                                                ArmsGenetix.Human,
                                                WingsGenetix.None,
                                                TailGenetix.None,
                                                HideGenetix.HumanBlack,
                                                BrainGenetix.HumanFantasy,
                                                LifeCycleGenetix.Human,
                                                FaceGenetix.Human,
                                                EarsGenetix.Human,
                                                EyesGenetix.Human,
                                                HairsGenetix.HumanBlack);

            Mutate(pFenotype, "NEGROS");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Phenotype<LandTypeInfoX> pFenotype = new Phenotype<LandTypeInfoX>(BodyGenetix.Elf,
                                                HeadGenetix.Human,
                                                LegsGenetix.Human,
                                                ArmsGenetix.Human,
                                                WingsGenetix.None,
                                                TailGenetix.None,
                                                HideGenetix.HumanWhite,
                                                BrainGenetix.Elf,
                                                LifeCycleGenetix.Elf,
                                                FaceGenetix.Human,
                                                EarsGenetix.Elf,
                                                EyesGenetix.Human,
                                                HairsGenetix.Elf);

            Mutate(pFenotype, "ELVES");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Phenotype<LandTypeInfoX> pFenotype = new Fenotype<LandTypeInfoX>(BodyGenetix.Dwarf,
                                                HeadGenetix.Human,
                                                LegsGenetix.Human,
                                                ArmsGenetix.Human,
                                                WingsGenetix.None,
                                                TailGenetix.None,
                                                HideGenetix.HumanWhite,
                                                BrainGenetix.HumanFantasy,
                                                LifeCycleGenetix.Dwarf,
                                                FaceGenetix.Human,
                                                EarsGenetix.Human,
                                                EyesGenetix.Human,
                                                HairsGenetix.Dwarf);

            Mutate(pFenotype, "DWARVES");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Phenotype<LandTypeInfoX> pFenotype = new Phenotype<LandTypeInfoX>(BodyGenetix.Orc,
                                                HeadGenetix.Pitecantrop,
                                                LegsGenetix.Human,
                                                ArmsGenetix.Human,
                                                WingsGenetix.None,
                                                TailGenetix.None,
                                                HideGenetix.Orc,
                                                BrainGenetix.Barbarian,
                                                LifeCycleGenetix.Orc,
                                                FaceGenetix.Human,
                                                EarsGenetix.Elf,
                                                EyesGenetix.Human,
                                                HairsGenetix.None);

            Mutate(pFenotype, "ORCS");
        }
    }
}
