using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;
using NameGen;

namespace VixenQuest.World
{
    public class StateInfo
    {
        public string m_sName;
        public int m_iRank;

        public string m_sRulerM;
        public string m_sRulerF;

        public string m_sHeirM;
        public string m_sHeirF;

        public Settlement.Size m_eCapital;

        public StateInfo(string sName, int iRank, string sRulerM, string sRulerF, string sHeirM, string sHeirF, Settlement.Size eCapital)
        {
            m_sName = sName;
            m_iRank = iRank;
            m_sRulerM = sRulerM;
            m_sRulerF = sRulerF;
            m_sHeirM = sHeirM;
            m_sHeirF = sHeirF;
            m_eCapital = eCapital;
        }
    }

    public class State
    {
        public int m_iTier;

        public string m_sName;

        private static StateInfo[] m_aPlace = 
        {
            new StateInfo("Empire", 17, "Emperor", "Empress", "Prince", "Princess", Settlement.Size.Methropoly),
            new StateInfo("Kingdom", 16, "King", "Queen", "Prince", "Princess", Settlement.Size.Capital),
            new StateInfo("Order", 15, "Grandmagister", "Grandmagistress", "Magister", "Magistress", Settlement.Size.City),
            new StateInfo("Tribes", 14, "Patriarch", "Matriarch", "Warlord", "Princess", Settlement.Size.Town),
        };

        public enum DwellersCathegory
        { 
            WildAnimals,
            DomesticAnimals,
            SacredAnimals,
            LesserRaces,
            MajorRaces
        }

        public Dictionary<DwellersCathegory, List<Race>> m_cRaces = new Dictionary<DwellersCathegory, List<Race>>();

        public List<Opponent> m_cRulers = new List<Opponent>();

        public List<Opponent> m_cHeirs = new List<Opponent>();

        public World m_pWorld;

        public string m_sShortName;

        public Race GetRandomRace(DwellersCathegory eCat)
        {
            if (m_cRaces[eCat].Count == 0)
                return null;

            int index = Rnd.Get(m_cRaces[eCat].Count);
            return m_cRaces[eCat][index];
        }

        private void GenerateName()
        {
            m_sShortName = NameGenerator.GetAbstractName();
            int variant = Rnd.Get(2);
            switch (variant)
            {
                case 0:
                    m_sName = m_sShortName + " " + m_pInfo.m_sName;
                    break;
                case 1:
                    m_sName = m_pInfo.m_sName + " of " + m_sShortName;
                    break;
            }
        }

        private void Populate()
        {
            m_cRaces[DwellersCathegory.MajorRaces] = new List<Race>();
            int iMainRacesCount = 1 + Rnd.Get(m_pWorld.m_cLocalRaces.Count / 2);
            for (int i = 0; i < iMainRacesCount; i++)
            {
                int random;
                do
                {
                    random = Rnd.Get(m_pWorld.m_cLocalRaces.Count);
                }
                while (m_cRaces[DwellersCathegory.MajorRaces].Contains(m_pWorld.m_cLocalRaces[random]));
                m_cRaces[DwellersCathegory.MajorRaces].Add(m_pWorld.m_cLocalRaces[random]);
            }

            m_cRaces[DwellersCathegory.LesserRaces] = new List<Race>();
            int iLesserRacesCount = 1 + Rnd.Get(m_pWorld.m_cLocalRaces.Count - iMainRacesCount);
            for (int i = 0; i < iLesserRacesCount; i++)
            {
                int random;
                do
                {
                    random = Rnd.Get(m_pWorld.m_cLocalRaces.Count);
                }
                while (m_cRaces[DwellersCathegory.MajorRaces].Contains(m_pWorld.m_cLocalRaces[random]) || 
                       m_cRaces[DwellersCathegory.LesserRaces].Contains(m_pWorld.m_cLocalRaces[random]));
                m_cRaces[DwellersCathegory.LesserRaces].Add(m_pWorld.m_cLocalRaces[random]);
            }

            m_cRaces[DwellersCathegory.WildAnimals] = new List<Race>();
            int iWildAnimalsCount = 1 + Rnd.Get(m_pWorld.m_cLocalAnimals.Count / 2);
            for (int i = 0; i < iWildAnimalsCount; i++)
            {
                int random;
                do
                {
                    random = Rnd.Get(m_pWorld.m_cLocalAnimals.Count);
                }
                while (m_cRaces[DwellersCathegory.WildAnimals].Contains(m_pWorld.m_cLocalAnimals[random]));
                m_cRaces[DwellersCathegory.WildAnimals].Add(m_pWorld.m_cLocalAnimals[random]);
            }

            m_cRaces[DwellersCathegory.DomesticAnimals] = new List<Race>();
            int iDomesticAnimalsCount = 1 + Rnd.Get((m_pWorld.m_cLocalAnimals.Count - iWildAnimalsCount) / 2);
            for (int i = 0; i < iDomesticAnimalsCount; i++)
            {
                int random;
                do
                {
                    random = Rnd.Get(m_pWorld.m_cLocalAnimals.Count);
                }
                while (m_cRaces[DwellersCathegory.WildAnimals].Contains(m_pWorld.m_cLocalAnimals[random]) || 
                       m_cRaces[DwellersCathegory.DomesticAnimals].Contains(m_pWorld.m_cLocalAnimals[random]));
                m_cRaces[DwellersCathegory.DomesticAnimals].Add(m_pWorld.m_cLocalAnimals[random]);
            }

            m_cRaces[DwellersCathegory.SacredAnimals] = new List<Race>();
            if (m_pWorld.m_cLocalAnimals.Count > iWildAnimalsCount + iDomesticAnimalsCount)
            {
                int random;
                do
                {
                    random = Rnd.Get(m_pWorld.m_cLocalAnimals.Count);
                }
                while (m_cRaces[DwellersCathegory.WildAnimals].Contains(m_pWorld.m_cLocalAnimals[random]) ||
                       m_cRaces[DwellersCathegory.DomesticAnimals].Contains(m_pWorld.m_cLocalAnimals[random]));
                m_cRaces[DwellersCathegory.SacredAnimals].Add(m_pWorld.m_cLocalAnimals[random]);
            }
        }

        public StateInfo m_pInfo;

        public State(World pWorld)
            : base()
        {
            m_iTier = Rnd.Get(6);
            m_pWorld = pWorld;

            int iPlace = Rnd.Get(m_aPlace.Length);
            m_pInfo = m_aPlace[iPlace];

            GenerateName();
            Populate();

            Opponent pPretendent = new Opponent(this, true);
            m_cRulers.Add(pPretendent);

            if (m_cRulers[0].Orientation != Orientation.Homo && Rnd.OneChanceFrom(2))
            {
                do
                {
                    pPretendent = new Opponent(this, true);
                }
                while (!pPretendent.WannaFuck(m_cRulers[0]) || !m_cRulers[0].WannaFuck(pPretendent));

                m_cRulers.Add(pPretendent);
            }

            int iHeirsCount = Rnd.Get(13);

            for (int i = 0; i < iHeirsCount; i++)
            {
                pPretendent = new Opponent(this, false);
                m_cHeirs.Add(pPretendent);
            }
        }

        public State(World pWorld, string sName)
            : base()
        {
            m_iTier = pWorld.m_iTier;
            m_pWorld = pWorld;

            m_pInfo = null;

            m_sName = sName;
            m_sShortName = sName;

            //m_cLands.Add(new Land(this, sName));
        }
    }
}
