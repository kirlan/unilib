using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Random;
using NameGen;
using LandscapeGeneration;

namespace Socium
{
    public class LandX: Land<LocationX, LandTypeInfoX>
    {
        public Province m_pProvince = null;

        public string m_sName = "";

        public Race m_pRace;

        public ContinentX Continent
        {
            get
            {
                LandMass<LandX> pLandMass = Owner as LandMass<LandX>;
                if (pLandMass != null)
                    return pLandMass.Owner as ContinentX;

                return null;
            }
        }

        public LocationX BuildLair()
        {
            //Теперь в этой земле выберем локацию, желательно не на границе с другой землёй.
            //Исключение для побережья - ему наоборот, предпочтение.
            List<int> cChances = new List<int>();
            bool bNoChances = true;
            foreach (LocationX pLoc in m_cContents)
            {
                bool bBorder = false;
                foreach (LocationX pLink in pLoc.m_aBorderWith)
                {
                    if (pLink.Owner != pLoc.Owner)
                        bBorder = true;
                }

                int iChances = bBorder ? 1 : 50;

                if (pLoc.m_pSettlement != null || 
                    pLoc.m_pBuilding != null || 
                    pLoc.m_cRoads.Count > 0 || 
                    pLoc.m_bBorder)
                    iChances = 0;
                else
                    bNoChances = false;

                cChances.Add(iChances);
            }

            if (bNoChances)
                return null;

            int iLair = Rnd.ChooseOne(cChances, 3);

            BuildingType eSize = BuildingType.Lair;

            int iSize = Rnd.ChooseOne(Type.m_cStandAloneBuildingsProbability.Values, 1);
            foreach (BuildingType eType in Type.m_cStandAloneBuildingsProbability.Keys)
            {
                iSize--;
                if (iSize < 0)
                {
                    eSize = eType;
                    break;
                }
            }

            if (eSize == BuildingType.None)
                return null;

            m_cContents[iLair].m_pBuilding = new BuildingStandAlone(eSize);
            //m_cLocations[iLair].m_sName = NameGenerator.GetAbstractName();

            foreach (LocationX pLoc in m_cContents[iLair].m_aBorderWith)
            {
                if (pLoc.m_pBuilding == null)
                {
                    pLoc.m_pBuilding = new BuildingStandAlone(BuildingType.HuntingFields);
                }
            }

            return m_cContents[iLair];
        }

        public LocationX BuildSettlement(SettlementInfo pInfo, bool bCapital, bool bFast)
        {
            //Теперь в этой земле выберем локацию, желательно не на границе с другой землёй.
            //Исключение для побережья - ему наоборот, предпочтение.
            List<int> cChances = new List<int>();
            bool bNoChances = true;
            foreach (LocationX pLoc in m_cContents)
            {
                bool bCoast = false;
                bool bBorder = false;
                foreach (Location pLink in pLoc.m_aBorderWith)
                {
                    if (pLink.Owner != pLoc.Owner)
                        bBorder = true;
                    if (pLink.Owner != null && (pLink.Owner as LandX).IsWater)
                        bCoast = true;
                }

                int iChances = (bBorder ? ((bCoast || pLoc.m_cRoads.Count > 0) ? 50 : 1) : ((bCoast || pLoc.m_cRoads.Count > 0) ? 50 : 10));

                if (pLoc.m_pSettlement != null || pLoc.m_pBuilding != null)
                    iChances = 0;

                if (pLoc.m_bBorder || pLoc.m_eType != RegionType.Empty)
                    iChances = 0;

                if(iChances > 0)
                    bNoChances = false;

                cChances.Add(iChances);
            }

            if (bNoChances)
            {
                if (bCapital)
                {
                    bNoChances = true;
                    cChances.Clear();
                    foreach (LocationX pLoc in m_cContents)
                    {
                        bool bCoast = false;
                        bool bBorder = false;
                        foreach (Location pLink in pLoc.m_aBorderWith)
                        {
                            if (pLink.Owner != pLoc.Owner)
                                bBorder = true;
                            if (pLink.Owner != null && (pLink.Owner as LandX).IsWater)
                                bCoast = true;
                        }

                        int iChances = (bBorder ? ((bCoast || pLoc.m_cRoads.Count > 0) ? 50 : 1) : ((bCoast || pLoc.m_cRoads.Count > 0) ? 50 : 10));

                        if (pLoc.m_pSettlement != null || pLoc.m_pBuilding != null)
                            iChances = 1;

                        if (pLoc.m_bBorder)
                            iChances = 0;

                        if (iChances > 0)
                            bNoChances = false;

                        cChances.Add(iChances);
                    }
                    //В самом деле, совершенно нет места!
                    if (bNoChances)
                        return null;
                }
                else
                    return null;
            }

            int iTown = Rnd.ChooseOne(cChances, 2);
            //Построим город в выбранной локации.
            //Все локации на 2 шага вокруг пометим как поля, чтобы там не возникало никаких новых поселений.

            m_cContents[iTown].m_pSettlement = new Settlement(pInfo, m_pRace, (m_pProvince.Owner as State).m_iTechLevel, (m_pProvince.Owner as State).m_iMagicLimit, bCapital, bFast);
            foreach (LocationX pLoc in m_cContents[iTown].m_aBorderWith)
                if (pLoc.m_pBuilding == null)
                    pLoc.m_pBuilding = new BuildingStandAlone(BuildingType.Farm);

            List<Road> cRoads = new List<Road>();
            cRoads.AddRange(m_cContents[iTown].m_cRoads[1]);
            cRoads.AddRange(m_cContents[iTown].m_cRoads[2]);
            cRoads.AddRange(m_cContents[iTown].m_cRoads[3]);

            Road[] aRoads = cRoads.ToArray();
            foreach(Road pRoad in aRoads)
                World.RenewRoad(pRoad);

            return m_cContents[iTown];
        }

