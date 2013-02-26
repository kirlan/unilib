using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;
using System.Windows;
using System.Drawing;

namespace TestCubePlanet
{
    public class Chunk
    {
        public struct NeighbourInfo
        {
            public Chunk m_pChunk;
            public VertexCH.Transformation m_eTransformation;

            public NeighbourInfo(Chunk pChunk, VertexCH.Transformation eTransformation)
            {
                m_pChunk = pChunk;
                m_eTransformation = eTransformation;
            }
        }

        public Dictionary<VertexCH.Direction, NeighbourInfo> m_cNeighbours = new Dictionary<VertexCH.Direction, NeighbourInfo>();

        public Location[] m_aBorderLocations;
        public Location[] m_aLocations;
        public Vertex[] m_aVertexes;

        //public bool m_bDirty = true;

        /// <summary>
        /// Выкидывает из списка локаций все "призрачные" локации (которые к этому моменту уже все должны быть "отрезаны" от основной массы).
        /// Обновляет список вершин, чтобы в него гарантированно входили все вершины, связанные с не-"призрачными" локациями.
        /// Смещает центр локации в её центр тяжести.
        /// </summary>
        public void Finalize()
        {
            List<Location> cNewLocations = new List<Location>();

            List<Vertex> cNewVertexes = new List<Vertex>();

            for (int i = 0; i < m_aLocations.Length; i++)
            {
                var pLoc = m_aLocations[i];
                if (pLoc.Ghost)
                    continue;

                cNewLocations.Add(pLoc);

                foreach (var pEdge in pLoc.m_cEdges)
                {
                    if (pEdge.Value.m_pFrom.m_pChunkMarker != this)
                    {
                        cNewVertexes.Add(pEdge.Value.m_pFrom);
                        pEdge.Value.m_pFrom.m_pChunkMarker = this;
                    }
                    if (pEdge.Value.m_pMidPoint.m_pChunkMarker != this)
                    {
                        cNewVertexes.Add(pEdge.Value.m_pMidPoint);
                        pEdge.Value.m_pMidPoint.m_pChunkMarker = this;
                    }
                    if (pEdge.Value.m_pInnerPoint.m_pChunkMarker != this)
                    {
                        cNewVertexes.Add(pEdge.Value.m_pInnerPoint);
                        pEdge.Value.m_pInnerPoint.m_pChunkMarker = this;
                    }
                }

                pLoc.Finalize();
            }

            m_aLocations = cNewLocations.ToArray();
            m_aVertexes = cNewVertexes.ToArray();

            //m_bDirty = true;

            //DebugVertexes();
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

        private Dictionary<uint, Location> m_cLocations = new Dictionary<uint, Location>();

        public Cube.Face3D m_eFace;
        private float m_fDX, m_fDY;

        public override string ToString()
        {
            return string.Format("{0} ({1}, {2})", m_eFace, m_fDX, m_fDY);
        }

        public Vertex m_pBoundTopLeft;
        public Vertex m_pBoundTopRight;
        public Vertex m_pBoundBottomRight;
        public Vertex m_pBoundBottomLeft;

        public Chunk(ref VertexCH[] locations, Rect pBounds2D, ref CellCH[] vertices, float fDX, float fDY, float fWholeChunkSize, int iR, Cube.Face3D eFace)
        {
            m_eFace = eFace;
            m_fDX = fDX;
            m_fDY = fDY;

            m_pBoundTopLeft = new Vertex((float)(pBounds2D.X + fDX - fWholeChunkSize/2), (float)(pBounds2D.Y + pBounds2D.Height + fDY - fWholeChunkSize/2), fWholeChunkSize/2, eFace, iR);
            m_pBoundTopRight = new Vertex((float)(pBounds2D.X + pBounds2D.Width + fDX - fWholeChunkSize / 2), (float)(pBounds2D.Y + pBounds2D.Height + fDY - fWholeChunkSize / 2), fWholeChunkSize / 2, eFace, iR);
            m_pBoundBottomRight = new Vertex((float)(pBounds2D.X + pBounds2D.Width + fDX - fWholeChunkSize / 2), (float)(pBounds2D.Y + fDY - fWholeChunkSize / 2), fWholeChunkSize / 2, eFace, iR);
            m_pBoundBottomLeft = new Vertex((float)(pBounds2D.X + fDX - fWholeChunkSize / 2), (float)(pBounds2D.Y + fDY - fWholeChunkSize / 2), fWholeChunkSize / 2, eFace, iR); 

            List<Location> cBorders = new List<Location>();

            m_aLocations = new Location[locations.Length];
            for (int i = 0; i < locations.Length; i++)
            {
                var loc = locations[i];
                Location myLocation = new Location(loc.m_iID, (float)loc.Position[0] + fDX - fWholeChunkSize / 2, (float)loc.Position[1] + fDY - fWholeChunkSize / 2, fWholeChunkSize / 2, iR, eFace, loc.m_eGhost, loc.m_bBorder);
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
                if(loc.m_bBorder && !myLocation.Ghost)
                    cBorders.Add(myLocation);
            }

            m_aVertexes = new Vertex[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                var vertex = vertices[i];
                Vertex myVertex = new Vertex((float)vertex.Circumcenter.X + fDX - fWholeChunkSize / 2, (float)vertex.Circumcenter.Y + fDY - fWholeChunkSize / 2, fWholeChunkSize / 2, eFace, iR);

                vertex.m_pTag = myVertex;
                m_aVertexes[i] = myVertex;
            }

            for (int i = 0; i < locations.Length; i++)
            {
                var loc = locations[i];
                Location myLocation = (Location)loc.m_pTag;
                foreach (var edge in loc.m_cEdges)
                {
                    Vertex pVertex1 = (Vertex)edge.Value.m_pFrom.m_pTag;
                    Vertex pVertex2 = (Vertex)edge.Value.m_pTo.m_pTag;
                    Vertex pMidPoint = (Vertex)edge.Value.m_pMidPoint.m_pTag;
                    Vertex pInner = (Vertex)edge.Value.m_pInnerPoint.m_pTag;
                    myLocation.m_cEdges[(Location)edge.Key.m_pTag] = new Location.Edge(pVertex1, pVertex2, pMidPoint, pInner);
                    if (!pVertex1.m_cLinked.Contains(myLocation))
                        pVertex1.m_cLinked.Add(myLocation);
                    if (!pVertex2.m_cLinked.Contains(myLocation))
                        pVertex2.m_cLinked.Add(myLocation);
                    if (!pMidPoint.m_cLinked.Contains(myLocation))
                        pMidPoint.m_cLinked.Add(myLocation);
                    if (!pInner.m_cLinked.Contains(myLocation))
                        pInner.m_cLinked.Add(myLocation);
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
                Dictionary<Location, Location.Edge> cEdges = new Dictionary<Location, Location.Edge>(pInnerLoc.m_cEdges);
                foreach (var pEdge in cEdges)
                {
                    //Нас интересуют только призрачные соседи
                    if (!pEdge.Key.Ghost)
                        continue;

                    //раз призрачная - значит лежит за границей квадрата
                    var pOuterLoc = pEdge.Key;

                    var pLine = pInnerLoc.m_cEdges[pOuterLoc];

                    bool bMadeIt = false;

                    VertexCH.Direction eDir = pOuterLoc.m_eGhost;
                    if (m_cNeighbours.ContainsKey(eDir))
                    { 
                        //находим реальное отражение призрачной локации
                        var pNeighbourChunk = m_cNeighbours[eDir];
                        Location pShadow = pNeighbourChunk.m_pChunk.m_cLocations[pOuterLoc.m_cShadow[pNeighbourChunk.m_eTransformation]];

                        //найдём среди соседей отражения призрачную локацию, соответствующую граничной
                        foreach (var pShadowEdge in pShadow.m_cEdges)
                        {
                            if (pShadowEdge.Key.Ghost)
                            {
                                var pShadowLine = pShadow.m_cEdges[pShadowEdge.Key];

                                VertexCH.Direction eShadowDir = pShadowEdge.Key.m_eGhost;
                                if (pNeighbourChunk.m_pChunk.m_cNeighbours.ContainsKey(eShadowDir))
                                {
                                    var pShadowNeighbourChunk = pNeighbourChunk.m_pChunk.m_cNeighbours[eShadowDir];
                                    Location pShadowShadow = pShadowNeighbourChunk.m_pChunk.m_cLocations[pShadowEdge.Key.m_cShadow[pShadowNeighbourChunk.m_eTransformation]];

                                    if(pShadowShadow == pInnerLoc)
                                    {
                                        //добавляем внутренней локации границу с отражением - такую же, как с его призраком
                                        if (!pInnerLoc.m_cEdges.ContainsKey(pShadow))
                                            pInnerLoc.m_cEdges[pShadow] = pLine;

                                        //добавляем отражению границу с внутренней локацией
                                        if (!pShadow.m_cEdges.ContainsKey(pInnerLoc))
                                            pShadow.m_cEdges[pInnerLoc] = pShadowLine;

                                        //убираем у внутренней локации границу с призраком
                                        pInnerLoc.m_cEdges.Remove(pOuterLoc);
                                        pShadow.m_cEdges.Remove(pShadowEdge.Key);

                                        //m_bDirty = true;
                                        //pNeighbourChunk.m_pChunk.m_bDirty = true;

                                        //сливаем вершины
                                        Vertex pGood1 = new Vertex(pLine.m_pFrom);
                                        Vertex pGood2 = new Vertex(pLine.m_pTo);
                                        Vertex pGood3 = new Vertex(pLine.m_pMidPoint);

                                        ReplaceVertexes(pLine.m_pFrom, pGood1, pLine.m_pTo, pGood2, pLine.m_pMidPoint, pGood3);
                                        pNeighbourChunk.m_pChunk.ReplaceVertexes(pShadowLine.m_pTo, pGood1, pShadowLine.m_pFrom, pGood2, pShadowLine.m_pMidPoint, pGood3);

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
    }
}
