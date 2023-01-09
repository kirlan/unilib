using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LandscapeGeneration;
using System.Drawing;
using Socium.Settlements;
using Socium.Nations;

namespace Socium
{
    /// <summary>
    /// расширение LandscapeGeneration.LandTypeInfo
    /// информация о доступных ресурсах
    /// </summary>
    public class ResourcesInfo : ILandTypeInfoExt
    {
        private readonly Dictionary<LandResource, float> m_cResources = new Dictionary<LandResource, float>();

        public ResourcesInfo(float fGrain, float fGame, float fWood, float fOre)
        {
            m_cResources[LandResource.Grain] = fGrain;
            m_cResources[LandResource.Game] = fGame;
            m_cResources[LandResource.Wood] = fWood;
            m_cResources[LandResource.Ore] = fOre;
        }

        public float GetAmount(LandResource key)
        {
            return m_cResources[key];
        }
    }

    /// <summary>
    /// расширение LandscapeGeneration.LandTypeInfo
    /// информация о допустимой частоте поселений
    /// </summary>
    public class SettlementsInfo : ILandTypeInfoExt
    {
        private readonly Dictionary<SettlementSize, float> m_cDensity = new Dictionary<SettlementSize, float>();

        /// <summary>
        /// Вероятность встретить поселение заданного типа в земле этого типа.
        /// ВАЖНО: взаимное соотношение вероятностей разных типов поселений для одного типа территории никакого значения НЕ ИГРАЕТ.
        /// Значение играет соотношение вероятностей одного и того же типа поселений для разных типов территорий -
        /// выбираться для постройки поселения заданного размера будет земля с более высокой вероятностью.
        /// </summary>
        /// <param name="fCity">Capital и City</param>
        /// <param name="fTown">Town</param>
        /// <param name="fVillage">Village и Hamlet</param>
        public SettlementsInfo(float fCity, float fTown, float fVillage)
        {
            m_cDensity[SettlementSize.Capital] = fCity;
            m_cDensity[SettlementSize.City] = fCity;
            m_cDensity[SettlementSize.Town] = fTown;
            m_cDensity[SettlementSize.Village] = fVillage;
            m_cDensity[SettlementSize.Hamlet] = fVillage;
        }

        public float GetDensity(SettlementSize eSize)
        {
            return m_cDensity[eSize];
        }
    }

    /// <summary>
    /// расширение LandscapeGeneration.LandTypeInfo
    /// добавляет ресурсы, допустимую частоту поселений и логов
    /// </summary>
    public class StandAloneBuildingsInfo : ILandTypeInfoExt
    {
        public Dictionary<BuildingType, int> Probability { get; } = new Dictionary<BuildingType, int>();

        public StandAloneBuildingsInfo(int iLair, int iHideout, int iNone)
        {
            Probability[BuildingType.Lair] = iLair;
            Probability[BuildingType.Hideout] = iHideout;
            Probability[BuildingType.None] = iNone;
        }
    }
}
