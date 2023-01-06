using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BenTools.Mathematics;
using Random;
using SimpleVectors;
using LandscapeGeneration.PathFind;
using System.Drawing;
using LandscapeGeneration.FastGrid;

namespace LandscapeGeneration
{
    public class Landscape: IInfoLayer
    {
        public IGrid m_pGrid = null;

        public Land[] m_aLands = null;

        public LandMass[] m_aLandMasses = null;

        public Continent[] m_aContinents = null;

        /// <summary>
        /// Общее количество `земель` - групп соседних локаций с одинаковым типом территории
        /// </summary>
        private readonly int m_iLandsCount = 500;//1000;
        /// <summary>
        /// Общее количество тектонических плит, являющихся строительными блоками при составлении континентов.
        /// Каждая тектоническая плита полностью содержит в себе одно или несколько государств.
        /// </summary>
        private readonly int m_iLandMassesCount = 50;
        /// <summary>
        /// Процент тектонических плит, лежащих на дне океана.
        /// </summary>
        protected int m_iOceansPercentage = 60;

        private readonly int m_iContinentsCount = 5;

        private readonly bool m_bGreatOcean = true;
        /// <summary>
        /// Y-координата экватора
        /// </summary>
        private readonly int m_iEquator = 0;
        /// <summary>
        /// Расстояние от экватора до полюсов
        /// </summary>
        private readonly int m_iPole = 0;

        public virtual void PresetLandTypesInfo()
        {
            LandTypes.Coastral.Init(10, 1, Environment.Flat | Environment.Open | Environment.Liquid | Environment.Wet, "sea");
            
            LandTypes.Ocean.Init(10, 5, Environment.Flat | Environment.Open | Environment.Liquid | Environment.Wet, "ocean");
            
            LandTypes.Plains.Init(1, 1, Environment.Flat | Environment.Open | Environment.Habitable, "plains");
            
            LandTypes.Savanna.Init(1, 1, Environment.Flat | Environment.Open | Environment.Hot | Environment.Habitable, "savanna");
            
            LandTypes.Tundra.Init(2, 0.5f, Environment.Flat | Environment.Open | Environment.Cold | Environment.Habitable, "tundra");
            
            LandTypes.Desert.Init(2, 0.1f, Environment.Flat | Environment.Open | Environment.Hot | Environment.Soft | Environment.Habitable, "desert");
            
            LandTypes.Forest.Init(3, 2, Environment.Habitable, "forest");
            
            LandTypes.Taiga.Init(3, 2, Environment.Cold | Environment.Habitable, "taiga");
            
            LandTypes.Swamp.Init(4, 0.1f, Environment.Flat | Environment.Open | Environment.Soft | Environment.Wet | Environment.Habitable, "swamp");
            
            LandTypes.Mountains.Init(5, 10, Environment.Open | Environment.Barrier | Environment.Habitable, "mountains");
            
            LandTypes.Jungle.Init(6, 2, Environment.Hot | Environment.Wet | Environment.Habitable, "jungle");
        }

        /// <summary>
        /// Генерация мира - старый алгоритм
        /// </summary>
        /// <param name="iLocations">Общее количество `локаций` - минимальных "кирпичиков" мира.</param>
        /// <param name="iLandsDiversity">Общее количество `земель` - групп соседних локаций с одинаковым типом территории.
        /// Сопредельные земли с одинаковым типом объединяются в 'зоны'</param>
        /// <param name="iLandMassesDiversity">Общее количество тектонических плит, являющихся строительными блоками при составлении континентов.</param>
        /// <param name="iOcean">Процент тектонических плит, лежащих на дне океана - от 10 до 90.</param>
        /// <param name="iEquator">Положение экватора на карте в процентах по вертикали. 50 - середина карты, 0 - верхний край, 100 - нижний край</param>
        /// <param name="iPole">Расстояние от экватора до полюсов в процентах по вертикали. Если экватор расположен посередине карты, то значение 50 даст для полюсов верхний и нижний края карты соответственно.</param>
        public Landscape(LocationsGrid cLocations, int iContinents, bool bGreatOcean, int iLandsDiversity, int iLandMassesDiversity, int iOcean, int iEquator, int iPole, BeginStepDelegate BeginStep, ProgressStepDelegate ProgressStep)
        {
            m_pGrid = cLocations;

            cLocations.Load(BeginStep, ProgressStep);

            if (iOcean > 90 || iOcean < 10)
                throw new ArgumentException("Oceans percent can't be less then 10 or greater then 100!");

            m_iLandsCount = 600 + iLandsDiversity * m_pGrid.Locations.Length / 200;
            m_iLandMassesCount = 30 + iLandMassesDiversity * 240 / 100;
            m_iOceansPercentage = iOcean;
            m_iContinentsCount = iContinents;
            m_bGreatOcean = bGreatOcean;

            PresetLandTypesInfo();

            m_iEquator = 2 * cLocations.RY * iEquator / 100 - cLocations.RY;
            m_iPole = 2 * cLocations.RY * iPole / 100;

            ShapeWorld(BeginStep, ProgressStep);
        }

        /// <summary>
        /// Генерация мира - новый быстрый алгоритм
        /// </summary>
        /// <param name="iLocations">Общее количество `локаций` - минимальных "кирпичиков" мира.</param>
        /// <param name="iLandsDiversity">Общее количество `земель` - групп соседних локаций с одинаковым типом территории.
        /// Сопредельные земли с одинаковым типом объединяются в 'зоны'</param>
        /// <param name="iLandMassesDiversity">Общее количество тектонических плит, являющихся строительными блоками при составлении континентов.</param>
        /// <param name="iOcean">Процент тектонических плит, лежащих на дне океана - от 10 до 90.</param>
        /// <param name="iEquator">Положение экватора на карте в процентах по вертикали. 50 - середина карты, 0 - верхний край, 100 - нижний край</param>
        /// <param name="iPole">Расстояние от экватора до полюсов в процентах по вертикали. Если экватор расположен посередине карты, то значение 50 даст для полюсов верхний и нижний края карты соответственно.</param>
        public Landscape(int iLocationsCount, int iFaceSize, int iContinents, bool bGreatOcean, int iLandsDiversity, int iLandMassesDiversity, int iOcean, int iEquator, int iPole, BeginStepDelegate BeginStep, ProgressStepDelegate ProgressStep)
        {
            m_pGrid = new ChunksGrid(iLocationsCount, iFaceSize, BeginStep, ProgressStep);

            if (iOcean > 90 || iOcean < 10)
                throw new ArgumentException("Oceans percent can't be less then 10 or greater then 100!");

            m_iLandsCount = 600 + iLandsDiversity * m_pGrid.Locations.Length / 200;
            m_iLandMassesCount = 30 + iLandMassesDiversity * 240 / 100;
            m_iOceansPercentage = iOcean;
            m_iContinentsCount = iContinents;
            m_bGreatOcean = bGreatOcean;

            PresetLandTypesInfo();

            m_iEquator = 2 * m_pGrid.RY * iEquator / 100 - m_pGrid.RY;
            m_iPole = 2 * m_pGrid.RY * iPole / 100;

            ShapeWorld(BeginStep, ProgressStep);
        }

