using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeneLab.Genetix;
using Random;
using Socium.Nations;
using Socium.Psychology;
using Socium.Settlements;

namespace Socium.Population
{
    /// <summary>
    /// Сообщество, объединённое по национальному признаку.
    /// Кроме титульной нации, хранит информацию о культуре, обычаях, доступном уровне жизни, технологическом уровне, 
    /// владении магическими способностями, название сообщества и список поселений, где сообщество представлено
    /// </summary>
    public class NationalSociety : Society
    {
        public Nation m_pTitularNation = null;

        /// <summary>
        /// Доступный жителям уровень жизни.
        /// Зависит от технического и магического развития, определяет доступные формы государственного правления
        /// </summary>
        public int m_iInfrastructureLevel = 0;

        public NationalSociety(Nation pNation)
        {
            m_pTitularNation = pNation;
            m_sName = m_pTitularNation.m_pRace.m_pLanguage.RandomCountryName();

            m_iTechLevel = m_pTitularNation.m_pSociety.m_iTechLevel;

            m_cCulture[Gender.Male] = new Culture(m_pTitularNation.m_pSociety.m_cCulture[Gender.Male], Customs.Mutation.Possible);
            m_cCulture[Gender.Female] = new Culture(m_pTitularNation.m_pSociety.m_cCulture[Gender.Female], Customs.Mutation.Possible);

            FixSexCustoms();
        }

        public override void AddBuildings(Settlement pSettlement)
        {
            throw new NotImplementedException();
        }

        protected override BuildingInfo ChooseNewBuilding(Settlement pSettlement)
        {
            throw new NotImplementedException();
        }
        internal void CheckResources(float fWood, float fOre, float fFood, int iPopulation, int iSize)
        {
            if (fWood * 2 < Rnd.Get(iPopulation) && fOre * 2 < Rnd.Get(iPopulation))// && Rnd.OneChanceFrom(2))
                m_iTechLevel -= 2;
            else if (fWood + fOre < Rnd.Get(iPopulation))// && Rnd.OneChanceFrom(2))
                m_iTechLevel--;
            else if ((fWood > Rnd.Get(iPopulation) * 2 && fOre > Rnd.Get(iPopulation) * 2))// || Rnd.OneChanceFrom(4))
                m_iTechLevel++;

            if (m_pTitularNation.m_bInvader)
            {
                if (m_iTechLevel < m_pTitularNation.m_pEpoch.m_iInvadersMinTechLevel)
                    m_iTechLevel = m_pTitularNation.m_pEpoch.m_iInvadersMinTechLevel;
                if (m_iTechLevel > m_pTitularNation.m_pEpoch.m_iInvadersMaxTechLevel)
                    m_iTechLevel = m_pTitularNation.m_pEpoch.m_iInvadersMaxTechLevel;
            }
            else
            {
                if (m_iTechLevel < m_pTitularNation.m_pEpoch.m_iNativesMinTechLevel)
                    m_iTechLevel = m_pTitularNation.m_pEpoch.m_iNativesMinTechLevel;
                if (m_iTechLevel > m_pTitularNation.m_pEpoch.m_iNativesMaxTechLevel)
                    m_iTechLevel = m_pTitularNation.m_pEpoch.m_iNativesMaxTechLevel;
            }

            //m_iInfrastructureLevel = 4 - (int)(m_pCulture.GetDifference(Culture.IdealSociety, m_iTechLevel, m_iTechLevel) * 4);
            m_iInfrastructureLevel = m_iTechLevel;// -(int)(m_iTechLevel * Math.Pow(Rnd.Get(1f), 3));

            if (iSize == 1 && m_iInfrastructureLevel > 4)
                m_iInfrastructureLevel /= 2;

            if (m_iTechLevel == 0 && m_pTitularNation.m_pSociety.m_iMagicLimit == 0)
                m_iInfrastructureLevel = 0;

            if (fFood * 2 < iPopulation)
                m_iInfrastructureLevel--;// = Rnd.Get(m_iCultureLevel);            
            if (fFood < iPopulation || Rnd.OneChanceFrom(10))
                m_iInfrastructureLevel--;// = Rnd.Get(m_iCultureLevel);
            if (fFood > iPopulation * 2 && Rnd.OneChanceFrom(10))
                m_iInfrastructureLevel++;

            if (m_iInfrastructureLevel < 0)//Math.Max(m_iTechLevel + 1, iAverageMagicLimit) / 2)
                m_iInfrastructureLevel = 0;//Math.Max(m_iTechLevel + 1, iAverageMagicLimit) / 2;
            if (m_iInfrastructureLevel > m_iTechLevel + 1)//Math.Max(m_iTechLevel + 1, iAverageMagicLimit - 1))
                m_iInfrastructureLevel = m_iTechLevel + 1;// Math.Max(m_iTechLevel + 1, iAverageMagicLimit - 1);
            if (m_iInfrastructureLevel > 8)
                m_iInfrastructureLevel = 8;

            // Adjusting TL due to infrastructure level
            while (GetEffectiveTech() > m_iInfrastructureLevel * 2)
                m_iTechLevel--;

            if (m_iTechLevel < 0)
                m_iTechLevel = 0;

            if (m_pTitularNation.m_bInvader)
            {
                if (m_iTechLevel > m_pTitularNation.m_pEpoch.m_iInvadersMaxTechLevel)
                    m_iTechLevel = m_pTitularNation.m_pEpoch.m_iInvadersMaxTechLevel;
            }
            else
            {
                if (m_iTechLevel > m_pTitularNation.m_pEpoch.m_iNativesMaxTechLevel)
                    m_iTechLevel = m_pTitularNation.m_pEpoch.m_iNativesMaxTechLevel;
            }
        }

        internal float GetAvailableFood(float fWood, float fOre, float fGrain, float fGame, float fFish, int iPopulation)
        { 
            switch (m_pTitularNation.m_pFenotype.m_pBody.m_eNutritionType)
            {
                case NutritionType.Eternal:
                    return iPopulation* 10;
                case NutritionType.Mineral:
                    return fOre;
                case NutritionType.Organic:
                    return fGrain + fGame + fFish;
                case NutritionType.ParasitismBlood:
                    return iPopulation;
                case NutritionType.ParasitismEmote:
                    return iPopulation;
                case NutritionType.ParasitismEnergy:
                    return iPopulation;
                case NutritionType.ParasitismMeat:
                    return iPopulation;
                case NutritionType.Photosynthesis:
                    return iPopulation* 10;
                case NutritionType.Thermosynthesis:
                    return iPopulation* 10;
                case NutritionType.Vegetarian:
                    return fGrain;
                case NutritionType.Carnivorous:
                    return fGame + fFish;
                default:
                    throw new Exception(string.Format("Unknown Nutrition type: {0}", m_pTitularNation.m_pFenotype.m_pBody.m_eNutritionType));
            }
}
    }
}
