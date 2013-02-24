using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MIConvexHull;
using System.Windows;
using SimpleVectors;

namespace TestCubePlanet
{
    public class Cube
    {
        public enum WorkingArea
        { 
            OneFace,
            HalfSphereEquatorial,
            HalfSpherePolar,
            WholeSphere
        }

        public enum Face3D
        {
            Top,
            Bottom,
            Left,
            Right,
            Forward,
            Backward
        };

        public Dictionary<Face3D, CubeFace> m_cFaces = new Dictionary<Face3D, CubeFace>();

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

        System.Random r = new System.Random();

        /// <summary>
        /// Строим периметр для диаграммы Вороного, который бы позволял мостить сферу как нам угодно
        /// </summary>
        /// <param name="iInnerCount"></param>
        /// <param name="k"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private List<VertexCH> BuildBorder(out int iInnerCount, double k, int size)
        {
            List<VertexCH> locations = new List<VertexCH>();

            int quanticSize = (int)(size / k);

            float fDeltaX = 0.1f;//0.25f;
            float fDeltaY = 0.25f;//0.25f;
            
            iInnerCount = 0;

            for (int i = 0; i <= quanticSize / 2; i++)
            {
                var x1 = k * 0.5 + i * k + k * (fDeltaX - fDeltaX * 2 * r.NextDouble());
                var y1 = -k * 0.5 + k * (fDeltaY - fDeltaY * 2 * r.NextDouble());

                var x2 = k * 0.5 + i * k + k * (fDeltaX - fDeltaX * 2 * r.NextDouble());
                var y2 = k * 0.5 + k * (fDeltaY - fDeltaY * 2 * r.NextDouble());

                //Внутренние точки квадрата. Сначала - по часовой стрелке.
                //Важно: в XNA используется правая координатная система, а значит ось Y должна быть направлена ВВЕРХ

                var v2tl = new VertexCH(x2, size - y2, VertexCH.Direction.CenterNone, VertexCH.EdgeSide.TopLeft, true);
                var v2rt = new VertexCH(size - y2, size - x2, VertexCH.Direction.CenterNone, VertexCH.EdgeSide.RightTop, true);
                var v2br = new VertexCH(size - x2, y2, VertexCH.Direction.CenterNone, VertexCH.EdgeSide.BottomRight, true);
                var v2lb = new VertexCH(y2, x2, VertexCH.Direction.CenterNone, VertexCH.EdgeSide.LeftBottom, true);

                //Потом - против часовой.

                var v1tr = new VertexCH(size - x1, size + y1, VertexCH.Direction.CenterNone, VertexCH.EdgeSide.TopRight, true);
                var v1rb = new VertexCH(size + y1, x1, VertexCH.Direction.CenterNone, VertexCH.EdgeSide.RightBottom, true);
                var v1bl = new VertexCH(x1, -y1, VertexCH.Direction.CenterNone, VertexCH.EdgeSide.BottomLeft, true);
                var v1lt = new VertexCH(-y1, size - x1, VertexCH.Direction.CenterNone, VertexCH.EdgeSide.LeftTop, true);

                //Те, которые по часовой - добавляем по любому.
                locations.Add(v2tl);
                locations.Add(v2br);
                locations.Add(v2lb);
                locations.Add(v2rt);

                iInnerCount += 4;

                //Те, которые против часовой - на первом шаге пропускаем, т.к. они будут путаться в углах с теми, которые по часовой.
                if (i != 0)
                {
                    locations.Add(v1tr);
                    locations.Add(v1bl);
                    locations.Add(v1lt);
                    locations.Add(v1rb);

                    iInnerCount += 4;
                }

                //Дальше считаем наружные точки. Наружные точки добаволяем всегда, но они могут быть отражением различных внутренних точек

                //Если это первый шаг, то наружная точка по часовой должна быть тенью внутренней по часовой, т.к. на первом шаге у нас нет внутренних против часовой
                var v_1tl = new VertexCH(v2lb.Position[0], v2lb.Position[1] + size, VertexCH.Direction.Up, VertexCH.EdgeSide.TopLeft, true);
                if (i != 0) //иначе - внутренней против часовой
                    v_1tl = new VertexCH(v1bl.Position[0], v1bl.Position[1] + size, VertexCH.Direction.Up, VertexCH.EdgeSide.TopLeft, true);
                locations.Add(v_1tl);

                var v_1rt = new VertexCH(v2tl.Position[0] + size, v2tl.Position[1], VertexCH.Direction.Right, VertexCH.EdgeSide.RightTop, true);
                if (i != 0)
                    v_1rt = new VertexCH(v1lt.Position[0] + size, v1lt.Position[1], VertexCH.Direction.Right, VertexCH.EdgeSide.RightTop, true);
                locations.Add(v_1rt);

                var v_1br = new VertexCH(v2rt.Position[0], v2rt.Position[1] - size, VertexCH.Direction.Down, VertexCH.EdgeSide.BottomRight, true);
                if (i != 0)
                    v_1br = new VertexCH(v1tr.Position[0], v1tr.Position[1] - size, VertexCH.Direction.Down, VertexCH.EdgeSide.BottomRight, true);
                locations.Add(v_1br);

                var v_1lb = new VertexCH(v2br.Position[0] - size, v2br.Position[1], VertexCH.Direction.Left, VertexCH.EdgeSide.LeftBottom, true);
                if (i != 0)
                    v_1lb = new VertexCH(v1rb.Position[0] - size, v1rb.Position[1], VertexCH.Direction.Left, VertexCH.EdgeSide.LeftBottom, true);
                locations.Add(v_1lb);

                //Для наружных точек против часовой всё проще, т.к. они являются тенями внутренних по часовой, которые у нас есть всегда
                var v_2tr = new VertexCH(v2br.Position[0], v2br.Position[1] + size, VertexCH.Direction.Up, VertexCH.EdgeSide.TopRight, true);
                locations.Add(v_2tr);

                var v_2rb = new VertexCH(v2lb.Position[0] + size, v2lb.Position[1], VertexCH.Direction.Right, VertexCH.EdgeSide.RightBottom, true);
                locations.Add(v_2rb);

                var v_2bl = new VertexCH(v2tl.Position[0], v2tl.Position[1] - size, VertexCH.Direction.Down, VertexCH.EdgeSide.BottomLeft, true);
                locations.Add(v_2bl);

                var v_2lt = new VertexCH(v2rt.Position[0] - size, v2rt.Position[1], VertexCH.Direction.Left, VertexCH.EdgeSide.LeftTop, true);
                locations.Add(v_2lt);

                //теперь - угловые внешние точки
                var v111 = new VertexCH(v1rb.Position[0] - size, v1rb.Position[1] + size, VertexCH.Direction.UpLeft, VertexCH.EdgeSide.CornerTopLeft, true);
                var v112 = new VertexCH(v1bl.Position[0] + size, v1bl.Position[1] + size, VertexCH.Direction.UpRight, VertexCH.EdgeSide.CornerTopRight, true);
                var v121 = new VertexCH(v1lt.Position[0] + size, v1lt.Position[1] - size, VertexCH.Direction.DownRight, VertexCH.EdgeSide.CornerBottomRight, true);
                var v122 = new VertexCH(v1tr.Position[0] - size, v1tr.Position[1] - size, VertexCH.Direction.DownLeft, VertexCH.EdgeSide.CornerBottomLeft, true);

                var v211 = new VertexCH(v2br.Position[0] - size, v2br.Position[1] + size, VertexCH.Direction.UpLeft, VertexCH.EdgeSide.CornerTopLeft, true);
                var v212 = new VertexCH(v2lb.Position[0] + size, v2lb.Position[1] + size, VertexCH.Direction.UpRight, VertexCH.EdgeSide.CornerTopRight, true);
                var v221 = new VertexCH(v2tl.Position[0] + size, v2tl.Position[1] - size, VertexCH.Direction.DownRight, VertexCH.EdgeSide.CornerBottomRight, true);
                var v222 = new VertexCH(v2rt.Position[0] - size, v2rt.Position[1] - size, VertexCH.Direction.DownLeft, VertexCH.EdgeSide.CornerBottomLeft, true);

                if (i < 4)
                {
                    if (i != 0)
                    {
                        locations.Add(v111);
                        locations.Add(v112);
                        locations.Add(v121);
                        locations.Add(v122);
                    }

                    locations.Add(v211);
                    locations.Add(v212);
                    locations.Add(v221);
                    locations.Add(v222);
                }

                //Наконец, для всех наружных точек укажем, какой внутренней точке на соседнем квадрате они соответствуют с учётом возможных типов соединения
                v_1tl.SetShadows(v2lb, v2tl, v2br, v2rt);
                v_1rt.SetShadows(v2tl, v2rt, v2lb, v2br);
                v_1br.SetShadows(v2rt, v2br, v2tl, v2lb);
                v_1lb.SetShadows(v2br, v2lb, v2rt, v2tl);

                if (i != 0)
                {
                    v_1tl.SetShadows(v1bl, v1lt, v1rb, v1tr);
                    v_1rt.SetShadows(v1lt, v1tr, v1bl, v1rb);
                    v_1br.SetShadows(v1tr, v1rb, v1lt, v1bl);
                    v_1lb.SetShadows(v1rb, v1bl, v1tr, v1lt);
                }

                v_2tr.SetShadows(v2br, v2lb, v2rt, v2tl);
                v_2rb.SetShadows(v2lb, v2tl, v2br, v2rt);
                v_2bl.SetShadows(v2tl, v2rt, v2lb, v2br);
                v_2lt.SetShadows(v2rt, v2br, v2tl, v2lb);

                v111.SetShadows(v2br, v2lb, v2rt, v2tl);
                v112.SetShadows(v2lb, v2tl, v2br, v2rt);
                v121.SetShadows(v2tl, v2rt, v2lb, v2br);
                v122.SetShadows(v2rt, v2br, v2tl, v2lb);

                if (i != 0)
                {
                    v111.SetShadows(v1rb, v1bl, v1tr, v1lt);
                    v112.SetShadows(v1bl, v1lt, v1rb, v1tr);
                    v121.SetShadows(v1lt, v1tr, v1bl, v1rb);
                    v122.SetShadows(v1tr, v1rb, v1lt, v1bl);
                }

                v211.SetShadows(v2br, v2lb, v2rt, v2tl);
                v212.SetShadows(v2lb, v2tl, v2br, v2rt);
                v221.SetShadows(v2tl, v2rt, v2lb, v2br);
                v222.SetShadows(v2rt, v2br, v2tl, v2lb);
            }

            return locations;
        }

