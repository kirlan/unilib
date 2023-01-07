using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;

namespace LandscapeGeneration
{
    /// <summary>
    /// Континент - группа сопредельных надводных тектонических плит.
    /// </summary>
    public class Continent : TerritoryCluster<Continent, LandMass>
    {
        private bool m_bIsland = false;

        public override void Start(LandMass pSeed)
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
        public override Territory Grow(int iMaxSize)
        {
            if (m_bIsland && !Rnd.OneChanceFrom(25))
                return null;

            Dictionary<LandMass, float> cBorderLength = new Dictionary<LandMass, float>();

            foreach (var pLandMass in m_cBorder)
            {
                if (pLandMass.Key.Forbidden)
                    continue;

                LandMass pLM = pLandMass.Key as LandMass;

                if (!pLM.HasOwner<Continent>() && !pLM.IsWater)
                {
                    bool bFree = true;
                    foreach (Territory pLink in pLM.BorderWith.Keys)
                    {
                        if (pLink.HasOwner<Continent>() && pLink.GetOwner<Continent>() != this)
                            bFree = false;
                    }
                    if (bFree)
                    {
                        cBorderLength[pLM] = 0;
                        foreach (var pLine in pLandMass.Value)
                            cBorderLength[pLM] += pLine.Length;
                    }
                }
            }


            if (cBorderLength.Count == 0)
                return null;

            int iChoice = Rnd.ChooseOne(cBorderLength.Values, 2);
            if (iChoice == -1)
                return null;

            LandMass pAddon = cBorderLength.ElementAt(iChoice).Key;

            Contents.Add(pAddon);
            pAddon.SetOwner(this);

            m_cBorder[pAddon].Clear();
            m_cBorder.Remove(pAddon);

            foreach (var pLandMass in pAddon.BorderWith)
            {
                if (!pLandMass.Key.Forbidden && Contents.Contains(pLandMass.Key as LandMass))
                    continue;

                if (!m_cBorder.ContainsKey(pLandMass.Key))
                    m_cBorder[pLandMass.Key] = new List<VoronoiEdge>();
                foreach (var pLine in pLandMass.Value)
                    m_cBorder[pLandMass.Key].Add(new VoronoiEdge(pLine));
            }

            //ChainBorder();

            return pAddon;
        }
    }
}
