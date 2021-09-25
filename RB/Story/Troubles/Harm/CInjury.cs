﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RB.Socium;

namespace RB.Story.Troubles.Harm
{
    public class CInjury : CMaterialHarm
    {
        public CInjury(CPerson pVictim, CPerson pVillain)
            : base(pVictim, pVillain)
        {
            m_sName = pVictim.ToString() + " ранен";
        }
    }
}
