using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RB.Story.Adventures;
using RB.Story.Troubles.Harm;

namespace RB.Story.Solutions.Threat
{
    public class CDefenceSeeking : CDefence
    {
        private CAcquisition m_pDefenceSeeking;

        public CAcquisition DefenceSeeking
        {
            get { return m_pDefenceSeeking; }
        }

        public CDefenceSeeking(CHarm pThreat, CHarmLiquidation pFailure)
            : base(pThreat, pFailure)
        {
            m_sName = "Поиски защиты";

            m_pDefenceSeeking = new CAcquisition(Threat.Villain, 1);

            m_pGoal = m_pDefenceSeeking;
        }
    }
}
