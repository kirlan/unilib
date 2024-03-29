﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NameGen;
using Random;

namespace RB.Socium
{
    public enum SettlementSize
    {
        Hamlet,
        Village,
        Fort,
        Town,
        City,
        Capital
    }

    public enum SettlementSpeciality
    { 
        None,
        Fishers,
        Lumberjacks,
        Hunters,
        Miners,
        Peasants,
        Farmers,
        Raiders,
        Pirates,
        Military,
        Naval,
        Factory,
        Artisans,
        Jevellers,
        Tailors,
        Resort,
        Cultural,
        Religious,
        Gambling,
        MilitaryAcademy,
        NavalAcademy,
        ArtsAcademy,
        SciencesAcademy
    }

    public class SettlementInfo
    {
        public string m_sName;
        public int m_iMinBuildingsCount;
        public int m_iDeltaBuildingsCount;
        public BuildingInfo m_pMainBuilding;
        public SettlementSize m_eSize;

        public SettlementInfo(SettlementSize eSize, string sName, BuildingInfo pMainBuilding)
        {
            m_eSize = eSize;
            m_sName = sName;

            switch (m_eSize)
            {
                case SettlementSize.Hamlet:
                    m_iMinBuildingsCount = 5;
                    m_iDeltaBuildingsCount = 1;
                    break;
                case SettlementSize.Village:
                    m_iMinBuildingsCount = 7;
                    m_iDeltaBuildingsCount = 2;
                    break;
                case SettlementSize.Town:
                    m_iMinBuildingsCount = 15;
                    m_iDeltaBuildingsCount = 2;
                    break;
                case SettlementSize.City:
                    m_iMinBuildingsCount = 20;
                    m_iDeltaBuildingsCount = 2;
                    break;
                case SettlementSize.Capital:
                    m_iMinBuildingsCount = 30;
                    m_iDeltaBuildingsCount = 2;
                    break;
                case SettlementSize.Fort:
                    m_iMinBuildingsCount = 10;
                    m_iDeltaBuildingsCount = 1;
                    break;
            }
            m_pMainBuilding = pMainBuilding;
        }
    }

    public class CSettlement
    {
        //private static Dictionary<SettlementSize, SettlementInfo> m_cInfo = new Dictionary<SettlementSize, SettlementInfo>();

        //internal static Dictionary<SettlementSize, SettlementInfo> Info
        //{
        //    get
        //    {
        //        if (m_cInfo.Count == 0)
        //        {
        //            m_cInfo[SettlementSize.Hamlet] = new SettlementInfo(SettlementSize.Hamlet, "Hamlet");
        //            m_cInfo[SettlementSize.Village] = new SettlementInfo(SettlementSize.Village, "Village");
        //            m_cInfo[SettlementSize.Town] = new SettlementInfo(SettlementSize.Town, "Town");
        //            m_cInfo[SettlementSize.City] = new SettlementInfo(SettlementSize.City, "City");
        //            m_cInfo[SettlementSize.Capital] = new SettlementInfo(SettlementSize.Capital, "City");
        //            m_cInfo[SettlementSize.Fort] = new SettlementInfo(SettlementSize.Fort, "Fort");
        //        }
        //        return CSettlement.m_cInfo;
        //    }
        //}

        public SettlementInfo m_pInfo;
        public SettlementSpeciality m_eSpeciality = SettlementSpeciality.None;

        public string m_sName;

        /// <summary>
        /// Возраст руин. Если 0, значит ещё не руины.
        /// </summary>
        public int m_iRuinsAge = 0;
        private CNation m_pNation;

        public int m_iTechLevel;
        public int m_iMagicLimit;

        public bool m_bCapital;

        public List<Building> m_cBuildings = new List<Building>();

        public CSettlement(SettlementInfo pInfo, CNation pNation, int iTech, int iMagic, bool bCapital, bool bFast)
        {
            // TODO: Complete member initialization
            m_pInfo = pInfo;
            m_pNation = pNation;

            if (bFast)
                m_sName = "Noname";
            else
            {
                if (pInfo.m_eSize == SettlementSize.Hamlet || pInfo.m_eSize == SettlementSize.Village)
                    m_sName = m_pNation.m_pRace.m_pLanguage.RandomVillageName();
                else
                    m_sName = m_pNation.m_pRace.m_pLanguage.RandomTownName();
            }

            m_iTechLevel = iTech;
            m_iMagicLimit = iMagic;

            m_bCapital = bCapital;
        }

        public override string ToString()
        {
            switch(m_iRuinsAge)
            {
                case 0:
                    return string.Format("{2} {0} {1}", m_pInfo.m_sName, m_sName, m_eSpeciality);
                case 1:
                    return string.Format("ruins of {2} {0} {1}", m_pNation.m_sName, m_pInfo.m_sName, m_eSpeciality).ToLower();
                case 2:
                    return string.Format("ancient ruins of {2} {0} {1}", m_pNation.m_sName, m_pInfo.m_sName, m_eSpeciality).ToLower();
                default:
                    return string.Format("forgotten ruins of {2} {0} {1}", m_pNation.m_sName, m_pInfo.m_sName, m_eSpeciality).ToLower();
            }
        }
    }
}
