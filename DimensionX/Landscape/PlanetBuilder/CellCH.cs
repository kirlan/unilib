﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using MIConvexHull;

namespace LandscapeGeneration.PlanetBuilder
{
    public class CellCH : TriangulationCell<VertexCH, CellCH>
    {
        static System.Random rnd = new System.Random();

        public object m_pTag = null;

        Point GetCircumcenter()
        {
            // From MathWorld: http://mathworld.wolfram.com/Circumcircle.html

            var points = Vertices;

            double[,] m = new double[3, 3];

            // x, y, 1
            for (int i = 0; i < 3; i++)
            {
                m[i, 0] = points[i].Position[0];
                m[i, 1] = points[i].Position[1];
                m[i, 2] = 1;
            }
            var a = StarMath.determinant(m);

            // size, y, 1
            for (int i = 0; i < 3; i++)
            {
                //                m[i, 0] = StarMath.norm2(points[i].Position, 2, true);
                m[i, 0] = StarMath.norm2(points[i].Position, true);
            }
            var dx = -StarMath.determinant(m);

            // size, x, 1
            for (int i = 0; i < 3; i++)
            {
                m[i, 1] = points[i].Position[0];
            }
            var dy = StarMath.determinant(m);

            // size, x, y
            for (int i = 0; i < 3; i++)
            {
                m[i, 2] = points[i].Position[1];
            }
            var c = -StarMath.determinant(m);

            var s = -1.0 / (2.0 * a);
            var r = System.Math.Abs(s) * System.Math.Sqrt(dx * dx + dy * dy - 4 * a * c);
            return new Point(s * dx, s * dy);
        }

        Point GetCentroid()
        {
            return new Point(Vertices.Select(v => v.Position[0]).Average(), Vertices.Select(v => v.Position[1]).Average());
        }

        Point? circumCenter;
        public Point Circumcenter
        {
            get
            {
                circumCenter = circumCenter ?? GetCircumcenter();
                return circumCenter.Value;
            }
        }

        Point? centroid;
        public Point Centroid
        {
            get
            {
                centroid = centroid ?? GetCentroid();
                return centroid.Value;
            }
        }

        public CellCH()
        {
        }

        public CellCH(CellCH pOrigin1, CellCH pOrigin2)
        {
            circumCenter = new Point((pOrigin1.Circumcenter.X + pOrigin2.Circumcenter.X) / 2, (pOrigin1.Circumcenter.Y + pOrigin2.Circumcenter.Y) / 2);
        }

        public CellCH(CellCH pOrigin1, VertexCH pOrigin2)
        {
            circumCenter = new Point((pOrigin1.Circumcenter.X + pOrigin2.Position[0]) / 2, (pOrigin1.Circumcenter.Y + pOrigin2.Position[1]) / 2);
        }

        public override string ToString()
        {
            return string.Format("[{0}, {1}]", Circumcenter.X, Circumcenter.Y);
        }
    }
}
