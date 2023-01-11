using LandscapeGeneration;
using Random;
using Socium.Nations;
using Socium.Settlements;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Socium
{
    /// <summary>
    /// Регион - группа сопредельных земель (<see cref="LandX"/>) одного типа, населённых одной нацией и имеющих одно название (например. Арденнский Лес).
    /// Регионы объеиняются в провинции (<see cref="Province"/>).
    /// </summary>
    public class Region: TerritoryCluster<Region, Province, LandX>
    {
        public LandTypeInfo Type { get; private set; }

        public bool IsWater
        {
            get { return Type != null && Type.Environment.HasFlag(LandscapeGeneration.Environment.Liquid); }
        }

        public bool IsBorder()
        {
            foreach (var pLand in Contents)
            {
                if (pLand.Origin.IsBorder())
                    return true;
            }

            return false;
        }

        public float MovementCost
        {
            get { return Type == null ? 100 : Type.MovementCost; }
        }

        public Continent Continent
        {
            get
            {
                return LandMass.GetOwner();
            }
        }

        public LandMass LandMass
        {
            get
            {
                return Contents.First().Origin.GetOwner();
            }
        }

        /// <summary>
        /// Военное присутсвие сил провинции в данной земле.
        /// Нужно только для формирования карты провинций.
        /// </summary>
        public int ProvincePresence { get; set; } = 0;

        /// <summary>
        /// коренное население региона - может отличаться от доминирующего населения локации
        /// </summary>
        public Nation Natives { get; set; } = null;

        public string Name { get; set; }

        private int m_iMaxSize = 1;

        /// <summary>
        /// Стартует новый <see cref="Region"/>, добавляя в него указанную <see cref="LandX"/>.<br/>
        /// Заполняет <c>m_cBorder</c>.<br/><c>BorderWith</c> будет заполнен ТОЛЬКО после вызова <c>Finish(float)</c>
        /// </summary>
        /// <param name="pSeed"></param>
        /// <param name="iMaxSize"></param>
        public void Start(LandX pSeed, int iMaxSize)
        {
            BorderWith.Clear();
            Contents.Clear();

            InitBorder(pSeed);

            Contents.Add(pSeed);
            pSeed.SetOwner(this);

            Type = pSeed.Origin.LandType;

            if (pSeed.Origin.IsWater)
                throw new ArgumentException("Region can't be underwater!");

            m_iMaxSize = iMaxSize / Type.MovementCost;
        }

        /// <summary>
        /// Присоединяет к территории сопредельную землю того же типа.
        /// </summary>
        /// <returns></returns>
        public override LandX Grow(int iMaxSize)
        {
            if (Contents.Count > m_iMaxSize && Rnd.OneChanceFrom(Contents.Count - m_iMaxSize))
                return null;

            Dictionary<LandX, float> cBorderLength = new Dictionary<LandX, float>();

            LandX[] aBorderLands = new List<LandX>(Border.Keys).ToArray();
            foreach (LandX pLand in aBorderLands)
            {
                if (pLand.Forbidden)
                    continue;

                if (!pLand.HasOwner() && pLand.Origin.LandType == Type)
                {
                    bool bHavePotential = false;

                    float fWholeLength = 1;
                    VoronoiEdge[] aBorderLine = Border[pLand].ToArray();
                    foreach (var pLine in aBorderLine)
                        fWholeLength += pLine.Length;

                    foreach (var pLinkTerr in pLand.Origin.BorderWith.Keys)
                    {
                        if (pLinkTerr.Forbidden)
                            continue;

                        if (pLinkTerr.LandType == Type && !pLinkTerr.HasOwner())
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

            LandX pAddon = null;

            int iChoice = Rnd.ChooseOne(cBorderLength.Values, 2);
            if (iChoice >= 0)
                pAddon = cBorderLength.ElementAt(iChoice).Key;

            if (pAddon == null)
                return null;

            Contents.Add(pAddon);
            pAddon.SetOwner(this);

            Border[pAddon].Clear();
            Border.Remove(pAddon);

            foreach (var pBorderLand in pAddon.BorderWith)
            {
                if (Contents.Contains(pBorderLand.Key))
                    continue;

                if (!Border.ContainsKey(pBorderLand.Key))
                    Border[pBorderLand.Key] = new List<VoronoiEdge>();
                VoronoiEdge[] cLines = pBorderLand.Value.ToArray();
                foreach (var pLine in cLines)
                    Border[pBorderLand.Key].Add(new VoronoiEdge(pLine));
            }

            return pAddon;
        }

        /// <summary>
        /// Заполняет словари границ с другими <see cref="LandX"/> (т.е. <c>BorderWith</c> и <c>m_aBorderWith</c>).
        /// </summary>
        /// <param name="fCycleShift">Величина смещения X-координаты для закольцованной карты</param>
        public override void Finish(float fCycleShift)
        {
            ChainBorder(fCycleShift);

            foreach (LandX pLand in Border.Keys)
            {
                Region pRegion;
                if (pLand.Forbidden || !pLand.HasOwner())
                    pRegion = m_pForbidden;
                else
                    pRegion = pLand.GetOwner();

                if (!BorderWith.ContainsKey(pRegion))
                    BorderWith[pRegion] = new List<VoronoiEdge>();
                BorderWith[pRegion].AddRange(Border[pLand]);
            }
            FillBorderWithKeys();
        }

        public override float GetMovementCost()
        {
            return Type == null ? 100.0f : Type.MovementCost;
        }

        public void SetRace(List<Nation> cPossibleNations)
        {
            Dictionary<Nation, float> cChances = new Dictionary<Nation, float>();
            foreach (Nation pNation in cPossibleNations)
            {
                cChances[pNation] = 1.0f;

                if (pNation.IsAncient)
                    cChances[pNation] /= 10.0f / Type.MovementCost;

                if (pNation.IsHegemon)
                    cChances[pNation] *= 10.0f;

                foreach (LandTypeInfo pType in pNation.PreferredLands)
                {
                    if (Type == pType)
                        cChances[pNation] *= 10.0f;
                }

                foreach (LandTypeInfo pType in pNation.HatedLands)
                {
                    if (Type == pType)
                        cChances[pNation] /= 100.0f;
                }
            }

            int iChance = Rnd.ChooseOne(cChances.Values, 3);
            foreach (Nation pNation in cChances.Keys)
            {
                iChance--;
                if (iChance < 0)
                {
                    Natives = pNation;
                    break;
                }
            }

            Name = Natives.Race.Language.RandomCountryName();

            foreach (LandX pLand in Contents)
            {
                pLand.Populate(Natives, Name);
            }
        }

        public string GetNativeRaceString()
        {
            if (Natives == null)
                return "unpopulated";
            else
                return Natives.ProtoSociety.Name;
        }

        internal Location BuildFort(State pEnemy, bool bFast)
        {
            //сначала составим список претендентов из числа незанятых локаций, лежащих непосредственно на границе с супостатом (или на берегу, если супостат не определён).
            Dictionary<Location, int> cChances = new Dictionary<Location, int>();
            bool bNoChances = true;
            foreach (LandX pLand in Contents)
            {
                foreach (Location pLoc in pLand.Origin.Contents)
                {
                    bool bCoast = false;
                    bool bBorder = false;
                    bool bMapBorder = false;
                    foreach (Location pLink in pLoc.BorderWithKeys)
                    {
                        Land pLandLink = pLink.GetOwner();
                        if (pLandLink != null)
                        {
                            Region pRegionLink = pLandLink.As<LandX>().GetOwner();
                            if (pRegionLink != null && pRegionLink.HasOwner() && pRegionLink.GetOwner().GetOwner() == pEnemy)
                                bBorder = true;

                            if (pLandLink.IsWater)
                                bCoast = true;
                        }

                        if (pLink.IsBorder)
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

                    LocationX pLocX = pLoc.As<LocationX>();

                    if (pLocX.Settlement != null)
                        iChances = 0;

                    if (pLocX.Building != null)
                        iChances = pLocX.Building.Type == BuildingType.Farm || pLocX.Building.Type == BuildingType.HuntingFields ? 1 : 0;

                    if (pLoc.IsBorder || pLoc.Landmark != LandmarkType.Empty)
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
            LocationX pChoosenLocationX = pChoosenLocation.As<LocationX>();
            //Построим форт в выбранной локации.
            //Все локации на 1 шаг вокруг пометим как поля, чтобы там не возникало никаких новых поселений.

            if (pChoosenLocationX.Settlement == null && pChoosenLocationX.Building == null)
            {
                LandX pChoosenLandX = pChoosenLocation.GetOwner().As<LandX>();
                pChoosenLocationX.Settlement = new Settlement(Settlement.Info[SettlementSize.Fort], pChoosenLandX.DominantNation, GetOwner().GetOwner().Society.TechLevel, GetOwner().LocalSociety.MagicLimit, false, bFast);

                foreach (Location pLoc in pChoosenLocation.BorderWithKeys)
                {
                    if (pLoc.As<LocationX>().Building == null)
                        pLoc.As<LocationX>().Building = new BuildingStandAlone(BuildingType.Farm);
                }

                List<Road> cRoads = new List<Road>();
                foreach (var pRoads in pChoosenLocationX.Roads)
                    cRoads.AddRange(pRoads.Value);

                Road[] aRoads = cRoads.ToArray();
                foreach (Road pRoad in aRoads)
                    World.RenewRoad(pRoad);

                return pChoosenLocation;
            }
            else
            {
                return null;
            }
        }

        public override string ToString()
        {
            return string.Format("{0} {1} ({2}, {3})", Name, Type.Name, Contents.Count, GetNativeRaceString());
        }
    }
}
