using System;
using System.Collections.Generic;
using System.Text;
using Random;

namespace RandomStory
{
    public class Story
    {
        private Repository m_pRepository = null;

        public Character m_pHero = null;

        public Character m_pTutor = null;

        public Character m_pVillain = null;

        public Character m_pMinion = null;

        public Character m_pHelper = null;

        public Setting m_pSetting;

        public Strings m_cGeography = new Strings();

        public Strings m_cLocations = new Strings();

        public Strings m_cEvents = new Strings();

        public string m_sProblem;

        public Strings m_cKeyItems = new Strings();

        public string m_sSolution;

        private bool m_bVoyagersAllowed;

        private bool m_bVoyagersAllowedRes;

        public Story(Repository pRepository, bool bVoyagers)
        {
            m_pRepository = pRepository;
            m_bVoyagersAllowed = bVoyagers;
            m_bVoyagersAllowedRes = m_bVoyagersAllowed;

            m_pSetting = m_pRepository.GetPrimeSetting();
//            m_pSetting = m_pRepository.GetRandomSetting(1, true);
        }

        public Character GetRandomHero()
        {
            bool bVoyagersAllowed = m_bVoyagersAllowed && Rnd.OneChanceFrom(2);
            if (m_pHero != null && m_pHero.m_pHomeSetting != m_pSetting)
                bVoyagersAllowed = true;
            return new Character(m_pRepository, m_pSetting, bVoyagersAllowed, false, null, "героя");
        }

        public void SetHero(Character pCharacter)
        {
            m_pHero = pCharacter;
            if (m_pHero.m_pHomeSetting != m_pSetting && !Rnd.OneChanceFrom(4))
                m_bVoyagersAllowed = false;

            FixRelations();
        }

        public Character GetRandomTutor()
        {
            bool bVoyagersAllowed = m_bVoyagersAllowed;
            if (m_pTutor != null && m_pTutor.m_pHomeSetting != m_pSetting)
                bVoyagersAllowed = true;

            Character pRelative = m_pHero;
            if (m_pVillain != null && Rnd.OneChanceFrom(2))
                pRelative = m_pVillain;
            return new Character(m_pRepository, m_pSetting, bVoyagersAllowed, true, pRelative, "наставника героя");
        }

        public void SetTutor(Character pCharacter)
        {
            m_pTutor = pCharacter;
            if (m_pTutor.m_pRelative != null)
                m_pTutor.m_pRelative.AddRelative();
            if (m_pTutor.m_pHomeSetting != m_pSetting && !Rnd.OneChanceFrom(4))
                m_bVoyagersAllowed = false;

            FixRelations();
        }

        public Character GetRandomVillain()
        {
            bool bVoyagersAllowed = m_bVoyagersAllowed;
            if (m_pVillain != null && m_pVillain.m_pHomeSetting != m_pSetting)
                bVoyagersAllowed = true;

            Character pRelative = m_pHero;
            if (m_pTutor != null && Rnd.OneChanceFrom(2))
                pRelative = m_pTutor;
            return new Character(m_pRepository, m_pSetting, bVoyagersAllowed, true, pRelative, "антагониста");
        }

        public void SetVillain(Character pCharacter)
        {
            m_pVillain = pCharacter;
            if (m_pVillain.m_pRelative != null)
                m_pVillain.m_pRelative.AddRelative();
            if (m_pVillain.m_pHomeSetting != m_pSetting && !Rnd.OneChanceFrom(4))
                m_bVoyagersAllowed = false;

            FixRelations();
        }

        public Character GetRandomMinion()
        {
            bool bVoyagersAllowed = m_bVoyagersAllowed;
            if (m_pMinion != null && m_pMinion.m_pHomeSetting != m_pSetting)
                bVoyagersAllowed = true;

            Character pRelative = m_pHero;
            if (m_pVillain != null && Rnd.OneChanceFrom(2))
                pRelative = m_pVillain;
            return new Character(m_pRepository, m_pSetting, bVoyagersAllowed, false, pRelative, "помощника антагониста");
        }

        public void SetMinion(Character pCharacter)
        {
            m_pMinion = pCharacter;
            if (m_pMinion.m_pRelative != null)
                m_pMinion.m_pRelative.AddRelative();
            if (m_pMinion.m_pHomeSetting != m_pSetting && !Rnd.OneChanceFrom(4))
                m_bVoyagersAllowed = false;

            FixRelations();
        }

        public Character GetRandomHelper()
        {
            bool bVoyagersAllowed = m_bVoyagersAllowed;
            if (m_pHelper != null && m_pHelper.m_pHomeSetting != m_pSetting)
                bVoyagersAllowed = true;

            Character pRelative = m_pHero;
            if (m_pTutor != null && Rnd.OneChanceFrom(2))
                pRelative = m_pTutor;
            if (m_pVillain != null && Rnd.OneChanceFrom(3))
                pRelative = m_pVillain;
            return new Character(m_pRepository, m_pSetting, bVoyagersAllowed, false, pRelative, "спутника героя");
        }

