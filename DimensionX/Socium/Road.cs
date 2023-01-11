using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LandscapeGeneration.PathFind;

namespace Socium
{
    public class Road
    {
        private readonly List<LocationX> m_cRoad = new List<LocationX>();

        public LocationX[] Locations
        {
            get { return m_cRoad.ToArray(); }
        }

        public RoadQuality Level { get; }

        public Road(LocationX pStart, RoadQuality eLevel)
        {
            m_cRoad.Add(pStart);
            Level = eLevel;
        }

        public void BuidTo(LocationX pNext)
        {
            m_cRoad.Add(pNext);
        }
    }
}
