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

        public int m_iLandMassesPercent;

        public int m_iContinentsCount;

        public int m_iWaterCoverage;

        public int m_iLandsCountPercent;

        public MapPreset(string sName, string sDescription, int iLandsCountPercent, int iLandMassesPercent, int iContinentsCount, int iWaterCoverage)
        {
            m_sName = sName;
            m_sDescription = sDescription;
            m_iLandsCountPercent = iLandsCountPercent;
            m_iLandMassesPercent = iLandMassesPercent;
            m_iContinentsCount = iContinentsCount;
            m_iWaterCoverage = iWaterCoverage;
        }
    }
}
