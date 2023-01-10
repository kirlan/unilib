﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;
using NameGen;
using LandscapeGeneration;
using LandscapeGeneration.PathFind;
using Socium.Languages;
using Socium.Settlements;
using Socium.Nations;
using Socium.Psychology;
using GeneLab.Genetix;
using Socium.Population;

namespace Socium
{
    /// <summary>
    /// Государство - группа сопредельных провинций (<see cref="Province"/>), объединённых общей властью.
    /// </summary>
    public class State: TerritoryCluster<State, ContinentX, Province>
    {
        public class Infrastructure
        {
            public RoadQuality m_eMaxGroundRoad;
            public RoadQuality m_eMaxNavalPath;
            public int m_iAerialAvailability;
            public List<SettlementSize> m_cAvailableSettlements;

            public Infrastructure(RoadQuality eMaxRoad, RoadQuality eMaxNaval, int iAerial, SettlementSize[] cSettlements)
            {
                m_eMaxGroundRoad = eMaxRoad;
                m_eMaxNavalPath = eMaxNaval;
                m_iAerialAvailability = iAerial;
                m_cAvailableSettlements = new List<SettlementSize>(cSettlements);
            }
        }

        public static readonly Infrastructure[] InfrastructureLevels = 
        {
            // 0 - только не соединённые дорогами поселения
            new Infrastructure(RoadQuality.None, RoadQuality.None, 0, new SettlementSize[]{SettlementSize.Hamlet}),
            // 1 - можно строить деревни, просёлочные дороги и короткие морские маршруты
            new Infrastructure(RoadQuality.Country, RoadQuality.Country, 0, new SettlementSize[]{SettlementSize.Hamlet, SettlementSize.Village}),
            // 2 - можно строить городки, обычные дороги и морские маршруты средней дальности
            new Infrastructure(RoadQuality.Normal, RoadQuality.Normal, 0, new SettlementSize[]{SettlementSize.Hamlet, SettlementSize.Village, SettlementSize.Town, SettlementSize.Fort}),
            // 3 - большие города, имперские дороги, неограниченные морские маршруты
            new Infrastructure(RoadQuality.Good, RoadQuality.Good, 0, new SettlementSize[]{SettlementSize.Hamlet, SettlementSize.Village, SettlementSize.Town, /*SettlementSize.City, */SettlementSize.Fort}),
            // 4 - большие города, имперские дороги, неограниченные морские маршруты, доступ к воздушному сообщению только в столице
            new Infrastructure(RoadQuality.Good, RoadQuality.Good, 1, new SettlementSize[]{SettlementSize.Hamlet, SettlementSize.Village, SettlementSize.Town, SettlementSize.City, SettlementSize.Fort}),
            // 5 - большие города, имперские дороги, неограниченные морские маршруты, доступ к воздушному сообщению только в столице и центрах провинций
            new Infrastructure(RoadQuality.Good, RoadQuality.Good, 2, new SettlementSize[]{SettlementSize.Village, SettlementSize.Town, SettlementSize.City, SettlementSize.Fort}),
            // 6 - большие города, имперские дороги, неограниченные морские маршруты, доступ к воздушному сообщению во всех поселениях
            new Infrastructure(RoadQuality.Good, RoadQuality.Good, 3, new SettlementSize[]{SettlementSize.Town, SettlementSize.City, SettlementSize.Fort}),
            // 7 - небольшие города, обычные дороги, морской транспорт не используется, доступ к воздушному сообщению во всех поселениях
            new Infrastructure(RoadQuality.Normal, RoadQuality.None, 3, new SettlementSize[]{SettlementSize.Hamlet, SettlementSize.Village, SettlementSize.Town, SettlementSize.Fort}),
            // 8 - деревни, просёлочные дороги, морской транспорт не используется, доступ к воздушному сообщению во всех поселениях
            new Infrastructure(RoadQuality.Country, RoadQuality.None, 3, new SettlementSize[]{SettlementSize.Hamlet, SettlementSize.Village, SettlementSize.Fort}),
        }; 
        
        /// <summary>
        /// Центральная провинция государства, в которой расположена столица
        /// </summary>
        public Province m_pMethropoly = null;

        public StateSociety m_pSociety = null;

        /// <summary>
        /// Зарождение государства в указанной провинции
        /// </summary>
        /// <param name="pSeed"></param>
        public override void Start(Province pSeed)
        {
            if (pSeed.HasOwner())
                throw new InvalidOperationException("This province already belongs to state!!!");

            BorderWith.Clear();
            Contents.Clear();

            InitBorder(pSeed);

            m_pMethropoly = pSeed;

            m_pSociety = new StateSociety(this);

            Contents.Add(pSeed);
            pSeed.SetOwner(this);
        }

        /// <summary>
        /// Присоеденять ничейные провинции вокруг, не учитывая уровень претензий конкурентов, до тех пор, пока рядом будут ничейные провинции.
        /// </summary>
        /// <returns></returns>
        public bool ForcedGrow()
        {
            Province[] aBorder = new List<Province>(Border.Keys).ToArray();

            bool bFullyGrown = true;

            foreach (Province pProvince in aBorder)
            {
                if (pProvince.Forbidden)
                    continue;

                if (!pProvince.HasOwner() && !pProvince.Center.IsWater && 
                    m_pMethropoly.LocalSociety.TitularNation.Race.Language == pProvince.LocalSociety.TitularNation.Race.Language)
                {
                    AddProvince(pProvince);
                    bFullyGrown = false;
                }
            }

            return !bFullyGrown;
        }

