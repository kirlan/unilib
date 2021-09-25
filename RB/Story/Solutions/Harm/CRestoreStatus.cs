using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RB.Story.Adventures;
using RB.Story.Troubles.Harm;

namespace RB.Story.Solutions.Harm
{
    public class CRestoreStatus : CDirectLiquidation
    {
        private List<CAcquisition> m_cProofsSeeking = new List<CAcquisition>();
        public List<CAcquisition> ProofsSeeking
        {
            get { return m_cProofsSeeking; }
        }

        public CRestoreStatus(CHarm pHarm, int iProofsCount)
            : base(pHarm)
        {
            m_sName = "Доказательство невиновности";

            for (int i = 0; i < iProofsCount; i++)
            {
                CAcquisition pProofSeeking = new CAcquisition(Harm.Villain, 1);
                m_cProofsSeeking.Add(pProofSeeking);
                m_pGoal = pProofSeeking;
            }
        }
    }
}
