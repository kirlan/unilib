using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LandscapeGeneration.PathFind;
using LandscapeGeneration.PlanetBuilder;

namespace LandscapeGeneration
{
    public abstract class BorderBuilder<INNER> : TransportationNode
        where INNER : class, ITerritory
    {
        public Dictionary<object, List<Location.Edge>> m_cBorder = new Dictionary<object, List<Location.Edge>>();

        public virtual void Start(INNER pSeed)
        {
            m_cBorder.Clear();

            foreach (var pInner in pSeed.BorderWith)
            {
                m_cBorder[pInner.Key] = new List<Location.Edge>();
                Location.Edge[] aLines = pInner.Value.ToArray();
                foreach (Location.Edge pLine in aLines)
                    m_cBorder[pInner.Key].Add(new Location.Edge(pLine));
            }

            //ChainBorder();
        }
        private List<Location.Edge> m_cFirstLines = new List<Location.Edge>();

        public bool TestChain()
        {
            List<Location.Edge> cTotalBorder = new List<Location.Edge>();
            foreach (var cLines in m_cBorder)
                cTotalBorder.AddRange(cLines.Value);

            Location.Edge[] aTotalBorder = cTotalBorder.ToArray();
            int iTotalCount = aTotalBorder.Length;

            for (int i = 0; i < iTotalCount; i++)
            {
                bool bGot1 = false;
                bool bGot2 = false;

                Location.Edge pCurrentLine = aTotalBorder[i];
                for (int j = 0; j < iTotalCount; j++)
                {
                    if (i == j)
                        continue;

                    Location.Edge pLine = aTotalBorder[j];
                    if (pLine.m_pPoint1 == pCurrentLine.m_pPoint2 ||
                                pLine.m_pPoint1.m_fY == pCurrentLine.m_pPoint2.m_fY)
                        bGot1 = true;
                    if (pLine.m_pPoint2 == pCurrentLine.m_pPoint1 ||
                                pLine.m_pPoint2.m_fY == pCurrentLine.m_pPoint1.m_fY)
                        bGot2 = true;

                    if (bGot1 && bGot2)
                        break;
                }

                if (!bGot1 || !bGot2)
                    return false;
            }

            return true;
        }

        public List<List<Vertex>> m_cOrdered = new List<List<Vertex>>();

        /// <summary>
        /// Настраивает связи "следующая"-"предыдущая" среди граней, уже хранящихся в словаре границ с другими локациями.
        /// </summary>
        /// <param name="fCycleShift">Величина смещения X-координаты для закольцованной карты</param>
        protected void ChainBorder()
        {
            List<Location.Edge> cTotalBorder = new List<Location.Edge>();
            foreach (var cLines in m_cBorder)
                cTotalBorder.AddRange(cLines.Value);

            Location.Edge[] aTotalBorder = cTotalBorder.ToArray();
            int iTotalCount = aTotalBorder.Length;

            X = 0;
            Y = 0;
            Z = 0;
            H = 0;
            float fPerimeter = 0;
            foreach (Location.Edge pLine in aTotalBorder)
            {
                pLine.m_pNext = null;
                //pLine.m_pPrevious = null;

                X += pLine.m_fLength * (pLine.m_pPoint1.X + pLine.m_pPoint2.X) / 2;
                Y += pLine.m_fLength * (pLine.m_pPoint1.Y + pLine.m_pPoint2.Y) / 2;
                Z += pLine.m_fLength * (pLine.m_pPoint1.Z + pLine.m_pPoint2.Z) / 2;
                H += pLine.m_fLength * (pLine.m_pPoint1.H + pLine.m_pPoint2.H) / 2;
                fPerimeter += pLine.m_fLength;
            }
            X /= fPerimeter;
            Y /= fPerimeter;
            Z /= fPerimeter;
            H /= fPerimeter;

            m_cFirstLines.Clear();
            m_cOrdered.Clear();

            Location.Edge pFirstLine;
            do
            {
                pFirstLine = null;
                for (int i = 0; i < iTotalCount; i++)
                {
                    Location.Edge pLine = aTotalBorder[i];
                    if (pLine.m_pNext == null)// && pLine.m_pPrevious == null)
                    {
                        pFirstLine = pLine;
                        break;
                    }
                }

                if (pFirstLine != null)
                {
                    List<Vertex> cVertexes = new List<Vertex>();

                    m_cFirstLines.Add(pFirstLine);
                    Location.Edge pCurrentLine = pFirstLine;

                    bool bGotIt;
                    int iCounter = 0;
                    do
                    {
                        bGotIt = false;
                        for (int i = 0; i < iTotalCount; i++)
                        {
                            Location.Edge pLine = aTotalBorder[i];
                            if (pLine.m_pPoint1 == pCurrentLine.m_pPoint2)
                            {
                                pCurrentLine.m_pNext = pLine;
                                //pLine.m_pPrevious = pCurrentLine;
                                cVertexes.Add(pCurrentLine.m_pPoint1);

                                pCurrentLine = pLine;
                                bGotIt = true;
                                iCounter++;
                                break;
                            }
                        }
                        if (!bGotIt)
                        {
                            throw new Exception("Can't chain the border!");
                        }
                    }
                    while (pCurrentLine != pFirstLine && bGotIt && iCounter <= iTotalCount);

                    m_cOrdered.Add(cVertexes);
                }
            }
            while (pFirstLine != null);

            //foreach (Location.Edge pFLine in m_cFirstLines)
            //{
            //    bool bOK = false;
            //    foreach (var pT in m_cBorder)
            //        if (pT.Value.Contains(pFLine))
            //            bOK = true;

            //    if (!bOK)
            //        throw new Exception("Wrong firstline!");

            //    List<Vertex> cVertexes = new List<Vertex>();

            //    Location.Edge pLine = pFLine;
            //    do
            //    {
            //        cVertexes.Add(pLine.m_pPoint1);
            //        pLine = pLine.m_pNext;
            //        if (cVertexes.Count > aTotalBorder.Length)
            //            throw new Exception("Borderline too long!");
            //    }
            //    while (pLine != pFLine);

            //}
        }
    }
}
