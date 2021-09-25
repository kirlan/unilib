using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MIConvexHull;
using System.Windows;
using Random;

namespace SimpleWorld.Geography
{
    public enum LocationType
    {
        Undefined,
        Field,
        Forbidden
    }

    /// <summary>
    /// Локация — это точка на карте, в которой могут происходить какие-либо события. 
    /// Это может быть город, деревня, горный перевал или поляна в лесу... Локации соединяются 
    /// между собой переходами,  определяющими возможность и затраты времени на перемещение 
    /// из одной локации в другую.
    /// </summary>
    public class CSimpleLocation<TERRITORY> : IVertex, ISimpleLocation
        where TERRITORY : class, ITerritory
    {
        public class VoronoiCell : TriangulationCell<CSimpleLocation<TERRITORY>, VoronoiCell>
        {
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

            public VoronoiCell()
            {
            }

            public VoronoiCell(VoronoiCell pOrigin1, VoronoiCell pOrigin2)
            {
                //double fLength = Math.Sqrt((pOrigin1.Circumcenter.X - pOrigin2.Circumcenter.X) * (pOrigin1.Circumcenter.X - pOrigin2.Circumcenter.X) + (pOrigin1.Circumcenter.Y - pOrigin2.Circumcenter.Y) * (pOrigin1.Circumcenter.Y - pOrigin2.Circumcenter.Y));


                double fAngle = Rnd.Get(2 * Math.PI);
                double fRX = Rnd.Get(Math.Abs(pOrigin1.Circumcenter.X - pOrigin2.Circumcenter.X) / 4);
                double fRY = Rnd.Get(Math.Abs(pOrigin1.Circumcenter.Y - pOrigin2.Circumcenter.Y) / 4);

                circumCenter = new Point((pOrigin1.Circumcenter.X + pOrigin2.Circumcenter.X) / 2 + fRX * Math.Cos(fAngle), (pOrigin1.Circumcenter.Y + pOrigin2.Circumcenter.Y) / 2 + fRY * Math.Sin(fAngle));
            }

            public VoronoiCell(VoronoiCell pOrigin1, CSimpleLocation<TERRITORY> pOrigin2)
            {
                circumCenter = new Point((pOrigin1.Circumcenter.X + pOrigin2.Position[0]) / 2, (pOrigin1.Circumcenter.Y + pOrigin2.Position[1]) / 2);
            }

            public override string ToString()
            {
                return string.Format("[{0}, {1}]", Circumcenter.X, Circumcenter.Y);
            }
        }

        public class VoronoiEdge
        {
            public VoronoiCell m_pFrom;
            public VoronoiCell m_pTo;
            public VoronoiCell m_pMidPoint;
            //public Cell m_pInnerPoint;

            public VoronoiEdge(VoronoiCell pFrom, VoronoiCell pTo, VoronoiCell pMidPoint)//, CLocation pInnerPoint)
            {
                m_pFrom = pFrom;
                m_pTo = pTo;
                m_pMidPoint = pMidPoint;
                //m_pInnerPoint = pInnerPoint;
            }

            public override string ToString()
            {
                return string.Format("{0} - {1}", m_pFrom, m_pTo);
            }
        }

        /// <summary>
        /// Gets or sets the coordinates.
        /// </summary>
        /// <value>The coordinates.</value>
        public double[] Position { get; set; }

        public int X
        {
            get { return (int)Position[0]; }
        }

        public int Y
        {
            get { return (int)Position[1]; }
        }

        public Point ToPoint()
        {
            return new Point(Position[0], Position[1]);
        }

        public Dictionary<CSimpleLocation<TERRITORY>, CLink> Links { get; } = new Dictionary<CSimpleLocation<TERRITORY>, CLink>();

        private const int m_iMaxLinks = 8;

        public int MaxLinks
        {
            get { return m_iMaxLinks; }
        }

        public LocationType Type { get; set; } = LocationType.Undefined;

        public TERRITORY Territory { get; set; } = null;
        public int MovementCost { get => Territory == null ? 0 : Territory.MovementCost; }

        public CSimpleLocation(int iX, int iY)
        {
            Position = new double[] { iX, iY };
        }

        /// <summary>
        /// Заполняется в CSimpleWorld::RebuildEdges()
        /// </summary>
        public Dictionary<CSimpleLocation<TERRITORY>, VoronoiEdge> m_cEdges = new Dictionary<CSimpleLocation<TERRITORY>, VoronoiEdge>();

        public override string ToString()
        {
            return string.Format("{2} ({0}, {1}, {3})", Position[0], Position[1],
                Type, Territory.Name);
        }
    }
}
