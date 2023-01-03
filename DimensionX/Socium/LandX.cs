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
    /// <summary>
    /// расширение LandscapeGeneration.Land
    /// добавляет ссылку на регион, к которому принадлежит земля, доминирующую нацию, имя, 
    /// а так же методы для строительства логова, поселения или форта
    /// </summary>
    public class LandX: Land<LocationX, LandTypeInfoX>
    {
        private Region m_pRegion = null;

        public Region Region
        {
            get { return m_pRegion; }
            set { m_pRegion = value; }
        }

        public string m_sName = "";

        /// <summary>
        /// доминирующая нация в локации, может отличаться от коренного населения региона
        /// </summary>
        public Nation m_pDominantNation;

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
                bool bMapBorder = false;
                foreach (LocationX pLink in pLoc.m_aBorderWith)
                {
                    if (pLink.Owner != pLoc.Owner)
                        bBorder = true;

                    if (pLink.m_bBorder)
                        bMapBorder = true;
                }

                int iChances = bBorder ? 1 : 50;

                if (pLoc.m_pSettlement != null || 
                    pLoc.m_pBuilding != null ||
                    pLoc.m_cRoads[RoadQuality.Country].Count > 0 ||
                    pLoc.m_cRoads[RoadQuality.Normal].Count > 0 ||
                    pLoc.m_cRoads[RoadQuality.Good].Count > 0 ||
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

            var pLair = m_cContents.ElementAt(iLair);

            pLair.m_pBuilding = new BuildingStandAlone(eSize);
            //m_cLocations[iLair].m_sName = NameGenerator.GetAbstractName();

            foreach (LocationX pLoc in pLair.m_aBorderWith)
            {
                if (pLoc.m_pBuilding == null)
                {
                    pLoc.m_pBuilding = new BuildingStandAlone(BuildingType.HuntingFields);
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
                bool bMapBorder = false;
                //определим, является ли эта локация пограничной с другой землёй или побережьем.
                foreach (LocationX pLink in pLoc.m_aBorderWith)
                {
                    if (pLink.Owner != pLoc.Owner)
                    {
                        bBorder = true;
                        if (pLink.Owner != null && (pLink.Owner as LandX).IsWater)
                            bCoast = true;
                    }

                    if (pLink.m_bBorder)
                    {
                        bMapBorder = true;
                    }
                }

                //сколько дорог проходит через этй локацию?
                int iRoadsCount = 0;
                foreach(var pRoads in pLoc.m_cRoads)
                    iRoadsCount += pRoads.Value.Count;

                //в пограничных локациях строим только если это побережье или есть дороги, во внутренних - отдаём предпочтение локациям с дорогами.
                int iChances = (bBorder ? ((bCoast || iRoadsCount > 0) ? 50 : 1) : (iRoadsCount > 0 ? 50 : 10));

                if (bMapBorder)
                    iChances = 0;

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
            var pTown = m_cContents.ElementAt(iTown);

            //Построим город в выбранной локации.
            //Все локации на 2 шага вокруг пометим как поля, чтобы там не возникало никаких новых поселений.
            pTown.m_pSettlement = new Settlement(pInfo, m_pDominantNation, Region.m_pProvince.m_pLocalSociety.m_iTechLevel, Region.m_pProvince.m_pLocalSociety.m_iMagicLimit, bCapital, bFast);
            //foreach (LocationX pLoc in m_cContents[iTown].m_aBorderWith)
            //    if (pLoc.m_pBuilding == null)
            //        pLoc.m_pBuilding = new BuildingStandAlone(BuildingType.Farm);

            //обновим все дороги, проходящие через новое поселение, чтобы на дорожных указателях появилось имя нового поселения.
            List<Road> cRoads = new List<Road>();
            cRoads.AddRange(pTown.m_cRoads[RoadQuality.Country]);
            cRoads.AddRange(pTown.m_cRoads[RoadQuality.Normal]);
            cRoads.AddRange(pTown.m_cRoads[RoadQuality.Good]);

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
            if (Region != null)
            {
                if (Region.m_pNatives == null)
                    return "unpopulated";
                else
                    return Region.m_pNatives.ToString();
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
            double fCost = pNation.GetClaimingCost(Type);

            foreach (var pLink in m_cBorder)
            {
                LandX pOtherLand = pLink.Key as LandX;
                if (pOtherLand.Owner == null && pOtherLand.Type.m_eEnvironment.HasFlag(LandscapeGeneration.Environment.Habitable))
                    fCost += (double)pNation.GetClaimingCost(pOtherLand.Type) / m_cBorder.Count;
            }

            if (fCost < 2)
                fCost = 2;
            return (int)(fCost / 2);
        }

        internal float GetResource(LandTypeInfoX.Resource resource)
        {
            return Type.m_cResources[resource] * m_cContents.Count;
        }
    }
}
