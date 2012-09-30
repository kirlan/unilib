using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BenTools.Mathematics;
using Random;
using System.IO;
using MIConvexHull;

namespace LandscapeGeneration
{
    public enum GridType
    {
        Square,
        Hex
    }

    public enum WorldShape
    {
        /// <summary>
        /// обычная плоская прямоугольная карта с непереходимым краем
        /// </summary>
        Plain,
        /// <summary>
        /// Мир-кольцо - внутренняя поверхность цилиндра, боковые грани непереходимые
        /// </summary>
        Ringworld,
        /// <summary>
        /// Наружная поверхность цилиндра, боковые грани непереходимые
        /// </summary>
        Cilinder,
        /// <summary>
        /// Планета - наружная поверхность сферы
        /// </summary>
        Planet,
        /// <summary>
        /// Пузырь - внутренная поверхность сферы
        /// </summary>
        Bubble,
        /// <summary>
        /// Панцирь черепахи - круглый плоский мир с горами в центре, со всех сторон окружённый бесконечным океаном
        /// </summary>
        Shell,
        /// <summary>
        /// Чаша - круглый плоский мир с центральным морем, со всех сторон окружённый непроходимыми горами
        /// </summary>
        Chalice
    }

    public class LocationsGrid<LOC>
        where LOC : Location, new()
    {
        /// <summary>
        /// Расстояние от экватора до полюса. 
        /// </summary>
        private const int m_iRY = 50000;

        public int RY
        {
            get { return m_iRY; }
        }

        private int m_iRX = 625000;

        /// <summary>
        /// Половина длины экватора
        /// </summary>
        public int RX
        {
            get { return m_iRX; }
        }

        public string m_sDescription = ""; 

        /// <summary>
        /// Общее количество `локаций` - минимальных "кирпичиков" мира.
        /// Обычно меньше реального числа элементов в m_cLocations, т.к. там присутсвуют ещё "бордюрные"
        /// локации, недоступные для посещения.
        /// </summary>
        public int m_iLocationsCount = 5000;//25000;

        public LOC[] m_aLocations = null;

        public Vertex[] m_aVertexes = null;

        public WorldShape m_eShape = WorldShape.Plain;

        class VertexCH : IVertex
        {
            public double[] Position { get; set; }

            public VertexCH(double x, double y, double z)
            {
                Position = new double[] { x, y, z };
            }
        }

        class FaceCH : ConvexFace<VertexCH, FaceCH>
        {

        }
        
        private bool CreateSphereCH()
        {
            float fTrueR = m_iRY;

            VertexCH[] pSphere = new VertexCH[m_iLocationsCount];

            for (int i = 0; i < m_iLocationsCount; i++)
            {
                float fX3d, fY3d, fZ3d;
                float fD, fD2;

                do
                {
                    fX3d = (float)(fTrueR - Rnd.Get(fTrueR * 2));
                    fY3d = (float)(fTrueR - Rnd.Get(fTrueR * 2));
                    fZ3d = (float)(fTrueR - Rnd.Get(fTrueR * 2));

                    fD2 = (float)Math.Sqrt(fX3d * fX3d + fY3d * fY3d);
                    fD = (float)Math.Sqrt(fD2 * fD2 + fZ3d * fZ3d);
                }
                while (fD > fTrueR || fD < fTrueR * 3 / 4);

                VertexCH pSV = new VertexCH(fX3d * fTrueR / fD, fY3d * fTrueR / fD, fZ3d * fTrueR / fD);

                pSphere[i] = pSV;
            }

            // calculate the triangulation
            var convexHull = ConvexHull.Create<VertexCH, FaceCH>(pSphere);
            var convexHullVertices = convexHull.Points.ToList();
            var faces = convexHull.Faces.ToList();

            List<LOC> cLocations = new List<LOC>();
            Dictionary<VertexCH, LOC> cLocationsCH = new Dictionary<VertexCH, LOC>();
            foreach (var pVertex in convexHullVertices)
            {
                LOC pLocation = new LOC();
                pLocation.Create(cLocations.Count, pVertex.Position[0], pVertex.Position[1], pVertex.Position[2]);
                cLocationsCH[pVertex] = pLocation;
                cLocations.Add(pLocation);
            }
            m_aLocations = cLocations.ToArray();

            Dictionary<LOC, List<LOC[]>> cFacesDic = new Dictionary<LOC, List<LOC[]>>();

            foreach (var pFace in faces)
            {
                LOC pLoc1 = cLocationsCH[pFace.Vertices[0]];
                LOC pLoc2 = cLocationsCH[pFace.Vertices[1]];
                LOC pLoc3 = cLocationsCH[pFace.Vertices[2]];

                if (!cFacesDic.ContainsKey(pLoc1))
                    cFacesDic[pLoc1] = new List<LOC[]>();
                if (!cFacesDic.ContainsKey(pLoc2))
                    cFacesDic[pLoc2] = new List<LOC[]>();
                if (!cFacesDic.ContainsKey(pLoc3))
                    cFacesDic[pLoc3] = new List<LOC[]>();

                cFacesDic[pLoc1].Add(new LOC[] { pLoc2, pLoc3 });
                cFacesDic[pLoc2].Add(new LOC[] { pLoc1, pLoc3 });
                cFacesDic[pLoc3].Add(new LOC[] { pLoc1, pLoc2 });

                if (!pLoc1.BorderWith.ContainsKey(pLoc2))
                    pLoc1.BorderWith[pLoc2] = new List<Line>();
                if (!pLoc1.BorderWith.ContainsKey(pLoc3))
                    pLoc1.BorderWith[pLoc3] = new List<Line>();

                if (!pLoc2.BorderWith.ContainsKey(pLoc1))
                    pLoc2.BorderWith[pLoc1] = new List<Line>();
                if (!pLoc2.BorderWith.ContainsKey(pLoc3))
                    pLoc2.BorderWith[pLoc3] = new List<Line>();

                if (!pLoc3.BorderWith.ContainsKey(pLoc1))
                    pLoc3.BorderWith[pLoc1] = new List<Line>();
                if (!pLoc3.BorderWith.ContainsKey(pLoc2))
                    pLoc3.BorderWith[pLoc2] = new List<Line>();
            }
            foreach (LOC pLoc in m_aLocations)
                pLoc.FillBorderWithKeys();

            List<Vertex> cVertexes = new List<Vertex>();
            RestoreVoronoi(ref cVertexes, ref cFacesDic);

            m_aVertexes = cVertexes.ToArray();

            foreach (LOC pLoc in m_aLocations)
                pLoc.FillBorderWithKeys();

            return true;
        }

        private bool RestoreVoronoi(ref List<Vertex> cVertexes, ref Dictionary<LOC, List<LOC[]>> cFacesDic)
        {
            float fTrueR = m_iRX / (float)Math.PI;

            foreach (LOC pLoc1 in m_aLocations)
            {
                List<LOC[]> cPairs = cFacesDic[pLoc1];

                Dictionary<LOC, List<LOC>> cWheel = new Dictionary<LOC, List<LOC>>();
                LOC pStarter = null;
                foreach (LOC pLoc2 in pLoc1.m_cBorderWith.Keys)
                {
                    List<LOC> cLinks = new List<LOC>();

                    foreach (LOC pLink in pLoc2.m_cBorderWith.Keys)
                        if (pLink.m_cBorderWith.ContainsKey(pLoc1))
                            cLinks.Add(pLink);

                    cWheel[pLoc2] = cLinks;

                    if (pStarter == null)
                        pStarter = pLoc2;
                }

                foreach (var pSpoke in cWheel)
                {
                    if (pSpoke.Value.Count < 2)
                        throw new Exception("Unchained location border!");

                    if (pSpoke.Value.Count > 2)
                    {
                        List<LOC> cFalseOnes = new List<LOC>();
                        foreach (var pLink in pSpoke.Value)
                        {
                            bool bTrueOne = false;
                            foreach (var pPair in cPairs)
                            {
                                if ((pPair[0] == pSpoke.Key && pPair[1] == pLink) ||
                                    (pPair[1] == pSpoke.Key && pPair[0] == pLink))
                                {
                                    bTrueOne = true;
                                    break;
                                }
                            }
                            if (!bTrueOne)
                                cFalseOnes.Add(pLink);
                        }
                        foreach (var pFalseOne in cFalseOnes)
                            pSpoke.Value.Remove(pFalseOne);
                    }
                }

                LOC[] aWheel = new LOC[cWheel.Count];
                int iCounter = 0;
                aWheel[iCounter++] = pStarter;
                aWheel[iCounter++] = cWheel[pStarter][0];
                do
                {
                    if (cWheel[aWheel[iCounter - 1]][0] == aWheel[iCounter - 2])
                        aWheel[iCounter] = cWheel[aWheel[iCounter - 1]][1];
                    else
                        aWheel[iCounter] = cWheel[aWheel[iCounter - 1]][0];

                    iCounter++;
                }
                while (iCounter < aWheel.Length);

                int iSwap = 0;
                for (int i = 0; i < aWheel.Length; i++)
                {
                    LOC pNext;
                    if (i == aWheel.Length - 1)
                        pNext = aWheel[0];
                    else
                        pNext = aWheel[i + 1];

                    LOC pCurrent = aWheel[i];

                    //Для определения того, лежат ли локации pLeft и pRight относительно центра локации pLoc по часовой стрелке
                    //или против, рассчитаем векторное произведение векторов (pLoc1, pLeft) и (pLoc1, pRight).
                    //Если результирующий вектор будет направлен от центра сферы, то все три вектора (направления на вершины из центра локации и 
                    //их векторное произведение) составляют "правую тройку", т.е. точки pLeft и pRight лежат против часовой 
                    //стрелки относительно центра.
                    //Мы же, для определённости будем все вектора границы локации приводить к направлению "по часовой стрелке"

                    float fAx = pCurrent.X - pLoc1.X;
                    float fAy = pCurrent.Y - pLoc1.Y;
                    float fAz = pCurrent.Z - pLoc1.Z;
                    float fBx = pNext.X - pLoc1.X;
                    float fBy = pNext.Y - pLoc1.Y;
                    float fBz = pNext.Z - pLoc1.Z;

                    float fCrossX = fAy * fBz - fAz * fBy;
                    float fCrossY = fAz * fBx - fAx * fBz;
                    float fCrossZ = fAx * fBy - fAy * fBx;

                    //чтобы определить направление результирующего вектора относительно центра сферы, вычислим скалярное произведение.
                    //скалярное произведение > 0, если угол между векторами острый, и < 0, если угол между векторами тупой.
                    if (fCrossX * pLoc1.X + fCrossY * pLoc1.Y + fCrossZ * pLoc1.Z > 0)
                        iSwap++;
                    else
                        iSwap--;
                }
                if (iSwap > 0)
                {
                    LOC[] aWheelNew = new LOC[cWheel.Count];

                    iCounter = 0;
                    for (int i = aWheel.Length - 1; i >= 0; i--)
                        aWheelNew[iCounter++] = aWheel[i];

                    aWheel = aWheelNew;
                }

                for (int i = 0; i < aWheel.Length; i++)
                {
                    LOC pLoc2 = aWheel[i];
                    //Найдём пару других локаций, так же связанных и с pLoc, и с pLink. Все 4 локации вместе образуют 2 треугольника,
                    //центры которых и будут вершинами нашей диаграммы Вороного
                    LOC pLeft, pRight;
                    if (i == 0)
                        pLeft = aWheel[aWheel.Length - 1];
                    else
                        pLeft = aWheel[i - 1];

                    if (i == aWheel.Length - 1)
                        pRight = aWheel[0];
                    else
                        pRight = aWheel[i + 1];

                    Vertex pVertexA = null;
                    Vertex pVertexB = null;

                    //проверим, возможно эти вершины у нас уже вычислены
                    if (pLoc1.BorderWith.ContainsKey(pLeft) && pLoc1.BorderWith[pLeft].Count > 0)
                    {
                        Line pLine = pLoc1.BorderWith[pLeft][0];
                        if (pLine.m_pPoint1.m_cLocationsBuild.Contains(pLoc2))
                            pVertexA = pLine.m_pPoint1;
                        else if (pLine.m_pPoint2.m_cLocationsBuild.Contains(pLoc2))
                            pVertexA = pLine.m_pPoint2;
                    }

                    if (pLoc1.BorderWith.ContainsKey(pRight) && pLoc1.BorderWith[pRight].Count > 0)
                    {
                        Line pLine = pLoc1.BorderWith[pRight][0];
                        if (pLine.m_pPoint1.m_cLocationsBuild.Contains(pLoc2))
                            pVertexB = pLine.m_pPoint1;
                        else if (pLine.m_pPoint2.m_cLocationsBuild.Contains(pLoc2))
                            pVertexB = pLine.m_pPoint2;
                    }

                    if (pVertexA == null)
                    {
                        pVertexA = new Vertex((pLoc1.X + pLoc2.X + pLeft.X) / 3, (pLoc1.Y + pLoc2.Y + pLeft.Y) / 3, (pLoc1.Z + pLoc2.Z + pLeft.Z) / 3);
                        cVertexes.Add(pVertexA);
                    }
                    if (pVertexB == null)
                    {
                        pVertexB = new Vertex((pLoc1.X + pLoc2.X + pRight.X) / 3, (pLoc1.Y + pLoc2.Y + pRight.Y) / 3, (pLoc1.Z + pLoc2.Z + pRight.Z) / 3);
                        cVertexes.Add(pVertexB);
                    }


                    //добавим вертексам ссылки друг на друга
                    if (!pVertexA.m_cVertexes.Contains(pVertexB))
                        pVertexA.m_cVertexes.Add(pVertexB);
                    if (!pVertexB.m_cVertexes.Contains(pVertexA))
                        pVertexB.m_cVertexes.Add(pVertexA);

                    Vertex pMidPoint = new Vertex((pVertexA.X + pVertexB.X) / 2, (pVertexA.Y + pVertexB.Y) / 2, (pVertexA.Z + pVertexB.Z) / 2);
                    Vertex pLoc1Point = new Vertex((pLoc1.X + pMidPoint.X) / 2, (pLoc1.Y + pMidPoint.Y) / 2, (pLoc1.Z + pMidPoint.Z) / 2);
                    Vertex pLoc2Point = new Vertex((pLoc2.X + pMidPoint.X) / 2, (pLoc2.Y + pMidPoint.Y) / 2, (pLoc2.Z + pMidPoint.Z) / 2);

                    cVertexes.Add(pMidPoint);
                    cVertexes.Add(pLoc1Point);
                    cVertexes.Add(pLoc2Point);

                    if (!pVertexA.m_cLocationsBuild.Contains(pLeft))
                        pVertexA.m_cLocationsBuild.Add(pLeft);
                    if (!pVertexB.m_cLocationsBuild.Contains(pRight))
                        pVertexB.m_cLocationsBuild.Add(pRight);

                    if (!pVertexA.m_cLocationsBuild.Contains(pLoc1))
                        pVertexA.m_cLocationsBuild.Add(pLoc1);
                    if (!pVertexB.m_cLocationsBuild.Contains(pLoc1))
                        pVertexB.m_cLocationsBuild.Add(pLoc1);

                    pMidPoint.m_cLocationsBuild.Add(pLoc1);
                    pMidPoint.m_cLocationsBuild.Add(pLoc2);

                    pLoc1Point.m_cLocationsBuild.Add(pLoc1);
                    pLoc2Point.m_cLocationsBuild.Add(pLoc2);

                    Line pLine1 = new Line(pVertexA, pVertexB, pMidPoint, pLoc1Point);
                    if (pLine1.m_fLength > 0 && pLoc1.BorderWith[pLoc2].Count == 0)
                    {
                        //foreach (List<Line> cLines in pLoc1.BorderWith.Values)
                        //    if (cLines[0].m_pPoint1 == pVertexA)
                        //        throw new Exception("Wrong edge!");

                        pLoc1.BorderWith[pLoc2].Add(pLine1);
                    }

                    if (!pVertexA.m_cLocationsBuild.Contains(pLoc2))
                        pVertexA.m_cLocationsBuild.Add(pLoc2);
                    if (!pVertexB.m_cLocationsBuild.Contains(pLoc2))
                        pVertexB.m_cLocationsBuild.Add(pLoc2);

                    Line pLine2 = new Line(pVertexB, pVertexA, pMidPoint, pLoc2Point);
                    if (pLine2.m_fLength > 0 && pLoc2.BorderWith[pLoc1].Count == 0)
                    {
                        //foreach (List<Line> cLines in pLoc2.m_cBorderWith.Values)
                        //    if (cLines[0].m_pPoint1 == pVertexB)
                        //        throw new Exception("Wrong edge!");

                        pLoc2.BorderWith[pLoc1].Add(pLine2);
                    }
                }
                pLoc1.BuildBorder();
                pLoc1.CorrectCenter();
            }

            return true;
        }
        
        /// <summary>
        /// Создание сетки из случайных точек
        /// </summary>
        /// <param name="iLocations">количество точек</param>
        /// <param name="iWidth">пропорции карты - ширина</param>
        /// <param name="iHeight">пропорции карты - высота)</param>
        public LocationsGrid(int iLocations, int iWidth, int iHeight, string sDescription, WorldShape eShape)
        {
            m_iRX = m_iRY*iWidth/iHeight;
            m_iLocationsCount = iLocations;
            m_eShape = eShape;

            bool bOK = true;
            do
            {
                if (eShape == WorldShape.Planet)
                {
                    m_iRX = (int)(m_iRY * Math.PI);

                    bOK = CreateSphereCH();
                }
                else
                {
                    BuildPlainRandomGrid();
                    bOK = CalculateVoronoi();
                    if (bOK)
                    {
                        ImproveGrid();
                        bOK = CalculateVoronoi();
                        if (bOK)
                        {
                            ImproveGrid();
                            bOK = CalculateVoronoi();
                            if (bOK)
                            {
                                ImproveGrid();
                                bOK = CalculateVoronoi();

                                if (bOK)
                                {
                                    //для всех ячеек связываем разрозненные рёбра в замкнутую ломаную границу
                                    foreach (LOC pLoc in m_aLocations)
                                    {
                                        pLoc.BuildBorder();
                                        pLoc.CorrectCenter();
                                    }

                                    if (m_eShape == WorldShape.Ringworld)
                                    {
                                        MergeVertexes();
                                        MakeRingWorld();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            while (!bOK);

            m_sDescription = sDescription;
            m_bLoaded = true;
        }

        /// <summary>
        /// Создание упорядоченной сетки
        /// </summary>
        /// <param name="iWidth">ширина</param>
        /// <param name="iHeight">высота</param>
        /// <param name="eGridType">тип сетки</param>
        public LocationsGrid(int iWidth, int iHeight, GridType eGridType, string sDescription, WorldShape eShape)
        {
            m_eShape = eShape;

            if (eGridType == GridType.Hex)
            {
                float fProportion = 2.0f * iWidth * (float)Math.Sqrt(0.75) / (iHeight+1);
                m_iRX = (int)(m_iRY * fProportion);

                m_iLocationsCount = iWidth * iHeight / 2;

                bool bOK = true;
                do
                {
                    BuildHexGrid(iWidth, iHeight);
                    bOK = CalculateVoronoi();
                }
                while (!bOK);

                //для всех ячеек связываем разрозненные рёбра в замкнутую ломаную границу
                foreach (LOC pLoc in m_aLocations)
                {
                    pLoc.BuildBorder();
                    pLoc.CorrectCenter();
                }

                m_sDescription = sDescription;
                m_bLoaded = true;
            }
            else
                throw new ArgumentException(string.Format("Grid type '{0}' is not implemented yet!", eGridType));
        }

        /// <summary>
        /// Строит сетку/диаграмму Вороного НА ПЛОСКОСТИ. Переход в 3D будет осуществлён позже.
        /// </summary>
        /// <returns></returns>
        private bool CalculateVoronoi()
        {
            Dictionary<BTVector, LOC> cData = new Dictionary<BTVector, LOC>();
            foreach (LOC pLoc in m_aLocations)
                cData[new BTVector(pLoc.X, pLoc.Y)] = pLoc;

            //Строим диаграмму вороного - определяем границы локаций
            VoronoiGraph graph = Fortune.ComputeVoronoiGraph(cData.Keys);
            Dictionary<BTVector, Vertex> cVertexes = new Dictionary<BTVector, Vertex>();
            List<Vertex> cMidPoints = new List<Vertex>();

            //Переводим данные из диаграммы Вороного в наш формат
            try
            {
                foreach (VoronoiEdge pEdge in graph.Edges)
                    AddEdge(cData, cVertexes, cMidPoints, pEdge);
            }
            catch (Exception ex)
            {
                //бывает, алгоритм выдаёт данные, которые мы не можем корректно перевести (нулевые рёбра, etc.)
                //в этом случае всё приходится начинать заново
                return false;
            }

            cMidPoints.AddRange(cVertexes.Values);
            m_aVertexes = cMidPoints.ToArray();

            foreach (LOC pLoc in m_aLocations)
                pLoc.FillBorderWithKeys();

            return true;
        }

        /// <summary>
        /// Сливает 2 вертекса в один
        /// </summary>
        /// <param name="pFrom">этот вертекс будет удалён</param>
        /// <param name="pTo">этот останется</param>
        private void MergeVertex(Vertex pFrom, Vertex pTo)
        {
            foreach (Location pLoc in pFrom.m_cLocationsBuild)
            {
                foreach (var vLine in pLoc.m_cBorderWith)
                    foreach (Line pLine in vLine.Value)
                        pLine.Merge(pFrom, pTo);

                if (!pTo.m_cLocationsBuild.Contains(pLoc))
                    pTo.m_cLocationsBuild.Add(pLoc);
            }

            foreach (Vertex pLink in pFrom.m_cVertexes)
            {
                if (pLink == pFrom)
                    continue;

                if (!pTo.m_cVertexes.Contains(pLink) && pTo != pLink)
                    pTo.m_cVertexes.Add(pLink);

                pLink.m_cVertexes.Remove(pFrom);
                if(pLink != pTo)
                    pLink.m_cVertexes.Add(pTo);
            }
        }

        /// <summary>
        /// Сливает дублирующиеся вертексы на плоской развёртке закольцованной (цилиндрической или сферической) карты
        /// </summary>
        private void MergeVertexes()
        {
            List<Vertex> cOutspace = new List<Vertex>();
            foreach (Vertex pVertex in m_aVertexes)
            {
                if (pVertex.m_fX < -RX)
                {
                    foreach (Vertex pDouble in m_aVertexes)
                    {
                        if (pDouble.m_fY == pVertex.m_fY && pDouble.m_fX == pVertex.m_fX + RX * 2)
                        {
                            MergeVertex(pVertex, pDouble);
                            cOutspace.Add(pVertex);
                            //foreach (Vertex pTest in m_aVertexes)
                            //{
                            //    if (cOutspace.Contains(pTest))
                            //        continue;

                            //    if (pTest.m_cVertexes.Contains(pVertex))
                            //    {
                            //        MergeVertex(pVertex, pDouble);
                            //        cOutspace.Add(pVertex);
                            //        break;
                            //    }
                            //}
                            break;
                        }
                    }
                }
                if (pVertex.m_fX > RX)
                {
                    foreach (Vertex pDouble in m_aVertexes)
                    {
                        if (pDouble.m_fY == pVertex.m_fY && pDouble.m_fX == pVertex.m_fX - RX * 2)
                        {
                            MergeVertex(pVertex, pDouble);
                            cOutspace.Add(pVertex);
                            //foreach (Vertex pTest in m_aVertexes)
                            //{
                            //    if (cOutspace.Contains(pTest))
                            //        continue;

                            //    if (pTest.m_cVertexes.Contains(pVertex))
                            //    {
                            //        MergeVertex(pVertex, pDouble);
                            //        cOutspace.Add(pVertex);
                            //        break;
                            //    }
                            //} 
                            break;
                        }
                    }
                }
            }

            List<Vertex> cVertexes = new List<Vertex>();
            cVertexes.AddRange(m_aVertexes);
            foreach (Vertex pOut in cOutspace)
            {
                pOut.m_fX = -1;
                pOut.m_fY = -1;
                cVertexes.Remove(pOut);
            }

            m_aVertexes = cVertexes.ToArray();

        }

        /// <summary>
        /// Преобразует плоскую развёртку в трёхмерный Мир-Кольцо
        /// </summary>
        private void MakeRingWorld()
        {
            float fTrueR = m_iRX / (float)Math.PI;

            foreach (LOC pLoc in m_aLocations)
            {
                float fRo = (float)pLoc.Y / m_iRY;
                float fPhy = (float)Math.PI * pLoc.X / m_iRX;

                pLoc.m_iGridX = (int)pLoc.X;

                pLoc.X = (float)((float)fTrueR * Math.Cos(fPhy));
                pLoc.Z = (float)((float)fTrueR * Math.Sin(fPhy));
                pLoc.Y = (float)((float)m_iRY * fRo);
            }

            foreach (Vertex pVertex in m_aVertexes)
            {
                float fRo = (float)pVertex.Y / m_iRY;
                float fPhy = (float)Math.PI * pVertex.X / m_iRX;

                pVertex.m_fX = (float)((float)fTrueR * Math.Cos(fPhy));
                pVertex.m_fZ = (float)((float)fTrueR * Math.Sin(fPhy));
                pVertex.m_fY = (float)((float)m_iRY * fRo);
            }
        }

        private bool AddEdge(Dictionary<BTVector, LOC> cData, Dictionary<BTVector, Vertex> cVertexes, List<Vertex> cMidPoints, VoronoiEdge pEdge)
        {
            LOC pLoc1 = null;
            LOC pLoc2 = null;

            if (pEdge.VVertexA == pEdge.VVertexB)
                return false;

            if (pEdge.VVertexA.data[0] == pEdge.VVertexB.data[0] && pEdge.VVertexA.data[1] == pEdge.VVertexB.data[1])
                return false;

            //получаем ссылки на реальные локации, с которыми связаны BT-вектора
            if (cData.ContainsKey(pEdge.LeftData))
                pLoc1 = cData[pEdge.LeftData];
            if (cData.ContainsKey(pEdge.RightData))
                pLoc2 = cData[pEdge.RightData];

            //если обе локации "призрачные" - нас это не интересует
            if (pLoc1.m_pOrigin != null && pLoc2.m_pOrigin != null)
                return false;

            //если одна из вершин грани оказывается в бесконечности - игнорируем такую грань
            if (pEdge.VVertexA == Fortune.VVUnkown || pEdge.VVertexB == Fortune.VVUnkown)
            {
                pLoc1.m_bUnclosed = true;
                pLoc2.m_bUnclosed = true;
                return false;
            }
            //если одна из вершин грани оказывается хоть и не в бесконечности, но всё-равно достаточно далеко за краем карты - тоже игнорируем
            if (pEdge.VVertexA.data[0] < -RX*2 ||
                pEdge.VVertexA.data[0] > RX*2 ||
                pEdge.VVertexA.data[1] < -RY*2 ||
                pEdge.VVertexA.data[1] > RY*2 ||
                pEdge.VVertexB.data[0] < -RX*2 ||
                pEdge.VVertexB.data[0] > RX*2 ||
                pEdge.VVertexB.data[1] < -RY*2 ||
                pEdge.VVertexB.data[1] > RY*2)
            {
                pLoc1.m_bUnclosed = true;
                pLoc2.m_bUnclosed = true;
                return false;
            }

            //при необходимости, создадим новые вертексы для вершин добавляемой грани
            if (!cVertexes.ContainsKey(pEdge.VVertexA))
                cVertexes[pEdge.VVertexA] = new Vertex(pEdge.VVertexA);

            Vertex pVertexA = cVertexes[pEdge.VVertexA];

            if (!cVertexes.ContainsKey(pEdge.VVertexB))
                cVertexes[pEdge.VVertexB] = new Vertex(pEdge.VVertexB);

            Vertex pVertexB = cVertexes[pEdge.VVertexB];

            if (pVertexA == pVertexB)
                throw new Exception("Vertexes too close!");

            //Для определения того, лежат ли точки A и B относительно центра локации L1 по часовой стрелке
            //или против, рассчитаем z-координату векторного произведения векторов (L1, A) и (L1, B).
            //Если она будет положительна, то все три вектора (направления на вершины из центра локации и 
            //их векторное произведение) составляют "правую тройку", т.е. точки A и B лежат против часовой 
            //стрелки относительно центра.
            //Мы же, для определённости будем все вектора границы локации приводить к направлению "по часовой стрелке".

            float fAx = pVertexA.X - pLoc1.X;
            float fAy = pVertexA.Y - pLoc1.Y;
            float fBx = pVertexB.X - pLoc1.X;
            float fBy = pVertexB.Y - pLoc1.Y;

            bool bSwap = false;
            if (fAx * fBy > fAy * fBx)
                bSwap = true;

            if (bSwap)
            {
                Vertex pVertexC = pVertexA;
                pVertexA = pVertexB;
                pVertexB = pVertexC;
            }

            //добавим вертексам ссылки друг на друга
            if (!pVertexA.m_cVertexes.Contains(pVertexB))
                pVertexA.m_cVertexes.Add(pVertexB);
            if (!pVertexB.m_cVertexes.Contains(pVertexA))
                pVertexB.m_cVertexes.Add(pVertexA);

            Vertex pMidPoint = new Vertex((pVertexA.X + pVertexB.X)/2, (pVertexA.Y + pVertexB.Y)/2, (pVertexA.Z + pVertexB.Z)/2);
            //pMidPoint.m_cVertexes.Add(pVertexA);
            //pMidPoint.m_cVertexes.Add(pVertexB);
            //pVertexA.m_cVertexes.Add(pMidPoint);
            //pVertexB.m_cVertexes.Add(pMidPoint);

            Vertex pLoc1Point = (pLoc1 != null && pLoc1.m_pOrigin == null) ? 
                new Vertex((pLoc1.X + pMidPoint.X) / 2, (pLoc1.Y + pMidPoint.Y) / 2, (pLoc1.Z + pMidPoint.Z) / 2) : 
                new Vertex(pMidPoint.X, pMidPoint.Y, pMidPoint.Z);

            Vertex pLoc2Point = (pLoc2 != null && pLoc2.m_pOrigin == null) ? 
                new Vertex((pLoc2.X + pMidPoint.X) / 2, (pLoc2.Y + pMidPoint.Y) / 2, (pLoc2.Z + pMidPoint.Z) / 2) : 
                new Vertex(pMidPoint.X, pMidPoint.Y, pMidPoint.Z);

            cMidPoints.Add(pMidPoint);
            cMidPoints.Add(pLoc1Point);
            cMidPoints.Add(pLoc2Point);

            if (pLoc1 != null && pLoc1.m_pOrigin == null)
            {
                if (!pVertexA.m_cLocationsBuild.Contains(pLoc1))
                    pVertexA.m_cLocationsBuild.Add(pLoc1);
                if (!pVertexB.m_cLocationsBuild.Contains(pLoc1))
                    pVertexB.m_cLocationsBuild.Add(pLoc1);
                
                if (!pMidPoint.m_cLocationsBuild.Contains(pLoc1))
                    pMidPoint.m_cLocationsBuild.Add(pLoc1);
                
                if (!pLoc1Point.m_cLocationsBuild.Contains(pLoc1))
                    pLoc1Point.m_cLocationsBuild.Add(pLoc1);

                if (pLoc2 != null)
                {
                    Line pLine = new Line(pVertexA, pVertexB, pMidPoint, pLoc1Point);
                    if (pLine.m_fLength > 0)
                    {
                        foreach (List<Line> cLines in pLoc1.BorderWith.Values)
                            if (cLines[0].m_pPoint1 == pVertexA)
                                throw new Exception("Wrong edge!");
                        //if(!bTwin)
                        if (pLoc2.m_pOrigin == null)
                        {
                            pLoc1.BorderWith[pLoc2] = new List<Line>();
                            pLoc1.BorderWith[pLoc2].Add(pLine);
                        }
                        else
                        {
                            pLoc1.BorderWith[(LOC)pLoc2.m_pOrigin] = new List<Line>();
                            pLoc1.BorderWith[(LOC)pLoc2.m_pOrigin].Add(pLine);
                        }
                    }
                }
            }

            if (pLoc2 != null && pLoc2.m_pOrigin == null)
            {
                if (!pVertexA.m_cLocationsBuild.Contains(pLoc2))
                    pVertexA.m_cLocationsBuild.Add(pLoc2);
                if (!pVertexB.m_cLocationsBuild.Contains(pLoc2))
                    pVertexB.m_cLocationsBuild.Add(pLoc2);
                if (!pMidPoint.m_cLocationsBuild.Contains(pLoc2))
                    pMidPoint.m_cLocationsBuild.Add(pLoc2);
                if (!pLoc2Point.m_cLocationsBuild.Contains(pLoc2))
                    pLoc2Point.m_cLocationsBuild.Add(pLoc2);

                if (pLoc1 != null)
                {
                    Line pLine = new Line(pVertexB, pVertexA, pMidPoint, pLoc2Point);
                    if (pLine.m_fLength > 0)
                    {
                        foreach (List<Line> cLines in pLoc2.m_cBorderWith.Values)
                            if (cLines[0].m_pPoint1 == pVertexB)
                                throw new Exception("Wrong edge!");
                        if (pLoc1.m_pOrigin == null)
                        {
                            pLoc2.BorderWith[pLoc1] = new List<Line>();
                            pLoc2.BorderWith[pLoc1].Add(pLine);
                        }
                        else
                        {
                            pLoc2.BorderWith[(LOC)pLoc1.m_pOrigin] = new List<Line>();
                            pLoc2.BorderWith[(LOC)pLoc1.m_pOrigin].Add(pLine);
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// не умеет строить сферу, всё-равно надо полностью перписывать, чтобы зря не гонять вороного
        /// </summary>
        /// <param name="iWidth"></param>
        /// <param name="iHeight"></param>
        private void BuildHexGrid(int iWidth, int iHeight)
        {
            List<LOC> cLocations = new List<LOC>();

            //float stepX = 2.0f * RX / (iWidth);
            //float stepY = stepX / (float)Math.Sqrt(0.75);
            
            float stepY = 4.0f * RY / (iHeight+1);
            float stepX = stepY * (float)Math.Sqrt(0.75);

            //List<long> cUsedIDs = new List<long>();

            int iWidthReserv = m_eShape == WorldShape.Ringworld ? 0:2;
            int iHeightReserv = 2;

            for (int yy = -iHeightReserv; yy < iHeight / 2 + iHeightReserv; yy++)
                for (int xx = -iWidthReserv; xx < iWidth + iWidthReserv; xx++)
                {
                    LOC pLocation = new LOC();

                    float fx = xx * stepX;

                    float fy = yy * stepY;
                    if ((xx + 2) % 2 == 1)
                        fy += stepY / 2;

                    bool bBorder = yy < 0 || yy >= iHeight/2 || xx < 0 || xx >= iWidth;

                    long iID = xx + yy * iWidth;
                    if (bBorder)
                        iID = xx + iWidthReserv + (yy + iHeightReserv) * (iWidth + 2 * iWidthReserv) + iWidth * iHeight * 2;

                    //if(cUsedIDs.Contains(iID))
                    //    iID += iWidth * iHeight * 2;

                    pLocation.Create(iID, -RX + stepX/2 + fx, -RY + stepY/2 + fy, 0, xx, yy);
                    pLocation.m_bBorder = bBorder;

                    cLocations.Add(pLocation);

                    if (m_eShape == WorldShape.Ringworld)
                    {
                        LOC pGhostLocation = new LOC();

                        if (stepX/2 + fx > RX)
                            pGhostLocation.Create(iID + iWidth * iHeight * 4, -RX * 3 + stepX/2 + fx, -RY + stepY / 2 + fy, 0, pLocation);
                        else
                            pGhostLocation.Create(iID + iWidth * iHeight * 4, RX + stepX/2 + fx, -RY + stepY / 2 + fy, 0, pLocation);
                        
                        pGhostLocation.m_bBorder = true;

                        cLocations.Add(pGhostLocation);
                    }

                    //cUsedIDs.Add(iID);

                }

            m_aLocations = cLocations.ToArray();
        }

        private void ImproveGrid()
        {
            List<LOC> cLocations = BuildRandomGridFrame();

            float kx = (int)(Math.Sqrt((float)RX * m_iLocationsCount / RY));
            float ky = m_iLocationsCount / kx;

            float dkx = RX / (kx * 2);
            float dky = RY / (ky * 2);

            foreach (LOC pLoc in m_aLocations)
            {
                if (!pLoc.m_bBorder && !pLoc.m_bGhost && !pLoc.m_bFixed)
                {
                    float fX = 0;
                    float fY = 0;
                    int iCount = 0;

                    foreach (var cLines in pLoc.m_cBorderWith)
                    {
                        foreach (Line pLine in cLines.Value)
                        {
                            fX += pLine.m_pPoint2.X;
                            fY += pLine.m_pPoint2.Y;
                            iCount++;
                        }
                    }

                    if (iCount != 0)
                    {
                        LOC pImprovedLoc = new LOC();
                        fX /= iCount;
                        fY /= iCount;
                        pImprovedLoc.Create(cLocations.Count, fX, fY, 0);
                        pImprovedLoc.m_bBorder = false;
                        cLocations.Add(pImprovedLoc);

                        if (m_eShape == WorldShape.Ringworld || m_eShape == WorldShape.Planet)
                        {
                            LOC pGhostLocation = new LOC();

                            if (pImprovedLoc.X > 0)
                                pGhostLocation.Create(cLocations.Count, pImprovedLoc.X - RX * 2, pImprovedLoc.Y, 0, pImprovedLoc);
                            else
                                pGhostLocation.Create(cLocations.Count, pImprovedLoc.X + RX * 2, pImprovedLoc.Y, 0, pImprovedLoc);

                            pGhostLocation.m_bBorder = true;
                            pGhostLocation.m_bGhost = true;

                            cLocations.Add(pGhostLocation);
                        }
                    }
                }
            }

            m_aLocations = cLocations.ToArray();
        }

        private List<LOC> BuildRandomGridFrame()
        {
            List<LOC> cLocations = new List<LOC>();

            if (m_eShape == WorldShape.Planet)
                return cLocations;

            float kx = (int)(Math.Sqrt((float)RX * m_iLocationsCount / RY));
            float ky = m_iLocationsCount / kx;

            float dkx = RX / (kx * 2);
            float dky = RY / (ky * 2);

            //Создаём периметр карты из "запретных" локаций, для того чтобы получить ровную кромку.
            for (int ii = 0; ii <= kx; ii++)
            {
                int xx = (int)(ii * 2 * RX / kx);

                LOC pLocation11 = new LOC();
                pLocation11.Create(cLocations.Count, RX - xx, RY - dky, 0);
                pLocation11.m_bBorder = xx == 0 || xx == 2 * RX;//false;
                pLocation11.m_bFixed = true;
                cLocations.Add(pLocation11);

                LOC pLocation12 = new LOC();
                pLocation12.Create(cLocations.Count, RX - xx, RY + dky, 0);
                pLocation12.m_bBorder = true;
                pLocation12.m_bFixed = true;
                cLocations.Add(pLocation12);

                LOC pLocation21 = new LOC();
                pLocation21.Create(cLocations.Count, RX - xx, -RY - dky, 0);
                pLocation21.m_bBorder = true;
                pLocation21.m_bFixed = true;
                cLocations.Add(pLocation21);

                LOC pLocation22 = new LOC();
                pLocation22.Create(cLocations.Count, RX - xx, -RY + dky, 0);
                pLocation22.m_bBorder = xx == 0 || xx == 2 * RX;//false;
                pLocation22.m_bFixed = true;
                cLocations.Add(pLocation22);

                if (m_eShape == WorldShape.Ringworld)
                {
                    LOC pLocation11Ghost = new LOC();
                    if (pLocation11.X > 0)
                        pLocation11Ghost.Create(cLocations.Count, pLocation11.X - RX * 2, pLocation11.Y, 0, pLocation11);
                    else
                        pLocation11Ghost.Create(cLocations.Count, pLocation11.X + RX * 2, pLocation11.Y, 0, pLocation11);
                    pLocation11Ghost.m_bBorder = true;
                    pLocation11Ghost.m_bFixed = true;
                    cLocations.Add(pLocation11Ghost);

                    LOC pLocation12Ghost = new LOC();
                    if (pLocation12.X > 0)
                        pLocation12Ghost.Create(cLocations.Count, pLocation12.X - RX * 2, pLocation12.Y, 0, pLocation12);
                    else
                        pLocation12Ghost.Create(cLocations.Count, pLocation12.X + RX * 2, pLocation12.Y, 0, pLocation12);
                    pLocation12Ghost.m_bBorder = true;
                    pLocation12Ghost.m_bFixed = true;
                    cLocations.Add(pLocation12Ghost);

                    LOC pLocation21Ghost = new LOC();
                    if (pLocation21.X > 0)
                        pLocation21Ghost.Create(cLocations.Count, pLocation21.X - RX * 2, pLocation21.Y, 0, pLocation21);
                    else
                        pLocation21Ghost.Create(cLocations.Count, pLocation21.X + RX * 2, pLocation21.Y, 0, pLocation21);
                    pLocation21Ghost.m_bBorder = true;
                    pLocation21Ghost.m_bFixed = true;
                    cLocations.Add(pLocation21Ghost);

                    LOC pLocation22Ghost = new LOC();
                    if (pLocation22.X > 0)
                        pLocation22Ghost.Create(cLocations.Count, pLocation22.X - RX * 2, pLocation22.Y, 0, pLocation22);
                    else
                        pLocation22Ghost.Create(cLocations.Count, pLocation22.X + RX * 2, pLocation22.Y, 0, pLocation22);
                    pLocation22Ghost.m_bBorder = true;
                    pLocation22Ghost.m_bFixed = true;
                    cLocations.Add(pLocation22Ghost);
                }
            }

            if (m_eShape != WorldShape.Ringworld)
            {
                for (int jj = 0; jj <= ky; jj++)
                {
                    int yy = (int)(jj * 2 * RY / ky);

                    LOC pLocation11 = new LOC();
                    pLocation11.Create(cLocations.Count, RX - dkx, RY - yy, 0);
                    pLocation11.m_bBorder = yy == 0 || yy == 2 * RY;//false;
                    pLocation11.m_bFixed = true;
                    cLocations.Add(pLocation11);

                    LOC pLocation12 = new LOC();
                    pLocation12.Create(cLocations.Count, RX + dkx, RY - yy, 0);
                    pLocation12.m_bBorder = true;
                    pLocation12.m_bFixed = true;
                    cLocations.Add(pLocation12);

                    LOC pLocation21 = new LOC();
                    pLocation21.Create(cLocations.Count, -RX - dkx, RY - yy, 0);
                    pLocation21.m_bBorder = true;
                    pLocation21.m_bFixed = true;
                    cLocations.Add(pLocation21);

                    LOC pLocation22 = new LOC();
                    pLocation22.Create(cLocations.Count, -RX + dkx, RY - yy, 0);
                    pLocation22.m_bBorder = yy == 0 || yy == 2 * RY;//false;
                    pLocation22.m_bFixed = true;
                    cLocations.Add(pLocation22);
                }
            }

            return cLocations;
        }

        /// <summary>
        /// используется только для построения плоских и цилиндрических сеток!
        /// </summary>
        private void BuildPlainRandomGrid()
        {
            if (m_eShape == WorldShape.Planet)
                throw new Exception("Use BuildSphererandomGrid() !!!");

            List<LOC> cLocations = BuildRandomGridFrame();

            float kx = (int)(Math.Sqrt((float)RX * m_iLocationsCount / RY));
            float ky = m_iLocationsCount / kx;

            float dkx = RX / (kx * 2);
            float dky = RY / (ky * 2);

            //Добавляем центры остальных локаций в случайные позиции внутри периметра.
            for (int i = 0; i < m_iLocationsCount; i++)
            {
                LOC pLocation = new LOC();
                pLocation.Create(cLocations.Count, RX - dkx * 2 - Rnd.Get(RX * 2 - 4 * dkx), RY - dky * 2 - Rnd.Get(RY * 2 - 4 * dky), 0);
                cLocations.Add(pLocation);

                if (m_eShape == WorldShape.Ringworld)
                {
                    LOC pGhostLocation = new LOC();

                    if (pLocation.X > 0)
                        pGhostLocation.Create(cLocations.Count, pLocation.X - RX * 2, pLocation.Y, 0, pLocation);
                    else
                        pGhostLocation.Create(cLocations.Count, pLocation.X + RX * 2, pLocation.Y, 0, pLocation);

                    pGhostLocation.m_bBorder = true;
                    pGhostLocation.m_bGhost = true;

                    cLocations.Add(pGhostLocation);
                }
            }

            m_aLocations = cLocations.ToArray();
        }

        private static int s_iVersion = 21;
        private static string s_sHeader = "DimensionX World Generator 3D Grid File.";

        public void Save(string sFilename)
        {
            using (BinaryWriter binWriter =
                new BinaryWriter(File.Open(sFilename, FileMode.Create)))
            {
                m_sFilename = sFilename;
                binWriter.Write(s_sHeader);
                binWriter.Write(s_iVersion);
                binWriter.Write(m_sDescription);
                binWriter.Write(m_iLocationsCount);
                binWriter.Write(m_iRX);
                binWriter.Write((int)m_eShape);

                binWriter.Write(m_aVertexes.Length);
                foreach (Vertex pVertex in m_aVertexes)
                {
                    pVertex.Save(binWriter);
                }
                
                binWriter.Write(m_aLocations.Length);
                foreach (LOC pLoc in m_aLocations)
                {
                    pLoc.Save(binWriter);
                }
            }

            m_sFilename = sFilename;
        }

        public static bool CheckFile(string sFilename, out string sDescription, out int iLocationsCount, out WorldShape eShape)
        {
            sDescription = "";
            eShape = WorldShape.Plain;
            iLocationsCount = 0;

            if (!File.Exists(sFilename))
                return false;

            BinaryReader binReader =
                new BinaryReader(File.Open(sFilename, FileMode.Open));
            
            try
            {
                // If the file is not empty,
                // read the application settings.
                // First read 4 bytes into a buffer to
                // determine if the file is empty.
                byte[] testArray = new byte[3];
                int count = binReader.Read(testArray, 0, 3);

                if (count != 0)
                {
                    // Reset the position in the stream to zero.
                    binReader.BaseStream.Seek(0, SeekOrigin.Begin);

                    string sHeader = binReader.ReadString();
                    if (sHeader != s_sHeader)
                        return false;

                    int iVersion = binReader.ReadInt32();
                    if (iVersion != s_iVersion)
                        return false;

                    sDescription = binReader.ReadString();
                    iLocationsCount = binReader.ReadInt32();
                    int iRX = binReader.ReadInt32();
                    eShape = (WorldShape)Enum.GetValues(typeof(WorldShape)).GetValue(binReader.ReadInt32());
                }
            }
            catch (EndOfStreamException e)
            {
                return false;
            }
            finally
            {
                binReader.Close();
            }

            return true;
        }

        public bool m_bLoaded = false;

        public string m_sFilename;

        public LocationsGrid(string sFilename)
        {
            if (!CheckFile(sFilename, out m_sDescription, out m_iLocationsCount, out m_eShape))
                throw new ArgumentException("File not exists or have a wrong format!");

            m_sFilename = sFilename;
        }

        public void Unload()
        {
            Reset();

            m_aLocations = null;
            m_aVertexes = null;

            m_bLoaded = false;
        }

        public delegate void BeginStepDelegate(string sDescription, int iLength);
        public delegate void ProgressStepDelegate();
        
        public void Load(BeginStepDelegate BeginStep, ProgressStepDelegate ProgressStep)
        {
            if (m_bLoaded)
                return;

            BinaryReader binReader =
                new BinaryReader(File.Open(m_sFilename, FileMode.Open));

            try
            {
                // If the file is not empty,
                // read the application settings.
                // First read 4 bytes into a buffer to
                // determine if the file is empty.
                byte[] testArray = new byte[3];
                int count = binReader.Read(testArray, 0, 3);

                if (count != 0)
                {
                    if(BeginStep != null)
                        BeginStep("Loading grid...", 1);

                    // Reset the position in the stream to zero.
                    binReader.BaseStream.Seek(0, SeekOrigin.Begin);

                    string sHeader = binReader.ReadString();
                    if (sHeader != s_sHeader)
                        throw new Exception("Header mismatch!");

                    int iVersion = binReader.ReadInt32();
                    if (iVersion != s_iVersion)
                        throw new Exception("Version mismatch!");

                    m_sDescription = binReader.ReadString();
                    m_iLocationsCount = binReader.ReadInt32();
                    m_iRX = binReader.ReadInt32();
                    m_eShape = (WorldShape)Enum.GetValues(typeof(WorldShape)).GetValue(binReader.ReadInt32());

                    if(ProgressStep != null)
                        ProgressStep();

                    Dictionary<long, Vertex> cTempDicVertex = new Dictionary<long, Vertex>();
                    int iVertexesCount = binReader.ReadInt32();
                    if (BeginStep != null)
                        BeginStep("Loading vertexes...", iVertexesCount * 2);
                    for (int i = 0; i < iVertexesCount; i++)
                    {
                        Vertex pVertexLoc = new Vertex(binReader);

                        cTempDicVertex[pVertexLoc.m_iID] = pVertexLoc;

                        if (ProgressStep != null)
                            ProgressStep();
                    }

                    m_aVertexes = new List<Vertex>(cTempDicVertex.Values).ToArray();

                    //Восстанавливаем словарь соседей
                    foreach (Vertex pVertex in m_aVertexes)
                    {
                        foreach (long iID in pVertex.m_cLinksTmp)
                        {
                            pVertex.m_cVertexes.Add(cTempDicVertex[iID]);
                        }
                        pVertex.m_cLinksTmp.Clear();

                        if (ProgressStep != null)
                            ProgressStep();
                    }

                    Dictionary<long, LOC> cTempDic = new Dictionary<long, LOC>();
                    int iLocationsCount = binReader.ReadInt32();
                    if (BeginStep != null)
                        BeginStep("Loading locations...", iLocationsCount * 2);
                    for (int i = 0; i < iLocationsCount; i++)
                    {
                        LOC pLoc = new LOC();
                        pLoc.Load(binReader, cTempDicVertex);

                        cTempDic[pLoc.m_iID] = pLoc;

                        if (ProgressStep != null)
                            ProgressStep();
                    }

                    m_aLocations = new List<LOC>(cTempDic.Values).ToArray();

                    //Восстанавливаем словарь соседей
                    foreach (LOC pLoc in m_aLocations)
                    {
                        foreach (var ID in pLoc.m_cBorderWithID)
                        {
                            pLoc.m_cBorderWith[cTempDic[ID.Key]] = ID.Value;
                        }
                        pLoc.m_cBorderWithID.Clear();
                        pLoc.FillBorderWithKeys();

                        if (ProgressStep != null)
                            ProgressStep();
                    }

                    //Восстанавливаем словарь соседей для вертексов
                    foreach (Vertex pVertex in m_aVertexes)
                    {
                        pVertex.m_aLocations = new Location[pVertex.m_cLocationsTmp.Count];
                        int iIndex = 0;
                        foreach (long iID in pVertex.m_cLocationsTmp)
                        {
                            pVertex.m_aLocations[iIndex++] = cTempDic[iID];
                        }
                        pVertex.m_cLocationsTmp.Clear();
                    }

                    cTempDicVertex.Clear();
                    cTempDic.Clear();

                    if (BeginStep != null)
                        BeginStep("Recalculating grid edges...", m_aLocations.Length);
                    //для всех ячеек связываем разрозненные рёбра в замкнутую ломаную границу
                    foreach (LOC pLoc in m_aLocations)
                    {
                        if (pLoc.Forbidden)
                            continue;

                        pLoc.BuildBorder();
                        if (ProgressStep != null)
                            ProgressStep();
                    }

                    m_bLoaded = true;
                }
            }
            catch (EndOfStreamException e)
            {
                throw new Exception("Wrong file format!", e);
            }
            finally
            {
                binReader.Close();
            }
        }

        public void Reset()
        {
            if (!m_bLoaded)
                return;

            foreach (LOC pLoc in m_aLocations)
                pLoc.Reset();
        }

        public override string ToString()
        {
            FileInfo pInfo = new FileInfo(m_sFilename);
            return m_sDescription + " (" + pInfo.Name + ")";
        }
    }
}