        /// <summary>
        /// Получаем плоскость, заполненную точками с распределением по Поиссону
        /// </summary>
        /// <param name="size"></param>
        /// <param name="count"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        private List<SimpleVector3d> BuildPoisson(int size, int count, double k)
        {
            float Sbig = size * size;
            var S1 = Sbig / count;
            var h = Math.Sqrt(S1 / (4 * Math.Sqrt(3)));
            var R = Math.Sqrt(S1 / (3 * Math.Sqrt(3)));

            R = (R + h) / 2;

            //Придётся сделать несколько попыток, чтобы добиться оптимального заполнения всей территории
            int c = 0;
            List<SimpleVector3d> cPoints = new List<SimpleVector3d>();
            //do
            //{
                cPoints = UniformPoissonDiskSampler.SampleRectangle(new SimpleVector3d(k, k, 0),
                                                    new SimpleVector3d(k + (size - 2 * k), k + (size - 2 * k), 0),
                                                        (float)R * 2);
            //    R *= 0.99f;
            //    c++;
            //}
            //while (cPoints.Count < count);

            return cPoints;
        }

        private Dictionary<CellCH, List<CellCH>> m_cZeroEdges = new Dictionary<CellCH, List<CellCH>>();

        private List<CellCH> m_cNewCells = new List<CellCH>();

