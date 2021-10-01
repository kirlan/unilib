using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeneLab.Genetix;
using Random;
using Socium.Psychology;
using Socium.Settlements;

namespace Socium.Population
{
    /// <summary>
    /// Сословие - является объединением страт, имеющих сходный доступный уровень жизни и общественное уважение
    /// </summary>
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

        public Dictionary<ProfessionInfo, Customs.GenderPriority> m_cGenderProfessionPreferences = new Dictionary<ProfessionInfo, Customs.GenderPriority>();

        private Society m_pSociety;

        public Culture m_pMajorsCreed = null;
        public Culture m_pMinorsCreed = null;

        public Culture GetCreed(Gender eGender)
        {
            if (m_pMajorsCreed.m_pCustoms.m_eGenderPriority == Customs.GenderPriority.Genders_equality ||
                (m_pMajorsCreed.m_pCustoms.m_eGenderPriority == Customs.GenderPriority.Patriarchy && eGender == Gender.Male) ||
                (m_pMajorsCreed.m_pCustoms.m_eGenderPriority == Customs.GenderPriority.Matriarchy && eGender == Gender.Female))
                return m_pMajorsCreed;
            else
                return m_pMinorsCreed;
        }

        public bool IsSegregated()
        {
            return m_pMajorsCreed.m_pCustoms.m_eGenderPriority != Customs.GenderPriority.Genders_equality;
        }

        void UpdateCreed(Culture pCreed, Position ePosition)
        {
            if (ePosition == Position.Outlaw)
                pCreed.m_iProgressLevel--;
            if (ePosition == Position.Elite)
                pCreed.m_iProgressLevel++;

            if (pCreed.m_iProgressLevel < 0)
                pCreed.m_iProgressLevel = 0;
            if (pCreed.m_iProgressLevel > 8)
                pCreed.m_iProgressLevel = 8;
        }

        /// <summary>
        /// Создаёт новое сословие, основываясь на заданном. Новое сословие в целом разделяет все культурные ценности и обычаи заданного, но возможны отклонения.
        /// </summary>
        /// <param name="pEstate"></param>
        /// <param name="ePosition"></param>
        public Estate(Estate pEstate, Position ePosition)
        {
            m_pMajorsCreed = new Culture(pEstate.m_pMajorsCreed);

            m_ePosition = ePosition;

            m_pSociety = pEstate.m_pSociety;

            UpdateCreed(m_pMajorsCreed, ePosition);

            m_pMinorsCreed = new Culture(pEstate.m_pMinorsCreed);

            UpdateCreed(m_pMinorsCreed, ePosition);

            m_pMinorsCreed.m_pCustoms.m_eMarriage = m_pMajorsCreed.m_pCustoms.m_eMarriage;
            m_pMinorsCreed.m_pCustoms.m_eGenderPriority = m_pMajorsCreed.m_pCustoms.m_eGenderPriority;

            Person.GetSkillPreferences(m_pMajorsCreed, ref m_eMostRespectedSkill, ref m_eLeastRespectedSkill);

            m_sName = m_pSociety.GetEstateName(m_ePosition);
        }

        /// <summary>
        /// Создаёт основное сословие для указанного государства. Культура и обычаи основного сословия всегда совпадают с культурой и обычаями самого государства.
        /// </summary>
        /// <param name="pSociety"></param>
        public Estate(Society pSociety, Position ePosition)
        {
            m_pMajorsCreed = new Culture(pSociety.m_pCulture);
            
            m_ePosition = ePosition;

            m_pSociety = pSociety;

            UpdateCreed(m_pMajorsCreed, ePosition);
            
            m_pMinorsCreed = new Culture(m_pMajorsCreed);
            m_pMinorsCreed.m_pCustoms.m_eMarriage = m_pMajorsCreed.m_pCustoms.m_eMarriage;
            m_pMinorsCreed.m_pCustoms.m_eGenderPriority = m_pMajorsCreed.m_pCustoms.m_eGenderPriority;

            Person.GetSkillPreferences(m_pMajorsCreed, ref m_eMostRespectedSkill, ref m_eLeastRespectedSkill);

            m_sName = m_pSociety.GetEstateName(m_ePosition);
        }

        public void FixGenderProfessionPreferences()
        {
            var cProfessions = m_cGenderProfessionPreferences.Keys.ToArray();
            foreach (var pProfession in cProfessions)
            {
                // По умолчанию - гендерные предпочтения страты совпадают представлениями сословия о "сильном" поле.
                var eGenderPriority = m_pMajorsCreed.m_pCustoms.m_eGenderPriority;

                // но, если это подчинённая должность...
                if (!pProfession.m_bMaster)
                {
                    // ...связанная с тем, чтобы нравиться клиенту...
                    if (pProfession.m_cSkills[Person.Skill.Charm] != ProfessionInfo.SkillLevel.None)
                    {
                        // ...то в гетеросексуальном обществе она считается более подходящей "слабому" полу
                        if (m_pMajorsCreed.m_pCustoms.m_eSexRelations == Customs.SexualOrientation.Heterosexual)
                        {
                            if (eGenderPriority == Customs.GenderPriority.Patriarchy)
                                eGenderPriority = Customs.GenderPriority.Matriarchy;
                            else if (eGenderPriority == Customs.GenderPriority.Matriarchy)
                                eGenderPriority = Customs.GenderPriority.Patriarchy;
                        }
                        // ...а в бисексуальном обществе - такая профессия так же бисексуальна
                        else if (m_pMajorsCreed.m_pCustoms.m_eSexRelations == Customs.SexualOrientation.Bisexual)
                            eGenderPriority = Customs.GenderPriority.Genders_equality;
                    }
                    else
                    {
                        // ...если профессия НЕ связанна с тем, чтобы нравиться клиенту -
                        // смотрим, насколько она считается престижной в данном сословии.
                        // более престижные профессии - считаются подходящими "сильному" полу
                        int iPreference = GetProfessionPreference(pProfession);

                        if (iPreference == 0)
                            eGenderPriority = Customs.GenderPriority.Genders_equality;
                        // менее престижные профессии - считаются подходящими "слабому" полу
                        if (iPreference < 0)
                            eGenderPriority = m_pSociety.GetMinorGender(eGenderPriority);
                    }
                }

                m_cGenderProfessionPreferences[pProfession] = eGenderPriority;
            }
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