        /// <summary>
        /// Присоединяет к стране сопредельную ничейную провинцию.
        /// Чем большая часть периметра ничейной провинции является общей с государством - тем выше вероятность того, что выбрана будет именно она.
        /// Так же на вероятность влияют взаимоотношения населяющих провинции народов и общность языка.
        /// Возвращает false, если больше расти некуда, иначе true.
        /// </summary>
        /// <returns></returns>
        public override Province Grow(int iMaxSize)
        {
            //если государство уже достаточно большое - сваливаем.
            if (Contents.Count > iMaxSize)
                return null;

            Dictionary<Province, float> cChances = new Dictionary<Province, float>();

            foreach (Province pProvince in Border.Keys)
            {
                if (pProvince.Forbidden)
                    continue;

                if (!pProvince.HasOwner() && !pProvince.Center.IsWater)
                {
                    if (m_pMethropoly.LocalSociety.TitularNation.Race.Language != pProvince.LocalSociety.TitularNation.Race.Language)
                        continue;

                    //int iHostility = m_pMethropoly.CalcHostility(pProvince);

                    //враждебное отношение - такую провинция не присоединяем ни при каких условиях.
                    //if (iHostility > 2)
                    //    continue;

                    //bool bHaveRoad = false;
                    //foreach (var pLinkedProvince in pProvince.m_cConnectionString)
                    //    if (pLinkedProvince.Value == "ok")
                    //    {
                    //        bHaveRoad = true;
                    //        break;
                    //    }

                    //if(!bHaveRoad)
                    //    iHostility = m_pMethropoly.CalcHostility(pProvince);

                    float fSharedPerimeter = 0;
                    foreach (var pLine in Border[pProvince])
                        fSharedPerimeter += pLine.Length;

                    fSharedPerimeter /= pProvince.PerimeterLength;

                    if (fSharedPerimeter < 0.15f)
                        continue;

                    //дружественное отношение - для этой провинции шансы выше.
                    //if (iHostility < -1)
                    //    fSharedPerimeter *= -iHostility;
                    if (m_pMethropoly.LocalSociety.TitularNation == pProvince.LocalSociety.TitularNation)
                        fSharedPerimeter *= 2;

                    cChances[pProvince] = fSharedPerimeter;
                }
            }

            Province pAddon = null;

            int iChoice = Rnd.ChooseOne(cChances.Values, 2);

            if (iChoice >= 0)
            {
                pAddon = cChances.ElementAt(iChoice).Key;

                //if (!Rnd.OneChanceFrom(1 + m_iPower * m_iPower))
                //if (Rnd.OneChanceFrom(1 + pAddon.m_pCenter.Type.m_iMovementCost * pAddon.m_pCenter.Type.m_iMovementCost))
                //    return true;

                //foreach (LandTypeInfoX pType in m_pRace.m_cHatedLands)
                //    if (pType == pAddon.m_pCenter.Type && !Rnd.OneChanceFrom(5))
                //        return true;

                AddProvince(pAddon);
            }

            return pAddon;
        }

        private void AddProvince(Province pAddon)
        {
            Contents.Add(pAddon);
            pAddon.SetOwner(this);

            Border[pAddon].Clear();
            Border.Remove(pAddon);

            foreach (var pLink in pAddon.BorderWith)
            {
                Province pLinkedProvince = pLink.Key;

                if (!pLinkedProvince.Forbidden && Contents.Contains(pLinkedProvince))
                {
                    continue;
                }

                if (!Border.ContainsKey(pLinkedProvince))
                    Border[pLinkedProvince] = new List<VoronoiEdge>();
                VoronoiEdge[] cLines = pLink.Value.ToArray();
                foreach (var pLine in cLines)
                {
                    Border[pLinkedProvince].Add(new VoronoiEdge(pLine));
                }
            }

            //TestChain();
        }

        /// <summary>
        /// Заполняет словарь границ с другими странами и гарантирует принадлежность государства той расе, которая доминирует на его территории.
        /// </summary>
        public override void Finish(float fCycleShift)
        {
            ChainBorder(fCycleShift);

            BorderWith.Clear();

            //добавляем в общий список контуры границ с соседними государствами
            foreach (Province pProvince in Border.Keys)
            {
                State pState;
                if (pProvince.Forbidden || !pProvince.HasOwner())
                    pState = State.m_pForbidden;
                else
                    pState = pProvince.GetOwner();

                if (!BorderWith.ContainsKey(pState))
                    BorderWith[pState] = new List<VoronoiEdge>();
                BorderWith[pState].AddRange(Border[pProvince]);
            }

            //добавляем в общий список пустые массивы контуров границ для тех государств, с которыми у нас 
            //морское сообщение
            foreach (Province pProvince in Contents)
            {
                foreach (LocationX pLoc in pProvince.LocalSociety.Settlements)
                    foreach (LocationX pOtherLoc in pLoc.HaveSeaRouteTo)
                    { 
                        State pState = pOtherLoc.OwnerState;
                        if(pState != this && !BorderWith.ContainsKey(pState))
                            BorderWith[pState] = new List<VoronoiEdge>();
                    }
            }

            FillBorderWithKeys();
        }

