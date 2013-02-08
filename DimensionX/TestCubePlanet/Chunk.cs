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

        private Dictionary<uint, Location> m_cLocations = new Dictionary<uint, Location>();

        public Chunk(ref VertexCH[] locations, ref CellCH[] vertices, float fDX, float fDY, float fR, Cube.Face3D eFace)
        {
            Microsoft.Xna.Framework.Color eColor = Microsoft.Xna.Framework.Color.White;
            eColor = Microsoft.Xna.Framework.Color.FromNonPremultiplied(127 + Rnd.Get(100), 127 + Rnd.Get(100), 127 + Rnd.Get(100), 256);

            List<Location> cBorders = new List<Location>();

            Dictionary<VertexCH, Location> cTempLocations = new Dictionary<VertexCH, Location>();
            for (int i=0; i<locations.Length; i++)
            {
                var loc = locations[i];
                Location myLocation = new Location(loc.m_iID, (float)loc.Position[0] + fDX - fR, (float)loc.Position[1] + fDY - fR, fR, eFace, loc.m_eGhost, loc.m_bBorder);
                myLocation.m_eColor = eColor;
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
                myVertex.m_eColor = eColor;

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
            List<Vertex> cNewVertexes = new List<Vertex>(m_aVertexes);
            //m_cResolvedBorder.Clear();

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
                                        Vertex pBad1 = pLine.m_pFrom;
                                        Vertex pGood1 = pShadowLine.m_pTo;
                                        Vertex pBad2 = pLine.m_pTo;
                                        Vertex pGood2 = pShadowLine.m_pFrom;

                                        if (pBad1 != pGood1)
                                        {
                                            List<Vertex> cTemp = new List<Vertex>(pLine.m_pFrom.m_cLinked);
                                            //проходим по всем локациям, связанным к "неправильной" вершиной
                                            foreach (Location pLinkedLoc in cTemp)
                                                pLinkedLoc.ReplaceVertex(pBad1, pGood1);

                                            pBad1.m_bForbidden = true;
                                        }

                                        if (pBad2 != pGood2)
                                        {
                                            List<Vertex> cTemp = new List<Vertex>(pLine.m_pTo.m_cLinked);
                                            foreach (Location pLinkedLoc in cTemp)
                                                pLinkedLoc.ReplaceVertex(pBad2, pGood2);

                                            pBad2.m_bForbidden = true;
                                        }

                                        foreach (var pT in pInnerLoc.m_cEdges)
                                        {
                                            if (pT.Key.Ghost)
                                                continue;

                                            foreach(var pTT in pT.Key.m_cEdges)
                                            {
                                                if (pTT.Value.m_pFrom.m_bForbidden || pTT.Value.m_pTo.m_bForbidden)
                                                    throw new Exception();
                                            }
                                        }

                                        if (!cNewVertexes.Contains(pGood1))
                                            cNewVertexes.Add(pGood1);
                                        if (!cNewVertexes.Contains(pGood2))
                                            cNewVertexes.Add(pGood2);

                                        break;
                                    }
                                }
                            }
                        }
                    }
                    //помечаем внутреннюю локацию для восстановления обоюдности границ
                    //m_cResolvedBorder.Add(pInnerLoc);
                }
                //foreach (var pEdge in pInnerLoc.m_cEdges)
                //{
                //    if (pEdge.Key.Ghost)
                //        throw new Exception();
                //}
            }

            m_aVertexes = cNewVertexes.ToArray();
        }

     /* public void FixEdges()
        {
            List<Vertex> cNewVertexes = new List<Vertex>(m_aVertexes);

            //перебираем все пограничные регионы
            foreach (var pInnerLoc in m_aBorderLocations)
            {
                //в каждом пограничном регионе смотрим все границы
                foreach (var pInnerLocEdge in pInnerLoc.m_cEdges)
                {
                    //это - вершины границы с точки зрение пограничного региона
                    var pInnerA = pInnerLocEdge.Value.m_pFrom;
                    var pInnerB = pInnerLocEdge.Value.m_pTo;

                    //это - регион с которым пограничный граничит по границе
                    var pOuterLoc = pInnerLocEdge.Key;

                    //есть ли у сопредельного региона граница с пограничным?
                    if (!pOuterLoc.m_cEdges.ContainsKey(pInnerLoc))
                        throw new Exception("Не обоюдная граница!");
                        //continue;

                    //это - вершины границы с точки зрения сопредельного региона
                    var pOuterA = pOuterLoc.m_cEdges[pInnerLoc].m_pTo;
                    var pOuterB = pOuterLoc.m_cEdges[pInnerLoc].m_pFrom;

                    //обе линии оперируют одними и теми же точками - no problem
                    if (pInnerA == pOuterA &&
                        pInnerB == pOuterB)
                    {
                        if (!cNewVertexes.Contains(pInnerLocEdge.Value.m_pFrom))
                            cNewVertexes.Add(pInnerLocEdge.Value.m_pFrom);
                        if (!cNewVertexes.Contains(pInnerLocEdge.Value.m_pTo))
                            cNewVertexes.Add(pInnerLocEdge.Value.m_pTo);
                        continue;
                    }

                    //назначим одну из точек А "неправильной". 
                    //Будем считать, что это наружная точка А.
                    //Если наружная точка А уже неправильная, поменяем точки местами
                    if (pOuterA.m_bForbidden)
                    {
                        var pSwap = pOuterA;
                        pOuterA = pInnerA;
                        pInnerA = pSwap;
                    }

                    if (pOuterA.m_bForbidden)
                        throw new Exception();

                    //заменим все ссылки на "неправильную" А ссылкой на правильную
                    List<Vertex> cTemp = new List<Vertex>(pOuterA.m_cLinked);
                    foreach (Location pLinkedLoc in cTemp)
                        pLinkedLoc.ReplaceVertex(pOuterA, pInnerA);
                    pInnerLoc.ReplaceVertex(pOuterA, pInnerA);
                    if (!cNewVertexes.Contains(pInnerA))
                        cNewVertexes.Add(pInnerA);

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
                    pInnerLoc.ReplaceVertex(pOuterB, pInnerB);
                    if (!cNewVertexes.Contains(pInnerB))
                        cNewVertexes.Add(pInnerB);

                    pOuterA.m_bForbidden = true;
                    pOuterB.m_bForbidden = true;

                    cNewVertexes.Remove(pOuterA);
                    cNewVertexes.Remove(pOuterB);

                    //if (!cNewVertexes.Contains(pInnerLocEdge.Value.m_pFrom))
                    //    cNewVertexes.Add(pInnerLocEdge.Value.m_pFrom);
                    //if (!cNewVertexes.Contains(pInnerLocEdge.Value.m_pTo))
                    //    cNewVertexes.Add(pInnerLocEdge.Value.m_pTo);
                }
            }

            m_aVertexes = cNewVertexes.ToArray();

            for (int i = 0; i < m_aLocations.Length; i++)
            {
                var pLoc = m_aLocations[i];
                if (pLoc.Ghost)
                    continue;

                //if (!pLoc.m_bBorder)
                //    continue;

                //if (!m_aBorderLocations.Contains(pLoc))
                //    throw new Exception();

                //foreach (var pEdge in pLoc.m_cEdges)
                //{
                //    if (!cNewVertexes.Contains(pEdge.Value.m_pFrom))
                //        cNewVertexes.Add(pEdge.Value.m_pFrom);
                //    if (!cNewVertexes.Contains(pEdge.Value.m_pTo))
                //        cNewVertexes.Add(pEdge.Value.m_pTo);
                //}

                Dictionary<Location, Location.Edge> cSequence = new Dictionary<Location,Location.Edge>();

                bool bUnclosed = false;
                Location pLastLoc = pLoc.m_cEdges.Keys.First();
                Location.Edge pLast = pLoc.m_cEdges[pLastLoc];
                cSequence[pLastLoc] = pLast;
                for(int j=0; j<pLoc.m_cEdges.Count; j++)
                {
                    foreach (var pEdge in pLoc.m_cEdges)
                    {
                        if (pEdge.Key.Ghost)
                            bUnclosed = true;
                        if(pEdge.Value.m_pFrom == pLast.m_pTo && !cSequence.ContainsKey(pEdge.Key))
                        {
                            pLast = pEdge.Value;
                            cSequence[pEdge.Key] = pLast;
                            break;
                        }
                    }
                }

                if (!bUnclosed && cSequence.Count != pLoc.m_cEdges.Count)
                {
                    if (cSequence.Count > 1 && cSequence.Keys.First() == cSequence.Keys.Last())
                    {
                        pLoc.m_cEdges = cSequence;
                    }
                    else
                        throw new Exception();
                }

                //foreach (var pEdge in pLoc.m_cEdges)
                //{
                //    var pInnerA = pEdge.Value.m_pFrom;
                //    var pInnerB = pEdge.Value.m_pTo;

                //    var pOuterA = pEdge.Key.m_cEdges[pLoc].m_pTo;
                //    var pOuterB = pEdge.Key.m_cEdges[pLoc].m_pFrom;

                //    if (pInnerA != pOuterA ||
                //        pInnerB != pOuterB)
                //        throw new Exception();

                //    if (pInnerA.m_bForbidden || pOuterA.m_bForbidden ||
                //        pInnerB.m_bForbidden || pOuterB.m_bForbidden)
                //        throw new Exception();
                //}
            }
        }
        */
    }
}
