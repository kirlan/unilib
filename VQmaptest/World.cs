using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;
using NameGen;
using System.Drawing;

namespace VQmaptest
{
    public class World
    {
        private static string[] m_aPlace = 
        {
            "Realm",
            "Plane",
            "Dimension",
            "World",
        };

        public List<Land> m_cLands = new List<Land>();

        private const int m_iWorldScale = 150;

        public int WorldScale
        {
            get { return m_iWorldScale; }
        }

        private const int m_iMinDist = 25;
        private const int m_iMaxDist = 35;

        public class LandPtr
        { 
            public Land m_pLand = null;
        }

        public Dictionary<int, Dictionary<int, LandPtr>> m_cMap = new Dictionary<int, Dictionary<int, LandPtr>>();

        private void GenerateMap()
        {
            int iX, iY;

            double fAngle = 0;
            do
            {
                iX = (int)(m_iWorldScale * Math.Cos(fAngle));
                iY = (int)(m_iWorldScale * Math.Sin(fAngle));
                Land pNewLand = AddLand(iX, iY);
                pNewLand.Type = LandType.Forbidden;
                pNewLand.Assign(null, false);

                int iDelta = m_iMinDist + Rnd.Get(m_iMaxDist - m_iMinDist);
                fAngle += (double)iDelta / m_iWorldScale;
            }
            while (fAngle < 2 * Math.PI);

            int iLocationsCount = (int)(Math.Pow(m_iWorldScale, 2) * Math.PI / 1000);
            while (m_cLands.Count < iLocationsCount)
            {
                do
                {
                    iX = m_iWorldScale - Rnd.Get(m_iWorldScale * 2);
                    iY = m_iWorldScale - Rnd.Get(m_iWorldScale * 2);
                }
                while (!PossibleLocation(iX, iY));

                AddLand(iX, iY);
            }

            List<Land> cEmpty = new List<Land>();
            for (int i = 0; i < m_cLands.Count; i++)
            {
                if (m_cLands[i].Links.Count == 0)
                    cEmpty.Add(m_cLands[i]);
            }

            foreach (Land pLand in cEmpty)
                m_cLands.Remove(pLand);
        }

        private Land AddLand(int iX, int iY)
        {
            Land pNewLand = new Land(this, iX, iY);
            m_cLands.Add(pNewLand);

            int k;
            int try_count = -1;
            do
            {
                k = MakeLink(pNewLand);
                try_count++;
                //if (k == -1)
                //    k = pNewLocation.MaxLinks + 1;
            }
            while (try_count < 2 * pNewLand.MaxLinks);
            //while (Rnd.Get(pNewLocation.MaxLinks * pNewLocation.MaxLinks) <= k * k && try_count < 2 * pNewLocation.MaxLinks) ;

            return pNewLand;
        }

        private int MakeLink(Land pNewLand)
        {
            float fDist;
            int k;
            for (int i = 0; i < m_cLands.Count; i++)
            {
                fDist = (m_cLands[i].X - pNewLand.X) * (m_cLands[i].X - pNewLand.X) +
                    (m_cLands[i].Y - pNewLand.Y) * (m_cLands[i].Y - pNewLand.Y);
                if (m_cLands[i] != pNewLand && fDist < 2 * m_iMaxDist * m_iMaxDist)
                {
                    k = CreateLink(m_cLands[i], pNewLand);
                    if (k != -1)
                        return k;
                }
            }
            return -1;
        }

        private int CreateLink(Land pLand1, Land pLand2)
        {
            if (pLand1.Links.Contains(pLand2))
                return -1;

            if (pLand1.Links.Count >= pLand1.MaxLinks || pLand2.Links.Count >= pLand2.MaxLinks)
                return -1;

            double k1, b1;
            //						коеффициент наклона потенциальной связи
            if (pLand1.X - pLand2.X != 0)
                k1 = (double)(pLand1.Y - pLand2.Y) / (pLand1.X - pLand2.X);
            else
                k1 = (double)(pLand1.Y - pLand2.Y) / (pLand1.X - pLand2.X + 0.00000001);

            b1 = (double)pLand2.Y - pLand2.X * k1;


            //	Проверка перекрёстков
            for (int i = 0; i < m_cLands.Count; i++)
            {
                Land pLand3 = m_cLands[i];
                if (pLand3 != pLand1 &&
                    pLand3 != pLand2)
                {
                    for (int j = 0; j < pLand3.Links.Count; j++)
                    {
                        Land pLand4 = pLand3.Links[j];
                        if (pLand4 != pLand1 &&
                            pLand4 != pLand2)
                        {
                            //	Нашли пару соединённых пещер, не входящих в рассматриваемую пару.
                            double k2, b2;
                            //						коеффициент наклона существующей связи
                            if (pLand3.X - pLand4.X != 0)
                                k2 = (double)(pLand3.Y - pLand4.Y) / (pLand3.X - pLand4.X);
                            else
                                k2 = (double)(pLand3.Y - pLand4.Y) / (pLand3.X - pLand4.X + 0.00000001);

                            b2 = (double)pLand4.Y - pLand4.X * k2;

                            if (k1 != k2)
                            {
                                double x1;
                                x1 = (b2 - b1) / (k1 - k2);

                                //						ЕСЛИ точка пересечения где-то здесь
                                if (((x1 > pLand1.X && x1 < pLand2.X) ||
                                     (x1 < pLand1.X && x1 > pLand2.X)) &&
                                    ((x1 > pLand3.X && x1 < pLand4.X) ||
                                     (x1 < pLand3.X && x1 > pLand4.X)))
                                {
                                    return -1;
                                }
                            }
                        }
                    }
                }
            }

            pLand1.Links.Add(pLand2);
            pLand2.Links.Add(pLand1);

            return pLand1.Links.Count;
        }