        public int m_iFood = 0;
        public readonly Dictionary<LandResource, float> m_cResources = new Dictionary<LandResource, float>();
        public int m_iLocationsCount = 0;

        /// <summary>
        /// Строит столицу государства, рассчитывает уровни технического и культурного развития, определяет форму правления...
        /// </summary>
        /// <param name="iMinSize">предел количества провинций, ниже которого государство считается карликовым</param>
        /// <param name="iEmpireTreshold">предел количества провинций, выше которого государство считается гигантским</param>
        /// <param name="bFast">флаг быстрой (упрощённой) генерации</param>
        /// <returns>локация, в которой построена столица</returns>
        public LocationX BuildCapital(int iMinSize, int iEmpireTreshold, bool bFast)
        {
            m_pSociety.CalculateTitularNation();

            m_pMethropoly.LocalSociety.UpdateTitularNation(m_pSociety.TitularNation);
            foreach (Region pRegion in m_pMethropoly.Contents)
                foreach (LandX pLand in pRegion.Contents)
                    pLand.DominantNation = m_pSociety.TitularNation;

            // если у нас нация паразитов или пришельцев, то они сами живут только в метрополии, а во всей остальной империи коренное население - местное
            if (m_pSociety.TitularNation.DominantPhenotype.m_pValues.Get<NutritionGenetix>().IsParasite() || m_pSociety.TitularNation.IsInvader)
            {
                foreach (Province pProvince in Contents)
                {
                    foreach (Region pRegion in pProvince.Contents)
                    {
                        foreach (LandX pLand in pRegion.Contents)
                        {
                            if (pLand.DominantNation == m_pSociety.TitularNation)
                                pLand.DominantNation = m_pSociety.HostNation;
                        }
                    }
                }
            }
            //int iAverageMagicLimit = 0;

            foreach (LandResource eRes in Enum.GetValues(typeof(LandResource)))
                m_cResources[eRes] = 0;

            m_iFood = 0;
            m_iLocationsCount = 0;

            #region Counting resources production and consumption
            int iMethropolyLocationsCount = 0;
            int iProvincialLocationsCount = 0;

            foreach (Province pProvince in Contents)
            {
                foreach (LocationX pLoc in pProvince.LocalSociety.Settlements)
                {
                    foreach (LocationX pOtherLoc in pLoc.HaveSeaRouteTo)
                    {
                        State pState = pOtherLoc.OwnerState;
                        if (pState != this && !BorderWith.ContainsKey(pState))
                            BorderWith[pState] = new List<VoronoiEdge>();
                    }
                }

                foreach (LandResource eRes in Enum.GetValues(typeof(LandResource)))
                    m_cResources[eRes] += pProvince.Resources[eRes];

                if (pProvince == m_pMethropoly)
                    iMethropolyLocationsCount += pProvince.LocationsCount;
                else
                    iProvincialLocationsCount += pProvince.LocationsCount;

                //foreach (LandX pLand in pProvince.m_cContents)
                //{
                //    iAverageMagicLimit += pProvince.m_pNation.m_iMagicLimit * pLand.m_cContents.Count;
                //}
            }

            m_iLocationsCount = iMethropolyLocationsCount + iProvincialLocationsCount;

            //TODO: нужно учитывать размеры и телосложение - гиганты и толстяки едят больше, чем карлики и худышки
            float fMethropolySatiety = m_pSociety.TitularNation.GetAvailableFood(m_cResources, iProvincialLocationsCount) / iMethropolyLocationsCount;
            float fProvincialSatiety = m_pSociety.HostNation.GetAvailableFood(m_cResources, iProvincialLocationsCount) / iProvincialLocationsCount;

            if (fMethropolySatiety > 2)
                fMethropolySatiety = 2;

            if (fProvincialSatiety > 2)
                fProvincialSatiety = 2;

            if (m_pSociety.TitularNation.DominantPhenotype.m_pValues.Get<NutritionGenetix>().IsParasite())
            {
                // для паразитов качество их питания зависит от качества питания их жертв
                m_iFood = (int)(fMethropolySatiety * fProvincialSatiety * m_iLocationsCount);
            }
            else
            {
                // для обычных людей - качество питания в метрополии и в провинции просто дополняют друг-друга
                m_iFood = (int)((fMethropolySatiety + fProvincialSatiety) * 0.5 * m_iLocationsCount);
            }

            //iAverageMagicLimit = iAverageMagicLimit / m_iPopulation;
            #endregion Counting resources production and consumption

            m_pSociety.CalculateSocietyFeatures(iEmpireTreshold);

            #region Build capital and regional centers
            m_pMethropoly.AdministrativeCenter.Settlement = new Settlement(m_pSociety.Polity.StateCapital, 
                m_pMethropoly.LocalSociety.TitularNation, 
                m_pSociety.TechLevel, 
                m_pSociety.MagicLimit, 
                true, 
                bFast);

            foreach (Province pProvince in Contents)
            {
                m_pSociety.Settlements.AddRange(pProvince.LocalSociety.Settlements);

                if (pProvince == m_pMethropoly)
                    continue;

                pProvince.AdministrativeCenter.Settlement = new Settlement(m_pSociety.Polity.ProvinceCapital, 
                    m_pMethropoly.LocalSociety.TitularNation, 
                    m_pSociety.TechLevel, 
                    m_pSociety.MagicLimit, 
                    false, 
                    bFast);
            }
            #endregion Build capital and regional centers

            if (m_pMethropoly.Center != null)
                m_pSociety.Name = m_pSociety.TitularNation.Race.Language.RandomCountryName();

            return m_pMethropoly.AdministrativeCenter;
        }

