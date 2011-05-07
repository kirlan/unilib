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

            List<LAND> cBorder = new List<LAND>();

            object[] aBorderLands = new List<object>(m_cBorder.Keys).ToArray();
            foreach (ITerritory pTerr in aBorderLands)
            {
                if (pTerr.Forbidden)
                    continue;

                LAND pLand = pTerr as LAND;

                if (pLand.Area == null && pLand.Type == m_pType)
                    cBorder.Add(pLand);
            }

            if (cBorder.Count == 0)
                return false;

            int iChoice = Rnd.Get(cBorder.Count);
            LAND pAddon = cBorder[iChoice];

            m_cContents.Add(pAddon);
            pAddon.Area = this;

            m_cBorder[pAddon].Clear();
            m_cBorder.Remove(pAddon);

            foreach (var pLand in pAddon.BorderWith)
            {
                if (m_cContents.Contains(pLand.Key))
                    continue;

                if (!m_cBorder.ContainsKey(pLand.Key))
                    m_cBorder[pLand.Key] = new List<Line>();
                Line[] cLines = pLand.Value.ToArray();
                foreach (Line pLine in cLines)
                    m_cBorder[pLand.Key].Add(new Line(pLine));
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
