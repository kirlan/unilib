using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RB.Story.Adventures;
using RB.Story.Troubles.Harm;

namespace RB.Story.Solutions.Harm
{
    public class CNewStatus : CAlternateLiquidation
    {
        private CSeekingSimple m_pOpportunitySeeking;

        public CSeekingSimple OpportunitySeeking
        {
            get { return m_pOpportunitySeeking; }
        }

        public CNewStatus(CHarm pHarm)
            : base(pHarm)
        {
            m_sName = "Обретение нового статуса";

            m_pOpportunitySeeking = new CSeekingSimple(Harm.Villain);
            m_pGoal = m_pOpportunitySeeking;
        }
    }
}
