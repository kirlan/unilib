using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleWorld.Geography
{
    public class CLink
    {
        public ISimpleLocation m_pLocation1;
        public ISimpleLocation m_pLocation2;

        public double Distance { get; }

        public int Cost
        {
            get
            {
                return (int)(Distance * (m_pLocation1.MovementCost + m_pLocation2.MovementCost) / 2);
            }
        }

        public CLink(ISimpleLocation pLoc1, ISimpleLocation pLoc2)
        {
            m_pLocation1 = pLoc1;
            m_pLocation2 = pLoc2;

            Distance = Math.Sqrt((pLoc1.X - pLoc2.X) * (pLoc1.X - pLoc2.X) + (pLoc1.Y - pLoc2.Y) * (pLoc1.Y - pLoc2.Y));
        }
    }
}
