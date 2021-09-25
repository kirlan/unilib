using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RB.Story.Adventures;
using RB.Story.Troubles.Harm;
using RB.Socium;

namespace RB.Story.Solutions.Harm
{
    public class CNewHome : CAlternateLiquidation
    {
        public CNewHome(CForcedTransportation pHarm)
            : base(pHarm)
        {
            m_sName = "Поиски нового дома";

            m_pGoal = new CAcquisition(Harm.Villain, 1);
            //m_pGoal.

            m_sDescription = string.Format("Luckily, {0} had some distant relatives in {1}. If you'll help {2} to get there, you'll be well paid!",
                pHarm.Victim.ToString(), pHarm.FarLands.FullName, pHarm.Victim.Gender == CPerson._Gender.Male ? "him" : "her");
        }
    }
}