        public State[] GetEnemiesList()
        {
            List<State> cList = new List<State>();
 
            foreach (State pState in BorderWithKeys)
            {
                if (pState.Forbidden)
                    continue;

                int iHostility = m_pSociety.CalcHostility(pState);
                int iHostility2 = pState.m_pSociety.CalcHostility(this);

                if (iHostility2 > iHostility)
                    iHostility = iHostility2;

                if (iHostility > 0)
                {
                    cList.Add(pState);
                }
            }

            return cList.ToArray();
        }

        public State[] GetAlliesList()
        {
            List<State> cList = new List<State>();

            foreach (State pState in BorderWithKeys)
            {
                if (pState.Forbidden)
                    continue;

                int iHostility = m_pSociety.CalcHostility(pState);
                int iHostility2 = pState.m_pSociety.CalcHostility(this);

                if (iHostility2 > iHostility)
                    iHostility = iHostility2;

                if (iHostility <= 0)
                {
                    cList.Add(pState);
                }
            }

            return cList.ToArray();
        }

        public void BuildForts(Dictionary<State, Dictionary<State, int>> cHostility, bool bFast)
        {
            if (!InfrastructureLevels[m_pSociety.InfrastructureLevel].m_cAvailableSettlements.Contains(SettlementSize.Fort))
                return;

            foreach (Province pProvince in Contents)
                foreach (Region pRegion in pProvince.Contents)
                    {
                        float fTreat = 0;
                        float fBorder = 0;

                        int iMaxHostility = 0;
                        State pMainEnemy = null;

                        foreach (var pLink in pRegion.BorderWith)
                        {
                            Region pLinkedRegion = pLink.Key;

                            if (pLinkedRegion.Forbidden)
                                continue;

                            if (pLinkedRegion.HasOwner() && pLinkedRegion.GetOwner().GetOwner() == this)
                                continue;

                            int iHostility = 0;
                            if (pLinkedRegion.HasOwner())
                            {
                                State pLinkedState = pLinkedRegion.GetOwner().GetOwner();

                                Dictionary<State, int> cLinkedStateHostility;
                                if (!cHostility.TryGetValue(pLinkedState, out cLinkedStateHostility))
                                {
                                    cLinkedStateHostility = new Dictionary<State, int>();
                                    cHostility[pLinkedState] = cLinkedStateHostility;
                                }

                                if (!cLinkedStateHostility.TryGetValue(this, out iHostility))
                                {
                                    iHostility = pLinkedState.m_pSociety.CalcHostility(this);
                                    cLinkedStateHostility[this] = iHostility;
                                }

                                if (iHostility <= 0)
                                    continue;

                                if (iHostility > iMaxHostility)
                                {
                                    iMaxHostility = iHostility;
                                    pMainEnemy = pLinkedState;
                                }
                            }

                            VoronoiEdge[] cLines = pLink.Value.ToArray();
                            foreach (var pLine in cLines)
                            {
                                fBorder += pLine.Length / pLinkedRegion.MovementCost;
                                if (!pLinkedRegion.HasOwner())
                                    fTreat += pLine.Length / pLinkedRegion.MovementCost;
                                else
                                    fTreat += pLine.Length * (float)Math.Sqrt(iHostility) / pLinkedRegion.MovementCost;
                            }
                        }

                        if (fTreat == 0)
                            continue;

                        if (Rnd.ChooseOne(fTreat, fBorder))// - fTreat))
                            //if (m_iSize > 1 || Rnd.OneChanceFrom(2))
                            {
                                Location pFort = pRegion.BuildFort(pMainEnemy, bFast);
                                if (pFort != null)
                                {
                                    pRegion.GetOwner().LocalSociety.Settlements.Add(pFort.As<LocationX>());
                                    //bHaveOne = true;
                                }
                            }
                        }
                    }

        public override string ToString()
        {
            return m_pSociety.ToString();
        }

