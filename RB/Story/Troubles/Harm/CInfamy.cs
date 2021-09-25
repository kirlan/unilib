using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RB.Socium;

namespace RB.Story.Troubles.Harm
{
    public class CInfamy : CStatusHarm
    {
        public CInfamy(CPerson pVictim, CPerson pVillain)
            : base(pVictim, pVillain)
        {
            m_sName = pVictim.ToString() + " опозорен";
        }
    }
}
