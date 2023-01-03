using System;
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

        /// <summary>
        /// Локации, составляющие территорию
        /// </summary>
        public List<INNER> m_cContents = new List<INNER>();

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

        internal void FillBorderWithKeys()
        {
            m_aBorderWith = new List<ITerritory>(m_cBorderWith.Keys).ToArray();

            m_fPerimeter = 0;
            foreach (var pBorder in m_cBorderWith)
                foreach (var pLine in pBorder.Value)
                    m_fPerimeter += pLine.m_fLength;
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
        public virtual ITerritory Grow()
        {
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
