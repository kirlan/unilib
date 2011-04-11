using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;
using System.ComponentModel;

[assembly: CLSCompliant(true)]

namespace MapGen
{
    public class CFractalWorld
    {
        private CFractalMap m_pMap;
        /// <summary>
        /// двухмерная карта регионов
        /// </summary>
        public CFractalMap Map
        {
            get { return m_pMap; }
        }

        private CParallel[] m_pAtlas;
        /// <summary>
        /// альтернативное хранение информации
        /// </summary>
        public CParallel[] Atlas
        {
            get { return m_pAtlas; }
        }

        public CFractalWorld()
        {}

        private int m_iDepth;
        /// <summary>
        /// глубина фрактала
        /// </summary>
        public int Depth
        {
            get { return m_iDepth; }
        }

        public class CreationArguments
        {
            private int m_iInitialDepth;
            /// <summary>
            /// стартовая глубина фрактала
            /// </summary>
            public int InitialDepth
            {
                get { return m_iInitialDepth; }
            }

            private int m_iFinalDepth;
            /// <summary>
            /// целевая глубина фрактала
            /// </summary>
            public int FinalDepth
            {
                get { return m_iFinalDepth; }
            }

            private int m_iWaterPercent;
            /// <summary>
            /// какой процент поверхности планеты покрыт водой
            /// </summary>
            public int WaterPercent
            {
                get { return m_iWaterPercent; }
            }

            private int m_iContinentsCount;
            /// <summary>
            /// количество континенотов
            /// </summary>
            public int ContinentsCount
            {
                get { return m_iContinentsCount; }
            }

            private int m_iIslandsCount;
            /// <summary>
            /// количество островов
            /// </summary>
            public int IslandsCount
            {
                get { return m_iIslandsCount; }
            }

            private int m_iSmoothness;
            /// <summary>
            /// коэффициент сглаживания
            /// </summary>
            public int Smoothness
            {
                get { return m_iSmoothness; }
                set { m_iSmoothness = value; }
            }

            /// <summary>
            /// конструктор
            /// </summary>
            /// <param name="iInitialDepth">стартовая глубина фрактала</param>
            /// <param name="iFinalDepth">целевая глубина фрактала</param>
            /// <param name="iWaterPercent">какой процент поверхности планеты покрыт водой</param>
            /// <param name="iContinentsCount">количество континенотов</param>
            /// <param name="iIslandsCount">количество островов</param>
            /// <param name="iSmoothness">коэффициент сглаживания</param>
            public CreationArguments(int iInitialDepth, int iFinalDepth, int iWaterPercent, int iContinentsCount, int iIslandsCount, int iSmoothness)
            {
                m_iInitialDepth = iInitialDepth;
                m_iFinalDepth = iFinalDepth;
                m_iWaterPercent = iWaterPercent;
                m_iContinentsCount = iContinentsCount;
                m_iIslandsCount = iIslandsCount;
                m_iSmoothness = iSmoothness;
            }
        }

        private int iTotalStepsCount;
        private int iCurrentStep;

        private void Update(BackgroundWorker bw, string message)
        {
            if (bw == null)
                return;

            int percent = (int)(100 * iCurrentStep / iTotalStepsCount);

            bw.ReportProgress(percent, message);

            iCurrentStep++;
        }

