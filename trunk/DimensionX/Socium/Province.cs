using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LandscapeGeneration;
using Random;
using NameGen;

namespace Socium
{
    public class Province : BorderBuilder<LandX>, ITerritory
    {
        public string m_sName;

        private bool m_bForbidden = false;

        private static Province m_pForbidden = new Province(true);

        public bool Forbidden
        {
            get { return m_bForbidden; /* this == Province.m_pForbidden; */ }
        }

        public List<LandX> m_cContents = new List<LandX>();

        private Dictionary<object, List<Line>> m_cBorderWith = new Dictionary<object, List<Line>>();

        private object m_pOwner = null;

        public object Owner
        {
            get { return m_pOwner; }
            set { m_pOwner = value; }
        }

        public Dictionary<object, List<Line>> BorderWith
        {
            get { return m_cBorderWith; }
        }

        public object[] m_aBorderWith = null;

        internal void FillBorderWithKeys()
        {
            m_aBorderWith = new List<object>(m_cBorderWith.Keys).ToArray();
        }

        public Race m_pRace = null;

        public LandX m_pCenter;

        public LocationX m_pAdministrativeCenter = null;

        public Province(bool bForbidden)
        {
            m_bForbidden = bForbidden;
        }

        public Province()
        {}

        public override void Start(LandX pSeed)
        {
            m_cBorderWith.Clear();
            m_cContents.Clear();

            base.Start(pSeed);

            m_pCenter = pSeed;
            m_pRace = pSeed.m_pRace;

            m_sName = m_pRace.m_pLanguage.RandomCountryName();

            m_cContents.Add(pSeed);
            pSeed.m_pProvince = this;
        }

        /// <summary>
        /// Присоединяет к провинции сопредельную нечейную землю.
        /// Чем длиннее общая граница с землёй - тем выше вероятность того, что выбрана будет именно она.
        /// </summary>
        /// <returns></returns>
        public bool Grow(int iMaxProvinceSize)
        {
            if (m_cContents.Count > iMaxProvinceSize)
                return false;

            Dictionary<LandX, float> cBorderLength = new Dictionary<LandX, float>();

            foreach (ITerritory pTerr in m_cBorder.Keys)
            {
                if (pTerr.Forbidden)
                    continue;
                
                LandX pLand = pTerr as LandX;
                
                if(pLand.m_pProvince == null && !pLand.IsWater)
                {
                    //if (m_pCenter.Type.m_iMovementCost > pLand.Type.m_iMovementCost)
                    //    continue;

                    //граница провинции с этой землёй
                    float fWholeLength = 1;
                    foreach (Line pLine in m_cBorder[pLand])
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
                    }

                    fWholeLength /= fTotalLength;

                    if (fWholeLength < 0.25f)
                        fWholeLength /= 10;
                    if (fWholeLength > 0.5f)
                        fWholeLength *= 10;
                    //if (fWholeLength < 0.25f && m_cContents.Count > 1)
                    //    continue;

                    //fWholeLength /= (float)pLand.Type.m_iMovementCost * pLand.Type.m_iMovementCost;
                    //if (m_pCenter.Type != pLand.Type)
                    //    fWholeLength /= 100;
                    fWholeLength /= Math.Max(pLand.Type.m_iMovementCost, m_pCenter.Type.m_iMovementCost) / Math.Min(pLand.Type.m_iMovementCost, m_pCenter.Type.m_iMovementCost);

                    foreach (LandTypeInfoX pType in m_pRace.m_cPrefferedLands)
                        if (pType == pLand.Type)
                            fWholeLength *= 10;// (float)pLand.Type.m_iMovementCost;//2;

                    foreach (LandTypeInfoX pType in m_pRace.m_cHatedLands)
                        if (pType == pLand.Type)
                            fWholeLength /= 100;// (float)pLand.Type.m_iMovementCost;//2;

                    //if(m_pCenter.Type != pLand.Type)
                    //    fWholeLength /= (float)Math.Max(pLand.Type.m_iMovementCost, m_pCenter.Type.m_iMovementCost)*10;//2;

                    cBorderLength[pLand] = fWholeLength;
                }
            }

            if (cBorderLength.Count == 0)
                return false;

            //foreach (ITerritory pTerr in m_cBorder.Keys)
            //{
            //    if (pTerr.Forbidden)
            //        continue;

            //    LandX pLand = pTerr as LandX;

            //    if (!cBorderLength.ContainsKey(pLand))
            //        continue;

            //    float fTotalLength = 0;
            //    foreach (ITerritory pLinkTerr in pLand.BorderWith.Keys)
            //    {
            //        if (pLinkTerr.Forbidden)
            //            continue;

            //        LandX pLink = pLinkTerr as LandX;
            //        foreach (Line pLine in pLand.BorderWith[pLink])
            //            fTotalLength += pLine.m_fLength;
            //    }

            //    cBorderLength[pLand] /= fTotalLength;
            //}

            LandX pAddon = null;

            int iChoice = Rnd.ChooseOne(cBorderLength.Values, 5);
            foreach (LandX pLand in cBorderLength.Keys)
            {
                iChoice--;
                if (iChoice < 0)
                {
                    pAddon = pLand;
                    break;
                }
            }

            if (pAddon == null)
                return false;
            
