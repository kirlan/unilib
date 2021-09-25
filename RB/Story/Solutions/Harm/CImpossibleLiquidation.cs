using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RB.Story.Troubles.Harm;

namespace RB.Story.Solutions.Harm
{
    public class CImpossibleLiquidation : CHarmLiquidation
    {
        public CImpossibleLiquidation(CHarm pHarm)
            : base(pHarm)
        {
            m_sName = "Ничего исправить нельзя";
        }
    }
}