        private void ShapeWorld(BeginStepDelegate BeginStep, ProgressStepDelegate ProgressStep)
        {
            BuildLands(BeginStep, ProgressStep);

            BuildLandMasses(BeginStep, ProgressStep);

            BuildContinents(BeginStep, ProgressStep);

            BuildAreas(BeginStep, ProgressStep);

            SmoothBiomes();
            
            CalculateElevations(BeginStep, ProgressStep);

            AddPeaks();

            BuildTransportGrid(BeginStep, ProgressStep);
        }

        private void AddPeaks()
        {
            foreach (Location pLoc in m_pGrid.Locations)
            {
                if (pLoc.Forbidden || !pLoc.HasLayer<Land>())
                    continue;

                if (pLoc.GetLayer<Land>().LandType == LandTypes.Mountains)
                {
                    bool bPeak = true;
                    foreach (Location pLink in pLoc.m_aBorderWith)
                    {
                        if (pLink.Forbidden || !pLink.HasLayer<Land>())
                            continue;

                        if (pLink.GetLayer<Land>().LandType != LandTypes.Mountains ||
                            pLink.H >= pLoc.H)
                        {
                            bPeak = false;
                        }
                    }
                    if (bPeak)
                    {
                        if (Rnd.OneChanceFrom(5))
                            pLoc.m_eType = LandmarkType.Volcano;
                        else
                            pLoc.m_eType = LandmarkType.Peak;
                    }
                }
            }
        }

        /// <summary>
        /// Биом - группа ВСЕХ сопредельных земель одного типа.
        /// Используется ТОЛЬКО при сглаживании границ локаций - внутри SmoothBiomes() 
        /// </summary>
        private class Biome : TerritoryCluster<Biome, Land>
        {
            public Biome(Land pSeed, float fCycleShift)
            {
                InitBorder(pSeed);

                Contents.Add(pSeed);

                bool bAdded;
                do
                {
                    bAdded = false;

                    ITerritory[] aBorderLands = new List<ITerritory>(m_cBorder.Keys).ToArray();
                    foreach (ITerritory pTerr in aBorderLands)
                    {
                        if (pTerr.Forbidden)
                            continue;

                        Land pLand = pTerr as Land;

                        if (pLand.LandType == pSeed.LandType && !Contents.Contains(pLand))
                        {
                            Contents.Add(pLand);

                            m_cBorder[pLand].Clear();
                            m_cBorder.Remove(pLand);

                            foreach (var pBorderLand in pLand.BorderWith)
                            {
                                if (Contents.Contains(pBorderLand.Key))
                                    continue;

                                if (!m_cBorder.ContainsKey(pBorderLand.Key))
                                    m_cBorder[pBorderLand.Key] = new List<VoronoiEdge>();
                                VoronoiEdge[] cLines = pBorderLand.Value.ToArray();
                                foreach (var pLine in cLines)
                                    m_cBorder[pBorderLand.Key].Add(new VoronoiEdge(pLine));
                            }

                            bAdded = true;
                        }
                    }
                }
                while (bAdded);

                ChainBorder(fCycleShift);
            }

            public override float GetMovementCost() { return 0; }
        }

        private void SmoothBiomes()
        {
            foreach (Continent pCont in m_aContinents)
            {
                Dictionary<LandTypeInfo, List<Biome>> cBiomes = new Dictionary<LandTypeInfo, List<Biome>>();
                foreach (var pLandType in LandTypes.m_pInstance.m_pLandTypes)
                    cBiomes[pLandType.Value] = new List<Biome>();

                HashSet<Land> cProcessedLands = new HashSet<Land>();
                foreach (var pLandMass in pCont.Contents)
                {
                    foreach (var pLand in pLandMass.Contents)
                    {
                        if (!pLand.Forbidden && !cProcessedLands.Contains(pLand))
                        {
                            Biome pNewBiome = new Biome(pLand, m_pGrid.CycleShift);
                            cBiomes[pLand.LandType].Add(pNewBiome);

                            cProcessedLands.UnionWith(pNewBiome.Contents);
                        }
                    }
                }

                foreach (Biome pBiome in cBiomes[LandTypes.Plains])
                    SmoothBorder(pBiome.m_cOrdered);

                foreach (Biome pBiome in cBiomes[LandTypes.Savanna])
                    SmoothBorder(pBiome.m_cOrdered);

                foreach (Biome pBiome in cBiomes[LandTypes.Tundra])
                    SmoothBorder(pBiome.m_cOrdered);

                foreach (Biome pBiome in cBiomes[LandTypes.Swamp])
                    SmoothBorder(pBiome.m_cOrdered);

                foreach (Biome pBiome in cBiomes[LandTypes.Desert])
                    SmoothBorder(pBiome.m_cOrdered);

                foreach (Biome pBiome in cBiomes[LandTypes.Forest])
                    SmoothBorder(pBiome.m_cOrdered);

                foreach (Biome pBiome in cBiomes[LandTypes.Jungle])
                    SmoothBorder(pBiome.m_cOrdered);

                foreach (Biome pBiome in cBiomes[LandTypes.Taiga])
                    SmoothBorder(pBiome.m_cOrdered);

                SmoothBorder(pCont.m_cOrdered);

                foreach (Biome pBiome in cBiomes[LandTypes.Mountains])
                    SmoothBorder(pBiome.m_cOrdered);
            }

            foreach (Location pLoc in m_pGrid.Locations)
            {
                if (pLoc.Forbidden || !pLoc.HasLayer<Land>())
                    continue;

                pLoc.CorrectCenter();
            }
        }

        private void SmoothBorder(List<List<VoronoiVertex>> cFirstLines)
        {
            foreach (var ordered in cFirstLines)
            {
                if (ordered.Count < 5)
                    continue;

                float smoothRate = 0.2f;

                for (int a = 0; a < 2; a++)
                {
                    ordered[0].PointOnCurve(ordered[ordered.Count - 2], ordered[ordered.Count - 1], ordered[1],
                                                 ordered[2], 0.5f, m_pGrid.CycleShift, smoothRate);
                    ordered[1].PointOnCurve(ordered[ordered.Count - 1], ordered[0], ordered[2],
                                                 ordered[3], 0.5f, m_pGrid.CycleShift, smoothRate);
                    for (int i = 2; i < ordered.Count - 2; i++)
                    {
                        ordered[i].PointOnCurve(ordered[i - 2], ordered[i - 1], ordered[i + 1],
                                                     ordered[i + 2], 0.5f, m_pGrid.CycleShift, smoothRate);
                    }
                    ordered[ordered.Count - 2].PointOnCurve(ordered[ordered.Count - 4], ordered[ordered.Count - 3], ordered[ordered.Count - 1],
                                                 ordered[0], 0.5f, m_pGrid.CycleShift, smoothRate);
                    ordered[ordered.Count - 1].PointOnCurve(ordered[ordered.Count - 3], ordered[ordered.Count - 2], ordered[0],
                                                 ordered[1], 0.5f, m_pGrid.CycleShift, smoothRate);
                }
            }
        }