            //float fTotalLength = 0;
            //foreach (ITerritory pLinkTerr in pAddon.BorderWith.Keys)
            //{
            //    if (pLinkTerr.Forbidden)
            //        continue;

            //    LandX pLink = pLinkTerr as LandX;
            //    foreach (Line pLine in pAddon.BorderWith[pLink])
            //        fTotalLength += pLine.m_fLength;
            //}

            //if (!Rnd.ChooseOne(cBorderLength[pAddon], fTotalLength - cBorderLength[pAddon]) || Rnd.OneChanceFrom(100))
            //    return true;

            if (Rnd.ChooseOne(m_cContents.Count, 10) || Rnd.OneChanceFrom(100))
                return true;

            //if (!Rnd.OneChanceFrom(1 + m_iPower * m_iPower))
            //if (Rnd.OneChanceFrom(1 + pAddon.Type.m_iMovementCost * pAddon.Type.m_iMovementCost))
            //    return true;

            //foreach (LandTypeInfoX pType in m_pRace.m_cHatedLands)
            //    if (pType == pAddon.Type && !Rnd.OneChanceFrom(5))
            //        return true;

            m_cContents.Add(pAddon);
            pAddon.m_pProvince = this;

            m_cBorder[pAddon].Clear();
            m_cBorder.Remove(pAddon);

            foreach (var pLand in pAddon.BorderWith)
            {
                if (!(pLand.Key as ITerritory).Forbidden && m_cContents.Contains(pLand.Key as LandX))
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

        /// <summary>
        /// Заполняет словарь границ с другими провинциями.
        /// </summary>
        public void Finish(float fCycleShift)
        {
            ChainBorder(fCycleShift);

            foreach (ITerritory pLand in m_cBorder.Keys)
            {
                Province pProvince;
                if (pLand.Forbidden || (pLand as LandX).m_pProvince == null)
                    pProvince = Province.m_pForbidden;
                else
                    pProvince = (pLand as LandX).m_pProvince;

                if (!m_cBorderWith.ContainsKey(pProvince))
                    m_cBorderWith[pProvince] = new List<Line>();
                m_cBorderWith[pProvince].AddRange(m_cBorder[pLand]);
            }
            FillBorderWithKeys();

            Dictionary<Race, int> cRacesCount = new Dictionary<Race, int>();

            int iMaxPop = 0;
            Race pMaxRace = null;

            foreach (LandX pLand in m_cContents)
            {
                bool bRestricted = true;
                foreach (LocationX pLoc in pLand.m_cContents)
                    if (!pLoc.Forbidden && !pLoc.m_bBorder)
                        bRestricted = false;

                if (bRestricted)
                    continue;

                if (!cRacesCount.ContainsKey(pLand.m_pRace))
                    cRacesCount[pLand.m_pRace] = 0;
                cRacesCount[pLand.m_pRace] += pLand.m_cContents.Count;
                if (cRacesCount[pLand.m_pRace] > iMaxPop)
                {
                    iMaxPop = cRacesCount[pLand.m_pRace];
                    pMaxRace = pLand.m_pRace;
                }
            }

            if (pMaxRace != null)
                m_pRace = pMaxRace;

            foreach (LandX pLand in m_cContents)
                pLand.m_pRace = m_pRace;
        }

        public LocationX BuildAdministrativeCenter(SettlementInfo pCenter, bool bFast)
        {
            Dictionary<LandX, float> cLandsChances = new Dictionary<LandX, float>();

            foreach (LandX pLand in m_cContents)
            {
                bool bRestricted = true;
                foreach (LocationX pLoc in pLand.m_cContents)
                    if (!pLoc.Forbidden && !pLoc.m_bBorder)
                        bRestricted = false;

                if (bRestricted)
                    continue;

                cLandsChances[pLand] = (float)pLand.m_cContents.Count * pLand.Type.m_cSettlementsDensity[pCenter.m_eSize];

                bool bProvinceBorder = false;
                bool bStateBorder = false;
                foreach (ITerritory pTerr in pLand.m_aBorderWith)
                { 
                    if(pTerr.Forbidden)
                        continue;

                    LandX pLink = pTerr as LandX;

                    if (pLink.IsWater)
                        continue;

                    if (pLink.m_pProvince != this)
                        bProvinceBorder = true;

                    if (pLink.m_pProvince.Owner != this.Owner)
                        bStateBorder = true;
                }

                if (bProvinceBorder)
                    cLandsChances[pLand] /= 100.0f;

                if (bStateBorder)
                    cLandsChances[pLand] /= 100.0f;
            }

            if (cLandsChances.Count == 0)
                return null;

            int iChance = Rnd.ChooseOne(cLandsChances.Values, 1);

            foreach (LandX pLand in cLandsChances.Keys)
            {
                iChance--;
                if (iChance < 0)
                {
                    m_pAdministrativeCenter = pLand.BuildSettlement(pCenter, true, bFast);
                    break;
                }
            }

            //Грязный хак: низкокультурные сообщества не могут быть многонациональными
            State pState = Owner as State;
            if (pState.m_iInfrastructureLevel < 2)
                m_pRace = pState.m_pRace;

            return m_pAdministrativeCenter;
        }

        public override string ToString()
        {
            return "province " + m_sName + " (" + m_pAdministrativeCenter.ToString() + ")";
        }

        public override float GetMovementCost()
        {
            throw new NotImplementedException();
        }
    }
}