        /// <summary>
        /// Восстанавливаем информацию о смежных гранях по выходным данным MIConvexHull.
        /// Возвращает минимальный Rect, полностью включающий в себя все вершины.
        /// </summary>
        /// <param name="locations"></param>
        /// <param name="cEdges"></param>
        /// <returns></returns>
        private Rect RebuildEdges(List<VertexCH> locations, IEnumerable<VoronoiEdge<VertexCH, CellCH>> cEdges)
        {
            float fMinX = float.MaxValue;
            float fMinY = float.MaxValue;
            float fMaxX = float.MinValue;
            float fMaxY = float.MinValue;

            m_cZeroEdges.Clear();
            m_cNewCells.Clear();

            foreach (var edge in cEdges)
            {
                var from = edge.Source;
                var to = edge.Target;

                double fMinDist = 1;//0.0000001;

                //сливаем близко расположенные вершины
                if (Point.Subtract(from.Circumcenter, to.Circumcenter).Length < fMinDist)
                {
                    //если from и to расположены слишком близко
                    //добавляем to в список вершин, сливаемых с from
                    List<CellCH> cFromList;
                    if (!m_cZeroEdges.TryGetValue(from, out cFromList))
                    {
                        cFromList = new List<CellCH>();
                        m_cZeroEdges[from] = cFromList;
                    }
                    cFromList.Add(to);
                    //добавляем from в список вершин, сливаемых с to
                    List<CellCH> cToList;
                    if (!m_cZeroEdges.TryGetValue(to, out cToList))
                    {
                        cToList = new List<CellCH>();
                        m_cZeroEdges[to] = cToList;
                    }
                    cToList.Add(from);
                    //больше не работаем с этой гранью!
                    continue;
                }

                VertexCH pLeft = null;
                VertexCH pRight = null;

                //для этого просканируем все локации, имеющие связь с начальной точкой ребра
                foreach (var n in from.Vertices)
                {
                    //и с конечной точкой ребра
                    foreach (var nnn in to.Vertices)
                    {
                        //нас интересуют те 2 локации, которые будут в обеих списках
                        if (n == nnn)
                        {
                            if (pLeft == null)
                                pLeft = n;
                            else
                            {
                                if (pRight == null)
                                    pRight = n;
                                else
                                    throw new Exception("Нашли больше 2 локаций с общей границей!");
                            }
                        }
                    }
                }

                if (pLeft == null || pRight == null)
                    throw new Exception("У границы меньше 2 сопредельных локаций!");

                //для порядка, определим порядок обхода вершин из локации - по часовой стрелке
                if (IsLeft(pLeft.ToPoint(), from.Circumcenter, to.Circumcenter) < 0)
                {
                    VertexCH pSwap = pLeft;
                    pLeft = pRight;
                    pRight = pSwap;
                }

                CellCH pMiddle = new CellCH(from, to);
                CellCH pInnerLeft = new CellCH(pMiddle, pLeft);
                CellCH pInnerRight = new CellCH(pMiddle, pRight);

                //пропишем ссылки на ребро в найденных локациях
                pLeft.m_cEdges[pRight] = new VertexCH.Edge(from, to, pMiddle, pInnerLeft);
                pRight.m_cEdges[pLeft] = new VertexCH.Edge(to, from, pMiddle, pInnerRight);

                m_cNewCells.Add(pMiddle);
                m_cNewCells.Add(pInnerLeft);
                m_cNewCells.Add(pInnerRight);

                if (pLeft.m_eGhost == VertexCH.Direction.CenterNone || pRight.m_eGhost == VertexCH.Direction.CenterNone)
                {
                    if (from.Circumcenter.X < fMinX)
                        fMinX = (float)from.Circumcenter.X;
                    if (from.Circumcenter.Y < fMinY)
                        fMinY = (float)from.Circumcenter.Y;
                    if (from.Circumcenter.X > fMaxX)
                        fMaxX = (float)from.Circumcenter.X;
                    if (from.Circumcenter.Y > fMaxY)
                        fMaxY = (float)from.Circumcenter.Y;
                }
            }

            foreach (var pLoc in locations)
            {
                foreach (var pEdge in pLoc.m_cEdges)
                {
                    if (m_cZeroEdges.ContainsKey(pEdge.Value.m_pFrom))
                        pEdge.Value.m_pFrom = SkipZero(pEdge.Value.m_pFrom);
                    if (m_cZeroEdges.ContainsKey(pEdge.Value.m_pTo))
                        pEdge.Value.m_pTo = SkipZero(pEdge.Value.m_pTo);
                }

                //if (pLoc.m_eGhost != VertexCH.Direction.CenterNone)
                //    continue;

                //List<VertexCH.Edge> cSequence = new List<VertexCH.Edge>();

                //VertexCH.Edge pLast = pLoc.m_cEdges.Values.First();
                //cSequence.Add(pLast);
                //for (int j = 0; j < pLoc.m_cEdges.Count; j++)
                //{
                //    foreach (var pEdge in pLoc.m_cEdges)
                //    {
                //        if (pEdge.Value.m_pFrom == pLast.m_pTo && !cSequence.Contains(pEdge.Value))
                //        {
                //            pLast = pEdge.Value;
                //            cSequence.Add(pLast);
                //            break;
                //        }
                //    }
                //}

                //if (cSequence.Count != pLoc.m_cEdges.Count)
                //{
                //    //foreach (var pEdge in pLoc.m_cEdges)
                //    //{
                //    //    if (m_cZeroEdges.ContainsKey(pEdge.Value.m_pFrom))
                //    //        pEdge.Value.m_pFrom = SkipZero(pEdge.Value.m_pFrom);
                //    //    if (m_cZeroEdges.ContainsKey(pEdge.Value.m_pTo))
                //    //        pEdge.Value.m_pTo = SkipZero(pEdge.Value.m_pTo);
                //    //}
                //    throw new Exception();
                //}
            }

            return new Rect(fMinX, fMinY, fMaxX - fMinX, fMaxY - fMinY);
        }