        /// <summary>
        /// Формирует "земли" - группы смежных локаций, имеющих одинаковый тип территории
        /// </summary>
        /// <param name="BeginStep"></param>
        /// <param name="ProgressStep"></param>
        private void BuildLands(BeginStepDelegate BeginStep, ProgressStepDelegate ProgressStep)
        {
            BeginStep("Building lands...", m_iLandsCount + m_pGrid.Locations.Length / m_iLandsCount + m_pGrid.Locations.Length + m_iLandsCount); 
            
            List<Land> cLands = new List<Land>();
            for (int i = 0; i < m_iLandsCount; i++)
            {
                int iIndex;
                do
                {
                    iIndex = Rnd.Get(m_pGrid.Locations.Length);
                }
                while (m_pGrid.Locations[iIndex].Forbidden || 
                       m_pGrid.Locations[iIndex].HasLayer<Land>());
                
                Land pLand = new Land();
                pLand.Start(m_pGrid.Locations[iIndex]);
                cLands.Add(pLand);
                ProgressStep();
            }
            m_aLands = cLands.ToArray();

//            BeginStep("Growing lands seeds...", m_pGrid.m_aLocations.Length / m_aLands.Length);
            bool bContinue = false;
            do
            {
                bContinue = false;
                foreach (Land pLand in m_aLands)
                {
                    if (pLand.Grow() != null)
                        bContinue = true;
                }
                ProgressStep();
            }
            while (bContinue);

//            BeginStep("Fixing void lands...", m_pGrid.m_aLocations.Length);
            foreach (Location pLoc in m_pGrid.Locations)
            {
                if (!pLoc.Forbidden && !pLoc.HasLayer<Land>())
                {
                    Land pLand = new Land();
                    pLand.Start(pLoc);
                    cLands.Add(pLand);
                    while (pLand.Grow() != null) { }
                }
                ProgressStep();
            }

            m_aLands = cLands.ToArray();

//            BeginStep("Recalculating lands edges...", m_aLands.Length);
            foreach (Land pLand in m_aLands)
            {
                pLand.Finish(m_pGrid.CycleShift);
                ProgressStep();
            }
        }

        /// <summary>
        /// Формирует тектонические плиты
        /// </summary>
        /// <param name="BeginStep"></param>
        /// <param name="ProgressStep"></param>
        private void BuildLandMasses(BeginStepDelegate BeginStep, ProgressStepDelegate ProgressStep)
        {
            BeginStep("Building landmasses...", m_iLandMassesCount + m_aLands.Length / m_iLandMassesCount + m_aLands.Length + m_iLandMassesCount); 
            List<LandMass> cLandMasses = new List<LandMass>();
            for (int i = 0; i < m_iLandMassesCount; i++)
            {
                int iIndex;
                do
                {
                    iIndex = Rnd.Get(m_aLands.Length);
                }
                while (m_aLands[iIndex].HasLayer<LandMass>());

                LandMass pLandMass = new LandMass();
                pLandMass.Start(m_aLands[iIndex]);
                cLandMasses.Add(pLandMass);
                ProgressStep();
            }

            m_aLandMasses = cLandMasses.ToArray();

//            BeginStep("Growing landmasses seeds...", m_aLands.Length / m_aLandMasses.Length); 
            bool bContinue = false;
            do
            {
                bContinue = false;
                foreach (LandMass pLandMass in m_aLandMasses)
                {
                    if (pLandMass.Grow() != null)
                        bContinue = true;
                }
                ProgressStep();
            }
            while (bContinue);

//            BeginStep("Fixing void landmasses...", m_aLands.Length); 
            foreach (Land pLand in m_aLands)
            {
                if (!pLand.Forbidden && !pLand.HasLayer<LandMass>())
                {
                    LandMass pLandMass = new LandMass();
                    pLandMass.Start(pLand);
                    cLandMasses.Add(pLandMass);
                    while (pLandMass.Grow() != null) { }
                }
                ProgressStep();
            }

            m_aLandMasses = cLandMasses.ToArray();

//            BeginStep("Recalculating landmasses edges...", m_aLandMasses.Length);
            foreach (LandMass pLandMass in m_aLandMasses)
            {
                pLandMass.Finish(m_pGrid.CycleShift);
                ProgressStep();
            }
        }

