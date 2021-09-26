using Socium.Psichology;
using Socium.Settlements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Socium
{
    /// <summary>
    /// Страта - группа граждан одного государства, объединённых по профессиональному и гендерному признаку.
    /// </summary>
    public class Strata
    {
        public Customs.GenderPriority m_eGenderPriority = Customs.GenderPriority.Genders_equality;

        public Society m_pSociety = null;

        public ProfessionInfo m_pProfession = null;

        public Strata(Society pSociety, ProfessionInfo pProfession)
        {
            m_pSociety = pSociety;
            m_pProfession = pProfession;

            m_eGenderPriority = m_pSociety.m_pCustoms.m_eGenderPriority;

            if (!m_pProfession.m_bMaster)
            {
                if (m_pProfession.m_cSkills[Person.Skill.Charm] != ProfessionInfo.SkillLevel.None)
                {
                    if (m_pSociety.m_pCustoms.m_eSexRelations == Customs.SexualOrientation.Heterosexual)
                    {
                        if (m_eGenderPriority == Customs.GenderPriority.Patriarchy)
                            m_eGenderPriority = Customs.GenderPriority.Matriarchy;
                        else if (m_eGenderPriority == Customs.GenderPriority.Matriarchy)
                            m_eGenderPriority = Customs.GenderPriority.Patriarchy;
                    }
                    if (m_pSociety.m_pCustoms.m_eSexRelations == Customs.SexualOrientation.Bisexual)
                        m_eGenderPriority = Customs.GenderPriority.Genders_equality;
                }
                else
                {
                    int iPreference = m_pSociety.GetProfessionPreference(m_pProfession);

                    if (iPreference == 0)
                        m_eGenderPriority = Customs.GenderPriority.Genders_equality;
                    if (iPreference < 0)
                        m_eGenderPriority = m_pSociety.FixGenderPriority(m_eGenderPriority);
                }
            }
        }

        public void FixGenderPriority(CEstate pEstate)
        {
            m_eGenderPriority = pEstate.m_pCustoms.m_eGenderPriority;

            if (!m_pProfession.m_bMaster)
            {
                if (m_pProfession.m_cSkills[Person.Skill.Charm] != ProfessionInfo.SkillLevel.None)
                {
                    if (pEstate.m_pCustoms.m_eSexRelations == Customs.SexualOrientation.Heterosexual)
                    {
                        if (m_eGenderPriority == Customs.GenderPriority.Patriarchy)
                            m_eGenderPriority = Customs.GenderPriority.Matriarchy;
                        else if (m_eGenderPriority == Customs.GenderPriority.Matriarchy)
                            m_eGenderPriority = Customs.GenderPriority.Patriarchy;
                    }
                    if (pEstate.m_pCustoms.m_eSexRelations == Customs.SexualOrientation.Bisexual)
                        m_eGenderPriority = Customs.GenderPriority.Genders_equality;
                }
                else
                {
                    int iPreference = pEstate.GetProfessionPreference(m_pProfession);

                    if (iPreference == 0)
                        m_eGenderPriority = Customs.GenderPriority.Genders_equality;
                    if (iPreference < 0)
                        m_eGenderPriority = m_pSociety.FixGenderPriority(m_eGenderPriority);
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Strata))
                return false;

            Strata pOther = (Strata)obj;

            return m_pSociety == pOther.m_pSociety &&
                m_pProfession == pOther.m_pProfession &&
                m_eGenderPriority == pOther.m_eGenderPriority;
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", m_eGenderPriority == Customs.GenderPriority.Matriarchy ? m_pProfession.m_sNameF : m_pProfession.m_sNameM,
                m_eGenderPriority == Customs.GenderPriority.Matriarchy ? "F" : (m_eGenderPriority == Customs.GenderPriority.Patriarchy ? "M" : "M/F"));
        }
    }
}
