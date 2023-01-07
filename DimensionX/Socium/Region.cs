using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;
using NameGen;
using LandscapeGeneration;
using Socium.Nations;
using Socium.Settlements;

namespace Socium
{
    /// <summary>
    /// Регион - группа сопредельных земель одного типа, населённых одной нацией и имеющих одно название (например. Арденнский Лес).
    /// Регионы объеиняются в провинции (Province).
    /// </summary>
    public class Region: TerritoryCluster<Region, LandX>
    {
        private static Region m_pForbidden = new Region(true);

        public LandTypeInfo m_pType;

        public bool IsWater
        {
            get { return m_pType != null && m_pType.m_eEnvironment.HasFlag(LandscapeGeneration.Environment.Liquid); }
        }

        public bool IsBorder()
        {
            foreach (var pLand in Contents)
                if (pLand.IsBorder())
                    return true;

            return false;
        }

        public float MovementCost
        {
            get { return m_pType == null ? 100 : m_pType.m_iMovementCost; }
        }

        public Continent Continent
        {
            get 
            {
                return GetOwner<LandMass>().GetOwner<Continent>();
            }
        }

        /// <summary>
        /// Военное присутсвие сил провинции в данной земле.
        /// Нужно только для формирования карты провинций.
        /// </summary>
        public int m_iProvincePresence = 0;

        /// <summary>
        /// коренное население региона - может отличаться от доминирующего населения локации
        /// </summary>
        public Nation m_pNatives = null;

        public string m_sName;

        private int m_iMaxSize = 1;

        public Region(bool bForbidden) : base(bForbidden)
        {
        }

        public Region()
        { }
        
        public void Start(Land pSeed, int iMaxSize)
        {
            BorderWith.Clear();
            Contents.Clear();

            InitBorder(pSeed);

            Contents.Add(pSeed);
            pSeed.SetOwner(this);

            m_pType = pSeed.LandType;

            m_iMaxSize = iMaxSize / m_pType.m_iMovementCost;
        }

        /// <summary>
        /// Присоединяет к территории сопредельную землю того же типа.
        /// </summary>
        /// <returns></returns>
        public override Territory Grow(int iMaxSize)
        {
            if (Contents.Count > m_iMaxSize && Rnd.OneChanceFrom(Contents.Count - m_iMaxSize))
                return null;

            //List<LAND> cBorder = new List<LAND>();

            Dictionary<Land, float> cBorderLength = new Dictionary<Land, float>();

            Territory[] aBorderLands = new List<Territory>(m_cBorder.Keys).ToArray();
            foreach (Territory pTerr in aBorderLands)
            {
                if (pTerr.Forbidden)
                    continue;

                Land pLand = pTerr as Land;

                if (!pLand.HasOwner<Region>() && pLand.LandType == m_pType)
                {
                    bool bHavePotential = false;

                    float fWholeLength = 1;
                    VoronoiEdge[] aBorderLine = m_cBorder[pLand].ToArray();
                    foreach (var pLine in aBorderLine)
                        fWholeLength += pLine.Length;

                    foreach (var pLinkTerr in pLand.BorderWith)
                    {
                        if (pLinkTerr.Key.Forbidden)
                            continue;

                        if ((pLinkTerr.Key as Land).LandType == m_pType &&
                            !pLinkTerr.Key.HasOwner<Region>())
                            bHavePotential = true;
                    }

                    fWholeLength /= pLand.PerimeterLength;

                    if (fWholeLength < 0.25f && Contents.Count > 1 && bHavePotential)
                        continue;
                    if (fWholeLength < 0.35f)
                        fWholeLength /= 10;
                    if (fWholeLength > 0.5f)
                        fWholeLength *= 10;

                    cBorderLength[pLand] = fWholeLength;
                }
            }

            if (cBorderLength.Count == 0)
                return null;

            Land pAddon = null;

            int iChoice = Rnd.ChooseOne(cBorderLength.Values, 2);
            if (iChoice >= 0)
                pAddon = cBorderLength.ElementAt(iChoice).Key;

            Contents.Add(pAddon);
            pAddon.SetOwner(this);

            m_cBorder[pAddon].Clear();
            m_cBorder.Remove(pAddon);

            foreach (var pBorderLand in pAddon.BorderWith)
            {
                if (Contents.Contains(pBorderLand.Key))
                    continue;

                if (!m_cBorder.ContainsKey(pBorderLand.Key))
                    m_cBorder[pBorderLand.Key] = new List<VoronoiEdge>();
                VoronoiEdge[] cLines = pBorderLand.Value.ToArray();
                foreach (var pLine in cLines)
                    m_cBorder[pBorderLand.Key].Add(new VoronoiEdge(pLine));
            }

            //ChainBorder();

            return pAddon;
        }

        public override void Finish(float fCycleShift)
        {
            //base.Finish();
            ChainBorder(fCycleShift);

            foreach (Territory pLand in m_cBorder.Keys)
            {
                Region pRegion;
                if (pLand.Forbidden || !pLand.HasOwner<Region>())
                    pRegion = Region.m_pForbidden;
                else
                    pRegion = pLand.GetOwner<Region>();

                if (!BorderWith.ContainsKey(pRegion))
                    BorderWith[pRegion] = new List<VoronoiEdge>();
                BorderWith[pRegion].AddRange(m_cBorder[pLand]);
            }
            FillBorderWithKeys();
        }

