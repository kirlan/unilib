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

        public Location[] m_aGhostLocations;
        public Location[] m_aLocations;
        public Vertex[] m_aVertexes;

        private Dictionary<uint, Location> m_cLocations = new Dictionary<uint, Location>();

        public Chunk(ref VertexCH[] locations, ref CellCH[] vertices, float fDX, float fDY, float fR, Cube.Face3D eFace)
        {
            List<Location> cGhosts = new List<Location>();

            Dictionary<VertexCH, Location> cTempLocations = new Dictionary<VertexCH, Location>();
            for (int i=0; i<locations.Length; i++)
            {
                var loc = locations[i];
                Location myLocation = new Location(loc.m_iID, (float)loc.Position[0] + fDX - fR, (float)loc.Position[1] + fDY - fR, fR, eFace, loc.m_eGhost);
                cTempLocations[loc] = myLocation;
                m_cLocations[loc.m_iID] = myLocation;
                if (myLocation.Ghost)
                {
                    myLocation.SetShadows(loc.m_cShadow[VertexCH.Transformation.Stright].m_iID,
                        loc.m_cShadow[VertexCH.Transformation.Rotate90CCW].m_iID,
                        loc.m_cShadow[VertexCH.Transformation.Rotate90CW].m_iID,
                        loc.m_cShadow[VertexCH.Transformation.Rotate180].m_iID);
                    cGhosts.Add(myLocation);
                }
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
            m_aGhostLocations = cGhosts.ToArray();
        }

        List<Location> m_cResolvedGhosts = new List<Location>();

        public void Ghostbusters()
        {
            List<Vertex> cNewVertexes = new List<Vertex>(m_aVertexes);
            m_cResolvedGhosts.Clear();
            for (int i = 0; i < m_aGhostLocations.Length; i++)
            {
                var pLoc = m_aGhostLocations[i];
                VertexCH.Direction eDir = pLoc.m_eGhost;
                if (m_cNeighbours.ContainsKey(eDir))
                { 
                    var pNeighbourChunk = m_cNeighbours[eDir];
                    Location pShadow = pNeighbourChunk.m_pChunk.m_cLocations[pLoc.m_cShadow[pNeighbourChunk.m_eTransformation]];

                    foreach (var pEdge in pLoc.m_cEdges)
                    {
                        if (pEdge.Key.Ghost)
                            continue;

                        var pLine = pEdge.Key.m_cEdges[pLoc];

                        bool bUnfinished = false;
                        bool bMirror = false;
                        foreach (var pShadowEdge in pShadow.m_cEdges)
                        {
                            if (pShadowEdge.Key.Ghost)
                                bUnfinished = true;

                            if (pShadowEdge.Key == pEdge.Key)
                                bMirror = true;
                        }

                        if (!bUnfinished && !bMirror)
                        {
                            pShadow.m_cEdges[pEdge.Key] = new Location.Edge(pLine.m_pTo, pLine.m_pFrom);
                            cNewVertexes.Add(pLine.m_pTo);
                            cNewVertexes.Add(pLine.m_pFrom);
//                            throw new Exception("Хрень!");
                        }

                        if (Math.Abs(pShadow.m_fX - pLoc.m_fX) > 0.1 ||
                            Math.Abs(pShadow.m_fY - pLoc.m_fY) > 0.1 ||
                            Math.Abs(pShadow.m_fZ - pLoc.m_fZ) > 0.1)
                            throw new Exception("Ложное отражение!");

                        pEdge.Key.m_cEdges[pShadow] = pLine;
                        pEdge.Key.m_cEdges.Remove(pLoc);
                    }
                    m_cResolvedGhosts.Add(pShadow);
                }
            }

            m_aVertexes = cNewVertexes.ToArray();
        }

        public void FixEdges()
        {
            List<Vertex> cNewVertexes = new List<Vertex>(m_aVertexes);
            //List<Vertex> cOldVertexes = new List<Vertex>(m_aVertexes);

            foreach (var pOuterLoc in m_cResolvedGhosts)
            {
                foreach (var pOuterLocEdge in pOuterLoc.m_cEdges)
                {
                    var pOuterA = pOuterLocEdge.Value.m_pFrom;
                    var pOuterB = pOuterLocEdge.Value.m_pTo;

                    cNewVertexes.Add(pOuterA);
                    cNewVertexes.Add(pOuterB);

                    if (!pOuterLocEdge.Key.m_cEdges.ContainsKey(pOuterLoc))
                        //throw new Exception("Не обоюдная граница!");
                        continue;

                    var pInnerA = pOuterLocEdge.Key.m_cEdges[pOuterLoc].m_pTo;
                    var pInnerB = pOuterLocEdge.Key.m_cEdges[pOuterLoc].m_pFrom;

                    //обе линии оперируют одними и теми же точками - no problem
                    if (pOuterA == pInnerA &&
                        pOuterB == pInnerB)
                        continue;

                    List<Vertex> cTemp = new List<Vertex>(pInnerA.m_cLinked);
                    foreach (Location pLinkedLoc in cTemp)
                        pLinkedLoc.ReplaceVertex(pInnerA, pOuterA);

                    cTemp.Clear();
                    cTemp.AddRange(pInnerB.m_cLinked);
                    foreach (Location pLinkedLoc in cTemp)
                        pLinkedLoc.ReplaceVertex(pInnerB, pOuterB);

                    cNewVertexes.Remove(pInnerA);
                    cNewVertexes.Remove(pInnerA);
                }
            }

            m_aVertexes = cNewVertexes.ToArray();
        }
    }
}
