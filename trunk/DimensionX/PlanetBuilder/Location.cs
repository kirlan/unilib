using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlanetBuilder;

namespace TestCubePlanet
{
    public class Location: Vertex
    {
        public bool m_bForest = false;

        /// <summary>
        /// Для "призрачной" локации - направление, в котором следует искать "настоящую" локацию.
        /// Если локация не "призрачная", то CenterNone
        /// </summary>
        public VertexCH.Direction m_eGhost;

        /// <summary>
        /// Является ли локация "призрачной", т.е. отражением какой-то локации, принадлежащей на самом деле соседнему квадрату
        /// </summary>
        public bool Ghost
        {
            get { return m_eGhost != VertexCH.Direction.CenterNone; }
        }

        /// <summary>
        /// Уникальный номер локации. Пока нужен только для отладки, в будущем возможно ещё понадобится для чего-нибудь...
        /// </summary>
        private uint m_iID;

        public Location(uint iID, float fX, float fY, float fSize, float fR, Cube.Face3D eFace, VertexCH.Direction eGhost, bool bBorder, bool bHighRes)
            : base(fX, fY, fSize, eFace, fR, bHighRes)
        {
            m_iID = iID;
            m_eGhost = eGhost;
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
            public Vertex m_pMidPoint;
            public Vertex m_pInnerPoint;

            public Edge m_pNext = null;

            public Edge(Vertex pFrom, Vertex pTo, Vertex pMidPoint, Vertex pInnerPoint)
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
                if (pEdge.Value.m_pMidPoint == pBad)
                {
                    pEdge.Value.m_pMidPoint = pGood;
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

        public void Finalize()
        {
            if (Ghost)
                return;

            List<Edge> cSequence = new List<Edge>();

            Edge pLast = m_cEdges.Values.First();
            cSequence.Add(pLast);
            for (int j = 0; j < m_cEdges.Count; j++)
            {
                foreach (var pEdge in m_cEdges)
                {
                    if (pEdge.Key.Ghost)
                        continue;

                    if (pEdge.Value.m_pFrom == pLast.m_pTo && !cSequence.Contains(pEdge.Value))
                    {
                        pLast = pEdge.Value;
                        cSequence.Add(pLast);
                        break;
                    }
                }
            }

            if (cSequence.Count != m_cEdges.Count)
            {
                throw new Exception();
            }

            pLast = cSequence.Last();
            foreach (var pEdge in cSequence)
            {
                pLast.m_pNext = pEdge;
                pLast = pEdge;

                m_fX += pEdge.m_pFrom.m_fX;
                m_fY += pEdge.m_pFrom.m_fY;
                m_fZ += pEdge.m_pFrom.m_fZ;

                //if (pEdge.Value.m_pMidPoint != pEdge.Key.m_cEdges[pLoc].m_pMidPoint)
                //    throw new Exception();
            }

            m_fX /= m_cEdges.Count + 1;
            m_fY /= m_cEdges.Count + 1;
            m_fZ /= m_cEdges.Count + 1;

            foreach (var pEdge in m_cEdges)
            {
                pEdge.Value.m_pInnerPoint.m_fX = (m_fX + pEdge.Value.m_pMidPoint.m_fX) / 2;
                pEdge.Value.m_pInnerPoint.m_fY = (m_fY + pEdge.Value.m_pMidPoint.m_fY) / 2;
                pEdge.Value.m_pInnerPoint.m_fZ = (m_fZ + pEdge.Value.m_pMidPoint.m_fZ) / 2;

                if (m_fH > 0 && pEdge.Key.m_fH <= 0)
                {
                    pEdge.Value.m_pFrom.m_fH = 0;
                    pEdge.Value.m_pMidPoint.m_fH = 0;
                }
                if (m_fH <= 0 && pEdge.Key.m_fH <= 0)
                {
                    //if (pEdge.Value.m_pFrom.m_fH > 0)
                    //    pEdge.Value.m_pFrom.m_fH = (m_fH + pEdge.Key.m_fH)/2;
                    if (pEdge.Value.m_pMidPoint.m_fH > 0)
                        pEdge.Value.m_pMidPoint.m_fH = (m_fH + pEdge.Key.m_fH) / 2;
                }
            }

            if (m_fH > 0.01 && m_fH < 1)
            {
                float fDensity = (float)ClassicNoise.noise(m_fX / 8, m_fY / 8, m_fZ / 8);
                if (fDensity > 0)
                    m_bForest = true;
            }
        }

        public override string ToString()
        {
            return string.Format("{0}{4} ({1}, {2}, {3})", Ghost ? "x" : "", m_fX, m_fY, m_fZ, m_iID);
        }
    }
}
