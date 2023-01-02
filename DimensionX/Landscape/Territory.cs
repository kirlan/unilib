using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;

namespace LandscapeGeneration
{
    public class Territory : BaseTerritory
    {
        /// <summary>
        /// Локации, составляющие территорию
        /// </summary>
        public List<BaseTerritory> m_cContents = new List<BaseTerritory>();

        /// <summary>
        /// границы с другими объектами НИЗШЕГО УРОВНЯ, т.е. составными частями данного объекта
        /// </summary>
        public Dictionary<BaseTerritory, List<Location.Edge>> m_cBorder = new Dictionary<BaseTerritory, List<Location.Edge>>();

        private static Territory m_pForbidden = new Territory(true);

        public Territory(bool bForbidden)
            : base(bForbidden)
        {
        }

        public void Start(BaseTerritory pSeed)
        {
            m_cContents.Clear();
            BorderWith.Clear();

            m_cBorder.Clear();

            foreach (var pInner in pSeed.BorderWith)
            {
                m_cBorder[pInner.Key] = new List<Location.Edge>();
                Location.Edge[] aLines = pInner.Value.ToArray();
                foreach (Location.Edge pLine in aLines)
                    m_cBorder[pInner.Key].Add(new Location.Edge(pLine));
            }

            m_cContents.Add(pSeed);
        }

        /// <summary>
        /// Присоединяет к земле сопредельную нечейную локацию.
        /// Чем длиннее общая граница с локацией - тем выше вероятность того, что выбрана будет именно она.
        /// </summary>
        /// <returns></returns>
        public virtual object Grow(Func<BaseTerritory, float> getWeightFunc)
        {
            Dictionary<BaseTerritory, float> cBorderLength = new Dictionary<BaseTerritory, float>();

            foreach (var pInner in m_cBorder)
            {
                BaseTerritory pInnerTerritory = pInner.Key as BaseTerritory;
                if (pInnerTerritory.Owner == null && !pInnerTerritory.Forbidden)
                {
                    float weight = getWeightFunc(pInnerTerritory);

                    //float fWholeLength = 1;
                    //Location.Edge[] aBorderLine = pInner.Value.ToArray();
                    //foreach (var pLine in aBorderLine)
                    //    fWholeLength += pLine.m_fLength;

                    //fWholeLength /= pInnerTerritory.PerimeterLength;

                    //if (fWholeLength < 0.15f && m_cContents.Count > 1)
                    //    continue;
                    //if (fWholeLength < 0.25f)
                    //    fWholeLength /= 10;
                    //if (fWholeLength > 0.5f)
                    //    fWholeLength *= 10; 
                    
                    if (weight > 0)
                        cBorderLength[pInnerTerritory] = weight;// 0;
                }
            }

            BaseTerritory pAddon = null;

            int iChoice = Rnd.ChooseOne(cBorderLength.Values, 4);
            foreach (var pInner in cBorderLength)
            {
                iChoice--;
                if (iChoice < 0)
                {
                    pAddon = pInner.Key;
                    break;
                }
            }

            if (pAddon == null)
                return null;

            m_cContents.Add(pAddon);
            pAddon.Owner = this;

            m_cBorder[pAddon].Clear();
            m_cBorder.Remove(pAddon);

            foreach (var pInner in pAddon.BorderWith)
            {
                if (m_cContents.Contains(pInner.Key))
                    continue;

                if (!m_cBorder.ContainsKey(pInner.Key))
                    m_cBorder[pInner.Key] = new List<Location.Edge>();
                Location.Edge[] aBorderLine = pInner.Value.ToArray();
                foreach (var pLine in aBorderLine)
                    m_cBorder[pInner.Key].Add(new Location.Edge(pLine)); 
            }

            //ChainBorder();

            return pAddon;
        }

        public List<Location.Edge> m_cFirstLines = new List<Location.Edge>();

        public readonly List<List<VoronoiVertex>> m_cOrdered = new List<List<VoronoiVertex>>();

        /// <summary>
        /// Заполняет словарь границ с другими землями.
        /// </summary>
        /// <param name="fCycleShift">Величина смещения X-координаты для закольцованной карты</param>
        public virtual void Finish(float fCycleShift)
        {
            ChainBorder(fCycleShift);

            foreach (BaseTerritory pInner in m_cBorder.Keys)
            {
                Territory pBorder = pInner.Owner as Territory;

                if (pBorder == null)
                    pBorder = Territory.m_pForbidden;

                if (!BorderWith.ContainsKey(pBorder))
                    BorderWith[pBorder] = new List<Location.Edge>();
                BorderWith[pBorder].AddRange(m_cBorder[pInner]);
            }
            FillBorderWithKeys();
        }

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

                X += pLine.m_fLength * (fRelativeX1 + fRelativeX2) / 2;
                Y += pLine.m_fLength * (pLine.m_pPoint1.Y + pLine.m_pPoint2.Y) / 2;
                H += pLine.m_fLength * (pLine.m_pPoint1.H + pLine.m_pPoint2.H) / 2;
                fPerimeter += pLine.m_fLength;
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
