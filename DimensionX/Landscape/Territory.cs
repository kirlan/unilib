using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LandscapeGeneration.PathFind;
using Random;

namespace LandscapeGeneration
{
    public class Territory<INNER> : TransportationNode, ITerritory
        where INNER: class, ITerritory
    {
        #region BorderBuilder
        /// <summary>
        /// границы с другими объектами НИЗШЕГО УРОВНЯ, т.е. составными частями данного объекта
        /// </summary>
        public Dictionary<ITerritory, List<Location.Edge>> m_cBorder = new Dictionary<ITerritory, List<Location.Edge>>();

        public void InitBorder(INNER pSeed)
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
        #endregion

        private bool m_bForbidden = false;

        private static Territory<INNER> m_pForbidden = new Territory<INNER>(true);

        public bool Forbidden
        {
            get { return m_bForbidden; /*this == Territory<INNER>.m_pForbidden;*/ }
        }

        /// <summary>
        /// Локации, составляющие территорию
        /// </summary>
        public HashSet<INNER> m_cContents = new HashSet<INNER>();

        /// <summary>
        /// Границы с другими такими же объектами
        /// </summary>
        private Dictionary<ITerritory, List<Location.Edge>> m_cBorderWith = new Dictionary<ITerritory, List<Location.Edge>>();

        private ITerritory m_pOwner = null;

        public Territory(bool bForbidden)
        {
            m_bForbidden = bForbidden;
        }

        public Territory()
        {}

        public ITerritory Owner
        {
            get { return m_pOwner; }
            set { m_pOwner = value; }
        }

        /// <summary>
        /// Границы с другими такими же объектами
        /// </summary>
        public Dictionary<ITerritory, List<Location.Edge>> BorderWith
        {
            get { return m_cBorderWith; }
        }

        public ITerritory[] m_aBorderWith = null;

        /// <summary>
        /// Суммарная длина всех линий в BorderWith
        /// </summary>
        private float m_fPerimeter = 0;

        public float PerimeterLength
        {
            get { return m_fPerimeter; }
        }

        protected void FillBorderWithKeys()
        {
            m_aBorderWith = new List<ITerritory>(m_cBorderWith.Keys).ToArray();

            m_fPerimeter = 0;
            foreach (var pBorder in m_cBorderWith)
                foreach (var pLine in pBorder.Value)
                    m_fPerimeter += pLine.m_fLength;
        }

        public virtual void Start(INNER pSeed)
        {
            m_cContents.Clear();
            m_cBorderWith.Clear();

            InitBorder(pSeed);

            m_cContents.Add(pSeed);
            pSeed.Owner = this;
        }

        /// <summary>
        /// Присоединяет к земле сопредельную нечейную локацию.
        /// Чем длиннее общая граница с локацией - тем выше вероятность того, что выбрана будет именно она.
        /// </summary>
        /// <returns></returns>
        public ITerritory Grow()
        {
            return Grow(int.MaxValue);
        }

        /// <summary>
        /// Присоединяет к земле сопредельную нечейную локацию.
        /// Чем длиннее общая граница с локацией - тем выше вероятность того, что выбрана будет именно она.
        /// </summary>
        /// <returns></returns>
        public virtual ITerritory Grow(int iMaxSize)
        {
            //если территория уже достаточно большая - игнорируем.
            if (m_cContents.Count > iMaxSize)
                return null;

            Dictionary<ITerritory, float> cBorderLength = new Dictionary<ITerritory, float>();

            foreach (var pInner in m_cBorder)
            {
                ITerritory pInnerTerritory = pInner.Key as ITerritory;
                if (pInnerTerritory.Owner == null && !pInnerTerritory.Forbidden)
                {
                    float fWholeLength = 1;
                    Location.Edge[] aBorderLine = pInner.Value.ToArray();
                    foreach (var pLine in aBorderLine)
                        fWholeLength += pLine.m_fLength;

                    fWholeLength /= pInnerTerritory.PerimeterLength;

                    if (fWholeLength < 0.15f && m_cContents.Count > 1)
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

            ITerritory pAddon = null;

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

            m_cContents.Add(pAddon as INNER);
            pAddon.Owner = this;

            m_cBorder[pAddon].Clear();
            m_cBorder.Remove(pAddon);

            foreach (var pInner in pAddon.BorderWith)
            {
                if (m_cContents.Contains(pInner.Key as INNER))
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

        /// <summary>
        /// Заполняет словарь границ с другими землями.
        /// </summary>
        /// <param name="fCycleShift">Величина смещения X-координаты для закольцованной карты</param>
        public virtual void Finish(float fCycleShift)
        {
            ChainBorder(fCycleShift);

            foreach (ITerritory pInner in m_cBorder.Keys)
            {
                Territory<INNER> pBorder = pInner.Owner as Territory<INNER>;

                if (pBorder == null)
                    pBorder = Territory<INNER>.m_pForbidden;

                if (!m_cBorderWith.ContainsKey(pBorder))
                    m_cBorderWith[pBorder] = new List<Location.Edge>();
                m_cBorderWith[pBorder].AddRange(m_cBorder[pInner]);
            }
            FillBorderWithKeys();
        }

        public override float GetMovementCost()
        {
            return 1000;
        }
    }
}
