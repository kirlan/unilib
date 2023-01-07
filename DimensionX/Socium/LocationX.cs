using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using LandscapeGeneration;
using Socium.Settlements;
using LandscapeGeneration.PathFind;
using Socium.Population;

namespace Socium
{
    internal class SeaRouteBuilderInfo
    {
        public LocationX m_pFrom = null;
        public float m_fCost = 0;
    }

    /// <summary>
    /// расширение LandscapeGeneration.Location
    /// добавляет ссылку на поселение, дороги, отдельностоящие постройки (aka логова)
    /// </summary>
    public class LocationX : TerritoryExtended<Location>
    {
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

        public LocationX(Location pLoc) : base(pLoc)
        {
            m_cRoads[RoadQuality.Country] = new List<Road>();
            m_cRoads[RoadQuality.Normal] = new List<Road>();
            m_cRoads[RoadQuality.Good] = new List<Road>();
        }

        public int GetPopulation()
        {
            int iRes = 0;

            if (m_pSettlement != null)
            {
                iRes = m_pSettlement.m_pInfo.m_iMinPop;// *((LandX)Owner).m_pProvince.m_iInfrastructureLevel;
            }

            return iRes;
        }

        public Province OwnerProvince
        {
            get
            {
                LandX pLand = Origin.GetOwner<Land>().As<LandX>();
                return pLand.GetOwner<Region>().GetOwner<Province>();
            }
        }

        public State OwnerState
        {
            get
            {
                return OwnerProvince.GetOwner<State>();
            }
        }

        public bool HaveEstate(Estate.Position eEstate)
        {
            StateSociety pOwnerSociety = OwnerState.m_pSociety;

            if (m_pSettlement != null && pOwnerSociety.m_cEstates.ContainsKey(eEstate))
            {
                Estate pEstate = pOwnerSociety.m_cEstates[eEstate];

                foreach (Building pBuilding in m_pSettlement.m_cBuildings)
                {
                    if (pEstate.m_cGenderProfessionPreferences.ContainsKey(pBuilding.m_pInfo.m_pOwnerProfession))
                    {
                        List<Person> cOwners = new List<Person>();
                        foreach (Person pDweller in pBuilding.m_cPersons)
                            if (pDweller.m_pProfession == pBuilding.m_pInfo.m_pOwnerProfession)
                                cOwners.Add(pDweller);

                        if (cOwners.Count < pBuilding.m_pInfo.OwnersCount)
                            return true;
                    }

                    if (pEstate.m_cGenderProfessionPreferences.ContainsKey(pBuilding.m_pInfo.m_pWorkersProfession))
                    {
                        List<Person> cWorkers = new List<Person>();
                        foreach (Person pDweller in pBuilding.m_cPersons)
                            if (pDweller.m_pProfession == pBuilding.m_pInfo.m_pWorkersProfession)
                                cWorkers.Add(pDweller);

                        if (cWorkers.Count < pBuilding.m_pInfo.WorkersCount)
                            return true;
                    }
                }
            }

            return false;

        }

        public override string ToString()
        {
            if (m_pSettlement != null)
                return string.Format("{0} {1}", m_pSettlement.ToString(), base.ToString());
            
            if (m_pBuilding != null)
                return string.Format("{0} {1}", m_pBuilding.ToString(), base.ToString());

            return string.Format("{0} {1}", Origin.m_eType, base.ToString());
        }

        //public void Reset()
        //{
        //    m_pSettlement = null;
        //    m_pBuilding = null;
        //    m_cRoads.Clear();
        //    m_cRoads[RoadQuality.Country] = new List<Road>();
        //    m_cRoads[RoadQuality.Normal] = new List<Road>();
        //    m_cRoads[RoadQuality.Good] = new List<Road>();

        //    m_cHaveRoadTo.Clear();
        //    m_cHaveSeaRouteTo.Clear();

        //    m_cSeaRouteBuildInfo.Clear();

        //    //base.Reset();
        //}
    }
}
