using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;
using NameGen;
using LandscapeGeneration;

namespace Socium
{
    public class AreaX: Area<LandX, LandTypeInfoX>
    {
        public Race m_pRace = null;

        public string m_sName;

        public void SetRace(List<Race> cPossibleRaces)
        {
            Dictionary<Race, float> cChances = new Dictionary<Race, float>();
            foreach (Race pRace in cPossibleRaces)
            {
                cChances[pRace] = 1.0f;// / pRace.m_iRank;

                if (pRace.m_bDying)
                    cChances[pRace] /= 10 / m_pType.m_iMovementCost;
                    //cChances[pRace] /= 10000 / (m_pType.m_iMovementCost * m_pType.m_iMovementCost);

                if (pRace.m_bHegemon)
                    cChances[pRace] *= 10;

                foreach (LandTypeInfoX pType in pRace.m_pTemplate.m_aPrefferedLands)
                    if (m_pType == pType)
                        cChances[pRace] *= 10;

                foreach (LandTypeInfoX pType in pRace.m_pTemplate.m_aHatedLands)
                    if (m_pType == pType)
                        cChances[pRace] /= 100;
            }

            int iChance = Rnd.ChooseOne(cChances.Values, 3);
            foreach (Race pRace in cChances.Keys)
            {
                iChance--;
                if (iChance < 0)
                {
                    m_pRace = pRace;
                    break;
                }
            }

            m_sName = m_pRace.m_pTemplate.m_pLanguage.RandomCountryName();

            foreach (LandX pLand in m_cContents)
            {
                pLand.m_sName = m_sName;
                pLand.m_pRace = m_pRace;
            }
        }

        public string GetNativeRaceString()
        {
            if (m_pRace == null)
                return "unpopulated";
            else
                return m_pRace.m_pTemplate.m_sName.Trim();
        }

        public override string ToString()
        {
            return string.Format("{0} {1} ({2}, {3})", m_sName, m_pType.m_sName, m_cContents.Count, GetNativeRaceString());
        }
    }
}
