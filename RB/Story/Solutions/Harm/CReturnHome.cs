using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RB.Story.Adventures;
using RB.Story.Troubles.Harm;
using RB.Geography;

namespace RB.Story.Solutions.Harm
{
    /// <summary>
    /// Возвращение домой
    /// </summary>
    public class CReturnHome : CDirectLiquidation
    {
        public CReturnHome(CHarm pHarm, CLocation pHome)
            : base(pHarm)
        {
            m_sName = "Возвращение домой";

            m_pGoal = new CSeekingSimple(pHarm.Villain);

            //m_sDescription = string.Format("If you'll help {0} to return to {1}, you'll be well paid!",
            //    pHarm.Victim.Name, ((CAbduction)pHarm).Home.FullName);
        }
    }
}
