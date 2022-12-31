using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;
using NameGen;
using LandscapeGeneration;
using Socium.Nations;

namespace Socium
{
    public class Region: BorderBuilder<LandX>, ITerritory
    {
        private static Region m_pForbidden = new Region(true);

        public List<LandX> m_cContents = new List<LandX>();

        public LandTypeInfoX m_pType;

        public bool IsWater
        {
            get { return m_pType != null && m_pType.m_eEnvironment.HasFlag(LandscapeGeneration.Environment.Liquid); }
        }

        #region ITerritory members
        public Dictionary<object, List<Location.Edge>> BorderWith { get; } = new Dictionary<object, List<Location.Edge>>();

        public bool Forbidden { get; } = false;

        public object Owner { get; set; } = null;

        public float PerimeterLength { get; private set; } = 0;
        #endregion ITerritory members

        public Province m_pProvince = null;

        /// <summary>
        /// Военное присутсвие сил провинции в данной земле.
        /// Нужно только для формирования карты провинций.
        /// </summary>
        public int m_iProvincePresence = 0;

        /// <summary>
        /// коренное население региона - может отличаться от доминирующего населения локации
        /// </summary>
        public Nation m_pNation = null;

        public string m_sName;

        private int m_iMaxSize = 1;

        public Region(bool bForbidden)
        {
            Forbidden = bForbidden;
        }

        public Region()
        { }
        
        public void Start(LandX pSeed, int iMaxSize)
        {
            BorderWith.Clear();
            m_cContents.Clear();

            base.Start(pSeed);

            m_cContents.Add(pSeed);
            pSeed.Region = this;

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

            Dictionary<LandX, float> cBorderLength = new Dictionary<LandX, float>();

            object[] aBorderLands = new List<object>(m_cBorder.Keys).ToArray();
            foreach (ITerritory pTerr in aBorderLands)
            {
                if (pTerr.Forbidden)
                    continue;

                LandX pLand = pTerr as LandX;

                if (pLand.Region == null && pLand.Type == m_pType)
                {
                    bool bHavePotential = false;

                    float fWholeLength = 1;
                    Location.Edge[] aBorderLine = m_cBorder[pLand].ToArray();
                    foreach (var pLine in aBorderLine)
                        fWholeLength += pLine.m_fLength;

                    foreach (var pLinkTerr in pLand.BorderWith)
                    {
                        if ((pLinkTerr.Key as ITerritory).Forbidden)
                            continue;

                        if ((pLinkTerr.Key as LandX).Type == m_pType &&
                            (pLinkTerr.Key as LandX).Region == null)
                            bHavePotential = true;
                    }

                    fWholeLength /= pLand.PerimeterLength;

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

            LandX pAddon = null;

            int iChoice = Rnd.ChooseOne(cBorderLength.Values, 2);
            LandX[] aBorderLength = new List<LandX>(cBorderLength.Keys).ToArray();
            foreach (LandX pInner in aBorderLength)
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
            pAddon.Region = this;

            m_cBorder[pAddon].Clear();
            m_cBorder.Remove(pAddon);

            foreach (var pBorderLand in pAddon.BorderWith)
            {
                if (m_cContents.Contains(pBorderLand.Key))
                    continue;

                if (!m_cBorder.ContainsKey(pBorderLand.Key))
                    m_cBorder[pBorderLand.Key] = new List<Location.Edge>();
                Location.Edge[] cLines = pBorderLand.Value.ToArray();
                foreach (var pLine in cLines)
                    m_cBorder[pBorderLand.Key].Add(new Location.Edge(pLine));
            }

            //ChainBorder();

            return true;
        }

        public virtual void Finish(float fCycleShift)
        {
            //base.Finish();
            ChainBorder(fCycleShift);

            foreach (ITerritory pLand in m_cBorder.Keys)
            {
                Region pRegion;
                if (pLand.Forbidden || (pLand as LandX).m_pProvince == null)
                    pRegion = Region.m_pForbidden;
                else
                    pRegion = (pLand as LandX).Region;

                if (!BorderWith.ContainsKey(pRegion))
                    BorderWith[pRegion] = new List<Location.Edge>();
                BorderWith[pRegion].AddRange(m_cBorder[pLand]);
            }
            FillBorderWithKeys();
        }

        public object[] m_aBorderWith = null;

        internal void FillBorderWithKeys()
        {
            m_aBorderWith = new List<object>(BorderWith.Keys).ToArray();

            PerimeterLength = 0;
            foreach (var pBorder in BorderWith)
                foreach (var pLine in pBorder.Value)
                    PerimeterLength += pLine.m_fLength;
        }

        public override float GetMovementCost()
        {
            return m_pType == null ? 100 : m_pType.m_iMovementCost;
        }
        public void SetRace(List<Nation> cPossibleNations)
        {
            Dictionary<Nation, float> cChances = new Dictionary<Nation, float>();
            foreach (Nation pNation in cPossibleNations)
            {
                cChances[pNation] = 1.0f;// / pRace.m_iRank;

                if (pNation.m_bDying)
                    cChances[pNation] /= 10 / m_pType.m_iMovementCost;
                    //cChances[pRace] /= 10000 / (m_pType.m_iMovementCost * m_pType.m_iMovementCost);

                if (pNation.m_bHegemon)
                    cChances[pNation] *= 10;

                foreach (LandTypeInfoX pType in pNation.m_aPreferredLands)
                    if (m_pType == pType)
                        cChances[pNation] *= 10;

                foreach (LandTypeInfoX pType in pNation.m_aHatedLands)
                    if (m_pType == pType)
                        cChances[pNation] /= 100;
            }

            int iChance = Rnd.ChooseOne(cChances.Values, 3);
            foreach (Nation pNation in cChances.Keys)
            {
                iChance--;
                if (iChance < 0)
                {
                    m_pNation = pNation;
                    break;
                }
            }

            m_sName = m_pNation.m_pRace.m_pLanguage.RandomCountryName();

            foreach (LandX pLand in m_cContents)
            {
                pLand.m_sName = m_sName;
                pLand.m_pNation = m_pNation;
            }
        }

        public string GetNativeRaceString()
        {
            if (m_pNation == null)
                return "unpopulated";
            else
                return m_pNation.m_pProtoSociety.m_sName;
        }

        public override string ToString()
        {
            return string.Format("{0} {1} ({2}, {3})", m_sName, m_pType.m_sName, m_cContents.Count, GetNativeRaceString());
        }
    }
}
