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
    public class LocationX : TerritoryExtended<LocationX, Location>
    {
        public Settlement Settlement { get; set; } = null;

        public BuildingStandAlone Building { get; set; } = null;

        /// <summary>
        /// Список дорог по типам. Ключи: 1 - просёлок, 2- обычная, 3 - трасса
        /// </summary>
        public Dictionary<RoadQuality, List<Road>> Roads { get; } = new Dictionary<RoadQuality, List<Road>>();

        public Dictionary<LocationX, Road> HaveRoadTo { get; } = new Dictionary<LocationX, Road>();
        public List<LocationX> HaveSeaRouteTo { get; } = new List<LocationX>();

        /// <summary>
        /// ТОЛЬКО для построения морских путей
        /// </summary>
        internal Dictionary<LocationX, SeaRouteBuilderInfo> m_cSeaRouteBuildInfo = new Dictionary<LocationX, SeaRouteBuilderInfo>();

        public LocationX(Location pLoc) : base(pLoc)
        {
            Roads[RoadQuality.Country] = new List<Road>();
            Roads[RoadQuality.Normal] = new List<Road>();
            Roads[RoadQuality.Good] = new List<Road>();
        }

        public LocationX()
        { }

        public int GetPopulation()
        {
            int iRes = 0;

            if (Settlement != null)
            {
                iRes = Settlement.Profile.MinPop;
            }

            return iRes;
        }

        public Province OwnerProvince
        {
            get
            {
                LandX pLand = Origin.GetOwner().As<LandX>();
                return pLand.GetOwner().GetOwner();
            }
        }

        public State OwnerState
        {
            get
            {
                return OwnerProvince.GetOwner();
            }
        }

        public bool HaveEstate(Estate.SocialRank eEstate)
        {
            StateSociety pOwnerSociety = OwnerState.m_pSociety;

            if (Settlement != null && pOwnerSociety.Estates.ContainsKey(eEstate))
            {
                Estate pEstate = pOwnerSociety.Estates[eEstate];

                foreach (Building pBuilding in Settlement.Buildings)
                {
                    if (pEstate.GenderProfessionPreferences.ContainsKey(pBuilding.Info.OwnerProfession))
                    {
                        List<Person> cOwners = new List<Person>();
                        foreach (Person pDweller in pBuilding.Persons)
                        {
                            if (pDweller.Profession == pBuilding.Info.OwnerProfession)
                                cOwners.Add(pDweller);
                        }

                        if (cOwners.Count < pBuilding.Info.OwnersCount)
                            return true;
                    }

                    if (pEstate.GenderProfessionPreferences.ContainsKey(pBuilding.Info.WorkersProfession))
                    {
                        List<Person> cWorkers = new List<Person>();
                        foreach (Person pDweller in pBuilding.Persons)
                        {
                            if (pDweller.Profession == pBuilding.Info.WorkersProfession)
                                cWorkers.Add(pDweller);
                        }

                        if (cWorkers.Count < pBuilding.Info.WorkersCount)
                            return true;
                    }
                }
            }

            return false;
        }

        public override string ToString()
        {
            if (Settlement != null)
                return string.Format("{0} {1}", Settlement.ToString(), base.ToString());

            if (Building != null)
                return string.Format("{0} {1}", Building.ToString(), base.ToString());

            return string.Format("{0} {1}", Origin.Landmark, base.ToString());
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
