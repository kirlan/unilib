using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LandscapeGeneration.PathFind;

namespace LandscapeGeneration.PlanetBuilder
{
    public enum RegionType
    {
        Empty,
        Peak,
        Volcano
    }

    public class Location : TransportationNode, ITerritory
    {
        #region ITerritory Members

        /// <summary>
        /// Границы с другими такими же объектами
        /// </summary>
        public Dictionary<object, List<Edge>> BorderWith
        {
            get { return m_cBorderWith; }
        }

        public object[] m_aBorderWith = null;

        internal void FillBorderWithKeys()
        {
            m_aBorderWith = new List<object>(m_cBorderWith.Keys).ToArray();
        }

        public bool m_bUnclosed = false;

        public bool Forbidden
        {
            get { return m_bUnclosed || m_bBorder; }
        }

        private object m_pOwner = null;

        public object Owner
        {
            get { return m_pOwner; }
            set { m_pOwner = value; }
        }

        private float m_fPerimeter = 0;

        public float PerimeterLength
        {
            get { return m_fPerimeter; }
        }

        #endregion

        public RegionType m_eType = RegionType.Empty;

        public override float GetMovementCost()
        {
            if (m_pOwner == null || Forbidden || m_bBorder)
                return 100;

            ILand pLand = m_pOwner as ILand;
            return pLand.MovementCost * (m_eType == RegionType.Empty ? 1 : 10);
        }

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
        /// Локация расположена за краем карты, здесь нельзя размещать постройки или прокладывать дороги.
        /// </summary>
        public bool m_bBorder = false;

        /// <summary>
        /// Уникальный номер локации. Пока нужен только для отладки, в будущем возможно ещё понадобится для чего-нибудь...
        /// </summary>
        private uint m_iID;

        /// <summary>
        /// Конструктор без параметров, чтобы класс можно было передавать параметром в шаблоны.
        /// После создания ОБЯЗАТЕЛЬНО вызвать Create()!
        /// </summary>
        public Location()
        { 
        }

        public void Create(uint iID, float fX, float fY, float fSize, float fR, CubeFace3D eFace, VertexCH.Direction eGhost, bool bBorder, bool bHighRes)
        {
            Create(fX, fY, fSize, eFace, fR, bHighRes);
            m_iID = iID;
            m_eGhost = eGhost;

            m_bBorder = !bHighRes;
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
            public Vertex m_pPoint1;
            public Vertex m_pPoint2;
            public Vertex m_pMidPoint;
            public Vertex m_pInnerPoint;

            public float m_fLength;

            public Edge m_pNext = null;

            public Edge(Vertex pFrom, Vertex pTo, Vertex pMidPoint, Vertex pInnerPoint)
            {
                m_pPoint1 = pFrom;
                m_pPoint2 = pTo;
                m_pMidPoint = pMidPoint;
                m_pInnerPoint = pInnerPoint;

                m_fLength = (float)Math.Sqrt((pFrom.m_fX - pTo.m_fX) * (pFrom.m_fX - pTo.m_fX) + (pFrom.m_fY - pTo.m_fY) * (pFrom.m_fY - pTo.m_fY) + (pFrom.m_fZ - pTo.m_fZ) * (pFrom.m_fZ - pTo.m_fZ));
            }

            public Edge(Edge pOriginal)
            {
                m_pPoint1 = pOriginal.m_pPoint1;
                m_pPoint2 = pOriginal.m_pPoint2;
                m_pMidPoint = pOriginal.m_pMidPoint;
                m_pInnerPoint = pOriginal.m_pInnerPoint;

                m_fLength = pOriginal.m_fLength;
            }

            public override string ToString()
            {
                return string.Format("{0} - {1}", m_pPoint1, m_pPoint2);
            }
        }

        public Dictionary<object, List<Edge>> m_cBorderWith = new Dictionary<object, List<Edge>>();

        public Edge m_pFirstLine = null;

        /// <summary>
        /// Во всех границах локации заменяет ссылку на "плохую" вершину ссылкой на "хорошую".
        /// Убирает связь с этой локацией у "плохой" вершины.
        /// Добавляет связь с этой локацией "хорошей" вершине
        /// </summary>
        /// <param name="pBad"></param>
        /// <param name="pGood"></param>
        public void ReplaceVertex(Vertex pBad, Vertex pGood)
        {
            foreach (var pEdge in m_cBorderWith)
            {
                bool bGotIt = false;
                if (pEdge.Value[0].m_pPoint1 == pBad)
                {
                    pEdge.Value[0].m_pPoint1 = pGood;
                    bGotIt = true;
                }
                if (pEdge.Value[0].m_pPoint2 == pBad)
                {
                    pEdge.Value[0].m_pPoint2 = pGood;
                    bGotIt = true;
                }
                if (pEdge.Value[0].m_pMidPoint == pBad)
                {
                    pEdge.Value[0].m_pMidPoint = pGood;
                    bGotIt = true;
                }

                if (bGotIt)
                {
                    if (!pGood.m_cLocations.Contains(this))
                        pGood.m_cLocations.Add(this);
                    pBad.m_cLocations.Remove(this);
                }
            }
        }

