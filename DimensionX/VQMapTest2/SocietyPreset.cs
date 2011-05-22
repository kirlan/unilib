using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VQMapTest2
{
    class SocietyPreset
    {
        public string m_sName;

        public override string ToString()
        {
            return m_sName;
        }
    
        public string m_sDescription;

        public int m_iMinTechLevel;
        public int m_iMaxTechLevel;
        public int m_iMinMagicLevel;
        public int m_iMaxMagicLevel;

        public SocietyPreset(string sName, string sDescription, int iMinTechLevel, int iMaxTechLevel, int iMinMagicLevel, int iMaxMagicLevel)
        {
            m_sName = sName;
            m_sDescription = sDescription;

            m_iMinTechLevel = iMinTechLevel;
            m_iMaxTechLevel = iMaxTechLevel;

            m_iMinMagicLevel = iMinMagicLevel;
            m_iMaxMagicLevel = iMaxMagicLevel;
        }
    }
}
