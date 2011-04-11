using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EconomyModelFlows
{
    class CRoute
    {
        int m_iCapacity;
        public int Capacity
        {
            get { return m_iCapacity; }
        }

        int m_iCurrentFlow;
        public int CurrentFlow
        {
            get { return m_iCurrentFlow; }
            set { m_iCurrentFlow = value; }
        }

        int m_iPrice;
        public int Price
        {
            get 
            {
                if (m_iCapacity > m_iCurrentFlow)
                    return m_iPrice;
                else
                    return int.MaxValue; 
            }
        }

        public CRoute(int iCapacity, int iPrice)
        {
            m_iCapacity = iCapacity;
            m_iPrice = iPrice;

            m_iCurrentFlow = 0;
        }
    }
}
