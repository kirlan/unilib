using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;
using Socium.Psichology;
using Socium.Settlements;

namespace Socium.Population
{
    public class Estate
    {
        public enum Position
        {
            Elite = 2,
            Middle = 1,
            Low = 0,
            Outlaw = -1
        }

        public Position m_ePosition;

        public bool IsOutlaw()
        {
            return m_ePosition == Position.Outlaw;
        }

        public bool IsElite()
        {
            return m_ePosition == Position.Elite;
        }

        public bool IsMiddleUp()
        {
            return m_ePosition == Position.Middle || m_ePosition == Position.Elite;
        }

        public string m_sName;

        public List<Strata> m_cStratas = new List<Strata>();

        private Society m_pSociety;

        public int m_iCultureLevel = 0;
        public Culture m_pCulture = null;
        public Customs m_pCustoms = null;

        public Customs m_pMinorsCustoms = null;

        public Customs GetCustoms(Person._Gender eGender)
        {
            if (m_pCustoms.m_eGenderPriority == Customs.GenderPriority.Genders_equality ||
                (m_pCustoms.m_eGenderPriority == Customs.GenderPriority.Patriarchy && eGender == Person._Gender.Male) ||
                (m_pCustoms.m_eGenderPriority == Customs.GenderPriority.Matriarchy && eGender == Person._Gender.Female))
                return m_pCustoms;
            else
                return m_pMinorsCustoms;
        }

        public bool IsSegregated()
        {
            return m_pCustoms.m_eGenderPriority != Customs.GenderPriority.Genders_equality;
        }

        /// <summary>
        /// Создаёт новое сословие, основываясь на заданном. Новое сословие в целом разделяет все культурные ценности и обычаи заданного, но возможны отклонения.
        /// </summary>
        /// <param name="pEstate"></param>
        /// <param name="ePosition"></param>
        public Estate(Estate pEstate, Position ePosition)
        {
            m_ePosition = ePosition;

            m_pSociety = pEstate.m_pSociety;

            //m_pCulture = new Culture(pEstate.m_pCulture);
            m_pCulture = pEstate.m_pCulture;

            m_iCultureLevel = m_pSociety.m_iCultureLevel;

            if (ePosition == Position.Outlaw)
                m_iCultureLevel--;
            if (ePosition == Position.Elite)
                m_iCultureLevel++;

            if (m_iCultureLevel < 0)
                m_iCultureLevel = 0;
            if (m_iCultureLevel > 8)
                m_iCultureLevel = 8;

            m_pCustoms = new Customs(pEstate.m_pCustoms, Customs.Mutation.Mandatory);
            m_pMinorsCustoms = new Customs(Customs.Merge(m_pCustoms, pEstate.m_pCustoms, pEstate.m_pMinorsCustoms), Customs.Mutation.Mandatory);
            m_pMinorsCustoms.m_eMarriage = m_pCustoms.m_eMarriage;
            m_pMinorsCustoms.m_eGenderPriority = m_pCustoms.m_eGenderPriority;

            Person.GetSkillPreferences(m_pCulture, m_iCultureLevel, m_pCustoms, ref m_eMostRespectedSkill, ref m_eLeastRespectedSkill);

            m_sName = m_pSociety.GetEstateName(m_ePosition);
        }

        /// <summary>
        /// Создаёт основное сословие для указанного государства. Культура и обычаи основного сословия всегда совпадают с культурой и обычаями самого государства.
        /// </summary>
        /// <param name="pSociety"></param>
        public Estate(Society pSociety, Position ePosition)
        {
            m_ePosition = ePosition;

            m_pSociety = pSociety;

            m_pCulture = m_pSociety.m_pCulture;
            m_iCultureLevel = m_pSociety.m_iCultureLevel;

            if (ePosition == Position.Outlaw)
                m_iCultureLevel--;
            if (ePosition == Position.Elite)
                m_iCultureLevel++;

            if (m_iCultureLevel < 0)
                m_iCultureLevel = 0;
            if (m_iCultureLevel > 8)
                m_iCultureLevel = 8;

            m_pCustoms = m_pSociety.m_pCustoms;
            m_pMinorsCustoms = new Customs(m_pCustoms, Customs.Mutation.Mandatory);
            m_pMinorsCustoms.m_eMarriage = m_pCustoms.m_eMarriage;
            m_pMinorsCustoms.m_eGenderPriority = m_pCustoms.m_eGenderPriority;

            Person.GetSkillPreferences(m_pCulture, m_iCultureLevel, m_pCustoms, ref m_eMostRespectedSkill, ref m_eLeastRespectedSkill);

            m_sName = m_pSociety.GetEstateName(m_ePosition);
        }

        public void FixStratums()
        {
            foreach (Strata pStrata in m_cStratas)
                pStrata.FixGenderPriority(this);
        }

        private Person.Skill m_eMostRespectedSkill;

        /// <summary>
        /// Наиболее престижный навык в рамках сословия. Имеет меньший приоритет, чем аналогичное значение для государства в целом!
        /// Например, в обществе в целом могут цениться умники, но в правящей верхушке может считаться, что ум - это конечно важно, но хорошая физическая форма тоже очень ценна.
        /// </summary>
        public Person.Skill MostRespectedSkill
        {
            get { return m_eMostRespectedSkill; }
        }

        private Person.Skill m_eLeastRespectedSkill;

        /// <summary>
        /// Наименее престижный навык в рамках сословия. Имеет меньший приоритет, чем аналогичное значение для государства в целом!
        /// </summary>
        public Person.Skill LeastRespectedSkill
        {
            get { return m_eLeastRespectedSkill; }
        }

        /// <summary>
        /// Возвращает уровень престижности указанной профессии в соответствии с отношением в обществе к требуемым в ней навыкам
        /// </summary>
        /// <param name="pProfession"></param>
        /// <returns></returns>
        public int GetProfessionPreference(ProfessionInfo pProfession)
        {
            int iPreference = 0;

            switch (pProfession.m_cSkills[m_eMostRespectedSkill])
            {
                case ProfessionInfo.SkillLevel.Bad:
                    iPreference++;
                    break;
                case ProfessionInfo.SkillLevel.Good:
                    iPreference += 2;
                    break;
                case ProfessionInfo.SkillLevel.Excellent:
                    iPreference += 3;
                    break;
            }
            switch (pProfession.m_cSkills[m_eLeastRespectedSkill])
            {
                case ProfessionInfo.SkillLevel.Bad:
                    iPreference--;
                    break;
                case ProfessionInfo.SkillLevel.Good:
                    iPreference -= 2;
                    break;
                case ProfessionInfo.SkillLevel.Excellent:
                    iPreference -= 3;
                    break;
            }

            return iPreference;
        }
    }
}
