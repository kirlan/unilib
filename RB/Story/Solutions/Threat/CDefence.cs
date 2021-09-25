using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RB.Story.Adventures;
using RB.Story.Troubles.Harm;

namespace RB.Story.Solutions.Threat
{
    public abstract class CDefence : CThreatPrevention
    {
        private CAcquisition m_pInitialInformationSeeking;

        public CAcquisition InitialInformationSeeking
        {
            get { return m_pInitialInformationSeeking; }
        }

        public CDefence(CHarm pThreat, CHarmLiquidation pFailure)
            : base(pThreat, pFailure)
        {
            m_pInitialInformationSeeking = new CAcquisition(Threat.Villain, 1);

            m_pGoal = m_pInitialInformationSeeking;
        }
    }
}
