using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VixenQuest
{
    class StoryLine
    {
        public List<Adventure> m_cAdventures = new List<Adventure>();

        private Vixen m_pVixen;
        private World m_pWorld;

        public StoryLine(Vixen pVixen, World pWorld)
        {
            m_pVixen = pVixen;
            m_pWorld = pWorld;
            GenerateNewAdventure("Prologue", 10);
        }

        private void GenerateNewAdventure(string sName, int iLength)
        {
            Adventure pNewAdventure = new Adventure(sName, m_pVixen, m_pWorld, iLength);

            m_cAdventures.Add(pNewAdventure);
        }

        private int m_iCurrentAdventure = 0;

        public Adventure CurrentAdventure
        {
            get { return m_cAdventures[m_iCurrentAdventure]; }
        }
        public Quest CurrentQuest
        {
            get { return CurrentAdventure.CurrentQuest; }
        }
        public Action CurrentAction
        {
            get { return CurrentQuest.CurrentAction; }
        }

        public Encounter CurrentEncounter
        {
            get { return CurrentQuest.CurrentEncounter; }
        }

        public void Advance()
        {
            CurrentAdventure.Advance();
            if (CurrentAdventure.m_bFinished)
            {
                m_iCurrentAdventure++;

                if (m_iCurrentAdventure >= m_cAdventures.Count)
                    GenerateNewAdventure("Act " + m_iCurrentAdventure.ToString(), m_iCurrentAdventure*2+10);
            }
        }
    }
}
