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
    public class Region: TerritoryCluster<LandX>
    {
        private static Region m_pForbidden = new Region(true);

        public LandTypeInfoX m_pType;

        public bool IsWater
        {
            get { return m_pType != null && m_pType.m_eEnvironment.HasFlag(LandscapeGeneration.Environment.Liquid); }
        }

        public bool IsBorder()
        {
            foreach (var pLand in m_cContents)
                if (pLand.IsBorder())
                    return true;

            return false;
        }

        public float MovementCost
        {
            get { return m_pType == null ? 100 : m_pType.m_iMovementCost; }
        }

        public ContinentX Continent
        {
            get 
            {
                return (Owner as LandMass<LandX>).Owner as ContinentX;
            }
        }

        public Province m_pProvince = null;

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
        
        public void Start(LandX pSeed, int iMaxSize)
        {
            BorderWith.Clear();
            m_cContents.Clear();

            InitBorder(pSeed);

            m_cContents.Add(pSeed);
            pSeed.Region = this;

            m_pType = pSeed.Type;

            m_iMaxSize = iMaxSize / m_pType.m_iMovementCost;
        }

        /// <summary>
        /// Присоединяет к территории сопредельную землю того же типа.
        /// </summary>
        /// <returns></returns>
        public override ITerritory Grow(int iMaxSize)
        {
            if (m_cContents.Count > m_iMaxSize && Rnd.OneChanceFrom(m_cContents.Count - m_iMaxSize))
                return null;

            //List<LAND> cBorder = new List<LAND>();

            Dictionary<LandX, float> cBorderLength = new Dictionary<LandX, float>();

            ITerritory[] aBorderLands = new List<ITerritory>(m_cBorder.Keys).ToArray();
            foreach (ITerritory pTerr in aBorderLands)
            {
                if (pTerr.Forbidden)
                    continue;

                LandX pLand = pTerr as LandX;

                if (pLand.Region == null && pLand.Type == m_pType)
                {
                    bool bHavePotential = false;

                    float fWholeLength = 1;
                    Location.Edge[] aBorderLine = m_cBorder[pLand].ToArray();
                    foreach (var pLine in aBorderLine)
                        fWholeLength += pLine.m_fLength;

                    foreach (var pLinkTerr in pLand.BorderWith)
                    {
                        if ((pLinkTerr.Key as ITerritory).Forbidden)
                            continue;

                        if ((pLinkTerr.Key as LandX).Type == m_pType &&
                            (pLinkTerr.Key as LandX).Region == null)
                            bHavePotential = true;
                    }

                    fWholeLength /= pLand.PerimeterLength;

                    if (fWholeLength < 0.25f && m_cContents.Count > 1 && bHavePotential)
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

            LandX pAddon = null;

            int iChoice = Rnd.ChooseOne(cBorderLength.Values, 2);
            LandX[] aBorderLength = new List<LandX>(cBorderLength.Keys).ToArray();
            foreach (LandX pInner in aBorderLength)
            {
                iChoice--;
                if (iChoice < 0)
                {
                    pAddon = pInner;
                    break;
                }
            }

            if (pAddon == null)
                return null;

            m_cContents.Add(pAddon);
            pAddon.Region = this;

            m_cBorder[pAddon].Clear();
            m_cBorder.Remove(pAddon);

            foreach (var pBorderLand in pAddon.BorderWith)
            {
                if (m_cContents.Contains(pBorderLand.Key))
                    continue;

                if (!m_cBorder.ContainsKey(pBorderLand.Key))
                    m_cBorder[pBorderLand.Key] = new List<Location.Edge>();
                Location.Edge[] cLines = pBorderLand.Value.ToArray();
                foreach (var pLine in cLines)
                    m_cBorder[pBorderLand.Key].Add(new Location.Edge(pLine));
            }

            //ChainBorder();

            return pAddon;
        }

        public override void Finish(float fCycleShift)
        {
            //base.Finish();
            ChainBorder(fCycleShift);

            foreach (ITerritory pLand in m_cBorder.Keys)
            {
                Region pRegion;
                if (pLand.Forbidden || (pLand as LandX).Region == null)
                    pRegion = Region.m_pForbidden;
                else
                    pRegion = (pLand as LandX).Region;

                if (!BorderWith.ContainsKey(pRegion))
                    BorderWith[pRegion] = new List<Location.Edge>();
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

                foreach (LandTypeInfoX pType in pNation.m_aPreferredLands)
                    if (m_pType == pType)
                        cChances[pNation] *= 10;

                foreach (LandTypeInfoX pType in pNation.m_aHatedLands)
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

            foreach (LandX pLand in m_cContents)
            {
                pLand.Populate(m_pNatives, m_sName);
            }
        }

        public string GetNativeRaceString()
        {
            if (m_pNatives == null)
                return "unpopulated";
            else
                return m_pNatives.m_pProtoSociety.m_sName;
        }

        internal LocationX BuildFort(State pEnemy, bool bFast)
        {
            //сначала составим список претендентов из числа незанятых локаций, лежащих непосредственно на границе с супостатом (или на берегу, если супостат не определён).
            Dictionary<LocationX, int> cChances = new Dictionary<LocationX, int>();
            bool bNoChances = true;
            foreach (LandX pLand in m_cContents)
            {
                foreach (LocationX pLoc in pLand.m_cContents)
                {
                    bool bCoast = false;
                    bool bBorder = false;
                    bool bMapBorder = false;
                    foreach (LocationX pLink in pLoc.m_aBorderWith)
                    {
                        if (pLink.Owner != null)
                        {
                            LandX pLandLink = pLink.Owner as LandX;
                            if (pLandLink.Region != null && pLandLink.Region.m_pProvince != null && pLandLink.Region.m_pProvince.Owner == pEnemy)
                                bBorder = true;
                        }
                        if (pLink.Owner != null && (pLink.Owner as LandX).IsWater)
                            bCoast = true;

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

                    if (pLoc.m_pSettlement != null)
                        iChances = 0;

                    if (pLoc.m_pBuilding != null)
                        iChances = pLoc.m_pBuilding.m_eType == BuildingType.Farm || pLoc.m_pBuilding.m_eType == BuildingType.HuntingFields ? 1 : 0;

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
            LocationX pChoosenLocation = cChances.ElementAt(iFort).Key;
            //Построим форт в выбранной локации.
            //Все локации на 1 шаг вокруг пометим как поля, чтобы там не возникало никаких новых поселений.

            if (pChoosenLocation.m_pSettlement == null && pChoosenLocation.m_pBuilding == null)
            {
                pChoosenLocation.m_pSettlement = new Settlement(Settlement.Info[SettlementSize.Fort], (pChoosenLocation.Owner as LandX).m_pDominantNation, m_pProvince.OwnerState.m_pSociety.m_iTechLevel, m_pProvince.m_pLocalSociety.m_iMagicLimit, false, bFast);

                foreach (LocationX pLoc in pChoosenLocation.m_aBorderWith)
                    if (pLoc.m_pBuilding == null)
                        pLoc.m_pBuilding = new BuildingStandAlone(BuildingType.Farm);

                List<Road> cRoads = new List<Road>();
                foreach (var pRoads in pChoosenLocation.m_cRoads)
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
            return string.Format("{0} {1} ({2}, {3})", m_sName, m_pType.m_sName, m_cContents.Count, GetNativeRaceString());
        }
    }
}
