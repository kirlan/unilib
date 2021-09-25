using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RB.Story.Troubles.Harm;

namespace RB.Story.Solutions
{
    public abstract class CThreatPrevention : CPlot
    {
        private CHarmLiquidation m_pFailure;
        private CHarm m_pThreat;

        public CHarm Threat
        {
            get { return m_pThreat; }
        }

        public CThreatPrevention(CHarm pThreat, CHarmLiquidation pFailure)
            : base()
        {
            m_pThreat = pThreat;
            m_pFailure = pFailure;
        }
    }
}
