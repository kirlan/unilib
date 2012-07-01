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
        public Dictionary<object, List<Line>> m_cBorder = new Dictionary<object, List<Line>>();

        public virtual void Start(INNER pSeed)
        {
            m_cBorder.Clear();

            foreach (var pInner in pSeed.BorderWith)
            {
                m_cBorder[pInner.Key] = new List<Line>();
                Line[] aLines = pInner.Value.ToArray();
                foreach (Line pLine in aLines)
                    m_cBorder[pInner.Key].Add(new Line(pLine));
            }

            //ChainBorder();
        }
        public List<Line> m_cFirstLines = new List<Line>();

        public bool TestChain()
        {
            List<Line> cTotalBorder = new List<Line>();
            foreach (var cLines in m_cBorder)
                cTotalBorder.AddRange(cLines.Value);
        
            Line[] aTotalBorder = cTotalBorder.ToArray();
            int iTotalCount = aTotalBorder.Length;

            for (int i = 0; i < iTotalCount; i++)
            {
                bool bGot1 = false;
                bool bGot2 = false; 
                
                Line pCurrentLine = aTotalBorder[i];
                for (int j = 0; j < iTotalCount; j++)
                {
                    if (i == j)
                        continue;

                    Line pLine = aTotalBorder[j];
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

        private List<List<Vertex>> m_pOrdered = new List<List<Vertex>>();

        /// <summary>
        /// Настраивает связи "следующая"-"предыдущая" среди граней, уже хранящихся в словаре границ с другими локациями.
        /// </summary>
        /// <param name="fCycleShift">Величина смещения X-координаты для закольцованной карты</param>
        protected void ChainBorder()
        {
            List<Line> cTotalBorder = new List<Line>();
            foreach (var cLines in m_cBorder)
                cTotalBorder.AddRange(cLines.Value);

            Line[] aTotalBorder = cTotalBorder.ToArray();
            int iTotalCount = aTotalBorder.Length;

            m_fAverageX = 0;
            m_fAverageY = 0;
            m_fAverageZ = 0;
            m_fAverageH = 0;
            float fPerimeter = 0;
            foreach (Line pLine in aTotalBorder)
            {
                pLine.m_pNext = null;
                pLine.m_pPrevious = null;

                m_fAverageX += pLine.m_fLength * (pLine.m_pPoint1.X + pLine.m_pPoint2.X) / 2;
                m_fAverageY += pLine.m_fLength * (pLine.m_pPoint1.Y + pLine.m_pPoint2.Y) / 2;
                m_fAverageZ += pLine.m_fLength * (pLine.m_pPoint1.Z + pLine.m_pPoint2.Z) / 2;
                m_fAverageH += pLine.m_fLength * (pLine.m_pPoint1.H + pLine.m_pPoint2.H) / 2;
                fPerimeter += pLine.m_fLength;
            }
            m_fAverageX /= fPerimeter;
            m_fAverageY /= fPerimeter;
            m_fAverageZ /= fPerimeter;
            m_fAverageH /= fPerimeter;

            m_cFirstLines.Clear();

            Line pFirstLine;
            do
            {
                pFirstLine = null;
                for (int i = 0; i < iTotalCount; i++)
                {
                    Line pLine = aTotalBorder[i];
                    if (pLine.m_pNext == null && pLine.m_pPrevious == null)
                    {
                        pFirstLine = pLine;
                        break;
                    }
                }

                if (pFirstLine != null)
                {
                    m_cFirstLines.Add(pFirstLine);
                    Line pCurrentLine = pFirstLine;

                    bool bGotIt;
                    int iCounter = 0;
                    do
                    {
                        bGotIt = false;
                        for (int i = 0; i < iTotalCount; i++)
                        {
                            Line pLine = aTotalBorder[i];
                            if (pLine.m_pPoint1 == pCurrentLine.m_pPoint2)
                            {
                                pCurrentLine.m_pNext = pLine;
                                pLine.m_pPrevious = pCurrentLine;

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
                }
            }
            while (pFirstLine != null);

            foreach (Line pFLine in m_cFirstLines)
            {
                List<Vertex> cVertexes = new List<Vertex>();

                Line pLine = pFLine;
                do
                {
                    cVertexes.Add(pLine.m_pPoint1);
                    pLine = pLine.m_pNext;
                }
                while (pLine != pFLine);

                m_pOrdered.Add(cVertexes);
            }
        }

        private float m_fAverageX = 0;
        private float m_fAverageY = 0;
        private float m_fAverageZ = 0;
        private float m_fAverageH = 0;

        public override float X
        {
            get { return m_fAverageX; }
            set { m_fAverageX = value; }
        }

        public override float Y
        {
            get { return m_fAverageY; }
            set { m_fAverageY = value; }
        }

        public override float Z
        {
            get { return m_fAverageZ; }
            set { m_fAverageZ = value; }
        }

        public override float H
        {
            get { return m_fAverageH; }
            set { m_fAverageH = value; }
        }
    }
}
