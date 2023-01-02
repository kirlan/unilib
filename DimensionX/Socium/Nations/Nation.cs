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
        public Phenotype<LandTypeInfoX> m_pPhenotypeM;
        public Phenotype<LandTypeInfoX> m_pPhenotypeF;

        public Society m_pProtoSociety = null;
        
        public Phenotype<LandTypeInfoX> DominantPhenotype
        {
            get
            {
                if (m_pProtoSociety.DominantCulture.m_pCustoms.Has(Customs.GenderPriority.Matriarchy))
                    return m_pPhenotypeF;
                else
                    return m_pPhenotypeM;
            }
        }

        /// <summary>
        /// Культура "слабого" пола
        /// </summary>
        public Phenotype<LandTypeInfoX> InferiorFenotype
        {
            get
            {
                if (m_pProtoSociety.DominantCulture.m_pCustoms.Has(Customs.GenderPriority.Matriarchy))
                    return m_pPhenotypeM;
                else
                    return m_pPhenotypeF;
            }
        }


        public Race m_pRace = null;

        public LandTypeInfoX[] m_aPreferredLands;
        public LandTypeInfoX[] m_aHatedLands;

        public bool IsAncient { get; private set; }
        public bool IsHegemon { get; private set; }
        public bool IsInvader { get; private set; }

        /// <summary>
        /// Пометить как "древнюю". Древние расы не могут быть гегемонами!
        /// </summary>
        public void Age()
        {
            IsAncient = true;
            IsHegemon = false;
        }

        /// <summary>
        /// Сделать гегемоном
        /// </summary>
        public void Grow()
        {
            IsHegemon = true;
        }

        public Epoch m_pEpoch = null;

        public Nation(Race pRace, Epoch pEpoch, bool bInvader) : this(pRace, pEpoch)
        {
            IsInvader = bInvader;
        }

        public Nation(Race pRace, Epoch pEpoch)
        {
            m_pRace = pRace;

            m_pEpoch = pEpoch;

            bool bNew = false;
            do
            {
                m_pPhenotypeM = (Phenotype<LandTypeInfoX>)m_pRace.m_pPhenotypeM.MutateNation();

                var pExpectedPhenotypeF = Phenotype.Combine(m_pPhenotypeM, m_pRace.m_pGenderDiffFemale);

                m_pPhenotypeF = (Phenotype<LandTypeInfoX>)pExpectedPhenotypeF.MutateGender();

                bNew = !m_pPhenotypeM.IsIdentical(m_pRace.m_pPhenotypeM) &&
                       !m_pPhenotypeF.IsIdentical(m_pRace.m_pPhenotypeF);

                foreach (Nation pOtherNation in m_pRace.m_cNations)
                {
                    if (m_pPhenotypeM.IsIdentical(pOtherNation.m_pPhenotypeM) &&
                        m_pPhenotypeF.IsIdentical(pOtherNation.m_pPhenotypeF))
                        bNew = false;
                }
            }
            while (!bNew);

            m_pProtoSociety = new NationalSociety(pRace, pEpoch, this);
            m_pProtoSociety.m_sName = m_pRace.m_pLanguage.RandomNationName();

            DominantPhenotype.GetTerritoryPreferences(out m_aPreferredLands, out m_aHatedLands);

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
            if (IsInvader)
            {
                //m_iTechLevel = Math.Min(pEpoch.m_iInvadersMaxTechLevel, pEpoch.m_iInvadersMinTechLevel + 1 + (int)(Math.Pow(Rnd.Get(20), 3) / 1000));
                //m_iMagicLimit = Math.Min(pEpoch.m_iInvadersMaxMagicLevel, pEpoch.m_iInvadersMinMagicLevel + (int)(Math.Pow(Rnd.Get(21), 3) / 1000));

                m_pProtoSociety.m_iTechLevel = pEpoch.m_iInvadersMinTechLevel + Rnd.Get(pEpoch.m_iInvadersMaxTechLevel - pEpoch.m_iInvadersMinTechLevel + 1);
                m_pProtoSociety.m_iMagicLimit = pEpoch.m_iInvadersMinMagicLevel + Rnd.Get(pEpoch.m_iInvadersMaxMagicLevel - pEpoch.m_iInvadersMinMagicLevel + 1);

                if (DominantPhenotype.m_pValues.Get<BrainGenetix>().Intelligence == Intelligence.Primitive)
                    m_pProtoSociety.m_iTechLevel = pEpoch.m_iInvadersMinTechLevel;

                int iMagicLimit = (int)(Math.Pow(Rnd.Get(15), 3) / 1000);
                if (Rnd.OneChanceFrom(10))
                    m_pProtoSociety.m_iMagicLimit += iMagicLimit;
                else
                    m_pProtoSociety.m_iMagicLimit -= iMagicLimit;

                int iOldTechLevel = m_pProtoSociety.m_iTechLevel;

                int iTechLevel = (int)(Math.Pow(Rnd.Get(15), 3) / 1000);
                if (DominantPhenotype.m_pValues.Get<BrainGenetix>().Intelligence != Intelligence.Primitive &&
                    (DominantPhenotype.m_pValues.Get<BrainGenetix>().Intelligence == Intelligence.Ingenious || Rnd.OneChanceFrom(10)))
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
                if (!IsAncient)
                {
                    //m_iTechLevel = Math.Min(pEpoch.m_iNativesMaxTechLevel, pEpoch.m_iNativesMinTechLevel + 1 + (int)(Math.Pow(Rnd.Get(20), 3) / 1000));
                    //m_iMagicLimit = Math.Min(pEpoch.m_iNativesMaxMagicLevel, pEpoch.m_iNativesMinMagicLevel + (int)(Math.Pow(Rnd.Get(21), 3) / 1000));
                    int iNewTechLevel = pEpoch.m_iNativesMinTechLevel + Rnd.Get(pEpoch.m_iNativesMaxTechLevel - pEpoch.m_iNativesMinTechLevel + 1);
                    int iNewMagicLimit = pEpoch.m_iNativesMinMagicLevel + Rnd.Get(pEpoch.m_iNativesMaxMagicLevel - pEpoch.m_iNativesMinMagicLevel + 1);

                    if (m_pProtoSociety.m_iMagicLimit <= pEpoch.m_iNativesMinMagicLevel)
                        m_pProtoSociety.m_iMagicLimit = pEpoch.m_iNativesMinMagicLevel;
                    else
                        m_pProtoSociety.m_iMagicLimit = (m_pProtoSociety.m_iMagicLimit + iNewMagicLimit + 1) / 2;

                    if (m_pProtoSociety.m_iTechLevel <= pEpoch.m_iNativesMinTechLevel || DominantPhenotype.m_pValues.Get<BrainGenetix>().Intelligence == Intelligence.Primitive)
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
                if (DominantPhenotype.m_pValues.Get<BrainGenetix>().Intelligence != Intelligence.Primitive && 
                    (DominantPhenotype.m_pValues.Get<BrainGenetix>().Intelligence == Intelligence.Ingenious || Rnd.OneChanceFrom(5)))
                    m_pProtoSociety.m_iTechLevel += iTechLevel;
                else
                    m_pProtoSociety.m_iTechLevel -= iTechLevel;

                if (!IsAncient)
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
            if (DominantPhenotype.m_pValues.Get<BrainGenetix>().MagicAbilityPotential > m_pProtoSociety.m_iMagicLimit + 1)
                m_pProtoSociety.DominantCulture.m_eMagicAbilityDistribution = MagicAbilityDistribution.mostly_powerful;
            if (DominantPhenotype.m_pValues.Get<BrainGenetix>().MagicAbilityPotential < m_pProtoSociety.m_iMagicLimit - 1)
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
        
        /// <summary>
        /// Вычисляет условную стоимость заселения территории указанной расой, в соответсвии с ландшафтом и фенотипом расы.
        /// Возвращает значение в диапазоне 1-100. 
        /// 1 - любая территория, идеально подходящая указанной расе (горы для гномов). Так же - простая для заселения территория, просто подходящая указанной расе.
        /// 10 - простая для заселения территория (равнины), но совсем не подходящая указанной расе (горы для эльфов). Так же - максимально сложная для заселения территория, просто подходящая указанной расе (горы для людей).
        /// 100 - максимально сложная для заселения территория (непроходимые горы), совсем не подходящая указанной расе.
        /// </summary>
        /// <param name="pNation"></param>
        /// <returns></returns>
        public int GetClaimingCost(LandTypeInfoX pLandType)
        {
            if (pLandType == null)
                return -1;

            if (!pLandType.m_eEnvironment.HasFlag(LandscapeGeneration.Environment.Habitable))
                return -1;

            float fCost = pLandType.m_iMovementCost; // 1 - 10

            if (m_aPreferredLands.Contains(pLandType))
                fCost /= 10;// (float)pLand.Type.m_iMovementCost;//2;

            if (m_aHatedLands.Contains(pLandType))
                fCost *= 10;// (float)pLand.Type.m_iMovementCost;//2;

            if (IsHegemon)
                fCost /= 2;

            if (fCost < 1)
                fCost = 1;

            if (fCost > int.MaxValue)
                fCost = int.MaxValue - 1;

            return (int)fCost;
        }

    }
}
