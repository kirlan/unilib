using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MinCostFlow
{
    public class CLink
    {
        public override string ToString()
        {
            return string.Format("{0}/{1} ({2})", m_iCurrentFlow, m_iCapacity, m_iCost);
        }

        long m_iCapacity;
        public long Capacity
        {
            get { return m_iCapacity; }
        }

        long m_iCurrentFlow;
        public long CurrentFlow
        {
            get { return m_iCurrentFlow; }
            set 
            { 
                m_iCurrentFlow = value;
                if (m_iCurrentFlow > m_iCapacity && m_iCurrentFlow > 0)
                    m_iCurrentFlow = m_iCapacity;
            }
        }

        long m_iCost;
        public long Cost
        {
            get 
            {
                if (m_iCapacity > m_iCurrentFlow)
                    return m_iCost;
                else
                    return int.MaxValue;
            }
        }

        public CLink(int iCapacity, int iCost)
        {
            m_iCapacity = iCapacity;
            m_iCost = iCost;
        }
    }
}
