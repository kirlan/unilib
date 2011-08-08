using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;

namespace VixenQuest.World
{
    enum LandType
    {
        Undefined,
        Forbidden
    }

    public class Land : Location
    {
        private static string[] m_aPlace = 
        {
            "Forest",
            "Woods",
            "Sands",
            "Desert",
            "Wastes",
            "Country",
            "Plains",
            "Shore",
            "Swamp",
            "Valley",
            "Mountains",
            "Hills",
        };

        public static ValuedString[] m_aProfessionM = 
        {
            //still low, but they have mo masters
            new ValuedString("stranger", 2),
            new ValuedString("traveller", 2),
            new ValuedString("hermit", 2),
            //small, but own buisiness
            new ValuedString("hunter", 3),
            new ValuedString("ranger", 3),
            new ValuedString("scavenger", 3),
            new ValuedString("priest", 3),
            //crime grunts
            new ValuedString("marauder", 4),
            new ValuedString("rogue", 4),
            new ValuedString("bandit", 4),
            new ValuedString("burglar", 4),
            //well-paid workers
            new ValuedString("slaver", 6),
            //common adventurers
            new ValuedString("fighter", 7),
            new ValuedString("monk", 7),
            new ValuedString("warden", 7),
            new ValuedString("mage apprentice", 7),
            new ValuedString("monk", 7),
            new ValuedString("bruiser", 7),
            new ValuedString("guardian", 7),
            new ValuedString("warrior", 7),
            //professional adventurers
            new ValuedString("gladiator", 8),
            new ValuedString("assasin", 8),
            new ValuedString("ninja", 8),
            new ValuedString("spy", 8),
            new ValuedString("sentinel", 8),
            new ValuedString("slut-hunter", 8),
            new ValuedString("head-hunter", 8),
            new ValuedString("bitch-hunter", 8),
            //mages, clerics, etc.
            new ValuedString("witch-hunter", 9),
            new ValuedString("demon-hunter", 9),
            new ValuedString("cleric", 9),
            new ValuedString("necromancer", 9),
            new ValuedString("eromancer", 9),
            new ValuedString("summoner", 9),
            new ValuedString("inquisitor", 9),
            new ValuedString("sage", 9),
            new ValuedString("shaman", 9),
            new ValuedString("mage", 9),
            new ValuedString("prophet", 9),
            //nobile & rich
            new ValuedString("knight", 10),
        };

        public static ValuedString[] m_aProfessionF = 
        {
            //small, but own buisiness
            new ValuedString("huntress", 3),
            new ValuedString("priestess", 3),
            //crime grunts
            new ValuedString("burglaress", 4),
            //city crime
            new ValuedString("thief", 5),
            //well-paid workers
            new ValuedString("bride", 6),
            //common adventurers
            new ValuedString("masturbatrix", 7),
            new ValuedString("wardeness", 7),
            //professional adventurers
            //new ValuedString("sentinel", 8),
            new ValuedString("head-huntress", 8),
            new ValuedString("bitch-huntress", 8),
            new ValuedString("assasiness", 8),
            new ValuedString("gladiatrix", 8),
            //mages, clerics, etc.
            new ValuedString("witch-huntress", 9),
            new ValuedString("demon-huntress", 9),
            new ValuedString("witch", 9),
            new ValuedString("sorceress", 9),
            new ValuedString("prophetess", 9),
            new ValuedString("inquisitrix", 9),
            new ValuedString("necromantress", 9),
            new ValuedString("enchantress", 9),
            new ValuedString("shamaness", 9),
            //nobile & rich
            new ValuedString("lady knight", 10),
        };

        private List<Land> m_cLinks = new List<Land>();

        internal List<Land> Links
        {
            get { return m_cLinks; }
        }

        private const int m_iMaxLinks = 8;

        public int MaxLinks
        {
            get { return m_iMaxLinks; }
        }

        public List<Location> m_cLocations = new List<Location>();

        public void Assign(State pState, bool bCapital)
        {
            if (pState != null && m_pWorld != pState.m_pWorld)
                throw new ArgumentException("Given state not belongs to this world!");

            if (m_eType == LandType.Forbidden && pState != null)
                throw new ArgumentException("Forbidden lands can't belong to any state!");

            m_iTier = Rnd.Get(6);
            m_pState = pState;

            if(m_eType != LandType.Forbidden)
                AddPopulation(50 + Rnd.Get(50), 10, pState.m_iTier * m_iTier);

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

            if (m_eType != LandType.Forbidden)
            {
                Settlement.Size eSize = m_pState.m_pInfo.m_eCapital;
                if (!bCapital)
                    eSize--;

                int iCount = 1;
                do
                {
                    for (int i = 0; i < iCount * iCount; i++)
                        m_cLocations.Add(new Settlement(this, eSize, bCapital && iCount == 1));
                    iCount++;

                    if (eSize == Settlement.Size.Hamlet)
                        break;
                    else
                        eSize--;
                }
                while (true);

                int iPlacesCount = Rnd.Get(25);
                for (int i = 0; i < iPlacesCount; i++)
                    m_cLocations.Add(new Place(this));
            }
        }

        private int m_iX;

        public int X
        {
            get { return m_iX; }
        }
        private int m_iY;

        public int Y
        {
            get { return m_iY; }
        }

        public Land(World pWorld, int iX, int iY)
            : base(pWorld)
        {
            m_iX = iX;
            m_iY = iY;
        }

        private LandType m_eType = LandType.Undefined;

        internal LandType Type
        {
            get { return m_eType; }
            set { m_eType = value; }
        }
        public Land(World pWorld, string sName)
            : base(pWorld)
        {
            m_sName = sName;
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
                    m_cRaces[State.DwellersCathegory.SacredAnimals] = 0;
                    m_cRaces[State.DwellersCathegory.LesserRaces] = 2;
                    m_cRaces[State.DwellersCathegory.MajorRaces] = 1;
                }
                return m_cRaces; 
            }
        }

        public override ValuedString[] ProfessionM
        {
            get { return m_aProfessionM; }
        }

        public override ValuedString[] ProfessionF
        {
            get { return m_aProfessionF; }
        }

        List<Opponent> m_cAvailableOpponents = new List<Opponent>();

        public override Opponent[] AvailableOpponents()
        {
            if (m_cAvailableOpponents.Count == 0)
            {
                m_cAvailableOpponents.AddRange(m_cDwellers);
                foreach (Location place in m_cLocations)
                {
                    m_cAvailableOpponents.AddRange(place.AvailableOpponents());
                }
            }
            return m_cAvailableOpponents.ToArray();
        }

        public override Location NextStepTo(Location pTarget)
        {
            if (this == pTarget)
                return null;

            if (pTarget is Land)
            {
                if (m_pWorld == pTarget.m_pWorld)
                {
                    Land[] pPath = Universe.ShortestWay(this, pTarget as Land);
                    return pPath[1];
                }
                else
                {
                    if (Rnd.OneChanceFrom(100))
                        return pTarget;
                    else
                        return this;
                }
            }

            if(pTarget is Settlement)
            {
                Location pNextStep = NextStepTo((pTarget as Settlement).m_pLand);
                if(pNextStep == null)
                    return pTarget;

                return pNextStep;
            }

            if(pTarget is Place)
            {
                Location pNextStep = NextStepTo((pTarget as Place).m_pLand);
                if (pNextStep == null)
                    return pTarget;

                return pNextStep;
            }

            if(pTarget is Building)
            {
                return NextStepTo((pTarget as Building).m_pSettlement);
            }

            return null;
        }
    }
}
