using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;

namespace LandscapeGeneration
{
    public class Area<LAND, LTI> : BorderBuilder<LAND>
        where LAND: class, ITypedLand<LTI>, ITerritory
        where LTI: LandTypeInfo
    {
        public List<LAND> m_cContents = new List<LAND>();

        public LTI m_pType;

        public bool IsWater
        {
            get { return m_pType != null && m_pType.m_eType == EnvironmentType.Water; }
        }

        public Area()
        { }

        private int m_iMaxSize = 1;

        public void Start(LAND pSeed, int iMaxSize)
        {
            m_cContents.Clear();

            base.Start(pSeed);

            m_cContents.Add(pSeed);
            pSeed.Area = this;

            m_pType = pSeed.Type;

            m_iMaxSize = iMaxSize / m_pType.m_iMovementCost;
        }

        /// <summary>
        /// Присоединяет к территории сопредельную землю того же типа.
        /// </summary>
        /// <returns></returns>
        public bool Grow()
        {
            if (m_cContents.Count > m_iMaxSize && Rnd.OneChanceFrom(m_cContents.Count - m_iMaxSize))
                return false;

            //List<LAND> cBorder = new List<LAND>();

            Dictionary<LAND, float> cBorderLength = new Dictionary<LAND, float>();

            object[] aBorderLands = new List<object>(m_cBorder.Keys).ToArray();
            foreach (ITerritory pTerr in aBorderLands)
            {
                if (pTerr.Forbidden)
                    continue;

                LAND pLand = pTerr as LAND;

                if (pLand.Area == null && pLand.Type == m_pType)
                {
                    bool bHavePotential = false;

                    float fWholeLength = 1;
                    Line[] aBorderLine = m_cBorder[pLand].ToArray();
                    foreach (Line pLine in aBorderLine)
                        fWholeLength += pLine.m_fLength;

                    //граница этой земли с окружающими землями
                    float fTotalLength = 0;
                    foreach (var pLinkTerr in pLand.BorderWith)
                    {
                        if ((pLinkTerr.Key as ITerritory).Forbidden)
                            continue;

                        Line[] cLines = pLinkTerr.Value.ToArray();
                        foreach (Line pLine in cLines)
                            fTotalLength += pLine.m_fLength;

                        if ((pLinkTerr.Key as LAND).Type == m_pType &&
                            (pLinkTerr.Key as LAND).Area == null)
                            bHavePotential = true;
                    }

                    fWholeLength /= fTotalLength;

                    if (fWholeLength < 0.25f && m_cContents.Count > 1 && bHavePotential)
                        continue;
                    if (fWholeLength < 0.35f)
                        fWholeLength /= 10;
                    if (fWholeLength > 0.5f)
                        fWholeLength *= 10; 
                    
                    cBorderLength[pLand] = fWholeLength;
                }
            }

            if (cBorderLength.Count == 0)
                return false;

            LAND pAddon = null;

            int iChoice = Rnd.ChooseOne(cBorderLength.Values, 2);
            LAND[] aBorderLength = new List<LAND>(cBorderLength.Keys).ToArray();
            foreach (LAND pInner in aBorderLength)
            {
                iChoice--;
                if (iChoice < 0)
                {
                    pAddon = pInner;
                    break;
                }
            }

            if (pAddon == null)
                return false;

            m_cContents.Add(pAddon);
            pAddon.Area = this;

            m_cBorder[pAddon].Clear();
            m_cBorder.Remove(pAddon);

            foreach (var pBorderLand in pAddon.BorderWith)
            {
                if (m_cContents.Contains(pBorderLand.Key))
                    continue;

                if (!m_cBorder.ContainsKey(pBorderLand.Key))
                    m_cBorder[pBorderLand.Key] = new List<Line>();
                Line[] cLines = pBorderLand.Value.ToArray();
                foreach (Line pLine in cLines)
                    m_cBorder[pBorderLand.Key].Add(new Line(pLine));
            }

            //ChainBorder();

            return true;
        }

        public virtual void Finish(float fCycleShift)
        {
            //base.Finish();
            ChainBorder(fCycleShift);
        }
    
        public override float GetMovementCost()
        {
            return m_pType == null ? 100 : m_pType.m_iMovementCost; 
        }    
    }
}