        /// <summary>
        /// Настраивает связи "следующая"-"предыдущая" среди граней, уже хранящихся в словаре границ с другими локациями.
        /// </summary>
        public void BuildBorder()
        {
            if (Ghost)
                return;

            m_pFirstLine = m_cBorderWith[m_aBorderWith[0]][0];

            List<Edge> cTotalBorder = new List<Edge>();

            m_fPerimeter = 0;
            foreach (var cEdges in m_cBorderWith)
            {
                cTotalBorder.AddRange(cEdges.Value);
                foreach (Edge pEdge in cEdges.Value)
                    m_fPerimeter += pEdge.m_fLength;
            } 
            
            List<Edge> cSequence = new List<Edge>();

            Edge pLast = m_cBorderWith.Values.First()[0];
            cSequence.Add(pLast);
            for (int j = 0; j < m_cBorderWith.Count; j++)
            {
                foreach (var pEdge in m_cBorderWith)
                {
                    if (((Location)pEdge.Key).Ghost)
                        continue;

                    if (pEdge.Value[0].m_pPoint1 == pLast.m_pPoint2 && !cSequence.Contains(pEdge.Value[0]))
                    {
                        pLast = pEdge.Value[0];
                        cSequence.Add(pLast);
                        break;
                    }
                }
            }

            if (cSequence.Count != m_cBorderWith.Count)
            {
                throw new Exception();
            }

            pLast = cSequence.Last();
            foreach (var pEdge in cSequence)
            {
                pLast.m_pNext = pEdge;
                pLast = pEdge;

                //if (pEdge.Value.m_pMidPoint != pEdge.Key.m_cEdges[pLoc].m_pMidPoint)
                //    throw new Exception();
            }
        }

        /// <summary>
        /// Смещает центр локации в реальный геометрический центр многоугольника
        /// </summary>
        public void CorrectCenter()
        {
            if (m_pFirstLine == null)
                m_bUnclosed = true;

            if (m_bUnclosed || m_bBorder)
                return;

            float fX = 0, fY = 0, fZ = 0, fLength = 0;

            Edge pLine = m_pFirstLine;
            do
            {
                fX += pLine.m_fLength * (pLine.m_pPoint1.X + pLine.m_pPoint2.X) / 2;
                fY += pLine.m_fLength * (pLine.m_pPoint1.Y + pLine.m_pPoint2.Y) / 2;
                fZ += pLine.m_fLength * (pLine.m_pPoint1.Z + pLine.m_pPoint2.Z) / 2;
                fLength += pLine.m_fLength;

                pLine = pLine.m_pNext;
            }
            while (pLine != m_pFirstLine);

            X = fX / fLength;
            Y = fY / fLength;
            Z = fZ / fLength;

            List<Edge> cTotalBorder = new List<Edge>();

            foreach (var cLines in m_cBorderWith)
                cTotalBorder.AddRange(cLines.Value);

            Edge[] aTotalBorder = cTotalBorder.ToArray();
            foreach (var pLne in aTotalBorder)
            {
                pLne.m_pMidPoint.X = (pLne.m_pPoint1.X + pLne.m_pPoint2.X) / 2;
                pLne.m_pMidPoint.Y = (pLne.m_pPoint1.Y + pLne.m_pPoint2.Y) / 2;
                pLne.m_pMidPoint.Z = (pLne.m_pPoint1.Z + pLne.m_pPoint2.Z) / 2;

                pLne.m_pInnerPoint.X = (pLne.m_pMidPoint.X + X) / 2;
                pLne.m_pInnerPoint.Y = (pLne.m_pMidPoint.Y + Y) / 2;
                pLne.m_pInnerPoint.Z = (pLne.m_pMidPoint.Z + Z) / 2;
            }
        }

        public virtual void Reset()
        {
            m_cLinks.Clear();
            m_eType = RegionType.Empty;
            m_pOwner = null;
        }
        
        public override string ToString()
        {
            return string.Format("{0}{4} ({1}, {2}, {3}) H:{5}", Ghost ? "x" : "", m_fX, m_fY, m_fZ, m_iID, m_fH);
        }

        public string GetStringID()
        {
            string sID = m_iID.ToString();

            return string.Format("{1}[{0}]", sID, m_bBorder || m_bUnclosed ? "x" : " ");
        }
    }
}
