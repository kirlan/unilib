using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RB.Geography
{
    public class CLink
    {
        public CLocation m_pLocation1;
        public CLocation m_pLocation2;

        private double m_fDistance;

        public int Cost
        {
            get
            {
                return (int)(m_fDistance * (m_pLocation1.Territory.m_iMovementCost + m_pLocation2.Territory.m_iMovementCost) / 2);
            }
        }

        public CLink(CLocation pLoc1, CLocation pLoc2)
        {
            m_pLocation1 = pLoc1;
            m_pLocation2 = pLoc2;

            m_fDistance = Math.Sqrt((pLoc1.X - pLoc2.X) * (pLoc1.X - pLoc2.X) + (pLoc1.Y - pLoc2.Y) * (pLoc1.Y - pLoc2.Y));
        }
    }
}
