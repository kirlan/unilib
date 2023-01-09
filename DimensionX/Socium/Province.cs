using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LandscapeGeneration;
using Random;
using NameGen;
using LandscapeGeneration.PathFind;
using Socium.Settlements;
using Socium.Nations;
using Socium.Psychology;
using GeneLab.Genetix;
using Socium.Population;

namespace Socium
{
    public enum LandResource
    {
        Fish,
        Grain,
        Game,
        Wood,
        Ore
    }

    /// <summary>
    /// Провинция - группа сопредельных регионов (<see cref="Region"/>), имеющих общую инфраструктуру и социум, принадлежащих одному государству.
    /// Провинции объединяются в государства (<see cref="State"/>).
    /// </summary>
    public class Province : TerritoryCluster<Province, State, Region>
    {
        /// <summary>
        /// ТОЛКО ДЛЯ ОТЛАДКИ!!!!
        /// </summary>
        public Dictionary<Province, string> m_cConnectionString = new Dictionary<Province,string>();
        
        public bool IsBorder()
        {
            foreach (var pRegion in Contents)
                if (pRegion.IsBorder())
                    return true;

            return false;
        }

        public Region m_pCenter;

        public LocationX m_pAdministrativeCenter = null;

        public NationalSociety m_pLocalSociety = null;

        public readonly Dictionary<LandResource, float> m_cResources = new Dictionary<LandResource, float>();

        public int m_iPopulation = 0;

        /// <summary>
        /// Зарождение провинции в указанном регионе
        /// </summary>
        /// <param name="pSeed"></param>
        public override void Start(Region pSeed)
        {
            if (pSeed.HasOwner())
                throw new InvalidOperationException("That land already belongs to province!!!");

            BorderWith.Clear();
            Contents.Clear();

            InitBorder(pSeed);

            m_pCenter = pSeed;

            m_pLocalSociety = new NationalSociety(pSeed.m_pNatives);

            Contents.Add(pSeed);
            pSeed.SetOwner(this);
            pSeed.m_iProvincePresence = 1;

            m_cGrowCosts.Clear();

            m_bFullyGrown = false;
        }

        /// <summary>
        /// Стоимость присоединения указанной земли к провинции
        /// </summary>
        /// <param name="pRegion"></param>
        /// <returns></returns>
        private int GetGrowCost(Region pRegion)
        {
            if (pRegion.IsWater)
                return -1;

            float fCost = pRegion.m_pType.MovementCost;

            //if (pLand.m_pProvince != this)
            //{
            //    //граница провинции с этой землёй
            //    float fProvinceBorderLength = 0;
            //    foreach (Line pLine in m_cBorder[pLand])
            //        fProvinceBorderLength += pLine.m_fLength;

            //    //граница этой земли с окружающими землями
            //    float fLinkedLandsBorderLength = 0;
            //    foreach (var pLinkTerr in pLand.BorderWith)
            //    {
            //        if ((pLinkTerr.Key as Territory).Forbidden)
            //            continue;

            //        Line[] cLines = pLinkTerr.Value.ToArray();
            //        foreach (Line pLine in cLines)
            //            fLinkedLandsBorderLength += pLine.m_fLength;
            //    }

            //    fCost *= fLinkedLandsBorderLength / fProvinceBorderLength;

            //    //если общая граница провинции с землёй меньше общей длины границы земли в 4 раза или больше, то это - очень плохой вариант.
            //    //стоимость расширения в эту землю - выше.
            //    if (fCost > 4)
            //        fCost *= 10;
            //    //если общая граница провинции с землёй меньше общей длины границы земли в 2 раза или меньше, то это - очень хороший вариант.
            //    //стоимость расширения в эту землю - ниже.
            //    if (fCost < 2)
            //        fCost /= 10;
            //}



            foreach (LandTypeInfo pType in m_pLocalSociety.TitularNation.PreferredLands)
                if (pType == pRegion.m_pType)
                    fCost /= 10;

            foreach (LandTypeInfo pType in m_pLocalSociety.TitularNation.HatedLands)
                if (pType == pRegion.m_pType)
                    fCost *= 10;

            if (pRegion.m_pNatives != m_pLocalSociety.TitularNation)
            {
                if (m_pLocalSociety.TitularNation.IsAncient)
                    fCost *= 9999999;
                else
                    fCost *= 99999;
            }

            if (m_pLocalSociety.TitularNation.IsHegemon)
                fCost /= 2;

            if (fCost < 1)
                fCost = 1;

            if (fCost > int.MaxValue)
                fCost = int.MaxValue - 1;

            return (int)fCost;
        }

