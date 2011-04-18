using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using LandscapeGeneration;

namespace Socium
{
    public class LocationX : Location
    {
        public string m_sName = "";

        public Settlement m_pSettlement = null;

        public BuildingStandAlone m_pBuilding = null;

        public int m_iRoad = 0;

        public List<Location> m_cHaveRoadTo = new List<Location>();
        public List<Location> m_cHaveSeaRouteTo = new List<Location>();

        public override string ToString()
        {
            if (m_pSettlement != null)
                return string.Format("{2} {0} {1}", m_pSettlement.ToString(), m_pCenter.ToString(), GetStringID());
            
            if (m_pBuilding != null)
                return string.Format("{2} {0} {1}", m_pBuilding.ToString(), m_pCenter.ToString(), GetStringID());

            return string.Format("{3} {0} {1} {2}", m_eType, m_sName, m_pCenter.ToString(), GetStringID());
        }

        public override void Reset()
        {
            m_sName = "";
            m_pSettlement = null;
            m_pBuilding = null;
            m_iRoad = 0;

            m_cHaveRoadTo.Clear();
            m_cHaveSeaRouteTo.Clear();

            base.Reset();
        }
    }
}
