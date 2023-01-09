using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleVectors;
using System.Windows;
using MIConvexHull;

namespace LandscapeGeneration.FastGrid
{
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
        /// Панцирь черепахи - круглый плоский мир с горами в центре, со всех сторон окружённый бесконечным океаном
        /// </summary>
        Shell,
        /// <summary>
        /// Чаша - круглый плоский мир с центральным морем, со всех сторон окружённый непроходимыми горами
        /// </summary>
        Chalice
    }

    public class LocationsGrid
    {
        private readonly Chunk[,] m_cChunk;

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

        private readonly System.Random rnd = new System.Random();

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

            float fDeltaX = 0.1f;
            float fDeltaY = 0.25f;

            iInnerCount = 0;

            for (int i = 0; i <= quanticSize / 2; i++)
            {
                var x1 = k * 0.5 + i * k + k * (fDeltaX - fDeltaX * 2 * rnd.NextDouble());
                var y1 = k * 0.5 + k * (fDeltaY - fDeltaY * 2 * rnd.NextDouble());

                var x2 = k * 0.5 + i * k + k * (fDeltaX - fDeltaX * 2 * rnd.NextDouble());
                var y2 = k * 0.5 + k * (fDeltaY - fDeltaY * 2 * rnd.NextDouble());

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

            List<SimpleVector3d> cPoints = UniformPoissonDiskSampler.SampleRectangle(new SimpleVector3d(k, k, 0),
                                                new SimpleVector3d(k + (size - 2 * k), k + (size - 2 * k), 0),
                                                    (float)R * 2);

            return cPoints;
        }

        private readonly Dictionary<CellCH, List<CellCH>> m_cZeroEdges = new Dictionary<CellCH, List<CellCH>>();

        private readonly List<CellCH> m_cNewCells = new List<CellCH>();

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

                const double fMinDist = 1;

                //сливаем близко расположенные вершины
                if (Point.Subtract(from.Circumcenter, to.Circumcenter).Length < fMinDist)
                {
                    //если from и to расположены слишком близко
                    //добавляем to в список вершин, сливаемых с from
                    if (!m_cZeroEdges.TryGetValue(from, out List<CellCH> cFromList))
                    {
                        cFromList = new List<CellCH>();
                        m_cZeroEdges[from] = cFromList;
                    }
                    cFromList.Add(to);
                    //добавляем from в список вершин, сливаемых с to
                    if (!m_cZeroEdges.TryGetValue(to, out List<CellCH> cToList))
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
                            {
                                pLeft = n;
                            }
                            else
                            {
                                if (pRight == null)
                                    pRight = n;
                                else
                                    throw new InvalidOperationException("Нашли больше 2 локаций с общей границей!");
                            }
                        }
                    }
                }

                if (pLeft == null || pRight == null)
                    throw new InvalidOperationException("У границы меньше 2 сопредельных локаций!");

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
                pLeft.Edges[pRight] = new VertexCH.Edge(from, to);
                pRight.Edges[pLeft] = new VertexCH.Edge(to, from);

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
                foreach (VertexCH.Edge pEdge in pLoc.Edges.Values)
                {
                    if (m_cZeroEdges.ContainsKey(pEdge.From))
                        pEdge.From = SkipZero(pEdge.From);
                    if (m_cZeroEdges.ContainsKey(pEdge.To))
                        pEdge.To = SkipZero(pEdge.To);
                }
            }

            return new Rect(fMinX, fMinY, fMaxX - fMinX, fMaxY - fMinY);
        }

        private readonly Dictionary<CellCH, CellCH> m_cReplacements = new Dictionary<CellCH, CellCH>();

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

        public WorldShape Shape { get; private set; } = WorldShape.Plain;

        public float CycleShift
        {
            get { return Shape == WorldShape.Ringworld ? RX * 2 : 0; }
        }

        public Location[] Locations { get; } = null;

        public LocationsGrid(int locationsCount, int iFaceSize, BeginStepDelegate BeginStep, ProgressStepDelegate ProgressStep)
        {
            Resolution = iFaceSize;

            var kHR = 1.2 * m_iChunkSize / Math.Sqrt(locationsCount);

            BeginStep?.Invoke("Building grid...", 5);

            int iInnerCount;
            List<VertexCH> border = BuildBorder(out iInnerCount, kHR, m_iChunkSize);
            List<VertexCH> locationsHR = new List<VertexCH>(border);

            ProgressStep?.Invoke();

            //Территорию внутри построенной границы заполним случайными точками с распределением по Поиссону (чтобы случайно, но в общем равномерно).
            List<SimpleVector3d> cPointsHR = BuildPoisson(m_iChunkSize, locationsCount - iInnerCount, kHR);
            
            //перенесём построенное облако Поиссона в основной рабочий массив
            for (var i = 0; i < cPointsHR.Count; i++)
            {
                var vi = new VertexCH(cPointsHR[i].X, cPointsHR[i].Y, VertexCH.Direction.CenterNone, VertexCH.EdgeSide.Inside, false);
                locationsHR.Add(vi);
            }

            ProgressStep?.Invoke();

            //Наконец, строим диаграмму Вороного.
            VoronoiMesh<VertexCH, CellCH, VoronoiEdge<VertexCH, CellCH>> voronoiMeshHR = VoronoiMesh.Create<VertexCH, CellCH>(locationsHR);

            ProgressStep?.Invoke();

            //Переведём результат в удобный нам формат.
            //Для каждого найденного ребра диаграммы Вороного найдём локации, которые оно разделяет
            Rect pBoundingRectHR = RebuildEdges(locationsHR, voronoiMeshHR.Edges);

            m_cNewCells.AddRange(voronoiMeshHR.Vertices);
            var locsHR = locationsHR.ToArray();
            var vertsHR = m_cNewCells.ToArray();

            foreach (var ploc in locsHR)
            {
                if (!ploc.IsBorder)
                {
                    foreach (var pedge in ploc.Edges)
                        if (pedge.Key.m_eShadowDir != VertexCH.Direction.CenterNone)
                            throw new InvalidOperationException();
                }

            }

            ProgressStep?.Invoke();

            BeginStep?.Invoke("Forming grid...", 15);

            m_cChunk = new Chunk[iFaceSize, iFaceSize];

            for (int x = 0; x < iFaceSize; x++)
                for (int y = 0; y < iFaceSize; y++)
                    m_cChunk[x, y] = new Chunk(ref locsHR, pBoundingRectHR, ref vertsHR, m_iChunkSize * x, m_iChunkSize * y, RX * 2);

            LinkNeighbours();

            ProgressStep?.Invoke();

            foreach (var pChunk in m_cChunk)
                pChunk.Ghostbusters();

            ProgressStep?.Invoke();

            foreach (var pChunk in m_cChunk)
                pChunk.Final(CycleShift);

            ProgressStep?.Invoke();

            List<Location> cLocations = new List<Location>();
            foreach (var pChunk in m_cChunk)
                cLocations.AddRange(pChunk.Locations);

            Locations = cLocations.ToArray();
        }

        public void LinkNeighbours()
        {
            for (int x = 0; x < Resolution; x++)
            {
                for (int y = 0; y < Resolution; y++)
                {
                    if (x > 0 && y > 0)
                        m_cChunk[x, y].Neighbours[VertexCH.Direction.DownLeft] = m_cChunk[x - 1, y - 1];

                    if (y > 0)
                        m_cChunk[x, y].Neighbours[VertexCH.Direction.Down] = m_cChunk[x, y - 1];

                    if (x < Resolution - 1 && y > 0)
                        m_cChunk[x, y].Neighbours[VertexCH.Direction.DownRight] = m_cChunk[x + 1, y - 1];

                    if (x < Resolution - 1)
                    {
                        m_cChunk[x, y].Neighbours[VertexCH.Direction.Right] = m_cChunk[x + 1, y];
                    }
                    else
                    {
                        if (Shape == WorldShape.Ringworld)
                            m_cChunk[x, y].Neighbours[VertexCH.Direction.Right] = m_cChunk[0, y];
                    }

                    if (x < Resolution - 1 && y < Resolution - 1)
                        m_cChunk[x, y].Neighbours[VertexCH.Direction.UpRight] = m_cChunk[x + 1, y + 1];

                    if (y < Resolution - 1)
                        m_cChunk[x, y].Neighbours[VertexCH.Direction.Up] = m_cChunk[x, y + 1];

                    if (x > 0 && y < Resolution - 1)
                        m_cChunk[x, y].Neighbours[VertexCH.Direction.UpLeft] = m_cChunk[x - 1, y + 1];

                    if (x > 0)
                    {
                        m_cChunk[x, y].Neighbours[VertexCH.Direction.Left] = m_cChunk[x - 1, y];
                    }
                    else
                    {
                        if (Shape == WorldShape.Ringworld)
                            m_cChunk[x, y].Neighbours[VertexCH.Direction.Left] = m_cChunk[Resolution - 1, y];
                    }
                }
            }
        }
    }
}