        public void CreateSphereDebug(CreationArguments args)
        {
            Clear();

            int iTrueHeight, iTrueWidth, iTotalLand;

            m_iDepth = args.InitialDepth - 1;

            do
            {
                m_iDepth++;

                iTrueHeight = (int)(3 * Math.Pow(2, m_iDepth));
                iTrueWidth = (int)(10 * Math.Pow(2, m_iDepth));//HEIGHT*5;

                iTotalLand = iTrueWidth * iTrueHeight * (100 - 3 * args.WaterPercent / 4) * 2 / 300;    // общая площадь суши
            }
            while (iTotalLand <= args.ContinentsCount);

            iTotalStepsCount = 0;
            iTotalStepsCount++; //Creation and initialization
            iTotalStepsCount += args.ContinentsCount; //Adding continents
            iTotalStepsCount += args.IslandsCount; //Adding islands
            iTotalStepsCount++; //BuildSeas();
            int TotalIteration = args.FinalDepth - m_iDepth;
            if (TotalIteration > 0)
                iTotalStepsCount += TotalIteration; //Rising fractal depth
            iTotalStepsCount++; //BalanceWater();
            iTotalStepsCount++; //Smoothing
            iTotalStepsCount++; //FixExits();
            iTotalStepsCount++; //BuildAtlas();

            iCurrentStep = 0;

            m_pMap = new CFractalMap(iTrueWidth, iTrueHeight);
            m_pMap.Initialize(0, -1);

            // добавляем континенты
            AddContinents(null, iTotalLand, args.ContinentsCount);

            // строим рельеф морского дна
            m_pMap.BuildSeas();
        }

        /// <summary>
        /// создать икасаэдрический мир
        /// </summary>
        /// <param name="args">параметры генерации мира</param>
        public void CreateSphere(CreationArguments args, BackgroundWorker bw)
        {
            Clear();

            int iTrueHeight, iTrueWidth, iTotalLand, iTotalWater;

            m_iDepth = args.InitialDepth - 1;

            do
            {
                m_iDepth++;

                iTrueHeight = (int)(3 * Math.Pow(2, m_iDepth));
                iTrueWidth = (int)(10 * Math.Pow(2, m_iDepth));//HEIGHT*5;

                iTotalLand = iTrueWidth * iTrueHeight * (100 - 3*args.WaterPercent/4) * 2 / 300;    // общая площадь суши
            }
            while (iTotalLand <= args.ContinentsCount);

            iTotalStepsCount = 0;
            iTotalStepsCount++; //Creation and initialization
            iTotalStepsCount += args.ContinentsCount; //Adding continents
            iTotalStepsCount += args.IslandsCount; //Adding islands
            iTotalStepsCount++; //BuildSeas();
            int TotalIteration = args.FinalDepth - m_iDepth;
            if (TotalIteration > 0)
                iTotalStepsCount += TotalIteration; //Rising fractal depth
            iTotalStepsCount++; //BalanceWater();
            iTotalStepsCount++; //Smoothing
            iTotalStepsCount++; //FixExits();
            iTotalStepsCount++; //BuildAtlas();

            iCurrentStep = 0;

            Update(bw, "Creating empty world...");
            m_pMap = new CFractalMap(iTrueWidth, iTrueHeight);
            m_pMap.Initialize(0, -1);

            // добавляем континенты
            AddContinents(bw, iTotalLand, args.ContinentsCount);
            int i;

            // строим рельеф морского дна
            Update(bw, "Building seas...");
            m_pMap.BuildSeas();

            // наращиваем глубину фрактала
            int iFinalDepth = args.FinalDepth;
            //if (TotalIteration > 1)
            //    iFinalDepth--;

            int iCount = 1;
            while (m_iDepth < iFinalDepth)
            {
                Update(bw, string.Format("Increasing map detalization. Iteration {0} of {1}...", iCount++, TotalIteration));
                IncreaseDepth();
            }

            Update(bw, "Fixing links between regions...");
            m_pMap.FixExits();

            Update(bw, "Balancing ocean level...");
            m_pMap.BalanceWater(args.WaterPercent + 1);

            iTrueHeight = (int)(3 * Math.Pow(2, m_iDepth));
            iTrueWidth = (int)(10 * Math.Pow(2, m_iDepth));//HEIGHT*5;
            iTotalWater = iTrueWidth * iTrueHeight * args.WaterPercent * 2 / 300;    // общая площадь морей и океанов
            // добавляем архипелаги
            AddArchipelagos(bw, iTotalWater / 100, args.IslandsCount);

            //Update(bw, "Balancing ocean level...");
            //m_pMap.BalanceWater(args.WaterPercent);

            //if (TotalIteration > 1)
            //{
            //    Update(bw, string.Format("Increasing map detalization. Iteration {0} of {1}...", TotalIteration, TotalIteration));
            //    IncreaseDepth();
            //}

            //Update(bw, "Fixing links between regions...");
            //m_pMap.BalanceWater(args.WaterPercent);

            // сглаживаем
            Update(bw, "Smoothing map...");
            for (i = 0; i < args.Smoothness * args.Smoothness; i++)
            {
                m_pMap.Smooth(m_iDepth);
            }

            m_pMap.FixExits();

            m_pMap.ClearLoneRegions(m_iDepth);

            Update(bw, "Building atlas...");
            m_pAtlas = m_pMap.BuildAtlas();
        }

