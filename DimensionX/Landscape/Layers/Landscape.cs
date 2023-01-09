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
    public class Landscape: Territory<Landscape>
    {
        public LocationsGrid LocationsGrid { get; } = null;

        public Land[] Lands { get; private set; } = null;

        public LandMass[] LandMasses { get; private set; } = null;

        public Continent[] Contents { get; private set; } = null;

        public override float GetMovementCost()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Общее количество `земель` - групп соседних локаций с одинаковым типом территории
        /// </summary>
        private readonly int m_iLandsCount = 500;
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

        public void PresetLandTypesInfo()
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

        public Landscape()
        { }

        /// <summary>
        /// Генерация мира
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
            LocationsGrid = new LocationsGrid(iLocationsCount, iFaceSize, BeginStep, ProgressStep);

            if (iOcean > 90 || iOcean < 10)
                throw new ArgumentException("Oceans percent can't be less then 10 or greater then 100!");

            m_iLandsCount = 600 + iLandsDiversity * LocationsGrid.Locations.Length / 200;
            m_iLandMassesCount = 30 + iLandMassesDiversity * 240 / 100;
            m_iOceansPercentage = iOcean;
            m_iContinentsCount = iContinents;
            m_bGreatOcean = bGreatOcean;

            PresetLandTypesInfo();

            m_iEquator = 2 * LocationsGrid.RY * iEquator / 100 - LocationsGrid.RY;
            m_iPole = 2 * LocationsGrid.RY * iPole / 100;

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
            foreach (Location pLoc in LocationsGrid.Locations)
            {
                if (pLoc.Forbidden || !pLoc.HasOwner())
                    continue;

                if (pLoc.GetOwner().LandType == LandTypes.Mountains)
                {
                    bool bPeak = true;
                    foreach (Location pLink in pLoc.BorderWithKeys)
                    {
                        if (pLink.Forbidden || !pLink.HasOwner())
                            continue;

                        if (pLink.GetOwner().LandType != LandTypes.Mountains ||
                            pLink.H >= pLoc.H)
                        {
                            bPeak = false;
                        }
                    }
                    if (bPeak)
                    {
                        if (Rnd.OneChanceFrom(5))
                            pLoc.Landmark = LandmarkType.Volcano;
                        else
                            pLoc.Landmark = LandmarkType.Peak;
                    }
                }
            }
        }

        private sealed class LandBiome : TerritoryExtended<LandBiome, Biome, Land>
        {
            public LandBiome(Land pLand) : base(pLand)
            { }

            public LandBiome()
            { }
        }

        /// <summary>
        /// Биом - группа ВСЕХ сопредельных земель (<see cref="Land"/>/<see cref="LandBiome"/>) одного типа <see cref="LandTypeInfo"/>.
        /// Используется ТОЛЬКО при сглаживании границ локаций - внутри <see cref="SmoothBiomes"/>
        /// </summary>
        private sealed class Biome : TerritoryCluster<Biome, Landscape, LandBiome>
        {
            public Biome()
            { }

            public Biome(LandBiome pSeed, float fCycleShift)
            {
                InitBorder(pSeed);

                Contents.Add(pSeed);

                bool bAdded;
                do
                {
                    bAdded = false;

                    LandBiome[] aBorderLands = new List<LandBiome>(Border.Keys).ToArray();
                    foreach (LandBiome pLand in aBorderLands)
                    {
                        if (pLand.Forbidden)
                            continue;

                        if (pLand.Origin.LandType == pSeed.Origin.LandType && !Contents.Contains(pLand))
                        {
                            Contents.Add(pLand);

                            Border[pLand].Clear();
                            Border.Remove(pLand);

                            foreach (var pBorderLand in pLand.BorderWith)
                            {
                                if (Contents.Contains(pBorderLand.Key))
                                    continue;

                                if (!Border.ContainsKey(pBorderLand.Key))
                                    Border[pBorderLand.Key] = new List<VoronoiEdge>();
                                VoronoiEdge[] cLines = pBorderLand.Value.ToArray();
                                foreach (var pLine in cLines)
                                    Border[pBorderLand.Key].Add(new VoronoiEdge(pLine));
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
            foreach (Land pLand in Lands)
            {
                pLand.AddLayer(new LandBiome(pLand));
            }

            foreach (Land pLand in Lands)
            {
                pLand.As<LandBiome>().FillBorderWithKeys();
            }

            foreach (Continent pCont in Contents)
            {
                Dictionary<LandTypeInfo, List<Biome>> cBiomes = new Dictionary<LandTypeInfo, List<Biome>>();
                foreach (var pLandType in LandTypes.Instance.Lands)
                    cBiomes[pLandType.Value] = new List<Biome>();

                HashSet<LandBiome> cProcessedLands = new HashSet<LandBiome>();
                foreach (var pLandMass in pCont.Contents)
                {
                    foreach (var pLand in pLandMass.Contents)
                    {
                        if (!pLand.Forbidden && !cProcessedLands.Contains(pLand.As<LandBiome>()))
                        {
                            Biome pNewBiome = new Biome(pLand.As<LandBiome>(), LocationsGrid.CycleShift);
                            cBiomes[pLand.LandType].Add(pNewBiome);

                            cProcessedLands.UnionWith(pNewBiome.Contents);
                        }
                    }
                }

                foreach (Biome pBiome in cBiomes[LandTypes.Plains])
                    SmoothBorder(pBiome.BorderOrdered);

                foreach (Biome pBiome in cBiomes[LandTypes.Savanna])
                    SmoothBorder(pBiome.BorderOrdered);

                foreach (Biome pBiome in cBiomes[LandTypes.Tundra])
                    SmoothBorder(pBiome.BorderOrdered);

                foreach (Biome pBiome in cBiomes[LandTypes.Swamp])
                    SmoothBorder(pBiome.BorderOrdered);

                foreach (Biome pBiome in cBiomes[LandTypes.Desert])
                    SmoothBorder(pBiome.BorderOrdered);

                foreach (Biome pBiome in cBiomes[LandTypes.Forest])
                    SmoothBorder(pBiome.BorderOrdered);

                foreach (Biome pBiome in cBiomes[LandTypes.Jungle])
                    SmoothBorder(pBiome.BorderOrdered);

                foreach (Biome pBiome in cBiomes[LandTypes.Taiga])
                    SmoothBorder(pBiome.BorderOrdered);

                SmoothBorder(pCont.BorderOrdered);

                foreach (Biome pBiome in cBiomes[LandTypes.Mountains])
                    SmoothBorder(pBiome.BorderOrdered);
            }

            foreach (Location pLoc in LocationsGrid.Locations)
            {
                if (pLoc.Forbidden || !pLoc.HasOwner())
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

                const float smoothRate = 0.2f;

                for (int a = 0; a < 2; a++)
                {
                    ordered[0].PointOnCurve(ordered[ordered.Count - 2], ordered[ordered.Count - 1], ordered[1],
                                                 ordered[2], 0.5f, LocationsGrid.CycleShift, smoothRate);
                    ordered[1].PointOnCurve(ordered[ordered.Count - 1], ordered[0], ordered[2],
                                                 ordered[3], 0.5f, LocationsGrid.CycleShift, smoothRate);
                    for (int i = 2; i < ordered.Count - 2; i++)
                    {
                        ordered[i].PointOnCurve(ordered[i - 2], ordered[i - 1], ordered[i + 1],
                                                     ordered[i + 2], 0.5f, LocationsGrid.CycleShift, smoothRate);
                    }
                    ordered[ordered.Count - 2].PointOnCurve(ordered[ordered.Count - 4], ordered[ordered.Count - 3], ordered[ordered.Count - 1],
                                                 ordered[0], 0.5f, LocationsGrid.CycleShift, smoothRate);
                    ordered[ordered.Count - 1].PointOnCurve(ordered[ordered.Count - 3], ordered[ordered.Count - 2], ordered[0],
                                                 ordered[1], 0.5f, LocationsGrid.CycleShift, smoothRate);
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
            BeginStep("Building lands...", m_iLandsCount + LocationsGrid.Locations.Length / m_iLandsCount + LocationsGrid.Locations.Length + m_iLandsCount);

            List<Land> cLands = new List<Land>();
            for (int i = 0; i < m_iLandsCount; i++)
            {
                int iIndex;
                do
                {
                    iIndex = Rnd.Get(LocationsGrid.Locations.Length);
                }
                while (LocationsGrid.Locations[iIndex].Forbidden ||
                       LocationsGrid.Locations[iIndex].HasOwner());

                Land pLand = new Land();
                pLand.Start(LocationsGrid.Locations[iIndex]);
                cLands.Add(pLand);
                ProgressStep();
            }
            Lands = cLands.ToArray();

//            BeginStep("Growing lands seeds...", m_pGrid.m_aLocations.Length / m_aLands.Length);
            bool bContinue = false;
            do
            {
                bContinue = false;
                foreach (Land pLand in Lands)
                {
                    if (pLand.Grow() != null)
                        bContinue = true;
                }
                ProgressStep();
            }
            while (bContinue);

//            BeginStep("Fixing void lands...", m_pGrid.m_aLocations.Length);
            foreach (Location pLoc in LocationsGrid.Locations)
            {
                if (!pLoc.Forbidden && !pLoc.HasOwner())
                {
                    Land pLand = new Land();
                    pLand.Start(pLoc);
                    cLands.Add(pLand);
                    while (pLand.Grow() != null) { }
                }
                ProgressStep();
            }

            Lands = cLands.ToArray();

//            BeginStep("Recalculating lands edges...", m_aLands.Length);
            foreach (Land pLand in Lands)
            {
                pLand.Finish(LocationsGrid.CycleShift);
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
            BeginStep("Building landmasses...", m_iLandMassesCount + Lands.Length / m_iLandMassesCount + Lands.Length + m_iLandMassesCount);
            List<LandMass> cLandMasses = new List<LandMass>();
            for (int i = 0; i < m_iLandMassesCount; i++)
            {
                int iIndex;
                do
                {
                    iIndex = Rnd.Get(Lands.Length);
                }
                while (Lands[iIndex].HasOwner());

                LandMass pLandMass = new LandMass();
                pLandMass.Start(Lands[iIndex]);
                cLandMasses.Add(pLandMass);
                ProgressStep();
            }

            LandMasses = cLandMasses.ToArray();

//            BeginStep("Growing landmasses seeds...", m_aLands.Length / m_aLandMasses.Length); 
            bool bContinue = false;
            do
            {
                bContinue = false;
                foreach (LandMass pLandMass in LandMasses)
                {
                    if (pLandMass.Grow() != null)
                        bContinue = true;
                }
                ProgressStep();
            }
            while (bContinue);

//            BeginStep("Fixing void landmasses...", m_aLands.Length); 
            foreach (Land pLand in Lands)
            {
                if (!pLand.Forbidden && !pLand.HasOwner())
                {
                    LandMass pLandMass = new LandMass();
                    pLandMass.Start(pLand);
                    cLandMasses.Add(pLandMass);
                    while (pLandMass.Grow() != null) { }
                }
                ProgressStep();
            }

            LandMasses = cLandMasses.ToArray();

//            BeginStep("Recalculating landmasses edges...", m_aLandMasses.Length);
            foreach (LandMass pLandMass in LandMasses)
            {
                pLandMass.Finish(LocationsGrid.CycleShift);
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

            BeginStep("Building  continents...", m_iContinentsCount + m_iContinentsCount);
            foreach (LandMass pLandMass in LandMasses)
            {
                if (pLandMass.Forbidden || (m_bGreatOcean && pLandMass.HaveForbiddenBorders()))
                {
                    pLandMass.IsWater = true;
                    iOceanCount += pLandMass.Contents.Count;
                    cChances[pLandMass] = 0;
                }
                else
                {
                    cChances[pLandMass] = pLandMass.MaxSize > 0 ? 66 : 100;
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

                if (pChoosenLM?.MaxSize == -1)
                {
                    Continent pContinent = new Continent();
                    pContinent.Start(pChoosenLM);
                    cContinents.Add(pContinent);

                    if (pChoosenLM.MaxSize == -1)
                        iRealContinentsCount++;

                    cChances[pChoosenLM] = 0;
                    foreach (LandMass pLandMass in pChoosenLM.BorderWithKeys)
                    {
                        if (pLandMass.Forbidden)
                            continue;
                        cChances[pLandMass] = 0;
                    }
                    iLandGrowActual += pChoosenLM.Contents.Count;

                    iCounter = 0;
                }
                else
                {
                    iCounter++;
                }

                ProgressStep();
            }

//            BeginStep("Growing continents seeds...", cContinents.Count); 
            bool bContinue = true;
            while (iLandGrowActual < iLandGrowLimit && bContinue)
            {
                bContinue = false;
                foreach (Continent pCont in cContinents)
                {
                    LandMass pAddon = pCont.Grow();
                    if (pAddon != null)
                    {
                        bContinue = true;
                        iLandGrowActual += pAddon.Contents.Count;

                        cChances[pAddon] = 0;
                        foreach (LandMass pLM in pAddon.BorderWithKeys)
                        {
                            if (pLM.Forbidden)
                                continue;
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
                if (cChances[pLandMass] > 0 && pLandMass.MaxSize > 0 && Rnd.OneChanceFrom(pLandMass.MaxSize+1))
                {
                    Continent pContinent = new Continent();
                    pContinent.Start(pLandMass);
                    cContinents.Add(pContinent);

                    cChances[pLandMass] = 0;
                    foreach (LandMass pLM in pLandMass.BorderWithKeys)
                    {
                        if (pLM.Forbidden)
                            continue;
                        cChances[pLM] = 0;
                    }
                }
                ProgressStep();
            }

            Contents = cContinents.ToArray();

            BeginStep("Recalculating continents edges...", Contents.Length);
            foreach (Continent pCont in Contents)
            {
                pCont.Finish(LocationsGrid.CycleShift);
                pCont.SetOwner(this);
                ProgressStep();
            }

            foreach (LandMass pLandMass in LandMasses)
            {
                if (!pLandMass.HasOwner())
                    pLandMass.IsWater = true;
            }
        }

        /// <summary>
        /// Формирует шельфовое "мелководье" там, где океанические и континентальные тектонические плиты расходятся
        /// </summary>
        /// <param name="BeginStep"></param>
        /// <param name="ProgressStep"></param>
        private void AddCoastral(BeginStepDelegate BeginStep, ProgressStepDelegate ProgressStep)
        {
            BeginStep("Setting coastral regions...", Lands.Length);
            foreach (Land pLand in Lands)
            {
                if (pLand.LandType == LandTypes.Ocean || pLand.LandType == LandTypes.Coastral)
                {
                    LandMass pLandMass = pLand.GetOwner();

                    float fMinCollision = float.MaxValue;
                    Land pBestLand = null;
                    foreach (Land pLinkedLand in pLand.BorderWithKeys)
                    {
                        if (pLinkedLand.Forbidden)
                            continue;

                        if (pLinkedLand.LandType != null)
                            continue;

                        LandMass pLinkedLandMass = pLinkedLand.GetOwner();

                        if (pLandMass != null && pLinkedLandMass != null && pLandMass != pLinkedLandMass)
                        {
                            float fDriftedX1 = pLand.X + (float)pLandMass.Drift.X;
                            float fDriftedY1 = pLand.Y + (float)pLandMass.Drift.Y;
                            float fDriftedX2 = pLinkedLand.X + (float)pLinkedLandMass.Drift.X;
                            float fDriftedY2 = pLinkedLand.Y + (float)pLinkedLandMass.Drift.Y;
                            if (Math.Abs(fDriftedX1 - fDriftedX2) > LocationsGrid.CycleShift / 2)
                            {
                                if (fDriftedX1 < 0)
                                    fDriftedX2 -= LocationsGrid.CycleShift;
                                else
                                    fDriftedX2 += LocationsGrid.CycleShift;
                            }

                            float fDriftedDist = (float)Math.Sqrt((fDriftedX1 - fDriftedX2) * (fDriftedX1 - fDriftedX2) + (fDriftedY1 - fDriftedY2) * (fDriftedY1 - fDriftedY2));

                            float fCollision = pLand.DistanceTo(pLinkedLand, LocationsGrid.CycleShift) - fDriftedDist;

                            if (fCollision < fMinCollision)
                            {
                                fMinCollision = fCollision;
                                pBestLand = pLinkedLand;
                            }
                        }
                    }

                    if (pBestLand == null)
                        continue;

                    if (fMinCollision < 1)// || Rnd.OneChanceFrom(2))
                        pLand.LandType = LandTypes.Coastral;

                    if (fMinCollision < -1.25 && pLandMass.Contents.Count > 3)// && (bCoast || Rnd.ChooseOne(iMaxElevation - pLM1.m_pDrift, pLM1.m_pDrift)))
                    {
                        foreach (Land pLink in pLand.BorderWithKeys)
                        {
                            if (pLink.Forbidden)
                                continue;

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
            BeginStep("Setting mountain regions...", Lands.Length);
            foreach (Land pLand in Lands)
            {
                if (pLand.LandType == null)
                {
                    LandMass pLM1 = pLand.GetOwner();

                    float fMaxCollision = float.MinValue;
                    foreach (Land pLink in pLand.BorderWithKeys)
                    {
                        if (pLink.Forbidden)
                            continue;

                        LandMass pLM2 = pLink.GetOwner();

                        if (pLM1 != null && pLM2 != null && pLM1 != pLM2)
                        {
                            float fDriftedX1 = pLand.X + (float)pLM1.Drift.X;
                            float fDriftedY1 = pLand.Y + (float)pLM1.Drift.Y;
                            float fDriftedX2 = pLink.X + (float)pLM2.Drift.X;
                            float fDriftedY2 = pLink.Y + (float)pLM2.Drift.Y;
                            if (Math.Abs(fDriftedX1 - fDriftedX2) > LocationsGrid.CycleShift / 2)
                            {
                                if (fDriftedX1 < 0)
                                    fDriftedX2 -= LocationsGrid.CycleShift;
                                else
                                    fDriftedX2 += LocationsGrid.CycleShift;
                            }

                            float fDriftedDist = (float)Math.Sqrt((fDriftedX1 - fDriftedX2) * (fDriftedX1 - fDriftedX2) + (fDriftedY1 - fDriftedY2) * (fDriftedY1 - fDriftedY2));

                            float fCollision = pLand.DistanceTo(pLink, LocationsGrid.CycleShift) - fDriftedDist;

                            if (fCollision > fMaxCollision)
                            {
                                fMaxCollision = fCollision;
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
            BeginStep("Setting lake regions...", Lands.Length);
            foreach (Land pLand in Lands)
            {
                if (pLand.LandType == null)
                {
                    bool bCouldBe = true;
                    foreach (Land pLink in pLand.BorderWithKeys)
                    {
                        if (pLink.Forbidden)
                            continue;

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
            return 1 - Math.Abs((m_iEquator - pVertex.Y) / m_iPole);
        }

        private void CalculateHumidity(BeginStepDelegate BeginStep, ProgressStepDelegate ProgressStep)
        {
            BeginStep("Calculating humidity...", Lands.Length);

            List<Land> cHumidityFront = new List<Land>();
            foreach (Land pLand in Lands)
            {
                if (pLand.IsWater)
                {
                    foreach (Land pLinkedLand in pLand.BorderWithKeys)
                    {
                        if (!pLinkedLand.Forbidden && !pLinkedLand.IsWater)
                        {
                            //удалённость точки от экватора 0..1
                            float fTemperatureMod = 1 - GetTemperature(pLinkedLand);

                            fTemperatureMod = 1.5f / (fTemperatureMod * fTemperatureMod) + 300 * fTemperatureMod * fTemperatureMod;
                            pLinkedLand.Humidity = (int)(fTemperatureMod - 5 + Rnd.Get(10.0f));

                            if (pLinkedLand.LandType != null && pLinkedLand.LandType.Environment.HasFlag(Environment.Barrier))
                                pLinkedLand.Humidity /= 2;

                            if (!cHumidityFront.Contains(pLinkedLand))
                                cHumidityFront.Add(pLinkedLand);
                        }
                    }
                    pLand.Humidity = 100;
                }

                ProgressStep();
            }

            List<Land> cNewWave = new List<Land>();

            Land[] aHumidityFront = cHumidityFront.ToArray();
            do
            {
                cNewWave.Clear();
                foreach (Land pLand in aHumidityFront)
                {
                    if (pLand.Humidity > 0)
                    {
                        foreach (Land pLinkedLand in pLand.BorderWithKeys)
                        {
                            if (pLinkedLand.Forbidden)
                                continue;

                            if (!pLinkedLand.IsWater && pLinkedLand.Humidity == 0)
                            {
                                pLinkedLand.Humidity = pLand.Humidity - 10 - Rnd.Get(5);

                                if (pLinkedLand.LandType != null && pLinkedLand.LandType.Environment.HasFlag(Environment.Barrier))
                                    pLinkedLand.Humidity /= 2;

                                if (!cNewWave.Contains(pLinkedLand))
                                    cNewWave.Add(pLinkedLand);
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
            foreach (Land pLand in Lands)
            {
                if (!pLand.HasOwner() || pLand.GetOwner().IsWater)
                    pLand.LandType = LandTypes.Ocean;
                else
                    pLand.LandType = null;
            }

            AddCoastral(BeginStep, ProgressStep);

            AddMountains(BeginStep, ProgressStep);

            AddLakes(50, BeginStep, ProgressStep);

            CalculateHumidity(BeginStep, ProgressStep);

            BeginStep("Setting other regions types...", Lands.Length);
            //Assign other land types
            foreach (Land pLand in Lands)
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
        public float MaxDepth { get; private set; } = 0;
        public float MaxHeight { get; private set; } = 0;

        protected void CalculateElevations(BeginStepDelegate BeginStep, ProgressStepDelegate ProgressStep)
        {
            BeginStep("Calculating elevation...", LocationsGrid.Locations.Length);

            List<Location> cOcean = new List<Location>();
            List<Location> cLand = new List<Location>();

            //Плясать будем от шельфа - у него фиксированная глубина -1
            //весь остальной океан - глубже, вся суша - выше
            foreach (Land pLand in Lands)
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

                        foreach (Location pLink in pLoc.BorderWithKeys)
                        {
                            if (pLink.Forbidden || !pLink.HasOwner() || pLink.GetOwner() == pLand)
                                continue;

                            if (pLink.GetOwner().LandType == LandTypes.Ocean)
                            {
                                if (!cOcean.Contains(pLink))
                                    cOcean.Add(pLink);
                            }
                            else
                            {
                                if (pLink.GetOwner().LandType != LandTypes.Coastral && !cLand.Contains(pLink))
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
                        foreach (Location pLink in pLoc.BorderWithKeys)
                        {
                            if (pLink.Forbidden || !pLink.HasOwner() || pLink.GetOwner() == pLand)
                                continue;

                            if (pLink.GetOwner().LandType != LandTypes.Ocean &&
                                pLink.GetOwner().LandType != LandTypes.Coastral)
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
            MaxDepth = -2;
            while (cOcean.Count > 0)
            {
                cWaveFront.Clear();
                foreach (Location pLoc in cOcean)
                {
                    pLoc.H = MaxDepth;
                    ProgressStep();

                    foreach (Location pLink in pLoc.BorderWithKeys)
                    {
                        if (pLink.Forbidden || !pLink.HasOwner() || !float.IsNaN(pLink.H))
                            continue;

                        if (pLink.GetOwner().LandType == LandTypes.Ocean)
                        {
                            cWaveFront.Add(pLink);
                            pLink.H = -1;
                        }
                    }
                }

                cOcean.Clear();
                cOcean.AddRange(cWaveFront);

                MaxDepth--;
            }

            //с сушей сложнее...
            MaxHeight = 0;
            Dictionary<float, Location> cUnfinished = new Dictionary<float, Location>();
            SortedSet<float> cHeights = new SortedSet<float>();

            while (cLand.Count > 0 || cUnfinished.Count > 0)
            {
                foreach (Location pLoc in cLand)
                {
                    float fNewHeight;
                    do
                    {
                        float fLinkElevation = GetElevationRnd(pLoc.GetOwner().LandType);
                        fNewHeight = MaxHeight + fLinkElevation;
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

                    foreach (Location pLink in pLoc.BorderWithKeys)
                    {
                        if (pLink.Forbidden || !pLink.HasOwner() || !float.IsNaN(pLink.H))
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
                    float fMinElevation = fMinHeight - MaxHeight;
                    MaxHeight += fMinElevation;
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

            foreach (Location pLoc in LocationsGrid.Locations)
            {
                if (pLoc.Forbidden || !pLoc.HasOwner())
                    continue;

                if (pLoc.H < 0)
                {
                    var pLine = pLoc.FirstLine;
                    do
                    {
                        if (pLine.Point1.H > 0)
                            pLine.Point1.H = 0;

                        if (pLine.Point2.H > 0)
                            pLine.Point2.H = 0;

                        //if (pLine.m_pMidPoint.m_fHeight > 0)
                        //    pLine.m_pMidPoint.m_fHeight = 0;

                        //if (pLine.m_pInnerPoint.m_fHeight > 0)
                        //    pLine.m_pInnerPoint.m_fHeight = 0;

                        pLine = pLine.Next;
                    }
                    while (pLine != pLoc.FirstLine);
                }

                if (pLoc.H > 0)
                {
                    var pLine = pLoc.FirstLine;
                    do
                    {
                        if (pLine.Point1.H < 0)
                            pLine.Point1.H = 0;

                        if (pLine.Point2.H < 0)
                            pLine.Point2.H = 0;

                        //if (pLine.m_pMidPoint.m_fHeight < 0)
                        //    pLine.m_pMidPoint.m_fHeight = 0;

                        //if (pLine.m_pInnerPoint.m_fHeight < 0)
                        //    pLine.m_pInnerPoint.m_fHeight = 0;

                        pLine = pLine.Next;
                    }
                    while (pLine != pLoc.FirstLine);
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
            foreach (Location pLoc in LocationsGrid.Locations)
            {
                if (pLoc.Forbidden || !pLoc.HasOwner())
                    continue;

                float fLokElevation = pLoc.GetOwner().LandType.Elevation;
                if (fLokElevation <= fMaxElevation)
                {
                    float fTotal = pLoc.H;
                    float fTotalWeight = 1;
                    foreach (Location pLink in pLoc.BorderWithKeys)
                    {
                        if (pLink.Forbidden || !pLink.HasOwner())
                            continue;

                        float fLinkElevation = pLink.GetOwner().LandType.Elevation;
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
            double widthDivisor = 0.5 / (double)LocationsGrid.RX;
            double heightDivisor = 0.5 / (double)LocationsGrid.RY;

            float vMin = 0;
            float vMax = 0;
            foreach (Location pLoc in LocationsGrid.Locations)
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

                if (pLoc.H > 0)
                {
                    float fLinkElevation = Math.Min(pLoc.H - 0.5f, 10);
                    pLoc.H += v * fLinkElevation;
                }
                else
                {
                    float fLinkElevation = Math.Max(pLoc.H + 0.5f, -10);
                    pLoc.H -= v * fLinkElevation;
                }
            }
        }

        private float GetElevationRnd(LandTypeInfo pLTI)
        {
            return pLTI.Elevation / 2 + Rnd.Get(pLTI.Elevation);
        }

        public List<TransportationLinkBase> TransportGrid { get; } = new List<TransportationLinkBase>();
        public List<TransportationLinkBase> LandsTransportGrid { get; } = new List<TransportationLinkBase>();
        public List<TransportationLinkBase> LandMassTransportGrid { get; } = new List<TransportationLinkBase>();

        /// <summary>
        /// Устанавливает возможность перехода между указанными локациями при поиске пути.
        /// </summary>
        /// <param name="pNode1">первая локация</param>
        /// <param name="pNode2">вторая локация</param>
        protected TransportationLinkBase SetLink(TransportationNode pNode1, TransportationNode pNode2)
        {
            if (!pNode1.Links.ContainsKey(pNode2))
            {
                if (!pNode2.Links.ContainsKey(pNode1))
                {
                    TransportationLinkBase pLink = null;
                    if (pNode1 is Location && pNode2 is Location)
                        pLink = new TransportationLinkBase(pNode1 as Location, pNode2 as Location, LocationsGrid.CycleShift);
                    if (pNode1 is Land && pNode2 is Land)
                        pLink = new TransportationLinkBase(pNode1 as Land, pNode2 as Land, LocationsGrid.CycleShift);
                    if (pNode1 is LandMass && pNode2 is LandMass)
                        pLink = new TransportationLinkBase(pNode1 as LandMass, pNode2 as LandMass, LocationsGrid.CycleShift);

                    if (pLink == null)
                        throw new InvalidOperationException("Can't create transportation link between " + pNode1.ToString() + " and " + pNode2.ToString());

                    pNode1.Links[pNode2] = pLink;
                    pNode2.Links[pNode1] = pLink;
                    if (pNode1 is Location && pNode2 is Location)
                        TransportGrid.Add(pLink);
                    if (pNode1 is Land && pNode2 is Land)
                        LandsTransportGrid.Add(pLink);
                    if (pNode1 is LandMass && pNode2 is LandMass)
                        LandMassTransportGrid.Add(pLink);

                    return pLink;
                }
                else
                {
                    pNode1.Links[pNode2] = pNode2.Links[pNode1];
                    return pNode1.Links[pNode2];
                }
            }
            else
                if (!pNode2.Links.ContainsKey(pNode1))
                {
                    pNode2.Links[pNode1] = pNode1.Links[pNode2];
                    return pNode2.Links[pNode1];
                }

            return pNode2.Links[pNode1];
        }

        protected TransportationLinkBase SetLink(TransportationNode pNode1, TransportationNode pNode2, TransportationLinkBase pLink)
        {
            if (!pNode1.Links.ContainsKey(pNode2))
            {
                if (!pNode2.Links.ContainsKey(pNode1))
                {
                    pNode1.Links[pNode2] = pLink;
                    pNode2.Links[pNode1] = pLink;
                    if (pNode1 is Location && pNode2 is Location)
                        TransportGrid.Add(pLink);
                    if (pNode1 is Land && pNode2 is Land)
                        LandsTransportGrid.Add(pLink);
                    if (pNode1 is LandMass && pNode2 is LandMass)
                        LandMassTransportGrid.Add(pLink);

                    return pLink;
                }
                else
                {
                    pNode1.Links[pNode2] = pNode2.Links[pNode1];
                    return pNode1.Links[pNode2];
                }
            }
            else
                if (!pNode2.Links.ContainsKey(pNode1))
                {
                    pNode2.Links[pNode1] = pNode1.Links[pNode2];
                    return pNode2.Links[pNode1];
                }

            return pNode2.Links[pNode1];
        }

        private void BuildTransportGrid(BeginStepDelegate BeginStep, ProgressStepDelegate ProgressStep)
        {
            BeginStep("Building transportation links...", LocationsGrid.Locations.Length + Lands.Length + LandMasses.Length);

            foreach (Location pLoc in LocationsGrid.Locations)
            {
                if (pLoc.Forbidden || !pLoc.HasOwner())
                    continue;

                foreach (Location pLink in pLoc.BorderWithKeys)
                {
                    if (pLink.Forbidden || !pLink.HasOwner())
                        continue;

                    TransportationLinkBase pTransLink = SetLink(pLoc, pLink);
                    pTransLink.Sea = pLink.GetOwner().IsWater && pLoc.GetOwner().IsWater;
                    pTransLink.Embark = pLink.GetOwner().IsWater != pLoc.GetOwner().IsWater;
                }

                ProgressStep();
            }

            foreach (Land pLand in Lands)
            {
                if (pLand.Forbidden || !pLand.HasOwner())
                    continue;

                foreach (Land pLinked in pLand.BorderWithKeys)
                {
                    if (pLinked.Forbidden || !pLinked.HasOwner())
                        continue;

                    TransportationLinkBase pLink = SetLink(pLand, pLinked);
                    pLink.Sea = pLinked.GetOwner().IsWater && pLand.GetOwner().IsWater;
                    pLink.Embark = pLinked.GetOwner().IsWater != pLand.GetOwner().IsWater;
                }

                ProgressStep();
            }
            foreach (LandMass pLandMass in LandMasses)
            {
                if (pLandMass.Forbidden)
                    continue;

                foreach (LandMass pLinked in pLandMass.BorderWithKeys)
                {
                    if (pLinked.Forbidden)
                        continue;

                    TransportationLinkBase pLink = SetLink(pLandMass, pLinked);
                    pLink.Sea = !pLinked.HasOwner() && !pLandMass.HasOwner();
                    pLink.Embark = (pLinked.HasOwner()) != (pLandMass.HasOwner());
                }

                ProgressStep();
            }
        }

        private static int s_iGreenLightCode = 0;

        public static ShortestPath FindReallyBestPath(Location pStart, Location pFinish, float fCycleShift, bool bNavalOnly)
        {
            ShortestPath pBestPath1 = FindBestPath(pStart, pFinish, fCycleShift, bNavalOnly);
            ShortestPath pBestPath2 = FindBestPath(pFinish, pStart, fCycleShift, bNavalOnly);

            if (pBestPath1 == null ||
                pBestPath1.m_aNodes.Length == 0 ||
                (pBestPath2 != null &&
                 pBestPath2.m_aNodes.Length != 0 &&
                 pBestPath2.m_fLength < pBestPath1.m_fLength))
            {
                return pBestPath2;
            }

            return pBestPath1;
        }

        public static ShortestPath FindBestPath(Location pStart, Location pFinish, float fCycleShift, bool bNavalOnly)
        {
            s_iGreenLightCode++;
            ShortestPath pLMPath = new ShortestPath(pStart.GetOwner().GetOwner(), pFinish.GetOwner().GetOwner(), fCycleShift, -1, bNavalOnly);
            foreach (TransportationNode pNode in pLMPath.m_aNodes)
            {
                LandMass pLandMass = pNode as LandMass;
                foreach (Land pLand in pLandMass.Contents)
                {
                    pLand.GreenLightCode = s_iGreenLightCode;
                }
                foreach (LandMass pLinkedLandMass in pLandMass.BorderWithKeys)
                {
                    if (!pLinkedLandMass.Forbidden)
                    {
                        foreach (Land pLand in pLinkedLandMass.Contents)
                            pLand.GreenLightCode = s_iGreenLightCode;
                    }
                }
            }

            ShortestPath pLandsPath = new ShortestPath(pStart.GetOwner(), pFinish.GetOwner(), fCycleShift, s_iGreenLightCode, bNavalOnly);
            foreach (TransportationNode pNode in pLandsPath.m_aNodes)
            {
                Land pLand = pNode as Land;
                foreach (Location pLoc in pLand.Contents)
                {
                    pLoc.GreenLightCode = s_iGreenLightCode;
                }
                foreach (Land pLinkedLand in pLand.BorderWithKeys)
                {
                    if (!pLinkedLand.Forbidden)
                    {
                        foreach (Location pLoc in pLinkedLand.Contents)
                            pLoc.GreenLightCode = s_iGreenLightCode;
                    }
                }
            }

            ShortestPath pBestPath = new ShortestPath(pStart, pFinish, fCycleShift, s_iGreenLightCode, bNavalOnly);

            return pBestPath;
        }
    }
}
