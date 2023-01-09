using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LandscapeGeneration.PathFind;
using Random;

namespace LandscapeGeneration
{
    /// <summary>
    /// Группа сопредельных <typeparamref name="INNER"/>, известная как <typeparamref name="LAYER"/> и являющаяся частью <typeparamref name="OWNER"/>
    /// </summary>
    /// <typeparam name="LAYER">Имя класса, содержащего всю логику</typeparam>
    /// <typeparam name="OWNER">Владеющая территория</typeparam>
    /// <typeparam name="INNER">Составляющие территории</typeparam>
    public abstract class TerritoryCluster<LAYER, OWNER, INNER> : TerritoryOf<LAYER, OWNER>
        where LAYER: TerritoryCluster<LAYER, OWNER, INNER>, new()
        where OWNER: class, IInfoLayer
        where INNER: TerritoryOf<INNER, LAYER>, new()
    {
        #region BorderBuilder
        /// <summary>
        /// границы с другими <typeparamref name="INNER"/>, т.е. составными частями соседних <typeparamref name="LAYER"/>
        /// </summary>
        public Dictionary<INNER, List<VoronoiEdge>> Border { get; } = new Dictionary<INNER, List<VoronoiEdge>>();

        public void InitBorder(INNER pSeed)
        {
            Border.Clear();

            foreach (var pInner in pSeed.BorderWith)
            {
                Border[pInner.Key] = new List<VoronoiEdge>();
                foreach (VoronoiEdge pLine in pInner.Value.ToArray())
                    Border[pInner.Key].Add(new VoronoiEdge(pLine));
            }
        }

        public List<VoronoiEdge> FirstLines { get; } = new List<VoronoiEdge>();

        /*
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
        */

        public List<List<VoronoiVertex>> BorderOrdered { get; } = new List<List<VoronoiVertex>>();

        /// <summary>
        /// Настраивает связи "следующая"-"предыдущая" среди граней, уже хранящихся в словаре границ с другими локациями.
        /// </summary>
        /// <param name="fCycleShift">Величина смещения X-координаты для закольцованной карты</param>
        protected void ChainBorder(float fCycleShift)
        {
            List<VoronoiEdge> cTotalBorder = new List<VoronoiEdge>();
            foreach (var cLines in Border)
                cTotalBorder.AddRange(cLines.Value);

            VoronoiEdge[] aTotalBorder = cTotalBorder.ToArray();
            int iTotalCount = aTotalBorder.Length;

            X = 0;
            Y = 0;
            H = 0;
            float fPerimeter = 0;
            foreach (VoronoiEdge pLine in aTotalBorder)
            {
                pLine.Next = null;

                float fRelativeX1 = pLine.Point1.X;
                if (fRelativeX1 > aTotalBorder[0].Point1.X + fCycleShift / 2)
                    fRelativeX1 -= fCycleShift;
                if (fRelativeX1 < aTotalBorder[0].Point1.X - fCycleShift / 2)
                    fRelativeX1 += fCycleShift;

                float fRelativeX2 = pLine.Point2.X;
                if (fRelativeX2 > aTotalBorder[0].Point1.X + fCycleShift / 2)
                    fRelativeX2 -= fCycleShift;
                if (fRelativeX2 < aTotalBorder[0].Point1.X - fCycleShift / 2)
                    fRelativeX2 += fCycleShift;

                X += pLine.Length * (fRelativeX1 + fRelativeX2) / 2;
                Y += pLine.Length * (pLine.Point1.Y + pLine.Point2.Y) / 2;
                H += pLine.Length * (pLine.Point1.H + pLine.Point2.H) / 2;
                fPerimeter += pLine.Length;
            }
            X /= fPerimeter;
            Y /= fPerimeter;
            H /= fPerimeter;

            FirstLines.Clear();
            BorderOrdered.Clear();

            VoronoiEdge pFirstLine;
            do
            {
                pFirstLine = null;
                for (int i = 0; i < iTotalCount; i++)
                {
                    VoronoiEdge pLine = aTotalBorder[i];
                    if (pLine.Next == null)// && pLine.m_pPrevious == null)
                    {
                        pFirstLine = pLine;
                        break;
                    }
                }

                if (pFirstLine != null)
                {
                    List<VoronoiVertex> cVertexes = new List<VoronoiVertex>();

                    FirstLines.Add(pFirstLine);
                    VoronoiEdge pCurrentLine = pFirstLine;

                    bool bGotIt;
                    int iCounter = 0;
                    do
                    {
                        bGotIt = false;
                        for (int i = 0; i < iTotalCount; i++)
                        {
                            VoronoiEdge pLine = aTotalBorder[i];
                            if (pLine.Point1 == pCurrentLine.Point2 ||
                                (pLine.Point1.Y == pCurrentLine.Point2.Y &&
                                 (pLine.Point1.X == pCurrentLine.Point2.X ||
                                  Math.Abs(pLine.Point1.X - pCurrentLine.Point2.X) == fCycleShift)))
                            {
                                pCurrentLine.Next = pLine;
                                cVertexes.Add(pCurrentLine.Point1);

                                pCurrentLine = pLine;
                                bGotIt = true;
                                iCounter++;
                                break;
                            }
                        }
                        if (!bGotIt)
                        {
                            throw new InvalidOperationException("Can't chain the border!");
                        }
                    }
                    while (pCurrentLine != pFirstLine && bGotIt && iCounter <= iTotalCount);

                    BorderOrdered.Add(cVertexes);
                }
            }
            while (pFirstLine != null);
        }
        #endregion

        /// <summary>
        /// Список <typeparamref name="INNER"/>, составляющих <typeparamref name="LAYER"/>
        /// </summary>
        public HashSet<INNER> Contents { get; } = new HashSet<INNER>();

        /// <summary>
        /// Соседние объекты <typeparamref name="LAYER"/>.<br/>
        /// Нельзя использовать до вызова <see cref="FillBorderWithKeys()"/> в <see cref="Finish(float)"/>
        /// </summary>
        public LAYER[] BorderWithKeys { get; set; } = null;

        protected void FillBorderWithKeys()
        {
            BorderWithKeys = new List<LAYER>(BorderWith.Keys).ToArray();

            PerimeterLength = 0;
            foreach (var pBorder in BorderWith)
                foreach (var pLine in pBorder.Value)
                    PerimeterLength += pLine.Length;
        }

        /// <summary>
        /// Стартует новый <typeparamref name="LAYER"/>, добавляя в него указанную <typeparamref name="INNER"/>.<br/>
        /// Заполняет <c>m_cBorder</c>.<br/><c>BorderWith</c> будет заполнен ТОЛЬКО после вызова <c>Finish(float)</c>
        /// </summary>
        /// <param name="pSeed"></param>
        public virtual void Start(INNER pSeed)
        {
            Contents.Clear();
            BorderWith.Clear();

            InitBorder(pSeed);

            Contents.Add(pSeed);
        }

        /// <summary>
        /// Присоединяет к <typeparamref name="LAYER"/> сопредельную ничейную <typeparamref name="INNER"/>.
        /// Чем длиннее общая граница с <typeparamref name="INNER"/> - тем выше вероятность того, что выбрана будет именно она.
        /// </summary>
        /// <returns></returns>
        public INNER Grow()
        {
            return Grow(int.MaxValue);
        }

        /// <summary>
        /// Присоединяет к <typeparamref name="LAYER"/> сопредельную нечейную <typeparamref name="INNER"/>.
        /// Чем длиннее общая граница с <typeparamref name="INNER"/> - тем выше вероятность того, что выбрана будет именно она.
        /// </summary>
        /// <returns></returns>
        public virtual INNER Grow(int iMaxSize)
        {
            //если территория уже достаточно большая - игнорируем.
            if (Contents.Count > iMaxSize)
                return null;

            Dictionary<INNER, float> cBorderLength = new Dictionary<INNER, float>();

            foreach (var pInner in Border)
            {
                INNER pInnerTerritory = pInner.Key;
                if (!pInnerTerritory.HasOwner() && !pInnerTerritory.Forbidden)
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

                    cBorderLength[pInnerTerritory] = fWholeLength;
                }
            }

            INNER pAddon = null;

            int iChoice = Rnd.ChooseOne(cBorderLength.Values, 4);
            if (iChoice >= 0)
                pAddon = cBorderLength.ElementAt(iChoice).Key;

            if (pAddon == null)
                return null;

            Contents.Add(pAddon);
            pAddon.SetOwner((LAYER)this);

            Border[pAddon].Clear();
            Border.Remove(pAddon);

            foreach (var pInner in pAddon.BorderWith)
            {
                if (Contents.Contains(pInner.Key))
                    continue;

                if (!Border.ContainsKey(pInner.Key))
                    Border[pInner.Key] = new List<VoronoiEdge>();
                VoronoiEdge[] aBorderLine = pInner.Value.ToArray();
                foreach (var pLine in aBorderLine)
                    Border[pInner.Key].Add(new VoronoiEdge(pLine));
            }

            return pAddon;
        }

        /// <summary>
        /// Заполняет словари границ с другими <typeparamref name="INNER"/> (т.е. <c>BorderWith</c> и <c>m_aBorderWith</c>).
        /// </summary>
        /// <param name="fCycleShift">Величина смещения X-координаты для закольцованной карты</param>
        public virtual void Finish(float fCycleShift)
        {
            ChainBorder(fCycleShift);

            foreach (INNER pInner in Border.Keys)
            {
                LAYER pBorderCluster = m_pForbidden;
                if (pInner.HasOwner())
                    pBorderCluster = pInner.GetOwner();

                if (!BorderWith.ContainsKey(pBorderCluster))
                    BorderWith[pBorderCluster] = new List<VoronoiEdge>();
                BorderWith[pBorderCluster].AddRange(Border[pInner]);
            }
            FillBorderWithKeys();
        }

        public override float GetMovementCost()
        {
            return 1000;
        }
    }
}
