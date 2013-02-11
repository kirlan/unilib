using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;

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

        public void RebuildVertexArray()
        {
            List<Vertex> cNewVertexes = new List<Vertex>();

            for (int i = 0; i < m_aLocations.Length; i++)
            {
                var pLoc = m_aLocations[i];
                if (pLoc.Ghost)
                    continue;

                foreach (var pEdge in pLoc.m_cEdges)
                {
                    if (pEdge.Value.m_pFrom.m_pChunkMarker != this)
                    {
                        cNewVertexes.Add(pEdge.Value.m_pFrom);
                        pEdge.Value.m_pFrom.m_pChunkMarker = this;
                    }
                    pLoc.m_fX += pEdge.Value.m_pFrom.m_fX;
                    pLoc.m_fY += pEdge.Value.m_pFrom.m_fY;
                    pLoc.m_fZ += pEdge.Value.m_pFrom.m_fZ;
                }

                pLoc.m_fX /= pLoc.m_cEdges.Count + 1;
                pLoc.m_fY /= pLoc.m_cEdges.Count + 1;
                pLoc.m_fZ /= pLoc.m_cEdges.Count + 1;
            }

            m_aVertexes = cNewVertexes.ToArray();
        }

        public void ReplaceVertexes(Vertex pBad1, Vertex pGood1, Vertex pBad2, Vertex pGood2)
        {
            if (pBad1 != pGood1)
                pBad1.Replace(pGood1);

            if (pBad2 != pGood2)
                pBad2.Replace(pGood2);
        }

        private Dictionary<uint, Location> m_cLocations = new Dictionary<uint, Location>();

        private string m_sName;

        public override string ToString()
        {
            return m_sName;
        }

        public Chunk(ref VertexCH[] locations, ref CellCH[] vertices, float fDX, float fDY, float fR, Cube.Face3D eFace)
        {
            Microsoft.Xna.Framework.Color eColor = Microsoft.Xna.Framework.Color.White;
            eColor = Microsoft.Xna.Framework.Color.FromNonPremultiplied(127 + Rnd.Get(100), 127 + Rnd.Get(100), 127 + Rnd.Get(100), 256);

            m_sName = string.Format("{0} ({1}, {2})", eFace, fDX, fDY);

            List<Location> cBorders = new List<Location>();

            List<Location> cTempLocations = new List<Location>();
            for (int i = 0; i < locations.Length; i++)
            {
                var loc = locations[i];
                Location myLocation = new Location(loc.m_iID, (float)loc.Position[0] + fDX - fR, (float)loc.Position[1] + fDY - fR, fR, eFace, loc.m_eGhost, loc.m_bBorder);
                myLocation.m_eColor = eColor;
                cTempLocations.Add(myLocation);
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

            List<Vertex> cTempVertexes = new List<Vertex>();
            for (int i = 0; i < vertices.Length; i++)
            {
                var vertex = vertices[i];
                Vertex myVertex = new Vertex((float)vertex.Circumcenter.X + fDX - fR, (float)vertex.Circumcenter.Y + fDY - fR, fR, eFace);
                myVertex.m_eColor = eColor;

                vertex.m_pTag = myVertex;
                cTempVertexes.Add(myVertex);
            }

            for (int i = 0; i < locations.Length; i++)
            {
                var loc = locations[i];
                Location myLocation = (Location)loc.m_pTag;
                foreach (var edge in loc.m_cEdges)
                {
                    Vertex pVertex1 = (Vertex)edge.Value.m_pFrom.m_pTag;
                    Vertex pVertex2 = (Vertex)edge.Value.m_pTo.m_pTag;
                    myLocation.m_cEdges[(Location)edge.Key.m_pTag] = new Location.Edge(pVertex1, pVertex2);
                    if (!pVertex1.m_cLinked.Contains(myLocation))
                        pVertex1.m_cLinked.Add(myLocation);
                    if (!pVertex2.m_cLinked.Contains(myLocation))
                        pVertex2.m_cLinked.Add(myLocation);
                }
            }

            m_aVertexes = cTempVertexes.ToArray();
            m_aLocations = cTempLocations.ToArray();
            m_aBorderLocations = cBorders.ToArray();
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
                                        //>>>>>>>>>>>>>>>>DEBUG
                                        //foreach (var pT in pShadow.m_cEdges)
                                        //{
                                        //    if (pT.Key.Ghost)
                                        //        continue;

                                        //    foreach (var pTT in pT.Key.m_cEdges)
                                        //    {
                                        //        if (pTT.Value.m_pFrom.m_bForbidden || pTT.Value.m_pTo.m_bForbidden)
                                        //            throw new Exception();
                                        //    }

                                        //    if (pT.Value.m_pFrom.m_bForbidden || pT.Value.m_pTo.m_bForbidden)
                                        //        throw new Exception();
                                        //}
                                        //<<<<<<<<<<<<<<<<DEBUG


                                        //добавляем внутренней локации границу с отражением - такую же, как с его призраком
                                        if (!pInnerLoc.m_cEdges.ContainsKey(pShadow))
                                            pInnerLoc.m_cEdges[pShadow] = pLine;

                                        //добавляем отражению границу с внутренней локацией
                                        if (!pShadow.m_cEdges.ContainsKey(pInnerLoc))
                                            pShadow.m_cEdges[pInnerLoc] = pShadowLine;

                                        //убираем у внутренней локации границу с призраком
                                        pInnerLoc.m_cEdges.Remove(pOuterLoc);
                                        pShadow.m_cEdges.Remove(pShadowEdge.Key);

                                        //сливаем вершины
                                        Vertex pGood1 = new Vertex(pLine.m_pFrom);
                                        pGood1.m_eColor = Microsoft.Xna.Framework.Color.Lerp(pGood1.m_eColor, pShadowLine.m_pTo.m_eColor, 0.5f);
                                        Vertex pGood2 = new Vertex(pLine.m_pTo);
                                        pGood2.m_eColor = Microsoft.Xna.Framework.Color.Lerp(pGood2.m_eColor, pShadowLine.m_pFrom.m_eColor, 0.5f);

                                        ReplaceVertexes(pLine.m_pFrom, pGood1, pLine.m_pTo, pGood2);
                                        pNeighbourChunk.m_pChunk.ReplaceVertexes(pShadowLine.m_pTo, pGood1, pShadowLine.m_pFrom, pGood2);

                                        //>>>>>>>>>>>>>>>>DEBUG
                                        //foreach (var pT in pInnerLoc.m_cEdges)
                                        //{
                                        //    if (pT.Key.Ghost)
                                        //        continue;

                                        //    foreach (var pTT in pT.Key.m_cEdges)
                                        //    {
                                        //        if (pTT.Value.m_pFrom.m_bForbidden || pTT.Value.m_pTo.m_bForbidden)
                                        //            throw new Exception();
                                        //    }

                                        //    if (pT.Value.m_pFrom.m_bForbidden || pT.Value.m_pTo.m_bForbidden)
                                        //        throw new Exception();
                                        //}

                                        //foreach (var pT in pShadow.m_cEdges)
                                        //{
                                        //    if (pT.Key.Ghost)
                                        //        continue;

                                        //    foreach (var pTT in pT.Key.m_cEdges)
                                        //    {
                                        //        if (pTT.Value.m_pFrom.m_bForbidden || pTT.Value.m_pTo.m_bForbidden)
                                        //            throw new Exception();
                                        //    }

                                        //    if (pT.Value.m_pFrom.m_bForbidden || pT.Value.m_pTo.m_bForbidden)
                                        //        throw new Exception();
                                        //}
                                        //<<<<<<<<<<<<<<<<DEBUG

                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                //>>>>>>>>>>>>>>>>DEBUG
                //foreach (var pEdge in pInnerLoc.m_cEdges)
                //{
                //    if (pEdge.Key.Ghost)
                //        continue;

                //    if (pEdge.Value.m_pFrom.m_bForbidden || pEdge.Value.m_pTo.m_bForbidden)
                //        throw new Exception();

                //    foreach(var pEdge2 in pEdge.Key.m_cEdges)
                //        if (pEdge.Value.m_pFrom.m_bForbidden || pEdge.Value.m_pTo.m_bForbidden)
                //            throw new Exception();
                //}
                //<<<<<<<<<<<<<<<<DEBUG
            }
        }
    }
}