        public override float GetMovementCost()
        {
            return m_pType == null ? 100 : m_pType.m_iMovementCost;
        }

        public void SetRace(List<Nation> cPossibleNations)
        {
            Dictionary<Nation, float> cChances = new Dictionary<Nation, float>();
            foreach (Nation pNation in cPossibleNations)
            {
                cChances[pNation] = 1.0f;// / pRace.m_iRank;

                if (pNation.IsAncient)
                    cChances[pNation] /= 10 / m_pType.m_iMovementCost;
                    //cChances[pRace] /= 10000 / (m_pType.m_iMovementCost * m_pType.m_iMovementCost);

                if (pNation.IsHegemon)
                    cChances[pNation] *= 10;

                foreach (LandTypeInfo pType in pNation.m_aPreferredLands)
                    if (m_pType == pType)
                        cChances[pNation] *= 10;

                foreach (LandTypeInfo pType in pNation.m_aHatedLands)
                    if (m_pType == pType)
                        cChances[pNation] /= 100;
            }

            int iChance = Rnd.ChooseOne(cChances.Values, 3);
            foreach (Nation pNation in cChances.Keys)
            {
                iChance--;
                if (iChance < 0)
                {
                    m_pNatives = pNation;
                    break;
                }
            }

            m_sName = m_pNatives.m_pRace.m_pLanguage.RandomCountryName();

            foreach (Land pLand in Contents)
            {
                pLand.GetOwner<LandX>().Populate(m_pNatives, m_sName);
            }
        }

        public string GetNativeRaceString()
        {
            if (m_pNatives == null)
                return "unpopulated";
            else
                return m_pNatives.m_pProtoSociety.m_sName;
        }

        internal Location BuildFort(State pEnemy, bool bFast)
        {
            //сначала составим список претендентов из числа незанятых локаций, лежащих непосредственно на границе с супостатом (или на берегу, если супостат не определён).
            Dictionary<Location, int> cChances = new Dictionary<Location, int>();
            bool bNoChances = true;
            foreach (Land pLand in Contents)
            {
                foreach (Location pLoc in pLand.Contents)
                {
                    bool bCoast = false;
                    bool bBorder = false;
                    bool bMapBorder = false;
                    foreach (Location pLink in pLoc.m_aBorderWith)
                    {
                        Land pLandLink = pLink.GetOwner<Land>();
                        if (pLandLink != null)
                        {
                            Region pRegionLink = pLandLink.GetOwner<Region>();
                            if (pRegionLink != null && pRegionLink.HasOwner<Province>() && pRegionLink.GetOwner<Province>().GetOwner<State>() == pEnemy)
                                bBorder = true;

                            if (pLandLink.IsWater)
                                bCoast = true;
                        }

                        if (pLink.m_bBorder)
                        {
                            bMapBorder = true;
                        }
                    }

                    int iChances = 100;

                    if (bMapBorder)
                        iChances = 0;

                    if (pEnemy != null && !bBorder)
                        iChances = 0;

                    if (pEnemy == null && !bCoast)
                        iChances = 0;

                    LocationX pLocX = pLoc.GetOwner<LocationX>();

                    if (pLocX.m_pSettlement != null)
                        iChances = 0;

                    if (pLocX.m_pBuilding != null)
                        iChances = pLocX.m_pBuilding.m_eType == BuildingType.Farm || pLocX.m_pBuilding.m_eType == BuildingType.HuntingFields ? 1 : 0;

                    if (pLoc.m_bBorder || pLoc.m_eType != LandmarkType.Empty)
                        iChances = 0;

                    if (iChances > 0)
                        bNoChances = false;

                    cChances[pLoc] = iChances;
                }
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

            int iFort = Rnd.ChooseOne(cChances.Values, 2);
            Location pChoosenLocation = cChances.ElementAt(iFort).Key;
            LocationX pChoosenLocationX = pChoosenLocation.GetOwner<LocationX>();
            //Построим форт в выбранной локации.
            //Все локации на 1 шаг вокруг пометим как поля, чтобы там не возникало никаких новых поселений.

            if (pChoosenLocationX.m_pSettlement == null && pChoosenLocationX.m_pBuilding == null)
            {
                LandX pChoosenLandX = pChoosenLocation.GetOwner<Land>().GetOwner<LandX>();
                pChoosenLocationX.m_pSettlement = new Settlement(Settlement.Info[SettlementSize.Fort], pChoosenLandX.m_pDominantNation, m_pProvince.Layers.Get<State>().m_pSociety.m_iTechLevel, m_pProvince.m_pLocalSociety.m_iMagicLimit, false, bFast);

                foreach (LocationX pLoc in pChoosenLocation.m_aBorderWith)
                    if (pLoc.m_pBuilding == null)
                        pLoc.m_pBuilding = new BuildingStandAlone(BuildingType.Farm);

                List<Road> cRoads = new List<Road>();
                foreach (var pRoads in pChoosenLocationX.m_cRoads)
                    cRoads.AddRange(pRoads.Value);

                Road[] aRoads = cRoads.ToArray();
                foreach (Road pRoad in aRoads)
                    World.RenewRoad(pRoad);

                return pChoosenLocation;
            }
            else
                return null;
        }

        public override string ToString()
        {
            return string.Format("{0} {1} ({2}, {3})", m_sName, m_pType.m_sName, Contents.Count, GetNativeRaceString());
        }
    }
}
