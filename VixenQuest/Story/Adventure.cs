using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;
using VixenQuest.World;
using VixenQuest.People;

namespace VixenQuest.Story
{
    class Adventure
    {
        public List<Quest> m_cQuests = new List<Quest>();

        public string m_sName;

        private int m_iQuestsCount;

        private Vixen m_pVixen;
        private Universe m_pWorld;

        public Adventure(string sName, Vixen pVixen, Universe pWorld, int iLength)
        {
            m_sName = sName;
            m_pWorld = pWorld;
            m_pVixen = pVixen;

            m_iQuestsCount = iLength/2 + Rnd.Get(iLength);

            Quest pNewQuest = new Quest(pVixen, pWorld);
            m_cQuests.Add(pNewQuest);
        }

        public int m_iCurrentQuestIndex = 0;

        public Quest CurrentQuest
        {
            get { return m_cQuests[m_iCurrentQuestIndex]; }
        }
        public VQAction CurrentAction
        {
            get { return CurrentQuest.CurrentAction; }
        }

        public bool m_bFinished = false;

        public void Advance()
        {
            if (m_bFinished)
                return;

            CurrentQuest.Advance(); 
            if (CurrentQuest.m_bFinished)
            {
                m_iCurrentQuestIndex++;

                if (m_iCurrentQuestIndex >= m_iQuestsCount)
                {
                    m_iCurrentQuestIndex = m_cQuests.Count - 1;
                    m_bFinished = true;
                    return;
                }

                if (m_iCurrentQuestIndex >= m_cQuests.Count)
                {
                    Quest pNewQuest = new Quest(m_pVixen, m_pWorld);
                    m_cQuests.Add(pNewQuest);
                }
            }
        }
    }
}
