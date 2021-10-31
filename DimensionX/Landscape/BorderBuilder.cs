using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LandscapeGeneration.PathFind;

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
        public List<Location.Edge> m_cFirstLines = new List<Location.Edge>();

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
                                pLine.m_pPoint1.Y == pCurrentLine.m_pPoint2.Y)
                        bGot1 = true;
                    if (pLine.m_pPoint2 == pCurrentLine.m_pPoint1 ||
                                pLine.m_pPoint2.Y == pCurrentLine.m_pPoint1.Y)
                        bGot2 = true;

                    if (bGot1 && bGot2)
                        break;
                }

                if (!bGot1 || !bGot2)
                    return false;
            }

            return true;
        }

        public readonly List<List<VoronoiVertex>> m_cOrdered = new List<List<VoronoiVertex>>();

        /// <summary>
        /// Настраивает связи "следующая"-"предыдущая" среди граней, уже хранящихся в словаре границ с другими локациями.
        /// </summary>
        /// <param name="fCycleShift">Величина смещения X-координаты для закольцованной карты</param>
        protected void ChainBorder(float fCycleShift)
        {
            List<Location.Edge> cTotalBorder = new List<Location.Edge>();
            foreach (var cLines in m_cBorder)
                cTotalBorder.AddRange(cLines.Value);

            Location.Edge[] aTotalBorder = cTotalBorder.ToArray();
            int iTotalCount = aTotalBorder.Length;

            X = 0;
            Y = 0;
            H = 0;
            float fPerimeter = 0;
            foreach (Location.Edge pLine in aTotalBorder)
            {
                pLine.m_pNext = null;
                //pLine.m_pPrevious = null;

                float fRelativeX1 = pLine.m_pPoint1.X;
                if (fRelativeX1 > aTotalBorder[0].m_pPoint1.X + fCycleShift / 2)
                    fRelativeX1 -= fCycleShift;
                if (fRelativeX1 < aTotalBorder[0].m_pPoint1.X - fCycleShift / 2)
                    fRelativeX1 += fCycleShift;

                float fRelativeX2 = pLine.m_pPoint2.X;
                if (fRelativeX2 > aTotalBorder[0].m_pPoint1.X + fCycleShift / 2)
                    fRelativeX2 -= fCycleShift;
                if (fRelativeX2 < aTotalBorder[0].m_pPoint1.X - fCycleShift / 2)
                    fRelativeX2 += fCycleShift;

                X += pLine.Length * (fRelativeX1 + fRelativeX2) / 2;
                Y += pLine.Length * (pLine.m_pPoint1.Y + pLine.m_pPoint2.Y) / 2;
                H += pLine.Length * (pLine.m_pPoint1.H + pLine.m_pPoint2.H) / 2;
                fPerimeter += pLine.Length;
            }
            X /= fPerimeter;
            Y /= fPerimeter;
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
                    List<VoronoiVertex> cVertexes = new List<VoronoiVertex>();

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
                            if (pLine.m_pPoint1 == pCurrentLine.m_pPoint2 ||
                                (pLine.m_pPoint1.Y == pCurrentLine.m_pPoint2.Y &&
                                 (pLine.m_pPoint1.X == pCurrentLine.m_pPoint2.X ||
                                  Math.Abs(pLine.m_pPoint1.X - pCurrentLine.m_pPoint2.X) == fCycleShift)))
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
        }
    }
}
