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
        public enum SocialRank
        {
            Outlaws = -1,
            Lowlifes = 0,
            Commoners = 1,
            Clergy = 2,
            Elite = 3
        }

        public SocialRank Rank { get; }

        public bool IsOutlaw()
        {
            return Rank == SocialRank.Outlaws;
        }

        public bool IsElite()
        {
            return Rank == SocialRank.Elite;
        }

        public bool IsCleregy()
        {
            return Rank == SocialRank.Clergy;
        }

        public bool IsMiddleUp()
        {
            return Rank == SocialRank.Commoners || Rank == SocialRank.Elite;
        }

        public bool IsSegregated()
        {
            return !DominantCulture.Customs.Has(Customs.GenderPriority.Genders_equality);
        }

        private void UpdateCultureProgress(Culture pCulture)
        {
            if (Rank == SocialRank.Outlaws)
                pCulture.ProgressLevel--;
            if (Rank == SocialRank.Elite)
            {
                if (Rnd.OneChanceFrom(2))
                    pCulture.ProgressLevel++; //progressive
                else
                    pCulture.ProgressLevel--; //decadent
            }
            if (Rank == SocialRank.Clergy)
                pCulture.ProgressLevel++;

            if (pCulture.ProgressLevel < 0)
                pCulture.ProgressLevel = 0;
            if (pCulture.ProgressLevel > 8)
                pCulture.ProgressLevel = 8;
        }

        /// <summary>
        /// Создаёт новое сословие, основываясь на заданном. Новое сословие в целом разделяет все культурные ценности и обычаи заданного, но возможны отклонения.
        /// </summary>
        /// <param name="pEstate"></param>
        /// <param name="ePosition"></param>
        /// <param name="sName"></param>
        /// <param name="bMainEstate">если true, то берём культуру базового сообщества без изменений</param>
        public Estate(NationalSociety pSociety, SocialRank ePosition, string sName, bool bMainEstate)
            : base(pSociety.TitularNation)
        {
            Rank = ePosition;

            Culture[Gender.Male] = new Culture(pSociety.Culture[Gender.Male], bMainEstate ? Customs.Mutation.None : Customs.Mutation.Mandatory);
            Culture[Gender.Male].Customs.ApplyFenotype(TitularNation.PhenotypeMale);
            UpdateCultureProgress(Culture[Gender.Male]);

            Culture[Gender.Female] = new Culture(pSociety.Culture[Gender.Female], bMainEstate ? Customs.Mutation.None : Customs.Mutation.Mandatory);
            Culture[Gender.Male].Customs.ApplyFenotype(TitularNation.PhenotypeFemale);
            UpdateCultureProgress(Culture[Gender.Female]);

            FixSexCustoms();

            Person.GetSkillPreferences(DominantCulture, ref m_eMostRespectedSkill, ref m_eLeastRespectedSkill);

            Name = sName;
        }
    }
}
