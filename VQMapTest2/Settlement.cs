using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NameGen;
using Random;

namespace VQMapTest2
{
    public enum SettlementSize
    {
        Hamlet,
        Village,
        Town,
        City,
        Capital,
        Fort
    }

    public class SettlementInfo
    {
        public string m_sName;
        public int m_iMinPop;
        public int m_iDeltaPop;
        public int m_iMaxProfessionRank;
        public int m_iMinBuildingsCount;
        public int m_iDeltaBuildingsCount;
        public BuildingInfo m_pMainBuilding;
        public SettlementSize m_eSize;

        public SettlementInfo(SettlementSize eSize, string sName, int iMinPop, int iDeltaPop, int iMaxProfessionRank, int iMinBuildingsCount, int iDeltaBuildingsCount, BuildingInfo pMainBuilding)
        {
            m_eSize = eSize;
            m_sName = sName;
            m_iMinPop = iMinPop;
            m_iDeltaPop = iDeltaPop;
            m_iMaxProfessionRank = iMaxProfessionRank;
            m_iMinBuildingsCount = iMinBuildingsCount;
            m_iDeltaBuildingsCount = iDeltaBuildingsCount;
            m_pMainBuilding = pMainBuilding;
        }
    }

    public class Settlement
    {
        private static Dictionary<SettlementSize, SettlementInfo> m_cInfo = new Dictionary<SettlementSize, SettlementInfo>();

        internal static Dictionary<SettlementSize, SettlementInfo> Info
        {
            get
            {
                if (m_cInfo.Count == 0)
                {
                    m_cInfo[SettlementSize.Hamlet] = new SettlementInfo(SettlementSize.Hamlet, "Hamlet", 5, 10, 2, 0, 1, null);
                    m_cInfo[SettlementSize.Village] = new SettlementInfo(SettlementSize.Village, "Village", 10, 20, 3, 0, 3, new BuildingInfo("Village hall", "Elder", "Elder", 3));
                    m_cInfo[SettlementSize.Town] = new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, 2, 5, new BuildingInfo("Castle", "Baron", "Baroness", 7));
                    m_cInfo[SettlementSize.City] = new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, 5, 7, new BuildingInfo("Palace", "Count", "Countess", 14));
                    m_cInfo[SettlementSize.Capital] = new SettlementInfo(SettlementSize.Capital, "City", 40, 80, 15, 5, 10, new BuildingInfo("Citadel", "Lord", "Lady", 15));
                    m_cInfo[SettlementSize.Fort] = new SettlementInfo(SettlementSize.Fort, "Fort", 20, 40, 7, 2, 5, new BuildingInfo("Castle", "General", "General", 7));
                }
                return Settlement.m_cInfo;
            }
        }

        public SettlementInfo m_pInfo;

        public string m_sName;

        /// <summary>
        /// Возраст руин. Если 0, значит ещё не руины.
        /// </summary>
        public int m_iRuinsAge = 0;
        private Race m_pRace;

        public int m_iTechLevel;
        public int m_iMagicLimit;

        public List<Building> m_cBuildings = new List<Building>();

        public Settlement(SettlementInfo pInfo, Race pRace, int iTech, int iMagic, bool bCapital, bool bFast)
        {
            // TODO: Complete member initialization
            m_pInfo = pInfo;
            m_pRace = pRace;

            if (bFast)
                m_sName = "Noname";
            else
            {
                if (pInfo.m_eSize == SettlementSize.Hamlet || pInfo.m_eSize == SettlementSize.Village)
                    m_sName = m_pRace.m_pLanguage.RandomVillageName();
                else
                    m_sName = m_pRace.m_pLanguage.RandomTownName();
            }

            m_iTechLevel = iTech;
            m_iMagicLimit = iMagic;

            if (m_pInfo.m_pMainBuilding != null && (bCapital || !Rnd.OneChanceFrom(3)))
            {
                Building pNewBuilding = new Building(this, m_pInfo.m_pMainBuilding, bCapital);
                m_cBuildings.Add(pNewBuilding);
            }

            int iBuildingsCount = m_pInfo.m_iMinBuildingsCount + Rnd.Get(m_pInfo.m_iDeltaBuildingsCount);
            for (int i = 0; i < iBuildingsCount; i++)
            {
                Building pNewBuilding = new Building(this);
                m_cBuildings.Add(pNewBuilding);
            }
        }

        public override string ToString()
        {
            switch(m_iRuinsAge)
            {
                case 0:
                    return string.Format("{0} {1}", m_pInfo.m_sName, m_sName);
                case 1:
                    return string.Format("ruins of {0} {1}", m_pRace.m_sName, m_pInfo.m_sName);
                case 2:
                    return string.Format("ancient ruins of {0} {1}", m_pRace.m_sName, m_pInfo.m_sName);
                default:
                    return string.Format("forgotten ruins of {0} {1}", m_pRace.m_sName, m_pInfo.m_sName);
            }
        }
    }
}
