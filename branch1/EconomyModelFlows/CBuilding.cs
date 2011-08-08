using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EconomyModelFlows
{
    class CBuilding
    {
        Dictionary<CBuilding, CRoute> m_cRoutes = new Dictionary<CBuilding, CRoute>();

        internal Dictionary<CBuilding, CRoute> Routes
        {
            get { return m_cRoutes; }
        }

        public int GetRoutePrice(CBuilding pBuilding)
        {
            if (pBuilding == this)
                return 0;

            CRoute pRoute;
            if (m_cRoutes.TryGetValue(pBuilding, out pRoute))
                return pRoute.Price;
            else
                return int.MaxValue;
        }

        int m_iPushed;
        public int Pushed
        {
            get { return m_iPushed; }
        }

        bool m_bMarked;
        public bool Marked
        {
            get { return m_bMarked; }
        }

        int m_iCost;
        public int Cost
        {
            get { return m_iCost; }
        }

        CBuilding m_pFrom;
        internal CBuilding From
        {
            get { return m_pFrom; }
        }

        public CBuilding()
        {
            m_iMarked = 0;
            m_iPushed = 0;
            m_pFrom = null;
            m_iCost = int.MaxValue;
        }

        public bool SetRoute(CBuilding pFrom)
        {
            int iNewCost = pFrom.Cost + pFrom.GetRoutePrice(this);
            
            if (m_iCost > iNewCost)
            {
                m_iCost = iNewCost;
                m_pFrom = pFrom;
                return true;
            }

            return false;
        }

        public bool Push(CBuilding pFrom, int iCount)
        {
            if(m_bMarked || iCount <= 0)
                return false;

            m_bMarked = true;
            m_pFrom = pFrom;
            m_iPushed = iCount;
        }
    }
}
