using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Random;
using NameGen;
using LandscapeGeneration;
using Socium.Settlements;
using Socium.Nations;
using LandscapeGeneration.PathFind;

namespace Socium
{
    public class LandX: Land<LocationX, LandTypeInfoX>
    {
        public Province m_pProvince = null;

        public string m_sName = "";

        public Nation m_pNation;

        /// <summary>
        /// Военное присутсвие сил провинции в данной земле.
        /// Нужно только для формирования карты провинций.
        /// </summary>
        public int m_iProvincePresence = 0;

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
                    pLoc.m_cRoads[RoadQuality.Country].Count > 0 ||
                    pLoc.m_cRoads[RoadQuality.Normal].Count > 0 ||
                    pLoc.m_cRoads[RoadQuality.Good].Count > 0 ||
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

        /// <summary>
        /// находит в земле незанятую локацию, строит там поселение и обновляет проходящие через него дороги.
        /// если незанятых локаций нет - возвращает null, иначе - локацию с построенным поселением
        /// </summary>
        /// <param name="pInfo">тип поселения</param>
        /// <param name="bCapital">если true - будем пытаться строить даже если все места заняты. исключение - только если пригодных для заселения локаций нет вообще в принципе</param>
        /// <param name="bFast">флаг режима ускоренной генерации - в "быстром" режиме не составляем имя для поселения</param>
        /// <returns></returns>
        public LocationX BuildSettlement(SettlementInfo pInfo, bool bCapital, bool bFast)
        {
            //Теперь в этой земле выберем локацию, желательно не на границе с другой землёй.
            //Исключение для побережья - ему наоборот, предпочтение.
            List<int> cChances = new List<int>();
            bool bNoChances = true;

            //вычислим шансы быть выбранной для строительства для всех локаций в земле.
            foreach (LocationX pLoc in m_cContents)
            {
                bool bCoast = false;
                bool bBorder = false;
                //определим, является ли эта локация пограничной с другой землёй или побережьем.
                foreach (LocationX pLink in pLoc.m_aBorderWith)
                {
                    if (pLink.Owner != pLoc.Owner)
                    {
                        bBorder = true;
                        if (pLink.Owner != null && (pLink.Owner as LandX).IsWater)
                            bCoast = true;
                    }
                }

                //сколько дорог проходит через этй локацию?
                int iRoadsCount = 0;
                foreach(var pRoads in pLoc.m_cRoads)
                    iRoadsCount += pRoads.Value.Count;

                //в пограничных локациях строим только если это побережье или есть дороги, во внутренних - отдаём предпочтение локациям с дорогами.
                int iChances = (bBorder ? ((bCoast || iRoadsCount > 0) ? 50 : 1) : (iRoadsCount > 0 ? 50 : 10));

                //если в локации уже есть поселение - ловить нечего. На руинах, однако, строить можно.
                if (pLoc.m_pSettlement != null && pLoc.m_pSettlement.m_iRuinsAge == 0)
                    iChances = 0;

                //если в локации есть какая-то одиночная постройка (монстрячье логово, например), опять ловить нечего.
                if (pLoc.m_pBuilding != null)
                    iChances = 0;

                //если это край карты или географическая аномалия (горный пик, вулкан...) - тоже пролетаем.
                if (pLoc.m_bBorder || pLoc.m_eType != RegionType.Empty)
                    iChances = 0;

                if(iChances > 0)
                    bNoChances = false;

                cChances.Add(iChances);
            }

            //если для всех локаций в земле шансы нулевые, а строим мы столицу - попытаемся найти подходящую локацию ещё раз, ослабив критерии отбора.
            //в частности, разрешаем строить поверх других поселений или одиночных построек и в географических аномалиях.
            //на краю карты строить по прежнему нельзя.
            if (bNoChances)
            {
                if (bCapital)
                {
                    bNoChances = true;
                    cChances.Clear();
                    //вычислим шансы быть выбранной для строительства для всех локаций в земле.
                    foreach (LocationX pLoc in m_cContents)
                    {
                        bool bCoast = false;
                        bool bBorder = false;
                        //определим, является ли эта локация пограничной с другой землёй или побережьем.
                        foreach (LocationX pLink in pLoc.m_aBorderWith)
                        {
                            if (pLink.Owner != pLoc.Owner)
                            {
                                bBorder = true;
                                if (pLink.Owner != null && (pLink.Owner as LandX).IsWater)
                                    bCoast = true;
                            }
                        }

                        //сколько дорог проходит через этй локацию?
                        int iRoadsCount = 0;
                        foreach (var pRoads in pLoc.m_cRoads)
                            iRoadsCount += pRoads.Value.Count;

                        //в пограничных локациях строим только если это побережье или есть дороги, во внутренних - отдаём предпочтение локациям с дорогами.
                        int iChances = (bBorder ? ((bCoast || iRoadsCount > 0) ? 50 : 1) : (iRoadsCount > 0 ? 50 : 10));

                        if (pLoc.m_pSettlement != null || pLoc.m_pBuilding != null)
                            iChances = 1;

                        //если это край карты - тоже пролетаем.
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
            m_cContents[iTown].m_pSettlement = new Settlement(pInfo, m_pNation, m_pProvince.m_iTechLevel, m_pProvince.m_pNation.m_iMagicLimit, bCapital, bFast);
            //foreach (LocationX pLoc in m_cContents[iTown].m_aBorderWith)
            //    if (pLoc.m_pBuilding == null)
            //        pLoc.m_pBuilding = new BuildingStandAlone(BuildingType.Farm);

            //обновим все дороги, проходящие через новое поселение, чтобы на дорожных указателях появилось имя нового поселения.
            List<Road> cRoads = new List<Road>();
            cRoads.AddRange(m_cContents[iTown].m_cRoads[RoadQuality.Country]);
            cRoads.AddRange(m_cContents[iTown].m_cRoads[RoadQuality.Normal]);
            cRoads.AddRange(m_cContents[iTown].m_cRoads[RoadQuality.Good]);

            Road[] aRoads = cRoads.ToArray();
            foreach(Road pRoad in aRoads)
                World.RenewRoad(pRoad);

            return m_cContents[iTown];
        }

        public string GetMajorRaceString()
        {
            if (m_pNation == null)
                return "unpopulated";
            else
                return m_pNation.ToString();
        }

        public string GetLesserRaceString()
        {
            if (Area != null)
            {
                if ((Area as AreaX).m_pNation == null)
                    return "unpopulated";
                else
                    return (Area as AreaX).m_pNation.ToString();
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

        internal LocationX BuildFort(State pEnemy, bool bFast)
        {
            //сначала составим список претендентов из числа незанятых локаций, лежащих непосредственно на границе с супостатом (или на берегу, если супостат не определён).
            List<int> cChances = new List<int>();
            bool bNoChances = true;
            foreach (LocationX pLoc in m_cContents)
            {
                bool bCoast = false;
                bool bBorder = false;
                foreach (LocationX pLink in pLoc.m_aBorderWith)
                {
                    if (pLink.Owner != null)
                    {
                        LandX pLandLink = pLink.Owner as LandX;
                        if (pLandLink.m_pProvince != null && pLandLink.m_pProvince.Owner == pEnemy)
                            bBorder = true;
                    }
                    if (pLink.Owner != null && (pLink.Owner as LandX).IsWater)
                        bCoast = true;
                }

                int iChances = 100;

                if (pEnemy != null && !bBorder)
                    iChances = 0;

                if (pEnemy == null && !bCoast)
                    iChances = 0;

                if (pLoc.m_pSettlement != null)
                    iChances = 0;

                if (pLoc.m_pBuilding != null)
                    iChances = pLoc.m_pBuilding.m_eType == BuildingType.Farm || pLoc.m_pBuilding.m_eType == BuildingType.HuntingFields ? 1 : 0;

                if (pLoc.m_bBorder || pLoc.m_eType != RegionType.Empty)
                    iChances = 0;

                if (iChances > 0)
                    bNoChances = false;

                cChances.Add(iChances);
            }

            //если список претендентов пуст - снизим критерии. теперь сгодится любая незанятая локация
            if (bNoChances)
            {
                //cChances.Clear();
                //foreach (LocationX pLoc in m_cContents)
                //{
                //    int iChances = 100;

                //    if (pLoc.m_pSettlement != null)
                //        iChances = 0;

                //    if (pLoc.m_pBuilding != null)
                //        iChances = pLoc.m_pBuilding.m_eType == BuildingType.Farm || pLoc.m_pBuilding.m_eType == BuildingType.HuntingFields ? 1 : 0;

                //    if (pLoc.m_bBorder || pLoc.m_eType != RegionType.Empty)
                //        iChances = 0;

                //    if (iChances > 0)
                //        bNoChances = false;

                //    cChances.Add(iChances);
                //}

                ////в самом деле, совершенно нет места!
                //if (bNoChances)
                    return null;
            }

            int iFort = Rnd.ChooseOne(cChances, 2);
            //Построим форт в выбранной локации.
            //Все локации на 1 шаг вокруг пометим как поля, чтобы там не возникало никаких новых поселений.

            if (m_cContents[iFort].m_pSettlement == null && m_cContents[iFort].m_pBuilding == null)
            {
                m_cContents[iFort].m_pSettlement = new Settlement(Settlement.Info[SettlementSize.Fort], m_pNation, (m_pProvince.Owner as State).m_iTechLevel, m_pProvince.m_pNation.m_iMagicLimit, false, bFast);

                foreach (LocationX pLoc in m_cContents[iFort].m_aBorderWith)
                    if (pLoc.m_pBuilding == null)
                        pLoc.m_pBuilding = new BuildingStandAlone(BuildingType.Farm);

                List<Road> cRoads = new List<Road>();
                foreach (var pRoads in m_cContents[iFort].m_cRoads)
                    cRoads.AddRange(pRoads.Value);

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
