using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RB.Genetix;
using RB.Socium.Psichology;
using Random;
using RB.Genetix.GenetixParts;
using RB.Geography;

namespace RB.Socium
{
    public enum MagicAbilityDistribution
    {
        mostly_weak,
        mostly_average,
        mostly_powerful
    }

    public class CNation 
    {
        public int m_iTechLevel = 0;
        public int m_iMagicLimit = 0;

        public Fenotype m_pFenotype;

        public MagicAbilityDistribution m_eMagicAbilityDistribution = MagicAbilityDistribution.mostly_weak;

        public Culture m_pCulture = null;
        public Customs m_pCustoms = null;

        public CRace m_pRace = null;

        public ITerritory[] m_aPreferredLands;
        public ITerritory[] m_aHatedLands;

        public bool m_bDying = false;
        public bool m_bHegemon = false;
        public bool m_bInvader = false;

        public CEpoch m_pEpoch = null;

        public string m_sName = "";

        public CNation(CRace pRace, CEpoch pEpoch)
        {
            m_pRace = pRace;

            m_sName = m_pRace.m_pLanguage.RandomNationName();

            m_pEpoch = pEpoch;

            bool bNew = false;
            do
            {
                m_pFenotype = (Fenotype)m_pRace.m_pFenotype.MutateNation();

                bNew = !m_pFenotype.IsIdentical(m_pRace.m_pFenotype);

                foreach (CNation pOtherNation in m_pRace.m_cNations)
                    if (m_pFenotype.IsIdentical(pOtherNation.m_pFenotype))
                        bNew = false;
            }
            while(!bNew);

            m_pFenotype.GetTerritoryPreferences(Territory.AllTerritories, out m_aPreferredLands, out m_aHatedLands);

            m_pCulture = new Culture(pRace.m_pCulture);
            m_pCustoms = new Customs();

            m_iTechLevel = pEpoch.m_iNativesMaxTechLevel;
            m_iMagicLimit = pEpoch.m_iNativesMaxMagicLevel;

            m_pRace.m_cNations.Add(this);
        }

        public override string ToString()
        {
            if (m_bDying)
                return string.Format("ancient {1} ({0})", m_sName, m_pRace).ToLower();
            else
                if (m_bHegemon)
                    return string.Format("great {1} ({0})", m_sName, m_pRace).ToLower();
                else
                    return string.Format("{1} ({0})", m_sName, m_pRace).ToLower();
        }

