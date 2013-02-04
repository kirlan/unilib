using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MIConvexHull;
using System.Windows;

namespace TestCubePlanet
{
    public class Cube
    {
        public enum TCubeFaces
        {
            Top,
            Bottom,
            Left,
            Right,
            Forward,
            Backward
        };

        public Dictionary<TCubeFaces, CubeFace> m_cFaces = new Dictionary<TCubeFaces, CubeFace>();

        static int IsLeft(Point a, Point b, Point c)
        {
            return ((b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X)) > 0 ? 1 : -1;
        }

        public List<VertexCH> locations = new List<VertexCH>();
        public List<CellCH> vertices = new List<CellCH>();

        public Cube()
        {
            var locationsCount = 2500;

            var size = 1000;
//            List<VertexCH> vertices = new List<VertexCH>();
            var r = new Random();

            var k = size / Math.Sqrt(locationsCount);

            int quanticSize = (int)(size / k);

            for (int i = 0; i <= quanticSize/2; i++)
            {
                var x1 = i * k + k * (0.25 - 0.5 * r.NextDouble());
                var y1 = -k * 0.5 + k * (0.25 - 0.5 * r.NextDouble());

                var x2 = i * k + k * (0.25 - 0.5 * r.NextDouble());
                var y2 = k * 0.5 + k * (0.25 - 0.5 * r.NextDouble());

                //верхняя грань квадрата - левая половина
                var v1 = new VertexCH(x1, y1, true, VertexCH.EdgeSide.TopLeft);
                locations.Add(v1);
                var v2 = new VertexCH(x2, y2, false, VertexCH.EdgeSide.TopLeft);
                locations.Add(v2);

                //верхняя грань квадрата - правая половина
                v1 = new VertexCH(size - x1, -y1, false, VertexCH.EdgeSide.TopRight);
                locations.Add(v1);
                v2 = new VertexCH(size - x2, -y2, true, VertexCH.EdgeSide.TopRight);
                locations.Add(v2);

                //нижняя грань квадрата - левая половина
                v1 = new VertexCH(x1, size + y1, false, VertexCH.EdgeSide.BottomLeft);
                locations.Add(v1);
                v2 = new VertexCH(x2, size + y2, true, VertexCH.EdgeSide.BottomLeft);
                locations.Add(v2);

                //нижняя грань квадрата - правая половина
                v1 = new VertexCH(size - x1, size - y1, true, VertexCH.EdgeSide.BottomRight);
                locations.Add(v1);
                v2 = new VertexCH(size - x2, size - y2, false, VertexCH.EdgeSide.BottomRight);
                locations.Add(v2);

                //левая грань квадрата - верхняя половина
                v1 = new VertexCH(y1, x1, true, VertexCH.EdgeSide.LeftTop);
                locations.Add(v1);
                v2 = new VertexCH(y2, x2, false, VertexCH.EdgeSide.LeftTop);
                locations.Add(v2);

                //левая грань квадрата - нижняя половина
                v1 = new VertexCH(-y1, size - x1, false, VertexCH.EdgeSide.LeftBottom);
                locations.Add(v1);
                v2 = new VertexCH(-y2, size - x2, true, VertexCH.EdgeSide.LeftBottom);
                locations.Add(v2);

                //правая грань квадрата - верхняя половина
                v1 = new VertexCH(size + y1, x1, false, VertexCH.EdgeSide.RightTop);
                locations.Add(v1);
                v2 = new VertexCH(size + y2, x2, true, VertexCH.EdgeSide.RightTop);
                locations.Add(v2);

                //правая грань квадрата - нижняя половина
                v1 = new VertexCH(size - y1, size - x1, true, VertexCH.EdgeSide.RightBottom);
                locations.Add(v1);
                v2 = new VertexCH(size - y2, size - x2, false, VertexCH.EdgeSide.RightBottom);
                locations.Add(v2);
            }

            /****** Random Vertices ******/
            for (var i = locations.Count; i < locationsCount; i++)
            {
                var vi = new VertexCH(k + (size - 2 * k) * r.NextDouble(), k + (size - 2 * k) * r.NextDouble(), false, VertexCH.EdgeSide.Inside);
                locations.Add(vi);
            }

            VoronoiMesh<VertexCH, CellCH, VoronoiEdge<VertexCH, CellCH>> voronoiMesh = VoronoiMesh.Create<VertexCH, CellCH>(locations);

            foreach (var edge in voronoiMesh.Edges)
            {
                var from = edge.Source;
                var to = edge.Target;

                VertexCH pLeft = null;
                VertexCH pRight = null;

                foreach (var n in from.Vertices)
                {
                    foreach (var nnn in to.Vertices)
                    {
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

                if (IsLeft(pLeft.ToPoint(), from.Circumcenter, to.Circumcenter) < 0)
                {
                    VertexCH pSwap = pLeft;
                    pLeft = pRight;
                    pRight = pSwap;
                }

                pLeft.m_cEdges[pRight] = new VertexCH.Edge(from, to);
                pRight.m_cEdges[pLeft] = new VertexCH.Edge(to, from);
            }

            vertices.Clear();
            vertices.AddRange(voronoiMesh.Vertices);

            m_cFaces[TCubeFaces.Backward] = new CubeFace(locations, vertices);
            m_cFaces[TCubeFaces.Bottom] = new CubeFace(locations, vertices);
            m_cFaces[TCubeFaces.Forward] = new CubeFace(locations, vertices);
            m_cFaces[TCubeFaces.Left] = new CubeFace(locations, vertices);
            m_cFaces[TCubeFaces.Right] = new CubeFace(locations, vertices);
            m_cFaces[TCubeFaces.Top] = new CubeFace(locations, vertices);
        }
    }
}
