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

        public VertexCH(VertexCH pOrigin)
        {
            m_iID = pOrigin.m_iID;

            Position = new double[] { pOrigin.Position[0], pOrigin.Position[1] };
            m_eGhost = pOrigin.m_eGhost;
            m_eSide = pOrigin.m_eSide;

            m_bBorder = pOrigin.m_bBorder;
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

        public void SetShadows(VertexCH stright, VertexCH r90ccw, VertexCH r90cw, VertexCH r180)
        {
            m_cShadow[VertexCH.Transformation.Stright] = stright;
            m_cShadow[VertexCH.Transformation.Rotate90CCW] = r90ccw;
            m_cShadow[VertexCH.Transformation.Rotate90CW] = r90cw;
            m_cShadow[VertexCH.Transformation.Rotate180] = r180;
        }

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

        public object m_pTag = null;

        public bool m_bBorder = false;

        public static uint ID_counter = 0;
        public uint m_iID = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="Vertex"/> class.
        /// </summary>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        public VertexCH(double x, double y, Direction eGhost, EdgeSide eSide, bool bBorder)
        {
            m_iID = ID_counter++;

            Position = new double[] { x, y };
            m_eGhost = eGhost;
            m_eSide = eSide;

            m_bBorder = bBorder;
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
            public CellCH m_pMidPoint;
            public CellCH m_pInnerPoint;

            public Edge(CellCH pFrom, CellCH pTo, CellCH pMidPoint, CellCH pInnerPoint)
            {
                m_pFrom = pFrom;
                m_pTo = pTo;
                m_pMidPoint = pMidPoint;
                m_pInnerPoint = pInnerPoint;
            }

            public override string ToString()
            {
                return string.Format("{0} - {1}", m_pFrom, m_pTo);
            }
        }

        /// <summary>
        /// Заполняется в Cube::RebuildEdges()
        /// </summary>
        public Dictionary<VertexCH, Edge> m_cEdges = new Dictionary<VertexCH,Edge>();

        public override string ToString()
        {
            return string.Format("{0}{4} ({1}, {2}, {3})", m_eGhost != Direction.CenterNone ? "x" : "", Position[0], Position[1], m_bBorder, m_iID);
        }
    }
}