        /// <summary>
        /// Согласовать параметры расы с параметрами мира.
        /// </summary>
        /// <param name="pWorld">мир</param>
        public void Accommodate(CEpoch pEpoch)
        {
            if (m_bInvader)
            {
                //m_iTechLevel = Math.Min(pEpoch.m_iInvadersMaxTechLevel, pEpoch.m_iInvadersMinTechLevel + 1 + (int)(Math.Pow(Rnd.Get(20), 3) / 1000));
                //m_iMagicLimit = Math.Min(pEpoch.m_iInvadersMaxMagicLevel, pEpoch.m_iInvadersMinMagicLevel + (int)(Math.Pow(Rnd.Get(21), 3) / 1000));

                m_iTechLevel = pEpoch.m_iInvadersMinTechLevel + Rnd.Get(pEpoch.m_iInvadersMaxTechLevel - pEpoch.m_iInvadersMinTechLevel + 1);
                m_iMagicLimit = pEpoch.m_iInvadersMinMagicLevel + Rnd.Get(pEpoch.m_iInvadersMaxMagicLevel - pEpoch.m_iInvadersMinMagicLevel + 1);

                if (m_pFenotype.m_pBrain.m_eIntelligence == Intelligence.Primitive)
                    m_iTechLevel = pEpoch.m_iInvadersMinTechLevel;
            
                int iMagicLimit = (int)(Math.Pow(Rnd.Get(15), 3) / 1000);
                if (Rnd.OneChanceFrom(10))
                    m_iMagicLimit += iMagicLimit;
                else
                    m_iMagicLimit -= iMagicLimit;

                int iOldTechLevel = m_iTechLevel;

                int iTechLevel = (int)(Math.Pow(Rnd.Get(15), 3) / 1000);
                if (m_pFenotype.m_pBrain.m_eIntelligence != Intelligence.Primitive &&
                    (m_pFenotype.m_pBrain.m_eIntelligence == Intelligence.Ingenious || Rnd.OneChanceFrom(10)))
                    m_iTechLevel += iTechLevel;
                else
                    m_iTechLevel -= iTechLevel;

                if (m_iMagicLimit < pEpoch.m_iInvadersMinMagicLevel)
                    m_iMagicLimit = pEpoch.m_iInvadersMinMagicLevel;
                if (m_iMagicLimit > pEpoch.m_iInvadersMaxMagicLevel)
                    m_iMagicLimit = pEpoch.m_iInvadersMaxMagicLevel;

                if (m_iTechLevel < pEpoch.m_iInvadersMinTechLevel)
                    m_iTechLevel = pEpoch.m_iInvadersMinTechLevel;
                if (m_iTechLevel > pEpoch.m_iInvadersMaxTechLevel)
                    m_iTechLevel = pEpoch.m_iInvadersMaxTechLevel;
            }
            else
            {
                if (!m_bDying)
                {
                    int iNewTechLevel = pEpoch.m_iNativesMinTechLevel + Rnd.Get(pEpoch.m_iNativesMaxTechLevel - pEpoch.m_iNativesMinTechLevel + 1);
                    int iNewMagicLimit = pEpoch.m_iNativesMinMagicLevel + Rnd.Get(pEpoch.m_iNativesMaxMagicLevel - pEpoch.m_iNativesMinMagicLevel + 1);

                    if (m_iMagicLimit <= pEpoch.m_iNativesMinMagicLevel)
                        m_iMagicLimit = pEpoch.m_iNativesMinMagicLevel;
                    else
                        m_iMagicLimit = (m_iMagicLimit + iNewMagicLimit + 1) / 2;

                    if (m_iTechLevel <= pEpoch.m_iNativesMinTechLevel || m_pFenotype.m_pBrain.m_eIntelligence == Intelligence.Primitive)
                        m_iTechLevel = pEpoch.m_iNativesMinTechLevel;
                    else
                        m_iTechLevel = (m_iTechLevel + iNewTechLevel + 1) / 2;
                }

                int iMagicLimit = (int)(Math.Pow(Rnd.Get(13), 3) / 1000);
                if (Rnd.OneChanceFrom(10))
                    m_iMagicLimit += iMagicLimit;
                else
                    m_iMagicLimit -= iMagicLimit;

                int iOldTechLevel = m_iTechLevel;

                int iTechLevel = (int)(Math.Pow(Rnd.Get(13), 3) / 1000);
                if (m_pFenotype.m_pBrain.m_eIntelligence != Intelligence.Primitive && 
                    (m_pFenotype.m_pBrain.m_eIntelligence == Intelligence.Ingenious || Rnd.OneChanceFrom(5)))
                    m_iTechLevel += iTechLevel;
                else
                    m_iTechLevel -= iTechLevel;

                if (!m_bDying)
                {
                    if (m_iMagicLimit < pEpoch.m_iNativesMinMagicLevel)
                        m_iMagicLimit = pEpoch.m_iNativesMinMagicLevel;
                    if (m_iMagicLimit > pEpoch.m_iNativesMaxMagicLevel)
                        m_iMagicLimit = pEpoch.m_iNativesMaxMagicLevel;

                    if (m_iTechLevel < pEpoch.m_iNativesMinTechLevel)
                        m_iTechLevel = pEpoch.m_iNativesMinTechLevel;
                    if (m_iTechLevel > pEpoch.m_iNativesMaxTechLevel)
                        m_iTechLevel = pEpoch.m_iNativesMaxTechLevel;
                }
                else
                {
                    if (m_iMagicLimit < 0)
                        m_iMagicLimit = 0;
                    if (m_iMagicLimit > 8)
                        m_iMagicLimit = 8;

                    if (m_iTechLevel < 0)
                        m_iTechLevel = 0;
                    if (m_iTechLevel > 8)
                        m_iTechLevel = 8;
                }

                //m_pCustoms = new Customs();
            }

            m_eMagicAbilityDistribution = MagicAbilityDistribution.mostly_average;
            if (m_pFenotype.m_pBrain.m_iMagicAbilityPotential > m_iMagicLimit + 1)
                m_eMagicAbilityDistribution = MagicAbilityDistribution.mostly_powerful;
            if (m_pFenotype.m_pBrain.m_iMagicAbilityPotential < m_iMagicLimit - 1)
                m_eMagicAbilityDistribution = MagicAbilityDistribution.mostly_weak;

            //int iNewLevel = Math.Max(m_iTechLevel, m_iMagicLimit);
            //if (iNewLevel > iOldLevel)
            //    for (int i = 0; i < iNewLevel * iNewLevel - iOldLevel * iOldLevel; i++)
            //    {
            //        m_pCulture.Evolve();
            //        m_pCustoms.Evolve();
            //    }
            //else
            //    for (int i = 0; i < iOldLevel - iNewLevel; i++)
            //    {
            //        m_pCulture.Degrade();
            //        m_pCustoms.Degrade();
            //    }
        }
    }
}
