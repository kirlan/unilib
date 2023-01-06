using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LandscapeGeneration.PathFind;
using Random;

namespace LandscapeGeneration
{
    public class TerritoryCluster<LAYER, INNER> : Territory, IInfoLayer
        where LAYER: TerritoryCluster<LAYER, INNER>
        where INNER: Territory
    {
        #region BorderBuilder
        /// <summary>
        /// границы с другими объектами НИЗШЕГО УРОВНЯ, т.е. составными частями данного объекта
        /// </summary>
        public Dictionary<Territory, List<VoronoiEdge>> m_cBorder = new Dictionary<Territory, List<VoronoiEdge>>();

        public void InitBorder(INNER pSeed)
        {
            m_cBorder.Clear();

            foreach (var pInner in pSeed.BorderWith)
            {
                m_cBorder[pInner.Key] = new List<VoronoiEdge>();
                VoronoiEdge[] aLines = pInner.Value.ToArray();
                foreach (VoronoiEdge pLine in aLines)
                    m_cBorder[pInner.Key].Add(new VoronoiEdge(pLine));
            }

            //ChainBorder();
        }
        public List<VoronoiEdge> m_cFirstLines = new List<VoronoiEdge>();

        public bool TestChain()
        {
            List<VoronoiEdge> cTotalBorder = new List<VoronoiEdge>();
            foreach (var cLines in m_cBorder)
                cTotalBorder.AddRange(cLines.Value);

            VoronoiEdge[] aTotalBorder = cTotalBorder.ToArray();
            int iTotalCount = aTotalBorder.Length;

            for (int i = 0; i < iTotalCount; i++)
            {
                bool bGot1 = false;
                bool bGot2 = false;

                VoronoiEdge pCurrentLine = aTotalBorder[i];
                for (int j = 0; j < iTotalCount; j++)
                {
                    if (i == j)
                        continue;

                    VoronoiEdge pLine = aTotalBorder[j];
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
            List<VoronoiEdge> cTotalBorder = new List<VoronoiEdge>();
            foreach (var cLines in m_cBorder)
                cTotalBorder.AddRange(cLines.Value);

            VoronoiEdge[] aTotalBorder = cTotalBorder.ToArray();
            int iTotalCount = aTotalBorder.Length;

            X = 0;
            Y = 0;
            H = 0;
            float fPerimeter = 0;
            foreach (VoronoiEdge pLine in aTotalBorder)
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

            VoronoiEdge pFirstLine;
            do
            {
                pFirstLine = null;
                for (int i = 0; i < iTotalCount; i++)
                {
                    VoronoiEdge pLine = aTotalBorder[i];
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
                    VoronoiEdge pCurrentLine = pFirstLine;

                    bool bGotIt;
                    int iCounter = 0;
                    do
                    {
                        bGotIt = false;
                        for (int i = 0; i < iTotalCount; i++)
                        {
                            VoronoiEdge pLine = aTotalBorder[i];
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
        #endregion

        private static TerritoryCluster<LAYER, INNER> m_pForbidden = new TerritoryCluster<LAYER, INNER>(true);

        /// <summary>
        /// Локации, составляющие территорию
        /// </summary>
        public HashSet<INNER> Contents { get; } = new HashSet<INNER>();

        public TerritoryCluster(bool bForbidden): base(bForbidden)
        {
        }

        public TerritoryCluster()
        {}

        //public Location Owner { get; set; } = null;


        /// <summary>
        /// Соседние ТАКИЕ ЖЕ объекты
        /// </summary>
        public Territory[] m_aBorderWith = null;

        protected void FillBorderWithKeys()
        {
            m_aBorderWith = new List<Territory>(BorderWith.Keys).ToArray();

            PerimeterLength = 0;
            foreach (var pBorder in BorderWith)
                foreach (var pLine in pBorder.Value)
                    PerimeterLength += pLine.Length;
        }

        public virtual void Start(INNER pSeed)
        {
            Contents.Clear();
            BorderWith.Clear();

            InitBorder(pSeed);

            Contents.Add(pSeed);
        }

        /// <summary>
        /// Присоединяет к земле сопредельную нечейную локацию.
        /// Чем длиннее общая граница с локацией - тем выше вероятность того, что выбрана будет именно она.
        /// </summary>
        /// <returns></returns>
        public Territory Grow()
        {
            return Grow(int.MaxValue);
        }

        /// <summary>
        /// Присоединяет к земле сопредельную нечейную локацию.
        /// Чем длиннее общая граница с локацией - тем выше вероятность того, что выбрана будет именно она.
        /// </summary>
        /// <returns></returns>
        public virtual Territory Grow(int iMaxSize)
        {
            //если территория уже достаточно большая - игнорируем.
            if (Contents.Count > iMaxSize)
                return null;

            Dictionary<Territory, float> cBorderLength = new Dictionary<Territory, float>();

            foreach (var pInner in m_cBorder)
            {
                Territory pInnerTerritory = pInner.Key as Territory;
                if (!pInnerTerritory.HasLayer<LAYER>() && !pInnerTerritory.Forbidden)
                {
                    float fWholeLength = 1;
                    VoronoiEdge[] aBorderLine = pInner.Value.ToArray();
                    foreach (var pLine in aBorderLine)
                        fWholeLength += pLine.Length;

                    fWholeLength /= pInnerTerritory.PerimeterLength;

                    if (fWholeLength < 0.15f && Contents.Count > 1)
                        continue;
                    if (fWholeLength < 0.25f)
                        fWholeLength /= 10;
                    if (fWholeLength > 0.5f)
                        fWholeLength *= 10; 
                    
                    cBorderLength[pInnerTerritory] = fWholeLength;// 0;
                    //Line[] aBorderLine = m_cBorder[pInner].ToArray();
                    //foreach (Line pLine in aBorderLine)
                    //    cBorderLength[pInner] += pLine.m_fLength;
                }
            }

            Territory pAddon = null;

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

            Contents.Add(pAddon as INNER);
            pAddon.AddLayer((LAYER) this);

            m_cBorder[pAddon].Clear();
            m_cBorder.Remove(pAddon);

            foreach (var pInner in pAddon.BorderWith)
            {
                if (Contents.Contains(pInner.Key as INNER))
                    continue;

                if (!m_cBorder.ContainsKey(pInner.Key))
                    m_cBorder[pInner.Key] = new List<VoronoiEdge>();
                VoronoiEdge[] aBorderLine = pInner.Value.ToArray();
                foreach (var pLine in aBorderLine)
                    m_cBorder[pInner.Key].Add(new VoronoiEdge(pLine)); 
            }

            //ChainBorder();

            return pAddon;
        }

        /// <summary>
        /// Заполняет словарь границ с другими землями.
        /// </summary>
        /// <param name="fCycleShift">Величина смещения X-координаты для закольцованной карты</param>
        public virtual void Finish(float fCycleShift)
        {
            ChainBorder(fCycleShift);

            foreach (Territory pInner in m_cBorder.Keys)
            {
                TerritoryCluster<LAYER, INNER> pBorderCluster = TerritoryCluster<LAYER, INNER>.m_pForbidden;
                if (pInner.HasLayer<LAYER>())
                    pBorderCluster = pInner.GetLayer<LAYER>();

                if (!BorderWith.ContainsKey(pBorderCluster))
                    BorderWith[pBorderCluster] = new List<VoronoiEdge>();
                BorderWith[pBorderCluster].AddRange(m_cBorder[pInner]);
            }
            FillBorderWithKeys();
        }

        public override float GetMovementCost()
        {
            return 1000;
        }
    }
}
