using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace LandscapeGeneration.PlanetBuilder
{
    public class Chunk<LOC>
        where LOC : Location, new()
    {
        public struct NeighbourInfo
        {
            public Chunk<LOC> m_pChunk;
            public VertexCH.Transformation m_eTransformation;

            public NeighbourInfo(Chunk<LOC> pChunk, VertexCH.Transformation eTransformation)
            {
                m_pChunk = pChunk;
                m_eTransformation = eTransformation;
            }
        }

        public Dictionary<VertexCH.Direction, NeighbourInfo> m_cNeighbours = new Dictionary<VertexCH.Direction, NeighbourInfo>();

        public LOC[] m_aBorderLocations;
        public LOC[] m_aLocations;
        public Vertex[] m_aVertexes;

        //public bool m_bDirty = true;

        /// <summary>
        /// Выкидывает из списка локаций все "призрачные" локации (которые к этому моменту уже все должны быть "отрезаны" от основной массы).
        /// Обновляет список вершин, чтобы в него гарантированно входили все вершины, связанные с не-"призрачными" локациями.
        /// Смещает центр локации в её центр тяжести.
        /// </summary>
        public void Final()
        {
            List<LOC> cNewLocations = new List<LOC>();

            List<Vertex> cNewVertexes = new List<Vertex>();

            for (int i = 0; i < m_aLocations.Length; i++)
            {
                var pLoc = m_aLocations[i];
                if (pLoc.Ghost)
                    continue;

                cNewLocations.Add(pLoc);

                pLoc.m_pChunkMarker = this;

                foreach (var pEdge in pLoc.m_cBorderWith)
                {
                    Location.Edge pEdgeValue = pEdge.Value[0];

                    if (pEdgeValue.m_pPoint1.m_pChunkMarker != this)
                    {
                        cNewVertexes.Add(pEdgeValue.m_pPoint1);
                        pEdgeValue.m_pPoint1.m_pChunkMarker = this;
                    }
                    if (pEdgeValue.m_pMidPoint.m_pChunkMarker != this)
                    {
                        cNewVertexes.Add(pEdgeValue.m_pMidPoint);
                        pEdgeValue.m_pMidPoint.m_pChunkMarker = this;
                    }
                    if (pEdgeValue.m_pInnerPoint.m_pChunkMarker != this)
                    {
                        cNewVertexes.Add(pEdgeValue.m_pInnerPoint);
                        pEdgeValue.m_pInnerPoint.m_pChunkMarker = this;
                    }
                }

                pLoc.FillBorderWithKeys();
                pLoc.BuildBorder();
                pLoc.CorrectCenter();
            }

            m_aLocations = cNewLocations.ToArray();
            m_aVertexes = cNewVertexes.ToArray();

            //m_bDirty = true;

            //DebugVertexes();
        }

        public void NormalizeNormals()
        {
            for (int i = 0; i < m_aLocations.Length; i++)
            {
                var pLoc = m_aLocations[i];
                if (pLoc.Ghost)
                    continue;

                float fLen = (float)Math.Sqrt(pLoc.m_fXN * pLoc.m_fXN + pLoc.m_fYN * pLoc.m_fYN + pLoc.m_fZN * pLoc.m_fZN);
                pLoc.m_fXN = pLoc.m_fXN / fLen;
                pLoc.m_fYN = pLoc.m_fYN / fLen;
                pLoc.m_fZN = pLoc.m_fZN / fLen;
            }
            for (int i = 0; i < m_aVertexes.Length; i++)
            {
                var pVertex = m_aVertexes[i];

                float fLen = (float)Math.Sqrt(pVertex.m_fXN * pVertex.m_fXN + pVertex.m_fYN * pVertex.m_fYN + pVertex.m_fZN * pVertex.m_fZN);
                pVertex.m_fXN = pVertex.m_fXN / fLen;
                pVertex.m_fYN = pVertex.m_fYN / fLen;
                pVertex.m_fZN = pVertex.m_fZN / fLen;
            }
        }

        //public void DebugVertexes()
        //{
        //    for (int i = 0; i < m_aLocations.Length; i++)
        //    {
        //        var pLoc = m_aLocations[i];

        //        foreach (var pEdge in pLoc.m_cEdges)
        //        {
        //            if (!m_aVertexes.Contains(pEdge.Value.m_pFrom))
        //            {
        //                throw new Exception();
        //            }
        //            if (!m_aVertexes.Contains(pEdge.Value.m_pTo))
        //            {
        //                throw new Exception();
        //            }
        //            if (!m_aVertexes.Contains(pEdge.Value.m_pMidPoint))
        //            {
        //                throw new Exception();
        //            }
        //            if (!m_aVertexes.Contains(pEdge.Value.m_pInnerPoint))
        //            {
        //                throw new Exception();
        //            }
        //            if (pEdge.Value.m_pTo != pEdge.Value.m_pNext.m_pFrom)
        //                throw new Exception();
        //        }
        //    }

        //    m_bDirty = false;
        //}

        public void ReplaceVertexes(Vertex pBad1, Vertex pGood1, Vertex pBad2, Vertex pGood2, Vertex pBad3, Vertex pGood3)
        {
            if (pBad1 != pGood1)
                pBad1.Replace(pGood1);

            if (pBad2 != pGood2)
                pBad2.Replace(pGood2);

            if (pBad3 != pGood3)
                pBad3.Replace(pGood3);

            //m_bDirty = true;
        }

        private Dictionary<uint, LOC> m_cLocations = new Dictionary<uint, LOC>();

        public CubeFace3D m_eFace;
        private float m_fDX, m_fDY;
        private float m_fR;

        public override string ToString()
        {
            return string.Format("{0} ({1}, {2})", m_eFace, m_fDX, m_fDY);
        }

        public Vertex m_pBoundTopLeft;
        public Vertex m_pBoundTopRight;
        public Vertex m_pBoundBottomRight;
        public Vertex m_pBoundBottomLeft;

        public Chunk(ref VertexCH[] locations, Rect pBounds2D, ref CellCH[] vertices, float fDX, float fDY, float fWholeChunkSize, int iR, CubeFace3D eFace, bool bHighRes)
        {
            m_eFace = eFace;
            m_fDX = fDX;
            m_fDY = fDY;

            m_fR = iR;

            m_pBoundTopLeft = new Vertex((float)(pBounds2D.X + fDX - fWholeChunkSize / 2), (float)(pBounds2D.Y + pBounds2D.Height + fDY - fWholeChunkSize / 2), fWholeChunkSize / 2, eFace, iR, bHighRes);
            m_pBoundTopRight = new Vertex((float)(pBounds2D.X + pBounds2D.Width + fDX - fWholeChunkSize / 2), (float)(pBounds2D.Y + pBounds2D.Height + fDY - fWholeChunkSize / 2), fWholeChunkSize / 2, eFace, iR, bHighRes);
            m_pBoundBottomRight = new Vertex((float)(pBounds2D.X + pBounds2D.Width + fDX - fWholeChunkSize / 2), (float)(pBounds2D.Y + fDY - fWholeChunkSize / 2), fWholeChunkSize / 2, eFace, iR, bHighRes);
            m_pBoundBottomLeft = new Vertex((float)(pBounds2D.X + fDX - fWholeChunkSize / 2), (float)(pBounds2D.Y + fDY - fWholeChunkSize / 2), fWholeChunkSize / 2, eFace, iR, bHighRes);

            List<LOC> cBorders = new List<LOC>();

            m_aLocations = new LOC[locations.Length];
            for (int i = 0; i < locations.Length; i++)
            {
                var loc = locations[i];
                LOC myLocation = new LOC();
                myLocation.Create(loc.m_iID, (float)loc.Position[0] + fDX - fWholeChunkSize / 2, (float)loc.Position[1] + fDY - fWholeChunkSize / 2, fWholeChunkSize / 2, iR, eFace, loc.m_eGhost, loc.m_bBorder, bHighRes);
                m_aLocations[i] = myLocation;
                loc.m_pTag = myLocation;
                m_cLocations[loc.m_iID] = myLocation;
                if (myLocation.Ghost)
                {
                    myLocation.SetShadows(loc.m_cShadow[VertexCH.Transformation.Stright].m_iID,
                        loc.m_cShadow[VertexCH.Transformation.Rotate90CCW].m_iID,
                        loc.m_cShadow[VertexCH.Transformation.Rotate90CW].m_iID,
                        loc.m_cShadow[VertexCH.Transformation.Rotate180].m_iID);
                }
                if (loc.m_bBorder && !myLocation.Ghost)
                    cBorders.Add(myLocation);
            }

            m_aVertexes = new Vertex[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                var vertex = vertices[i];
                Vertex myVertex = new Vertex((float)vertex.Circumcenter.X + fDX - fWholeChunkSize / 2, (float)vertex.Circumcenter.Y + fDY - fWholeChunkSize / 2, fWholeChunkSize / 2, eFace, iR, bHighRes);

                vertex.m_pTag = myVertex;
                m_aVertexes[i] = myVertex;
            }

            for (int i = 0; i < locations.Length; i++)
            {
                var loc = locations[i];
                LOC myLocation = (LOC)loc.m_pTag;
                foreach (var edge in loc.m_cEdges)
                {
                    Vertex pVertex1 = (Vertex)edge.Value.m_pFrom.m_pTag;
                    Vertex pVertex2 = (Vertex)edge.Value.m_pTo.m_pTag;
                    Vertex pMidPoint = (Vertex)edge.Value.m_pMidPoint.m_pTag;
                    Vertex pInner = (Vertex)edge.Value.m_pInnerPoint.m_pTag;
                    
                    myLocation.m_cBorderWith[(Location)edge.Key.m_pTag] = new List<Location.Edge>();
                    myLocation.m_cBorderWith[(Location)edge.Key.m_pTag].Add(new Location.Edge(pVertex1, pVertex2, pMidPoint, pInner));
                    
                    if (!pVertex1.m_cLocations.Contains(myLocation))
                        pVertex1.m_cLocations.Add(myLocation);
                    if (!pVertex2.m_cLocations.Contains(myLocation))
                        pVertex2.m_cLocations.Add(myLocation);
                    if (!pMidPoint.m_cLocations.Contains(myLocation))
                        pMidPoint.m_cLocations.Add(myLocation);
                    if (!pInner.m_cLocations.Contains(myLocation))
                        pInner.m_cLocations.Add(myLocation);
                }
            }

            m_aBorderLocations = cBorders.ToArray();

            //m_bDirty = true;
        }

        public void Ghostbusters()
        {
            //Перебираем все внутренние локации, лежащие на границах квадрата
            for (int i = 0; i < m_aBorderLocations.Length; i++)
            {
                var pInnerLoc = m_aBorderLocations[i];
                //Перебираем всех соседей граничной локации
                Dictionary<object, List<Location.Edge>> cEdges = new Dictionary<object, List<Location.Edge>>(pInnerLoc.m_cBorderWith);
                foreach (var pEdge in cEdges)
                {
                    //Нас интересуют только призрачные соседи
                    if (!((Location)pEdge.Key).Ghost)
                        continue;

                    //раз призрачная - значит лежит за границей квадрата
                    Location pOuterLoc = (Location)pEdge.Key;

                    var pLine = pInnerLoc.m_cBorderWith[pOuterLoc][0];

                    bool bMadeIt = false;

                    VertexCH.Direction eDir = pOuterLoc.m_eGhost;
                    if (m_cNeighbours.ContainsKey(eDir))
                    {
                        //находим реальное отражение призрачной локации
                        var pNeighbourChunk = m_cNeighbours[eDir];
                        LOC pShadow = pNeighbourChunk.m_pChunk.m_cLocations[pOuterLoc.m_cShadow[pNeighbourChunk.m_eTransformation]];

                        //найдём среди соседей отражения призрачную локацию, соответствующую граничной
                        foreach (var pShadowEdge in pShadow.m_cBorderWith)
                        {
                            if (((Location)pShadowEdge.Key).Ghost)
                            {
                                var pShadowLine = pShadow.m_cBorderWith[pShadowEdge.Key][0];

                                VertexCH.Direction eShadowDir = ((Location)pShadowEdge.Key).m_eGhost;
                                if (pNeighbourChunk.m_pChunk.m_cNeighbours.ContainsKey(eShadowDir))
                                {
                                    var pShadowNeighbourChunk = pNeighbourChunk.m_pChunk.m_cNeighbours[eShadowDir];
                                    LOC pShadowShadow = pShadowNeighbourChunk.m_pChunk.m_cLocations[((Location)pShadowEdge.Key).m_cShadow[pShadowNeighbourChunk.m_eTransformation]];

                                    if (pShadowShadow == pInnerLoc)
                                    {
                                        //добавляем внутренней локации границу с отражением - такую же, как с его призраком
                                        if (!pInnerLoc.m_cBorderWith.ContainsKey(pShadow))
                                        {
                                            pInnerLoc.m_cBorderWith[pShadow] = new List<Location.Edge>();
                                            pInnerLoc.m_cBorderWith[pShadow].Add(pLine);
                                        }

                                        //добавляем отражению границу с внутренней локацией
                                        if (!pShadow.m_cBorderWith.ContainsKey(pInnerLoc))
                                        {
                                            pShadow.m_cBorderWith[pInnerLoc] = new List<Location.Edge>();
                                            pShadow.m_cBorderWith[pInnerLoc].Add(pShadowLine);
                                        }

                                        //убираем у внутренней локации границу с призраком
                                        pInnerLoc.m_cBorderWith.Remove(pOuterLoc);
                                        pShadow.m_cBorderWith.Remove(pShadowEdge.Key);

                                        //m_bDirty = true;
                                        //pNeighbourChunk.m_pChunk.m_bDirty = true;

                                        //сливаем вершины
                                        Vertex pGood1 = new Vertex(pLine.m_pPoint1);
                                        Vertex pGood2 = new Vertex(pLine.m_pPoint2);
                                        Vertex pGood3 = new Vertex(pLine.m_pMidPoint);

                                        ReplaceVertexes(pLine.m_pPoint1, pGood1, pLine.m_pPoint2, pGood2, pLine.m_pMidPoint, pGood3);
                                        pNeighbourChunk.m_pChunk.ReplaceVertexes(pShadowLine.m_pPoint2, pGood1, pShadowLine.m_pPoint1, pGood2, pShadowLine.m_pMidPoint, pGood3);

                                        bMadeIt = true;

                                        break;
                                    }
                                }
                            }
                        }
                    }

                    if (!bMadeIt)
                    {
                        throw new Exception();
                    }
                }

                //foreach (var pEdge in pInnerLoc.m_cEdges)
                //{
                //    if (pEdge.Key.Ghost)
                //        throw new Exception();
                //}
            }

            //for (int i = 0; i < m_aLocations.Length; i++)
            //{
            //    var pInnerLoc = m_aLocations[i];

            //    if (pInnerLoc.Ghost)
            //        continue;

            //    foreach (var pEdge in pInnerLoc.m_cEdges)
            //    {
            //        if (pEdge.Key.Ghost)
            //            throw new Exception();
            //    }
            //}
        }

        internal void BuildNormals()
        {
            for (int i = 0; i < m_aLocations.Length; i++)
            {
                var pLoc = m_aLocations[i];
                if (pLoc.Ghost)
                    continue;

                foreach (var pEdge in pLoc.m_cBorderWith)
                {
                    var pEdgeValue = pEdge.Value[0];

                    float fXN1, fYN1, fZN1;
                    GetNormal(pEdgeValue.m_pInnerPoint, pEdgeValue.m_pMidPoint, pEdgeValue.m_pPoint1, out fXN1, out fYN1, out fZN1);
                    pEdgeValue.m_pInnerPoint.m_fXN += fXN1;
                    pEdgeValue.m_pInnerPoint.m_fYN += fYN1;
                    pEdgeValue.m_pInnerPoint.m_fZN += fZN1;
                    pEdgeValue.m_pMidPoint.m_fXN += fXN1;
                    pEdgeValue.m_pMidPoint.m_fYN += fYN1;
                    pEdgeValue.m_pMidPoint.m_fZN += fZN1;
                    pEdgeValue.m_pPoint1.m_fXN += fXN1;
                    pEdgeValue.m_pPoint1.m_fYN += fYN1;
                    pEdgeValue.m_pPoint1.m_fZN += fZN1;

                    float fXN2, fYN2, fZN2;
                    GetNormal(pEdgeValue.m_pInnerPoint, pEdgeValue.m_pPoint2, pEdgeValue.m_pMidPoint, out fXN2, out fYN2, out fZN2);
                    pEdgeValue.m_pInnerPoint.m_fXN += fXN2;
                    pEdgeValue.m_pInnerPoint.m_fYN += fYN2;
                    pEdgeValue.m_pInnerPoint.m_fZN += fZN2;
                    pEdgeValue.m_pPoint2.m_fXN += fXN2;
                    pEdgeValue.m_pPoint2.m_fYN += fYN2;
                    pEdgeValue.m_pPoint2.m_fZN += fZN2;
                    pEdgeValue.m_pMidPoint.m_fXN += fXN2;
                    pEdgeValue.m_pMidPoint.m_fYN += fYN2;
                    pEdgeValue.m_pMidPoint.m_fZN += fZN2;

                    float fXN3, fYN3, fZN3;
                    GetNormal(pEdgeValue.m_pInnerPoint, pEdgeValue.m_pNext.m_pInnerPoint, pEdgeValue.m_pPoint2, out fXN3, out fYN3, out fZN3);
                    pEdgeValue.m_pInnerPoint.m_fXN += fXN3;
                    pEdgeValue.m_pInnerPoint.m_fYN += fYN3;
                    pEdgeValue.m_pInnerPoint.m_fZN += fZN3;
                    pEdgeValue.m_pNext.m_pInnerPoint.m_fXN += fXN3;
                    pEdgeValue.m_pNext.m_pInnerPoint.m_fYN += fYN3;
                    pEdgeValue.m_pNext.m_pInnerPoint.m_fZN += fZN3;
                    pEdgeValue.m_pPoint2.m_fXN += fXN3;
                    pEdgeValue.m_pPoint2.m_fYN += fYN3;
                    pEdgeValue.m_pPoint2.m_fZN += fZN3;

                    float fXN4, fYN4, fZN4;
                    GetNormal(pEdgeValue.m_pInnerPoint, pLoc, pEdgeValue.m_pNext.m_pInnerPoint, out fXN4, out fYN4, out fZN4);
                    pEdgeValue.m_pInnerPoint.m_fXN += fXN4;
                    pEdgeValue.m_pInnerPoint.m_fYN += fYN4;
                    pEdgeValue.m_pInnerPoint.m_fZN += fZN4;
                    pLoc.m_fXN += fXN4;
                    pLoc.m_fYN += fYN4;
                    pLoc.m_fZN += fZN4;
                    pEdgeValue.m_pNext.m_pInnerPoint.m_fXN += fXN4;
                    pEdgeValue.m_pNext.m_pInnerPoint.m_fYN += fYN4;
                    pEdgeValue.m_pNext.m_pInnerPoint.m_fZN += fZN4;
                }
            }
        }

        private void GetNormal(Vertex vertex1, Vertex vertex2, Vertex vertex3, out float fX, out float fY, out float fZ)
        {
            float vX1 = vertex1.m_fX + vertex1.m_fX * vertex1.m_fH / m_fR;
            float vY1 = vertex1.m_fY + vertex1.m_fY * vertex1.m_fH / m_fR;
            float vZ1 = vertex1.m_fZ + vertex1.m_fZ * vertex1.m_fH / m_fR;

            float vX2 = vertex2.m_fX + vertex2.m_fX * vertex2.m_fH / m_fR;
            float vY2 = vertex2.m_fY + vertex2.m_fY * vertex2.m_fH / m_fR;
            float vZ2 = vertex2.m_fZ + vertex2.m_fZ * vertex2.m_fH / m_fR;

            float vX3 = vertex3.m_fX + vertex3.m_fX * vertex3.m_fH / m_fR;
            float vY3 = vertex3.m_fY + vertex3.m_fY * vertex3.m_fH / m_fR;
            float vZ3 = vertex3.m_fZ + vertex3.m_fZ * vertex3.m_fH / m_fR;

            float side1X = vX1 - vX3;
            float side1Y = vY1 - vY3;
            float side1Z = vZ1 - vZ3;

            float side2X = vX1 - vX2;
            float side2Y = vY1 - vY2;
            float side2Z = vZ1 - vZ2;

            fX = side1Y * side2Z - side1Z * side2Y;
            fY = side1Z * side2X - side1X * side2Z;
            fZ = side1X * side2Y - side1Y * side2X;
        }
    }
}
