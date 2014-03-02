using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;

namespace RandomStory
{
    public class Story
    {
        public Character m_pHero = null;

        public Character m_pTutor = null;

        public Character m_pVillain = null;

        public Character m_pMinion = null;
        //public string m_sMinions;

        //public string m_sHeroVillainRelations;

        public Character m_pHelper = null;

        //public string m_sHeroHelperRelations;

        public Setting m_pSetting;

        public Strings m_cLocations = new Strings();

        public string m_sProblem;

        public string m_sKeyItem;

        public string m_sSolution;

        public Story(Repository pRep, bool bVoyagersAllowed)
        {
            m_pSetting = pRep.GetRandomSetting(1, true);

            if (m_pSetting == null)
                return;

            m_pHero = new Character(pRep, m_pSetting, bVoyagersAllowed, false, null, "героя");
            if (m_pHero.m_pHomeSetting != m_pSetting && !Rnd.OneChanceFrom(3))
                bVoyagersAllowed = false;
            
            if (Rnd.OneChanceFrom(2))
            {
                m_pTutor = new Character(pRep, m_pSetting, bVoyagersAllowed, true, m_pHero, "наставника героя");
                if (m_pTutor.m_pHomeSetting != m_pSetting && !Rnd.OneChanceFrom(3))
                    bVoyagersAllowed = false;
            }
            
            m_pVillain = new Character(pRep, m_pSetting, bVoyagersAllowed, true, Rnd.OneChanceFrom(2) ? m_pHero : m_pTutor, "антагониста");
            if (m_pVillain.m_pHomeSetting != m_pSetting && !Rnd.OneChanceFrom(3))
                bVoyagersAllowed = false;

            if (Rnd.OneChanceFrom(2))
            {
                m_pMinion = new Character(pRep, m_pSetting, bVoyagersAllowed, false, m_pVillain, "помощника антагониста");
                if (m_pMinion.m_pHomeSetting != m_pSetting && !Rnd.OneChanceFrom(3))
                    bVoyagersAllowed = false;
            }

            if (m_pTutor == null || Rnd.OneChanceFrom(2))
            {
                m_pHelper = new Character(pRep, m_pSetting, bVoyagersAllowed, false, m_pHero, "спутника героя");
                if (m_pHelper.m_pHomeSetting != m_pSetting && !Rnd.OneChanceFrom(3))
                    bVoyagersAllowed = false;
            }

            int iLocationsCount = 5 + Rnd.Get(4);
            for (int i = 0; i < iLocationsCount; i++)
            {
                string sLoc = m_pSetting.GetRandomLocation(m_cLocations);
                if(!string.IsNullOrWhiteSpace(sLoc))
                    m_cLocations.Add(sLoc);
            }

            m_sProblem = pRep.GetRandomProblem();

            m_sSolution = pRep.GetRandomSolution();

            m_sKeyItem = m_pSetting.GetRandomArtefact();
        }
    }
}