        Dictionary<CellCH, CellCH> m_cReplacements = new Dictionary<CellCH, CellCH>();

        /// <summary>
        /// Игнорируем слишком короткие рёбра
        /// </summary>
        /// <param name="pFrom"></param>
        /// <returns></returns>
        private CellCH SkipZero(CellCH pFrom)
        {
            if (m_cReplacements.ContainsKey(pFrom))
                return m_cReplacements[pFrom];

            Claim(pFrom, pFrom);
            return pFrom;
        }

        /// <summary>
        /// Объединяем вершины, соединённые слишком короткими рёбрами
        /// </summary>
        /// <param name="pAnchor"></param>
        /// <param name="pNewValue"></param>
        private void Claim(CellCH pAnchor, CellCH pNewValue)
        {
            m_cReplacements[pAnchor] = pNewValue;
            if (m_cZeroEdges.ContainsKey(pAnchor))
            {
                foreach (var pPretender in m_cZeroEdges[pAnchor])
                {
                    if (m_cReplacements.ContainsKey(pPretender))
                        continue;
                    Claim(pPretender, pNewValue);
                }
            }
        }

        public int R = 150;

        public Cube(int locationsCount, int iFaceSize, WorkingArea eArea)
        {
            var size = 1000;
            var kHR = 1.2 * size / Math.Sqrt(locationsCount);
            var kLR = 1.2 * size / Math.Sqrt(locationsCount/5);

            int iInnerCount;
            List<VertexCH> border = BuildBorder(out iInnerCount, kHR, size);
            List<VertexCH> locationsHR = new List<VertexCH>(border);
            List<VertexCH> locationsLR = new List<VertexCH>();

            Dictionary<VertexCH, VertexCH> pHi2Lo = new Dictionary<VertexCH, VertexCH>();
            foreach (var pBorderLocHR in border)
            {
                VertexCH pBorderLocLR = new VertexCH(pBorderLocHR);
                pHi2Lo[pBorderLocHR] = pBorderLocLR;
                locationsLR.Add(pBorderLocLR);
            }
            foreach (var pBorderLoc in pHi2Lo)
            {
                foreach(var pShadow in pBorderLoc.Key.m_cShadow)
                {
                    if(pShadow.Value == null)
                        continue;

                    pBorderLoc.Value.m_cShadow[pShadow.Key] = pHi2Lo[pShadow.Value];
                }
            }


            //Территорию внутри построенной границы заполним случайными точками с распределением по Поиссону (чтобы случайно, но в общем равномерно).
            List<SimpleVector3d> cPointsHR = BuildPoisson(size, locationsCount - iInnerCount, kHR);
            List<SimpleVector3d> cPointsLR = BuildPoisson(size, (locationsCount - iInnerCount)/5, kLR);

            //перенесём построенное облако Поиссона в основной рабочий массив
            for (var i = 0; i < cPointsHR.Count; i++)
            {
                var vi = new VertexCH(cPointsHR[i].X, cPointsHR[i].Y, VertexCH.Direction.CenterNone, VertexCH.EdgeSide.Inside, false);
                locationsHR.Add(vi);
            }
            for (var i = 0; i < cPointsLR.Count; i++)
            {
                var vi = new VertexCH(cPointsLR[i].X, cPointsLR[i].Y, VertexCH.Direction.CenterNone, VertexCH.EdgeSide.Inside, false);
                locationsLR.Add(vi);
            }

            //Наконец, строим диаграмму Вороного.
            VoronoiMesh<VertexCH, CellCH, VoronoiEdge<VertexCH, CellCH>> voronoiMeshHR = VoronoiMesh.Create<VertexCH, CellCH>(locationsHR);
            VoronoiMesh<VertexCH, CellCH, VoronoiEdge<VertexCH, CellCH>> voronoiMeshLR = VoronoiMesh.Create<VertexCH, CellCH>(locationsLR);

            //Переведём результат в удобный нам формат.
            //Для каждого найденного ребра диаграммы Вороного найдём локации, которые оно разделяет
            Rect pBoundingRectHR = RebuildEdges(locationsHR, voronoiMeshHR.Edges);

            m_cNewCells.AddRange(voronoiMeshHR.Vertices);
            var locsHR = locationsHR.ToArray();
            var vertsHR = m_cNewCells.ToArray();

            foreach (var ploc in locsHR)
            {
                if (!ploc.m_bBorder)
                {
                    foreach (var pedge in ploc.m_cEdges)
                        if (pedge.Key.m_eGhost != VertexCH.Direction.CenterNone)
                            throw new Exception();
                }

            }

            m_cNewCells.Clear();
            Rect pBoundingRectLR = RebuildEdges(locationsLR, voronoiMeshLR.Edges);

            m_cNewCells.AddRange(voronoiMeshLR.Vertices);
            var locsLR = locationsLR.ToArray();
            var vertsLR = m_cNewCells.ToArray();

            foreach (var ploc in locsLR)
            {
                if (!ploc.m_bBorder)
                {
                    foreach (var pedge in ploc.m_cEdges)
                        if (pedge.Key.m_eGhost != VertexCH.Direction.CenterNone)
                            throw new Exception();
                }

            }

            m_cFaces[Face3D.Backward] = new CubeFace(iFaceSize, pBoundingRectHR, ref locsHR, ref vertsHR, size, R, Face3D.Backward);
            //m_cFaces[Face3D.Bottom] = new CubeFace(iFaceSize, pBoundingRectHR, ref locsHR, ref vertsHR, size, R, Face3D.Bottom);
            //m_cFaces[Face3D.Forward] = new CubeFace(iFaceSize, pBoundingRectHR, ref locsHR, ref vertsHR, size, R, Face3D.Forward);
            //m_cFaces[Face3D.Left] = new CubeFace(iFaceSize, pBoundingRectHR, ref locsHR, ref vertsHR, size, R, Face3D.Left);
            //m_cFaces[Face3D.Right] = new CubeFace(iFaceSize, pBoundingRectHR, ref locsHR, ref vertsHR, size, R, Face3D.Right);
            //m_cFaces[Face3D.Top] = new CubeFace(iFaceSize, pBoundingRectHR, ref locsHR, ref vertsHR, size, R, Face3D.Top);
            if (eArea == WorkingArea.OneFace)
            {
                m_cFaces[Face3D.Bottom] = new CubeFace(iFaceSize, pBoundingRectLR, ref locsLR, ref vertsLR, size, R, Face3D.Bottom);
                m_cFaces[Face3D.Forward] = new CubeFace(iFaceSize, pBoundingRectLR, ref locsLR, ref vertsLR, size, R, Face3D.Forward);
                m_cFaces[Face3D.Left] = new CubeFace(iFaceSize, pBoundingRectLR, ref locsLR, ref vertsLR, size, R, Face3D.Left);
                m_cFaces[Face3D.Right] = new CubeFace(iFaceSize, pBoundingRectLR, ref locsLR, ref vertsLR, size, R, Face3D.Right);
                m_cFaces[Face3D.Top] = new CubeFace(iFaceSize, pBoundingRectLR, ref locsLR, ref vertsLR, size, R, Face3D.Top);
            }
            else
            {
                m_cFaces[Face3D.Left] = new CubeFace(iFaceSize, pBoundingRectHR, ref locsHR, ref vertsHR, size, R, Face3D.Left);

                if(eArea == WorkingArea.WholeSphere)
                {
                    m_cFaces[Face3D.Bottom] = new CubeFace(iFaceSize, pBoundingRectHR, ref locsHR, ref vertsHR, size, R, Face3D.Bottom);
                    m_cFaces[Face3D.Forward] = new CubeFace(iFaceSize, pBoundingRectHR, ref locsHR, ref vertsHR, size, R, Face3D.Forward);
                    m_cFaces[Face3D.Left] = new CubeFace(iFaceSize, pBoundingRectHR, ref locsHR, ref vertsHR, size, R, Face3D.Left);
                    m_cFaces[Face3D.Top] = new CubeFace(iFaceSize, pBoundingRectHR, ref locsHR, ref vertsHR, size, R, Face3D.Top);
                }
                else
                {
                    if (eArea == WorkingArea.HalfSphereEquatorial)
                    {
                        m_cFaces[Face3D.Bottom] = new CubeFace(iFaceSize, pBoundingRectHR, ref locsHR, ref vertsHR, size, R, Face3D.Bottom);
                        m_cFaces[Face3D.Top] = new CubeFace(iFaceSize, pBoundingRectLR, ref locsLR, ref vertsLR, size, R, Face3D.Top);
                    }
                    else
                    {
                        m_cFaces[Face3D.Bottom] = new CubeFace(iFaceSize, pBoundingRectLR, ref locsLR, ref vertsLR, size, R, Face3D.Bottom);
                        m_cFaces[Face3D.Top] = new CubeFace(iFaceSize, pBoundingRectHR, ref locsHR, ref vertsHR, size, R, Face3D.Top);
                    }
                    //m_cFaces[Face3D.Bottom] = new CubeFace(iFaceSize, pBoundingRectLR, ref locsLR, ref vertsLR, size, R, Face3D.Bottom);
                    //m_cFaces[Face3D.Top] = new CubeFace(iFaceSize, pBoundingRectLR, ref locsLR, ref vertsLR, size, R, Face3D.Top);
                    m_cFaces[Face3D.Forward] = new CubeFace(iFaceSize, pBoundingRectLR, ref locsLR, ref vertsLR, size, R, Face3D.Forward);
                    m_cFaces[Face3D.Right] = new CubeFace(iFaceSize, pBoundingRectLR, ref locsLR, ref vertsLR, size, R, Face3D.Right);
                }
            }

            m_cFaces[Face3D.Backward].LinkNeighbours(m_cFaces[Face3D.Top], VertexCH.Transformation.Rotate90CW,
                                                        m_cFaces[Face3D.Top], VertexCH.Transformation.Rotate180,
                                                        m_cFaces[Face3D.Top], VertexCH.Transformation.Rotate90CCW,
                                                        m_cFaces[Face3D.Left], VertexCH.Transformation.Stright,
                                                        m_cFaces[Face3D.Bottom], VertexCH.Transformation.Rotate90CW,
                                                        m_cFaces[Face3D.Bottom], VertexCH.Transformation.Rotate180,
                                                        m_cFaces[Face3D.Top], VertexCH.Transformation.Rotate90CCW,
                                                        m_cFaces[Face3D.Right], VertexCH.Transformation.Stright);
            m_cFaces[Face3D.Bottom].LinkNeighbours(m_cFaces[Face3D.Left], VertexCH.Transformation.Stright,
                                                        m_cFaces[Face3D.Forward], VertexCH.Transformation.Stright,
                                                        m_cFaces[Face3D.Right], VertexCH.Transformation.Stright,
                                                        m_cFaces[Face3D.Right], VertexCH.Transformation.Rotate90CW,
                                                        m_cFaces[Face3D.Backward], VertexCH.Transformation.Rotate90CW,
                                                        m_cFaces[Face3D.Backward], VertexCH.Transformation.Rotate180,
                                                        m_cFaces[Face3D.Backward], VertexCH.Transformation.Rotate90CCW,
                                                        m_cFaces[Face3D.Left], VertexCH.Transformation.Rotate90CCW);
            m_cFaces[Face3D.Forward].LinkNeighbours(m_cFaces[Face3D.Top], VertexCH.Transformation.Rotate90CCW,
                                                        m_cFaces[Face3D.Top], VertexCH.Transformation.Stright,
                                                        m_cFaces[Face3D.Top], VertexCH.Transformation.Rotate90CW,
                                                        m_cFaces[Face3D.Right], VertexCH.Transformation.Stright,
                                                        m_cFaces[Face3D.Bottom], VertexCH.Transformation.Rotate90CCW,
                                                        m_cFaces[Face3D.Bottom], VertexCH.Transformation.Stright,
                                                        m_cFaces[Face3D.Bottom], VertexCH.Transformation.Rotate90CW,
                                                        m_cFaces[Face3D.Left], VertexCH.Transformation.Stright);
            m_cFaces[Face3D.Left].LinkNeighbours(m_cFaces[Face3D.Top], VertexCH.Transformation.Rotate180,
                                                        m_cFaces[Face3D.Top], VertexCH.Transformation.Rotate90CCW,
                                                        m_cFaces[Face3D.Top], VertexCH.Transformation.Stright,
                                                        m_cFaces[Face3D.Forward], VertexCH.Transformation.Stright,
                                                        m_cFaces[Face3D.Bottom], VertexCH.Transformation.Stright,
                                                        m_cFaces[Face3D.Bottom], VertexCH.Transformation.Rotate90CW,
                                                        m_cFaces[Face3D.Bottom], VertexCH.Transformation.Rotate180,
                                                        m_cFaces[Face3D.Backward], VertexCH.Transformation.Stright);
            m_cFaces[Face3D.Right].LinkNeighbours(m_cFaces[Face3D.Top], VertexCH.Transformation.Stright,
                                                        m_cFaces[Face3D.Top], VertexCH.Transformation.Rotate90CW,
                                                        m_cFaces[Face3D.Top], VertexCH.Transformation.Rotate180,
                                                        m_cFaces[Face3D.Backward], VertexCH.Transformation.Stright,
                                                        m_cFaces[Face3D.Bottom], VertexCH.Transformation.Rotate180,
                                                        m_cFaces[Face3D.Bottom], VertexCH.Transformation.Rotate90CCW,
                                                        m_cFaces[Face3D.Bottom], VertexCH.Transformation.Stright,
                                                        m_cFaces[Face3D.Forward], VertexCH.Transformation.Stright);
            m_cFaces[Face3D.Top].LinkNeighbours(m_cFaces[Face3D.Backward], VertexCH.Transformation.Rotate90CW,
                                                        m_cFaces[Face3D.Backward], VertexCH.Transformation.Rotate180,
                                                        m_cFaces[Face3D.Backward], VertexCH.Transformation.Rotate90CCW,
                                                        m_cFaces[Face3D.Right], VertexCH.Transformation.Rotate90CCW,
                                                        m_cFaces[Face3D.Right], VertexCH.Transformation.Stright,
                                                        m_cFaces[Face3D.Forward], VertexCH.Transformation.Stright,
                                                        m_cFaces[Face3D.Left], VertexCH.Transformation.Stright,
                                                        m_cFaces[Face3D.Left], VertexCH.Transformation.Rotate90CW);

            foreach (var pFace in m_cFaces)
                pFace.Value.Ghostbusting();

            foreach (var pFace in m_cFaces)
                pFace.Value.Finalize();

            //foreach (var pFace in m_cFaces)
            //    foreach(var pChunk in pFace.Value.m_cChunk)
            //        pChunk.DebugVertexes();
        }
    }
}
