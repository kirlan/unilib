﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;

namespace LandscapeGeneration
{
    public class Territory<INNER> : BorderBuilder<INNER>, ITerritory
        where INNER: class, ITerritory
    {
        private bool m_bForbidden = false;

        private static Territory<INNER> m_pForbidden = new Territory<INNER>(true);

        public bool Forbidden
        {
            get { return m_bForbidden; /*this == Territory<INNER>.m_pForbidden;*/ }
        }

        public List<INNER> m_cContents = new List<INNER>();

        private Dictionary<object, List<Line>> m_cBorderWith = new Dictionary<object, List<Line>>();

        private object m_pOwner = null;

        public Territory(bool bForbidden)
        {
            m_bForbidden = bForbidden;
        }

        public Territory()
        {}

        public object Owner
        {
            get { return m_pOwner; }
            set { m_pOwner = value; }
        }

        /// <summary>
        /// Границы с другими такими же объектами
        /// </summary>
        public Dictionary<object, List<Line>> BorderWith
        {
            get { return m_cBorderWith; }
        }

        public object[] m_aBorderWith = null;

        internal void FillBorderWithKeys()
        {
            m_aBorderWith = new List<object>(m_cBorderWith.Keys).ToArray();
        }

        public override void Start(INNER pSeed)
        {
            m_cContents.Clear();
            m_cBorderWith.Clear();

            base.Start(pSeed);

            m_cContents.Add(pSeed);
            pSeed.Owner = this;
        }

        /// <summary>
        /// Присоединяет к земле сопредельную нечейную локацию.
        /// Чем длиннее общая граница с локацией - тем выше вероятность того, что выбрана будет именно она.
        /// </summary>
        /// <returns></returns>
        public virtual object Grow()
        {
            Dictionary<ITerritory, float> cBorderLength = new Dictionary<ITerritory, float>();

            object[] aBorder = new List<object>(m_cBorder.Keys).ToArray();

            foreach (ITerritory pInner in aBorder)
            {
                if (pInner.Owner == null && !pInner.Forbidden)
                {
                    float fWholeLength = 1;
                    Line[] aBorderLine = m_cBorder[pInner].ToArray();
                    foreach (Line pLine in aBorderLine)
                        fWholeLength += pLine.m_fLength;

                    //граница этой земли с окружающими землями
                    float fTotalLength = 0;
                    foreach (var pLinkTerr in pInner.BorderWith)
                    {
                        if ((pLinkTerr.Key as ITerritory).Forbidden)
                            continue;

                        Line[] cLines = pLinkTerr.Value.ToArray();
                        foreach (Line pLine in cLines)
                            fTotalLength += pLine.m_fLength;
                    }

                    fWholeLength /= fTotalLength;

                    if (fWholeLength < 0.25f)
                        fWholeLength /= 10;
                    if (fWholeLength > 0.5f)
                        fWholeLength *= 10; 
                    
                    cBorderLength[pInner] = fWholeLength;// 0;
                    //Line[] aBorderLine = m_cBorder[pInner].ToArray();
                    //foreach (Line pLine in aBorderLine)
                    //    cBorderLength[pInner] += pLine.m_fLength;
                }
            }

            ITerritory pAddon = null;

            int iChoice = Rnd.ChooseOne(cBorderLength.Values, 2);
            ITerritory[] aBorderLength = new List<ITerritory>(cBorderLength.Keys).ToArray();
            foreach (ITerritory pInner in aBorderLength)
            {
                iChoice--;
                if (iChoice < 0)
                {
                    pAddon = pInner;
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
                    m_cBorder[pInner.Key] = new List<Line>();
                Line[] aBorderLine = pInner.Value.ToArray();
                foreach (Line pLine in aBorderLine)
                    m_cBorder[pInner.Key].Add(new Line(pLine)); 
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
                    m_cBorderWith[pBorder] = new List<Line>();
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