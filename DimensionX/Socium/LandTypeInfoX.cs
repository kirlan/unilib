using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LandscapeGeneration;
using System.Drawing;

namespace Socium
{
    public class LandTypeInfoX : LandTypeInfo
    {
        public Color m_pColor;
        public Brush m_pBrush;

        public void SetColor(Color pColor)
        {
            m_pColor = pColor;
            m_pBrush = new SolidBrush(m_pColor);
        }

        public float m_fFood;
        public float m_fWood;
        public float m_fOre;

        public void SetResources(float fFood, float fWood, float fOre)
        {
            m_fFood = fFood;
            m_fWood = fWood;
            m_fOre = fOre;
        }

        public Dictionary<SettlementSize, float> m_cSettlementsDensity = new Dictionary<SettlementSize, float>();

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
