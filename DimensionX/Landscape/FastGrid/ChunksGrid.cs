using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleVectors;
using System.Windows;
using MIConvexHull;

namespace LandscapeGeneration.FastGrid
{
    public class ChunksGrid<LOC> : IGrid<LOC>
        where LOC : Location, new()
    {
        public Chunk<LOC>[,] m_cChunk;

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
                var y1 = k * 0.5 + k * (fDeltaY - fDeltaY * 2 * r.NextDouble());

                var x2 = k * 0.5 + i * k + k * (fDeltaX - fDeltaX * 2 * r.NextDouble());
                var y2 = k * 0.5 + k * (fDeltaY - fDeltaY * 2 * r.NextDouble());

                //Внутренние точки квадрата. Сначала - по часовой стрелке.
                //Важно: в XNA используется правая координатная система, а значит ось Y должна быть направлена ВВЕРХ

                var v2tl = new VertexCH(x2, size - y2, VertexCH.Direction.CenterNone, VertexCH.EdgeSide.TopLeft, true);
                var v2rt = new VertexCH(size - y2, size - x2, VertexCH.Direction.CenterNone, VertexCH.EdgeSide.RightTop, true);
                var v2br = new VertexCH(size - x2, y2, VertexCH.Direction.CenterNone, VertexCH.EdgeSide.BottomRight, true);
                var v2lb = new VertexCH(y2, x2, VertexCH.Direction.CenterNone, VertexCH.EdgeSide.LeftBottom, true);

                //Потом - против часовой.

                var v1tr = new VertexCH(size - x1, size - y1, VertexCH.Direction.CenterNone, VertexCH.EdgeSide.TopRight, true);
                var v1rb = new VertexCH(size - y1, x1, VertexCH.Direction.CenterNone, VertexCH.EdgeSide.RightBottom, true);
                var v1bl = new VertexCH(x1, y1, VertexCH.Direction.CenterNone, VertexCH.EdgeSide.BottomLeft, true);
                var v1lt = new VertexCH(y1, size - x1, VertexCH.Direction.CenterNone, VertexCH.EdgeSide.LeftTop, true);

                if (i == 0)
                {
                    v2rt = v1tr;
                    v2lb = v1bl;
                    v2tl = v1lt;
                    v2br = v1rb;
                }

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

                //Если это первый шаг, то наружная точка по часовой должна быть тенью внутренней против часовой
                var v_1tl = new VertexCH(v1bl.Position[0], v1bl.Position[1] + size, VertexCH.Direction.Up, VertexCH.EdgeSide.TopLeft, true);
                locations.Add(v_1tl);

                var v_1rt = new VertexCH(v1lt.Position[0] + size, v1lt.Position[1], VertexCH.Direction.Right, VertexCH.EdgeSide.RightTop, true);
                locations.Add(v_1rt);

                var v_1br = new VertexCH(v1tr.Position[0], v1tr.Position[1] - size, VertexCH.Direction.Down, VertexCH.EdgeSide.BottomRight, true);
                locations.Add(v_1br);

                var v_1lb = new VertexCH(v1rb.Position[0] - size, v1rb.Position[1], VertexCH.Direction.Left, VertexCH.EdgeSide.LeftBottom, true);
                locations.Add(v_1lb);

                //Для наружные точки против часовой являются тенями внутренних по часовой
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
                if (i != 0)
                {
                    v_1tl.SetShadows(v1bl);
                    v_1rt.SetShadows(v1lt);
                    v_1br.SetShadows(v1tr);
                    v_1lb.SetShadows(v1rb);
                }
                else
                {
                    v_1tl.SetShadows(v2lb);
                    v_1rt.SetShadows(v2tl);
                    v_1br.SetShadows(v2rt);
                    v_1lb.SetShadows(v2br);
                }

                v_2tr.SetShadows(v2br);
                v_2rb.SetShadows(v2lb);
                v_2bl.SetShadows(v2tl);
                v_2lt.SetShadows(v2rt);

                if (i < 4)
                {
                    //v111.SetShadows(v2br);
                    //v112.SetShadows(v2lb);
                    //v121.SetShadows(v2tl);
                    //v122.SetShadows(v2rt);

                    if (i != 0)
                    {
                        v111.SetShadows(v1rb);
                        v112.SetShadows(v1bl);
                        v121.SetShadows(v1lt);
                        v122.SetShadows(v1tr);
                    }

                    v211.SetShadows(v2br);
                    v212.SetShadows(v2lb);
                    v221.SetShadows(v2tl);
                    v222.SetShadows(v2rt);
                }
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
            R *= 0.96;

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
                pLeft.m_cEdges[pRight] = new VertexCH.Edge(from, to);
                pRight.m_cEdges[pLeft] = new VertexCH.Edge(to, from);

                m_cNewCells.Add(pMiddle);
                m_cNewCells.Add(pInnerLeft);
                m_cNewCells.Add(pInnerRight);

                if (pLeft.m_eShadowDir == VertexCH.Direction.CenterNone || pRight.m_eShadowDir == VertexCH.Direction.CenterNone)
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

        public readonly int m_iChunkSize = 20000; // WTF??? Why this walue is critical for proper work of MapDraw::CheckMousePosition()?
        public readonly int Resolution;

        public int RX
        {
            get
            {
                return m_iChunkSize * Resolution / 2;
            }
        }

        public int RY
        {
            get
            {
                return m_iChunkSize * Resolution / 2;
            }
        }
        public int FrameWidth 
        { 
            get { return m_iChunkSize / 100;  } 
        }

        public WorldShape m_eShape = WorldShape.Plain;

        public float CycleShift
        {
            get { return m_eShape == WorldShape.Ringworld ? RX * 2 : 0; }
        }

        private LOC[] m_aLocations;

        public VoronoiVertex[] m_aVertexes;

        public VoronoiVertex[] Vertexes { get => m_aVertexes; }

        public LOC[] Locations { get => m_aLocations; set => m_aLocations = value; }

        public ChunksGrid(int locationsCount, int iFaceSize, BeginStepDelegate BeginStep, ProgressStepDelegate ProgressStep)
        {
            Resolution = iFaceSize;

            var kHR = 1.2 * m_iChunkSize / Math.Sqrt(locationsCount);

            if (BeginStep != null)
                BeginStep("Building grid...", 5);

            int iInnerCount;
            List<VertexCH> border = BuildBorder(out iInnerCount, kHR, m_iChunkSize);
            List<VertexCH> locationsHR = new List<VertexCH>(border);

            if (ProgressStep != null)
                ProgressStep();

            //Территорию внутри построенной границы заполним случайными точками с распределением по Поиссону (чтобы случайно, но в общем равномерно).
            List<SimpleVector3d> cPointsHR = BuildPoisson(m_iChunkSize, locationsCount - iInnerCount, kHR);
            
            //перенесём построенное облако Поиссона в основной рабочий массив
            for (var i = 0; i < cPointsHR.Count; i++)
            {
                var vi = new VertexCH(cPointsHR[i].X, cPointsHR[i].Y, VertexCH.Direction.CenterNone, VertexCH.EdgeSide.Inside, false);
                locationsHR.Add(vi);
            }

            if (ProgressStep != null)
                ProgressStep();

            //Наконец, строим диаграмму Вороного.
            VoronoiMesh<VertexCH, CellCH, VoronoiEdge<VertexCH, CellCH>> voronoiMeshHR = VoronoiMesh.Create<VertexCH, CellCH>(locationsHR);

            if (ProgressStep != null)
                ProgressStep();

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
                        if (pedge.Key.m_eShadowDir != VertexCH.Direction.CenterNone)
                            throw new Exception();
                }

            }