        private bool PossibleLocation(int iX, int iY)
        {
            float fDist = iX * iX + iY * iY;
            if (fDist > m_iWorldScale * m_iWorldScale)
                return false;

            bool result = false;

            for (int i = 0; i < m_cLands.Count; i++)
            {
                fDist = (m_cLands[i].X - iX) * (m_cLands[i].X - iX) + (m_cLands[i].Y - iY) * (m_cLands[i].Y - iY);
                if (fDist < m_iMinDist * m_iMinDist)
                    return false;
                if (fDist < m_iMaxDist * m_iMaxDist && m_cLands[i].Links.Count < m_cLands[i].MaxLinks)
                    result = true;
            }

            if (m_cLands.Count < 1)
                return true;

            return result;
        }

        private void PopulateMap()
        {
            for (int i = 0; i < m_cStates.Count; i++)
            {
                int iLoc = -1;
                do
                {
                    iLoc = Rnd.Get(m_cLands.Count);
                }
                while (m_cLands[iLoc].m_pState != null || m_cLands[iLoc].Type == LandType.Forbidden);

                m_cLands[iLoc].Assign(m_cStates[i], true);
            }

            bool bExpanding = false;
            do
            {
                bExpanding = false;
                for (int i = 0; i < m_cLands.Count; i++)
                {
                    if (m_cLands[i].m_pState != null)
                    {
                        List<Land> cPossibleDirections = new List<Land>();
                        for (int j = 0; j < m_cLands[i].Links.Count; j++)
                        {
                            if (m_cLands[i].Links[j].m_pState == null && m_cLands[i].Links[j].Type != LandType.Forbidden)
                                cPossibleDirections.Add(m_cLands[i].Links[j]);
                        }
                        if (cPossibleDirections.Count > 0)
                        {
                            int iLoc = Rnd.Get(cPossibleDirections.Count);
                            cPossibleDirections[iLoc].Assign(m_cLands[i].m_pState, false);
                            bExpanding = true;
                        }
                    }
                }
            }
            while (bExpanding);
        }