        private Dictionary<Region, int> m_cGrowCosts = new Dictionary<Region, int>();

        /// <summary>
        /// Усилить претензии провинции на сопредельный ничейный регион
        /// </summary>
        /// <param name="pRegion"></param>
        /// <param name="iValue"></param>
        /// <returns></returns>
        private Region GrowPresence(Region pRegion, int iValue)
        {
            pRegion.m_iProvincePresence += iValue;

            int iOwnCost = 0;
            if (!m_cGrowCosts.TryGetValue(pRegion, out iOwnCost))
            {
                iOwnCost = GetGrowCost(pRegion);
                m_cGrowCosts[pRegion] = iOwnCost;
            }

            if (pRegion.m_iProvincePresence > iOwnCost)
            {
                float fBestValue = float.MaxValue;
                int iBestCost = 0;
                Region pBestRegion = null;

                foreach (Region pLinkedRegion in pRegion.BorderWithKeys)
                {
                    if (pLinkedRegion.Forbidden)
                        continue;

                    if (pLinkedRegion.IsWater || (pLinkedRegion.HasOwner() && pLinkedRegion.GetOwner() != this))
                        continue;

                    int iCost = 0;
                    if (!m_cGrowCosts.TryGetValue(pLinkedRegion, out iCost))
                    {
                        iCost = GetGrowCost(pLinkedRegion);
                        m_cGrowCosts[pLinkedRegion] = iCost;
                    }

                    if (iCost == -1)
                        continue;

                    float fCostModified = iCost;

                    if (!pLinkedRegion.HasOwner())
                    {
                        //общая граница провинции и нового региона
                        float fSharedPerimeter = 1;
                        VoronoiEdge[] aBorderLine = Border[pLinkedRegion].ToArray();
                        foreach (var pLine in aBorderLine)
                            fSharedPerimeter += pLine.Length;

                        fCostModified = iCost * pLinkedRegion.PerimeterLength / fSharedPerimeter;

                        if (fSharedPerimeter < pLinkedRegion.PerimeterLength / 4)
                            fCostModified *= 10;
                        if (fSharedPerimeter > pLinkedRegion.PerimeterLength / 2)
                            fCostModified /= 10;

                        fCostModified = Math.Max(1, fCostModified);
                    }

                    if (pLinkedRegion.m_iProvincePresence + fCostModified < fBestValue ||
                        (pLinkedRegion.m_iProvincePresence + fCostModified == fBestValue &&
                         Rnd.OneChanceFrom(pRegion.BorderWith.Count)))
                    {
                        fBestValue = pLinkedRegion.m_iProvincePresence + fCostModified;
                        iBestCost = iCost;
                        pBestRegion = pLinkedRegion;
                    }
                }

                if (pBestRegion != null && pBestRegion.m_iProvincePresence + iBestCost < pRegion.m_iProvincePresence - iBestCost)
                {
                    pRegion.m_iProvincePresence -= iBestCost;
                    if (!pBestRegion.HasOwner())
                    {
                        if(pBestRegion.m_pNatives != pRegion.m_pNatives)
                            GetGrowCost(pBestRegion);
                        pBestRegion.m_iProvincePresence += iBestCost;
                        return pBestRegion;
                    }
                    else
                    {
                        return GrowPresence(pBestRegion, iBestCost);
                    }
                }
                else
                    return null;
            }
            else
                return null;
        }

