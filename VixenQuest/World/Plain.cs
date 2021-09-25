using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;
using NameGen;

namespace VixenQuest.World
{
    public class Plain : Space
    {
        private static string[] m_aPlace = 
        {
            "Realm",
            "Plane",
            "Dimension",
            "World",
        };

        public Universe m_pUniverse;

        public List<Race> m_cLocalRaces = new List<Race>();
        public List<Race> m_cLocalAnimals = new List<Race>();

        public List<State> m_cStates = new List<State>();

        /// <summary>
        /// Заказной мир с единственной локацией
        /// </summary>
        /// <param name="sName">имя локации</param>
        /// <param name="pUniverse">ссылка на вселенную</param>
        public Plain(string sName, Universe pUniverse) :
            base(0)
        {
            m_pUniverse = pUniverse;

            m_sName = sName;

            m_cStates.Add(new State(this, sName));
        }

        /// <summary>
        /// Обычный мир
        /// </summary>
        /// <param name="iTier">тир мира</param>
        /// <param name="pUniverse">ссылка на вселенную</param>
        public Plain(int iTier, Universe pUniverse) :
            base(iTier)
        {
            m_pUniverse = pUniverse;

            int variant = Rnd.Get(2);

            switch (variant)
            {
                case 0:
                    {
                        int iEpithet = Rnd.Get(m_aEpithet.Length);
                        m_sName = m_aEpithet[iEpithet];

                        int iPlace = Rnd.Get(m_aPlace.Length);
                        m_sName += m_aPlace[iPlace];
                    }
                    break;
                case 1:
                    {
                        int iPlace = Rnd.Get(m_aPlace.Length);
                        m_sName = m_aPlace[iPlace];

                        int iDescription = Rnd.Get(m_aDescription.Length);
                        m_sName += m_aDescription[iDescription];
                    }
                    break;
                default:
                    {
                        int iEpithet = Rnd.Get(m_aEpithet.Length);
                        m_sName = m_aEpithet[iEpithet];

                        int iPlace = Rnd.Get(m_aPlace.Length);
                        m_sName += m_aPlace[iPlace];

                        int iDescription = Rnd.Get(m_aDescription.Length);
                        m_sName += m_aDescription[iDescription];
                    }
                    break;
            }

            m_sName = NameGenerator.GetAbstractName() + ", " + m_sName;

            //m_sName += " (T" + m_iTier.ToString();// +")";

            int iDiversity = 4 + Rnd.Get(4);
            for (int i = 0; i < iDiversity; i++)
            {
                Race pLocalAnimal = GetRandomRace(Sapience.Animal);
                m_cLocalAnimals.Add(pLocalAnimal);
                //m_sName += ", " + pLocalAnimals.ToString();
            }

            iDiversity = 4 + Rnd.Get(4);
            for (int i = 0; i < iDiversity; i++)
            {
                Race pNewRace = GetRandomRace(Sapience.Human);
                m_cLocalRaces.Add(pNewRace);
                //m_sName += ", " + pNewRace.ToString();
            }
            //m_sName += ")";

            iDiversity = 4 + Rnd.Get(4);
            for (int i = 0; i < iDiversity; i++)
            {
                State pNewState = new State(this);
                m_cStates.Add(pNewState);
            }
        }

        private Race GetRandomRace(Sapience eSapience)
        {
            Race pPretendent;

            Race pBestPretendent = null;
            int iBestDifference = int.MaxValue;

            int iCounter = 0;
            do
            {
                int index = Rnd.Get(m_aAllRaces.Length);
                pPretendent = m_aAllRaces[index];

                int iDifference = Math.Abs(pPretendent.m_iRank - (m_iTier - 1) * 10);

                if (pPretendent.m_iRank < m_iTier * 10)
                    iDifference = 2 * iDifference;

                if (pPretendent.m_eSapience != eSapience)
                    iDifference = int.MaxValue;

                if (m_cLocalRaces.Contains(pPretendent))
                    iDifference = int.MaxValue;

                if (pBestPretendent == null || iBestDifference > iDifference)
                {
                    pBestPretendent = pPretendent;
                    iBestDifference = iDifference;
                }

                if (Rnd.Gauss(iDifference, 3))
                    return pPretendent;

                if (iCounter++ > 100)
                    return pBestPretendent;
            }
            while (true);
        }
    }
}
