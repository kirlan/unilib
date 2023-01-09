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

            m_iTechLevel = m_pTitularNation.m_pProtoSociety.m_iTechLevel;
            m_iMagicLimit = m_pTitularNation.m_pProtoSociety.m_iMagicLimit;

            m_cCulture[Gender.Male] = new Culture(m_pTitularNation.m_pProtoSociety.m_cCulture[Gender.Male], Customs.Mutation.Possible);
            m_cCulture[Gender.Male].m_pCustoms.ApplyFenotype(m_pTitularNation.m_pPhenotypeM);
            m_cCulture[Gender.Female] = new Culture(m_pTitularNation.m_pProtoSociety.m_cCulture[Gender.Female], Customs.Mutation.Possible);
            m_cCulture[Gender.Female].m_pCustoms.ApplyFenotype(m_pTitularNation.m_pPhenotypeF);

            FixSexCustoms();
        }

        public NationalSociety(Race pRace, Epoch pEpoch, Nation pNation)
        {
            m_pTitularNation = pNation;
            m_sName = m_pTitularNation.m_pRace.m_pLanguage.RandomCountryName();

            m_iTechLevel = pEpoch.m_iNativesMaxTechLevel;
            m_iMagicLimit = pEpoch.m_iNativesMaxMagicLevel;

            var pRaceMentality = new Mentality(pRace.m_pMentalityTemplate);
            var pRaceCustoms = new Customs();

            m_cCulture[Gender.Male] = new Culture(pRaceMentality, m_iTechLevel, pRaceCustoms);
            m_cCulture[Gender.Female] = new Culture(pRaceMentality, m_iTechLevel, new Customs(pRaceCustoms, Customs.Mutation.Possible));

            m_cCulture[Gender.Female].m_pCustoms.ApplyFenotype(m_pTitularNation.m_pPhenotypeF);

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
        internal void CheckResources(Dictionary<LandResource, float> cResources, int iPopulation, int iSize)
        {
            float fFood = m_pTitularNation.GetAvailableFood(cResources, iPopulation);

            if (cResources[LandResource.Wood] * 2 < Rnd.Get(iPopulation) && cResources[LandResource.Ore] * 2 < Rnd.Get(iPopulation))// && Rnd.OneChanceFrom(2))
                m_iTechLevel -= 2;
            else if (cResources[LandResource.Wood] + cResources[LandResource.Ore] < Rnd.Get(iPopulation))// && Rnd.OneChanceFrom(2))
                m_iTechLevel--;
            else if ((cResources[LandResource.Wood] > Rnd.Get(iPopulation) * 2 && cResources[LandResource.Ore] > Rnd.Get(iPopulation) * 2))// || Rnd.OneChanceFrom(4))
                m_iTechLevel++;

            if (m_pTitularNation.IsInvader)
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

            if (m_iTechLevel == 0 && m_pTitularNation.m_pProtoSociety.m_iMagicLimit == 0)
                m_iInfrastructureLevel = 0;

            //TODO: нужно учитывать размеры и телосложение - гиганты и толстяки едят больше, чем карлики и худышки
            if (fFood * 2 < iPopulation)
                m_iInfrastructureLevel--;            
            if (fFood < iPopulation || Rnd.OneChanceFrom(10))
                m_iInfrastructureLevel--;
            if (fFood > iPopulation * 2 && Rnd.OneChanceFrom(10))
                m_iInfrastructureLevel++;

            if (m_iInfrastructureLevel < 0)
                m_iInfrastructureLevel = 0;
            if (m_iInfrastructureLevel > m_iTechLevel + 1)
                m_iInfrastructureLevel = m_iTechLevel + 1;
            if (m_iInfrastructureLevel > 8)
                m_iInfrastructureLevel = 8;

            // Adjusting TL due to infrastructure level
            while (GetEffectiveTech() > m_iInfrastructureLevel * 2)
                m_iTechLevel--;

            if (m_iTechLevel < 0)
                m_iTechLevel = 0;

            if (m_pTitularNation.IsInvader)
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

        public void UpdateTitularNation(Nation pNewTitularNation)
        {
            m_pTitularNation = pNewTitularNation;

            m_cCulture[Gender.Male].m_pCustoms.ApplyFenotype(m_pTitularNation.m_pPhenotypeM);
            m_cCulture[Gender.Female].m_pCustoms.ApplyFenotype(m_pTitularNation.m_pPhenotypeF);
        }
    }
}