        /// <summary>
        /// Присоеденять ничейные земли вокруг, не учитывая уровень претензий конкурентов, до тех пор, пока рядом будут ничейные земли.
        /// </summary>
        /// <returns></returns>
        public bool ForcedGrow()
        {
            Region[] aBorder = new List<Region>(Border.Keys).ToArray();

            m_bFullyGrown = true;

            if (m_pLocalSociety.TitularNation.IsAncient)
                return !m_bFullyGrown;

            foreach (Region pRegion in aBorder)
            {
                if (pRegion.Forbidden)
                    continue;

                if (pRegion.m_pNatives != m_pLocalSociety.TitularNation)
                    continue;

                if (!pRegion.HasOwner() && !pRegion.IsWater)
                {
                    Contents.Add(pRegion);
                    pRegion.SetOwner(this);

                    Border[pRegion].Clear();
                    Border.Remove(pRegion);

                    foreach (var pAddonLinkedRegion in pRegion.BorderWith)
                    {
                        if (!pAddonLinkedRegion.Key.Forbidden && Contents.Contains(pAddonLinkedRegion.Key))
                            continue;

                        if (!Border.ContainsKey(pAddonLinkedRegion.Key))
                            Border[pAddonLinkedRegion.Key] = new List<VoronoiEdge>();
                        VoronoiEdge[] cLines = pAddonLinkedRegion.Value.ToArray();
                        foreach (var pLine in cLines)
                            Border[pAddonLinkedRegion.Key].Add(new VoronoiEdge(pLine));
                    }

                    m_bFullyGrown = false;
                }
            }

            return !m_bFullyGrown;
        }

        public bool m_bFullyGrown = false;

        /// <summary>
        /// Присоединяет к провинции сопредельную нечейную землю.
        /// Чем длиннее общая граница с землёй - тем выше вероятность того, что выбрана будет именно она.
        /// </summary>
        /// <returns></returns>
        public override Region Grow(int iMaxSize)
        {
            m_bFullyGrown = false;
            if (Contents.Count >= iMaxSize || m_pCenter.m_iProvincePresence > 200 * Math.Sqrt(iMaxSize / Math.PI))
            {
                //GrowForce(m_pCenter, 1);
                m_bFullyGrown = true;
                return null;
            }

            Region pAddon = GrowPresence(m_pCenter, 1);
            if (pAddon != null)
            {
                Contents.Add(pAddon);
                pAddon.SetOwner(this);

                Border[pAddon].Clear();
                Border.Remove(pAddon);

                foreach (var pAddonLinkedLand in pAddon.BorderWith)
                {
                    if (!pAddonLinkedLand.Key.Forbidden && Contents.Contains(pAddonLinkedLand.Key))
                        continue;

                    if (!Border.ContainsKey(pAddonLinkedLand.Key))
                        Border[pAddonLinkedLand.Key] = new List<VoronoiEdge>();
                    VoronoiEdge[] cLines = pAddonLinkedLand.Value.ToArray();
                    foreach (var pLine in cLines)
                        Border[pAddonLinkedLand.Key].Add(new VoronoiEdge(pLine));
                }
            }

            return pAddon;
        }

        /// <summary>
        /// Заполняет словарь границ с другими провинциями.
        /// </summary>
        public override void Finish(float fCycleShift)
        {
            ChainBorder(fCycleShift);

            foreach (Region pRegion in Border.Keys)
            {
                Province pProvince;
                if (pRegion.Forbidden || !pRegion.HasOwner())
                    pProvince = m_pForbidden;
                else
                    pProvince = pRegion.GetOwner();

                if (!BorderWith.ContainsKey(pProvince))
                    BorderWith[pProvince] = new List<VoronoiEdge>();
                BorderWith[pProvince].AddRange(Border[pRegion]);
            }
            FillBorderWithKeys();

            int iMaxPop = 0;
            Nation pMaxNation = null;
            Dictionary<Nation, int> cClaims = new Dictionary<Nation, int>();
            
            foreach (Region pRegion in Contents)
            {
                bool bRestricted = true;
                foreach (LandX pLand in pRegion.Contents)
                    foreach (Location pLoc in pLand.Origin.Contents)
                        if (!pLoc.Forbidden && !pLoc.IsBorder)
                            bRestricted = false;

                if (bRestricted)
                    continue;

                int iCount = 0;
                cClaims.TryGetValue(pRegion.m_pNatives, out iCount);
                cClaims[pRegion.m_pNatives] = iCount + pRegion.Contents.Count;
                if (cClaims[pRegion.m_pNatives] > iMaxPop)
                {
                    iMaxPop = cClaims[pRegion.m_pNatives];
                    pMaxNation = pRegion.m_pNatives;
                }
            }

            if (pMaxNation != null && !m_pLocalSociety.TitularNation.IsAncient)
                m_pLocalSociety.UpdateTitularNation(pMaxNation);

            foreach (Region pRegion in Contents)
                pRegion.m_pNatives = m_pLocalSociety.TitularNation;
        }

