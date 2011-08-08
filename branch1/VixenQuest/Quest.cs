using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;

namespace VixenQuest
{
    class Quest
    {
        public List<Encounter> m_cEncounters = new List<Encounter>();

        public string m_sName;

        private static string[] m_aPersonActions = 
        {
            "Seduce ",
            "Pervert ",
            "Pleasure ",
            "Entertain ",
            "Save ",
            "Meet with ",
            "Talk with ",
            "Disturb ",
            "Ressurect ",
            "Help ",
            "Rescue ",
            "Deflorate ",
            "Challenge ",
            "Dominate ",
            "Track ",
            "Catch ",
            "Punish ",
            "Teach ",
            "Discipline ",
            "Disgrace ",
            "Train ",
            "Enslave ",
            "Wake up ",
            "Arouse ",
            "Provoke ",
            "Subdue ",
            "Publicly undress ",
            "Have a public sex with ",
            "Compromise ",
            "Kidnap ",
            "Abduct "
        };

        private static string[] m_aItemActions = 
        {
            "Deliver the ",
            "Steal the ",
            "Seek for the ",
            "Got the ",
            "Find the ",
            "Bring the ",
            "Enchant the ",
            "Bless the ",
            "Destroy the ",
            "Break the ",
            "Burn the ",
            "Bury the ",
            "Hide the ",
            "Conceal the ",
            "Smuggle the ",
            "Solve the mystery of ",
            "Cleanse the ",
            "Purify the ",
            "Remove the curse from the ",
            "Clean the ",
            "Repair the ",
            "Restore the ",
            "Recover the ",
        };

        private static string[] m_aLocationActions = 
        {
            "Purify the ",
            "Explore the ",
            "Clean the ",
            "Seek for the secrets of the ",
            "Solve the mystery of the ",
            "Find the last virgin of the ",
            "Remove the curse from the ",
            "Defend the ",
            "Find the lost treasure of ",
            "Obtain the ancient knowlege of the ",
            "Obtain the ancient sex technique of the ",
            "Learn the ancient sex art of the ",
        };

        public Opponent m_pBoss;

        private int m_iCurrentEncounterIndex = 0;

        public int m_iProgress = 0;
        public int m_iProgressMax = 1;

        public Encounter CurrentEncounter
        {
            get { return m_cEncounters[m_iCurrentEncounterIndex]; }
        }

        public Action CurrentAction
        {
            get { return CurrentEncounter.CurrentAction; }
        }

        private World m_pWorld;
        private Vixen m_pVixen;

        private Location m_pBossLair;

        public Quest(Vixen pVixen, World pWorld)
        {
            m_pWorld = pWorld;
            m_pVixen = pVixen;

            Location[] cAvailableLocations = pWorld.AvailableLocations(pVixen.Level);
            int randomLoc = Rnd.Get(cAvailableLocations.Length);
            Location pCurrentLocation = cAvailableLocations[randomLoc];

            Encounter pWayTo = new Encounter(pVixen, pCurrentLocation, ActionType.Move);
            m_cEncounters.Add(pWayTo);

            m_iProgressMax = 5 + Rnd.Get(7);

            //m_pBossEncounter = new Encounter(pVixen, pCurrentLocation, true);
            m_pBossLair = pCurrentLocation;
            m_pBoss = pCurrentLocation.GetLover(pVixen, true);

            if (Rnd.OneChanceFrom(3))
            {
                int part1 = Rnd.Get(m_aItemActions.Length);
                m_sName = m_aItemActions[part1];
                m_sName += Reward.MakeReward(m_pBoss).m_sName.ToLower();
            }
            else
            {
                if (Rnd.OneChanceFrom(2))
                {
                    int part1 = Rnd.Get(m_aPersonActions.Length);
                    m_sName = m_aPersonActions[part1];
                    m_sName += m_pBoss.SingleName;
                }
                else
                {
                    int part1 = Rnd.Get(m_aLocationActions.Length);
                    m_sName = m_aLocationActions[part1];
                    m_sName += m_pBossLair.m_sName;
                }
            }

            m_iCurrentEncounterIndex = 0;
            m_iProgress = 0;
        }

        public bool m_bFinished = false;

        public void Advance()
        {
            if (m_bFinished)
                return;

            CurrentEncounter.Advance();
            if (CurrentEncounter.m_bFinished)
            {
                Location pCurrentLocation = CurrentEncounter.m_cActions[CurrentEncounter.m_cActions.Count - 1].m_pLocation;

                if (CurrentEncounter.m_pTarget != null)
                {
                    if (CurrentEncounter.Succcess)
                    {
                        m_iProgress++;
                        m_pVixen.Complete(CurrentEncounter);
                    }
                    else 
                    {
                        if (CurrentEncounter.m_pTarget == m_pBoss)
                            m_iProgress = (m_iProgressMax - 1) / 2 + Rnd.Get((m_iProgressMax - 1) / 2);
                        else
                            m_iProgress--;

                        if (m_iProgress < 0)
                            m_iProgress = 0;

                        m_pVixen.Lose(CurrentEncounter);
                    }
                 
                    if (m_iProgress >= m_iProgressMax)
                    {
                        m_iCurrentEncounterIndex = m_cEncounters.Count - 1;
                        m_bFinished = true;
                        return;
                    }

                    Encounter pResting = new Encounter(m_pVixen, pCurrentLocation, ActionType.Rest);
                    m_cEncounters.Add(pResting);
                    m_iCurrentEncounterIndex++;
                    return;
                }

                if (Rnd.Get(m_pVixen.MaxEncumbrance * m_pVixen.MaxEncumbrance) < m_pVixen.m_iEncumbrance * m_pVixen.m_iEncumbrance && 
                    CurrentEncounter.m_cActions[0].m_eType != ActionType.Move)
                {
                    Encounter pMarket = new Encounter(m_pVixen, m_pWorld, pCurrentLocation);
                    m_cEncounters.Add(pMarket);
                    m_iCurrentEncounterIndex++;
                    return;
                }

                if (m_iProgress + 1 >= m_iProgressMax)
                {
                    Encounter pBossEncounter = new Encounter(m_pVixen, m_pBoss, pCurrentLocation, m_pBossLair);
                    m_cEncounters.Add(pBossEncounter);
                    m_iCurrentEncounterIndex++;
                    return;
                }

                if (Rnd.OneChanceFrom(7) && CurrentEncounter.m_cActions[0].m_eType != ActionType.Move)// || m_pVixen.Level > pCurrentLocation.m_iLevel)
                {
                    Location[] cAvailableLocations = m_pWorld.AvailableLocations(m_pVixen.Level);
                    int randomLoc;
                    do
                    {
                        randomLoc = Rnd.Get(cAvailableLocations.Length);
                    }
                    while (pCurrentLocation == cAvailableLocations[randomLoc] && cAvailableLocations.Length > 1);

                    pCurrentLocation = cAvailableLocations[randomLoc];

                    Encounter pMoving = new Encounter(m_pVixen, pCurrentLocation, ActionType.Move);
                    m_cEncounters.Add(pMoving);
                    m_iCurrentEncounterIndex++;
                }
                else
                {
                    Encounter pNewEncounter = new Encounter(m_pVixen, pCurrentLocation);
                    m_cEncounters.Add(pNewEncounter);
                    m_iCurrentEncounterIndex++;
                }
            }
        }
    }
}
