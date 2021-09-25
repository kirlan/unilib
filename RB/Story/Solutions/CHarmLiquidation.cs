using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RB.Story.Troubles.Harm;
using RB.Story.Solutions.Threat;

namespace RB.Story.Solutions
{
    public abstract class CHarmLiquidation : CPlot
    {
        public enum _PunishmentOption
        {
            AtStart,
            AtFinish,
            Never
        }

        private CHarm m_pHarm;

        public CHarm Harm
        {
            get { return m_pHarm; }
        }
        private CVillainPunishment m_pPunishment = null;

        public CVillainPunishment Punishment
        {
            get { return m_pPunishment; }
        }

        private _PunishmentOption m_ePunishmentOption;

        public _PunishmentOption PunishmentOption
        {
            get { return m_ePunishmentOption; }
            set { m_ePunishmentOption = value; }
        }

        public CHarmLiquidation(CHarm pHarm)
            : base()
        {
            m_pHarm = pHarm;
        }
    }
}
