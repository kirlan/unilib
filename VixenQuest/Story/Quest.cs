using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;
using VixenQuest.World;
using VixenQuest.People;

namespace VixenQuest.Story
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

        public VQAction CurrentAction
        {
            get { return CurrentEncounter.CurrentAction; }
        }

        private Universe m_pWorld;
        private Vixen m_pVixen;

        public static Opponent GetLover(Vixen pVixen, Universe pWorld, Opponent[] aPretendents, bool bBoss)
        {
            int iTargetLevel = pVixen.EffectiveLevel;
            if (bBoss)
                iTargetLevel += (int)Math.Sqrt(iTargetLevel);

            Opponent pBestPretendent = null;
            int iBestDifference = int.MaxValue;

            int iCounter = 0;
            int index;
            do
            {
                index = Rnd.Get(aPretendents.Length);

                int iDifference = Math.Abs(aPretendents[index].EncounterRank - iTargetLevel);

                Location[] aPath = Universe.GetPathTo(pVixen, aPretendents[index]);
                iDifference += aPath.Length;

                if (aPretendents[index].EncounterRank < iTargetLevel)
                    iDifference = iDifference * iDifference;

                if (aPretendents[index].m_pRace.m_eSapience == Sapience.Animal)
                    if (bBoss)
                        iDifference = int.MaxValue;
                    else
                        iDifference = iDifference * 2;

                if (!pVixen.WannaFuck(aPretendents[index]) && (!aPretendents[index].WannaFuck(pVixen) || bBoss))
                    iDifference = int.MaxValue;

                if (pBestPretendent == null || iBestDifference > iDifference)
                {
                    pBestPretendent = aPretendents[index];
                    iBestDifference = iDifference;
                }

                if (Rnd.Gauss(iDifference, (int)Math.Sqrt(iTargetLevel)))
                    return aPretendents[index];

                if (iCounter++ > 1000)
                {
                    return pBestPretendent;
                }
            }
            while (true);
        }

        public Quest(Vixen pVixen, Universe pWorld)
        {
            m_pWorld = pWorld;
            m_pVixen = pVixen;

            Opponent[] cAvailableOpponents = pWorld.AvailableOpponents();
            m_pBoss = GetLover(pVixen, pWorld, cAvailableOpponents, true);

            if(pVixen.m_pHome == null)
                pVixen.m_pHome = m_pBoss.m_pHome;

            Encounter pWayTo = new Encounter(pVixen, m_pBoss.m_pHome, null);
            m_cEncounters.Add(pWayTo);

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
                    m_sName += m_pBoss.m_pHome.m_sName;
                }
            }

            m_iProgressMax = 5 + Rnd.Get(7);

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
                m_pVixen.m_pHome = CurrentEncounter.m_cActions[CurrentEncounter.m_cActions.Count - 1].m_pLocation;

                if (CurrentEncounter.m_pTarget != null)
                {
                    if (CurrentEncounter.Succcess)
                    {
                        //m_iProgress++;
                        m_pVixen.Complete(CurrentEncounter);
                    }
                    else 
                    {
                        if (CurrentEncounter.m_pTarget == m_pBoss)
                            m_iProgress = (m_iProgressMax - 1) / 2 + Rnd.Get((m_iProgressMax - 1) / 2);
                        //else
                            //m_iProgress--;

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
                }

                if (Rnd.Get(m_pVixen.MaxEncumbrance * m_pVixen.MaxEncumbrance) < m_pVixen.m_iEncumbrance * m_pVixen.m_iEncumbrance && 
                    CurrentEncounter.m_cActions[0].m_eType != ActionType.Move)
                {
                    Encounter pMarket = new Encounter(m_pVixen, m_pWorld);
                    m_cEncounters.Add(pMarket);
                    m_iCurrentEncounterIndex++;
                    return;
                }

                if (m_iCurrentEncounterIndex == m_cEncounters.Count - 1)
                {
                    m_iProgress++;

                    Opponent[] cAvailableOpponents = m_pWorld.AvailableOpponents();
                    Opponent pNewOpponent = GetLover(m_pVixen, m_pWorld, cAvailableOpponents, false);

                    if (m_iProgress + 1 >= m_iProgressMax)
                        pNewOpponent = m_pBoss;

                    Location[] aPath = Universe.GetPathTo(m_pVixen, pNewOpponent);

                    for (int i=0; i<aPath.Length; i++)
                    {
                        Location pWayPoint = aPath[i];
                        if (i + 1 < aPath.Length)
                        {
                            Location pDestination = aPath[i + 1];
                            if (Rnd.OneChanceFrom(2))
                            {
                                Encounter pMoving = new Encounter(m_pVixen, pWayPoint, pDestination);
                                m_cEncounters.Add(pMoving);
                            }
                            else
                            {
                                Opponent pRandomOpponent = GetLover(m_pVixen, m_pWorld, pWayPoint.m_cDwellers.ToArray(), false);
                                Encounter pMoving = new Encounter(m_pVixen, pRandomOpponent, pDestination);
                                m_cEncounters.Add(pMoving);
                            }
                        }
                    }
                    Encounter pMiniBoss = new Encounter(m_pVixen, pNewOpponent, null);
                    m_cEncounters.Add(pMiniBoss);
                }

                m_iCurrentEncounterIndex++;
            }
        }

        public override string ToString()
        {
            return m_sName;
        }
    }
}
