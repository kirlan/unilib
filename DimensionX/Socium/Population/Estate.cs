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
    public class Estate: NationalSociety
    {
        public enum Position
        {
            Elite = 2,
            Commoners = 1,
            Lowlifes = 0,
            Outlaws = -1
        }

        public Position m_ePosition;

        public bool IsOutlaw()
        {
            return m_ePosition == Position.Outlaws;
        }

        public bool IsElite()
        {
            return m_ePosition == Position.Elite;
        }

        public bool IsMiddleUp()
        {
            return m_ePosition == Position.Commoners || m_ePosition == Position.Elite;
        }

        /// <summary>
        /// Государство, которому принадлежит сословие
        /// </summary>
        private State m_pState;

        public bool IsSegregated()
        {
            return !DominantCulture.m_pCustoms.Has(Customs.GenderPriority.Genders_equality);
        }

        void UpdateCultureProgress(Culture pCulture)
        {
            if (m_ePosition == Position.Outlaws)
                pCulture.m_iProgressLevel--;
            if (m_ePosition == Position.Elite)
                pCulture.m_iProgressLevel++;

            if (pCulture.m_iProgressLevel < 0)
                pCulture.m_iProgressLevel = 0;
            if (pCulture.m_iProgressLevel > 8)
                pCulture.m_iProgressLevel = 8;
        }

        /// <summary>
        /// Создаёт новое сословие, основываясь на заданном. Новое сословие в целом разделяет все культурные ценности и обычаи заданного, но возможны отклонения.
        /// </summary>
        /// <param name="pEstate"></param>
        /// <param name="ePosition"></param>
        /// <param name="sName"></param>
        /// <param name="bMainEstate">если true, то берём культуру базового сообщества без изменений</param>
        public Estate(NationalSociety pSociety, Position ePosition, string sName, bool bMainEstate)
            : base(pSociety.m_pTitularNation)
        {
            m_ePosition = ePosition;

            m_cCulture[Gender.Male] = new Culture(pSociety.m_cCulture[Gender.Male], bMainEstate ? Customs.Mutation.None : Customs.Mutation.Mandatory);
            UpdateCultureProgress(m_cCulture[Gender.Male]);

            m_cCulture[Gender.Female] = new Culture(pSociety.m_cCulture[Gender.Female], bMainEstate ? Customs.Mutation.None : Customs.Mutation.Mandatory);
            UpdateCultureProgress(m_cCulture[Gender.Female]);

            FixSexCustoms();

            Person.GetSkillPreferences(DominantCulture, ref m_eMostRespectedSkill, ref m_eLeastRespectedSkill);

            m_sName = sName;
        }
    }
}