        /// <summary>
        /// Определяем границы провинций
        /// </summary>
        /// <param name="lake"></param>
        private void BuildProvinceMap(int lake)
        {
            double lake_modifer;

	        if(lake>15)
		        lake_modifer = lake-14;
	        else
		        lake_modifer = 1.0/(16-lake);

            //последовательно перебираем все точки карты
	        foreach(int xx in m_cMap.Keys)
	        {
		        foreach(int yy in m_cMap[xx].Keys)
		        {
                    //квадраты расстояний от данной точки до центров всех провинций
                    Dictionary<Land, long> distance = new Dictionary<Land, long>();
                    Dictionary<Land, double> distance_ = new Dictionary<Land, double>();

			        double mim_dist;
			        mim_dist = m_iWorldScale*m_iWorldScale*2;
			        Dictionary<Land,int>	conn_count = new Dictionary<Land,int>();
			        
                    //считаем квадраты расстояний от данной точки до центров всех провинций
                    //запоминаем только те земли, которые попадают в радиус m_iMaxDist
                    foreach(Land pLand in m_cLands)
			        {
                        long iDistSquare = (xx-pLand.X)*(xx-pLand.X) + (yy-pLand.Y)*(yy-pLand.Y);
                        if (iDistSquare < m_iMaxDist * m_iMaxDist)
                            distance[pLand] = iDistSquare;
			        }
                    
                    //для каждой провинции в радиусе m_iMaxDist считаем количество смежных, так же входящих в этот радиус
                    foreach (Land pLand1 in distance.Keys)
			        {
				        conn_count[pLand1]=1;
        		        foreach(Land pLand2 in pLand1.Links)
					        if(distance.ContainsKey(pLand2))
						        conn_count[pLand1]++;
			        }
                    

                    //перебираем все провинции и смотрим, может ли эта точка принадлежать им
			        foreach(Land pLand1 in distance.Keys)
			        {
                        if (distance[pLand1] < m_iMaxDist * m_iMaxDist * 0.81)
                        {
                            double mif_dist; //коеффициент претензий i-того города на данную точку
                            mif_dist = 2 * m_iMaxDist;
                            //в первом приближении считаем по реальной дистанции.
                            distance_[pLand1] = Math.Sqrt(distance[pLand1]);

                            

                            //перебираем все провинции, центры которых лежат в радиусе m_iMaxDist от нашей точки
                            foreach (Land pLand2 in distance.Keys)
                            {
                                if (pLand2 != pLand1)
                                {
                                    //квадрат расстояния между центрами провинций
                                    double tdist = Math.Sqrt((pLand2.X - pLand1.X) * (pLand2.X - pLand1.X) + (pLand2.Y - pLand1.Y) * (pLand2.Y - pLand1.Y));
                                    if (tdist < m_iMaxDist * 1.8) //рассматриваем только те j-тые города, которые реально могут быть соединены с i-тым городом
                                    {
                                        double mx, my;
                                        mx = (double)(conn_count[pLand2] * pLand1.X + conn_count[pLand1] * pLand2.X) / (conn_count[pLand1] + conn_count[pLand2]);
                                        my = (double)(conn_count[pLand2] * pLand1.Y + conn_count[pLand1] * pLand2.Y) / (conn_count[pLand1] + conn_count[pLand2]);
                                        double ddist;
                                        ddist = (double)(xx - mx) * (xx - mx) + (yy - my) * (yy - my);
                                        double mdist;
                                        mdist = (double)(pLand1.X - mx) * (pLand1.X - mx) + (pLand1.Y - my) * (pLand1.Y - my);
                                        double miff_dist; //вклад j-того города в коеффициент претензий i-того города на данную точку
                                        if (pLand2.Links.Contains(pLand1))
                                        {
                                            miff_dist = 1.0 - Math.Sqrt(mdist) / (Math.Sqrt(distance[pLand1]) + Math.Sqrt(ddist));
                                            miff_dist = miff_dist * miff_dist;
                                        }
                                        else
                                        {
                                            miff_dist = Math.Sqrt(mdist) / (Math.Sqrt(distance[pLand1]) + Math.Sqrt(ddist));
                                            double dist_mod;
                                            dist_mod = m_iMaxDist * 1.8 / tdist;
                                            miff_dist = (1.0 - (1.0 - miff_dist) * (1.0 - miff_dist)) * 2.0 * dist_mod * dist_mod;
                                        }
                                        mif_dist = mif_dist * 1.9 * miff_dist;
                                    }
                                }
                            }
                            if (distance[pLand1] > (m_iMinDist / 4) * (m_iMinDist / 4))
                                distance_[pLand1] = (Math.Sqrt(distance[pLand1]) - m_iMinDist / 3.5) * (Math.Sqrt(distance[pLand1]) - m_iMinDist / 3.5) * mif_dist * 0.0005 * 4 / conn_count[pLand1];
                            else
                                distance_[pLand1] = 14.0 * (distance[pLand1] - (m_iMinDist / 4) * (m_iMinDist / 4) - 50.0);
                             
                        }
                        else
                            distance_[pLand1] = mim_dist;
			        }
                    
                    //ищем в рабочем радиусе тройки соединённых между собой провинций
                    foreach (Land pLand1 in distance.Keys)
                    {
                            foreach (Land pLand2 in pLand1.Links)
                            {
                                if (distance.ContainsKey(pLand2))
						        {
                                    foreach (Land pLand3 in pLand1.Links)
                                    {
                                        if (pLand2.Links.Contains(pLand3) && distance.ContainsKey(pLand3))
								        {
									        double a,b,c,a1,b1,c1,s1,s2,s3,s4;
                                            a = (double)Math.Sqrt((pLand1.X - pLand2.X) * (pLand1.X - pLand2.X) + (pLand1.Y - pLand2.Y) * (pLand1.Y - pLand2.Y));//расстояние между 1 и 2 провинциями
                                            b = (double)Math.Sqrt((pLand1.X - pLand3.X) * (pLand1.X - pLand3.X) + (pLand1.Y - pLand3.Y) * (pLand1.Y - pLand3.Y));//расстояние между 1 и 3 провинциями
                                            c = (double)Math.Sqrt((pLand3.X - pLand2.X) * (pLand3.X - pLand2.X) + (pLand3.Y - pLand2.Y) * (pLand3.Y - pLand2.Y));//расстояние между 2 и 3 провинциями
                                            c1 = (double)Math.Sqrt(distance[pLand1]);
                                            b1 = (double)Math.Sqrt(distance[pLand2]);
                                            a1 = (double)Math.Sqrt(distance[pLand3]);
                                            s1 = (double)Math.Sqrt(Math.Abs((a + b + c) * (a + b - c) * (b + c - a) * (a + c - b)));
                                            s2 = (double)Math.Sqrt(Math.Abs((a1 + b1 + c) * (a1 + b1 - c) * (b1 + c - a1) * (a1 + c - b1)));
                                            s3 = (double)Math.Sqrt(Math.Abs((a + b1 + c1) * (a + b1 - c1) * (b1 + c1 - a) * (a + c1 - b1)));
                                            s4 = (double)Math.Sqrt(Math.Abs((a1 + b + c1) * (a1 + b - c1) * (b + c1 - a1) * (a1 + c1 - b)));
									        if(s2+s3+s4 <= s1*1.15)
									        {
										        distance_[pLand1] = distance_[pLand1]/lake_modifer;
										        distance_[pLand2] = distance_[pLand2]/lake_modifer;
										        distance_[pLand3] = distance_[pLand3]/lake_modifer;
									        }
								        }
							        }
						        }
					        }
			        }
                     

                    //mim_dist = int.MaxValue;
                    foreach (Land pLand in distance_.Keys)
                    {
				        if(distance_[pLand] < mim_dist)
				        {
					        mim_dist = distance_[pLand];
					        m_cMap[xx][yy].m_pLand = pLand;
				        }
			        }
                    //if(mim_dist > m_iMinDist/2)
                    //{
                    //    m_cMap[xx][yy] = null;
                    //}

                    //if (m_cMap[xx][yy] != null)
                    //{
                    //    //if (xx < m_cMap[xx][yy].minx)
                    //    //    m_cMap[xx][yy].minx = xx;
                    //    //if (yy < m_cMap[xx][yy].miny)
                    //    //    m_cMap[xx][yy].miny = yy;
                    //    //if (xx > m_cMap[xx][yy].maxx - 1)
                    //    //    m_cMap[xx][yy].maxx = xx + 1;
                    //    //if (yy > m_cMap[xx][yy].maxy - 1)
                    //    //    m_cMap[xx][yy].maxy = yy + 1;

                    //    //m_cMap[xx][yy].size++;
                    //    overallSize++;
                    //}
		        }
	        }
        }

