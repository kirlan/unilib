using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;

namespace VixenQuest
{
    class World
    {
        public Dictionary<int, List<Location>> m_cLocationsByTiers = new Dictionary<int, List<Location>>();

        public Location m_pMarketplace;

        public World()
        {
            m_pMarketplace = new Location("Superior Marketplace");
        }

        public Location[] AvailableLocations(int iLevel)
        { 
            List<Location> cResult = new List<Location>();

            int iTier = 1 + iLevel / 10;

            if(iTier > 1 )
                foreach (Location loc in m_cLocationsByTiers[iTier - 1])
                    cResult.Add(loc);

            if (!m_cLocationsByTiers.ContainsKey(iTier))
                UnlockNewTier(iTier);

            foreach (Location loc in m_cLocationsByTiers[iTier])
                cResult.Add(loc);

            return cResult.ToArray();
        }

        public void UnlockNewTier(int iTier)
        {
            if (!m_cLocationsByTiers.ContainsKey(iTier) || m_cLocationsByTiers[iTier] == null)
                m_cLocationsByTiers[iTier] = new List<Location>();

            int iNewLockCount = 5 + Rnd.Get(6);
            for (int i = 0; i < iNewLockCount; i++)
            {
                m_cLocationsByTiers[iTier].Add(new Location(iTier));
            }
        }
    }
}