        /// <summary>
        /// Формирует континенты - несоприкасающиеся между собой группы смежных тектонических плит.
        /// Тектонические плиты, не принадлежащие ни одному континенту, маркируются как "океан".
        /// </summary>
        /// <param name="BeginStep"></param>
        /// <param name="ProgressStep"></param>
        private void BuildContinents(BeginStepDelegate BeginStep, ProgressStepDelegate ProgressStep)
        {
            int iMaxOceanCount = m_iLandsCount * m_iOceansPercentage / 100;
            int iOceanCount = 0;
            Dictionary<LandMass, int> cChances = new Dictionary<LandMass,int>();
            int iTotalChances = 0;

            //string sS = m_aLandMasses[0].GetLandsString();

            BeginStep("Building  continents...", m_iContinentsCount + m_iContinentsCount); 
            foreach (LandMass pLandMass in m_aLandMasses)
            {
                if (pLandMass.Forbidden || (m_bGreatOcean && pLandMass.HaveForbiddenBorders()))
                {
                    pLandMass.IsWater = true;
                    iOceanCount += pLandMass.Contents.Count;
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

            List<Continent> cContinents = new List<Continent>();

            while (iRealContinentsCount < iRealMaxContinentsCount && iCounter < cChances.Count*2)
            {
                int iChoice = Rnd.ChooseOne(cChances.Values, 1);
                if (iChoice == -1)
                    break;

                LandMass pChoosenLM = null;
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
                    Continent pContinent = new Continent();
                    pContinent.Start(pChoosenLM);
                    cContinents.Add(pContinent);

                    if (pChoosenLM.m_iMaxSize == -1)
                        iRealContinentsCount++;

                    cChances[pChoosenLM] = 0;
                    foreach (ITerritory pTerr in pChoosenLM.m_aBorderWith)
                    {
                        if (pTerr.Forbidden)
                            continue;
                        LandMass pLandMass = pTerr as LandMass;
                        //if (cChances[pLandMass] > 0)
                        cChances[pLandMass] = 0;
                    }
                    iLandGrowActual += pChoosenLM.Contents.Count;

                    iCounter = 0;
                }
                else
                    iCounter++;

                ProgressStep();
            }

//            BeginStep("Growing continents seeds...", cContinents.Count); 
            bool bContinue = true;
            while (iLandGrowActual < iLandGrowLimit && bContinue)
            {
                bContinue = false;
                foreach (Continent pCont in cContinents)
                {
                    LandMass pAddon = pCont.Grow() as LandMass;
                    if (pAddon != null)
                    {
                        bContinue = true;
                        iLandGrowActual += pAddon.Contents.Count;

                        cChances[pAddon] = 0;
                        foreach (ITerritory pTerr in pAddon.m_aBorderWith)
                        {
                            if (pTerr.Forbidden)
                                continue;
                            LandMass pLM = pTerr as LandMass;
                            //if (cChances[pLandMass] > 0)
                            cChances[pLM] = 0;
                        }
                    }
                    ProgressStep();
                }
            }

            LandMass[] pList = new List<LandMass>(cChances.Keys).ToArray();
            BeginStep("Fixing void continents...", pList.Length);
            foreach (LandMass pLandMass in pList)
            {
                if (cChances[pLandMass] > 0 && pLandMass.m_iMaxSize > 0 && Rnd.OneChanceFrom(pLandMass.m_iMaxSize+1))
                {
                    Continent pContinent = new Continent();
                    pContinent.Start(pLandMass);
                    cContinents.Add(pContinent);

                    cChances[pLandMass] = 0;
                    foreach (ITerritory pTerr in pLandMass.m_aBorderWith)
                    {
                        if (pTerr.Forbidden)
                            continue;
                        LandMass pLM = pTerr as LandMass;
                        //if (cChances[pLandMass] > 0)
                        cChances[pLM] = 0;
                    }
                }
                ProgressStep();
            }

            m_aContinents = cContinents.ToArray();

            BeginStep("Recalculating continents edges...", m_aContinents.Length);
            foreach (Continent pCont in m_aContinents)
            {
                pCont.Finish(m_pGrid.CycleShift);
                pCont.AddLayer(this);
                ProgressStep();
            }

            foreach (LandMass pLandMass in m_aLandMasses)
                if (!pLandMass.HasLayer<Continent>())
                    pLandMass.IsWater = true;
        }

        /// <summary>
        /// Формирует шельфовое "мелководье" там, где океанические и континентальные тектонические плиты расходятся
        /// </summary>
        /// <param name="BeginStep"></param>
        /// <param name="ProgressStep"></param>
        private void AddCoastral(BeginStepDelegate BeginStep, ProgressStepDelegate ProgressStep)
        {
            BeginStep("Setting coastral regions...", m_aLands.Length);
            foreach (Land pLand in m_aLands)
            {
                if (pLand.LandType == LandTypes.Ocean || pLand.LandType == LandTypes.Coastral)
                {
                    LandMass pLM1 = pLand.GetLayer<LandMass>();

                    float fMinCollision = float.MaxValue;
                    Land pBestLand = null;
                    foreach (ITerritory pTerr in pLand.m_aBorderWith)
                    {
                        if (pTerr.Forbidden)
                            continue;

                        Land pLink = pTerr as Land;

                        if (pLink.LandType != null)
                            continue;

                        LandMass pLM2 = pLink.GetLayer<LandMass>();

                        if (pLM1 != null && pLM2 != null && pLM1 != pLM2)
                        {
                            float fDriftedX1 = pLand.X + (float)pLM1.m_pDrift.X;
                            float fDriftedY1 = pLand.Y + (float)pLM1.m_pDrift.Y;
                            float fDriftedX2 = pLink.X + (float)pLM2.m_pDrift.X;
                            float fDriftedY2 = pLink.Y + (float)pLM2.m_pDrift.Y;
                            if (Math.Abs(fDriftedX1 - fDriftedX2) > m_pGrid.CycleShift / 2)
                                if (fDriftedX1 < 0)
                                    fDriftedX2 -= m_pGrid.CycleShift;
                                else
                                    fDriftedX2 += m_pGrid.CycleShift;

                            float fDriftedDist = (float)Math.Sqrt((fDriftedX1 - fDriftedX2) * (fDriftedX1 - fDriftedX2) + (fDriftedY1 - fDriftedY2) * (fDriftedY1 - fDriftedY2));

                            float fCollision = pLand.DistanceTo(pLink, m_pGrid.CycleShift) - fDriftedDist;

                            if (fCollision < fMinCollision)
                            {
                                fMinCollision = fCollision;
                                pBestLand = pLink;
                            }
                        }
                    }

                    if (pBestLand == null)
                        continue;

                    if (fMinCollision < 1)// || Rnd.OneChanceFrom(2))
                        pLand.LandType = LandTypes.Coastral;

                    if (fMinCollision < -1.25 && pLM1.Contents.Count > 3)// && (bCoast || Rnd.ChooseOne(iMaxElevation - pLM1.m_pDrift, pLM1.m_pDrift)))
                    {
                        foreach (ITerritory pTerr in pLand.m_aBorderWith)
                        {
                            if (pTerr.Forbidden)
                                continue;

                            Land pLink = pTerr as Land;

                            if(pLink.LandType == LandTypes.Ocean)
                                pLink.LandType = LandTypes.Coastral;
                        }
                    }

                }

                ProgressStep();
            }
        }

        /// <summary>
        /// Формирует горы там, где тектонические плиты сталкиваются
        /// </summary>
        /// <param name="BeginStep"></param>
        /// <param name="ProgressStep"></param>
        private void AddMountains(BeginStepDelegate BeginStep, ProgressStepDelegate ProgressStep)
        {
            BeginStep("Setting mountain regions...", m_aLands.Length);
            foreach (Land pLand in m_aLands)
            {
                if (pLand.LandType == null)
                {
                    LandMass pLM1 = pLand.GetLayer<LandMass>();

                    float fMaxCollision = float.MinValue;
                    Land pBestLand = null;
                    foreach (ITerritory pTerr in pLand.m_aBorderWith)
                    {
                        if (pTerr.Forbidden)
                            continue;

                        Land pLink = pTerr as Land;

                        LandMass pLM2 = pLink.GetLayer<LandMass>();

                        if (pLM1 != null && pLM2 != null && pLM1 != pLM2)
                        {
                            float fDriftedX1 = pLand.X + (float)pLM1.m_pDrift.X;
                            float fDriftedY1 = pLand.Y + (float)pLM1.m_pDrift.Y;
                            float fDriftedX2 = pLink.X + (float)pLM2.m_pDrift.X;
                            float fDriftedY2 = pLink.Y + (float)pLM2.m_pDrift.Y;
                            if (Math.Abs(fDriftedX1 - fDriftedX2) > m_pGrid.CycleShift / 2)
                                if (fDriftedX1 < 0)
                                    fDriftedX2 -= m_pGrid.CycleShift;
                                else
                                    fDriftedX2 += m_pGrid.CycleShift;

                            float fDriftedDist = (float)Math.Sqrt((fDriftedX1 - fDriftedX2) * (fDriftedX1 - fDriftedX2) + (fDriftedY1 - fDriftedY2) * (fDriftedY1 - fDriftedY2));

                            float fCollision = pLand.DistanceTo(pLink, m_pGrid.CycleShift) - fDriftedDist;

                            if (fCollision > fMaxCollision)
                            {
                                fMaxCollision = fCollision;
                                pBestLand = pLink;
                            }
                        }
                    }

                    if (fMaxCollision > 1.25)// && (bCoast || Rnd.ChooseOne(iMaxElevation - pLM1.m_pDrift, pLM1.m_pDrift)))
                    {
                        pLand.LandType = LandTypes.Mountains;

                        //if (!Rnd.OneChanceFrom(3))
                        //{
                        //    int iPeak = Rnd.Get(pLand.m_cContents.Count);
                        //    if (Rnd.OneChanceFrom(3))
                        //        pLand.m_cContents[iPeak].m_eType = RegionType.Volcano;
                        //    else
                        //        pLand.m_cContents[iPeak].m_eType = RegionType.Peak;
                        //}
                    }
                }

                ProgressStep();
            }
        }

        private void AddLakes(int iOneChanceFrom, BeginStepDelegate BeginStep, ProgressStepDelegate ProgressStep)
        {
            BeginStep("Setting lake regions...", m_aLands.Length);
            foreach (Land pLand in m_aLands)
            {
                if (pLand.LandType == null)
                {
                    bool bCouldBe = true;
                    foreach (ITerritory pTerr in pLand.m_aBorderWith)
                    {
                        if (pTerr.Forbidden)
                            continue;

                        Land pLink = pTerr as Land;
                        if (pLink.IsWater)
                        {
                            bCouldBe = false;
                            break;
                        }
                    }

                    if (bCouldBe && Rnd.OneChanceFrom(iOneChanceFrom))
                        pLand.LandType = LandTypes.Ocean;
                }

                ProgressStep();
            }
        }


        private float GetTemperature(VoronoiVertex pVertex)
        {
            //float fNorthX = m_pPlanet.R / (float)Math.Sqrt(3);
            //float fNorthY = m_pPlanet.R / (float)Math.Sqrt(3);
            //float fNorthZ = m_pPlanet.R / (float)Math.Sqrt(3);

            //float fDistNorth = (float)Math.Sqrt((pVertex.m_fX - fNorthX) * (pVertex.m_fX - fNorthX) + (pVertex.m_fY - fNorthY) * (pVertex.m_fY - fNorthY) + (pVertex.m_fZ - fNorthZ) * (pVertex.m_fZ - fNorthZ));
            //float fDistSouth = (float)Math.Sqrt((pVertex.m_fX + fNorthX) * (pVertex.m_fX + fNorthX) + (pVertex.m_fY + fNorthY) * (pVertex.m_fY + fNorthY) + (pVertex.m_fZ + fNorthZ) * (pVertex.m_fZ + fNorthZ));

            //if (fDistNorth < fDistSouth)
            //    return fDistNorth / (m_pPlanet.R * (float)Math.Sqrt(2));
            //else
            //    return fDistSouth / (m_pPlanet.R * (float)Math.Sqrt(2));
            return 1 - Math.Abs((m_iEquator - pVertex.Y) / m_iPole);
        }

        private void CalculateHumidity(BeginStepDelegate BeginStep, ProgressStepDelegate ProgressStep)
        {
            BeginStep("Calculating humidity...", m_aLands.Length);

            List<ITerritory> cHumidityFront = new List<ITerritory>();
            foreach (Land pLand in m_aLands)
            {
                if (pLand.IsWater)
                {
                    foreach (ITerritory pLink in pLand.m_aBorderWith)
                    {
                        if (!pLink.Forbidden && !(pLink as Land).IsWater)
                        {
                            Land pLinkedLand = pLink as Land;

                            //удалённость точки от экватора 0..1
                            float fTemperatureMod = 1 - GetTemperature(pLinkedLand); 

                            fTemperatureMod = 1.5f / (fTemperatureMod * fTemperatureMod) + 300 * fTemperatureMod * fTemperatureMod;// *fTemperatureMod;
                            pLinkedLand.Humidity = (int)(fTemperatureMod - 5 + Rnd.Get(10.0f));

                            if (pLinkedLand.LandType != null && pLinkedLand.LandType.m_eEnvironment.HasFlag(Environment.Barrier))
                                pLinkedLand.Humidity /= 2;

                            if (!cHumidityFront.Contains(pLink))
                                cHumidityFront.Add(pLink);
                        }
                    }
                    pLand.Humidity = 100;
                }

                ProgressStep();
            }

            List<ITerritory> cNewWave = new List<ITerritory>();

            ITerritory[] aHumidityFront = cHumidityFront.ToArray();
            do
            {
                cNewWave.Clear();
                foreach (ITerritory pTerr in aHumidityFront)
                {
                    Land pLand = pTerr as Land;
                    if (pLand.Humidity > 0)
                    {
                        foreach (ITerritory pLink in pLand.m_aBorderWith)
                        {
                            if (pLink.Forbidden)
                                continue;

                            Land pLinkedLand = pLink as Land;
                            if (!pLinkedLand.IsWater && pLinkedLand.Humidity == 0)
                            {
                                pLinkedLand.Humidity = pLand.Humidity - 10 - Rnd.Get(5);

                                if (pLinkedLand.LandType != null && pLinkedLand.LandType.m_eEnvironment.HasFlag(Environment.Barrier))
                                    pLinkedLand.Humidity /= 2;

                                if (!cNewWave.Contains(pLink))
                                    cNewWave.Add(pLink);
                            }
                        }
                    }
                }
                aHumidityFront = cNewWave.ToArray();

                ProgressStep();
            }
            while (aHumidityFront.Length > 0);
        }

        /// <summary>
        /// Распределяет типы территорий и объединяет смежные земли с одинаковым типом территории в "зоны"
        /// </summary>
        /// <param name="BeginStep"></param>
        /// <param name="ProgressStep"></param>
        private void BuildAreas(BeginStepDelegate BeginStep, ProgressStepDelegate ProgressStep)
        {
            //Make seas
            foreach (Land pLand in m_aLands)
            {
                if (!pLand.HasLayer<LandMass>() || pLand.GetLayer<LandMass>().IsWater)
                    pLand.LandType = LandTypes.Ocean;
                else
                    pLand.LandType = null;
            }

            AddCoastral(BeginStep, ProgressStep);

            AddMountains(BeginStep, ProgressStep);

            AddLakes(50, BeginStep, ProgressStep);

            CalculateHumidity(BeginStep, ProgressStep);

            BeginStep("Setting other regions types...", m_aLands.Length);
            //Assign other land types
            foreach (Land pLand in m_aLands)
            {
                if (pLand.LandType == null)
                {
                    float fTemperature = GetTemperature(pLand) * 8.0f;

                    fTemperature = (float)(0.1875 * Math.Pow(2, fTemperature));

                    if (fTemperature < 0.25 || (pLand.Humidity < 5 && fTemperature < 0.35))
                    {
                        pLand.LandType = LandTypes.Tundra;
                        continue;
                    }

                    if (pLand.Humidity > 80 && Rnd.OneChanceFrom(10))
                    {
                        pLand.LandType = LandTypes.Swamp;
                        continue;
                    }

                    if (pLand.Humidity < 40 && fTemperature > 5)
                    {
                        pLand.LandType = LandTypes.Desert;
                        continue;
                    }

                    if (fTemperature > 8)
                    {
                        if (pLand.Humidity < 75 || Rnd.ChooseOne(Math.Sqrt(100 - pLand.Humidity), Math.Sqrt(pLand.Humidity)))
                            pLand.LandType = LandTypes.Savanna;
                        else
                            pLand.LandType = LandTypes.Jungle;
                        continue;
                    }

                    if (fTemperature < 0.475)
                    {
                        if (pLand.Humidity < 50 || Rnd.ChooseOne(Math.Sqrt(100 - pLand.Humidity), Math.Sqrt(pLand.Humidity)))
                            pLand.LandType = LandTypes.Plains;
                        else
                            pLand.LandType = LandTypes.Taiga;
                        continue;
                    }

                    if (pLand.Humidity < 50 || Rnd.ChooseOne(Math.Sqrt(100 - pLand.Humidity), Math.Sqrt(pLand.Humidity)))// || Rnd.OneChanceFrom(2))
                        pLand.LandType = LandTypes.Plains;
                    else
                        pLand.LandType = LandTypes.Forest;
                }

                ProgressStep();
            }
        }
        public float m_fMaxDepth = 0;
        public float m_fMaxHeight = 0;

        protected void CalculateElevations(BeginStepDelegate BeginStep, ProgressStepDelegate ProgressStep)
        {
            BeginStep("Calculating elevation...", m_pGrid.Locations.Length);

            List<Location> cOcean = new List<Location>();
            List<Location> cLand = new List<Location>();

            //Плясать будем от шельфа - у него фиксированная глубина -1
            //весь остальной океан - глубже, вся суша - выше
            foreach (Land pLand in m_aLands)
            {
                if (pLand.Forbidden)
                {
                    foreach (Location pLoc in pLand.Contents)
                        ProgressStep();
                    continue;
                }


                if (pLand.LandType == LandTypes.Coastral)
                {
                    foreach (Location pLoc in pLand.Contents)
                    {
                        pLoc.H = -1;

                        ProgressStep();

                        foreach (Location pLink in pLoc.m_aBorderWith)
                        {
                            if (pLink.Forbidden || !pLink.HasLayer<Land>() || pLink.GetLayer<Land>() == pLand)
                                continue;

                            if (pLink.GetLayer<Land>().LandType == LandTypes.Ocean)
                            {
                                if (!cOcean.Contains(pLink))
                                    cOcean.Add(pLink);
                            }
                            else
                            {
                                if (pLink.GetLayer<Land>().LandType != LandTypes.Coastral && !cLand.Contains(pLink))
                                    cLand.Add(pLink);
                            }
                        }
                    }
                }

                //Бывают прибрежные участки океана, где нет шельфа...
                //Их тоже надо учесть!
                if (pLand.LandType == LandTypes.Ocean)
                {
                    foreach (Location pLoc in pLand.Contents)
                    {
                        foreach (Location pLink in pLoc.m_aBorderWith)
                        {
                            if (pLink.Forbidden || !pLink.HasLayer<Land>() || pLink.GetLayer<Land>() == pLand)
                                continue;

                            if (pLink.GetLayer<Land>().LandType != LandTypes.Ocean &&
                                pLink.GetLayer<Land>().LandType != LandTypes.Coastral)
                            {
                                if (!cOcean.Contains(pLoc))
                                    cOcean.Add(pLoc);
                                if (!cLand.Contains(pLink))
                                    cLand.Add(pLink);
                            }
                        }
                    }
                }
            }

            List<Location> cWaveFront = new List<Location>();

            //с океаном всё просто - чем дальше от берега, тем глубже
            m_fMaxDepth = -2;
            while (cOcean.Count > 0)
            {
                cWaveFront.Clear();
                foreach (Location pLoc in cOcean)
                {
                    pLoc.H = m_fMaxDepth;
                    ProgressStep();

                    foreach (Location pLink in pLoc.m_aBorderWith)
                    {
                        if (pLink.Forbidden || !pLink.HasLayer<Land>() || !float.IsNaN(pLink.H))
                            continue;

                        //if (cWaveFront.Contains(pLink))
                        //    continue;

                        if (pLink.GetLayer<Land>().LandType == LandTypes.Ocean)
                        {
                            cWaveFront.Add(pLink);
                            pLink.H = -1;
                        }
                    }
                }

                cOcean.Clear();
                cOcean.AddRange(cWaveFront);

                m_fMaxDepth--;
            }

            //с сушей сложнее...
            m_fMaxHeight = 0;
            Dictionary<float, Location> cUnfinished = new Dictionary<float, Location>();
            SortedSet<float> cHeights = new SortedSet<float>();

            while (cLand.Count > 0 || cUnfinished.Count > 0)
            {
                foreach (Location pLoc in cLand)
                {
                    float fNewHeight;
                    do
                    {
                        float fLinkElevation = GetElevationRnd(pLoc.GetLayer<Land>().LandType);
                        fNewHeight = m_fMaxHeight + fLinkElevation;
                    }
                    while (cUnfinished.ContainsKey(fNewHeight));

                    pLoc.H = fNewHeight;
                    cHeights.Add(fNewHeight);
                    cUnfinished.Add(fNewHeight, pLoc);
                    ProgressStep();
                }
                cLand.Clear();

                if (cUnfinished.Count > 0)
                {
                    float fMin = cHeights.Min;
                    Location pLoc = cUnfinished[fMin];

                    foreach (Location pLink in pLoc.m_aBorderWith)
                    {
                        if (pLink.Forbidden || !pLink.HasLayer<Land>() || !float.IsNaN(pLink.H))
                            continue;

                        cLand.Add(pLink);
                        pLink.H = 1;
                    }
                    cHeights.Remove(pLoc.H);
                    cUnfinished.Remove(pLoc.H);
                }

                if (cHeights.Count > 0)
                {
                    float fMinHeight = cHeights.Min;
                    float fMinElevation = fMinHeight - m_fMaxHeight;
                    m_fMaxHeight += fMinElevation;
                }
            }

            NoiseMap();

            SmoothMap(100);
            SmoothMap(2);
            SmoothMap(1);
            SmoothMap(0.5f);
            SmoothMap(0.1f);
            //PlainMap();

            //CalculateVertexes();
            //SmoothVertexes();
            //SmoothVertexes();

            foreach (Location pLoc in m_pGrid.Locations)
            {
                if (pLoc.Forbidden || !pLoc.HasLayer<Land>())
                    continue;

                if (pLoc.H < 0)
                {
                    var pLine = pLoc.m_pFirstLine;
                    do
                    {
                        if (pLine.m_pPoint1.H > 0)
                            pLine.m_pPoint1.H = 0;

                        if (pLine.m_pPoint2.H > 0)
                            pLine.m_pPoint2.H = 0;

                        //if (pLine.m_pMidPoint.m_fHeight > 0)
                        //    pLine.m_pMidPoint.m_fHeight = 0;

                        //if (pLine.m_pInnerPoint.m_fHeight > 0)
                        //    pLine.m_pInnerPoint.m_fHeight = 0;

                        pLine = pLine.m_pNext;
                    }
                    while (pLine != pLoc.m_pFirstLine);
                }

                if (pLoc.H > 0)
                {
                    var pLine = pLoc.m_pFirstLine;
                    do
                    {
                        if (pLine.m_pPoint1.H < 0)
                            pLine.m_pPoint1.H = 0;

                        if (pLine.m_pPoint2.H < 0)
                            pLine.m_pPoint2.H = 0;

                        //if (pLine.m_pMidPoint.m_fHeight < 0)
                        //    pLine.m_pMidPoint.m_fHeight = 0;

                        //if (pLine.m_pInnerPoint.m_fHeight < 0)
                        //    pLine.m_pInnerPoint.m_fHeight = 0;

                        pLine = pLine.m_pNext;
                    }
                    while (pLine != pLoc.m_pFirstLine);
                }
            }
        }
        //private void CalculateVertexes()
        //{
        //    foreach (VoronoiVertex pVertex in m_pGrid.m_aVertexes)
        //    {
        //        float fTotalWeight = 0;
        //        bool bOcean = false;
        //        bool bLand = false;
        //        foreach (LOC pLoc in pVertex.m_aLocations)
        //        {
        //            if (pLoc.Forbidden || pLoc.Owner == null)
        //                continue;

        //            float fLinkElevation = (pLoc.Owner as LAND).Type.m_fElevation;
        //            pVertex.m_fZ += pLoc.m_fHeight / fLinkElevation;
        //            fTotalWeight += 1 / fLinkElevation;

        //            if (pLoc.m_fHeight > 0)
        //                bLand = true;
        //            if (pLoc.m_fHeight < 0)
        //                bOcean = true;
        //        }

        //        if (fTotalWeight > 0)
        //            pVertex.m_fZ /= fTotalWeight;

        //        if (bOcean && bLand)
        //            pVertex.m_fZ = 0;
        //    }
        //}

        //private void SmoothVertexes()
        //{
        //    foreach (VoronoiVertex pVertex in m_pGrid.m_aVertexes)
        //    {
        //        if (float.IsNaN(pVertex.m_fZ))
        //            continue;
        //        foreach (VoronoiVertex pLink in pVertex.m_cVertexes)
        //        {
        //            if (float.IsNaN(pLink.m_fZ))
        //                continue;

        //            float fDist = (float)Math.Sqrt((pVertex.m_fX - pLink.m_fX) * (pVertex.m_fX - pLink.m_fX) +
        //                                          (pVertex.m_fY - pLink.m_fY) * (pVertex.m_fY - pLink.m_fY));

        //            if (fDist < 50)
        //            {
        //                pVertex.m_fZ = (pVertex.m_fZ + pLink.m_fZ) / 2;
        //                pLink.m_fZ = pVertex.m_fZ;
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// Сглаживает карту.
        /// Локации с типом, предполагающим более высокий перепад уровней, чем заданный, не затрагиваются
        /// </summary>
        /// <param name="fMaxElevation"></param>
        private void SmoothMap(float fMaxElevation)
        {
            foreach (Location pLoc in m_pGrid.Locations)
            {
                if (pLoc.Forbidden || !pLoc.HasLayer<Land>())
                    continue;

                float fLokElevation = pLoc.GetLayer<Land>().LandType.m_fElevation;
                if (fLokElevation <= fMaxElevation)
                {
                    float fTotal = pLoc.H;
                    float fTotalWeight = 1;
                    foreach (Location pLink in pLoc.m_aBorderWith)
                    {
                        if (pLink.Forbidden || !pLink.HasLayer<Land>())
                            continue;

                        float fLinkElevation = pLink.GetLayer<Land>().LandType.m_fElevation;
                        float fWeight = 1;
                        if (fLinkElevation > fMaxElevation)
                            fWeight = 0.5f;
                        ////if(Math.Abs(pLoc.m_fHeight - pLink.m_fHeight) > (fLokElevation + fLinkElevation)/2)
                        //{
                        //    float fAverage = (pLoc.m_fHeight + pLink.m_fHeight)/2;
                        //    if (pLoc.m_fHeight > 0)
                        //        pLoc.m_fHeight = Math.Max(0.1f, (pLink.m_fHeight + pLoc.m_fHeight * (fLokElevation + 1)) / (fLokElevation + 2));
                        //    else
                        //        pLoc.m_fHeight = Math.Min(-0.1f, (pLink.m_fHeight + pLoc.m_fHeight * (fLokElevation + 1)) / (fLokElevation + 2));

                        //    if (pLink.m_fHeight > 0)
                        //        pLink.m_fHeight = Math.Max(0.1f, (pLoc.m_fHeight + pLink.m_fHeight * (fLinkElevation + 1)) / (fLinkElevation + 2));
                        //    else
                        //        pLink.m_fHeight = Math.Min(-0.1f, (pLoc.m_fHeight + pLink.m_fHeight * (fLinkElevation + 1)) / (fLinkElevation + 2));
                        //}

                        fTotal += pLink.H * fWeight;
                        fTotalWeight += fWeight;
                    }

                    if (pLoc.H > 0)
                        pLoc.H = Math.Max(0.1f, fTotal / fTotalWeight);
                    else
                        pLoc.H = Math.Min(-0.5f, fTotal / fTotalWeight);
                }
            }
        }

        private void NoiseMap()
        {
            PerlinNoise perlinNoise = new PerlinNoise(99);
            double widthDivisor = 0.5 / (double)m_pGrid.RX;
            double heightDivisor = 0.5 / (double)m_pGrid.RY;

            float vMin = 0;
            float vMax = 0;
            foreach (Location pLoc in m_pGrid.Locations)
            {
                if (pLoc.Forbidden || pLoc.H < 0)
                    continue;

                // Note that the result from the noise function is in the range -1 to 1, but I want it in the range of 0 to 1
                // that's the reason of the strange code
                float v = (float)(
                    // First octave
                    (perlinNoise.Noise(16 * pLoc.X * widthDivisor, 16 * pLoc.Y * heightDivisor, -0.5) + 1) / 2 * 0.5 +
                    // Second octave
                    (perlinNoise.Noise(32 * pLoc.X * widthDivisor, 32 * pLoc.Y * heightDivisor, 0) + 1) / 2 * 0.3 +
                    // Third octave
                    (perlinNoise.Noise(64 * pLoc.X * widthDivisor, 64 * pLoc.Y * heightDivisor, +0.5) + 1) / 2 * 0.2);

                v = v - 0.5f;

                if (v < vMin)
                    vMin = v;
                if (v > vMax)
                    vMax = v;

                v = Math.Min(1, Math.Max(-1, v * 5));

                //float fLinkElevation = GetElevation(LandTypes.GetLandType((pLoc.Owner as LAND).Type));
                if (pLoc.H > 0)
                {
                    float fLinkElevation = Math.Min(pLoc.H - 0.5f, 10);
                    pLoc.H += v * fLinkElevation;
                    //pLoc.m_fHeight = 10 + v * 10;
                }
                else
                {
                    float fLinkElevation = Math.Max(pLoc.H + 0.5f, -10);
                    pLoc.H -= v * fLinkElevation;
                    //pLoc.m_fHeight = 10 + v * 10;
                }
            }
        }

        private float GetElevationRnd(LandTypeInfo pLTI)
        {
            return pLTI.m_fElevation / 2 + Rnd.Get(pLTI.m_fElevation);
        }

        public List<TransportationLinkBase> m_cTransportGrid = new List<TransportationLinkBase>();
        public List<TransportationLinkBase> m_cLandsTransportGrid = new List<TransportationLinkBase>();
        public List<TransportationLinkBase> m_cLMTransportGrid = new List<TransportationLinkBase>();

        /// <summary>
        /// Устанавливает возможность перехода между указанными локациями при поиске пути.
        /// </summary>
        /// <param name="pNode1">первая локация</param>
        /// <param name="pNode2">вторая локация</param>
        protected TransportationLinkBase SetLink(TransportationNode pNode1, TransportationNode pNode2)
        {
            if (!pNode1.m_cLinks.ContainsKey(pNode2))
            {
                if (!pNode2.m_cLinks.ContainsKey(pNode1))
                {
                    TransportationLinkBase pLink = null;
                    if (pNode1 is Location && pNode2 is Location)
                        pLink = new TransportationLinkBase(pNode1 as Location, pNode2 as Location, m_pGrid.CycleShift);
                    if (pNode1 is Land && pNode2 is Land)
                        pLink = new TransportationLinkBase(pNode1 as Land, pNode2 as Land, m_pGrid.CycleShift);
                    if (pNode1 is LandMass && pNode2 is LandMass)
                        pLink = new TransportationLinkBase(pNode1 as LandMass, pNode2 as LandMass, m_pGrid.CycleShift);

                    if (pLink == null)
                        throw new Exception("Can't create transportation link between " + pNode1.ToString() + " and " + pNode2.ToString());

                    pNode1.m_cLinks[pNode2] = pLink;
                    pNode2.m_cLinks[pNode1] = pLink;
                    if (pNode1 is Location && pNode2 is Location)
                        m_cTransportGrid.Add(pLink);
                    if (pNode1 is Land && pNode2 is Land)
                        m_cLandsTransportGrid.Add(pLink);
                    if (pNode1 is LandMass && pNode2 is LandMass)
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

        protected TransportationLinkBase SetLink(TransportationNode pNode1, TransportationNode pNode2, TransportationLinkBase pLink)
        {
            if (!pNode1.m_cLinks.ContainsKey(pNode2))
            {
                if (!pNode2.m_cLinks.ContainsKey(pNode1))
                {
                    pNode1.m_cLinks[pNode2] = pLink;
                    pNode2.m_cLinks[pNode1] = pLink;
                    if (pNode1 is Location && pNode2 is Location)
                        m_cTransportGrid.Add(pLink);
                    if (pNode1 is Land && pNode2 is Land)
                        m_cLandsTransportGrid.Add(pLink);
                    if (pNode1 is LandMass && pNode2 is LandMass)
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

        private void BuildTransportGrid(BeginStepDelegate BeginStep, ProgressStepDelegate ProgressStep)
        {
            BeginStep("Building transportation links...", m_pGrid.Locations.Length + m_aLands.Length + m_aLandMasses.Length);

            foreach (Location pLoc in m_pGrid.Locations)
            {
                if (pLoc.Forbidden || !pLoc.HasLayer<Land>())// || pLoc.m_bBorder)
                    continue;


                foreach (Location pLink in pLoc.m_aBorderWith)
                {
                    if (pLink.Forbidden || !pLink.HasLayer<Land>())// || pLink.m_bBorder)
                        continue;

                    TransportationLinkBase pTransLink = SetLink(pLoc, pLink);
                    pTransLink.Sea = pLink.GetLayer<Land>().IsWater && pLoc.GetLayer<Land>().IsWater;
                    pTransLink.Embark = pLink.GetLayer<Land>().IsWater != pLoc.GetLayer<Land>().IsWater;
                }

                ProgressStep();
            }

            foreach (Land pLand in m_aLands)
            {
                if (pLand.Forbidden || !pLand.HasLayer<LandMass>())// || pLoc.m_bBorder)
                    continue;

                foreach (ITerritory pTerr in pLand.m_aBorderWith)
                {
                    if (pTerr.Forbidden || !pTerr.HasLayer<LandMass>())// || pLink.m_bBorder)
                        continue;

                    Land pLinked = pTerr as Land;
                    TransportationLinkBase pLink = SetLink(pLand, pLinked);
                    pLink.Sea = pLinked.GetLayer<LandMass>().IsWater && pLand.GetLayer<LandMass>().IsWater;
                    pLink.Embark = pLinked.GetLayer<LandMass>().IsWater != pLand.GetLayer<LandMass>().IsWater;
                }

                ProgressStep();
            }
            foreach (LandMass pLandMass in m_aLandMasses)
            {
                if (pLandMass.Forbidden)// || pLandMass.Owner == null)// || pLoc.m_bBorder)
                    continue;

                foreach (ITerritory pTerr in pLandMass.m_aBorderWith)
                {
                    if (pTerr.Forbidden)// || pLink.Owner == null)// || pLink.m_bBorder)
                        continue;

                    LandMass pLinked = pTerr as LandMass;
                    TransportationLinkBase pLink = SetLink(pLandMass, pLinked);
                    pLink.Sea = !pLinked.HasLayer<Continent>() && !pLandMass.HasLayer<Continent>();
                    pLink.Embark = (pLinked.HasLayer<Continent>()) != (pLandMass.HasLayer<Continent>());
                }

                ProgressStep();
            }
        }

        private static int m_iPassword = 0;

        public static ShortestPath FindReallyBestPath(Location pStart, Location pFinish, float fCycleShift, bool bNavalOnly)
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

        public static ShortestPath FindBestPath(Location pStart, Location pFinish, float fCycleShift, bool bNavalOnly)
        {
            m_iPassword++;
            ShortestPath pLMPath = new ShortestPath(pStart.GetLayer<Land>().GetLayer<LandMass>(), pFinish.GetLayer<Land>().GetLayer<LandMass>(), fCycleShift, -1, bNavalOnly);
            foreach (TransportationNode pNode in pLMPath.m_aNodes)
            {
                LandMass pLandMass = pNode as LandMass;
                foreach (Land pLand in pLandMass.Contents)
                {
                    pLand.m_iPassword = m_iPassword;
                }
                foreach (ITerritory pTerr in pLandMass.m_aBorderWith)
                {
                    if (!pTerr.Forbidden)
                    {
                        LandMass pLinkedLandMass = pTerr as LandMass;
                        foreach (Land pLand in pLinkedLandMass.Contents)
                            pLand.m_iPassword = m_iPassword;
                    }
                }
            }

            ShortestPath pLandsPath = new ShortestPath(pStart.GetLayer<Land>(), pFinish.GetLayer<Land>(), fCycleShift, m_iPassword, bNavalOnly);
            foreach (TransportationNode pNode in pLandsPath.m_aNodes)
            {
                Land pLand = pNode as Land;
                foreach (Location pLoc in pLand.Contents)
                {
                    pLoc.m_iPassword = m_iPassword;
                }
                foreach (ITerritory pTerr in pLand.m_aBorderWith)
                {
                    if (!pTerr.Forbidden)
                    {
                        Land pLinkedLand = pTerr as Land;
                        foreach (Location pLoc in pLinkedLand.Contents)
                            pLoc.m_iPassword = m_iPassword;
                    }
                }
            }

            ShortestPath pBestPath = new ShortestPath(pStart, pFinish, fCycleShift, m_iPassword, bNavalOnly);

            //List<TransportationNode> cSucceedPath = new List<TransportationNode>();
            //if (pBestPath.m_aNodes.Length == 0)
            //{
            //    foreach (TransportationNode pNode in pLandsPath.m_aNodes)
            //    {
            //        if (!pBestPath.visited.Contains(pNode))
            //            break;
            //        cSucceedPath.Add(pNode);
            //    }
            //    //pBestPath.m_cPath = cSucceedPath.ToArray();
            //}

            return pBestPath;
        }
    }
}
