using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RB.Story.Solutions;
using Random;
using RB.Story.Solutions.Harm;
using RB.Geography;
using RB.Socium;

namespace RB.Story.Troubles.Harm
{
    public class CForcedTransportation : CHarm
    {
        private CLocation m_pFarLands;
        public CLocation FarLands
        {
            get { return m_pFarLands; }
        }

        private CLocation m_pHome;
        public CLocation Home
        {
            get { return m_pHome; }
        }

        public CForcedTransportation(CPerson pVictim, CPerson pVillain, CLocation pFarLands, CLocation pHome)
            : base(pVictim, pVillain)
        {
            m_pFarLands = pFarLands;
            m_pHome = pHome;

            m_sName = pVictim.ToString() + " похищен из " + pHome.FullName;

            switch (Rnd.Get(3))
            {
                case 0:
                    m_sDescription = string.Format("{0} told you, that evil {1} takes {2} from home and left here.",
                        pVictim.ToString(), pVillain.ToString(), pVictim.Gender == CPerson._Gender.Male ? "him" : "her");
                    break;
                case 1:
                    m_sDescription = string.Format("{0} told you, that evil {1} forced {2} to run from home.",
                        pVictim.ToString(), pVillain.ToString(), pVictim.Gender == CPerson._Gender.Male ? "him" : "her");
                    break;
                case 2:
                    m_sDescription = string.Format("{0} told you, that evil {1} exiled {2} from home.",
                        pVictim.ToString(), pVillain.ToString(), pVictim.Gender == CPerson._Gender.Male ? "him" : "her");
                    break;
            }

            m_bCanBeRestored = Rnd.Chances(1, 2);
            m_bCanBeReplaced = Rnd.Chances(1, 2) || !m_bCanBeRestored;

            if (!m_bCanBeRestored)
                m_sDescription += string.Format(" Unfortunately, it seems, that after all it's impossible for {0} to return home.");
        }

        protected override CHarmLiquidation GetLiquidation()
        {
            switch (ChooseSolution())
            {
                case HarmLiquidationType.Direct:
                    return new CReturnHome(this, m_pHome);
                case HarmLiquidationType.Alternative:
                    return new CNewHome(this);
                case HarmLiquidationType.Impossible:
                    return new CImpossibleLiquidation(this);
            }

            return new CImpossibleLiquidation(this);
        }
    }
}
