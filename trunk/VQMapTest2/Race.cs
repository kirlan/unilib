using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LandscapeGeneration;
using Random;
using VQMapTest2.Languages;

namespace VQMapTest2
{
    public class Race 
    {
        public int m_iTechLevel = 0;
        public int m_iMagicLimit = 0;

        public MagicAbilityPrevalence m_eMagicAbilityPrevalence = MagicAbilityPrevalence.rare;
        public MagicAbilityDistribution m_eMagicAbilityDistribution = MagicAbilityDistribution.mostly_weak;

        public Culture m_pCulture = null;
        
        public GenderPriority m_eGenderPriority = GenderPriority.GendersEquality;

        public string m_sName;
        public string m_sNameF;
        public int m_iRank;
        public Language m_pLanguage;

        public LandTypeInfoX[] m_cPrefferedLands;
        public LandTypeInfoX[] m_cHatedLands;

        public Race(string sName, int iRank, Language pLanguage, LandTypeInfoX[] cPrefferedLands, LandTypeInfoX[] cHatedLands)
            :this(sName, sName, iRank, pLanguage, cPrefferedLands, cHatedLands)
        { }

        public Race(string sNameM, string sNameF, int iRank, Language pLanguage, LandTypeInfoX[] cPrefferedLands, LandTypeInfoX[] cHatedLands)
        {
            m_sName = sNameM;
            m_sNameF = sNameF;
            m_iRank = iRank;

            m_pLanguage = pLanguage;

            m_cPrefferedLands = cPrefferedLands;
            m_cHatedLands = cHatedLands;

            m_pCulture = new Culture();
        }

        public override string ToString()
        {
            return string.Format("{0}[{1}]", m_sName, m_iRank);
        }

        /// <summary>
        /// Согласовать параметры расы с параметрами мира.
        /// </summary>
        /// <param name="pWorld">мир</param>
        public void Accommodate(World pWorld)
        {
            int iOldLevel = Math.Max(m_iTechLevel, m_iMagicLimit);

            int iMagicLimit = (int)(Math.Pow(Rnd.Get(15), 3) / 1000);
            if (Rnd.OneChanceFrom(10))
                m_iMagicLimit = pWorld.m_iMagicLimit + iMagicLimit;
            else
                m_iMagicLimit = pWorld.m_iMagicLimit - iMagicLimit;

            if (m_iMagicLimit < 0)
                m_iMagicLimit = 0;
            if (m_iMagicLimit > 8)
                m_iMagicLimit = 8;

            int iOldTechLevel = m_iTechLevel;

            int iTechLevel = (int)(Math.Pow(Rnd.Get(15), 3) / 1000);
            if (Rnd.OneChanceFrom(10))
                m_iTechLevel = pWorld.m_iTechLevel + iTechLevel;
            else
                m_iTechLevel = pWorld.m_iTechLevel - iTechLevel;

            if (m_iTechLevel < 0)
                m_iTechLevel = 0;
            if (m_iTechLevel > 8)
                m_iTechLevel = 8;

            int iNewLevel = Math.Max(m_iTechLevel, m_iMagicLimit);
            if (iNewLevel > iOldLevel)
                for (int i = 0; i < iNewLevel - iOldLevel; i++)
                    m_pCulture.Evolve();
            else
                for (int i = 0; i < iOldLevel - iNewLevel; i++)
                    m_pCulture.Degrade();

            if (Rnd.OneChanceFrom(3))
                m_eGenderPriority = (GenderPriority)Rnd.Get(typeof(GenderPriority));
            else
                m_eGenderPriority = pWorld.m_eGenderPriority;

            if (Rnd.OneChanceFrom(3))
                m_eMagicAbilityPrevalence = (MagicAbilityPrevalence)Rnd.Get(typeof(MagicAbilityPrevalence));
            else
                m_eMagicAbilityPrevalence = pWorld.m_eMagicAbilityPrevalence;

            if (Rnd.OneChanceFrom(3))
                m_eMagicAbilityDistribution = (MagicAbilityDistribution)Rnd.Get(typeof(MagicAbilityDistribution));
            else
                m_eMagicAbilityDistribution = pWorld.m_eMagicAbilityDistribution;
        }
    }
}
