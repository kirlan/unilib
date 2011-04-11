using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;

namespace VixenQuest.World
{
    class Place : Location
    {
        private static string[] m_aPlace = 
        {
            "Grove",
            "Lake",
            "River",
            "Cave",
            "Spot",
            "Grotto",
            "Peak",
            "Mountain",
            "Ruins",
            "Hole",
            "Hill",
            "Nest",
            "Lair",
            "Weil",
            "Cemetary",
            "Graveyard",
        };
        public Land m_pLand;

        public Place(Land pLand)
            : base(pLand.m_pWorld)
        {
            m_pLand = pLand;
            m_pState = m_pLand.m_pState;

            AddPopulation(1 + Rnd.Get(2), 10, pLand.m_pState.m_iTier * pLand.m_iTier);

            int iPlace = Rnd.Get(m_aPlace.Length);
            int iEpithet = Rnd.Get(m_aEpithet.Length);
            int iDescription = Rnd.Get(m_aDescription.Length);

            int variant = Rnd.Get(3);

            switch (variant)
            {
                case 0:
                    {
                        m_sName = m_aEpithet[iEpithet];
                        m_sName += m_aPlace[iPlace];
                    }
                    break;
                case 1:
                    {
                        m_sName = m_aPlace[iPlace];
                        m_sName += m_aDescription[iDescription];
                    }
                    break;
                default:
                    {
                        m_sName = m_aEpithet[iEpithet];
                        m_sName += m_aPlace[iPlace];
                        m_sName += m_aDescription[iDescription];
                    }
                    break;
            }
        }

        private static Dictionary<State.DwellersCathegory, int> m_cRaces = new Dictionary<State.DwellersCathegory, int>();

        public override Dictionary<State.DwellersCathegory, int> Races
        {
            get
            {
                if (m_cRaces.Count == 0)
                {
                    m_cRaces[State.DwellersCathegory.WildAnimals] = 2;
                    m_cRaces[State.DwellersCathegory.DomesticAnimals] = 0;
                    m_cRaces[State.DwellersCathegory.SacredAnimals] = 1;
                    m_cRaces[State.DwellersCathegory.LesserRaces] = 2;
                    m_cRaces[State.DwellersCathegory.MajorRaces] = 1;
                }
                return m_cRaces;
            }
        }

        public override ValuedString[] ProfessionM
        {
            get { return Land.m_aProfessionM; }
        }

        public override ValuedString[] ProfessionF
        {
            get { return Land.m_aProfessionF; }
        }

        public override Opponent[] AvailableOpponents()
        {
            return m_cDwellers.ToArray();
        }

        public override Location NextStepTo(Location pTarget)
        {
            if (this == pTarget)
                return null;

            return m_pLand;
        }
    }
}
