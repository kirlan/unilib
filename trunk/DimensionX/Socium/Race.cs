﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LandscapeGeneration;
using Random;
using Socium.Languages;

namespace Socium
{
    public class Race 
    {
        public int m_iTechLevel = 0;
        public int m_iMagicLimit = 0;

        public MagicAbilityPrevalence m_eMagicAbilityPrevalence = MagicAbilityPrevalence.rare;
        public MagicAbilityDistribution m_eMagicAbilityDistribution = MagicAbilityDistribution.mostly_weak;

        public Culture m_pCulture = null;
        public Customs m_pCustoms = null;
        
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

            if (m_iMagicLimit <= pWorld.m_iMagicLimit)
                m_iMagicLimit = pWorld.m_iMagicLimit;
            else
                m_iMagicLimit = (m_iMagicLimit + pWorld.m_iMagicLimit + 1)/2;

            if (m_iTechLevel <= pWorld.m_iTechLevel)
                m_iTechLevel = pWorld.m_iTechLevel;
            else
                m_iTechLevel = (m_iTechLevel + pWorld.m_iTechLevel + 1)/2;

            int iMagicLimit = (int)(Math.Pow(Rnd.Get(15), 3) / 1000);
            if (Rnd.OneChanceFrom(10))
                m_iMagicLimit += iMagicLimit;
            else
                m_iMagicLimit -= iMagicLimit;

            if (m_iMagicLimit < pWorld.m_iMinMagicLevel)
                m_iMagicLimit = pWorld.m_iMinMagicLevel;
            if (m_iMagicLimit > pWorld.m_iMaxMagicLevel)
                m_iMagicLimit = pWorld.m_iMaxMagicLevel;

            int iOldTechLevel = m_iTechLevel;

            int iTechLevel = (int)(Math.Pow(Rnd.Get(15), 3) / 1000);
            if (Rnd.OneChanceFrom(10))
                m_iTechLevel += iTechLevel;
            else
                m_iTechLevel -= iTechLevel;

            if (m_iTechLevel < pWorld.m_iMinTechLevel)
                m_iTechLevel = pWorld.m_iMinTechLevel;
            if (m_iTechLevel > pWorld.m_iMaxTechLevel)
                m_iTechLevel = pWorld.m_iMaxTechLevel;

            m_pCustoms = new Customs();

            if (Rnd.OneChanceFrom(3))
                m_eMagicAbilityPrevalence = (MagicAbilityPrevalence)Rnd.GetExp(typeof(MagicAbilityPrevalence), 4);
            else
                if (iOldLevel == 0)
                    m_eMagicAbilityPrevalence = pWorld.m_eMagicAbilityPrevalence;

            if (Rnd.OneChanceFrom(3))
                m_eMagicAbilityDistribution = (MagicAbilityDistribution)Rnd.GetExp(typeof(MagicAbilityDistribution), 4);
            else
                if (iOldLevel == 0)
                    m_eMagicAbilityDistribution = pWorld.m_eMagicAbilityDistribution;

            if (iOldLevel == 0 && Rnd.Chances(pWorld.m_iInvasionProbability, 100))
            {
                //int iBalance = Rnd.Get(201);

                //if (iBalance > 100)
                //{
                //    m_iMagicLimit = pWorld.m_iMagicLimit + (9 - pWorld.m_iMagicLimit) / 2 + (int)(Math.Pow(Rnd.Get(10), 3) * (4 - pWorld.m_iMagicLimit / 2) / 1000);
                //    m_iTechLevel = (200 - iBalance) * m_iMagicLimit / iBalance;
                //}
                //else
                //{
                //    m_iTechLevel = pWorld.m_iTechLevel + (9 - pWorld.m_iTechLevel) / 2 + (int)(Math.Pow(Rnd.Get(10), 3) * (4 - pWorld.m_iTechLevel / 2) / 1000);
                //    m_iMagicLimit = iBalance * m_iTechLevel / (200 - iBalance);
                //} 
                m_iTechLevel = Math.Min(pWorld.m_iInvadersMaxTechLevel, pWorld.m_iTechLevel + (9 - pWorld.m_iTechLevel) / 2 + (int)(Math.Pow(Rnd.Get(10), 3) * (4 - pWorld.m_iTechLevel / 2) / 1000));
                m_iMagicLimit = Math.Min(pWorld.m_iInvadersMaxMagicLevel, pWorld.m_iMagicLimit + (9 - pWorld.m_iMagicLimit) / 2 + (int)(Math.Pow(Rnd.Get(10), 3) * (4 - pWorld.m_iMagicLimit / 2) / 1000));
                
                m_eMagicAbilityPrevalence = (MagicAbilityPrevalence)Rnd.Get(typeof(MagicAbilityPrevalence));
                m_eMagicAbilityDistribution = (MagicAbilityDistribution)Rnd.Get(typeof(MagicAbilityDistribution));
            }

            int iNewLevel = Math.Max(m_iTechLevel, m_iMagicLimit);
            if (iNewLevel > iOldLevel)
                for (int i = 0; i < iNewLevel - iOldLevel; i++)
                {
                    m_pCulture.Evolve();
                    m_pCustoms.Evolve();
                }
            else
                for (int i = 0; i < iOldLevel - iNewLevel; i++)
                {
                    m_pCulture.Degrade();
                    m_pCustoms.Degrade();
                }
        }
    }
}
