using GeneLab;
using GeneLab.Genetix;
using Random;
using Socium.Population;
using Socium.Psychology;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Socium.Nations
{
    public enum MagicAbilityDistribution
    {
        mostly_weak,
        mostly_average,
        mostly_powerful
    }

    public class Nation 
    {
        public Fenotype<LandTypeInfoX> m_pFenotypeM;
        public Fenotype<LandTypeInfoX> m_pFenotypeF;

        public Society m_pProtoSociety = null;
        
        public Fenotype<LandTypeInfoX> DominantFenotype
        {
            get
            {
                if (m_pProtoSociety.DominantCulture.m_pCustoms.Has(Customs.GenderPriority.Matriarchy))
                    return m_pFenotypeF;
                else
                    return m_pFenotypeM;
            }
        }

        /// <summary>
        /// Культура "слабого" пола
        /// </summary>
        public Fenotype<LandTypeInfoX> InferiorFenotype
        {
            get
            {
                if (m_pProtoSociety.DominantCulture.m_pCustoms.Has(Customs.GenderPriority.Matriarchy))
                    return m_pFenotypeM;
                else
                    return m_pFenotypeF;
            }
        }

        public Race m_pRace = null;

        public LandTypeInfoX[] m_aPreferredLands;
        public LandTypeInfoX[] m_aHatedLands;

        public bool m_bDying = false;
        public bool m_bHegemon = false;
        public bool m_bInvader = false;

        public Epoch m_pEpoch = null;

        public Nation(Race pRace, Epoch pEpoch)
        {
            m_pRace = pRace;

            m_pEpoch = pEpoch;

            bool bNew = false;
            do
            {
                m_pFenotypeM = (Fenotype<LandTypeInfoX>)m_pRace.m_pFenotypeM.MutateNation();

                bNew = !m_pFenotypeM.IsIdentical(m_pRace.m_pFenotypeM);

                foreach (Nation pOtherNation in m_pRace.m_cNations)
                    if (m_pFenotypeM.IsIdentical(pOtherNation.m_pFenotypeM))
                        bNew = false;
            }
            while (!bNew);

            bNew = false;
            do
            {
                m_pFenotypeF = (Fenotype<LandTypeInfoX>)m_pRace.m_pFenotypeF.MutateNation();

                bNew = !m_pFenotypeF.IsIdentical(m_pRace.m_pFenotypeF);

                foreach (Nation pOtherNation in m_pRace.m_cNations)
                    if (m_pFenotypeF.IsIdentical(pOtherNation.m_pFenotypeF))
                        bNew = false;
            }
            while (!bNew);

            m_pProtoSociety = new NationalSociety(pRace, pEpoch, this);
            m_pProtoSociety.m_sName = m_pRace.m_pLanguage.RandomNationName();

            DominantFenotype.GetTerritoryPreferences(out m_aPreferredLands, out m_aHatedLands);

            m_pRace.m_cNations.Add(this);
        }

        public override string ToString()
        {
            //if (m_bDying)
            //    return string.Format("ancient {1} ({0})", m_sName, m_pRace).ToLower();
            //else
            //    if(m_bHegemon)
            //        return string.Format("great {1} ({0})", m_sName, m_pRace).ToLower();
            //    else

            if (m_pProtoSociety.m_sName == m_pRace.ToString())
                return m_pProtoSociety.m_sName;

            return string.Format("{1} ({0})", m_pProtoSociety.m_sName, m_pRace).ToLower();
        }

        /// <summary>
        /// Согласовать параметры расы с параметрами мира.
        /// Параметры мира могут немного отличаться от параметров эпохи - это нормально
        /// </summary>
        /// <param name="pWorld">мир</param>
        public void Accommodate(Epoch pEpoch)
        {
            if (m_bInvader)
            {
                //m_iTechLevel = Math.Min(pEpoch.m_iInvadersMaxTechLevel, pEpoch.m_iInvadersMinTechLevel + 1 + (int)(Math.Pow(Rnd.Get(20), 3) / 1000));
                //m_iMagicLimit = Math.Min(pEpoch.m_iInvadersMaxMagicLevel, pEpoch.m_iInvadersMinMagicLevel + (int)(Math.Pow(Rnd.Get(21), 3) / 1000));

                m_pProtoSociety.m_iTechLevel = pEpoch.m_iInvadersMinTechLevel + Rnd.Get(pEpoch.m_iInvadersMaxTechLevel - pEpoch.m_iInvadersMinTechLevel + 1);
                m_pProtoSociety.m_iMagicLimit = pEpoch.m_iInvadersMinMagicLevel + Rnd.Get(pEpoch.m_iInvadersMaxMagicLevel - pEpoch.m_iInvadersMinMagicLevel + 1);

                if (DominantFenotype.m_pBrain.m_eIntelligence == Intelligence.Primitive)
                    m_pProtoSociety.m_iTechLevel = pEpoch.m_iInvadersMinTechLevel;

                int iMagicLimit = (int)(Math.Pow(Rnd.Get(15), 3) / 1000);
                if (Rnd.OneChanceFrom(10))
                    m_pProtoSociety.m_iMagicLimit += iMagicLimit;
                else
                    m_pProtoSociety.m_iMagicLimit -= iMagicLimit;

                int iOldTechLevel = m_pProtoSociety.m_iTechLevel;

                int iTechLevel = (int)(Math.Pow(Rnd.Get(15), 3) / 1000);
                if (DominantFenotype.m_pBrain.m_eIntelligence != Intelligence.Primitive &&
                    (DominantFenotype.m_pBrain.m_eIntelligence == Intelligence.Ingenious || Rnd.OneChanceFrom(10)))
                    m_pProtoSociety.m_iTechLevel += iTechLevel;
                else
                    m_pProtoSociety.m_iTechLevel -= iTechLevel;

                if (m_pProtoSociety.m_iMagicLimit < pEpoch.m_iInvadersMinMagicLevel)
                    m_pProtoSociety.m_iMagicLimit = pEpoch.m_iInvadersMinMagicLevel;
                if (m_pProtoSociety.m_iMagicLimit > pEpoch.m_iInvadersMaxMagicLevel)
                    m_pProtoSociety.m_iMagicLimit = pEpoch.m_iInvadersMaxMagicLevel;

                if (m_pProtoSociety.m_iTechLevel < pEpoch.m_iInvadersMinTechLevel)
                    m_pProtoSociety.m_iTechLevel = pEpoch.m_iInvadersMinTechLevel;
                if (m_pProtoSociety.m_iTechLevel > pEpoch.m_iInvadersMaxTechLevel)
                    m_pProtoSociety.m_iTechLevel = pEpoch.m_iInvadersMaxTechLevel;
            }
            else
            {
                if (!m_bDying)
                {
                    //m_iTechLevel = Math.Min(pEpoch.m_iNativesMaxTechLevel, pEpoch.m_iNativesMinTechLevel + 1 + (int)(Math.Pow(Rnd.Get(20), 3) / 1000));
                    //m_iMagicLimit = Math.Min(pEpoch.m_iNativesMaxMagicLevel, pEpoch.m_iNativesMinMagicLevel + (int)(Math.Pow(Rnd.Get(21), 3) / 1000));
                    int iNewTechLevel = pEpoch.m_iNativesMinTechLevel + Rnd.Get(pEpoch.m_iNativesMaxTechLevel - pEpoch.m_iNativesMinTechLevel + 1);
                    int iNewMagicLimit = pEpoch.m_iNativesMinMagicLevel + Rnd.Get(pEpoch.m_iNativesMaxMagicLevel - pEpoch.m_iNativesMinMagicLevel + 1);

                    if (m_pProtoSociety.m_iMagicLimit <= pEpoch.m_iNativesMinMagicLevel)
                        m_pProtoSociety.m_iMagicLimit = pEpoch.m_iNativesMinMagicLevel;
                    else
                        m_pProtoSociety.m_iMagicLimit = (m_pProtoSociety.m_iMagicLimit + iNewMagicLimit + 1) / 2;

                    if (m_pProtoSociety.m_iTechLevel <= pEpoch.m_iNativesMinTechLevel || DominantFenotype.m_pBrain.m_eIntelligence == Intelligence.Primitive)
                        m_pProtoSociety.m_iTechLevel = pEpoch.m_iNativesMinTechLevel;
                    else
                        m_pProtoSociety.m_iTechLevel = (m_pProtoSociety.m_iTechLevel + iNewTechLevel + 1) / 2;
                }

                int iMagicLimit = (int)(Math.Pow(Rnd.Get(13), 3) / 1000);
                if (Rnd.OneChanceFrom(10))
                    m_pProtoSociety.m_iMagicLimit += iMagicLimit;
                else
                    m_pProtoSociety.m_iMagicLimit -= iMagicLimit;

                int iOldTechLevel = m_pProtoSociety.m_iTechLevel;

                int iTechLevel = (int)(Math.Pow(Rnd.Get(13), 3) / 1000);
                if (DominantFenotype.m_pBrain.m_eIntelligence != Intelligence.Primitive && 
                    (DominantFenotype.m_pBrain.m_eIntelligence == Intelligence.Ingenious || Rnd.OneChanceFrom(5)))
                    m_pProtoSociety.m_iTechLevel += iTechLevel;
                else
                    m_pProtoSociety.m_iTechLevel -= iTechLevel;

                if (!m_bDying)
                {
                    if (m_pProtoSociety.m_iMagicLimit < pEpoch.m_iNativesMinMagicLevel)
                        m_pProtoSociety.m_iMagicLimit = pEpoch.m_iNativesMinMagicLevel;
                    if (m_pProtoSociety.m_iMagicLimit > pEpoch.m_iNativesMaxMagicLevel)
                        m_pProtoSociety.m_iMagicLimit = pEpoch.m_iNativesMaxMagicLevel;

                    if (m_pProtoSociety.m_iTechLevel < pEpoch.m_iNativesMinTechLevel)
                        m_pProtoSociety.m_iTechLevel = pEpoch.m_iNativesMinTechLevel;
                    if (m_pProtoSociety.m_iTechLevel > pEpoch.m_iNativesMaxTechLevel)
                        m_pProtoSociety.m_iTechLevel = pEpoch.m_iNativesMaxTechLevel;
                }
                else
                {
                    if (m_pProtoSociety.m_iMagicLimit < 0)
                        m_pProtoSociety.m_iMagicLimit = 0;
                    if (m_pProtoSociety.m_iMagicLimit > 8)
                        m_pProtoSociety.m_iMagicLimit = 8;

                    if (m_pProtoSociety.m_iTechLevel < 0)
                        m_pProtoSociety.m_iTechLevel = 0;
                    if (m_pProtoSociety.m_iTechLevel > 8)
                        m_pProtoSociety.m_iTechLevel = 8;
                }

                //m_pCustoms = new Customs();
            }

            m_pProtoSociety.m_cCulture[Gender.Male].m_eMagicAbilityDistribution = MagicAbilityDistribution.mostly_average;
            m_pProtoSociety.m_cCulture[Gender.Female].m_eMagicAbilityDistribution = MagicAbilityDistribution.mostly_average;
            if (DominantFenotype.m_pBrain.m_iMagicAbilityPotential > m_pProtoSociety.m_iMagicLimit + 1)
                m_pProtoSociety.DominantCulture.m_eMagicAbilityDistribution = MagicAbilityDistribution.mostly_powerful;
            if (DominantFenotype.m_pBrain.m_iMagicAbilityPotential < m_pProtoSociety.m_iMagicLimit - 1)
                m_pProtoSociety.InferiorCulture.m_eMagicAbilityDistribution = MagicAbilityDistribution.mostly_weak;

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
