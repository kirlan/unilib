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
            return ((b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X)) > 0 ? 1 : -1;
        }

        public List<VertexCH> locations = new List<VertexCH>();
        public List<CellCH> vertices = new List<CellCH>();

        public Cube()
        {
            var locationsCount = 2500;// 2500;

            var size = 1000;
//            List<VertexCH> vertices = new List<VertexCH>();
            var r = new System.Random();

            var k = 1.2 * size / Math.Sqrt(locationsCount);
//            var k = size / Math.Sqrt(locationsCount);

            int quanticSize = (int)(size / k);

            bool bCutOff = true;

            float fDelta = 0.4f;//0.25f;

            for (int i = 0; i <= quanticSize/2; i++)
            {
                var x1 = k * 0.5 + i * k + k * (fDelta - fDelta * 2 * r.NextDouble());
                var y1 = -k * 0.5 + k * (fDelta - fDelta * 2 * r.NextDouble());

                var x2 = k * 0.5 + i * k + k * (fDelta - fDelta * 2 * r.NextDouble());
                var y2 = k * 0.5 + k * (fDelta - fDelta * 2 * r.NextDouble());

                //Внутренние точки квадрата. Сначала - по часовой стрелке.

                var v2tl = new VertexCH(x2, y2, VertexCH.Direction.CenterNone, VertexCH.EdgeSide.TopLeft);
                var v2rt = new VertexCH(size - y2, x2, VertexCH.Direction.CenterNone, VertexCH.EdgeSide.RightTop);
                var v2br = new VertexCH(size - x2, size - y2, VertexCH.Direction.CenterNone, VertexCH.EdgeSide.BottomRight);
                var v2lb = new VertexCH(y2, size - x2, VertexCH.Direction.CenterNone, VertexCH.EdgeSide.LeftBottom);

                //Потом - против часовой.

                var v1tr = new VertexCH(size - x1, -y1, VertexCH.Direction.CenterNone, VertexCH.EdgeSide.TopRight);
                var v1rb = new VertexCH(size + y1, size - x1, VertexCH.Direction.CenterNone, VertexCH.EdgeSide.RightBottom);
                var v1bl = new VertexCH(x1, size + y1, VertexCH.Direction.CenterNone, VertexCH.EdgeSide.BottomLeft);
                var v1lt = new VertexCH(-y1, x1, VertexCH.Direction.CenterNone, VertexCH.EdgeSide.LeftTop);

                //Те, которые по часовой - добавляем по любому.
                locations.Add(v2tl);
                locations.Add(v2br);
                locations.Add(v2lb);
                locations.Add(v2rt);

                //Те, которые против часовой - на первом шаге пропускаем, т.к. они будут путаться в углах с теми, которые по часовой.
                if (i != 0 || !bCutOff)
                {
                    locations.Add(v1tr);
                    locations.Add(v1bl);
                    locations.Add(v1lt);
                    locations.Add(v1rb);
                }

                //Дальше считаем наружные точки. Наружные точки добаволяем всегда, но они могут быть отражением различных внутренних точек
                
                //Если это первый шаг, то наружная точка по часовой должна быть тенью внутренней по часовой, т.к. на первом шаге у нас нет внутренних против часовой
                var v_1tl = new VertexCH(v2lb.Position[0], v2lb.Position[1] - size, VertexCH.Direction.Up, VertexCH.EdgeSide.TopLeft);
                if (i != 0 || !bCutOff) //иначе - внутренней против часовой
                    v_1tl = new VertexCH(v1bl.Position[0], v1bl.Position[1] - size, VertexCH.Direction.Up, VertexCH.EdgeSide.TopLeft);
                locations.Add(v_1tl);

                var v_1rt = new VertexCH(v2tl.Position[0] + size, v2tl.Position[1], VertexCH.Direction.Right, VertexCH.EdgeSide.RightTop);
                if (i != 0 || !bCutOff)
                    v_1rt = new VertexCH(v1lt.Position[0] + size, v1lt.Position[1], VertexCH.Direction.Right, VertexCH.EdgeSide.RightTop);
                locations.Add(v_1rt);

                var v_1br = new VertexCH(v2rt.Position[0], v2rt.Position[1] + size, VertexCH.Direction.Down, VertexCH.EdgeSide.BottomRight);
                if (i != 0 || !bCutOff)
                    v_1br = new VertexCH(v1tr.Position[0], v1tr.Position[1] + size, VertexCH.Direction.Down, VertexCH.EdgeSide.BottomRight);
                locations.Add(v_1br);

                var v_1lb = new VertexCH(v2br.Position[0] - size, v2br.Position[1], VertexCH.Direction.Left, VertexCH.EdgeSide.LeftBottom);
                if (i != 0 || !bCutOff)
                    v_1lb = new VertexCH(v1rb.Position[0] - size, v1rb.Position[1], VertexCH.Direction.Left, VertexCH.EdgeSide.LeftBottom);
                locations.Add(v_1lb);

                //Для наружных точек против часовой всё проще, т.к. они являются тенями внутренних по часовой, которые у нас есть всегда
                var v_2tr = new VertexCH(v2br.Position[0], v2br.Position[1] - size, VertexCH.Direction.Up, VertexCH.EdgeSide.TopRight);
                locations.Add(v_2tr);

                var v_2rb = new VertexCH(v2lb.Position[0] + size, v2lb.Position[1], VertexCH.Direction.Right, VertexCH.EdgeSide.RightBottom);
                locations.Add(v_2rb);

                var v_2bl = new VertexCH(v2tl.Position[0], v2tl.Position[1] + size, VertexCH.Direction.Down, VertexCH.EdgeSide.BottomLeft);
                locations.Add(v_2bl);

                var v_2lt = new VertexCH(v2rt.Position[0] - size, v2rt.Position[1], VertexCH.Direction.Left, VertexCH.EdgeSide.LeftTop);
                locations.Add(v_2lt);

                //теперь - угловые внешние точки
                if (i < 1)
                {
                    var v111 = new VertexCH(v1rb.Position[0] - size, v1rb.Position[1] - size, VertexCH.Direction.UpLeft, VertexCH.EdgeSide.CornerTopLeft);
                    var v112 = new VertexCH(v1bl.Position[0] + size, v1bl.Position[1] - size, VertexCH.Direction.UpRight, VertexCH.EdgeSide.CornerTopRight);
                    var v121 = new VertexCH(v1lt.Position[0] + size, v1lt.Position[1] + size, VertexCH.Direction.DownRight, VertexCH.EdgeSide.CornerBottomRight);
                    var v122 = new VertexCH(v1tr.Position[0] - size, v1tr.Position[1] + size, VertexCH.Direction.DownLeft, VertexCH.EdgeSide.CornerBottomLeft);
                    if (i != 0 || !bCutOff)
                    {
                        locations.Add(v111);
                        locations.Add(v112);
                        locations.Add(v121);
                        locations.Add(v122);
                    }

                    var v211 = new VertexCH(v2br.Position[0] - size, v2br.Position[1] - size, VertexCH.Direction.UpLeft, VertexCH.EdgeSide.CornerTopLeft);
                    var v212 = new VertexCH(v2lb.Position[0] + size, v2lb.Position[1] - size, VertexCH.Direction.UpRight, VertexCH.EdgeSide.CornerTopRight);
                    var v221 = new VertexCH(v2tl.Position[0] + size, v2tl.Position[1] + size, VertexCH.Direction.DownRight, VertexCH.EdgeSide.CornerBottomRight);
                    var v222 = new VertexCH(v2rt.Position[0] - size, v2rt.Position[1] + size, VertexCH.Direction.DownLeft, VertexCH.EdgeSide.CornerBottomLeft);

                    locations.Add(v211);
                    locations.Add(v212);
                    locations.Add(v221);
                    locations.Add(v222);
                }

                //Наконец, для всех наружных точек укажем, какой внутренней точке на соседнем квадрате они соответствуют с учётом возможных типов соединения
                v_1tl.m_cShadow[VertexCH.Transformation.Stright] = v_1lb;
            }


            //Территорию внутри построенной границы заполним случайными точками с распределением по Поиссону (чтобы случайно, но в общем равномерно).
            float Sbig = size * size;
            var S1 = Sbig / (locationsCount - locations.Count);
            var h = Math.Sqrt(S1 / (4 * Math.Sqrt(3)));
            var R = Math.Sqrt(S1 / (3 * Math.Sqrt(3)));

            R = (R + h) / 2;

            //Придётся сделать несколько попыток, чтобы добиться оптимального заполнения всей территории
            int c = 0;
            List<SimpleVector3d> cPoints = new List<SimpleVector3d>();
            do
            {
                cPoints = UniformPoissonDiskSampler.SampleRectangle(new SimpleVector3d(k, k, 0),
                                                    new SimpleVector3d(k + (size - 2 * k), k + (size - 2 * k), 0),
                                                        (float)R * 2);
                R *= 0.99f;
                c++;
            }
            while (cPoints.Count < locationsCount - locations.Count);

            locationsCount = locations.Count + cPoints.Count;

            //перенесём построенное облако Поиссона в основной рабочий массив
            for (var i = 0; i < cPoints.Count; i++)
            {
//                var vi = new VertexCH(k + (size - 2 * k) * r.NextDouble(), k + (size - 2 * k) * r.NextDouble(), VertexCH.Direction.CenterNone, VertexCH.EdgeSide.Inside);
                var vi = new VertexCH(cPoints[i].X, cPoints[i].Y, VertexCH.Direction.CenterNone, VertexCH.EdgeSide.Inside);
                locations.Add(vi);
            }

            //Наконец, строим диаграмму Вороного.
            VoronoiMesh<VertexCH, CellCH, VoronoiEdge<VertexCH, CellCH>> voronoiMesh = VoronoiMesh.Create<VertexCH, CellCH>(locations);

            //Переведём результат в удобный нам формат.
            //Для каждого найденного ребра диаграммы Вороного найдём локации, которые оно разделяет
            foreach (var edge in voronoiMesh.Edges)
            {
                var from = edge.Source;
                var to = edge.Target;

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

                //пропишем ссылки на ребро в найденных локациях
                pLeft.m_cEdges[pRight] = new VertexCH.Edge(from, to);
                pRight.m_cEdges[pLeft] = new VertexCH.Edge(to, from);
            }

            vertices.Clear();
            vertices.AddRange(voronoiMesh.Vertices);

            m_cFaces[Face3D.Backward] = new CubeFace(locations, vertices, size, Face3D.Backward);
            m_cFaces[Face3D.Bottom] = new CubeFace(locations, vertices, size, Face3D.Bottom);
            m_cFaces[Face3D.Forward] = new CubeFace(locations, vertices, size, Face3D.Forward);
            m_cFaces[Face3D.Left] = new CubeFace(locations, vertices, size, Face3D.Left);
            m_cFaces[Face3D.Right] = new CubeFace(locations, vertices, size, Face3D.Right);
            m_cFaces[Face3D.Top] = new CubeFace(locations, vertices, size, Face3D.Top);
        }
    }
}
