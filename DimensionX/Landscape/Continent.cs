using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;

namespace LandscapeGeneration
{
    public class Continent<LAND, LTI> : Territory<LandMass<LAND>>
        where LAND: class, ITypedLand<LTI>, ITerritory
        where LTI: LandTypeInfo
    {
        private bool m_bIsland = false;

        public override void  Start(LandMass<LAND> pSeed)
        {
            if (pSeed.IsWater)
                return;

 	        base.Start(pSeed);

            m_bIsland = pSeed.m_iMaxSize > 0;
        }

        /// <summary>
        /// Присоединяет к континенту сопредельную неприкаянную страну.
        /// </summary>
        /// <returns></returns>
        public override object Grow()
        {
            if (m_bIsland && !Rnd.OneChanceFrom(25))
                return null;

            Dictionary<LandMass<LAND>, float> cBorderLength = new Dictionary<LandMass<LAND>, float>();

            foreach (var pLandMass in m_cBorder)
            {
                if ((pLandMass.Key as ITerritory).Forbidden)
                    continue;

                LandMass<LAND> pLM = pLandMass.Key as LandMass<LAND>;

                if (pLM.Owner == null && !pLM.IsWater)
                {
                    bool bFree = true;
                    foreach (ITerritory pLink in pLM.BorderWith.Keys)
                    {
                        if (pLink.Owner != null && pLink.Owner != this)
                            bFree = false;
                    }
                    if (bFree)
                    {
                        cBorderLength[pLM] = 0;
                        foreach (var pLine in pLandMass.Value)
                            cBorderLength[pLM] += pLine.m_fLength;
                    }
                }
            }


            if (cBorderLength.Count == 0)
                return null;

            LandMass<LAND> pAddon = null;
            
            int iChoice = Rnd.ChooseOne(cBorderLength.Values, 2);
            if (iChoice == -1)
                return null;

            foreach (var pInner in cBorderLength)
            {
                iChoice--;
                if (iChoice < 0)
                {
                    pAddon = pInner.Key;
                    break;
                }
            }

            m_cContents.Add(pAddon);
            pAddon.Owner = this;

            m_cBorder[pAddon].Clear();
            m_cBorder.Remove(pAddon);

            foreach (var pLandMass in pAddon.BorderWith)
            {
                if (!(pLandMass.Key as ITerritory).Forbidden && m_cContents.Contains(pLandMass.Key as LandMass<LAND>))
                    continue;

                if (!m_cBorder.ContainsKey(pLandMass.Key))
                    m_cBorder[pLandMass.Key] = new List<Location.Edge>();
                foreach (var pLine in pLandMass.Value)
                    m_cBorder[pLandMass.Key].Add(new Location.Edge(pLine));
            }

            //ChainBorder();

            return pAddon;
        }
    }
}
