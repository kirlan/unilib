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
        public Nation TitularNation { get; private set; } = null;

        /// <summary>
        /// Доступный жителям уровень жизни.
        /// Зависит от технического и магического развития, определяет доступные формы государственного правления
        /// </summary>
        public int InfrastructureLevel { get; protected set; } = 0;

        public NationalSociety(Nation pNation)
        {
            TitularNation = pNation;
            Name = TitularNation.Race.Language.RandomCountryName();

            TechLevel = TitularNation.ProtoSociety.TechLevel;
            MagicLimit = TitularNation.ProtoSociety.MagicLimit;

            Culture[Gender.Male] = new Culture(TitularNation.ProtoSociety.Culture[Gender.Male], Customs.Mutation.Possible);
            Culture[Gender.Male].Customs.ApplyFenotype(TitularNation.PhenotypeMale);
            Culture[Gender.Female] = new Culture(TitularNation.ProtoSociety.Culture[Gender.Female], Customs.Mutation.Possible);
            Culture[Gender.Female].Customs.ApplyFenotype(TitularNation.PhenotypeFemale);

            FixSexCustoms();
        }

        public NationalSociety(Race pRace, Epoch pEpoch, Nation pNation)
        {
            TitularNation = pNation;
            Name = TitularNation.Race.Language.RandomCountryName();

            TechLevel = pEpoch.m_iNativesMaxTechLevel;
            MagicLimit = pEpoch.m_iNativesMaxMagicLevel;

            var pRaceMentality = new Mentality(pRace.MentalityTemplate);
            var pRaceCustoms = new Customs();

            Culture[Gender.Male] = new Culture(pRaceMentality, TechLevel, pRaceCustoms);
            Culture[Gender.Female] = new Culture(pRaceMentality, TechLevel, new Customs(pRaceCustoms, Customs.Mutation.Possible));

            Culture[Gender.Female].Customs.ApplyFenotype(TitularNation.PhenotypeFemale);

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
            float fFood = TitularNation.GetAvailableFood(cResources, iPopulation);

            if (cResources[LandResource.Wood] * 2 < Rnd.Get(iPopulation) && cResources[LandResource.Ore] * 2 < Rnd.Get(iPopulation))// && Rnd.OneChanceFrom(2))
                TechLevel -= 2;
            else if (cResources[LandResource.Wood] + cResources[LandResource.Ore] < Rnd.Get(iPopulation))// && Rnd.OneChanceFrom(2))
                TechLevel--;
            else if (cResources[LandResource.Wood] > Rnd.Get(iPopulation) * 2 && cResources[LandResource.Ore] > Rnd.Get(iPopulation) * 2)// || Rnd.OneChanceFrom(4))
                TechLevel++;

            if (TitularNation.IsInvader)
            {
                if (TechLevel < TitularNation.Epoch.m_iInvadersMinTechLevel)
                    TechLevel = TitularNation.Epoch.m_iInvadersMinTechLevel;
                if (TechLevel > TitularNation.Epoch.m_iInvadersMaxTechLevel)
                    TechLevel = TitularNation.Epoch.m_iInvadersMaxTechLevel;
            }
            else
            {
                if (TechLevel < TitularNation.Epoch.m_iNativesMinTechLevel)
                    TechLevel = TitularNation.Epoch.m_iNativesMinTechLevel;
                if (TechLevel > TitularNation.Epoch.m_iNativesMaxTechLevel)
                    TechLevel = TitularNation.Epoch.m_iNativesMaxTechLevel;
            }

            //m_iInfrastructureLevel = 4 - (int)(m_pCulture.GetDifference(Culture.IdealSociety, m_iTechLevel, m_iTechLevel) * 4);
            InfrastructureLevel = TechLevel;// -(int)(m_iTechLevel * Math.Pow(Rnd.Get(1f), 3));

            if (iSize == 1 && InfrastructureLevel > 4)
                InfrastructureLevel /= 2;

            if (TechLevel == 0 && TitularNation.ProtoSociety.MagicLimit == 0)
                InfrastructureLevel = 0;

            //TODO: нужно учитывать размеры и телосложение - гиганты и толстяки едят больше, чем карлики и худышки
            if (fFood * 2 < iPopulation)
                InfrastructureLevel--;            
            if (fFood < iPopulation || Rnd.OneChanceFrom(10))
                InfrastructureLevel--;
            if (fFood > iPopulation * 2 && Rnd.OneChanceFrom(10))
                InfrastructureLevel++;

            if (InfrastructureLevel < 0)
                InfrastructureLevel = 0;
            if (InfrastructureLevel > TechLevel + 1)
                InfrastructureLevel = TechLevel + 1;
            if (InfrastructureLevel > 8)
                InfrastructureLevel = 8;

            // Adjusting TL due to infrastructure level
            while (GetEffectiveTech() > InfrastructureLevel * 2)
                TechLevel--;

            if (TechLevel < 0)
                TechLevel = 0;

            if (TitularNation.IsInvader)
            {
                if (TechLevel > TitularNation.Epoch.m_iInvadersMaxTechLevel)
                    TechLevel = TitularNation.Epoch.m_iInvadersMaxTechLevel;
            }
            else
            {
                if (TechLevel > TitularNation.Epoch.m_iNativesMaxTechLevel)
                    TechLevel = TitularNation.Epoch.m_iNativesMaxTechLevel;
            }
        }

        public void UpdateTitularNation(Nation pNewTitularNation)
        {
            TitularNation = pNewTitularNation;

            Culture[Gender.Male].Customs.ApplyFenotype(TitularNation.PhenotypeMale);
            Culture[Gender.Female].Customs.ApplyFenotype(TitularNation.PhenotypeFemale);
        }
    }
}
