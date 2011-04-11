using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CinemaEngine
{
    /// <summary>
    /// Последовательность эпизодов, объединённая общим списком действующих лиц
    /// </summary>
    class Scenario
    {
        private string m_sName;

        public string Name
        {
            get { return m_sName; }
            set { m_sName = value; }
        }
        
        private List<Character> m_cRoles = new List<Character>();

        public List<Character> Roles
        {
            get { return m_cRoles; }
        }

        private List<Episode> m_cEpisodes;

        internal List<Episode> Episodes
        {
            get { return m_cEpisodes; }
        }
    }
}
