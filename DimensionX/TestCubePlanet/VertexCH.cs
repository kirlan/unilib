using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using MIConvexHull;
using System.Windows.Media;
using System.Windows;

namespace TestCubePlanet
{
    public class VertexCH: IVertex
    {
        public VertexCH()
        {
        }

        public bool m_bGhost;

        public enum EdgeSide
        {
            Inside,
            TopLeft,
            TopRight,
            BottomLeft, 
            BottomRight,
            LeftTop,
            LeftBottom,
            RightTop,
            RightBottom
        }

        public EdgeSide m_eSide;

        /// <summary>
        /// Initializes a new instance of the <see cref="Vertex"/> class.
        /// </summary>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        public VertexCH(double x, double y, bool bGhost, EdgeSide eSide)
        {
            Position = new double[] { x, y };
            m_bGhost = bGhost;
            m_eSide = eSide;
        }

        public Point ToPoint()
        {
            return new Point(Position[0], Position[1]);
        }

        /// <summary>
        /// Gets or sets the Z. Not used by MIConvexHull2D.
        /// </summary>
        /// <value>The Z position.</value>
        // private double Z { get; set; }

        /// <summary>
        /// Gets or sets the coordinates.
        /// </summary>
        /// <value>The coordinates.</value>
        public double[] Position { get; set; }

        public class Edge
        {
            public CellCH m_pFrom;
            public CellCH m_pTo;

            public Edge(CellCH pFrom, CellCH pTo)
            {
                m_pFrom = pFrom;
                m_pTo = pTo;
            }
        }

        public Dictionary<VertexCH, Edge> m_cEdges = new Dictionary<VertexCH,Edge>();
    }
}
