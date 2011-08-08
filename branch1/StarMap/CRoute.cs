using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarMap
{
    public class CRoute
    {
        public List<CStarship> m_cShips = new List<CStarship>();

        public CWorld m_pFrom;
        public CWorld m_pTo;

        public CRoute(CWorld pFrom, CWorld pTo, CStarship pFounder)
        {
            m_pFrom = pFrom;
            m_pTo = pTo;
            m_cShips.Add(pFounder);

            m_pFrom.m_cOutcoming.Add(this);
            m_pTo.m_cIncoming.Add(this);
            pFounder.m_pRoute = this;
        }
    }
}
