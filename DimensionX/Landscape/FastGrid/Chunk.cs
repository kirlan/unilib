using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace LandscapeGeneration.FastGrid
{
    public class Chunk
    {
        public Dictionary<VertexCH.Direction, Chunk> m_cNeighbours = new Dictionary<VertexCH.Direction, Chunk>();

        public Location[] m_aBorderLocations;
        public Location[] m_aLocations;
        public VoronoiVertex[] m_aVertexes;

        //public bool m_bDirty = true;

        /// <summary>
        /// Выкидывает из списка локаций все "призрачные" локации (которые к этому моменту уже все должны быть "отрезаны" от основной массы).
        /// Обновляет список вершин, чтобы в него гарантированно входили все вершины, связанные с не-"призрачными" локациями.
        /// Смещает центр локации в её центр тяжести.
        /// </summary>
        public void Final(float fCycleShit)
        {
            List<Location> cNewLocations = new List<Location>();

            List<VoronoiVertex> cNewVertexes = new List<VoronoiVertex>();

            for (int i = 0; i < m_aLocations.Length; i++)
            {
                var pLoc = m_aLocations[i];
                if (pLoc.IsShaded)
                    continue;

                cNewLocations.Add(pLoc);

                pLoc.m_pChunkMarker = this;

                foreach (var pEdge in pLoc.BorderWith)
                {
                    if (!pLoc.Forbidden && ((Location)pEdge.Key).IsShaded)
                        throw new Exception("Still shaded!");

                    VoronoiEdge pEdgeValue = pEdge.Value[0];

                    var aLocations1 = pEdgeValue.m_pPoint1.m_cLocations.ToArray();
                    foreach (var pEdgeLoc1 in aLocations1)
                    {
                        if (pEdgeLoc1.IsShaded)
                            pEdgeValue.m_pPoint1.m_cLocations.Remove(pEdgeLoc1);
                    }

                    var aLocations2 = pEdgeValue.m_pPoint2.m_cLocations.ToArray();
                    foreach (var pEdgeLoc2 in aLocations2)
                    {
                        if (pEdgeLoc2.IsShaded)
                            pEdgeValue.m_pPoint2.m_cLocations.Remove(pEdgeLoc2);
                    }

                    if (pEdgeValue.m_pPoint1.m_pChunkMarker != this)
                    {
                        cNewVertexes.Add(pEdgeValue.m_pPoint1);
                        pEdgeValue.m_pPoint1.m_pChunkMarker = this;
                    }
                }

                pLoc.FillBorderWithKeys();
                pLoc.BuildBorder(fCycleShit);
                pLoc.CorrectCenter();
            }

            m_aLocations = cNewLocations.ToArray();
            m_aVertexes = cNewVertexes.ToArray();

            //m_bDirty = true;

            //DebugVertexes();
        }

        public void ReplaceVertexes(VoronoiVertex pBad1, VoronoiVertex pGood1, VoronoiVertex pBad2, VoronoiVertex pGood2)
        {
            if (pBad1 != pGood1)
                pBad1.Replace(pGood1);

            if (pBad2 != pGood2)
                pBad2.Replace(pGood2);
        }

        private Dictionary<uint, Location> m_cLocations = new Dictionary<uint, Location>();

        private float m_fDX, m_fDY;

        public override string ToString()
        {
            return string.Format("({0}, {1})", m_fDX, m_fDY);
        }

        public VoronoiVertex m_pBoundTopLeft;
        public VoronoiVertex m_pBoundTopRight;
        public VoronoiVertex m_pBoundBottomRight;
        public VoronoiVertex m_pBoundBottomLeft;

        public Chunk(ref VertexCH[] locations, Rect pBounds2D, ref CellCH[] vertices, float fDX, float fDY, float fWholeGridSize)
        {
            m_fDX = fDX;
            m_fDY = fDY;

            m_pBoundTopLeft = new VoronoiVertex((float)(pBounds2D.X + fDX - fWholeGridSize / 2), (float)(pBounds2D.Y + pBounds2D.Height + fDY - fWholeGridSize / 2));
            m_pBoundTopRight = new VoronoiVertex((float)(pBounds2D.X + pBounds2D.Width + fDX - fWholeGridSize / 2), (float)(pBounds2D.Y + pBounds2D.Height + fDY - fWholeGridSize / 2));
            m_pBoundBottomRight = new VoronoiVertex((float)(pBounds2D.X + pBounds2D.Width + fDX - fWholeGridSize / 2), (float)(pBounds2D.Y + fDY - fWholeGridSize / 2));
            m_pBoundBottomLeft = new VoronoiVertex((float)(pBounds2D.X + fDX - fWholeGridSize / 2), (float)(pBounds2D.Y + fDY - fWholeGridSize / 2));

            List<Location> cBorders = new List<Location>();

            m_aLocations = new Location[locations.Length];
            for (int i = 0; i < locations.Length; i++)
            {
                var loc = locations[i];
                Location myLocation = new Location();
                myLocation.Create(loc.m_iID, (float)loc.Position[0] + fDX - fWholeGridSize / 2, (float)loc.Position[1] + fDY - fWholeGridSize / 2, loc.m_eShadowDir);
                m_aLocations[i] = myLocation;
                loc.m_pTag = myLocation;
                m_cLocations[loc.m_iID] = myLocation;
                if (myLocation.IsShaded)
                {
                    myLocation.SetShadow(loc.m_pShadow.m_iID);

                    if (loc.Position[0] != loc.m_pShadow.Position[0] &&
                        Math.Abs(loc.Position[0] - loc.m_pShadow.Position[0]) != 20000f)
                        throw new Exception("Wrong shadow coordinates!");

                    if (loc.Position[1] != loc.m_pShadow.Position[1] &&
                        Math.Abs(loc.Position[1] - loc.m_pShadow.Position[1]) != 20000f)
                        throw new Exception("Wrong shadow coordinates!");
                }
                if (loc.m_bBorder && !myLocation.IsShaded)
                    cBorders.Add(myLocation);
            }

            m_aVertexes = new VoronoiVertex[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                var vertex = vertices[i];
                VoronoiVertex myVertex = new VoronoiVertex((float)vertex.Circumcenter.X + fDX - fWholeGridSize / 2, (float)vertex.Circumcenter.Y + fDY - fWholeGridSize / 2);

                vertex.m_pTag = myVertex;
                m_aVertexes[i] = myVertex;
            }

            for (int i = 0; i < locations.Length; i++)
            {
                var loc = locations[i];
                Location myLocation = (Location)loc.m_pTag;
                foreach (var edge in loc.m_cEdges)
                {
                    VoronoiVertex pVertex1 = (VoronoiVertex)edge.Value.m_pFrom.m_pTag;
                    VoronoiVertex pVertex2 = (VoronoiVertex)edge.Value.m_pTo.m_pTag;
                    
                    myLocation.BorderWith[(Location)edge.Key.m_pTag] = new List<VoronoiEdge>();
                    myLocation.BorderWith[(Location)edge.Key.m_pTag].Add(new VoronoiEdge(pVertex1, pVertex2));
                    
                    if (!pVertex1.m_cLocations.Contains(myLocation))
                        pVertex1.m_cLocations.Add(myLocation);
                    if (!pVertex2.m_cLocations.Contains(myLocation))
                        pVertex2.m_cLocations.Add(myLocation);
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
                Dictionary<ITerritory, List<VoronoiEdge>> cEdges = new Dictionary<ITerritory, List<VoronoiEdge>>(pInnerLoc.BorderWith);
                foreach (var pEdge in cEdges)
                {
                    //Нас интересуют только призрачные соседи
                    if (!((Location)pEdge.Key).IsShaded)
                        continue;

                    //раз призрачная - значит лежит за границей квадрата
                    Location pOuterLoc = (Location)pEdge.Key;

                    var pLine = pInnerLoc.BorderWith[pOuterLoc][0];

                    bool bMadeIt = false;

                    VertexCH.Direction eDir = pOuterLoc.m_eShadowDir;
                    if (m_cNeighbours.ContainsKey(eDir))
                    {
                        //находим реальное отражение призрачной локации
                        var pNeighbourChunk = m_cNeighbours[eDir];
                        Location pShadow = pNeighbourChunk.m_cLocations[pOuterLoc.m_iShadow];

                        //найдём среди соседей отражения призрачную локацию, соответствующую граничной
                        foreach (var pShadowEdge in pShadow.BorderWith)
                        {
                            if (((Location)pShadowEdge.Key).IsShaded)
                            {
                                var pShadowLine = pShadow.BorderWith[pShadowEdge.Key][0];

                                VertexCH.Direction eShadowDir = ((Location)pShadowEdge.Key).m_eShadowDir;
                                if (pNeighbourChunk.m_cNeighbours.ContainsKey(eShadowDir))
                                {
                                    var pShadowNeighbourChunk = pNeighbourChunk.m_cNeighbours[eShadowDir];
                                    Location pShadowShadow = pShadowNeighbourChunk.m_cLocations[((Location)pShadowEdge.Key).m_iShadow];

                                    if (pShadowShadow == pInnerLoc)
                                    {
                                        //добавляем внутренней локации границу с отражением - такую же, как с его призраком
                                        if (!pInnerLoc.BorderWith.ContainsKey(pShadow))
                                        {
                                            pInnerLoc.BorderWith[pShadow] = new List<VoronoiEdge>();
                                            pInnerLoc.BorderWith[pShadow].Add(pLine);
                                        }

                                        //добавляем отражению границу с внутренней локацией
                                        if (!pShadow.BorderWith.ContainsKey(pInnerLoc))
                                        {
                                            pShadow.BorderWith[pInnerLoc] = new List<VoronoiEdge>();
                                            pShadow.BorderWith[pInnerLoc].Add(pShadowLine);
                                        }

                                        //убираем у внутренней локации границу с призраком
                                        pInnerLoc.BorderWith.Remove(pOuterLoc);
                                        pShadow.BorderWith.Remove(pShadowEdge.Key);

                                        //m_bDirty = true;
                                        //pNeighbourChunk.m_pChunk.m_bDirty = true;

                                        //сливаем вершины
                                        VoronoiVertex pGood1 = new VoronoiVertex(pLine.m_pPoint1);
                                        VoronoiVertex pGood2 = new VoronoiVertex(pLine.m_pPoint2);

                                        ReplaceVertexes(pLine.m_pPoint1, pGood1, pLine.m_pPoint2, pGood2);
                                        pNeighbourChunk.ReplaceVertexes(pShadowLine.m_pPoint2, pGood1, pShadowLine.m_pPoint1, pGood2);

                                        bMadeIt = true;

                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        pOuterLoc.m_bBorder = true;
                        pOuterLoc.m_eShadowDir = VertexCH.Direction.CenterNone;

                        bMadeIt = true;
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

            //    if (!pInnerLoc.IsShaded)
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
