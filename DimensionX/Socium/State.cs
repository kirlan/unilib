using System;
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
    public class State: BorderBuilder<Province>, ITerritory
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

        public static Infrastructure[] InfrastructureLevels = 
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
        
        private static State m_pForbidden = new State();

        public List<Province> m_cContents = new List<Province>();

        #region ITerritory members
        /// <summary>
        /// границы с другими государствами
        /// </summary>
        public Dictionary<object, List<Location.Edge>> BorderWith { get; } = new Dictionary<object, List<Location.Edge>>();

        public bool Forbidden
        {
            get { return this == State.m_pForbidden; }
        }

        public object Owner { get; set; } = null;

        public float PerimeterLength { get; private set; } = 0;
        #endregion ITerritory members

        /// <summary>
        /// соседние государствами (включая те, с которыми только морское сообщение)
        /// </summary>
        public object[] m_aBorderWith = null;

        internal void FillBorderWithKeys()
        {
            m_aBorderWith = new List<object>(BorderWith.Keys).ToArray();

            PerimeterLength = 0;
            foreach (var pBorder in BorderWith)
                foreach (Location.Edge pLine in pBorder.Value)
                    PerimeterLength += pLine.m_fLength;
        }

        public Province m_pMethropoly = null;

        public StateSociety m_pSociety = null;

        /// <summary>
        /// Зарождение государства в указанной провинции
        /// </summary>
        /// <param name="pSeed"></param>
        public override void Start(Province pSeed)
        {
            if (pSeed.Owner != null)
                throw new Exception("This province already belongs to state!!!");

            BorderWith.Clear();
            m_cContents.Clear();

            base.Start(pSeed);

            m_pMethropoly = pSeed;

            m_pSociety = new StateSociety(this);

            m_cContents.Add(pSeed);
            pSeed.Owner = this;
        }

        /// <summary>
        /// Присоеденять ничейные провинции вокруг, не учитывая уровень претензий конкурентов, до тех пор, пока рядом будут ничейные провинции.
        /// </summary>
        /// <returns></returns>
        public bool ForcedGrow()
        {
            object[] aBorder = new List<object>(m_cBorder.Keys).ToArray();

            bool bFullyGrown = true;

            foreach (ITerritory pTerr in aBorder)
            {
                if (pTerr.Forbidden)
                    continue;

                Province pProvince = pTerr as Province;

                if (pProvince != null && pProvince.Owner == null && !pProvince.m_pCenter.IsWater && 
                    m_pMethropoly.m_pLocalSociety.m_pTitularNation.m_pRace.m_pLanguage == pProvince.m_pLocalSociety.m_pTitularNation.m_pRace.m_pLanguage)
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
        public bool Grow(int iMaxStateSize)
        {
            //если государство уже достаточно большое - сваливаем.
            if (m_cContents.Count > iMaxStateSize)
                return false;

            Dictionary<Province, float> cChances = new Dictionary<Province, float>();

            foreach (ITerritory pTerr in m_cBorder.Keys)
            {
                if (pTerr.Forbidden)
                    continue;

                Province pProvince = pTerr as Province;

                if (pProvince != null && pProvince.Owner == null && !pProvince.m_pCenter.IsWater)
                {
                    if (m_pMethropoly.m_pLocalSociety.m_pTitularNation.m_pRace.m_pLanguage != pProvince.m_pLocalSociety.m_pTitularNation.m_pRace.m_pLanguage)
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
                    foreach (var pLine in m_cBorder[pProvince])
                        fSharedPerimeter += pLine.m_fLength;

                    fSharedPerimeter /= pProvince.PerimeterLength;

                    if (fSharedPerimeter < 0.15f)
                        continue;

                    //дружественное отношение - для этой провинции шансы выше.
                    //if (iHostility < -1)
                    //    fSharedPerimeter *= -iHostility;
                    if (m_pMethropoly.m_pLocalSociety.m_pTitularNation == pProvince.m_pLocalSociety.m_pTitularNation)
                        fSharedPerimeter *= 2;

                    cChances[pProvince] = fSharedPerimeter;
                }
            }

            Province pAddon = null;

            int iChoice = Rnd.ChooseOne(cChances.Values, 2);

            if (iChoice < 0)
                return false;

            foreach (Province pProvince in cChances.Keys)
            {
                iChoice--;
                if (iChoice < 0)
                {
                    pAddon = pProvince;
                    break;
                }
            }

            if (pAddon == null)
                return false;

            //if (!Rnd.OneChanceFrom(1 + m_iPower * m_iPower))
            //if (Rnd.OneChanceFrom(1 + pAddon.m_pCenter.Type.m_iMovementCost * pAddon.m_pCenter.Type.m_iMovementCost))
            //    return true;

            //foreach (LandTypeInfoX pType in m_pRace.m_cHatedLands)
            //    if (pType == pAddon.m_pCenter.Type && !Rnd.OneChanceFrom(5))
            //        return true;

            AddProvince(pAddon);

            return true;
        }

        private void AddProvince(Province pAddon)
        {
            m_cContents.Add(pAddon);
            pAddon.Owner = this;

            //List<Line> cListLine = m_cBorder[pAddon];
            m_cBorder[pAddon].Clear();
            m_cBorder.Remove(pAddon);

            //List<Line> cNewBorder = new List<Line>();
            //List<Line> cFalseBorder = new List<Line>();
            foreach (var pLand in pAddon.BorderWith)
            {
                ITerritory pL = pLand.Key as ITerritory;

                if (!pL.Forbidden && m_cContents.Contains(pL))
                {
                    //foreach (Line pLine in pLand.Value)
                    //    cFalseBorder.Add(new Line(pLine));
                    continue;
                }

                if (!m_cBorder.ContainsKey(pL))
                    m_cBorder[pL] = new List<Location.Edge>();
                Location.Edge[] cLines = pLand.Value.ToArray();
                foreach (var pLine in cLines)
                {
                    m_cBorder[pL].Add(new Location.Edge(pLine));
                    //cNewBorder.Add(new Line(pLine));
                }
            }

            //TestChain();

            //if (cListLine.Count != cFalseBorder.Count && cFalseBorder.Count > 0)
            //{
            //    Line[] aListLine = SortLines(cListLine);
            //    Line[] aListLine2 = SortLines(cNewBorder);
            //    Line[] aListLine3 = SortLines(cFalseBorder);
            //}
        }

        //private Location.Edge[] SortLines(List<Location.Edge> cListLine)
        //{
        //    Location.Edge[] aListLine = new Location.Edge[cListLine.Count];
        //    int iIndex = -1;
        //    do
        //    {
        //        foreach (Location.Edge pLine in cListLine)
        //        {
        //            if (iIndex < 0)
        //            {
        //                bool bPrevious = false;
        //                foreach (Location.Edge pLine2 in cListLine)
        //                    if (pLine2.m_pPoint2.Y == pLine.m_pPoint1.Y)
        //                        bPrevious = true;

        //                if (!bPrevious)
        //                    aListLine[++iIndex] = pLine;
        //            }
        //            else
        //            {
        //                if (pLine.m_pPoint1.Y == aListLine[iIndex].m_pPoint2.Y)
        //                    aListLine[++iIndex] = pLine;
        //            }
        //        }
        //    }
        //    while (iIndex < cListLine.Count - 1);

        //    return aListLine;
        //}

        /// <summary>
        /// Заполняет словарь границ с другими странами и гарантирует принадлежность государства той расе, которая доминирует на его территории.
        /// </summary>
        public void Finish(float fCycleShift)
        {
            ChainBorder(fCycleShift);

            BorderWith.Clear();

            //добавляем в общий список контуры границ с соседними государствами
            foreach (ITerritory pProvince in m_cBorder.Keys)
            {
                State pState;
                if (pProvince.Forbidden || (pProvince as Province).Owner == null)
                    pState = State.m_pForbidden;
                else
                    pState = (pProvince as Province).Owner as State;

                if (!BorderWith.ContainsKey(pState))
                    BorderWith[pState] = new List<Location.Edge>();
                BorderWith[pState].AddRange(m_cBorder[pProvince]);
            }

            //добавляем в общий список пустые массивы контуров границ для тех государств, с которыми у нас 
            //морское сообщение
            foreach (Province pProvince in m_cContents)
            {
                foreach (LocationX pLoc in pProvince.m_pLocalSociety.Settlements)
                    foreach (LocationX pOtherLoc in pLoc.m_cHaveSeaRouteTo)
                    { 
                        State pState = pOtherLoc.OwnerState;
                        if(pState != this && !BorderWith.ContainsKey(pState))
                            BorderWith[pState] = new List<Location.Edge>();
                    }
            }

            FillBorderWithKeys();
        }

        public int m_iFood = 0;
        public int m_iOre = 0;
        public int m_iWood = 0;
        public int m_iPopulation = 0;

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

            m_pMethropoly.m_pLocalSociety.UpdateTitularNation(m_pSociety.m_pTitularNation);
            foreach (LandX pLand in m_pMethropoly.m_cContents)
                pLand.m_pNation = m_pSociety.m_pTitularNation;

            // если у нас нация паразитов или пришельцев, то они сами живут только в метрополии, а во всей остальной империи коренное население - местное
            if (m_pSociety.m_pTitularNation.DominantPhenotype.m_pValues.Get<NutritionGenetix>().IsParasite() || m_pSociety.m_pTitularNation.m_bInvader)
            {
                foreach (Province pProvince in m_cContents)
                {
                    foreach (LandX pLand in pProvince.m_cContents)
                    {
                        if (pLand.m_pNation == m_pSociety.m_pTitularNation)
                            pLand.m_pNation = m_pSociety.m_pHostNation;
                    }
                }
            }
            //int iAverageMagicLimit = 0;

            m_iFood = 0;
            m_iWood = 0;
            m_iOre = 0;
            m_iPopulation = 0;

            #region Counting resources production and consumption
            int iGrain = 0;
            int iGame = 0;
            int iFish = 0;
            int iMethropolyPopulation = 0;
            int iProvincialPopulation = 0;

            foreach (Province pProvince in m_cContents)
            {
                foreach (LocationX pLoc in pProvince.m_pLocalSociety.Settlements)
                {
                    foreach (LocationX pOtherLoc in pLoc.m_cHaveSeaRouteTo)
                    {
                        State pState = pOtherLoc.OwnerState;
                        if (pState != this && !BorderWith.ContainsKey(pState))
                            BorderWith[pState] = new List<Location.Edge>();
                    }
                }
                
                //m_iFood += (int)(pProvince.m_fGrain + pProvince.m_fFish + pProvince.m_fGame);
                iGrain += (int)pProvince.m_fGrain;
                iGame += (int)pProvince.m_fGame;
                iFish += (int)pProvince.m_fFish; 
                m_iWood += (int)pProvince.m_fWood;
                m_iOre += (int)pProvince.m_fOre;

                if (pProvince == m_pMethropoly)
                    iMethropolyPopulation += pProvince.m_iPopulation;
                else
                    iProvincialPopulation += pProvince.m_iPopulation;

                //foreach (LandX pLand in pProvince.m_cContents)
                //{
                //    iAverageMagicLimit += pProvince.m_pNation.m_iMagicLimit * pLand.m_cContents.Count;
                //}
            }

            m_iPopulation = iMethropolyPopulation + iProvincialPopulation;

            int getFood(NutritionType eNutritionType)
            {
                switch (eNutritionType)
                {
                    case NutritionType.Eternal:
                        return m_iPopulation * 10;
                    case NutritionType.Mineral:
                        return m_iOre;
                    case NutritionType.Organic:
                        return iGrain + iGame + iFish;
                    case NutritionType.ParasitismBlood:
                        return iProvincialPopulation;
                    case NutritionType.ParasitismEmote:
                        return iProvincialPopulation;
                    case NutritionType.ParasitismEnergy:
                        return iProvincialPopulation;
                    case NutritionType.ParasitismMeat:
                        return iProvincialPopulation;
                    case NutritionType.Photosynthesis:
                        return m_iPopulation * 10;
                    case NutritionType.Thermosynthesis:
                        return m_iPopulation * 10;
                    case NutritionType.Vegetarian:
                        return iGrain;
                    case NutritionType.Carnivorous:
                        return iGame + iFish;
                    default:
                        throw new Exception(string.Format("Unknown Nutrition type: {0}", m_pSociety.m_pTitularNation.DominantPhenotype.m_pValues.Get<NutritionGenetix>().NutritionType));
                }
            }

            float fMethropolySatiety = (float)getFood(m_pSociety.m_pTitularNation.DominantPhenotype.m_pValues.Get<NutritionGenetix>().NutritionType) / iMethropolyPopulation;
            float fProvincialSatiety = (float)getFood(m_pSociety.m_pHostNation.DominantPhenotype.m_pValues.Get<NutritionGenetix>().NutritionType) / iProvincialPopulation;

            if (fMethropolySatiety > 2)
                fMethropolySatiety = 2;

            if (m_pSociety.m_pTitularNation.DominantPhenotype.m_pValues.Get<NutritionGenetix>().IsParasite())
            {
                // для паразитов качество их питания зависит от качества питания их жертв
                m_iFood = (int)(fMethropolySatiety * fProvincialSatiety * m_iPopulation);
            }
            else
            {
                // для обычных людей - качество питания в метрополии и в провинции просто дополняют друг-друга
                m_iFood = (int)((fMethropolySatiety + fProvincialSatiety) * 0.5 * m_iPopulation);
            }

            //iAverageMagicLimit = iAverageMagicLimit / m_iPopulation;
            #endregion Counting resources production and consumption

            m_pSociety.CalculateSocietyFeatures(iEmpireTreshold);

            #region Build capital and regional centers
            m_pMethropoly.m_pAdministrativeCenter.m_pSettlement = new Settlement(m_pSociety.m_pStateModel.m_pStateCapital, 
                m_pMethropoly.m_pLocalSociety.m_pTitularNation, 
                m_pSociety.m_iTechLevel, 
                m_pSociety.m_iMagicLimit, 
                true, 
                bFast);

            foreach (Province pProvince in m_cContents)
            {
                m_pSociety.Settlements.AddRange(pProvince.m_pLocalSociety.Settlements);

                if (pProvince == m_pMethropoly)
                    continue;

                pProvince.m_pAdministrativeCenter.m_pSettlement = new Settlement(m_pSociety.m_pStateModel.m_pProvinceCapital, 
                    m_pMethropoly.m_pLocalSociety.m_pTitularNation, 
                    m_pSociety.m_iTechLevel, 
                    m_pSociety.m_iMagicLimit, 
                    false, 
                    bFast);
            }
            #endregion Build capital and regional centers

            if (m_pMethropoly.m_pCenter.Area != null)
                m_pSociety.m_sName = m_pSociety.m_pTitularNation.m_pRace.m_pLanguage.RandomCountryName();

            return m_pMethropoly.m_pAdministrativeCenter;
        }

        public State[] GetEnemiesList()
        {
            List<State> cList = new List<State>();
 
            foreach (ITerritory pTerr in m_aBorderWith)
            {
                if (pTerr.Forbidden)
                    continue;

                State pState = pTerr as State;
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

            foreach (ITerritory pTerr in m_aBorderWith)
            {
                if (pTerr.Forbidden)
                    continue;

                State pState = pTerr as State;
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
            if (!InfrastructureLevels[m_pSociety.m_iInfrastructureLevel].m_cAvailableSettlements.Contains(SettlementSize.Fort))
                return;

            foreach (Province pProvince in m_cContents)
                foreach (LandX pLand in pProvince.m_cContents)
                {
                    float fTreat = 0;
                    float fBorder = 0;

                    int iMaxHostility = 0;
                    State pMainEnemy = null;

                    foreach (var pLinkedTerr in pLand.BorderWith)
                    {
                        if((pLinkedTerr.Key as ITerritory).Forbidden)
                            continue;

                        LandX pLinkedLand = pLinkedTerr.Key as LandX;

                        if (pLinkedLand.m_pProvince != null && pLinkedLand.m_pProvince.Owner == this)
                            continue;

                        int iHostility = 0;
                        if (pLinkedLand.m_pProvince != null)
                        {
                            State pLinkedState = pLinkedLand.m_pProvince.OwnerState;

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

                        Location.Edge[] cLines = pLinkedTerr.Value.ToArray();
                        foreach (var pLine in cLines)
                        {
                            fBorder += pLine.m_fLength / pLinkedLand.MovementCost;
                            if (pLinkedLand.m_pProvince == null)
                                fTreat += pLine.m_fLength / pLinkedLand.MovementCost;
                            else
                                fTreat += pLine.m_fLength * (float)Math.Sqrt(iHostility) / pLinkedLand.MovementCost;
                        }
                    }

                    if (fTreat == 0)
                        continue;

                    if (Rnd.ChooseOne(fTreat, fBorder))// - fTreat))
                        //if (m_iSize > 1 || Rnd.OneChanceFrom(2))
                        {
                            LocationX pFort = pLand.BuildFort(pMainEnemy, bFast);
                            if (pFort != null)
                            {
                                pLand.m_pProvince.m_pLocalSociety.Settlements.Add(pFort);
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
            RoadQuality eRoadLevel = State.InfrastructureLevels[m_pSociety.m_iInfrastructureLevel].m_eMaxGroundRoad;

            if (eRoadLevel == RoadQuality.None)
                return;

            //Закрываем границы, чтобы нельзя было "срезать" путь по чужой территории
            foreach (Province pProvince in m_cContents)
                foreach (LandX pLand in pProvince.m_cContents)
                    foreach (LocationX pLoc in pLand.m_cContents)
                        foreach (var pLinked in pLoc.m_cLinks)
                        {
                            if (pLinked.Key is LocationX)
                            {
                                LandX pLinkedOwner = (pLinked.Key as LocationX).Owner as LandX;
                                if (pLinkedOwner.m_pProvince == null || pLinkedOwner.m_pProvince.Owner != this || pLinked.Value.Sea)
                                    pLoc.m_cLinks[pLinked.Key].m_bClosed = true;
                            }
                            else
                                pLoc.m_cLinks[pLinked.Key].m_bClosed = true;
                        }

            //Сначала соеденим все центры провинций
            List<LocationX> cConnected = new List<LocationX>();
            cConnected.Add(m_pMethropoly.m_pAdministrativeCenter);

            while (cConnected.Count < m_cContents.Count)
            {
                //Road pBestRoad = null;
                LocationX pBestTown1 = null;
                LocationX pBestTown2 = null;
                float fMinLength = float.MaxValue;

                foreach (Province pProvince in m_cContents)
                {
                    LocationX pTown = pProvince.m_pAdministrativeCenter;

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

            //теперь займёмся фортами
            cConnected.Clear();
            cConnected.Add(m_pMethropoly.m_pAdministrativeCenter);

            List<LocationX> cSettlements = new List<LocationX>();
            foreach (Province pProvince in m_cContents)
                cSettlements.AddRange(pProvince.m_pLocalSociety.Settlements);
            LocationX[] aSettlements = cSettlements.ToArray();

            eRoadLevel = RoadQuality.Normal;
            if (eRoadLevel > State.InfrastructureLevels[m_pSociety.m_iInfrastructureLevel].m_eMaxGroundRoad)
                eRoadLevel = State.InfrastructureLevels[m_pSociety.m_iInfrastructureLevel].m_eMaxGroundRoad;

            List<LocationX> cForts = new List<LocationX>();
            foreach (LocationX pTown in aSettlements)
            {
                if (!cConnected.Contains(pTown) && (pTown.m_cRoads[RoadQuality.Normal].Count > 0 || pTown.m_cRoads[RoadQuality.Good].Count > 0))
                    cConnected.Add(pTown);
                else
                    if (pTown.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Fort)
                        cForts.Add(pTown);
            }
            LocationX[] aForts = cForts.ToArray();

            foreach (LocationX pFort in aForts)
            {
                LocationX pBestTown = null;
                float fMinLength = float.MaxValue;

                foreach (LocationX pOtherTown in cConnected)
                {
                    float fDist = pFort.DistanceTo(pOtherTown, fCycleShift);// (float)Math.Sqrt((pTown.X - pOtherTown.X) * (pTown.X - pOtherTown.X) + (pTown.Y - pOtherTown.Y) * (pTown.Y - pOtherTown.Y));

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
                        float fDist = pFort.DistanceTo(pOtherTown, fCycleShift);// (float)Math.Sqrt((pBestTown1.X - pOtherTown.X) * (pBestTown1.X - pOtherTown.X) + (pBestTown1.Y - pOtherTown.Y) * (pBestTown1.Y - pOtherTown.Y));

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
            foreach (Province pProvince in m_cContents)
                foreach (LandX pLand in pProvince.m_cContents)
                    foreach (LocationX pLoc in pLand.m_cContents)
                        foreach (TransportationNode pLink in pLoc.m_cLinks.Keys)
                            pLoc.m_cLinks[pLink].m_bClosed = false;
        }

        public void SpecializeSettlements()
        {
            foreach (Province pProvince in m_cContents)
            {
                foreach (LocationX pLoc in pProvince.m_pLocalSociety.Settlements)
                {
                    if (pLoc.m_pSettlement.m_eSpeciality != SettlementSpeciality.None)
                        continue;

                    bool bCoast = false;
                    foreach (LocationX pLink in pLoc.m_aBorderWith)
                    {
                        if (pLink.Owner != null && (pLink.Owner as LandX).IsWater)
                            bCoast = true;
                    }

                    LandX pLand = pLoc.Owner as LandX;

                    switch (pLoc.m_pSettlement.m_pInfo.m_eSize)
                    {
                        #region case SettlementSize.Hamlet
                        case SettlementSize.Hamlet:
                            if (bCoast)
                            {
                                pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Fishers;
                            }
                            else
                            {
                                List<float> cResources = new List<float>();
                                cResources.Add(pLand.GetResource(LandTypeInfoX.Resource.Grain));
                                cResources.Add(pLand.GetResource(LandTypeInfoX.Resource.Game));

                                //в развитом обществе охота - это уже не способ добычи пищи, а больше развлечение
                                if (m_pSociety.m_iInfrastructureLevel > 2)
                                {
                                    cResources[0] += cResources[1];
                                    cResources[1] = 0;
                                }

                                if (m_pSociety.m_iInfrastructureLevel > 1)
                                {
                                    cResources.Add(pLand.GetResource(LandTypeInfoX.Resource.Ore));
                                    cResources.Add(pLand.GetResource(LandTypeInfoX.Resource.Wood));
                                }

                                int iChoosen = Rnd.ChooseOne(cResources, 2);

                                switch (iChoosen)
                                {
                                    case 0:
                                        pLoc.m_pSettlement.m_eSpeciality = m_pSociety.m_iInfrastructureLevel >= 4 ? SettlementSpeciality.Farmers : SettlementSpeciality.Peasants;
                                        break;
                                    case 1:
                                        pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Hunters;
                                        break;
                                    case 2:
                                        pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Miners;
                                        break;
                                    case 3:
                                        pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Lumberjacks;
                                        break;
                                }
                            }
                            break;
                        #endregion
                        #region case SettlementSize.Village
                        case SettlementSize.Village:
                            if (bCoast)
                            {
                                pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Fishers;
                            }
                            else
                            {
                                List<float> cResources = new List<float>();
                                cResources.Add(pLand.GetResource(LandTypeInfoX.Resource.Grain));
                                cResources.Add(pLand.GetResource(LandTypeInfoX.Resource.Game));
                                cResources.Add(pLand.GetResource(LandTypeInfoX.Resource.Ore));
                                cResources.Add(pLand.GetResource(LandTypeInfoX.Resource.Wood));

                                //в развитом обществе охота - это уже не способ добычи пищи, а больше развлечение
                                if (m_pSociety.m_iInfrastructureLevel > 2)
                                {
                                    cResources[0] += cResources[1];
                                    cResources[1] = 0;
                                }

                                int iChoosen = Rnd.ChooseOne(cResources, 2);

                                switch (iChoosen)
                                {
                                    case 0:
                                        pLoc.m_pSettlement.m_eSpeciality = m_pSociety.m_iInfrastructureLevel >= 4 ? SettlementSpeciality.Farmers : SettlementSpeciality.Peasants;
                                        break;
                                    case 1:
                                        pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Hunters;
                                        break;
                                    case 2:
                                        pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Miners;
                                        break;
                                    case 3:
                                        pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Lumberjacks;
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
                                    pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Naval;
                                else
                                    pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Fishers;
                            }
                            else
                            {
                                List<float> cResources = new List<float>();
                                //в толерантном обществе специализация городов выбирается исходя из общего состояния ресурсов в государстве,
                                //а в эгоистичном обществе - в каждой провинции свои приоритеты
                                if (m_pSociety.DominantCulture.GetTrait(Trait.Fanaticism) < 1 + Rnd.Get(1f))
                                {
                                    cResources.Add(m_iFood);
                                    cResources.Add(m_iOre);
                                    cResources.Add(m_iWood);
                                }
                                else
                                {
                                    cResources.Add(pProvince.m_fGame + pProvince.m_fGrain);
                                    cResources.Add(pProvince.m_fOre);
                                    cResources.Add(pProvince.m_fWood);
                                }

                                int iChoosen = Rnd.ChooseOne(cResources, 2);

                                switch (iChoosen)
                                {
                                    case 0:
                                        pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Tailors;
                                        break;
                                    case 1:
                                        pLoc.m_pSettlement.m_eSpeciality = Rnd.OneChanceFrom(3) ? SettlementSpeciality.Jevellers : SettlementSpeciality.Factory;
                                        break;
                                    case 2:
                                        pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Artisans;
                                        break;
                                }
                            }
                            break;
                        #endregion
                        #region case SettlementSize.City
                        case SettlementSize.City:
                            if (bCoast && Rnd.OneChanceFrom(2))
                                pLoc.m_pSettlement.m_eSpeciality = Rnd.OneChanceFrom(3) ? SettlementSpeciality.NavalAcademy : SettlementSpeciality.Naval;
                            else
                            {
                                if (bCoast && m_pSociety.m_iInfrastructureLevel > 2 && m_pSociety.DominantCulture.GetTrait(Trait.Simplicity) > 1 + Rnd.Get(1f))
                                    pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Resort;
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
                                        if (m_pSociety.DominantCulture.m_pCustoms.Has(Customs.MindSet.Balanced_mind))
                                            fScience = 0.25f;
                                        else if (m_pSociety.DominantCulture.m_pCustoms.Has(Customs.MindSet.Logic))
                                            fScience = 0.5f;

                                        if (m_pSociety.m_pTitularNation.DominantPhenotype.m_pValues.Get<BrainGenetix>().Intelligence == Intelligence.Sapient)
                                            fScience *= 2;
                                        if (m_pSociety.m_pTitularNation.DominantPhenotype.m_pValues.Get<BrainGenetix>().Intelligence == Intelligence.Ingenious)
                                            fScience *= 4;

                                        cResources.Add(fScience);
                                        //cResources.Add(2 - m_pCulture.MentalityValues[Mentality.Selfishness][m_iInfrastructureLevel]);

                                        int iChoosen = Rnd.ChooseOne(cResources, 2);

                                        switch (iChoosen)
                                        {
                                            case 0:
                                                pLoc.m_pSettlement.m_eSpeciality = Rnd.OneChanceFrom(3) ? SettlementSpeciality.Cultural : SettlementSpeciality.ArtsAcademy;
                                                break;
                                            case 1:
                                                pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Religious;
                                                break;
                                            case 2:
                                                pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.MilitaryAcademy;
                                                break;
                                            case 3:
                                                pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Gambling;
                                                break;
                                            case 4:
                                                pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.SciencesAcademy;
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
                                            cResources.Add(m_iOre);
                                            cResources.Add(m_iWood);
                                        }
                                        else
                                        {
                                            cResources.Add(pProvince.m_fGame + pProvince.m_fGrain);
                                            cResources.Add(pProvince.m_fOre);
                                            cResources.Add(pProvince.m_fWood);
                                        }

                                        int iChoosen = Rnd.ChooseOne(cResources, 2);

                                        switch (iChoosen)
                                        {
                                            case 0:
                                                pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Tailors;
                                                break;
                                            case 1:
                                                pLoc.m_pSettlement.m_eSpeciality = Rnd.OneChanceFrom(3) ? SettlementSpeciality.Jevellers : SettlementSpeciality.Factory;
                                                break;
                                            case 2:
                                                pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Artisans;
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
                                pLoc.m_pSettlement.m_eSpeciality = Rnd.OneChanceFrom(3) ? SettlementSpeciality.NavalAcademy : SettlementSpeciality.Naval;
                            else
                            {
                                if (bCoast && m_pSociety.m_iInfrastructureLevel > 2 && m_pSociety.DominantCulture.GetTrait(Trait.Simplicity) > 1 + Rnd.Get(1f))
                                    pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Resort;
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
                                        if (m_pSociety.DominantCulture.m_pCustoms.Has(Customs.MindSet.Balanced_mind))
                                            fScience = 0.25f;
                                        else if (m_pSociety.DominantCulture.m_pCustoms.Has(Customs.MindSet.Logic))
                                            fScience = 0.5f;

                                        if (m_pSociety.m_pTitularNation.DominantPhenotype.m_pValues.Get<BrainGenetix>().Intelligence == Intelligence.Sapient)
                                            fScience *= 2;
                                        if (m_pSociety.m_pTitularNation.DominantPhenotype.m_pValues.Get<BrainGenetix>().Intelligence == Intelligence.Ingenious)
                                            fScience *= 4;

                                        cResources.Add(fScience);
                                        //cResources.Add(2 - m_pCulture.MentalityValues[Mentality.Selfishness][m_iInfrastructureLevel]);

                                        int iChoosen = Rnd.ChooseOne(cResources, 2);

                                        switch (iChoosen)
                                        {
                                            case 0:
                                                pLoc.m_pSettlement.m_eSpeciality = Rnd.OneChanceFrom(3) ? SettlementSpeciality.Cultural : SettlementSpeciality.ArtsAcademy;
                                                break;
                                            case 1:
                                                pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Religious;
                                                break;
                                            case 2:
                                                pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.MilitaryAcademy;
                                                break;
                                            case 3:
                                                pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Gambling;
                                                break;
                                            case 4:
                                                pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.SciencesAcademy;
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
                                            cResources.Add(m_iOre);
                                            cResources.Add(m_iWood);
                                        }
                                        else
                                        {
                                            cResources.Add(pProvince.m_fGame + pProvince.m_fGrain);
                                            cResources.Add(pProvince.m_fOre);
                                            cResources.Add(pProvince.m_fWood);
                                        }

                                        int iChoosen = Rnd.ChooseOne(cResources, 2);

                                        switch (iChoosen)
                                        {
                                            case 0:
                                                pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Tailors;
                                                break;
                                            case 1:
                                                pLoc.m_pSettlement.m_eSpeciality = Rnd.OneChanceFrom(3) ? SettlementSpeciality.Jevellers : SettlementSpeciality.Factory;
                                                break;
                                            case 2:
                                                pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Artisans;
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
                                    pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Pirates;
                                else
                                    pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Naval;
                            }
                            else
                            {
                                if (m_pSociety.DominantCulture.GetTrait(Trait.Agression) > 1.5 &&
                                    m_pSociety.DominantCulture.GetTrait(Trait.Treachery) > 1.5)
                                    pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Raiders;
                                else
                                    pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Military;
                            }
                            break;
                        #endregion
                    }
                }
            }
        }
    }
}
