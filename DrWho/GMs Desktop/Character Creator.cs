using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AITAS_Engine;
using AITAS_Engine.Skills;

namespace GMs_Desktop
{
    public partial class Character_Creator : Form
    {
        Character m_pCharacter = new Character();

        public Character_Creator()
        {
            InitializeComponent();

            AthleticsExpertise.Items.AddRange(m_pCharacter.m_cSkills[Skl.Athletics].AllExpertises);
            ConvinceExpertise.Items.AddRange(m_pCharacter.m_cSkills[Skl.Convince].AllExpertises);
            CraftExpertise.Items.AddRange(m_pCharacter.m_cSkills[Skl.Craft].AllExpertises);
            CreationExpertise.Items.AddRange(m_pCharacter.m_cSkills[Skl.Creation].AllExpertises);
            EntertainmentExpertise.Items.AddRange(m_pCharacter.m_cSkills[Skl.Entertainment].AllExpertises);
            FightingExpertise.Items.AddRange(m_pCharacter.m_cSkills[Skl.Fighting].AllExpertises);
            KnowledgeExpertise.Items.AddRange(m_pCharacter.m_cSkills[Skl.Knowledge].AllExpertises);
            MarksmanExpertise.Items.AddRange(m_pCharacter.m_cSkills[Skl.Marksman].AllExpertises);
            MedicineExpertise.Items.AddRange(m_pCharacter.m_cSkills[Skl.Medicine].AllExpertises);
            ScienceExpertise.Items.AddRange(m_pCharacter.m_cSkills[Skl.Science].AllExpertises);
            SubterfugeExpertise.Items.AddRange(m_pCharacter.m_cSkills[Skl.Subterfuge].AllExpertises);
            SurvivalExpertise.Items.AddRange(m_pCharacter.m_cSkills[Skl.Survival].AllExpertises);
            TechnologyExpertise.Items.AddRange(m_pCharacter.m_cSkills[Skl.Technology].AllExpertises);
            TransportExpertise.Items.AddRange(m_pCharacter.m_cSkills[Skl.Transport].AllExpertises);

            CommonGoodTraits.Items.Clear();
            foreach (Trait pTrait in Trait.s_aGoodTraits)
                CommonGoodTraits.Items.Add(pTrait);

            CommonBadTraits.Items.Clear();
            foreach (Trait pTrait in Trait.s_aBadTraits)
                CommonBadTraits.Items.Add(pTrait);

            SpecialTraits.Items.Clear();
            foreach (Trait pTrait in Trait.s_aSpecialTraits)
                SpecialTraits.Items.Add(pTrait);

            AlienTraits.Items.Clear();
            foreach (Trait pTrait in Trait.s_aAlienTraits)
                AlienTraits.Items.Add(pTrait);
        }

        private void tabPage5_Click(object sender, EventArgs e)
        {

        }

        private void CommonGoodTraits_MouseHover(object sender, EventArgs e)
        {

        }

        private Trait m_pTraitUnderCursor = null;

        private void Traits_MouseMove(object sender, MouseEventArgs e)
        {
            CheckedListBox pBox = sender as CheckedListBox;

            if (pBox == null)
                return;

            int index = -1;
            for (int i = 0; i < pBox.Items.Count; i++)
                if (pBox.GetItemRectangle(i).Contains(e.Location))
                {
                    index = i;
                    break;
                }

            if (index == -1)
            {
                toolTip1.SetToolTip(pBox, "");
                m_pTraitUnderCursor = null;
            }
            else
                if (m_pTraitUnderCursor != pBox.Items[index])
                {
                    m_pTraitUnderCursor = pBox.Items[index] as Trait;
                    toolTip1.SetToolTip(pBox, m_pTraitUnderCursor.m_sDescription);
                }
        }

        private void CommonBadTraits_MouseMove(object sender, MouseEventArgs e)
        {

        }
    }
}
