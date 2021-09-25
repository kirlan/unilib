using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RB.Story.Adventures;
using RB.Story.Troubles.Harm;

namespace RB.Story.Solutions.Harm
{
    public class CReplaceObject : CAlternateLiquidation
    {
        private CAcquisition m_pReplacementSeeking;
        public CAcquisition ReplacementSeeking
        {
            get { return m_pReplacementSeeking; }
        }

        public CReplaceObject(CHarm pHarm)
            : base(pHarm)
        {
            m_sName = "Поиски замены";

            m_pReplacementSeeking = new CAcquisition(Harm.Villain, 1);
            m_pGoal = m_pReplacementSeeking;
        }
    }
}
