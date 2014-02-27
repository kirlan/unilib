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

        public World m_pWorld;

        public List<string> m_cLocations = new List<string>();

        public string m_sProblem;

        public string m_sKeyItem;

        public string m_sSolution;

        public Story(Repository pRep, bool bVoyagersAllowed)
        {
            m_pWorld = pRep.GetRandomWorld(2);

            if (m_pWorld == null)
                return;

            m_pHero = new Character(pRep, m_pWorld, bVoyagersAllowed, false, null, "героя");
            if (Rnd.OneChanceFrom(2))
                m_pTutor = new Character(pRep, m_pWorld, bVoyagersAllowed, true, m_pHero, "наставника героя");
            m_pVillain = new Character(pRep, m_pWorld, bVoyagersAllowed, true, Rnd.OneChanceFrom(2) ? m_pHero : m_pTutor, "злодея");
            if (Rnd.OneChanceFrom(2))
                m_pMinion = new Character(pRep, m_pWorld, bVoyagersAllowed, false, m_pVillain, "помощника злодея");
            if (Rnd.OneChanceFrom(2))
                m_pHelper = new Character(pRep, m_pWorld, bVoyagersAllowed, false, m_pHero, "спутника героя");

            int iLocationsCount = 5 + Rnd.Get(4);
            for (int i = 0; i < iLocationsCount; i++)
            {
                string sLoc = m_pWorld.GetRandomLocation(m_cLocations);
                if(!string.IsNullOrWhiteSpace(sLoc))
                    m_cLocations.Add(sLoc);
            }

            m_sProblem = pRep.GetRandomProblem();

            m_sSolution = pRep.GetRandomSolution();

            m_sKeyItem = m_pWorld.GetRandomArtefact();
        }
    }
}
