using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RB.Story.Adventures;
using RB.Story.Troubles.Harm;

namespace RB.Story.Solutions.Threat
{
    public class CVillainPunishment : CThreatPrevention
    {
        private CChallenge m_pConfrontation;
        internal CChallenge Confrontation
        {
            get { return m_pConfrontation; }
        }

        private CAcquisition m_pVillainSeeking;
        public CAcquisition VillainSeeking
        {
            get { return m_pVillainSeeking; }
        }

        public CVillainPunishment(CHarm pThreat, CHarmLiquidation pFailure)
            : base(pThreat, pFailure)
        {
            m_sName = "Конфронтация со злодеем";

            m_pVillainSeeking = new CAcquisition(Threat.Villain, 1);
            m_pGoal = m_pVillainSeeking;

            m_pConfrontation = new CChallenge(Threat.Villain, CChallenge.Importance.Optional, CChallenge.Occurance.Repetable);
            m_pGoal = m_pConfrontation;
        }
    }
}
