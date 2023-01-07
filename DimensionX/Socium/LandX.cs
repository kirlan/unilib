﻿using System;
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
    /// <summary>
    /// расширение LandscapeGeneration.Land
    /// добавляет ссылку на регион, к которому принадлежит земля, доминирующую нацию, имя, 
    /// а так же методы для строительства логова, поселения или форта
    /// </summary>
    public class LandX: TerritoryExtended<Land>
    {
        private string m_sName = "";

        /// <summary>
        /// доминирующая нация в локации, может отличаться от коренного населения региона
        /// </summary>
        public Nation m_pDominantNation;

        public ContinentX Continent
        {
            get
            {
                LandMass pLandMass = Origin.GetOwner<LandMass>();
                if (pLandMass != null)
                    return pLandMass.GetOwner<Continent>().As<ContinentX>();

                return null;
            }
        }

        public LandX(Land pLand) : base(pLand)
        { 
        }

        public Location BuildLair()
        {
            //Теперь в этой земле выберем локацию, желательно не на границе с другой землёй.
            //Исключение для побережья - ему наоборот, предпочтение.
            List<int> cChances = new List<int>();
            bool bNoChances = true;
            foreach (Location pLoc in Origin.Contents)
            {
                bool bBorder = false;
                bool bMapBorder = false;
                foreach (Location pLink in pLoc.m_aBorderWith)
                {
                    if (pLink.GetOwner<Land>() != pLoc.GetOwner<Land>())
                        bBorder = true;

                    if (pLink.m_bBorder)
                        bMapBorder = true;
                }

                int iChances = bBorder ? 1 : 50;

                LocationX pLocX = pLoc.As<LocationX>();

                if (pLocX.m_pSettlement != null || 
                    pLocX.m_pBuilding != null ||
                    pLocX.m_cRoads[RoadQuality.Country].Count > 0 ||
                    pLocX.m_cRoads[RoadQuality.Normal].Count > 0 ||
                    pLocX.m_cRoads[RoadQuality.Good].Count > 0 ||
                    pLoc.m_bBorder ||
                    bMapBorder)
                    iChances = 0;
                else
                    bNoChances = false;

                cChances.Add(iChances);
            }

            if (bNoChances)
                return null;

            int iLair = Rnd.ChooseOne(cChances, 3);
            if (iLair < 0)
                return null;

            BuildingType eSize = BuildingType.Lair;

            int iSize = Rnd.ChooseOne(Origin.LandType.GetLayer<LandTypeInfoStandAlone>().m_cStandAloneBuildingsProbability.Values, 1);
            foreach (BuildingType eType in Origin.LandType.GetLayer<LandTypeInfoStandAlone>().m_cStandAloneBuildingsProbability.Keys)
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

            var pLair = Origin.Contents.ElementAt(iLair);

            pLair.As<LocationX>().m_pBuilding = new BuildingStandAlone(eSize);
            //m_cLocations[iLair].m_sName = NameGenerator.GetAbstractName();

            foreach (Location pLoc in pLair.m_aBorderWith)
            {
                if (pLoc.As<LocationX>().m_pBuilding == null)
                {
                    pLoc.As<LocationX>().m_pBuilding = new BuildingStandAlone(BuildingType.HuntingFields);
                }
            }

            return pLair;
        }

        /// <summary>
        /// находит в земле незанятую локацию, строит там поселение и обновляет проходящие через него дороги.
        /// если незанятых локаций нет - возвращает null, иначе - локацию с построенным поселением
        /// </summary>
        /// <param name="pInfo">тип поселения</param>
        /// <param name="bCapital">если true - будем пытаться строить даже если все места заняты. исключение - только если пригодных для заселения локаций нет вообще в принципе</param>
        /// <param name="bFast">флаг режима ускоренной генерации - в "быстром" режиме не составляем имя для поселения</param>
        /// <returns></returns>
        public Location BuildSettlement(SettlementInfo pInfo, bool bCapital, bool bFast)
        {
            //Теперь в этой земле выберем локацию, желательно не на границе с другой землёй.
            //Исключение для побережья - ему наоборот, предпочтение.
            List<int> cChances = new List<int>();
            bool bNoChances = true;

            //вычислим шансы быть выбранной для строительства для всех локаций в земле.
            foreach (Location pLoc in Origin.Contents)
            {
                bool bCoast = false;
                bool bBorder = false;
                bool bMapBorder = false;
                //определим, является ли эта локация пограничной с другой землёй или побережьем.
                foreach (Location pLink in pLoc.m_aBorderWith)
                {
                    if (pLink.GetOwner<Land>() != pLoc.GetOwner<Land>())
                    {
                        bBorder = true;
                        if (pLink.HasOwner<Land>() && pLink.GetOwner<Land>().IsWater)
                            bCoast = true;
                    }

                    if (pLink.m_bBorder)
                    {
                        bMapBorder = true;
                    }
                }

                LocationX pLocX = pLoc.As<LocationX>();

                //сколько дорог проходит через этй локацию?
                int iRoadsCount = 0;
                foreach(var pRoads in pLocX.m_cRoads)
                    iRoadsCount += pRoads.Value.Count;

                //в пограничных локациях строим только если это побережье или есть дороги, во внутренних - отдаём предпочтение локациям с дорогами.
                int iChances = (bBorder ? ((bCoast || iRoadsCount > 0) ? 50 : 1) : (iRoadsCount > 0 ? 50 : 10));

                if (bMapBorder)
                    iChances = 0;

                //если в локации уже есть поселение - ловить нечего. На руинах, однако, строить можно.
                if (pLocX.m_pSettlement != null && pLocX.m_pSettlement.m_iRuinsAge == 0)
                    iChances = 0;

                //если в локации есть какая-то одиночная постройка (монстрячье логово, например), опять ловить нечего.
                if (pLocX.m_pBuilding != null)
                    iChances = 0;

                //если это край карты или географическая аномалия (горный пик, вулкан...) - тоже пролетаем.
                if (pLoc.m_bBorder || pLoc.m_eType != LandmarkType.Empty)
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
                    foreach (Location pLoc in Origin.Contents)
                    {
                        bool bCoast = false;
                        bool bBorder = false;
                        //определим, является ли эта локация пограничной с другой землёй или побережьем.
                        foreach (Location pLink in pLoc.m_aBorderWith)
                        {
                            if (pLink.GetOwner<Land>() != pLoc.GetOwner<Land>())
                            {
                                bBorder = true;
                                if (pLink.HasOwner<Land>() && pLink.GetOwner<Land>().IsWater)
                                    bCoast = true;
                            }
                        }

                        LocationX pLocX = pLoc.As<LocationX>();

                        //сколько дорог проходит через этй локацию?
                        int iRoadsCount = 0;
                        foreach (var pRoads in pLocX.m_cRoads)
                            iRoadsCount += pRoads.Value.Count;

                        //в пограничных локациях строим только если это побережье или есть дороги, во внутренних - отдаём предпочтение локациям с дорогами.
                        int iChances = (bBorder ? ((bCoast || iRoadsCount > 0) ? 50 : 1) : (iRoadsCount > 0 ? 50 : 10));

                        if (pLocX.m_pSettlement != null || pLocX.m_pBuilding != null)
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
            if (iTown < 0)
                return null;

            var pTown = Origin.Contents.ElementAt(iTown);
            var pTownX = pTown.As<LocationX>();

            //Построим город в выбранной локации.
            //Все локации на 2 шага вокруг пометим как поля, чтобы там не возникало никаких новых поселений.
            pTownX.m_pSettlement = new Settlement(pInfo, m_pDominantNation, GetOwner<Region>().GetOwner<Province>().m_pLocalSociety.m_iTechLevel, GetOwner<Region>().GetOwner<Province>().m_pLocalSociety.m_iMagicLimit, bCapital, bFast);
            //foreach (LocationX pLoc in m_cContents[iTown].m_aBorderWith)
            //    if (pLoc.m_pBuilding == null)
            //        pLoc.m_pBuilding = new BuildingStandAlone(BuildingType.Farm);

            //обновим все дороги, проходящие через новое поселение, чтобы на дорожных указателях появилось имя нового поселения.
            List<Road> cRoads = new List<Road>();
            cRoads.AddRange(pTownX.m_cRoads[RoadQuality.Country]);
            cRoads.AddRange(pTownX.m_cRoads[RoadQuality.Normal]);
            cRoads.AddRange(pTownX.m_cRoads[RoadQuality.Good]);

            Road[] aRoads = cRoads.ToArray();
            foreach(Road pRoad in aRoads)
                World.RenewRoad(pRoad);

            return pTown;
        }

        public string GetMajorRaceString()
        {
            if (m_pDominantNation == null)
                return "unpopulated";
            else
                return m_pDominantNation.ToString();
        }

        public string GetLesserRaceString()
        {
            if (HasOwner<Region>())
            {
                if (GetOwner<Region>().m_pNatives == null)
                    return "unpopulated";
                else
                    return GetOwner<Region>().m_pNatives.ToString();
            }
            return "unpopulated";
        }

        public void Populate(Nation pNation, string sName)
        {
            m_pDominantNation = pNation;
            m_sName = sName;
        }

        public override string ToString()
        {
            //return GetLandsString();

            string sRace = GetMajorRaceString();
            if (sRace != GetLesserRaceString())
                sRace = sRace + "/" + GetLesserRaceString();

            return string.Format("{0} {1} ({2}, {3}) (H:{4}%)", m_sName, Origin.LandType.m_sName, Origin.Contents.Count, sRace, Origin.Humidity);
        }

        /// <summary>
        /// Считает стоимость заселения локации указанной расой с учётом ландшафта локации и её соседей.
        /// Возвращает значение в диапазоне 1-100.
        /// 1 - любая территория, идеально подходящая указанной расе, окружённая так же идеально подходящими.
        /// 5 - идеально подходящая, окружённая простыми для заселения, но совсем не подходящими. Или наоборот.
        /// 10 - простая, но не подходящая, окружённая другими простыми, но не подходящими.
        /// 50 - подходящая, окружённая сложными и не подходящими, или наоборот.
        /// 55 - простая и не подходящая, окружённая сложными и не подходящими, или наоборот.
        /// 100 - сложная и не подходящая, окружённая сложными и не подходящими.
        /// </summary>
        /// <param name="pNation"></param>
        /// <returns></returns>
        public int GetClaimingCost(Nation pNation)
        {
            double fCost = pNation.GetClaimingCost(Origin.LandType);

            foreach (var pLink in Origin.m_cBorder)
            {
                Land pOtherLand = pLink.Key as Land;
                if (!pOtherLand.HasOwner<LandMass>() && pOtherLand.LandType.m_eEnvironment.HasFlag(LandscapeGeneration.Environment.Habitable))
                    fCost += (double)pNation.GetClaimingCost(pOtherLand.LandType) / Origin.m_cBorder.Count;
            }

            if (fCost < 2)
                fCost = 2;
            return (int)(fCost / 2);
        }

        internal float GetResource(LandTypeInfoResources.Resource resource)
        {
            return Origin.LandType.GetLayer<LandTypeInfoResources>().m_cResources[resource] * Origin.Contents.Count;
        }
    }
}