        public override float GetMovementCost()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Достраиваем необходимые дороги между центрами провинций, так же вкючаем в дорожную сеть форты
        /// </summary>
        /// <param name="fCycleShift"></param>
        internal void FixRoads(float fCycleShift)
        {
            RoadQuality eRoadLevel = State.InfrastructureLevels[m_pSociety.InfrastructureLevel].m_eMaxGroundRoad;

            if (eRoadLevel == RoadQuality.None)
                return;

            //Закрываем границы, чтобы нельзя было "срезать" путь по чужой территории
            foreach (Province pProvince in Contents)
                foreach (Region pRegion in pProvince.Contents)
                    foreach (LandX pLand in pRegion.Contents)
                        foreach (Location pLoc in pLand.As<Land>().Contents)
                            foreach (var pLinked in pLoc.Links)
                            {
                                if (pLinked.Key is Location)
                                {
                                    LandX pLinkedLandX = (pLinked.Key as Location).GetOwner().As<LandX>();
                                    if (!pLinkedLandX.HasOwner() || !pLinkedLandX.GetOwner().HasOwner() || pLinkedLandX.GetOwner().GetOwner().GetOwner() != this || pLinked.Value.Sea)
                                        pLoc.Links[pLinked.Key].IsClosed = true;
                                }
                                else
                                    pLoc.Links[pLinked.Key].IsClosed = true;
                            }

            //Сначала соеденим все центры провинций
            List<LocationX> cConnected = new List<LocationX>();
            cConnected.Add(m_pMethropoly.AdministrativeCenter);

            while (cConnected.Count < Contents.Count)
            {
                LocationX pBestTown1 = null;
                LocationX pBestTown2 = null;
                float fMinLength = float.MaxValue;

                foreach (Province pProvince in Contents)
                {
                    LocationX pTown = pProvince.AdministrativeCenter;

                    if (cConnected.Contains(pTown))
                        continue;

                    foreach (LocationX pOtherTown in cConnected)
                    {
                        float fDist = pTown.DistanceTo(pOtherTown, fCycleShift);

                        if (fDist < fMinLength &&
                            (fMinLength == float.MaxValue ||
                             Rnd.OneChanceFrom(2)))
                        {
                            fMinLength = fDist;

                            pBestTown1 = pTown;
                            pBestTown2 = pOtherTown;
                        }
                    }
                }
                if (pBestTown2 != null)
                {
                    World.BuildRoad(pBestTown1, pBestTown2, eRoadLevel, fCycleShift);

                    fMinLength = float.MaxValue;
                    LocationX pBestTown3 = null;
                    foreach (LocationX pOtherTown in cConnected)
                    {
                        float fDist = pBestTown1.DistanceTo(pOtherTown, fCycleShift);

                        if (pOtherTown != pBestTown2 &&
                            fDist < fMinLength &&
                            (fMinLength == float.MaxValue ||
                             Rnd.OneChanceFrom(2)))
                        {
                            fMinLength = fDist;

                            pBestTown3 = pOtherTown;
                        }
                    }

                    if (pBestTown3 != null)
                        World.BuildRoad(pBestTown1, pBestTown3, eRoadLevel, fCycleShift);

                    cConnected.Add(pBestTown1);
                }
            }

            //теперь займёмся фортами
            cConnected.Clear();
            cConnected.Add(m_pMethropoly.AdministrativeCenter);

            List<LocationX> cSettlements = new List<LocationX>();
            foreach (Province pProvince in Contents)
                cSettlements.AddRange(pProvince.LocalSociety.Settlements);
            LocationX[] aSettlements = cSettlements.ToArray();

            eRoadLevel = RoadQuality.Normal;
            if (eRoadLevel > State.InfrastructureLevels[m_pSociety.InfrastructureLevel].m_eMaxGroundRoad)
                eRoadLevel = State.InfrastructureLevels[m_pSociety.InfrastructureLevel].m_eMaxGroundRoad;

            List<LocationX> cForts = new List<LocationX>();
            foreach (LocationX pTown in aSettlements)
            {
                if (!cConnected.Contains(pTown) && (pTown.Roads[RoadQuality.Normal].Count > 0 || pTown.Roads[RoadQuality.Good].Count > 0))
                    cConnected.Add(pTown);
                else
                    if (pTown.Settlement.Profile.Size == SettlementSize.Fort)
                        cForts.Add(pTown);
            }
            LocationX[] aForts = cForts.ToArray();

            foreach (LocationX pFort in aForts)
            {
                LocationX pBestTown = null;
                float fMinLength = float.MaxValue;

                foreach (LocationX pOtherTown in cConnected)
                {
                    float fDist = pFort.DistanceTo(pOtherTown, fCycleShift);

                    if (fDist < fMinLength &&
                        (fMinLength == float.MaxValue ||
                            Rnd.OneChanceFrom(2)))
                    {
                        fMinLength = fDist;
                        pBestTown = pOtherTown;
                    }
                }
                if (pBestTown != null)
                {
                    World.BuildRoad(pFort, pBestTown, eRoadLevel, fCycleShift);

                    fMinLength = float.MaxValue;
                    LocationX pBestTown2 = null;
                    foreach (LocationX pOtherTown in cConnected)
                    {
                        float fDist = pFort.DistanceTo(pOtherTown, fCycleShift);

                        if (pOtherTown != pBestTown &&
                            fDist < fMinLength &&
                            (fMinLength == float.MaxValue ||
                             Rnd.OneChanceFrom(2)))
                        {
                            fMinLength = fDist;
                            pBestTown2 = pOtherTown;
                        }
                    }

                    if (pBestTown2 != null)
                        World.BuildRoad(pFort, pBestTown2, eRoadLevel, fCycleShift);

                    cConnected.Add(pFort);
                }
            }

            //открываем закрытые в начале функции границы
            foreach (Province pProvince in Contents)
                foreach (Region pRegion in pProvince.Contents)
                    foreach (LandX pLand in pRegion.Contents)
                        foreach (Location pLoc in pLand.As<Land>().Contents)
                            foreach (TransportationNode pLink in pLoc.Links.Keys)
                                pLoc.Links[pLink].IsClosed = false;
        }

