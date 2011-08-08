using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;
using NameGen;

namespace VQmaptest
{
    public class StateInfo
    {
        public string m_sName;
        public int m_iRank;

        public string m_sRulerM;
        public string m_sRulerF;

        public string m_sHeirM;
        public string m_sHeirF;

        public StateInfo(string sName, int iRank, string sRulerM, string sRulerF, string sHeirM, string sHeirF)
        {
            m_sName = sName;
            m_iRank = iRank;
            m_sRulerM = sRulerM;
            m_sRulerF = sRulerF;
            m_sHeirM = sHeirM;
            m_sHeirF = sHeirF;
        }
    }

    public class State
    {
        public string m_sName;

        private static StateInfo[] m_aPlace = 
        {
            new StateInfo("Empire", 17, "Emperor", "Empress", "Prince", "Princess"),
            new StateInfo("Kingdom", 16, "King", "Queen", "Prince", "Princess"),
            new StateInfo("Order", 15, "Grandmagister", "Grandmagistress", "Magister", "Magistress"),
            new StateInfo("Tribes", 14, "Patriarch", "Matriarch", "Warlord", "Princess"),
        };

        public World m_pWorld;

        public string m_sShortName;

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

        public StateInfo m_pInfo;

        public State(World pWorld)
        {
            m_pWorld = pWorld;

            int iPlace = Rnd.Get(m_aPlace.Length);
            m_pInfo = m_aPlace[iPlace];

            GenerateName();
        }
    }
}
