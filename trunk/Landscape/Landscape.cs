using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BenTools.Mathematics;
using Random;
using SimpleVectors;
using LandscapeGeneration.PathFind;

namespace LandscapeGeneration
{
    public class Landscape<LOC, LAND, AREA, CONT, LTI>
        where LOC:Location, new()
        where LAND:Land<LOC, LTI>, new()
        where AREA:Area<LAND, LTI>, new()
        where CONT:Continent<AREA, LAND, LTI>, new()
        where LTI:LandTypeInfo, new()
    {
        public LocationsGrid<LOC> m_cGrid = null;

        public LAND[] m_aLands = null;

        public LandMass<LAND>[] m_aLandMasses = null;

        public CONT[] m_aContinents = null;

        /// <summary>
        /// Общее количество `земель` - групп соседних локаций с одинаковым типом территории
        /// </summary>
        private int m_iLandsCount = 500;//1000;
        /// <summary>
        /// Общее количество тектонических плит, являющихся строительными блоками при составлении континентов.
        /// Каждая тектоническая плита полностью содержит в себе одно или несколько государств.
        /// </summary>
        private int m_iLandMassesCount = 50;
        /// <summary>
        /// Процент тектонических плит, лежащих на дне океана.
        /// </summary>
        protected int m_iOceansPercentage = 60;

        private int m_iContinentsCount = 5;

        private bool m_bGreatOcean = true;
        /// <summary>
        /// Y-координата экватора
        /// </summary>
        private int m_iEquator = 0;
        /// <summary>
        /// Расстояние от экватора до полюсов
        /// </summary>
        private int m_iPole = 0;

        public virtual void PresetLandTypesInfo()
        {
            LandTypes<LTI>.Sea.Init(10, EnvironmentType.Water, "sea");
            LandTypes<LTI>.Plains.Init(1, EnvironmentType.Ground, "plains");
            LandTypes<LTI>.Savanna.Init(1, EnvironmentType.Ground, "savanna");
            LandTypes<LTI>.Tundra.Init(2, EnvironmentType.Ground, "tundra");
            LandTypes<LTI>.Desert.Init(2, EnvironmentType.Ground, "desert");
            LandTypes<LTI>.Forest.Init(3, EnvironmentType.Ground, "forest");
            LandTypes<LTI>.Taiga.Init(3, EnvironmentType.Ground, "taiga");
            LandTypes<LTI>.Swamp.Init(4, EnvironmentType.Ground, "swamp");
            LandTypes<LTI>.Mountains.Init(5, EnvironmentType.Mountains, "mountains");
            LandTypes<LTI>.Jungle.Init(6, EnvironmentType.Ground, "jungle");
        }

        /// <summary>
        /// Генерация мира
        /// </summary>
        /// <param name="iLocations">Общее количество `локаций` - минимальных "кирпичиков" мира.</param>
        /// <param name="iLands">Общее количество `земель` - групп соседних локаций с одинаковым типом территории.
        /// Сопредельные земли с одинаковым типом объединяются в 'зоны'</param>
        /// <param name="iLandMasses">Общее количество тектонических плит, являющихся строительными блоками при составлении континентов.</param>
        /// <param name="iOcean">Процент тектонических плит, лежащих на дне океана - от 10 до 90.</param>
        /// <param name="iEquator">Положение экватора на карте в процентах по вертикали. 50 - середина карты, 0 - верхний край, 100 - нижний край</param>
        /// <param name="iPole">Расстояние от экватора до полюсов в процентах по вертикали. Если экватор расположен посередине карты, то значение 50 даст для полюсов верхний и нижний края карты соответственно.</param>
        public Landscape(LocationsGrid<LOC> cLocations, int iContinents, bool bGreatOcean, int iLands, int iLandMasses, int iOcean, int iEquator, int iPole)
        {
            m_cGrid = cLocations;

            if (iLands > m_cGrid.m_iLocationsCount)
                throw new ArgumentException("Lands count can't be greater then locations count!");
            if (iLandMasses > iLands)
                throw new ArgumentException("Land masses count can't be greater then lands count!");
            if (iOcean > 90 || iOcean < 10)
                throw new ArgumentException("Oceans percent can't be less then 10 or greater then 100!");

            m_iLandsCount = iLands;
            m_iLandMassesCount = iLandMasses;
            m_iOceansPercentage = iOcean;
            m_iContinentsCount = iContinents;
            m_bGreatOcean = bGreatOcean;

            PresetLandTypesInfo();

            m_iEquator = 2 * m_cGrid.RY * iEquator / 100 - m_cGrid.RY;
            m_iPole = 2 * m_cGrid.RY * iPole / 100;

            ShapeWorld();
        }

        private void ShapeWorld()
        {
            BuildLands();

            BuildLandMasses();

            BuildOceans();

            //BuildContinents();

            BuildAreas();

            BuildTransportGrid();
        }

        private void BuildLands()
        {
            List<LAND> cLands = new List<LAND>();
            for (int i = 0; i < m_iLandsCount; i++)
            {
                int iIndex;
                do
                {
                    iIndex = Rnd.Get(m_cGrid.m_aLocations.Length);
                }
                while (m_cGrid.m_aLocations[iIndex].Forbidden || 
                       m_cGrid.m_aLocations[iIndex].Owner != null);
                
                LAND pLand = new LAND();
                pLand.Start(m_cGrid.m_aLocations[iIndex]);
                cLands.Add(pLand);
            }
            m_aLands = cLands.ToArray();

            bool bContinue = false;
            do
            {
                bContinue = false;
                foreach (LAND pLand in m_aLands)
                    if (pLand.Grow() != null)
                        bContinue = true;
            }
            while (bContinue);

            foreach (LAND pLand in m_aLands)
                pLand.Finish(m_cGrid.CycleShift);
        }

        private void BuildLandMasses()
        {
            List<LandMass<LAND>> cLandMasses = new List<LandMass<LAND>>();
            for (int i = 0; i < m_iLandMassesCount; i++)
            {
                int iIndex;
                do
                {
                    iIndex = Rnd.Get(m_aLands.Length);
                }
                while (m_aLands[iIndex].Owner != null);

                LandMass<LAND> pLandMass = new LandMass<LAND>();
                pLandMass.Start(m_aLands[iIndex]);
                cLandMasses.Add(pLandMass);
            }

            m_aLandMasses = cLandMasses.ToArray();

            bool bContinue = false;
            do
            {
                bContinue = false;
                foreach (LandMass<LAND> pLandMass in m_aLandMasses)
                    if (pLandMass.Grow() != null)
                        bContinue = true;
            }
            while (bContinue);

            foreach (LandMass<LAND> pLandMass in m_aLandMasses)
                pLandMass.Finish(m_cGrid.CycleShift);
        }

        private void BuildOceans()
        {
            int iMaxOceanCount = m_iLandsCount * m_iOceansPercentage / 100;
            int iOceanCount = 0;
            Dictionary<LandMass<LAND>, int> cChances = new Dictionary<LandMass<LAND>,int>();
            int iTotalChances = 0;

            //string sS = m_aLandMasses[0].GetLandsString();

            foreach (LandMass<LAND> pLandMass in m_aLandMasses)
            {
                if (pLandMass.Forbidden || (m_bGreatOcean && pLandMass.HaveForbiddenBorders()))
                {
                    pLandMass.IsWater = true;
                    iOceanCount += pLandMass.m_cContents.Count;
                    cChances[pLandMass] = 0;
                }
                else
                {
                    cChances[pLandMass] = pLandMass.m_iMaxSize > 0 ? 66 : 100;
                    iTotalChances++;
                }
            }

            int iRealMaxContinentsCount = Math.Min(iTotalChances, m_iContinentsCount);
            int iRealContinentsCount = 0;

            int iLandGrowLimit = m_iLandsCount - iMaxOceanCount;
            int iLandGrowActual = 0;
            int iCounter = 0;

            List<CONT> cContinents = new List<CONT>();

            while (iRealContinentsCount < iRealMaxContinentsCount && iCounter < cChances.Count*2)
            {
                int iChoice = Rnd.ChooseOne(cChances.Values, 1);
                if (iChoice == -1)
                    break;

                LandMass<LAND> pChoosenLM = null;
                foreach (var pLandMass in cChances)
                {
                    iChoice--;
                    if (iChoice < 0)
                    {
                        pChoosenLM = pLandMass.Key;
                        break;
                    }
                }

                if (pChoosenLM != null && pChoosenLM.m_iMaxSize == -1)
                {
                    CONT pContinent = new CONT();
                    pContinent.Start(pChoosenLM);
                    cContinents.Add(pContinent);

                    if (pChoosenLM.m_iMaxSize == -1)
                        iRealContinentsCount++;

                    cChances[pChoosenLM] = 0;
                    foreach (ITerritory pTerr in pChoosenLM.m_aBorderWith)
                    {
                        if (pTerr.Forbidden)
                            continue;
                        LandMass<LAND> pLandMass = pTerr as LandMass<LAND>;
                        //if (cChances[pLandMass] > 0)
                        cChances[pLandMass] = 0;
                    }
                    iLandGrowActual += pChoosenLM.m_cContents.Count;

                    iCounter = 0;
                }
                else
                    iCounter++;
            }

            bool bContinue = true;
            while (iLandGrowActual < iLandGrowLimit && bContinue)
            {
                bContinue = false;
                foreach (CONT pCont in cContinents)
                {
                    LandMass<LAND> pAddon = pCont.Grow() as LandMass<LAND>;
                    if (pAddon != null)
                    {
                        bContinue = true;
                        iLandGrowActual += pAddon.m_cContents.Count;

                        cChances[pAddon] = 0;
                        foreach (ITerritory pTerr in pAddon.m_aBorderWith)
                        {
                            if (pTerr.Forbidden)
                                continue;
                            LandMass<LAND> pLM = pTerr as LandMass<LAND>;
                            //if (cChances[pLandMass] > 0)
                            cChances[pLM] = 0;
                        }
                    }
                }
            }

            LandMass<LAND>[] pList = new List<LandMass<LAND>>(cChances.Keys).ToArray();
            foreach (LandMass<LAND> pLandMass in pList)
            {
                if (cChances[pLandMass] > 0 && pLandMass.m_iMaxSize > 0 && Rnd.OneChanceFrom(pLandMass.m_iMaxSize+1))
                {
                    CONT pContinent = new CONT();
                    pContinent.Start(pLandMass);
                    cContinents.Add(pContinent);

                    cChances[pLandMass] = 0;
                    foreach (ITerritory pTerr in pLandMass.m_aBorderWith)
                    {
                        if (pTerr.Forbidden)
                            continue;
                        LandMass<LAND> pLM = pTerr as LandMass<LAND>;
                        //if (cChances[pLandMass] > 0)
                        cChances[pLM] = 0;
                    }
                }
            }

            m_aContinents = cContinents.ToArray();

            foreach (CONT pCont in m_aContinents)
                pCont.Finish(m_cGrid.CycleShift);

            foreach (LandMass<LAND> pLandMass in m_aLandMasses)
                if (pLandMass.Owner == null)
                    pLandMass.IsWater = true;
        }

        //private void BuildContinents()
        //{
        //    foreach (LandMass<LAND> pLandMass in m_cLandMasses)
        //    {
        //        if (!pLandMass.m_bOcean && pLandMass.Owner == null)
        //        {
        //            CONT pContinent = new CONT();
        //            pContinent.Start(pLandMass);
        //            while (pContinent.Grow() != null) { }
        //            pContinent.Finish();
        //            m_cContinents.Add(pContinent);
        //        }
        //    }
        //}

        private void AddMountains()
        {
            foreach (LAND pLand in m_aLands)
            {
                if (pLand.Type == null)
                {
                    LandMass<LAND> pLM1 = pLand.Owner as LandMass<LAND>;

                    float fMaxCollision = 0;
                    LAND pBestLand = null;
                    foreach (ITerritory pTerr in pLand.m_aBorderWith)
                    {
                        if (pTerr.Forbidden)
                            continue;

                        LAND pLink = pTerr as LAND;

                        LandMass<LAND> pLM2 = pLink.Owner as LandMass<LAND>;

                        if (pLM1 != null && pLM2 != null && pLM1 != pLM2)
                        {
                            if (!(pLM1.m_pDrift + pLM2.m_pDrift) > fMaxCollision)//Это не отрицание, это модуль вектора!
                            {
                                fMaxCollision = (float)(2 * !(pLM1.m_pDrift + pLM2.m_pDrift));
                                pBestLand = pLink;
                            }
                        }
                    }

                    if (fMaxCollision > 2.5)// && (bCoast || Rnd.ChooseOne(iMaxElevation - pLM1.m_pDrift, pLM1.m_pDrift)))
                    {
                        SimpleVector3d pL12 = new SimpleVector3d(pLand.X - pBestLand.X, pLand.Y - pBestLand.Y, 0);
                        SimpleVector3d pL21 = new SimpleVector3d(pBestLand.X - pLand.X, pBestLand.Y - pLand.Y, 0);

                        pL12 = pL12 / !pL12;
                        pL21 = pL21 / !pL21;

                        LandMass<LAND> pLMO = pBestLand.Owner as LandMass<LAND>;

                        if (!(pL12 + pLM1.m_pDrift) > !(pL21 + pLMO.m_pDrift))
                            continue;
                        
                        pLand.Type = LandTypes<LTI>.Mountains;

                        if(!Rnd.OneChanceFrom(3))
                        {
                            int iPeak = Rnd.Get(pLand.m_cContents.Count);
                            if (Rnd.OneChanceFrom(3))
                                pLand.m_cContents[iPeak].m_eType = RegionType.Volcano;
                            else
                                pLand.m_cContents[iPeak].m_eType = RegionType.Peak;
                        }
                    }
                }
            }
        }

        private void AddLakes(int iOneChanceFrom)
        {
            foreach (LAND pLand in m_aLands)
            {
                if (pLand.Type == null)
                {
                    bool bCouldBe = true;
                    foreach (ITerritory pTerr in pLand.m_aBorderWith)
                    {
                        if (pTerr.Forbidden)
                            continue;

                        LAND pLink = pTerr as LAND;
                        if (pLink.IsWater)
                        {
                            bCouldBe = false;
                            break;
                        }
                    }

                    if (bCouldBe && Rnd.OneChanceFrom(iOneChanceFrom))
                        pLand.Type = LandTypes<LTI>.Sea;
                }
            }
        }

        private void CalculateHumidity()
        {
            List<ITerritory> cHumidityFront = new List<ITerritory>();
            foreach (LAND pLand in m_aLands)
            {
                if (pLand.IsWater)
                {
                    foreach (ITerritory pLink in pLand.m_aBorderWith)
                    {
                        if (!pLink.Forbidden && !(pLink as ILand).IsWater)
                        {
                            LAND pLinkedLand = pLink as LAND;

                            //удалённость точки от экватора 0..1
                            float fTemperatureMod = Math.Abs((m_iEquator - pLinkedLand.Y) / m_iPole);

                            fTemperatureMod = 1.5f / (fTemperatureMod * fTemperatureMod) + 300 * fTemperatureMod * fTemperatureMod;// *fTemperatureMod;
                            pLinkedLand.Humidity = (int)(fTemperatureMod - 5 + Rnd.Get(10.0f));

                            if (pLinkedLand.Type != null && pLinkedLand.Type.m_eType == EnvironmentType.Mountains)
                                pLinkedLand.Humidity /= 2;

                            if (!cHumidityFront.Contains(pLink))
                                cHumidityFront.Add(pLink);
                        }
                    }
                    pLand.Humidity = 100;
                }
            }

            List<ITerritory> cNewWave = new List<ITerritory>();

            ITerritory[] aHumidityFront = cHumidityFront.ToArray();
            do
            {
                cNewWave.Clear();
                foreach (ITerritory pTerr in aHumidityFront)
                {
                    LAND pLand = pTerr as LAND;
                    if (pLand.Humidity > 0)
                    {
                        foreach (ITerritory pLink in pLand.m_aBorderWith)
                        {
                            if (pLink.Forbidden)
                                continue;

                            LAND pLinkedLand = pLink as LAND;
                            if (!pLinkedLand.IsWater && pLinkedLand.Humidity == 0)
                            {
                                pLinkedLand.Humidity = pLand.Humidity - 10 - Rnd.Get(5);

                                if (pLinkedLand.Type != null && pLinkedLand.Type.m_eType == EnvironmentType.Mountains)
                                    pLinkedLand.Humidity /= 2;

                                if (!cNewWave.Contains(pLink))
                                    cNewWave.Add(pLink);
                            }
                        }
                    }
                }
                aHumidityFront = cNewWave.ToArray();
            }
            while (aHumidityFront.Length > 0);
        }

        private void BuildAreas()
        {
            //Make seas
            foreach (LAND pLand in m_aLands)
            {
                if (pLand.Owner == null || (pLand.Owner as LandMass<LAND>).IsWater)
                    pLand.Type = LandTypes<LTI>.Sea;
                else
                    pLand.Type = null;
            }

            AddMountains();

            AddLakes(50);

            CalculateHumidity();

            //Assign other land types
            foreach (LAND pLand in m_aLands)
            {
                if (pLand.Type == null)
                {
                    float fTemperature = (1 - Math.Abs((m_iEquator - (pLand as ILand).Y) / m_iPole))*8.0f;

                    fTemperature = (float)(0.1875 * Math.Pow(2, fTemperature));

                    if (fTemperature < 0.25 || (pLand.Humidity < 5 && fTemperature < 0.35))
                    {
                        pLand.Type = LandTypes<LTI>.Tundra;
                        continue;
                    }

                    if (pLand.Humidity > 80 && Rnd.OneChanceFrom(10))
                    {
                        pLand.Type = LandTypes<LTI>.Swamp;
                        continue;
                    }

                    if (pLand.Humidity < 40 && fTemperature > 5)
                    {
                        pLand.Type = LandTypes<LTI>.Desert;
                        continue;
                    }

                    if (fTemperature > 8)
                    {
                        if (pLand.Humidity < 75 || Rnd.ChooseOne(Math.Sqrt(100 - pLand.Humidity), Math.Sqrt(pLand.Humidity)))
                            pLand.Type = LandTypes<LTI>.Savanna;
                        else
                            pLand.Type = LandTypes<LTI>.Jungle;
                        continue;
                    }

                    if (fTemperature < 0.475)
                    {
                        if (pLand.Humidity < 50 || Rnd.ChooseOne(Math.Sqrt(100 - pLand.Humidity), Math.Sqrt(pLand.Humidity)))
                            pLand.Type = LandTypes<LTI>.Plains;
                        else
                            pLand.Type = LandTypes<LTI>.Taiga;
                        continue;
                    }

                    if (pLand.Humidity < 50 || Rnd.ChooseOne(Math.Sqrt(100 - pLand.Humidity), Math.Sqrt(pLand.Humidity)) || Rnd.OneChanceFrom(3))
                        pLand.Type = LandTypes<LTI>.Plains;
                    else
                        pLand.Type = LandTypes<LTI>.Forest;
                }
            }

            //Gathering areas of the same land type
            foreach (CONT pContinent in m_aContinents)
                pContinent.BuildAreas(m_cGrid.CycleShift, m_iLandsCount / 100);
        }

        public List<TransportationLink> m_cTransportGrid = new List<TransportationLink>();
        public List<TransportationLink> m_cLandsTransportGrid = new List<TransportationLink>();
        public List<TransportationLink> m_cLMTransportGrid = new List<TransportationLink>();

        /// <summary>
        /// Устанавливает возможность перехода между указанными локациями при поиске пути.
        /// </summary>
        /// <param name="pNode1">первая локация</param>
        /// <param name="pNode2">вторая локация</param>
        protected TransportationLink SetLink(TransportationNode pNode1, TransportationNode pNode2)
        {
            if (!pNode1.m_cLinks.ContainsKey(pNode2))
            {
                if (!pNode2.m_cLinks.ContainsKey(pNode1))
                {
                    TransportationLink pLink = null;
                    if (pNode1 is Location && pNode2 is Location)
                        pLink = new TransportationLink(pNode1 as Location, pNode2 as Location, m_cGrid.CycleShift);
                    if (pNode1 is ILand && pNode2 is ILand)
                        pLink = new TransportationLink(pNode1 as ILand, pNode2 as ILand, m_cGrid.CycleShift);
                    if (pNode1 is ILandMass && pNode2 is ILandMass)
                        pLink = new TransportationLink(pNode1 as ILandMass, pNode2 as ILandMass, m_cGrid.CycleShift);

                    if (pLink == null)
                        throw new Exception("Can't create transportation link between " + pNode1.ToString() + " and " + pNode2.ToString());

                    pNode1.m_cLinks[pNode2] = pLink;
                    pNode2.m_cLinks[pNode1] = pLink;
                    if (pNode1 is Location && pNode2 is Location)
                        m_cTransportGrid.Add(pLink);
                    if (pNode1 is ILand && pNode2 is ILand)
                        m_cLandsTransportGrid.Add(pLink);
                    if (pNode1 is ILandMass && pNode2 is ILandMass)
                        m_cLMTransportGrid.Add(pLink);

                    return pLink;
                }
                else
                {
                    pNode1.m_cLinks[pNode2] = pNode2.m_cLinks[pNode1];
                    return pNode1.m_cLinks[pNode2];
                }
            }
            else
                if (!pNode2.m_cLinks.ContainsKey(pNode1))
                {
                    pNode2.m_cLinks[pNode1] = pNode1.m_cLinks[pNode2];
                    return pNode2.m_cLinks[pNode1];
                }

            return pNode2.m_cLinks[pNode1];
        }

        protected TransportationLink SetLink(TransportationNode pNode1, TransportationNode pNode2, TransportationLink pLink)
        {
            if (!pNode1.m_cLinks.ContainsKey(pNode2))
            {
                if (!pNode2.m_cLinks.ContainsKey(pNode1))
                {
                    pNode1.m_cLinks[pNode2] = pLink;
                    pNode2.m_cLinks[pNode1] = pLink;
                    if (pNode1 is Location && pNode2 is Location)
                        m_cTransportGrid.Add(pLink);
                    if (pNode1 is ILand && pNode2 is ILand)
                        m_cLandsTransportGrid.Add(pLink);
                    if (pNode1 is ILandMass && pNode2 is ILandMass)
                        m_cLMTransportGrid.Add(pLink);

                    return pLink;
                }
                else
                {
                    pNode1.m_cLinks[pNode2] = pNode2.m_cLinks[pNode1];
                    return pNode1.m_cLinks[pNode2];
                }
            }
            else
                if (!pNode2.m_cLinks.ContainsKey(pNode1))
                {
                    pNode2.m_cLinks[pNode1] = pNode1.m_cLinks[pNode2];
                    return pNode2.m_cLinks[pNode1];
                }

            return pNode2.m_cLinks[pNode1];
        }

        private void BuildTransportGrid()
        {
            foreach (LOC pLoc in m_cGrid.m_aLocations)
            {
                if (pLoc.Forbidden || pLoc.Owner == null)// || pLoc.m_bBorder)
                    continue;


                foreach (LOC pLink in pLoc.m_aBorderWith)
                {
                    if (pLink.Forbidden || pLink.Owner == null)// || pLink.m_bBorder)
                        continue;

                    TransportationLink pTransLink = SetLink(pLoc, pLink);
                    pTransLink.m_bSea = (pLink.Owner as LAND).IsWater && (pLoc.Owner as LAND).IsWater;
                    pTransLink.m_bEmbark = (pLink.Owner as LAND).IsWater != (pLoc.Owner as LAND).IsWater;
                }
            }
            foreach (LAND pLand in m_aLands)
            {
                if (pLand.Forbidden || pLand.Owner == null)// || pLoc.m_bBorder)
                    continue;

                foreach (ITerritory pTerr in pLand.m_aBorderWith)
                {
                    if (pTerr.Forbidden || pTerr.Owner == null)// || pLink.m_bBorder)
                        continue;

                    LAND pLinked = pTerr as LAND;
                    TransportationLink pLink = SetLink(pLand, pLinked);
                    pLink.m_bSea = (pLinked.Owner as LandMass<LAND>).IsWater && (pLand.Owner as LandMass<LAND>).IsWater;
                    pLink.m_bEmbark = (pLinked.Owner as LandMass<LAND>).IsWater != (pLand.Owner as LandMass<LAND>).IsWater;
                }
            }
            foreach (LandMass<LAND> pLandMass in m_aLandMasses)
            {
                if (pLandMass.Forbidden)// || pLandMass.Owner == null)// || pLoc.m_bBorder)
                    continue;

                foreach (ITerritory pTerr in pLandMass.m_aBorderWith)
                {
                    if (pTerr.Forbidden)// || pLink.Owner == null)// || pLink.m_bBorder)
                        continue;

                    LandMass<LAND> pLinked = pTerr as LandMass<LAND>;
                    TransportationLink pLink = SetLink(pLandMass, pLinked);
                    pLink.m_bSea = (pLinked.Owner == null) && (pLandMass.Owner == null);
                    pLink.m_bEmbark = (pLinked.Owner == null) != (pLandMass.Owner == null);
                }
            }
        }

        private static int m_iPassword = 0;

        public static ShortestPath FindReallyBestPath(LOC pStart, LOC pFinish, float fCycleShift, bool bNavalOnly)
        {
            ShortestPath pBestPath1 = FindBestPath(pStart, pFinish, fCycleShift, bNavalOnly);
            ShortestPath pBestPath2 = FindBestPath(pFinish, pStart, fCycleShift, bNavalOnly);
            
            if (pBestPath1 == null ||
                pBestPath1.m_aNodes.Length == 0 ||
                (pBestPath2 != null &&
                 pBestPath2.m_aNodes.Length != 0 &&
                 pBestPath2.m_fLength < pBestPath1.m_fLength))
                return pBestPath2;

            return pBestPath1;
        }

        public static ShortestPath FindBestPath(LOC pStart, LOC pFinish, float fCycleShift, bool bNavalOnly)
        {
            m_iPassword++;
            ShortestPath pLMPath = new ShortestPath((pStart.Owner as LAND).Owner as LandMass<LAND>, (pFinish.Owner as LAND).Owner as LandMass<LAND>, fCycleShift, -1, bNavalOnly);
            foreach (TransportationNode pNode in pLMPath.m_aNodes)
            {
                LandMass<LAND> pLandMass = pNode as LandMass<LAND>;
                foreach (LAND pLand in pLandMass.m_cContents)
                {
                    pLand.m_iPassword = m_iPassword;
                }
                foreach (ITerritory pTerr in pLandMass.m_aBorderWith)
                {
                    if (!pTerr.Forbidden)
                    {
                        LandMass<LAND> pLinkedLandMass = pTerr as LandMass<LAND>;
                        foreach (LAND pLand in pLinkedLandMass.m_cContents)
                            pLand.m_iPassword = m_iPassword;
                    }
                }
            }

            ShortestPath pLandsPath = new ShortestPath(pStart.Owner as LAND, pFinish.Owner as LAND, fCycleShift, m_iPassword, bNavalOnly);
            foreach (TransportationNode pNode in pLandsPath.m_aNodes)
            {
                LAND pLand = pNode as LAND;
                foreach (LOC pLoc in pLand.m_cContents)
                {
                    pLoc.m_iPassword = m_iPassword;
                }
                foreach (ITerritory pTerr in pLand.m_aBorderWith)
                {
                    if (!pTerr.Forbidden)
                    {
                        LAND pLinkedLand = pTerr as LAND;
                        foreach (LOC pLoc in pLinkedLand.m_cContents)
                            pLoc.m_iPassword = m_iPassword;
                    }
                }
            }

            ShortestPath pBestPath = new ShortestPath(pStart, pFinish, fCycleShift, m_iPassword, bNavalOnly);

            List<TransportationNode> cSucceedPath = new List<TransportationNode>();
            if (pBestPath.m_aNodes.Length == 0)
            {
                foreach (TransportationNode pNode in pLandsPath.m_aNodes)
                {
                    if (!pBestPath.visited.Contains(pNode))
                        break;
                    cSucceedPath.Add(pNode);
                }
                //pBestPath.m_cPath = cSucceedPath.ToArray();
            }

            return pBestPath;
        }
    }
}
