using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        private enum LandType
        {
            Sea,
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

        private Dictionary<LandType, LTI> m_pLandTypes = new Dictionary<LandType, LTI>();

        private static LandTypes<LTI> m_pInstance = new LandTypes<LTI>();

        private LandTypes()
        {
            foreach (LandType eType in Enum.GetValues(typeof(LandType)))
                m_pLandTypes[eType] = new LTI();
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

        public static LTI Sea
        {
            get { return m_pInstance.m_pLandTypes[LandType.Sea]; }
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
