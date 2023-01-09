using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using MIConvexHull;

namespace LandscapeGeneration.FastGrid
{
    public class VertexCH : IVertex
    {
        public VertexCH()
        {
        }

        public VertexCH(VertexCH pOrigin)
        {
            ID = pOrigin.ID;

            Position = new double[] { pOrigin.Position[0], pOrigin.Position[1] };
            m_eShadowDir = pOrigin.m_eShadowDir;
            m_eSide = pOrigin.m_eSide;

            IsBorder = pOrigin.IsBorder;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VoronoiVertex"/> class.
        /// </summary>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        public VertexCH(double x, double y, Direction eShadowDir, EdgeSide eSide, bool bBorder)
        {
            ID = ID_counter++;

            Position = new double[] { x, y };
            m_eShadowDir = eShadowDir;
            m_eSide = eSide;

            IsBorder = bBorder;
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

        private readonly EdgeSide m_eSide;

        public VertexCH Shadow { get; private set; } = null;

        public void SetShadows(VertexCH stright)
        {
            Shadow = stright;

            if (Position[0] != stright.Position[0] &&
                Math.Abs(Position[0] - stright.Position[0]) != 20000f)
            {
                throw new InvalidOperationException("Wrong shadow coordinates!");
            }

            if (Position[1] != stright.Position[1] &&
                Math.Abs(Position[1] - stright.Position[1]) != 20000f)
            {
                throw new InvalidOperationException("Wrong shadow coordinates!");
            }
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

        public Direction m_eShadowDir { get; }

        public object Tag { get; set; } = null;

        public bool IsBorder { get; } = false;

        private static uint ID_counter = 0;
        public uint ID { get; } = 0;

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
            public CellCH From { get; set; }
            public CellCH To { get; set; }

            public Edge(CellCH pFrom, CellCH pTo)
            {
                From = pFrom;
                To = pTo;
            }

            public override string ToString()
            {
                return string.Format("{0} - {1}", From, To);
            }
        }

        /// <summary>
        /// Заполняется в Cube::RebuildEdges()
        /// </summary>
        public Dictionary<VertexCH, Edge> Edges { get; } = new Dictionary<VertexCH, Edge>();

        public override string ToString()
        {
            return string.Format("{0}{4} ({1}, {2}, {3})", m_eShadowDir != Direction.CenterNone ? "x" : "", Position[0], Position[1], IsBorder, ID);
        }
    }
}
