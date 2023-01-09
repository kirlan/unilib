using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace LandscapeGeneration.FastGrid
{
    public class Chunk
    {
        public Dictionary<VertexCH.Direction, Chunk> Neighbours { get; } = new Dictionary<VertexCH.Direction, Chunk>();

        private readonly Location[] m_aBorderLocations;
        public Location[] Locations { get; private set; }
        public VoronoiVertex[] Vertexes { get; private set; }

        /// <summary>
        /// Выкидывает из списка локаций все "призрачные" локации (которые к этому моменту уже все должны быть "отрезаны" от основной массы).
        /// Обновляет список вершин, чтобы в него гарантированно входили все вершины, связанные с не-"призрачными" локациями.
        /// Смещает центр локации в её центр тяжести.
        /// </summary>
        public void Final(float fCycleShit)
        {
            List<Location> cNewLocations = new List<Location>();

            List<VoronoiVertex> cNewVertexes = new List<VoronoiVertex>();

            for (int i = 0; i < Locations.Length; i++)
            {
                var pLoc = Locations[i];
                if (pLoc.IsShaded)
                    continue;

                cNewLocations.Add(pLoc);

                pLoc.ChunkMarker = this;

                foreach (var pEdge in pLoc.BorderWith)
                {
                    if (!pLoc.Forbidden && pEdge.Key.IsShaded)
                        throw new InvalidOperationException("Still shaded!");

                    VoronoiEdge pEdgeValue = pEdge.Value[0];

                    var aLocations1 = pEdgeValue.Point1.Locations.ToArray();
                    foreach (var pEdgeLoc1 in aLocations1)
                    {
                        if (pEdgeLoc1.IsShaded)
                            pEdgeValue.Point1.Locations.Remove(pEdgeLoc1);
                    }

                    var aLocations2 = pEdgeValue.Point2.Locations.ToArray();
                    foreach (var pEdgeLoc2 in aLocations2)
                    {
                        if (pEdgeLoc2.IsShaded)
                            pEdgeValue.Point2.Locations.Remove(pEdgeLoc2);
                    }

                    if (pEdgeValue.Point1.ChunkMarker != this)
                    {
                        cNewVertexes.Add(pEdgeValue.Point1);
                        pEdgeValue.Point1.ChunkMarker = this;
                    }
                }

                pLoc.FillBorderWithKeys();
                pLoc.BuildBorder(fCycleShit);
                pLoc.CorrectCenter();
            }

            Locations = cNewLocations.ToArray();
            Vertexes = cNewVertexes.ToArray();
        }

        public void ReplaceVertexes(VoronoiVertex pBad1, VoronoiVertex pGood1, VoronoiVertex pBad2, VoronoiVertex pGood2)
        {
            if (pBad1 != pGood1)
                pBad1.Replace(pGood1);

            if (pBad2 != pGood2)
                pBad2.Replace(pGood2);
        }

        private readonly Dictionary<uint, Location> m_cLocations = new Dictionary<uint, Location>();
        private readonly float m_fDX;
        private readonly float m_fDY;

        public override string ToString()
        {
            return string.Format("({0}, {1})", m_fDX, m_fDY);
        }

        private readonly VoronoiVertex m_pBoundTopLeft;
        private readonly VoronoiVertex m_pBoundTopRight;
        private readonly VoronoiVertex m_pBoundBottomRight;
        private readonly VoronoiVertex m_pBoundBottomLeft;

        public Chunk(ref VertexCH[] locations, Rect pBounds2D, ref CellCH[] vertices, float fDX, float fDY, float fWholeGridSize)
        {
            m_fDX = fDX;
            m_fDY = fDY;

            m_pBoundTopLeft = new VoronoiVertex((float)(pBounds2D.X + fDX - fWholeGridSize / 2), (float)(pBounds2D.Y + pBounds2D.Height + fDY - fWholeGridSize / 2));
            m_pBoundTopRight = new VoronoiVertex((float)(pBounds2D.X + pBounds2D.Width + fDX - fWholeGridSize / 2), (float)(pBounds2D.Y + pBounds2D.Height + fDY - fWholeGridSize / 2));
            m_pBoundBottomRight = new VoronoiVertex((float)(pBounds2D.X + pBounds2D.Width + fDX - fWholeGridSize / 2), (float)(pBounds2D.Y + fDY - fWholeGridSize / 2));
            m_pBoundBottomLeft = new VoronoiVertex((float)(pBounds2D.X + fDX - fWholeGridSize / 2), (float)(pBounds2D.Y + fDY - fWholeGridSize / 2));

            List<Location> cBorders = new List<Location>();

            Locations = new Location[locations.Length];
            for (int i = 0; i < locations.Length; i++)
            {
                var loc = locations[i];
                Location myLocation = new Location();
                myLocation.Create(loc.ID, (float)loc.Position[0] + fDX - fWholeGridSize / 2, (float)loc.Position[1] + fDY - fWholeGridSize / 2, loc.m_eShadowDir);
                Locations[i] = myLocation;
                loc.Tag = myLocation;
                m_cLocations[loc.ID] = myLocation;
                if (myLocation.IsShaded)
                {
                    myLocation.SetShadow(loc.Shadow.ID);

                    if (loc.Position[0] != loc.Shadow.Position[0] &&
                        Math.Abs(loc.Position[0] - loc.Shadow.Position[0]) != 20000f)
                    {
                        throw new InvalidOperationException("Wrong shadow coordinates!");
                    }

                    if (loc.Position[1] != loc.Shadow.Position[1] &&
                        Math.Abs(loc.Position[1] - loc.Shadow.Position[1]) != 20000f)
                    {
                        throw new InvalidOperationException("Wrong shadow coordinates!");
                    }
                }
                if (loc.IsBorder && !myLocation.IsShaded)
                    cBorders.Add(myLocation);
            }

            Vertexes = new VoronoiVertex[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                var vertex = vertices[i];
                VoronoiVertex myVertex = new VoronoiVertex((float)vertex.Circumcenter.X + fDX - fWholeGridSize / 2, (float)vertex.Circumcenter.Y + fDY - fWholeGridSize / 2);

                vertex.Tag = myVertex;
                Vertexes[i] = myVertex;
            }

            for (int i = 0; i < locations.Length; i++)
            {
                var loc = locations[i];
                Location myLocation = (Location)loc.Tag;
                foreach (var edge in loc.Edges)
                {
                    VoronoiVertex pVertex1 = (VoronoiVertex)edge.Value.From.Tag;
                    VoronoiVertex pVertex2 = (VoronoiVertex)edge.Value.To.Tag;

                    myLocation.BorderWith[(Location)edge.Key.Tag] = new List<VoronoiEdge>();
                    myLocation.BorderWith[(Location)edge.Key.Tag].Add(new VoronoiEdge(pVertex1, pVertex2));

                    if (!pVertex1.Locations.Contains(myLocation))
                        pVertex1.Locations.Add(myLocation);
                    if (!pVertex2.Locations.Contains(myLocation))
                        pVertex2.Locations.Add(myLocation);
                }
            }

            m_aBorderLocations = cBorders.ToArray();
        }

        public void Ghostbusters()
        {
            //Перебираем все внутренние локации, лежащие на границах квадрата
            for (int i = 0; i < m_aBorderLocations.Length; i++)
            {
                var pInnerLoc = m_aBorderLocations[i];
                //Перебираем всех соседей граничной локации
                Dictionary<Location, List<VoronoiEdge>> cEdges = new Dictionary<Location, List<VoronoiEdge>>(pInnerLoc.BorderWith);
                foreach (Location pOuterLoc in cEdges.Keys)
                {
                    //Нас интересуют только призрачные соседи
                    if (!pOuterLoc.IsShaded)
                        continue;

                    //раз призрачная - значит лежит за границей квадрата
                    var pLine = pInnerLoc.BorderWith[pOuterLoc][0];

                    bool bMadeIt = false;

                    VertexCH.Direction eDir = pOuterLoc.ShadowDir;
                    if (Neighbours.ContainsKey(eDir))
                    {
                        //находим реальное отражение призрачной локации
                        var pNeighbourChunk = Neighbours[eDir];
                        Location pShadow = pNeighbourChunk.m_cLocations[pOuterLoc.ShadowID];

                        //найдём среди соседей отражения призрачную локацию, соответствующую граничной
                        foreach (Location pShadowEdge in pShadow.BorderWith.Keys)
                        {
                            if (pShadowEdge.IsShaded)
                            {
                                var pShadowLine = pShadow.BorderWith[pShadowEdge][0];

                                VertexCH.Direction eShadowDir = pShadowEdge.ShadowDir;
                                if (pNeighbourChunk.Neighbours.ContainsKey(eShadowDir))
                                {
                                    var pShadowNeighbourChunk = pNeighbourChunk.Neighbours[eShadowDir];
                                    Location pShadowShadow = pShadowNeighbourChunk.m_cLocations[pShadowEdge.ShadowID];

                                    if (pShadowShadow == pInnerLoc)
                                    {
                                        //добавляем внутренней локации границу с отражением - такую же, как с его призраком
                                        if (!pInnerLoc.BorderWith.ContainsKey(pShadow))
                                        {
                                            pInnerLoc.BorderWith[pShadow] = new List<VoronoiEdge>() { pLine };
                                        }

                                        //добавляем отражению границу с внутренней локацией
                                        if (!pShadow.BorderWith.ContainsKey(pInnerLoc))
                                        {
                                            pShadow.BorderWith[pInnerLoc] = new List<VoronoiEdge>() { pShadowLine };
                                        }

                                        //убираем у внутренней локации границу с призраком
                                        pInnerLoc.BorderWith.Remove(pOuterLoc);
                                        pShadow.BorderWith.Remove(pShadowEdge);

                                        //сливаем вершины
                                        VoronoiVertex pGood1 = new VoronoiVertex(pLine.Point1);
                                        VoronoiVertex pGood2 = new VoronoiVertex(pLine.Point2);

                                        ReplaceVertexes(pLine.Point1, pGood1, pLine.Point2, pGood2);
                                        pNeighbourChunk.ReplaceVertexes(pShadowLine.Point2, pGood1, pShadowLine.Point1, pGood2);

                                        bMadeIt = true;

                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        pOuterLoc.IsBorder = true;
                        pOuterLoc.ShadowDir = VertexCH.Direction.CenterNone;

                        bMadeIt = true;
                    }

                    if (!bMadeIt)
                    {
                        throw new InvalidOperationException();
                    }
                }
            }
        }
     }
}