        public void SpecializeSettlements()
        {
            foreach (Province pProvince in Contents)
            {
                foreach (LocationX pLoc in pProvince.LocalSociety.Settlements)
                {
                    if (pLoc.Settlement.Speciality != SettlementSpeciality.None)
                        continue;

                    bool bCoast = false;
                    foreach (Location pLink in pLoc.As<Location>().BorderWithKeys)
                    {
                        if (pLink.HasOwner() && pLink.GetOwner().IsWater)
                            bCoast = true;
                    }

                    LandX pLandX = pLoc.As<Location>().GetOwner().As<LandX>();

                    switch (pLoc.Settlement.Profile.Size)
                    {
                        #region case SettlementSize.Hamlet
                        case SettlementSize.Hamlet:
                            if (bCoast)
                            {
                                pLoc.Settlement.Speciality = SettlementSpeciality.Fishers;
                            }
                            else
                            {
                                List<float> cResources = new List<float>();
                                cResources.Add(pLandX.GetResource(LandResource.Grain));
                                cResources.Add(pLandX.GetResource(LandResource.Game));

                                //в развитом обществе охота - это уже не способ добычи пищи, а больше развлечение
                                if (m_pSociety.InfrastructureLevel > 2)
                                {
                                    cResources[0] += cResources[1];
                                    cResources[1] = 0;
                                }

                                if (m_pSociety.InfrastructureLevel > 1)
                                {
                                    cResources.Add(pLandX.GetResource(LandResource.Ore));
                                    cResources.Add(pLandX.GetResource(LandResource.Wood));
                                }

                                int iChoosen = Rnd.ChooseOne(cResources, 2);

                                switch (iChoosen)
                                {
                                    case 0:
                                        pLoc.Settlement.Speciality = m_pSociety.InfrastructureLevel >= 4 ? SettlementSpeciality.Farmers : SettlementSpeciality.Peasants;
                                        break;
                                    case 1:
                                        pLoc.Settlement.Speciality = SettlementSpeciality.Hunters;
                                        break;
                                    case 2:
                                        pLoc.Settlement.Speciality = SettlementSpeciality.Miners;
                                        break;
                                    case 3:
                                        pLoc.Settlement.Speciality = SettlementSpeciality.Lumberjacks;
                                        break;
                                }
                            }
                            break;
                        #endregion
                        #region case SettlementSize.Village
                        case SettlementSize.Village:
                            if (bCoast)
                            {
                                pLoc.Settlement.Speciality = SettlementSpeciality.Fishers;
                            }
                            else
                            {
                                List<float> cResources = new List<float>();
                                cResources.Add(pLandX.GetResource(LandResource.Grain));
                                cResources.Add(pLandX.GetResource(LandResource.Game));
                                cResources.Add(pLandX.GetResource(LandResource.Ore));
                                cResources.Add(pLandX.GetResource(LandResource.Wood));

                                //в развитом обществе охота - это уже не способ добычи пищи, а больше развлечение
                                if (m_pSociety.InfrastructureLevel > 2)
                                {
                                    cResources[0] += cResources[1];
                                    cResources[1] = 0;
                                }

                                int iChoosen = Rnd.ChooseOne(cResources, 2);

                                switch (iChoosen)
                                {
                                    case 0:
                                        pLoc.Settlement.Speciality = m_pSociety.InfrastructureLevel >= 4 ? SettlementSpeciality.Farmers : SettlementSpeciality.Peasants;
                                        break;
                                    case 1:
                                        pLoc.Settlement.Speciality = SettlementSpeciality.Hunters;
                                        break;
                                    case 2:
                                        pLoc.Settlement.Speciality = SettlementSpeciality.Miners;
                                        break;
                                    case 3:
                                        pLoc.Settlement.Speciality = SettlementSpeciality.Lumberjacks;
                                        break;
                                }
                            }
                            break;
                        #endregion
                        #region case SettlementSize.Town
                        case SettlementSize.Town:
                            if (bCoast && !Rnd.OneChanceFrom(3))
                            {
                                if (Rnd.OneChanceFrom(2))
                                    pLoc.Settlement.Speciality = SettlementSpeciality.Naval;
                                else
                                    pLoc.Settlement.Speciality = SettlementSpeciality.Fishers;
                            }
                            else
                            {
                                List<float> cResources = new List<float>();
                                //в толерантном обществе специализация городов выбирается исходя из общего состояния ресурсов в государстве,
                                //а в эгоистичном обществе - в каждой провинции свои приоритеты
                                if (m_pSociety.DominantCulture.GetTrait(Trait.Fanaticism) < 1 + Rnd.Get(1f))
                                {
                                    cResources.Add(m_iFood);
                                    cResources.Add(m_cResources[LandResource.Ore]);
                                    cResources.Add(m_cResources[LandResource.Wood]);
                                }
                                else
                                {
                                    cResources.Add(pProvince.Resources[LandResource.Game] + pProvince.Resources[LandResource.Grain]);
                                    cResources.Add(pProvince.Resources[LandResource.Ore]);
                                    cResources.Add(pProvince.Resources[LandResource.Wood]);
                                }

                                int iChoosen = Rnd.ChooseOne(cResources, 2);

                                switch (iChoosen)
                                {
                                    case 0:
                                        pLoc.Settlement.Speciality = SettlementSpeciality.Tailors;
                                        break;
                                    case 1:
                                        pLoc.Settlement.Speciality = Rnd.OneChanceFrom(3) ? SettlementSpeciality.Jevellers : SettlementSpeciality.Factory;
                                        break;
                                    case 2:
                                        pLoc.Settlement.Speciality = SettlementSpeciality.Artisans;
                                        break;
                                }
                            }
                            break;
                        #endregion
                        #region case SettlementSize.City
                        case SettlementSize.City:
                            if (bCoast && Rnd.OneChanceFrom(2))
                                pLoc.Settlement.Speciality = Rnd.OneChanceFrom(3) ? SettlementSpeciality.NavalAcademy : SettlementSpeciality.Naval;
                            else
                            {
                                if (bCoast && m_pSociety.InfrastructureLevel > 2 && m_pSociety.DominantCulture.GetTrait(Trait.Simplicity) > 1 + Rnd.Get(1f))
                                    pLoc.Settlement.Speciality = SettlementSpeciality.Resort;
                                else
                                {
                                    if (Rnd.OneChanceFrom(2))
                                    {
                                        List<float> cResources = new List<float>();
                                        cResources.Add(2 - m_pSociety.DominantCulture.GetTrait(Trait.Simplicity));
                                        cResources.Add(m_pSociety.DominantCulture.GetTrait(Trait.Piety));
                                        cResources.Add(m_pSociety.DominantCulture.GetTrait(Trait.Agression));
                                        cResources.Add(m_pSociety.DominantCulture.GetTrait(Trait.Treachery));

                                        float fScience = 0.05f;
                                        if (m_pSociety.DominantCulture.Customs.Has(Customs.MindSet.Balanced_mind))
                                            fScience = 0.25f;
                                        else if (m_pSociety.DominantCulture.Customs.Has(Customs.MindSet.Logic))
                                            fScience = 0.5f;

                                        if (m_pSociety.TitularNation.DominantPhenotype.m_pValues.Get<BrainGenetix>().Intelligence == Intelligence.Sapient)
                                            fScience *= 2;
                                        if (m_pSociety.TitularNation.DominantPhenotype.m_pValues.Get<BrainGenetix>().Intelligence == Intelligence.Ingenious)
                                            fScience *= 4;

                                        cResources.Add(fScience);
                                        //cResources.Add(2 - m_pCulture.MentalityValues[Mentality.Selfishness][m_iInfrastructureLevel]);

                                        int iChoosen = Rnd.ChooseOne(cResources, 2);

                                        switch (iChoosen)
                                        {
                                            case 0:
                                                pLoc.Settlement.Speciality = Rnd.OneChanceFrom(3) ? SettlementSpeciality.Cultural : SettlementSpeciality.ArtsAcademy;
                                                break;
                                            case 1:
                                                pLoc.Settlement.Speciality = SettlementSpeciality.Religious;
                                                break;
                                            case 2:
                                                pLoc.Settlement.Speciality = SettlementSpeciality.MilitaryAcademy;
                                                break;
                                            case 3:
                                                pLoc.Settlement.Speciality = SettlementSpeciality.Gambling;
                                                break;
                                            case 4:
                                                pLoc.Settlement.Speciality = SettlementSpeciality.SciencesAcademy;
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        List<float> cResources = new List<float>();
                                        //в толерантном обществе специализация городов выбирается исходя из общего состояния ресурсов в государстве,
                                        //а в эгоистичном обществе - в каждой провинции свои приоритеты
                                        if (m_pSociety.DominantCulture.GetTrait(Trait.Fanaticism) < 1 + Rnd.Get(1f))
                                        {
                                            cResources.Add(m_iFood);
                                            cResources.Add(m_cResources[LandResource.Ore]);
                                            cResources.Add(m_cResources[LandResource.Wood]);
                                        }
                                        else
                                        {
                                            cResources.Add(pProvince.Resources[LandResource.Game] + pProvince.Resources[LandResource.Grain]);
                                            cResources.Add(pProvince.Resources[LandResource.Ore]);
                                            cResources.Add(pProvince.Resources[LandResource.Wood]);
                                        }

                                        int iChoosen = Rnd.ChooseOne(cResources, 2);

                                        switch (iChoosen)
                                        {
                                            case 0:
                                                pLoc.Settlement.Speciality = SettlementSpeciality.Tailors;
                                                break;
                                            case 1:
                                                pLoc.Settlement.Speciality = Rnd.OneChanceFrom(3) ? SettlementSpeciality.Jevellers : SettlementSpeciality.Factory;
                                                break;
                                            case 2:
                                                pLoc.Settlement.Speciality = SettlementSpeciality.Artisans;
                                                break;
                                        }
                                    }
                                }
                            }
                            break;
                        #endregion
                        #region case SettlementSize.Capital
                        case SettlementSize.Capital:
                            if (bCoast && Rnd.OneChanceFrom(2))
                                pLoc.Settlement.Speciality = Rnd.OneChanceFrom(3) ? SettlementSpeciality.NavalAcademy : SettlementSpeciality.Naval;
                            else
                            {
                                if (bCoast && m_pSociety.InfrastructureLevel > 2 && m_pSociety.DominantCulture.GetTrait(Trait.Simplicity) > 1 + Rnd.Get(1f))
                                    pLoc.Settlement.Speciality = SettlementSpeciality.Resort;
                                else
                                {
                                    if (Rnd.OneChanceFrom(2))
                                    {
                                        List<float> cResources = new List<float>();
                                        cResources.Add(2 - m_pSociety.DominantCulture.GetTrait(Trait.Simplicity));
                                        cResources.Add(m_pSociety.DominantCulture.GetTrait(Trait.Piety));
                                        cResources.Add(m_pSociety.DominantCulture.GetTrait(Trait.Agression));
                                        cResources.Add(m_pSociety.DominantCulture.GetTrait(Trait.Treachery));

                                        float fScience = 0.05f;
                                        if (m_pSociety.DominantCulture.Customs.Has(Customs.MindSet.Balanced_mind))
                                            fScience = 0.25f;
                                        else if (m_pSociety.DominantCulture.Customs.Has(Customs.MindSet.Logic))
                                            fScience = 0.5f;

                                        if (m_pSociety.TitularNation.DominantPhenotype.m_pValues.Get<BrainGenetix>().Intelligence == Intelligence.Sapient)
                                            fScience *= 2;
                                        if (m_pSociety.TitularNation.DominantPhenotype.m_pValues.Get<BrainGenetix>().Intelligence == Intelligence.Ingenious)
                                            fScience *= 4;

                                        cResources.Add(fScience);
                                        //cResources.Add(2 - m_pCulture.MentalityValues[Mentality.Selfishness][m_iInfrastructureLevel]);

                                        int iChoosen = Rnd.ChooseOne(cResources, 2);

                                        switch (iChoosen)
                                        {
                                            case 0:
                                                pLoc.Settlement.Speciality = Rnd.OneChanceFrom(3) ? SettlementSpeciality.Cultural : SettlementSpeciality.ArtsAcademy;
                                                break;
                                            case 1:
                                                pLoc.Settlement.Speciality = SettlementSpeciality.Religious;
                                                break;
                                            case 2:
                                                pLoc.Settlement.Speciality = SettlementSpeciality.MilitaryAcademy;
                                                break;
                                            case 3:
                                                pLoc.Settlement.Speciality = SettlementSpeciality.Gambling;
                                                break;
                                            case 4:
                                                pLoc.Settlement.Speciality = SettlementSpeciality.SciencesAcademy;
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        List<float> cResources = new List<float>();
                                        //в толерантном обществе специализация городов выбирается исходя из общего состояния ресурсов в государстве,
                                        //а в эгоистичном обществе - в каждой провинции свои приоритеты
                                        if (m_pSociety.DominantCulture.GetTrait(Trait.Fanaticism) < 1 + Rnd.Get(1f))
                                        {
                                            cResources.Add(m_iFood);
                                            cResources.Add(m_cResources[LandResource.Ore]);
                                            cResources.Add(m_cResources[LandResource.Wood]);
                                        }
                                        else
                                        {
                                            cResources.Add(pProvince.Resources[LandResource.Game] + pProvince.Resources[LandResource.Grain]);
                                            cResources.Add(pProvince.Resources[LandResource.Ore]);
                                            cResources.Add(pProvince.Resources[LandResource.Wood]);
                                        }

                                        int iChoosen = Rnd.ChooseOne(cResources, 2);

                                        switch (iChoosen)
                                        {
                                            case 0:
                                                pLoc.Settlement.Speciality = SettlementSpeciality.Tailors;
                                                break;
                                            case 1:
                                                pLoc.Settlement.Speciality = Rnd.OneChanceFrom(3) ? SettlementSpeciality.Jevellers : SettlementSpeciality.Factory;
                                                break;
                                            case 2:
                                                pLoc.Settlement.Speciality = SettlementSpeciality.Artisans;
                                                break;
                                        }
                                    }
                                }
                            }
                            break;
                        #endregion
                        #region case SettlementSize.Fort
                        case SettlementSize.Fort:
                            if (bCoast)
                            {
                                if (m_pSociety.DominantCulture.GetTrait(Trait.Agression) > 1.5 &&
                                    m_pSociety.DominantCulture.GetTrait(Trait.Treachery) > 1.5)
                                    pLoc.Settlement.Speciality = SettlementSpeciality.Pirates;
                                else
                                    pLoc.Settlement.Speciality = SettlementSpeciality.Naval;
                            }
                            else
                            {
                                if (m_pSociety.DominantCulture.GetTrait(Trait.Agression) > 1.5 &&
                                    m_pSociety.DominantCulture.GetTrait(Trait.Treachery) > 1.5)
                                    pLoc.Settlement.Speciality = SettlementSpeciality.Raiders;
                                else
                                    pLoc.Settlement.Speciality = SettlementSpeciality.Military;
                            }
                            break;
                        #endregion
                    }
                }
            }
        }
    }
}
