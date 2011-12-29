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

            LandTypes<LandTypeInfoX>.Coastral.Init(10, EnvironmentType.Water, "sea");

            LandTypes<LandTypeInfoX>.Ocean.Init(10, EnvironmentType.Water, "ocean");

            LandTypes<LandTypeInfoX>.Plains.Init(1, EnvironmentType.Ground, "plains");

            LandTypes<LandTypeInfoX>.Savanna.Init(1, EnvironmentType.Ground, "savanna");

            LandTypes<LandTypeInfoX>.Tundra.Init(2, EnvironmentType.Ground, "tundra");

            LandTypes<LandTypeInfoX>.Desert.Init(2, EnvironmentType.Ground, "desert");

            LandTypes<LandTypeInfoX>.Forest.Init(3, EnvironmentType.Ground, "forest");

            LandTypes<LandTypeInfoX>.Taiga.Init(3, EnvironmentType.Ground, "taiga");

            LandTypes<LandTypeInfoX>.Swamp.Init(4, EnvironmentType.Ground, "swamp");

            LandTypes<LandTypeInfoX>.Mountains.Init(5, EnvironmentType.Mountains, "mountains");

            LandTypes<LandTypeInfoX>.Jungle.Init(6, EnvironmentType.Ground, "jungle");
        }

        private void Mutate(Fenotype<LandTypeInfoX> pFenotype, string sName)
        {
            richTextBox1.Clear();

            richTextBox1.AppendText("Original race: " + sName + ".\n\r");
            richTextBox1.AppendText(sName + " " + pFenotype.GetDescription() + "\n\r");

            Fenotype<LandTypeInfoX> pMutant;

            do
            {
                pMutant = (Fenotype<LandTypeInfoX>)pFenotype.MutateRace();
            }
            while (pMutant.IsIdentical(pFenotype));

            richTextBox1.AppendText("Original race mutations:\n\r");
            richTextBox1.AppendText("They " + pMutant.GetComparsion(pFenotype) + "\n\r\n\r");

            pFenotype = pMutant;

            List<Fenotype<LandTypeInfoX>> cMutants = new List<Fenotype<LandTypeInfoX>>();
            for (int i = 0; i < 5; i++)
            {
                bool bNew = false;
                do
                {
                    pMutant = (Fenotype<LandTypeInfoX>)pFenotype.MutateNation();

                    bNew = !pMutant.IsIdentical(pFenotype);

                    foreach (Fenotype<LandTypeInfoX> pOldMutation in cMutants)
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
            Fenotype<LandTypeInfoX> pFenotype = new Fenotype<LandTypeInfoX>(BodyGenetix.Human,
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
            Fenotype<LandTypeInfoX> pFenotype = new Fenotype<LandTypeInfoX>(BodyGenetix.Human,
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
            Fenotype<LandTypeInfoX> pFenotype = new Fenotype<LandTypeInfoX>(BodyGenetix.Elf,
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
            Fenotype<LandTypeInfoX> pFenotype = new Fenotype<LandTypeInfoX>(BodyGenetix.Dwarf,
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
            Fenotype<LandTypeInfoX> pFenotype = new Fenotype<LandTypeInfoX>(BodyGenetix.Orc,
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
