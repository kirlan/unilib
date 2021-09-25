using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;
using MIConvexHull;
using System.Windows;
using SimpleWorld.Geography;

namespace SimpleWorld
{
    /// <summary>
    /// Просто совокупность локаций, соединённых между собой по диаграмме Вороного 
    /// </summary>
    public class CSimpleWorld<LOC, TERR>
        where LOC: CSimpleLocation<TERR>
        where TERR: class, ITerritory
    {
        public LOC[] Locations { get; private set; } = new LOC[] { };

        private readonly List<CLink> m_cLinks = new List<CLink>();

        internal CLink[] Links
        {
            get { return m_cLinks.ToArray(); }
        }

        public int WorldScale { get; private set; } = 150;

        public const int m_iMinDist = 25;
        public const int m_iMaxDist = 35;

        //private List<int> m_cRandomTests = new List<int>();

        public CSimpleWorld(int iScale)
        {
            WorldScale = iScale;

            //m_cRandomTests.Add(Rnd.Get(1000));

            GenerateMap();

            //m_cRandomTests.Add(Rnd.Get(1000));

            MarkForbiddenSubTypes();
            MarkSubTypes();

            //m_cRandomTests.Add(Rnd.Get(1000));

            RandomizeLinks();

            //m_cRandomTests.Add(Rnd.Get(1000));
        }

        private void RandomizeLinks()
        {
            for (int i = 0; i < Locations.Length*5; i++)
            {
                var pLoc = Locations[Rnd.Get(Locations.Length)];
                foreach (var pLink in pLoc.Links)
                {
                    bool bCanDelete = false;
                    foreach (var pLink2 in pLoc.Links)
                    {
                        if (pLink2.Key.Links.ContainsKey(pLink.Key) && pLink.Value.Distance > pLink2.Value.Distance * 1.5)
                            bCanDelete = true;
                    }

                    if (bCanDelete)
                    {
                        m_cLinks.Remove(pLink.Value);
                        pLoc.Links.Remove(pLink.Key);
                        pLink.Key.Links.Remove(pLoc);
                        break;
                    }
                }
            }
        }

        protected virtual void MarkSubTypes()
        {
        }

        protected virtual void MarkForbiddenSubTypes()
        {
        }

        protected virtual void SetupPredefinedLocations(ref List<LOC> cLocations)
        {
            int iX, iY;

            // Creating 3 rings of outer forbidden locations
            // inner ring
            double fAngle = (double)(m_iMinDist + Rnd.Get(m_iMaxDist - m_iMinDist)) / WorldScale;
            do
            {
                int iR = WorldScale - (m_iMaxDist - m_iMinDist) / 2 + Rnd.Get(m_iMaxDist - m_iMinDist);
                iX = (int)(iR * Math.Cos(fAngle));
                iY = (int)(iR * Math.Sin(fAngle));
                AddLocation(ref cLocations, iX, iY, true);

                int iDelta = m_iMinDist + Rnd.Get(m_iMaxDist - m_iMinDist);
                fAngle += (double)iDelta / WorldScale;
            }
            while (fAngle < 2 * Math.PI);

            // middle ring
            fAngle -= 2 * Math.PI;
            do
            {
                int iR = WorldScale + m_iMinDist + Rnd.Get(m_iMaxDist - m_iMinDist);
                iX = (int)(iR * Math.Cos(fAngle));
                iY = (int)(iR * Math.Sin(fAngle));
                AddLocation(ref cLocations, iX, iY, true);

                int iDelta = m_iMinDist + Rnd.Get(m_iMaxDist - m_iMinDist);
                fAngle += (double)iDelta / WorldScale;
            }
            while (fAngle < 2 * Math.PI);

            // outer ring
            fAngle -= 2 * Math.PI;
            do
            {
                iX = (int)(WorldScale * 2 * Math.Cos(fAngle));
                iY = (int)(WorldScale * 2 * Math.Sin(fAngle));
                AddLocation(ref cLocations, iX, iY, true);

                int iDelta = m_iMinDist + Rnd.Get(m_iMaxDist - m_iMinDist);
                fAngle += (double)iDelta * 0.5 / WorldScale;
            }
            while (fAngle < 2 * Math.PI);
        }

        #region GenerateMap() and helper functions
        private void GenerateMap()
        {
            int iX, iY;

            List<LOC> cLocations = new List<LOC>();

            SetupPredefinedLocations(ref cLocations);

            // add common locations
            int iLocationsCount = cLocations.Count + (int)(Math.Pow(WorldScale, 2) * Math.PI / 1500);
            int iTriesCount = 0;
            while (cLocations.Count < iLocationsCount && iTriesCount++ < iLocationsCount * 1000)
            {
                iX = WorldScale - Rnd.Get(WorldScale * 2);
                iY = WorldScale - Rnd.Get(WorldScale * 2);

                if (PossibleLocation(ref cLocations, iX, iY, false))
                    AddLocation(ref cLocations, iX, iY, false);
            }

            // fill remained gaps with impassable locations
            iTriesCount = 0;
            while (iTriesCount++ < iLocationsCount*1000)
            {
                iX = WorldScale - Rnd.Get(WorldScale * 2);
                iY = WorldScale - Rnd.Get(WorldScale * 2);

                if(PossibleLocation(ref cLocations, iX, iY, true))
                    AddLocation(ref cLocations, iX, iY, true);
            }

            //List<CLocation> cEmpty = new List<CLocation>();
            //for (int i = 0; i < m_cLocations.Count; i++)
            //{
            //    if (m_cLocations[i].Links.Count == 0)
            //        cEmpty.Add(m_cLocations[i]);
            //}

            //foreach (CLocation pLoc in cEmpty)
            //    m_cLocations.Remove(pLoc);

            Locations = cLocations.ToArray();

            //Наконец, строим диаграмму Вороного.
            VoronoiMesh<CSimpleLocation<TERR>, CSimpleLocation<TERR>.VoronoiCell, VoronoiEdge<CSimpleLocation<TERR>, CSimpleLocation<TERR>.VoronoiCell>> voronoiMesh = VoronoiMesh.Create<CSimpleLocation<TERR>, CSimpleLocation<TERR>.VoronoiCell>(Locations);

            //Переведём результат в удобный нам формат.
            //Для каждого найденного ребра диаграммы Вороного найдём локации, которые оно разделяет
            RebuildEdges(voronoiMesh.Edges);
        }

        /// <summary>
        /// Восстанавливаем информацию о смежных гранях по выходным данным MIConvexHull.
        /// </summary>
        /// <param name="cEdges"></param>
        /// <returns></returns>
        private void RebuildEdges(IEnumerable<VoronoiEdge<CSimpleLocation<TERR>, CSimpleLocation<TERR>.VoronoiCell>> cEdges)
        {
            foreach (var edge in cEdges)
            {
                var from = edge.Source;
                var to = edge.Target;

                List<CSimpleLocation<TERR>> cNeighbours = new List<CSimpleLocation<TERR>>();

                //для этого просканируем все локации, имеющие связь с начальной точкой ребра
                foreach (var pFromLoc in from.Vertices)
                {
                    //и с конечной точкой ребра
                    foreach (var pToLoc in to.Vertices)
                    {
                        //нас интересуют те 2 локации, которые будут в обеих списках
                        if (pFromLoc == pToLoc)
                        {
                            cNeighbours.Add(pFromLoc);
                            if (cNeighbours.Count > 2)
                                throw new Exception("Нашли больше 2 локаций с общей границей!");
                        }
                    }
                }

                if (cNeighbours.Count < 2)
                    throw new Exception("У границы меньше 2 сопредельных локаций!");

                //для порядка, определим порядок обхода вершин из локации - по часовой стрелке
                if (IsLeft(cNeighbours[0].ToPoint(), from.Circumcenter, to.Circumcenter) < 0)
                {
                    cNeighbours.Reverse();
                }

                CSimpleLocation<TERR>.VoronoiCell pMiddle = new CSimpleLocation<TERR>.VoronoiCell(from, to);
                //CLocation.Cell pInnerLeft = new CLocation.Cell(pMiddle, pLeft);
                //CLocation.Cell pInnerRight = new CLocation.Cell(pMiddle, pRight);

                //пропишем ссылки на ребро в найденных локациях
                cNeighbours[0].m_cEdges[cNeighbours[1]] = new CSimpleLocation<TERR>.VoronoiEdge(from, to, pMiddle);//, pInnerLeft);
                cNeighbours[1].m_cEdges[cNeighbours[0]] = new CSimpleLocation<TERR>.VoronoiEdge(to, from, pMiddle);//, pInnerRight);
            }
        }

        static int IsLeft(Point a, Point b, Point c)
        {
            decimal ax = (decimal)a.X;
            decimal bx = (decimal)b.X;
            decimal cx = (decimal)c.X;
            decimal ay = (decimal)a.Y;
            decimal by = (decimal)b.Y;
            decimal cy = (decimal)c.Y;
            return ((bx - ax) * (cy - ay) - (by - ay) * (cx - ax)) > 0 ? 1 : -1;
        }

        protected LOC AddLocation(ref List<LOC> cLocations, int iX, int iY, bool bForbidden)
        {
            LOC pNewLocation = Activator.CreateInstance(typeof(LOC), iX, iY) as LOC;
            if (bForbidden)
                pNewLocation.Type = LocationType.Forbidden;
            cLocations.Add(pNewLocation);

            if (!bForbidden)
            {
                int k;
                int try_count = -1;
                do
                {
                    k = MakeLink(ref cLocations, pNewLocation);
                    try_count++;
                    //if (k == -1)
                    //    k = pNewLocation.MaxLinks + 1;
                }
                while (try_count < 2 * pNewLocation.MaxLinks);
                //while (Rnd.Get(pNewLocation.MaxLinks * pNewLocation.MaxLinks) <= k * k && try_count < 2 * pNewLocation.MaxLinks) ;
            }

            return pNewLocation;
        }

        private int MakeLink(ref List<LOC> cLocations, LOC pNewLocation)
        {
            float fDist;
            int k;
            for (int i = 0; i < cLocations.Count; i++)
            {
                if (cLocations[i].Type == LocationType.Forbidden)
                    continue;

                fDist = (cLocations[i].X - pNewLocation.X) * (cLocations[i].X - pNewLocation.X) +
                    (cLocations[i].Y - pNewLocation.Y) * (cLocations[i].Y - pNewLocation.Y);
                if (cLocations[i] != pNewLocation && fDist < 2 * m_iMaxDist * m_iMaxDist)
                {
                    k = CreateLink(ref cLocations, cLocations[i], pNewLocation);
                    if (k != -1)
                        return k;
                }
            }
            return -1;
        }

        private int CreateLink(ref List<LOC> cLocations, LOC pLocation1, LOC pLocation2)
        {
	        if(pLocation1.Links.ContainsKey(pLocation2))
		        return -1;

            if (pLocation1.Links.Count >= pLocation1.MaxLinks || pLocation2.Links.Count >= pLocation2.MaxLinks)
                return -1;

            double k1, b1;
            //						коеффициент наклона потенциальной связи
            if (pLocation1.X - pLocation2.X != 0)
                k1 = (double)(pLocation1.Y - pLocation2.Y) / (pLocation1.X - pLocation2.X);
            else
                k1 = (double)(pLocation1.Y - pLocation2.Y) / (pLocation1.X - pLocation2.X + 0.00000001);

            b1 = (double)pLocation2.Y - pLocation2.X * k1;
            
            
            //	Проверка перекрёстков
	        for(int i=0; i<cLocations.Count; i++)
	        {
                LOC pLocation3 = cLocations[i];
                if (pLocation3 != pLocation1 &&
                    pLocation3 != pLocation2)
                {
                    foreach (var pLink4 in pLocation3.Links)
                    {
                        if (pLink4.Key != pLocation1 &&
                            pLink4.Key != pLocation2)
                        {
                            //	Нашли пару соединённых пещер, не входящих в рассматриваемую пару.
                            double k2, b2;
                            //						коеффициент наклона существующей связи
                            if (pLocation3.X - pLink4.Key.X != 0)
                                k2 = (double)(pLocation3.Y - pLink4.Key.Y) / (pLocation3.X - pLink4.Key.X);
                            else
                                k2 = (double)(pLocation3.Y - pLink4.Key.Y) / (pLocation3.X - pLink4.Key.X + 0.00000001);

                            b2 = (double)pLink4.Key.Y - pLink4.Key.X * k2;

                            if (k1 != k2)
                            {
                                double x1;
                                x1 = (b2 - b1) / (k1 - k2);

                                //						ЕСЛИ точка пересечения где-то здесь
                                if (((x1 > pLocation1.X && x1 < pLocation2.X) ||
                                     (x1 < pLocation1.X && x1 > pLocation2.X)) &&
                                    ((x1 > pLocation3.X && x1 < pLink4.Key.X) ||
                                     (x1 < pLocation3.X && x1 > pLink4.Key.X)))
                                {
                                    return -1;
                                }
                            }
                        }
                    }
                }
	        }

            CLink pNewLink = new CLink(pLocation1, pLocation2);

            pLocation1.Links[pLocation2] = pNewLink;
            pLocation2.Links[pLocation1] = pNewLink;
            m_cLinks.Add(pNewLink);

	        return pLocation1.Links.Count;
        }

        protected bool PossibleLocation(ref List<LOC> cLocations, int iX, int iY, bool bForbidden)
        {
            float fDist = iX * iX + iY * iY;
            if (fDist > WorldScale * WorldScale)
                return false;

            bool result = false;

            int iPossibleAnchors = 0;
            for (int i = 0; i < cLocations.Count; i++)
            {
                fDist = (cLocations[i].X - iX) * (cLocations[i].X - iX) + (cLocations[i].Y - iY) * (cLocations[i].Y - iY);
                if (fDist < m_iMinDist * m_iMinDist)
                    return false;

                if (cLocations[i].Type == LocationType.Forbidden)
                    continue;

                if (cLocations[i].Type != LocationType.Forbidden)
                    iPossibleAnchors++;

                if (fDist < m_iMaxDist * m_iMaxDist && cLocations[i].Links .Count < cLocations[i].MaxLinks)
                    result = true;
            }

            if (bForbidden || iPossibleAnchors < 10)
                return true;

            return result;
        }
        #endregion
    }
}