        public List<State> m_cStates = new List<State>();

        public World()
        {
            for (int i = -m_iWorldScale; i < m_iWorldScale; i++)
            {
                m_cMap[i] = new Dictionary<int, LandPtr>();
                for (int j = -m_iWorldScale; j < m_iWorldScale; j++)
                    m_cMap[i][j] = new LandPtr();
            }

            GenerateMap();

            int iDiversity = 4 + Rnd.Get(4);
            for (int i = 0; i < iDiversity; i++)
            {
                State pNewState = new State(this);
                m_cStates.Add(pNewState);
            }

            PopulateMap();
            BuildProvinceMap(20);
        }

        public Land[] ShortestWay(Land pFrom, Land pTo)
        {
            Dictionary<Land, int> pMatrix = new Dictionary<Land, int>();

            Queue<Land> Q = new Queue<Land>();
            Q.Enqueue(pFrom);

            pMatrix[pFrom] = 0;

            bool bFinished = false;
            while (Q.Count > 0 && !bFinished)
            {
                Land pLand = Q.Dequeue();
                foreach (Land pLinked in pLand.Links)
                {
                    if (!pMatrix.ContainsKey(pLinked))
                    {
                        pMatrix[pLinked] = pMatrix[pLand] + 1;
                        Q.Enqueue(pLinked);
                    }

                    if (pLinked == pTo)
                        bFinished = true;
                }
            }

            List<Land> cResult = new List<Land>();

            if (!bFinished)
                return cResult.ToArray();

            cResult.Add(pTo);
            Land pPoint = cResult[0];

            while (pPoint != pFrom)
            {
                Land pBestPretender = null;
                int iBestDistance = int.MaxValue;
                foreach (Land pLinked in pPoint.Links)
                {
                    if (pMatrix.ContainsKey(pLinked) && pMatrix[pLinked] < iBestDistance)
                    {
                        iBestDistance = pMatrix[pLinked];
                        pBestPretender = pLinked;
                    }
                }
                if (pBestPretender == null)
                    throw new Exception("Can't find path between lands!");

                pPoint = pBestPretender;
                cResult.Insert(0, pPoint);
            }

            return cResult.ToArray();
        }
    }
}
