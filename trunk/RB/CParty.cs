using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RB
{
    class CParty
    {
        CPerson m_pLeader;

        internal CPerson Leader
        {
            get { return m_pLeader; }
            set { m_pLeader = value; }
        }

        List<CPerson> m_cFollowers = new List<CPerson>();

        internal List<CPerson> Followers
        {
            get { return m_cFollowers; }
        }

        public CParty(CPerson pLeader)
        {
            m_pLeader = pLeader;
        }
    }
}
