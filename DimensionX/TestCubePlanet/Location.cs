using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestCubePlanet
{
    public class Location: Vertex
    {
        public VertexCH.Direction m_eGhost;

        public bool Ghost
        {
            get { return m_eGhost != VertexCH.Direction.CenterNone; }
        }

        private uint m_iID;

        public bool m_bBorder = false;

        public Location(uint iID, float fX, float fY, float fR, Cube.Face3D eFace, VertexCH.Direction eGhost, bool bBorder)
            : base(fX, fY, fR, eFace)
        {
            m_iID = iID;
            m_eGhost = eGhost;

            m_bBorder = bBorder;
        }

        public Dictionary<VertexCH.Transformation, uint> m_cShadow = new Dictionary<VertexCH.Transformation, uint>();

        public void SetShadows(uint stright, uint r90ccw, uint r90cw, uint r180)
        {
            m_cShadow[VertexCH.Transformation.Stright] = stright;
            m_cShadow[VertexCH.Transformation.Rotate90CCW] = r90ccw;
            m_cShadow[VertexCH.Transformation.Rotate90CW] = r90cw;
            m_cShadow[VertexCH.Transformation.Rotate180] = r180;
        }

        public class Edge
        {
            public Vertex m_pFrom;
            public Vertex m_pTo;

            public Edge(Vertex pFrom, Vertex pTo)
            {
                m_pFrom = pFrom;
                m_pTo = pTo;
            }

            public override string ToString()
            {
                return string.Format("{0} - {1}", m_pFrom, m_pTo);
            }

            public Edge Reverse()
            {
                return new Edge(m_pTo, m_pFrom);
            }
        }

        public Dictionary<Location, Edge> m_cEdges = new Dictionary<Location, Edge>();

        /// <summary>
        /// Во всех границах локации заменяет ссылку на "плохую" вершину ссылкой на "хорошую".
        /// Убирает связь с этой локацией у "плохой" вершины.
        /// Добавляет связь с этой локацией "хорошей" вершине
        /// </summary>
        /// <param name="pBad"></param>
        /// <param name="pGood"></param>
        public void ReplaceVertex(Vertex pBad, Vertex pGood)
        {
            if (pGood.m_bForbidden)
                throw new Exception();

            foreach (var pEdge in m_cEdges)
            {
                bool bGotIt = false;
                if (pEdge.Value.m_pFrom == pBad)
                {
                    pEdge.Value.m_pFrom = pGood;
                    bGotIt = true;
                }
                if (pEdge.Value.m_pTo == pBad)
                {
                    pEdge.Value.m_pTo = pGood;
                    bGotIt = true;
                }

                if (bGotIt)
                {
                    if (!pGood.m_cLinked.Contains(this))
                        pGood.m_cLinked.Add(this);
                    pBad.m_cLinked.Remove(this);
                }
            }
        }

        public override string ToString()
        {
            return string.Format("{0}{4} ({1}, {2}, {3})", Ghost ? "x":"", m_fX, m_fY, m_fZ, m_iID);
        }
    }
}
