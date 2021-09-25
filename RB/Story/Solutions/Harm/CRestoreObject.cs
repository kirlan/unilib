using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RB.Story.Adventures;
using RB.Story.Troubles.Harm;

namespace RB.Story.Solutions.Harm
{
    public class CRestoreObject : CDirectLiquidation
    {
        private CAcquisition m_pObjectSeeking;
        public CAcquisition ObjectSeeking
        {
            get { return m_pObjectSeeking; }
        }

        public CRestoreObject(CHarm pHarm)
            : base(pHarm)
        {
            m_sName = "Поиски утраченного";

            m_pObjectSeeking = new CAcquisition(Harm.Villain, 1);
            m_pGoal = m_pObjectSeeking;
        }
    }
}
