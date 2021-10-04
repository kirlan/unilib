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
    public class AreaX: Area<LandX, LandTypeInfoX>
    {
        public Nation m_pNation = null;

        public string m_sName;

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
