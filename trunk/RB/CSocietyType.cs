using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RB
{
    class CSocietyType
    {
        private int m_iLocationsCanOwn;
        private string m_sName;

        public string Name
        {
            get { return m_sName; }
        }

        private Dictionary<int, string> m_cRanks = new Dictionary<int, string>();

        public Dictionary<int, string> Ranks
        {
            get { return m_cRanks; }
        }

        public CSocietyType()
        {
            m_iLocationsCanOwn = 5;
            m_sName = "Kingdom";

            m_cRanks[0] = "Slave";
            m_cRanks[1] = "Servant";
            m_cRanks[2] = "Freeman";
            m_cRanks[3] = "Burgher";
            m_cRanks[4] = "Noble";
            m_cRanks[5] = "Ruling Family";
        }
    }
}
