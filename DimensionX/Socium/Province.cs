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
    public class Province : BorderBuilder<LandX>, ITerritory
    {
        /// <summary>
        /// ТОЛКО ДЛЯ ОТЛАДКИ!!!!
        /// </summary>
        public Dictionary<Province, string> m_cConnectionString = new Dictionary<Province,string>();
        
        private static Province m_pForbidden = new Province(true);

        /// <summary>
        /// Земли, составляющие провинцию
        /// </summary>
        public readonly List<LandX> m_cContents = new List<LandX>();

        #region ITerritory members
        public Dictionary<object, List<Location.Edge>> BorderWith { get; } = new Dictionary<object, List<Location.Edge>>();

        public bool Forbidden { get; } = false;

        public object Owner { get; set; } = null;

        public float PerimeterLength { get; private set; } = 0;
        #endregion ITerritory members

        public State OwnerState
        {
            get
            {
                return Owner as State;
            }
        }

        public object[] m_aBorderWith = null;

        internal void FillBorderWithKeys()
        {
            m_aBorderWith = new List<object>(BorderWith.Keys).ToArray();

            PerimeterLength = 0;
            foreach (var pBorder in BorderWith)
                foreach (Location.Edge pLine in pBorder.Value)
                    PerimeterLength += pLine.m_fLength;
        }

        public LandX m_pCenter;

        public LocationX m_pAdministrativeCenter = null;

        public NationalSociety m_pLocalSociety = null;

        public float m_fFish = 0;
        public float m_fGrain = 0;
        public float m_fGame = 0;
        public float m_fOre = 0;
        public float m_fWood = 0;
        public int m_iPopulation = 0;

        public Province(bool bForbidden)
        {
            Forbidden = bForbidden;
        }

        public Province()
        {}

        /// <summary>
        /// Зарождение провинции в указанной земле
        /// </summary>
        /// <param name="pSeed"></param>
        public override void Start(LandX pSeed)
        {
            if (pSeed.m_pProvince != null)
                throw new Exception("That land already belongs to province!!!");

            BorderWith.Clear();
            m_cContents.Clear();

            base.Start(pSeed);

            m_pCenter = pSeed;

            m_pLocalSociety = new NationalSociety(pSeed.m_pNation);

            m_cContents.Add(pSeed);
            pSeed.m_pProvince = this;
            pSeed.m_iProvincePresence = 1;

            m_cGrowCosts.Clear();

            m_bFullyGrown = false;
        }

        /// <summary>
        /// Стоимость присоединения указанной земли к провинции
        /// </summary>
        /// <param name="pLand"></param>
        /// <returns></returns>
        private int GetGrowCost(LandX pLand)
        {
            if (pLand.IsWater)
                return -1;

            float fCost = pLand.Type.m_iMovementCost;

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
            //        if ((pLinkTerr.Key as ITerritory).Forbidden)
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



            foreach (LandTypeInfoX pType in m_pLocalSociety.m_pTitularNation.m_aPreferredLands)
                if (pType == pLand.Type)
                    fCost /= 10;// (float)pLand.Type.m_iMovementCost;//2;

            foreach (LandTypeInfoX pType in m_pLocalSociety.m_pTitularNation.m_aHatedLands)
                if (pType == pLand.Type)
                    fCost *= 10;// (float)pLand.Type.m_iMovementCost;//2;

            if (pLand.m_pNation != m_pLocalSociety.m_pTitularNation)
            {
                if (m_pLocalSociety.m_pTitularNation.m_bDying)
                    fCost *= 9999999;
                else
                    fCost *= 99999;
            }

            if (m_pLocalSociety.m_pTitularNation.m_bHegemon)
                fCost /= 2;

            if (fCost < 1)
                fCost = 1;

            if (fCost > int.MaxValue)
                fCost = int.MaxValue - 1;

            return (int)fCost;
        }

        private Dictionary<LandX, int> m_cGrowCosts = new Dictionary<LandX, int>();

        /// <summary>
        /// Усилить претензии провинции на сопредельную ничейную землю
        /// </summary>
        /// <param name="pLand"></param>
        /// <param name="iValue"></param>
        /// <returns></returns>
        private LandX GrowPresence(LandX pLand, int iValue)
        {
            pLand.m_iProvincePresence += iValue;

            int iOwnCost = 0;
            if (!m_cGrowCosts.TryGetValue(pLand, out iOwnCost))
            {
                iOwnCost = GetGrowCost(pLand);
                m_cGrowCosts[pLand] = iOwnCost;
            }

            if (pLand.m_iProvincePresence > iOwnCost)
            {
                float fBestValue = float.MaxValue;
                int iBestCost = 0;
                LandX pBestLand = null;

                foreach (var pLinkTerr in pLand.BorderWith)
                {
                    if ((pLinkTerr.Key as ITerritory).Forbidden)
                        continue;

                    LandX pLinkedLand = pLinkTerr.Key as LandX;

                    if (pLinkedLand.IsWater || (pLinkedLand.m_pProvince != this && pLinkedLand.m_pProvince != null))
                        continue;

                    int iCost = 0;
                    if (!m_cGrowCosts.TryGetValue(pLinkedLand, out iCost))
                    {
                        iCost = GetGrowCost(pLinkedLand);
                        m_cGrowCosts[pLinkedLand] = iCost;
                    }

                    if (iCost == -1)
                        continue;

                    float fCostModified = iCost;

                    if (pLinkedLand.m_pProvince == null)
                    {
                        //общая граница провинции и новой земли
                        float fSharedPerimeter = 1;
                        Location.Edge[] aBorderLine = m_cBorder[pLinkedLand].ToArray();
                        foreach (Location.Edge pLine in aBorderLine)
                            fSharedPerimeter += pLine.m_fLength;

                        //fCommonLength /= fTotalLength;

                        //if (fCommonLength < 0.25f)
                        //    fCommonLength /= 10;
                        //if (fCommonLength > 0.5f)
                        //    fCommonLength *= 10;

                        fCostModified = iCost * pLinkedLand.PerimeterLength / fSharedPerimeter;

                        if (fSharedPerimeter < pLinkedLand.PerimeterLength / 4)
                            fCostModified *= 10;
                        if (fSharedPerimeter > pLinkedLand.PerimeterLength / 2)
                            fCostModified /= 10;

                        fCostModified = Math.Max(1, fCostModified);
                    }

                    if (pLinkedLand.m_iProvincePresence + fCostModified < fBestValue ||
                        (pLinkedLand.m_iProvincePresence + fCostModified == fBestValue &&
                         Rnd.OneChanceFrom(pLand.BorderWith.Count)))
                    {
                        fBestValue = pLinkedLand.m_iProvincePresence + fCostModified;
                        iBestCost = iCost;
                        pBestLand = pLinkedLand;
                    }
                }

                if (pBestLand != null && pBestLand.m_iProvincePresence + iBestCost < pLand.m_iProvincePresence - iBestCost)
                {
                    pLand.m_iProvincePresence -= iBestCost;
                    if (pBestLand.m_pProvince == null)
                    {
                        if(pBestLand.m_pNation != pLand.m_pNation)
                            GetGrowCost(pBestLand);
                        pBestLand.m_iProvincePresence += iBestCost;
                        return pBestLand;
                    }
                    else
                    {
                        return GrowPresence(pBestLand, iBestCost);
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
            object[] aBorder = new List<object>(m_cBorder.Keys).ToArray();

            m_bFullyGrown = true;

            if (m_pLocalSociety.m_pTitularNation.m_bDying)
                return !m_bFullyGrown;

            foreach (ITerritory pTerr in aBorder)
            {
                if (pTerr.Forbidden)
                    continue;

                LandX pLand = pTerr as LandX;

                if (pLand.m_pNation != m_pLocalSociety.m_pTitularNation)
                    continue;

                if (pLand.m_pProvince == null && !pLand.IsWater)
                {
                    m_cContents.Add(pLand);
                    pLand.m_pProvince = this;

                    m_cBorder[pLand].Clear();
                    m_cBorder.Remove(pLand);

                    foreach (var pAddonLinkedLand in pLand.BorderWith)
                    {
                        if (!(pAddonLinkedLand.Key as ITerritory).Forbidden && m_cContents.Contains(pAddonLinkedLand.Key as LandX))
                            continue;

                        if (!m_cBorder.ContainsKey(pAddonLinkedLand.Key))
                            m_cBorder[pAddonLinkedLand.Key] = new List<Location.Edge>();
                        Location.Edge[] cLines = pAddonLinkedLand.Value.ToArray();
                        foreach (Location.Edge pLine in cLines)
                            m_cBorder[pAddonLinkedLand.Key].Add(new Location.Edge(pLine));
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
        public bool Grow(int iMaxProvinceSize)
        {
            m_bFullyGrown = false;
            //if (m_pCenter.m_iProvinceForce > 20*Math.Sqrt(iMaxProvinceSize/Math.PI))
            if (m_cContents.Count >= iMaxProvinceSize || m_pCenter.m_iProvincePresence > 200 * Math.Sqrt(iMaxProvinceSize / Math.PI))
            {
                //GrowForce(m_pCenter, 1);
                m_bFullyGrown = true;
                return false;
            }

            LandX pAddon = GrowPresence(m_pCenter, 1);
            if (pAddon != null)
            {
                m_cContents.Add(pAddon);
                pAddon.m_pProvince = this;

                m_cBorder[pAddon].Clear();
                m_cBorder.Remove(pAddon);

                foreach (var pAddonLinkedLand in pAddon.BorderWith)
                {
                    if (!(pAddonLinkedLand.Key as ITerritory).Forbidden && m_cContents.Contains(pAddonLinkedLand.Key as LandX))
                        continue;

                    if (!m_cBorder.ContainsKey(pAddonLinkedLand.Key))
                        m_cBorder[pAddonLinkedLand.Key] = new List<Location.Edge>();
                    Location.Edge[] cLines = pAddonLinkedLand.Value.ToArray();
                    foreach (Location.Edge pLine in cLines)
                        m_cBorder[pAddonLinkedLand.Key].Add(new Location.Edge(pLine));
                }
            }

            return true;
        }

        /// <summary>
        /// Заполняет словарь границ с другими провинциями.
        /// </summary>
        public void Finish(float fCycleShift)
        {
            ChainBorder(fCycleShift);

            foreach (ITerritory pLand in m_cBorder.Keys)
            {
                Province pProvince;
                if (pLand.Forbidden || (pLand as LandX).m_pProvince == null)
                    pProvince = Province.m_pForbidden;
                else
                    pProvince = (pLand as LandX).m_pProvince;

                if (!BorderWith.ContainsKey(pProvince))
                    BorderWith[pProvince] = new List<Location.Edge>();
                BorderWith[pProvince].AddRange(m_cBorder[pLand]);
            }
            FillBorderWithKeys();

            int iMaxPop = 0;
            Nation pMaxNation = null;
            Dictionary<Nation, int> cClaims = new Dictionary<Nation, int>();
            
            foreach (LandX pLand in m_cContents)
            {
                bool bRestricted = true;
                foreach (LocationX pLoc in pLand.m_cContents)
                    if (!pLoc.Forbidden && !pLoc.m_bBorder)
                        bRestricted = false;

                if (bRestricted)
                    continue;

                int iCount = 0;
                cClaims.TryGetValue((pLand.Area as AreaX).m_pNation, out iCount);
                cClaims[(pLand.Area as AreaX).m_pNation] = iCount + pLand.m_cContents.Count;
                if (cClaims[(pLand.Area as AreaX).m_pNation] > iMaxPop)
                {
                    iMaxPop = cClaims[(pLand.Area as AreaX).m_pNation];
                    pMaxNation = (pLand.Area as AreaX).m_pNation;
                }
            }

            if (pMaxNation != null && !m_pLocalSociety.m_pTitularNation.m_bDying)
                m_pLocalSociety.m_pTitularNation = pMaxNation;

            foreach (LandX pLand in m_cContents)
                pLand.m_pNation = m_pLocalSociety.m_pTitularNation;
        }

        public void BuildLairs(int iScale)
        {
            foreach (LandX pLand in m_cContents)
                pLand.BuildLair();
        }

        public void BuildSettlements(SettlementSize eSize, bool bFast)
        {
            //проверим для начала, а позволяет ли вообще текущий уровень инфраструктуры поселения такого размера?
            if (!State.InfrastructureLevels[m_pLocalSociety.m_iInfrastructureLevel].m_cAvailableSettlements.Contains(eSize))
                return;

            //определим, сколько поселений должно быть.
            //считаем, что каждая третья земля в провинции содержит поселение, 2/3 из них деревни, 2/9 городки и 1/9 - большие города
            int iMinCount = m_cContents.Count / 3;
            switch (eSize)
            {
                case SettlementSize.City:
                    iMinCount = m_cContents.Count / 9;
                    break;
                case SettlementSize.Town:
                    iMinCount = m_cContents.Count / 6;
                    break;
            }

            //если провинция состоит из единственной земли, увеличиваем там плотность населения вдвое
            int iSingleLandMultiplier = 1;
            if(m_cContents.Count == 1)
                iSingleLandMultiplier = 2;

            iMinCount *= iSingleLandMultiplier;

            //рассчитаем для каждой земли, входящей в провинцию, шанс быть выбранной для поселения заданного размера
            Dictionary<LandX, float> cLandsChances = new Dictionary<LandX, float>();
            foreach (LandX pLand in m_cContents)
                cLandsChances[pLand] = (float)pLand.m_cContents.Count * pLand.Type.m_cSettlementsDensity[eSize];

            //пытаемся построить заданное количество поселений.
            //реально может быть построено меньше, если свободных мест меньше, чем поселений
            for (int i = 0; i < iMinCount; i++)
            {
                int iChance = Rnd.ChooseOne(cLandsChances.Values, 1);

                foreach (LandX pLand in cLandsChances.Keys)
                {
                    iChance--;
                    if (iChance < 0)
                    {
                        LocationX pSettlement = pLand.BuildSettlement(Settlement.Info[eSize], false, bFast);
                        if (pSettlement != null)
                        {
                            m_pLocalSociety.Settlements.Add(pSettlement);
                            //bHaveOne = true;
                        }
                        cLandsChances[pLand] = cLandsChances[pLand] / 2;//0;
                        break;
                    }
                }
            }

            //закончив "обязательную программу" пройдёмся по всем землям и во всех, где нет ни одного поселения, попытаемся что-нибудь построить.
            foreach (LandX pLand in m_cContents)
            {
                int iSettlements = 0;
                foreach (LocationX pLoc in pLand.m_cContents)
                    if (pLoc.m_pSettlement != null && pLoc.m_pSettlement.m_iRuinsAge == 0)
                    {
                        iSettlements++;
                        //break;
                    }
                //if (bHaveOne)
                //    continue;

                //считаем среднее количество поселений в земле исходя из размеров земли и вероятности поселения.
                //если количество выходит меньше 1, то считаем вероятность единственного поселения.
                //впрочем, больше одного строить всё равно не будем.
                int iSettlementsCount = (int)(pLand.m_cContents.Count * pLand.Type.m_cSettlementsDensity[eSize] * iSingleLandMultiplier);
                if (iSettlementsCount == 0)
                {
                    int iSettlementChance = (int)(1 / (pLand.m_cContents.Count * pLand.Type.m_cSettlementsDensity[eSize] * iSingleLandMultiplier));
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

        /// <summary>
        /// Присоединяет в общую транспортную сеть ещё не присоединённые города государства.
        /// </summary>
        /// <param name="eRoadLevel">Уровень новых дорог: 1 - просёлок, 2 - обычная дорога, 3 - имперская дорога</param>
        public void BuildRoads(RoadQuality eRoadLevel, float fCycleShift)
        {
            if (eRoadLevel > State.InfrastructureLevels[m_pLocalSociety.m_iInfrastructureLevel].m_eMaxGroundRoad)
                eRoadLevel = State.InfrastructureLevels[m_pLocalSociety.m_iInfrastructureLevel].m_eMaxGroundRoad;

            if (eRoadLevel == RoadQuality.None)
                return;

            foreach (LandX pLand in m_cContents)
                foreach (LocationX pLoc in pLand.m_cContents)
                    foreach (TransportationNode pLinked in pLoc.m_cLinks.Keys)
                    {
                        if (pLinked is LocationX)
                        {
                            LandX pLinkedOwner = (pLinked as LocationX).Owner as LandX;
                            if (pLinkedOwner.m_pProvince != this)
                                pLoc.m_cLinks[pLinked].m_bClosed = true;
                        }
                        else
                            pLoc.m_cLinks[pLinked].m_bClosed = true;
                    }

            List<LocationX> cConnected = new List<LocationX>();
            cConnected.Add(m_pAdministrativeCenter);

            LocationX[] aSettlements = m_pLocalSociety.Settlements.ToArray();

            foreach (LocationX pTown in aSettlements)
            {
                if (!cConnected.Contains(pTown) && (pTown.m_cRoads[RoadQuality.Normal].Count > 0 || pTown.m_cRoads[RoadQuality.Good].Count > 1))
                    cConnected.Add(pTown);
            }

            while (cConnected.Count < aSettlements.Length)
            {
                //Road pBestRoad = null;
                LocationX pBestTown1 = null;
                LocationX pBestTown2 = null;
                float fMinLength = float.MaxValue;

                foreach (LocationX pTown in aSettlements)
                {
                    if (cConnected.Contains(pTown))
                        continue;

                    foreach (LocationX pOtherTown in cConnected)
                    {
                        float fDist = pTown.DistanceTo(pOtherTown, fCycleShift);// (float)Math.Sqrt((pTown.X - pOtherTown.X) * (pTown.X - pOtherTown.X) + (pTown.Y - pOtherTown.Y) * (pTown.Y - pOtherTown.Y));

                        if (fDist < fMinLength &&
                            (fMinLength == float.MaxValue ||
                             Rnd.OneChanceFrom(2)))
                        {
                            fMinLength = fDist;
                            //pBestRoad = pRoad;

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
                        float fDist = pBestTown1.DistanceTo(pOtherTown, fCycleShift);// (float)Math.Sqrt((pBestTown1.X - pOtherTown.X) * (pBestTown1.X - pOtherTown.X) + (pBestTown1.Y - pOtherTown.Y) * (pBestTown1.Y - pOtherTown.Y));

                        if (pOtherTown != pBestTown2 &&
                            fDist < fMinLength &&
                            (fMinLength == float.MaxValue ||
                             Rnd.OneChanceFrom(2)))
                        {
                            fMinLength = fDist;
                            //pBestRoad = pRoad;

                            pBestTown3 = pOtherTown;
                        }
                    }

                    if (pBestTown3 != null)
                        World.BuildRoad(pBestTown1, pBestTown3, eRoadLevel, fCycleShift);

                    cConnected.Add(pBestTown1);
                }
            }

            foreach (LandX pLand in m_cContents)
                foreach (LocationX pLoc in pLand.m_cContents)
                    foreach (TransportationNode pLink in pLoc.m_cLinks.Keys)
                        pLoc.m_cLinks[pLink].m_bClosed = false;
        }

        /// <summary>
        /// Строит столицу провинции, рассчитывает уровни технического и культурного развития...
        /// </summary>
        /// <param name="bFast">флаг быстрой (упрощённой) генерации</param>
        /// <returns>локация со столицей</returns>
        public LocationX BuildCapital(bool bFast)
        {
            int iAverageMagicLimit = 0;

            m_fFish = 0;
            m_fGrain = 0;
            m_fGame = 0;
            m_fWood = 0;
            m_fOre = 0;
            m_iPopulation = 0;

            foreach (LandX pLand in m_cContents)
            {
                int iCoast = 0;
                int iBorder = 0;
                foreach (LocationX pLoc in pLand.m_cContents)
                {
                    foreach (LocationX pLink in pLoc.m_aBorderWith)
                    {
                        if (pLink.Owner != pLoc.Owner)
                            iBorder++;
                        if (pLink.Owner != null && (pLink.Owner as LandX).IsWater)
                            iCoast++;
                    }
                }

                m_fFish += iCoast*3/pLand.MovementCost;
                m_fGrain += pLand.m_cContents.Count * pLand.Type.m_cResources[LandTypeInfoX.Resource.Grain];
                m_fGame += pLand.m_cContents.Count * pLand.Type.m_cResources[LandTypeInfoX.Resource.Game];
                m_fWood += pLand.m_cContents.Count * pLand.Type.m_cResources[LandTypeInfoX.Resource.Wood];
                m_fOre += pLand.m_cContents.Count * pLand.Type.m_cResources[LandTypeInfoX.Resource.Ore];

                m_iPopulation += pLand.m_cContents.Count;
                iAverageMagicLimit += m_pLocalSociety.m_pTitularNation.m_pSociety.m_iMagicLimit * pLand.m_cContents.Count;
            }

            iAverageMagicLimit = iAverageMagicLimit / m_iPopulation;

            float fFood = m_pLocalSociety.GetAvailableFood(m_fWood, m_fOre, m_fGrain, m_fGame, m_fFish, m_iPopulation);

            m_pLocalSociety.CheckResources(m_fWood, m_fOre, fFood, m_iPopulation, m_cContents.Count);

            SettlementInfo pSettlementInfo = Settlement.Info[SettlementSize.Hamlet];

            if (State.InfrastructureLevels[m_pLocalSociety.m_iInfrastructureLevel].m_cAvailableSettlements.Contains(SettlementSize.Capital))
                pSettlementInfo = Settlement.Info[SettlementSize.Capital];
            else
                if (State.InfrastructureLevels[m_pLocalSociety.m_iInfrastructureLevel].m_cAvailableSettlements.Contains(SettlementSize.City))
                    pSettlementInfo = Settlement.Info[SettlementSize.City];
                else
                    if (State.InfrastructureLevels[m_pLocalSociety.m_iInfrastructureLevel].m_cAvailableSettlements.Contains(SettlementSize.Town))
                        pSettlementInfo = Settlement.Info[SettlementSize.Town];
                    else
                        if (State.InfrastructureLevels[m_pLocalSociety.m_iInfrastructureLevel].m_cAvailableSettlements.Contains(SettlementSize.Village))
                            pSettlementInfo = Settlement.Info[SettlementSize.Village];

            BuildAdministrativeCenter(pSettlementInfo, bFast);
            if (m_pAdministrativeCenter != null)
                m_pLocalSociety.Settlements.Add(m_pAdministrativeCenter);
            else
                throw new Exception("Can't build capital!");

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

            foreach (LandX pLand in m_cContents)
            {
                bool bRestricted = true;
                foreach (LocationX pLoc in pLand.m_cContents)
                    if (!pLoc.Forbidden && !pLoc.m_bBorder)
                        bRestricted = false;

                if (bRestricted)
                    continue;

                cLandsChances[pLand] = (float)pLand.m_cContents.Count * pLand.Type.m_cSettlementsDensity[pCenter.m_eSize];

                bool bProvinceBorder = false;
                bool bStateBorder = false;
                foreach (ITerritory pTerr in pLand.m_aBorderWith)
                { 
                    if(pTerr.Forbidden)
                        continue;

                    LandX pLink = pTerr as LandX;

                    if (pLink.IsWater)
                        continue;

                    if (pLink.m_pProvince != this)
                        bProvinceBorder = true;

                    if (pLink.m_pProvince.Owner != this.Owner)
                        bStateBorder = true;
                }

                if (bProvinceBorder)
                    cLandsChances[pLand] /= 100.0f;

                if (bStateBorder)
                    cLandsChances[pLand] /= 100.0f;
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
            //if (m_pAdministrativeCenter != null)
                return string.Format("province {0} ({2}, {1})", m_pLocalSociety.m_sName, m_pAdministrativeCenter == null ? "-" : m_pAdministrativeCenter.ToString(), m_pLocalSociety.m_pTitularNation);
            //else
            //    return "province " + m_sName + " [" + m_pRace.ToString() + "]";
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

            if (m_pLocalSociety.m_pTitularNation != pOpponent.m_pLocalSociety.m_pTitularNation)
            {
                iHostility++;

                if (m_pLocalSociety.m_pTitularNation.m_pRace.m_pLanguage != pOpponent.m_pLocalSociety.m_pTitularNation.m_pRace.m_pLanguage)
                    iHostility++;
            }
            else
                iHostility--;

            iHostility += m_pLocalSociety.DominantCulture.m_pCustoms.GetDifference(pOpponent.m_pLocalSociety.DominantCulture.m_pCustoms);

            //if (m_iFood < m_iPopulation && pOpponent.m_iFood > pOpponent.m_iPopulation * 2)
            //    iHostility++;
            //if (m_iWood < m_iPopulation && pOpponent.m_iWood > pOpponent.m_iPopulation * 2)
            //    iHostility++;
            //if (m_iOre < m_iPopulation && pOpponent.m_iOre > pOpponent.m_iPopulation * 2)
            //    iHostility++;

            if (pOpponent.m_pLocalSociety.m_iInfrastructureLevel > m_pLocalSociety.m_iInfrastructureLevel)
                iHostility++;//= pOpponent.m_iLifeLevel - m_iLifeLevel;
            else
                if (pOpponent.m_pLocalSociety.m_iInfrastructureLevel < m_pLocalSociety.m_iInfrastructureLevel)
                    iHostility++;//= m_iLifeLevel - pOpponent.m_iLifeLevel;

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

        internal void AddBuildings()
        {
            foreach (LocationX pLoc in m_pLocalSociety.Settlements)
                pLoc.m_pSettlement.AddBuildings(this);
        }
    }
}
