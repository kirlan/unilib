using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorldGeneration
{
    class MapPreset
    {
        public string m_sName;

        public override string ToString()
        {
            return m_sName;
        }

        public string m_sDescription;

        public bool m_bLooped;

        public bool m_bBordered;

        public int m_iLandMassesCount;

        public int m_iContinentsCount;

        public int m_iWaterCoverage;

        public int m_iEquatorPosition;

        public int m_iPoleDistance;

        public int m_iLandsCountPercent;

        public MapPreset(string sName, string sDescription, bool bLooped, bool bBordered, int iLandsCountPercent, int iLandMassesCount, int iContinentsCount, int iWaterCoverage, int iEquatorPosition, int iPoleDistance)
        {
            m_sName = sName;
            m_sDescription = sDescription;
            m_bLooped = bLooped;
            m_bBordered = bBordered;
            m_iLandsCountPercent = iLandsCountPercent;
            m_iLandMassesCount = iLandMassesCount;
            m_iContinentsCount = iContinentsCount;
            m_iWaterCoverage = iWaterCoverage;
            m_iEquatorPosition = iEquatorPosition;
            m_iPoleDistance = iPoleDistance;
        }
    }
}