            if (ProgressStep != null)
                ProgressStep();

            if (BeginStep != null)
                BeginStep("Forming grid...", 15);

            m_cChunk = new Chunk<LOC>[iFaceSize, iFaceSize];

            for (int x = 0; x < iFaceSize; x++)
                for (int y = 0; y < iFaceSize; y++)
                    m_cChunk[x, y] = new Chunk<LOC>(ref locsHR, pBoundingRectHR, ref vertsHR, m_iChunkSize * x, m_iChunkSize * y, RX * 2);
            
            LinkNeighbours();
            
            if (ProgressStep != null)
                ProgressStep();

            foreach (var pChunk in m_cChunk)
                pChunk.Ghostbusters();

            if (ProgressStep != null)
                ProgressStep();

            foreach (var pChunk in m_cChunk)
            {
                pChunk.Final(CycleShift);
            }

            if (ProgressStep != null)
                ProgressStep();

            List<LOC> cLocations = new List<LOC>();
            foreach (var pChunk in m_cChunk)
                cLocations.AddRange(pChunk.m_aLocations);

            m_aLocations = cLocations.ToArray();

            List<VoronoiVertex> cVertexes = new List<VoronoiVertex>();
            foreach (var pChunk in m_cChunk)
                cVertexes.AddRange(pChunk.m_aVertexes);

            m_aVertexes = cVertexes.ToArray();
        }

        public void LinkNeighbours()
        {
            for (int x = 0; x < Resolution; x++)
            {
                for (int y = 0; y < Resolution; y++)
                {
                    if (x > 0 && y > 0)
                        m_cChunk[x, y].m_cNeighbours[VertexCH.Direction.DownLeft] = m_cChunk[x - 1, y - 1];

                    if (y > 0)
                        m_cChunk[x, y].m_cNeighbours[VertexCH.Direction.Down] = m_cChunk[x, y - 1];

                    if (x < Resolution - 1 && y > 0)
                        m_cChunk[x, y].m_cNeighbours[VertexCH.Direction.DownRight] = m_cChunk[x + 1, y - 1];

                    if (x < Resolution - 1)
                        m_cChunk[x, y].m_cNeighbours[VertexCH.Direction.Right] = m_cChunk[x + 1, y];
                    else
                    {
                        if (m_eShape == WorldShape.Ringworld)
                            m_cChunk[x, y].m_cNeighbours[VertexCH.Direction.Right] = m_cChunk[0, y];
                    }

                    if (x < Resolution - 1 && y < Resolution - 1)
                        m_cChunk[x, y].m_cNeighbours[VertexCH.Direction.UpRight] = m_cChunk[x + 1, y + 1];

                    if (y < Resolution - 1)
                        m_cChunk[x, y].m_cNeighbours[VertexCH.Direction.Up] = m_cChunk[x, y + 1];

                    if (x > 0 && y < Resolution - 1)
                        m_cChunk[x, y].m_cNeighbours[VertexCH.Direction.UpLeft] = m_cChunk[x - 1, y + 1];

                    if (x > 0)
                        m_cChunk[x, y].m_cNeighbours[VertexCH.Direction.Left] = m_cChunk[x - 1, y];
                    else
                    {
                        if (m_eShape == WorldShape.Ringworld)
                            m_cChunk[x, y].m_cNeighbours[VertexCH.Direction.Left] = m_cChunk[Resolution - 1, y];
                    }
                }
            }
        }
    }
}
