using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace LandscapeGeneration
{
    public enum EnvironmentType
    { 
        Water,
        Ground,
        Mountains
    }

    public class LandTypeInfo
    {
        public Color m_pColor;
        public Brush m_pBrush;

        public void SetColor(Color pColor)
        {
            m_pColor = pColor;
            m_pBrush = new SolidBrush(m_pColor);
        }
        
        public int m_iMovementCost = 100;

        public EnvironmentType m_eType = EnvironmentType.Ground;

        public string m_sName;

        public void Init(int iMovementCost, EnvironmentType eType, string sName)
        {
            m_iMovementCost = iMovementCost;
            m_eType = eType;
            m_sName = sName;
        }
    }

    public class LandTypes<LTI>
        where LTI: LandTypeInfo, new()
    {
        public enum LandType
        {
            Coastral,
            Ocean,
            Mountains,
            Tundra,
            Taiga,
            Forest,
            Plains,
            Desert,
            Savanna,
            Swamp,
            Jungle
        }

        public Dictionary<LandType, LTI> m_pLandTypes = new Dictionary<LandType, LTI>();

        public static LandTypes<LTI> m_pInstance = new LandTypes<LTI>();

        private LandTypes()
        {
            foreach (LandType eType in Enum.GetValues(typeof(LandType)))
                m_pLandTypes[eType] = new LTI();
        }

        public static LandType GetLandType(LTI pLTI)
        {
            foreach (var pItem in m_pInstance.m_pLandTypes)
                if (pItem.Value == pLTI)
                    return pItem.Key;

            return LandType.Ocean;
        }

        public static LTI Desert
        {
            get { return m_pInstance.m_pLandTypes[LandType.Desert]; }
        }

        public static LTI Forest
        {
            get { return m_pInstance.m_pLandTypes[LandType.Forest]; }
        }

        public static LTI Jungle
        {
            get { return m_pInstance.m_pLandTypes[LandType.Jungle]; }
        }

        public static LTI Mountains
        {
            get { return m_pInstance.m_pLandTypes[LandType.Mountains]; }
        }

        public static LTI Plains
        {
            get { return m_pInstance.m_pLandTypes[LandType.Plains]; }
        }

        public static LTI Savanna
        {
            get { return m_pInstance.m_pLandTypes[LandType.Savanna]; }
        }

        public static LTI Ocean
        {
            get { return m_pInstance.m_pLandTypes[LandType.Ocean]; }
        }

        public static LTI Coastral
        {
            get { return m_pInstance.m_pLandTypes[LandType.Coastral]; }
        }

        public static LTI Swamp
        {
            get { return m_pInstance.m_pLandTypes[LandType.Swamp]; }
        }

        public static LTI Taiga
        {
            get { return m_pInstance.m_pLandTypes[LandType.Taiga]; }
        }

        public static LTI Tundra
        {
            get { return m_pInstance.m_pLandTypes[LandType.Tundra]; }
        }
    }
}
