using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        private Dictionary<uint, Location> m_cLocations = new Dictionary<uint, Location>();

        public Chunk(ref VertexCH[] locations, ref CellCH[] vertices, float fDX, float fDY, float fR, Cube.Face3D eFace)
        {
            List<Location> cBorders = new List<Location>();

            Dictionary<VertexCH, Location> cTempLocations = new Dictionary<VertexCH, Location>();
            for (int i=0; i<locations.Length; i++)
            {
                var loc = locations[i];
                Location myLocation = new Location(loc.m_iID, (float)loc.Position[0] + fDX - fR, (float)loc.Position[1] + fDY - fR, fR, eFace, loc.m_eGhost, loc.m_bBorder);
                cTempLocations[loc] = myLocation;
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

            Dictionary<CellCH, Vertex> cTempVertexes = new Dictionary<CellCH, Vertex>();
            for (int i = 0; i < vertices.Length; i++)
            {
                var vertex = vertices[i];
                Vertex myVertex = new Vertex((float)vertex.Circumcenter.X + fDX - fR, (float)vertex.Circumcenter.Y + fDY - fR, fR, eFace);

                foreach (var vert in vertex.Vertices)
                    myVertex.m_cLinked.Add(cTempLocations[vert]);

                cTempVertexes[vertex] = myVertex;
            }

            for (int i = 0; i < locations.Length; i++)
            {
                var loc = locations[i];
                Location myLocation = cTempLocations[loc];
                foreach (var edge in loc.m_cEdges)
                {
                    myLocation.m_cEdges[cTempLocations[edge.Key]] = new Location.Edge(cTempVertexes[edge.Value.m_pFrom], cTempVertexes[edge.Value.m_pTo]);
                }
            }

            m_aVertexes = cTempVertexes.Values.ToArray();
            m_aLocations = cTempLocations.Values.ToArray();
            m_aBorderLocations = cBorders.ToArray();
        }

        List<Location> m_cResolvedBorder = new List<Location>();

        public void Ghostbusters()
        {
            //List<Vertex> cNewVertexes = new List<Vertex>(m_aVertexes);
            m_cResolvedBorder.Clear();

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

                    VertexCH.Direction eDir = pOuterLoc.m_eGhost;
                    if (m_cNeighbours.ContainsKey(eDir))
                    { 
                        //находим реальное отражение призрачной локации
                        var pNeighbourChunk = m_cNeighbours[eDir];
                        Location pShadow = pNeighbourChunk.m_pChunk.m_cLocations[pOuterLoc.m_cShadow[pNeighbourChunk.m_eTransformation]];

                        var pLine = pInnerLoc.m_cEdges[pOuterLoc];

                        //добавляем внутренней локации границу с отражением - такую же, как с его призраком
                        if (!pInnerLoc.m_cEdges.ContainsKey(pShadow))
                            pInnerLoc.m_cEdges[pShadow] = pLine;

                        //добавляем отражению границу с внутренней локацией
                        if (!pShadow.m_cEdges.ContainsKey(pInnerLoc))
                            pShadow.m_cEdges[pInnerLoc] = pLine.Reverse();

                        //убираем у внутренней локации границу с призраком
                        pInnerLoc.m_cEdges.Remove(pOuterLoc);
                    }
                    //помечаем внутреннюю локацию для восстановления обоюдности границ
                    m_cResolvedBorder.Add(pInnerLoc);
                }
                //foreach (var pEdge in pInnerLoc.m_cEdges)
                //{
                //    if (pEdge.Key.Ghost)
                //        throw new Exception();
                //}
            }

            //m_aVertexes = cNewVertexes.ToArray();
        }

        public void FixEdges()
        {
            List<Vertex> cNewVertexes = new List<Vertex>(m_aVertexes);

            foreach (var pInnerLoc in m_cResolvedBorder)
            {
                foreach (var pInnerLocEdge in pInnerLoc.m_cEdges)
                {
                    var pInnerA = pInnerLocEdge.Value.m_pFrom;
                    var pInnerB = pInnerLocEdge.Value.m_pTo;

                    //cNewVertexes.Add(pInner_A);
                    //cNewVertexes.Add(pInner_B);

                    var pOuterLoc = pInnerLocEdge.Key;

                    if (!pOuterLoc.m_cEdges.ContainsKey(pInnerLoc))
                        throw new Exception("Не обоюдная граница!");
                        //continue;

                    var pOuterA = pOuterLoc.m_cEdges[pInnerLoc].m_pTo;
                    var pOuterB = pOuterLoc.m_cEdges[pInnerLoc].m_pFrom;

                    //обе линии оперируют одними и теми же точками - no problem
                    if (pInnerA == pOuterA &&
                        pInnerB == pOuterB)
                        continue;

                    if (pOuterA.m_bForbidden)
                    {
                        var pSwap = pOuterA;
                        pOuterA = pInnerA;
                        pInnerA = pSwap;
                    }

                    if (pOuterA.m_bForbidden)
                        throw new Exception();

                    List<Vertex> cTemp = new List<Vertex>(pOuterA.m_cLinked);
                    foreach (Location pLinkedLoc in cTemp)
                        pLinkedLoc.ReplaceVertex(pOuterA, pInnerA);
                    //if (!cNewVertexes.Contains(pInnerA))
                    //    cNewVertexes.Add(pInnerA);

                    if (pOuterB.m_bForbidden)
                    {
                        var pSwap = pOuterB;
                        pOuterB = pInnerB;
                        pInnerB = pSwap;
                    }

                    cTemp.Clear();
                    cTemp.AddRange(pOuterB.m_cLinked);
                    foreach (Location pLinkedLoc in cTemp)
                        pLinkedLoc.ReplaceVertex(pOuterB, pInnerB);
                    //if (!cNewVertexes.Contains(pInnerB))
                    //    cNewVertexes.Add(pInnerB);

                    pOuterA.m_bForbidden = true;
                    pOuterB.m_bForbidden = true;

                    //cNewVertexes.Remove(pOuterA);
                    //cNewVertexes.Remove(pOuterA);

                    if (!cNewVertexes.Contains(pInnerLocEdge.Value.m_pFrom))
                        cNewVertexes.Add(pInnerLocEdge.Value.m_pFrom);
                    if (!cNewVertexes.Contains(pInnerLocEdge.Value.m_pTo))
                        cNewVertexes.Add(pInnerLocEdge.Value.m_pTo);
                }
            }

            m_aVertexes = cNewVertexes.ToArray();
        }
    }
}