        public void SetHelper(Character pCharacter)
        {
            m_pHelper = pCharacter;
            if (m_pHelper.m_pRelative != null)
                m_pHelper.m_pRelative.AddRelative();
            if (m_pHelper.m_pHomeSetting != m_pSetting && !Rnd.OneChanceFrom(4))
                m_bVoyagersAllowed = false;

            FixRelations();
        }

        private void FixRelations()
        {
            List<Character> cCharas = new List<Character>();
            if (m_pHero != null)
                cCharas.Add(m_pHero);
            if (m_pVillain != null)
                cCharas.Add(m_pVillain);
            if (m_pTutor != null)
                cCharas.Add(m_pTutor);
            if (m_pMinion != null)
                cCharas.Add(m_pMinion);
            if (m_pHelper != null)
                cCharas.Add(m_pHelper);

            foreach (Character pChar in cCharas)
            {
                if (pChar.m_pRelative != null && !cCharas.Contains(pChar.m_pRelative))
                    pChar.m_pRelative = null;
            }
        }

        public string GetRandomProblem()
        {
            return m_pRepository.GetRandomProblem();
        }

        public void SetProblem(string pProblem)
        {
            m_sProblem = pProblem;
        }

        public string GetRandomSolution()
        {
            return m_pRepository.GetRandomSolution();
        }

        public void SetSolution(string pSolution)
        {
            m_sSolution = pSolution;
        }

        public Strings GetRandomItems()
        {
            Strings cItems = new Strings();
            int iItemsCount = 1 + Rnd.Get(3);
            for (int i = 0; i < iItemsCount; i++)
            {
                string sItem = m_pSetting.GetRandomArtefact(cItems);
                if (!string.IsNullOrWhiteSpace(sItem))
                    cItems.Add(sItem);
            }

            return cItems;
        }

        public void SetItems(Strings cKeyItems)
        {
            m_cKeyItems = cKeyItems;
        }

        public Strings GetRandomGeography()
        {
            Strings cGeography = new Strings();
            int iGeographyCount = 2 + Rnd.GetExp(3,3);
            for (int i = 0; i < iGeographyCount; i++)
            {
                string sLoc = m_pSetting.GetRandomGeography(cGeography);
                if (!string.IsNullOrWhiteSpace(sLoc))
                    cGeography.Add(sLoc);
            }

            return cGeography;
        }

        public void SetGeography(Strings cGeography)
        {
            m_cGeography = cGeography;
        }

        public Strings GetRandomPlaces()
        {
            Strings cLocations = new Strings();
            int iLocationsCount = 5 + Rnd.Get(4);
            for (int i = 0; i < iLocationsCount; i++)
            {
                string sLoc = m_pSetting.GetRandomLocation(cLocations);
                if (!string.IsNullOrWhiteSpace(sLoc))
                    cLocations.Add(sLoc);
            }

            return cLocations;
        }

        public void SetPlaces(Strings cLocations)
        {
            m_cLocations = cLocations;
        }

        public Strings GetRandomEvents()
        {
            Strings cEvents = new Strings();
            int iEventsCount = 8 + Rnd.Get(4);
            for (int i = 0; i < iEventsCount; i++)
            {
                string sEvent = m_pSetting.GetRandomEvent(cEvents);
                if (!string.IsNullOrWhiteSpace(sEvent))
                    cEvents.Add(sEvent);
            }

            return cEvents;
        }

        public void SetEvents(Strings cEvents)
        {
            m_cEvents = cEvents;
        }

        public void BuildFullStory()
        {
            if (m_pSetting == null)
                return;

            SetGeography(GetRandomGeography());

            SetHero(GetRandomHero());

            SetVillain(GetRandomVillain());

            if (Rnd.OneChanceFrom(2))
                SetTutor(GetRandomTutor());

            if (Rnd.OneChanceFrom(2))
                SetMinion(GetRandomMinion());

            if (m_pTutor == null || Rnd.OneChanceFrom(2))
                SetHelper(GetRandomHelper());

            SetItems(GetRandomItems());
            SetPlaces(GetRandomPlaces());
            SetEvents(GetRandomEvents());

            SetProblem(GetRandomProblem());
            SetSolution(GetRandomSolution());
        }
        public override string ToString()
        {
            if (m_pSetting == null)
                return base.ToString();

            if(m_bVoyagersAllowedRes)
                return m_pSetting.ToString() + " / +Попаданцы";

            return m_pSetting.ToString();
        }
    }
}
