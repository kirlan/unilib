﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using LandscapeGeneration;
using Socium.Settlements;
using LandscapeGeneration.PathFind;

namespace Socium
{
    internal class SeaRouteBuilderInfo
    {
        public LocationX m_pFrom = null;
        public float m_fCost = 0;
    }

    public class LocationX : Location
    {
        public string m_sName = "";

        public Settlement m_pSettlement = null;

        public BuildingStandAlone m_pBuilding = null;

        /// <summary>
        /// Список дорог по типам. Ключи: 1 - просёлок, 2- обычная, 3 - трасса
        /// </summary>
        public Dictionary<RoadQuality, List<Road>> m_cRoads = new Dictionary<RoadQuality, List<Road>>();

        public Dictionary<LocationX, Road> m_cHaveRoadTo = new Dictionary<LocationX,Road>();
        public List<LocationX> m_cHaveSeaRouteTo = new List<LocationX>();

        /// <summary>
        /// ТОЛЬКО для построения морских путей
        /// </summary>
        internal Dictionary<LocationX, SeaRouteBuilderInfo> m_cSeaRouteBuildInfo = new Dictionary<LocationX, SeaRouteBuilderInfo>();

        public LocationX()
            :base()
        {
            m_cRoads[RoadQuality.Country] = new List<Road>();
            m_cRoads[RoadQuality.Normal] = new List<Road>();
            m_cRoads[RoadQuality.Good] = new List<Road>();
        }

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
            m_cRoads.Clear();
            m_cRoads[RoadQuality.Country] = new List<Road>();
            m_cRoads[RoadQuality.Normal] = new List<Road>();
            m_cRoads[RoadQuality.Good] = new List<Road>();

            m_cHaveRoadTo.Clear();
            m_cHaveSeaRouteTo.Clear();

            m_cSeaRouteBuildInfo.Clear();

            base.Reset();
        }
    }
}