        public string GetMajorRaceString()
        {
            if (m_pRace == null)
                return "unpopulated";
            else
                return m_pRace.ToString();
        }

        public string GetLesserRaceString()
        {
            if (Area != null)
            {
                if ((Area as AreaX).m_pRace == null)
                    return "unpopulated";
                else
                    return (Area as AreaX).m_pRace.ToString();
            }
            return "unpopulated";
        }

        public override string ToString()
        {
            //return GetLandsString();

            string sRace = GetMajorRaceString();
            if (sRace != GetLesserRaceString())
                sRace = sRace + "/" + GetLesserRaceString();

            return string.Format("{0} {1} ({2}, {3}) (H:{4}%)", m_sName, Type.m_sName, m_cContents.Count, sRace, Humidity);
        }

        internal LocationX BuildFort(bool bFast)
        {
            //Теперь в этой земле выберем локацию, желательно на границе с другой землёй или на побережье.
            List<int> cChances = new List<int>();
            bool bNoChances = true;
            foreach (LocationX pLoc in m_cContents)
            {
                //bool bCoast = false;
                bool bBorder = false;
                foreach (Location pLink in pLoc.m_aBorderWith)
                {
                    if (pLink.Owner != null)
                    {
                        LandX pLandLink = pLink.Owner as LandX;
                        if (pLandLink.m_pProvince == null || pLandLink.m_pProvince.Owner != m_pProvince.Owner)
                            bBorder = true;
                    }
                    //if (pLink.Owner != null && (pLink.Owner as LandX).IsWater)
                    //    bCoast = true;
                }

                int iChances = bBorder ? 100 : 1;

                if (pLoc.m_pSettlement != null || pLoc.m_pBuilding != null)// && pLoc.Type != RegionType.Field)
                    iChances = 0;

                if (pLoc.m_bBorder || pLoc.m_eType != RegionType.Empty)
                    iChances = 0;

                if (iChances > 0)
                    bNoChances = false;

                cChances.Add(iChances);
            }

            if (bNoChances)
                return null;

            int iFort = Rnd.ChooseOne(cChances, 2);
            //Построим город в выбранной локации.
            //Все локации на 2 шага вокруг пометим как поля, чтобы там не возникало никаких новых поселений.

            if (m_cContents[iFort].m_pSettlement == null && m_cContents[iFort].m_pBuilding == null)
            {
                m_cContents[iFort].m_pSettlement = new Settlement(Settlement.Info[SettlementSize.Fort], m_pRace, (m_pProvince.Owner as State).m_iTechLevel, (m_pProvince.Owner as State).m_iMagicLimit, false, bFast);

                foreach (LocationX pLoc in m_cContents[iFort].m_aBorderWith)
                    if (pLoc.m_pBuilding == null)
                        pLoc.m_pBuilding = new BuildingStandAlone(BuildingType.Farm);

                List<Road> cRoads = new List<Road>();
                cRoads.AddRange(m_cContents[iFort].m_cRoads[1]);
                cRoads.AddRange(m_cContents[iFort].m_cRoads[2]);
                cRoads.AddRange(m_cContents[iFort].m_cRoads[3]);

                Road[] aRoads = cRoads.ToArray();
                foreach (Road pRoad in aRoads)
                    World.RenewRoad(pRoad);

                return m_cContents[iFort];
            }
            else
                return null;
        }
    }
}
