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
    /// добавляет ресурсы, допустимую частоту поселений и логов
    /// </summary>
    public class LandTypeInfoX : LandTypeInfo
    {
        public enum Resource
        { 
            Grain,
            Game,
            Wood,
            Ore
        }

        public readonly Dictionary<Resource, float> m_cResources = new Dictionary<Resource, float>();

        public void SetResources(float fGrain, float fGame, float fWood, float fOre)
        {
            m_cResources[Resource.Grain] = fGrain;
            m_cResources[Resource.Game] = fGame;
            m_cResources[Resource.Wood] = fWood;
            m_cResources[Resource.Ore] = fOre;
        }

        public Dictionary<SettlementSize, float> m_cSettlementsDensity = new Dictionary<SettlementSize, float>();

        /// <summary>
        /// Вероятность встретить поселение заданного типа в земле этого типа.
        /// ВАЖНО: взаимное соотношение вероятностей разных типов поселений для одного типа территории никакого значения НЕ ИГРАЕТ.
        /// Значение играет соотношение вероятностей одного и того же типа поселений для разных типов территорий - 
        /// выбираться для постройки поселения заданного размера будет земля с более высокой вероятностью.
        /// </summary>
        /// <param name="fCity">Capital и City</param>
        /// <param name="fTown">Town</param>
        /// <param name="fVillage">Village и Hamlet</param>
        public void SetSettlementsDensity(float fCity, float fTown, float fVillage)
        {
            m_cSettlementsDensity[SettlementSize.Capital] = fCity;
            m_cSettlementsDensity[SettlementSize.City] = fCity;
            m_cSettlementsDensity[SettlementSize.Town] = fTown;
            m_cSettlementsDensity[SettlementSize.Village] = fVillage;
            m_cSettlementsDensity[SettlementSize.Hamlet] = fVillage;
        }

        public Dictionary<BuildingType, int> m_cStandAloneBuildingsProbability = new Dictionary<BuildingType, int>();

        public void SetStandAloneBuildingsProbability(int iLair, int iHideout, int iNone)
        {
            m_cStandAloneBuildingsProbability[BuildingType.Lair] = iLair;
            m_cStandAloneBuildingsProbability[BuildingType.Hideout] = iHideout;
            m_cStandAloneBuildingsProbability[BuildingType.None] = iNone;
        }
    }
}
