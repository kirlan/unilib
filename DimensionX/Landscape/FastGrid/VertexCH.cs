﻿using System;
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
            m_iID = pOrigin.m_iID;

            Position = new double[] { pOrigin.Position[0], pOrigin.Position[1] };
            m_eShadowDir = pOrigin.m_eShadowDir;
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

        public VertexCH m_pShadow = null;

        public void SetShadows(VertexCH stright)
        {
            m_pShadow = stright;
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

        public Direction m_eShadowDir;

        public object m_pTag = null;

        public bool m_bBorder = false;

        public static uint ID_counter = 0;
        public uint m_iID = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="VoronoiVertex"/> class.
        /// </summary>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        public VertexCH(double x, double y, Direction eShadowDir, EdgeSide eSide, bool bBorder)
        {
            m_iID = ID_counter++;

            Position = new double[] { x, y };
            m_eShadowDir = eShadowDir;
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

            public Edge(CellCH pFrom, CellCH pTo)
            {
                m_pFrom = pFrom;
                m_pTo = pTo;
            }

            public override string ToString()
            {
                return string.Format("{0} - {1}", m_pFrom, m_pTo);
            }
        }

        /// <summary>
        /// Заполняется в Cube::RebuildEdges()
        /// </summary>
        public Dictionary<VertexCH, Edge> m_cEdges = new Dictionary<VertexCH, Edge>();

        public override string ToString()
        {
            return string.Format("{0}{4} ({1}, {2}, {3})", m_eShadowDir != Direction.CenterNone ? "x" : "", Position[0], Position[1], m_bBorder, m_iID);
        }
    }
}