        public void FinalizeDebug(int iWaterPercent, int iSmooth)
        {
            m_pMap.FixExits();

            m_pMap.BalanceWater(iWaterPercent);

            for (int i = 0; i < iSmooth * iSmooth; i++)
            {
                m_pMap.Smooth(m_iDepth);
            }

            m_pMap.FixExits();

            m_pMap.ClearLoneRegions(m_iDepth);

            m_pAtlas = m_pMap.BuildAtlas();
        }

        private void AddContinents(BackgroundWorker bw, int iTotalSize, int iCount)
        {
            for (int i = 0; i < iCount; i++)
            {
                Update(bw, string.Format("Adding continent {0} of {1}...", i + 1, iCount));
                int iAverageContinentSize = iTotalSize / (iCount + 1 - i);   // средняя площадь одного континента
                if (iAverageContinentSize < 1)
                {
                    iAverageContinentSize = 1;
                }

                int iRealSize = iAverageContinentSize + Rnd.Get(iAverageContinentSize); // реальная площадь каждого конкретного континента может отличкаться от средней

                if (iRealSize > iTotalSize - (iCount - i) * iAverageContinentSize / 2)
                    iRealSize = iTotalSize - (iCount - i) * iAverageContinentSize / 2;

                if (i == iCount - 1)
                    iRealSize = iTotalSize;

                //if (iRealSize > iTotalSize - iCount + i)
                //{
                //    iRealSize = iTotalSize - iCount + i;
                //}
                if (iRealSize < 1)
                {
                    iRealSize = 1;
                }
                m_pMap.CreateLandMass(iRealSize, true);
                iTotalSize -= iRealSize;
            }

            bool groving = false;
            do
            {
                groving = false;
                foreach(CLandMass land in m_pMap.LandMass)
                {
                    if (m_pMap.LandGrowUp(land))
                        groving = true;
                    else
                    {
                        if (land.SizeNeeded > land.Regions.Count)
                            m_pMap.LandGrowUp(land);
                    }
                }
            }
            while(groving);

            return;

            // добавляем континенты
            for (int i = 0; i < iCount; i++)
            {
                Update(bw, string.Format("Adding continent {0} of {1}...", i + 1, iCount));

                int iAverageContinentSize = iTotalSize / (iCount - i);   // средняя площадь одного континента
                if (iAverageContinentSize < 1)
                {
                    iAverageContinentSize = 1;
                }

                int iRealSize = iAverageContinentSize + Rnd.Get(iAverageContinentSize); // реальная площадь каждого конкретного континента может отличкаться от средней
                if (iRealSize > iTotalSize - iCount + i)
                {
                    iRealSize = iTotalSize - iCount + i;
                }
                if (iRealSize < 1)
                {
                    iRealSize = 1;
                }
                int iMass = m_pMap.AddLand(iRealSize, true);
                iTotalSize -= iMass;
            }
        }

        private void AddArchipelagos(BackgroundWorker bw, int iTotalSize, int iCount)
        {
            // добавляем архипелаги
            int iAverageIslandSize = iTotalSize / iCount;
            for (int i = 0; i < iCount; i++)
            {
                Update(bw, string.Format("Adding island {0} of {1}...", i + 1, iCount));
                int iRealSize = iAverageIslandSize / 4 + Rnd.Get(iAverageIslandSize);
                if (iRealSize > iTotalSize - iCount + i)
                {
                    iRealSize = iTotalSize - iCount + i;
                }
                if (iRealSize <= m_iDepth)
                {
                    iRealSize = m_iDepth+1;
                }
                iTotalSize -= m_pMap.AddLand(iRealSize, Rnd.OneChanceFrom(6) ? true : false);

                if (iTotalSize <= 0)
                    break;
            }
        }