        public void BuildLairs(int iScale)
        {
            foreach (Region pRegion in Contents)
                foreach (LandX pLand in pRegion.Contents)
                    pLand.BuildLair();
        }

        public void BuildSettlements(SettlementSize eSize, bool bFast)
        {
            //проверим для начала, а позволяет ли вообще текущий уровень инфраструктуры поселения такого размера?
            if (!State.InfrastructureLevels[m_pLocalSociety.InfrastructureLevel].m_cAvailableSettlements.Contains(eSize))
                return;

            //определим, сколько поселений должно быть.
            //считаем, что каждая третья земля в провинции содержит поселение, 2/3 из них деревни, 2/9 городки и 1/9 - большие города
            int iMinCount = Contents.Count / 3;
            switch (eSize)
            {
                case SettlementSize.City:
                    iMinCount = Contents.Count / 9;
                    break;
                case SettlementSize.Town:
                    iMinCount = Contents.Count / 6;
                    break;
            }

            //если провинция состоит из единственной земли, увеличиваем там плотность населения вдвое
            int iSingleLandMultiplier = 1;
            if(Contents.Count == 1)
                iSingleLandMultiplier = 2;

            iMinCount *= iSingleLandMultiplier;

            //рассчитаем для каждой земли, входящей в провинцию, шанс быть выбранной для поселения заданного размера
            Dictionary<LandX, float> cLandsChances = new Dictionary<LandX, float>();
            foreach (Region pRegion in Contents)
                foreach (LandX pLand in pRegion.Contents)
                    cLandsChances[pLand] = pLand.Origin.Contents.Count * pLand.Origin.LandType.Get<SettlementsInfo>().GetDensity(eSize);

            //пытаемся построить заданное количество поселений.
            //реально может быть построено меньше, если свободных мест меньше, чем поселений
            for (int i = 0; i < iMinCount; i++)
            {
                int iChance = Rnd.ChooseOne(cLandsChances.Values, 1);
                if (iChance >= 0)
                {
                    LandX pLandX = cLandsChances.ElementAt(iChance).Key;

                    LocationX pSettlement = pLandX.BuildSettlement(Settlement.Info[eSize], false, bFast);
                    if (pSettlement != null)
                    {
                        m_pLocalSociety.Settlements.Add(pSettlement);
                        //bHaveOne = true;
                    }
                    cLandsChances[pLandX] = cLandsChances[pLandX] / 2;//0;
                }
            }

            //закончив "обязательную программу" пройдёмся по всем землям и во всех, где нет ни одного поселения, попытаемся что-нибудь построить.
            foreach (Region pRegion in Contents)
            {
                foreach (LandX pLand in pRegion.Contents)
                {
                    int iSettlements = 0;
                    foreach (Location pLoc in pLand.Origin.Contents)
                    {
                        LocationX pLocX = pLoc.As<LocationX>();
                        if (pLocX.Settlement != null && pLocX.Settlement.RuinsAge == 0)
                        {
                            iSettlements++;
                            //break;
                        }
                    }
                    //if (bHaveOne)
                    //    continue;

                    //считаем среднее количество поселений в земле исходя из размеров земли и вероятности поселения.
                    //если количество выходит меньше 1, то считаем вероятность единственного поселения.
                    //впрочем, больше одного строить всё равно не будем.
                    int iSettlementsCount = (int)(pLand.Origin.Contents.Count * pLand.Origin.LandType.Get<SettlementsInfo>().GetDensity(eSize) * iSingleLandMultiplier);
                    if (iSettlementsCount == 0)
                    {
                        int iSettlementChance = (int)(1 / (pLand.Origin.Contents.Count * pLand.Origin.LandType.Get<SettlementsInfo>().GetDensity(eSize) * iSingleLandMultiplier));
                        if (Rnd.OneChanceFrom(iSettlementChance))
                            iSettlementsCount = 1;
                    }
                    //else
                    //    iSettlementsCount = 1;

                    iSettlementsCount -= iSettlements;

                    for (int i = 0; i < iSettlementsCount; i++)
                    {
                        //if (bHaveOne && !Rnd.OneChanceFrom(3))
                        //    continue;

                        LocationX pSettlement = pLand.BuildSettlement(Settlement.Info[eSize], false, bFast);
                        if (pSettlement != null)
                        {
                            m_pLocalSociety.Settlements.Add(pSettlement);
                            //bHaveOne = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Присоединяет в общую транспортную сеть ещё не присоединённые города государства.
        /// </summary>
        /// <param name="eRoadLevel">Уровень новых дорог: 1 - просёлок, 2 - обычная дорога, 3 - имперская дорога</param>
        public void BuildRoads(RoadQuality eRoadLevel, float fCycleShift)
        {
            if (eRoadLevel > State.InfrastructureLevels[m_pLocalSociety.InfrastructureLevel].m_eMaxGroundRoad)
                eRoadLevel = State.InfrastructureLevels[m_pLocalSociety.InfrastructureLevel].m_eMaxGroundRoad;

            if (eRoadLevel == RoadQuality.None)
                return;

            foreach (Region pRegion in Contents)
                foreach (LandX pLand in pRegion.Contents)
                    foreach (Location pLoc in pLand.Origin.Contents)
                        foreach (TransportationNode pLinked in pLoc.Links.Keys)
                        {
                            if (pLinked is Location)
                            {
                                Land pLinkedOwner = (pLinked as Location).GetOwner();
                                if (!pLinkedOwner.As<LandX>().HasOwner() || pLinkedOwner.As<LandX>().GetOwner().GetOwner() != this)
                                    pLoc.Links[pLinked].IsClosed = true;
                            }
                            else
                                pLoc.Links[pLinked].IsClosed = true;
                        }

            List<LocationX> cConnected = new List<LocationX>();
            cConnected.Add(m_pAdministrativeCenter);

            LocationX[] aSettlements = m_pLocalSociety.Settlements.ToArray();

            foreach (LocationX pTown in aSettlements)
            {
                if (!cConnected.Contains(pTown) && (pTown.Roads[RoadQuality.Normal].Count > 0 || pTown.Roads[RoadQuality.Good].Count > 1))
                    cConnected.Add(pTown);
            }

            while (cConnected.Count < aSettlements.Length)
            {
                LocationX pBestTown1 = null;
                LocationX pBestTown2 = null;
                float fMinLength = float.MaxValue;

                foreach (LocationX pTown in aSettlements)
                {
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

            foreach (Region pRegion in Contents)
                foreach (LandX pLand in pRegion.Contents)
                    foreach (Location pLoc in pLand.Origin.Contents)
                        foreach (TransportationNode pLink in pLoc.Links.Keys)
                            pLoc.Links[pLink].IsClosed = false;
        }

        /// <summary>
        /// Строит столицу провинции, рассчитывает уровни технического и культурного развития...
        /// </summary>
        /// <param name="bFast">флаг быстрой (упрощённой) генерации</param>
        /// <returns>локация со столицей</returns>
        public LocationX BuildCapital(bool bFast)
        {
            int iAverageMagicLimit = 0;

            foreach (LandResource eRes in Enum.GetValues(typeof(LandResource)))
                m_cResources[eRes] = 0;

            m_iPopulation = 0;

            foreach (Region pRegion in Contents)
            {
                foreach (LandX pLand in pRegion.Contents)
                {
                    int iCoast = 0;
                    int iBorder = 0;
                    foreach (Location pLoc in pLand.Origin.Contents)
                    {
                        foreach (Location pLink in pLoc.BorderWithKeys)
                        {
                            if (pLink.GetOwner() != pLoc.GetOwner())
                                iBorder++;
                            if (pLink.HasOwner() && pLink.GetOwner().IsWater)
                                iCoast++;
                        }
                    }

                    m_cResources[LandResource.Fish] += iCoast * 3 / pLand.Origin.MovementCost;
                    m_cResources[LandResource.Grain] += pLand.Origin.Contents.Count * pLand.Origin.LandType.Get<ResourcesInfo>().GetAmount(LandResource.Grain);
                    m_cResources[LandResource.Game] += pLand.Origin.Contents.Count * pLand.Origin.LandType.Get<ResourcesInfo>().GetAmount(LandResource.Game);
                    m_cResources[LandResource.Wood] += pLand.Origin.Contents.Count * pLand.Origin.LandType.Get<ResourcesInfo>().GetAmount(LandResource.Wood);
                    m_cResources[LandResource.Ore] += pLand.Origin.Contents.Count * pLand.Origin.LandType.Get<ResourcesInfo>().GetAmount(LandResource.Ore);

                    m_iPopulation += pLand.Origin.Contents.Count;
                    iAverageMagicLimit += m_pLocalSociety.TitularNation.ProtoSociety.MagicLimit * pLand.Origin.Contents.Count;
                }
            }

            iAverageMagicLimit = iAverageMagicLimit / m_iPopulation;

            m_pLocalSociety.CheckResources(m_cResources, m_iPopulation, Contents.Count);

            SettlementInfo pSettlementInfo = Settlement.Info[SettlementSize.Hamlet];

            if (State.InfrastructureLevels[m_pLocalSociety.InfrastructureLevel].m_cAvailableSettlements.Contains(SettlementSize.Capital))
                pSettlementInfo = Settlement.Info[SettlementSize.Capital];
            else
                if (State.InfrastructureLevels[m_pLocalSociety.InfrastructureLevel].m_cAvailableSettlements.Contains(SettlementSize.City))
                    pSettlementInfo = Settlement.Info[SettlementSize.City];
                else
                    if (State.InfrastructureLevels[m_pLocalSociety.InfrastructureLevel].m_cAvailableSettlements.Contains(SettlementSize.Town))
                        pSettlementInfo = Settlement.Info[SettlementSize.Town];
                    else
                        if (State.InfrastructureLevels[m_pLocalSociety.InfrastructureLevel].m_cAvailableSettlements.Contains(SettlementSize.Village))
                            pSettlementInfo = Settlement.Info[SettlementSize.Village];

            BuildAdministrativeCenter(pSettlementInfo, bFast);
            if (m_pAdministrativeCenter != null)
            {
                m_pLocalSociety.Settlements.Add(m_pAdministrativeCenter);
            }
            else
                throw new InvalidOperationException("Can't build capital!");

            return m_pAdministrativeCenter;
        }

        /// <summary>
        /// Находит подходящую локацию и строит там поселение указанного типа.
        /// Вспомогательная приватная функция, вызывается из BuildCapital.
        /// </summary>
        /// <param name="pCenter">тип поселения</param>
        /// <param name="bFast">флаг быстрой (упрощённой) генерации</param>
        /// <returns>локация с построенным поселением</returns>
        private LocationX BuildAdministrativeCenter(SettlementInfo pCenter, bool bFast)
        {
            Dictionary<LandX, float> cLandsChances = new Dictionary<LandX, float>();

            foreach (Region pRegion in Contents)
            {
                foreach (LandX pLand in pRegion.Contents)
                {
                    bool bRestricted = true;
                    foreach (Location pLoc in pLand.Origin.Contents)
                        if (!pLoc.Forbidden && !pLoc.IsBorder)
                            bRestricted = false;

                    if (bRestricted)
                        continue;

                    cLandsChances[pLand] = (float)pLand.Origin.Contents.Count * pLand.Origin.LandType.Get<SettlementsInfo>().GetDensity(pCenter.Size);

                    bool bProvinceBorder = false;
                    bool bStateBorder = false;
                    foreach (Land pLink in pLand.Origin.BorderWithKeys)
                    {
                        if (pLink.Forbidden)
                            continue;

                        if (pLink.IsWater)
                            continue;

                        if (pLink.As<LandX>().GetOwner().GetOwner() != this)
                            bProvinceBorder = true;

                        if (pLink.As<LandX>().GetOwner().GetOwner().GetOwner() != GetOwner())
                            bStateBorder = true;
                    }

                    if (bProvinceBorder)
                        cLandsChances[pLand] /= 100.0f;

                    if (bStateBorder)
                        cLandsChances[pLand] /= 100.0f;
                }
            }

            if (cLandsChances.Count == 0)
                return null;

            int iChance = Rnd.ChooseOne(cLandsChances.Values, 1);

            foreach (LandX pLand in cLandsChances.Keys)
            {
                iChance--;
                if (iChance < 0)
                {
                    m_pAdministrativeCenter = pLand.BuildSettlement(pCenter, true, bFast);
                    break;
                }
            }

            //Грязный хак: низкокультурные сообщества не могут быть многонациональными
            //State pState = Owner as State;
            //if (pState.m_iInfrastructureLevel < 2)
            //    m_pRace = pState.m_pRace;

            return m_pAdministrativeCenter;
        }

        public override string ToString()
        {
            return string.Format("province {0} ({2}, {1})", m_pLocalSociety.Name, m_pAdministrativeCenter == null ? "-" : m_pAdministrativeCenter.ToString(), m_pLocalSociety.TitularNation);
        }

        public override float GetMovementCost()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Негативное отношение к другому государству.
        /// </summary>
        /// <param name="pOpponent"></param>
        /// <returns></returns>
        public int CalcHostility(Province pOpponent)
        {
            int iHostility = 0;

            if (m_pLocalSociety.TitularNation != pOpponent.m_pLocalSociety.TitularNation)
            {
                iHostility++;

                if (m_pLocalSociety.TitularNation.Race.Language != pOpponent.m_pLocalSociety.TitularNation.Race.Language)
                    iHostility++;
            }
            else
                iHostility--;

            iHostility += m_pLocalSociety.DominantCulture.Customs.GetDifference(pOpponent.m_pLocalSociety.DominantCulture.Customs);

            //if (m_iFood < m_iPopulation && pOpponent.m_iFood > pOpponent.m_iPopulation * 2)
            //    iHostility++;
            //if (m_iWood < m_iPopulation && pOpponent.m_iWood > pOpponent.m_iPopulation * 2)
            //    iHostility++;
            //if (m_iOre < m_iPopulation && pOpponent.m_iOre > pOpponent.m_iPopulation * 2)
            //    iHostility++;

            if (pOpponent.m_pLocalSociety.InfrastructureLevel > m_pLocalSociety.InfrastructureLevel)
                iHostility++;
            else
                if (pOpponent.m_pLocalSociety.InfrastructureLevel < m_pLocalSociety.InfrastructureLevel)
                    iHostility++;

            float iMentalityDifference = m_pLocalSociety.DominantCulture.GetMentalityDifference(pOpponent.m_pLocalSociety.DominantCulture);
            if (iMentalityDifference < -0.75)
                iHostility -= 2;
            else
                if (iMentalityDifference < -0.5)
                    iHostility--;
                else
                    if (iMentalityDifference > 0.5)
                        iHostility += 2;
                    else
                        if (iMentalityDifference > 0)
                            iHostility++;

            if (iHostility > 0)
            {
                iHostility = (int)(m_pLocalSociety.DominantCulture.GetTrait(Trait.Fanaticism) * iHostility + 0.25);
                iHostility = (int)(m_pLocalSociety.DominantCulture.GetTrait(Trait.Agression) * iHostility + 0.25);
                if (iHostility == 0)
                    iHostility = 1;
            }
            else
            {
                if (iHostility < 0)
                {
                    iHostility = (int)((2.0f - m_pLocalSociety.DominantCulture.GetTrait(Trait.Fanaticism)) * iHostility - 0.25);
                    iHostility = (int)((2.0f - m_pLocalSociety.DominantCulture.GetTrait(Trait.Agression)) * iHostility - 0.25);
                    if (iHostility == 0)
                        iHostility = -1;
                }
            }

            return iHostility;
        }
    }
}
