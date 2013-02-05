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
            RightBottom,
            CornerTopLeft,
            CornerTopRight,
            CornerBottomLeft,
            CornerBottomRight
        }

        public EdgeSide m_eSide;

        public enum Transformation
        {
            Stright,
            Rotate90CW,
            Rotate90CCW,
            Rotate180
        }

        public Dictionary<Transformation, VertexCH> m_cShadow = new Dictionary<Transformation, VertexCH>();

        public enum Direction
        {
            CenterNone,
            Up,
            Down,
            Left,
            Right,
            UpLeft,
            UpRight,
            DownLeft,
            DownRight
        }

        public Direction m_eGhost;

        /// <summary>
        /// Initializes a new instance of the <see cref="Vertex"/> class.
        /// </summary>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        public VertexCH(double x, double y, Direction eGhost, EdgeSide eSide)
        {
            Position = new double[] { x, y };
            m_eGhost = eGhost;
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
