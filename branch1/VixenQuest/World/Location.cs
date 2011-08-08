using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;

namespace VixenQuest.World
{
    public abstract class Location: Space
    {
        public abstract Location NextStepTo(Location pTarget);

        public abstract Dictionary<State.DwellersCathegory, int> Races
        { get; }

        public abstract ValuedString[] ProfessionM
        { get; }

        public abstract ValuedString[] ProfessionF
        { get; }

        public ValuedString[] GetProfessionM(int iMaxProfessionRank)
        {
            List<ValuedString> cResult = new List<ValuedString>();

            foreach (ValuedString pVal in ProfessionM)
                if (pVal.m_iValue <= iMaxProfessionRank)
                    cResult.Add(pVal);

            return cResult.ToArray();
        }

        public ValuedString[] GetProfessionF(int iMaxProfessionRank)
        {
            List<ValuedString> cResult = new List<ValuedString>();

            foreach (ValuedString pVal in ProfessionF)
                if (pVal.m_iValue <= iMaxProfessionRank)
                    cResult.Add(pVal);

            return cResult.ToArray();
        }

        public State m_pState = null;

        public List<Opponent> m_cDwellers = new List<Opponent>();

        public World m_pWorld;

        public Location(World pWorld)
            : base()
        {
            m_pWorld = pWorld;
        }

        protected void AddPopulation(int iPopulation, int iMaxProfessionRank, int iAverageLevel)
        {
            for (int i = 0; i < iPopulation; i++)
            {
                Opponent pPretendent;
                do
                {
                    pPretendent = new Opponent(this, GetRandomRace(), GetProfessionM(iMaxProfessionRank), GetProfessionF(iMaxProfessionRank), false, null);

                    bool twin = false;
                    foreach (Opponent pDweller in m_cDwellers)
                        if (pDweller.SingleName == pPretendent.SingleName)
                        {
                            twin = true;
                            break;
                        }

                    if (!twin)
                        break;
                }
                while (true);

                m_cDwellers.Add(pPretendent);
                //m_sName += ", " + pNewRace.ToString();
            }
        }

        public Race GetRandomRace()
        {
            Race pRace = m_pState.m_pWorld.m_cLocalRaces[0];
            State.DwellersCathegory eChoice = State.DwellersCathegory.SacredAnimals;

            do
            {
                int iChoice = Rnd.ChooseOne(Races.Values, 1);
                foreach (State.DwellersCathegory eCat in Races.Keys)
                {
                    iChoice--;
                    if (iChoice < 0)
                    {
                        eChoice = eCat;
                        break;
                    }
                }
            }
            while (m_pState.m_cRaces[eChoice].Count == 0);

            int index = Rnd.Get(m_pState.m_cRaces[eChoice].Count);
            pRace = m_pState.m_cRaces[eChoice][index];

            return pRace;
        }
    }
}