        /// <summary>
        /// всё обнулить
        /// </summary>
        private void Clear()
        {
            if (m_pAtlas != null)
            {
                for (int i = 0; i < m_pMap.Height; i++)
                {
                    m_pAtlas[i].Regions = null;
                }
                m_pAtlas = null;
            }

            if (m_pMap != null)
                m_pMap.Clear();
            m_pMap = null;
        }

        ///// <summary>
        ///// наростить глубину фрактала до заданной величины
        ///// </summary>
        ///// <param name="iNewDepth">новая глубина фрактала</param>
        //public void GoDeeper(int iNewDepth)
        //{
        //    if (iNewDepth <= m_iDepth)
        //    {
        //        return;
        //    }

        //    do
        //    {
        //        IncreaseDepth();
        //    }
        //    while (iNewDepth > m_iDepth);

        //    m_pMap.FixExits();
        //    m_pAtlas = m_pMap.BuildAtlas();
        //}

        /// <summary>
        /// Наращивает глубину фрактала на 1
        /// </summary>
        public void IncreaseDepth()
        {
            int iNewHeight = m_pMap.Height * 2;
            int iNewWidth = m_pMap.Width * 2;

            // строим новую карту с более частой координатной сеткой
            CFractalMap pNewMap = new CFractalMap(iNewWidth, iNewHeight);

            // переносим на новую карту регионы со старой
            pNewMap.CopyFrom(m_pMap);

            Clear();
            m_pMap = pNewMap;
            m_iDepth++;

            //	for(int g=0; g<3; g++)
            m_pMap.FixExits();
            m_pMap.Smooth(m_iDepth);
        }
    	
        /*
        //TWay* GetCource(int x1, int y1, int x2, int y2);
        //CMapPoint GetCourcePoint(int x1, int y1, int x2, int y2, double speed, int time);
        
        /// <summary>
        /// Получить декартовы координаты по угловым
        /// </summary>
        /// <param name="l">долгота</param>
        /// <param name="w">широта</param>
        /// <returns>точка в декартовых координатах</returns>
        CMapPoint GetMapCoord(double l, double w)
        {
            TMapPoint result;

            double minDist = 32000;

            int x, y;
            for (y = 0; y < HEIGHT; y++)
            {
                if (m_pAtlas[y].minW < w && m_pAtlas[y].maxW >= w)
                {
                    int minx, maxx;
                    x = m_pAtlas[y].iLength * l / (2.0 * M_PI);
                    minx = x - 1;
                    maxx = x + 1;
                    for (int i = minx; i < maxx; i++)
                    {
                        if (i < 0)
                        {
                            i += m_pAtlas[y].iLength;
                        }
                        if (i >= m_pAtlas[y + 1].iLength)
                        {
                            i -= m_pAtlas[y].iLength;
                        }

                        Vector p1, p2;

                        p1.x = cos(m_pAtlas[y].m_pRegions[i]->l) * cos(m_pAtlas[y].m_pRegions[i]->w);
                        p1.y = sin(m_pAtlas[y].m_pRegions[i]->l) * cos(m_pAtlas[y].m_pRegions[i]->w);

                        p1.z = sin(fabs(m_pAtlas[y].m_pRegions[i]->w));
                        if (m_pAtlas[y].m_pRegions[i]->w < 0)
                            p1.z = -p1.z;

                        p2.x = cos(l) * cos(w);
                        p2.y = sin(l) * cos(w);

                        p2.z = sin(fabs(w));
                        if (w < 0)
                            p2.z = -p2.z;

                        double dist = !(p2 - p1);

                        if (dist < minDist)
                        {
                            minDist = dist;
                            result.x = m_pAtlas[y].m_pRegions[i]->x;
                            result.y = m_pAtlas[y].m_pRegions[i]->y;
                        }
                    }
                }
            }

            return result;
        }
        */
    }
}
