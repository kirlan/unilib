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
            var locationsCount = 500;// 2500;

            var size = 1000;
//            List<VertexCH> vertices = new List<VertexCH>();
            var r = new System.Random();

            var k = 1.1 * size / Math.Sqrt(locationsCount);
//            var k = size / Math.Sqrt(locationsCount);

            int quanticSize = (int)(size / k);

            bool bCutOff = false;

            float fDelta = 0.4f;//0.25f;

            for (int i = 0; i <= quanticSize/2; i++)
            {
                var x1 = k * 0.75 + i * k + k * (fDelta - fDelta * 2 * r.NextDouble());
                var y1 = -k * 0.5 + k * (fDelta - fDelta * 2 * r.NextDouble());

                var x2 = k * 0.75 + i * k + k * (fDelta - fDelta * 2 * r.NextDouble());
                var y2 = k * 0.5 + k * (fDelta - fDelta * 2 * r.NextDouble());

                if (i < 1)
                {
                    if (i != 0 || !bCutOff)
                    {
                        var v111 = new VertexCH(-x2, -y2, VertexCH.Direction.UpLeft, VertexCH.EdgeSide.CornerTopLeft);
                        locations.Add(v111);
                    }
                    var v112 = new VertexCH(size + x1, y1, VertexCH.Direction.UpRight, VertexCH.EdgeSide.CornerTopRight);
                    locations.Add(v112);
                    if (i != 0 || !bCutOff)
                    {
                        var v121 = new VertexCH(size + x2, size + y2, VertexCH.Direction.DownRight, VertexCH.EdgeSide.CornerBottomRight);
                        locations.Add(v121);
                    }
                    var v122 = new VertexCH(-x1, size - y1, VertexCH.Direction.DownLeft, VertexCH.EdgeSide.CornerBottomLeft);
                    locations.Add(v122);

                    var v211 = new VertexCH(-y2, -x2, VertexCH.Direction.UpLeft, VertexCH.EdgeSide.CornerTopLeft);
                    locations.Add(v211);
                    if (i != 0 || !bCutOff)
                    {
                        var v212 = new VertexCH(size - y1, -x1, VertexCH.Direction.UpRight, VertexCH.EdgeSide.CornerTopRight);
                        locations.Add(v212);
                    }
                    var v221 = new VertexCH(size + y2, size + x2, VertexCH.Direction.DownRight, VertexCH.EdgeSide.CornerBottomRight);
                    locations.Add(v221);
                    if (i != 0 || !bCutOff)
                    {
                        var v222 = new VertexCH(y1, size + x1, VertexCH.Direction.DownLeft, VertexCH.EdgeSide.CornerBottomLeft);
                        locations.Add(v222);
                    }
                }

                //верхняя грань квадрата - левая половина
                var v_1tl = new VertexCH(x1, y1, VertexCH.Direction.Up, VertexCH.EdgeSide.TopLeft);
                locations.Add(v_1tl);
                if (i != 0 || !bCutOff)
                {
                    var v2tl = new VertexCH(x2, y2, VertexCH.Direction.CenterNone, VertexCH.EdgeSide.TopLeft);
                    locations.Add(v2tl);
                }

                //верхняя грань квадрата - правая половина
                if (i != 0 || !bCutOff)
                {
                    var v1tr = new VertexCH(size - x1, -y1, VertexCH.Direction.CenterNone, VertexCH.EdgeSide.TopRight);
                    locations.Add(v1tr);
                }
                var v_2tr = new VertexCH(size - x2, -y2, VertexCH.Direction.Up, VertexCH.EdgeSide.TopRight);
                locations.Add(v_2tr);

                //нижняя грань квадрата - левая половина
                if (i != 0 || !bCutOff)
                {
                    var v1bl = new VertexCH(x1, size + y1, VertexCH.Direction.CenterNone, VertexCH.EdgeSide.BottomLeft);
                    locations.Add(v1bl);
                }
                var v_2bl = new VertexCH(x2, size + y2, VertexCH.Direction.Down, VertexCH.EdgeSide.BottomLeft);
                locations.Add(v_2bl);

                //нижняя грань квадрата - правая половина
                var v_1br = new VertexCH(size - x1, size - y1, VertexCH.Direction.Down, VertexCH.EdgeSide.BottomRight);
                locations.Add(v_1br);
                if (i != 0 || !bCutOff)
                {
                    var v2br = new VertexCH(size - x2, size - y2, VertexCH.Direction.CenterNone, VertexCH.EdgeSide.BottomRight);
                    locations.Add(v2br);
                }

                //левая грань квадрата - верхняя половина
                var v_1lt = new VertexCH(-y1, x1, VertexCH.Direction.CenterNone, VertexCH.EdgeSide.LeftTop);
                locations.Add(v_1lt);
                var v2lt = new VertexCH(-y2, x2, VertexCH.Direction.Left, VertexCH.EdgeSide.LeftTop);
                locations.Add(v2lt);

                //левая грань квадрата - нижняя половина
                var v1lb = new VertexCH(y1, size - x1, VertexCH.Direction.Left, VertexCH.EdgeSide.LeftBottom);
                locations.Add(v1lb);
                var v_2lb = new VertexCH(y2, size - x2, VertexCH.Direction.CenterNone, VertexCH.EdgeSide.LeftBottom);
                locations.Add(v_2lb);

                //правая грань квадрата - верхняя половина
                var v1rt = new VertexCH(size - y1, x1, VertexCH.Direction.Right, VertexCH.EdgeSide.RightTop);
                locations.Add(v1rt);
                var v_2rt = new VertexCH(size - y2, x2, VertexCH.Direction.CenterNone, VertexCH.EdgeSide.RightTop);
                locations.Add(v_2rt);

                //правая грань квадрата - нижняя половина
                var v_1rb = new VertexCH(size + y1, size - x1, VertexCH.Direction.CenterNone, VertexCH.EdgeSide.RightBottom);
                locations.Add(v_1rb);
                var v2rb = new VertexCH(size + y2, size - x2, VertexCH.Direction.Right, VertexCH.EdgeSide.RightBottom);
                locations.Add(v2rb);
            }

            float Sbig = size * size;
            var S1 = Sbig / (locationsCount - locations.Count);
            var h = Math.Sqrt(S1 / (4 * Math.Sqrt(3)));
            var R = Math.Sqrt(S1 / (3 * Math.Sqrt(3)));

            R = (R + h) / 2;

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

            /****** Random Vertices ******/
            //for (var i = locations.Count; i < locationsCount; i++)
            for (var i = 0; i < cPoints.Count; i++)
            {
//                var vi = new VertexCH(k + (size - 2 * k) * r.NextDouble(), k + (size - 2 * k) * r.NextDouble(), VertexCH.Direction.CenterNone, VertexCH.EdgeSide.Inside);
                var vi = new VertexCH(cPoints[i].X, cPoints[i].Y, VertexCH.Direction.CenterNone, VertexCH.EdgeSide.Inside);
                locations.Add(vi);
            }

            VoronoiMesh<VertexCH, CellCH, VoronoiEdge<VertexCH, CellCH>> voronoiMesh = VoronoiMesh.Create<VertexCH, CellCH>(locations);

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
