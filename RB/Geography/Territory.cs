using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RB.Genetix;
using RB.Socium;

namespace RB.Geography
{
    public class Territory : ITerritory
    {
        #region Static Members
        public enum TerritoryType
        {
            Undefined,
            DeadSea,
            DeadMountains,
            DeadSwamp,
            DeadDesert,
            Plains,
            Hills,
            Forest,
            Wastes
        }

        private static Dictionary<TerritoryType, Territory> s_cTerritories = new Dictionary<TerritoryType, Territory>();

        public static Territory[] AllTerritories
        {
            get
            {
                return s_cTerritories.Values.ToArray();
            }
        }

        public static Territory Undefined
        {
            get
            {
                if (!s_cTerritories.ContainsKey(TerritoryType.Undefined))
                    s_cTerritories[TerritoryType.Undefined] = new Territory(TerritoryType.Undefined, LandscapeProperty.None, int.MaxValue, "Unknown");

                return s_cTerritories[TerritoryType.Undefined];
            }
        }

        public static Territory DeadSea
        {
            get
            {
                if (!s_cTerritories.ContainsKey(TerritoryType.DeadSea))
                {
                    s_cTerritories[TerritoryType.DeadSea] = new Territory(TerritoryType.DeadSea, LandscapeProperty.Flat | LandscapeProperty.Open | LandscapeProperty.Soft | LandscapeProperty.Wet, 10, "sea");
                    s_cTerritories[TerritoryType.DeadSea].SetResources(0, 3, 0, 0);
                }

                return s_cTerritories[TerritoryType.DeadSea];
            }
        }

        public static Territory DeadMountains
        {
            get
            {
                if (!s_cTerritories.ContainsKey(TerritoryType.DeadMountains))
                {
                    s_cTerritories[TerritoryType.DeadMountains] = new Territory(TerritoryType.DeadMountains, LandscapeProperty.Open | LandscapeProperty.Habitable, 10, "mountains");
                    s_cTerritories[TerritoryType.DeadMountains].SetResources(0, 0.5f, 0, 10);
                }

                return s_cTerritories[TerritoryType.DeadMountains];
            }
        }

        public static Territory DeadSwamp
        {
            get
            {
                if (!s_cTerritories.ContainsKey(TerritoryType.DeadSwamp))
                {
                    s_cTerritories[TerritoryType.DeadSwamp] = new Territory(TerritoryType.DeadSwamp, LandscapeProperty.Flat | LandscapeProperty.Open | LandscapeProperty.Soft | LandscapeProperty.Wet | LandscapeProperty.Habitable, 10, "marshes");
                    s_cTerritories[TerritoryType.DeadSwamp].SetResources(0, 0.2f, 1, 0);
                }

                return s_cTerritories[TerritoryType.DeadSwamp];
            }
        }

        public static Territory DeadDesert
        {
            get
            {
                if (!s_cTerritories.ContainsKey(TerritoryType.DeadDesert))
                {
                    s_cTerritories[TerritoryType.DeadDesert] = new Territory(TerritoryType.DeadDesert, LandscapeProperty.Flat | LandscapeProperty.Open | LandscapeProperty.Soft | LandscapeProperty.Hot | LandscapeProperty.Habitable, 10, "desert");
                    s_cTerritories[TerritoryType.DeadDesert].SetResources(0, 0.1f, 0, 0);
                }

                return s_cTerritories[TerritoryType.DeadDesert];
            }
        }

        public static Territory Plains
        {
            get
            {
                if (!s_cTerritories.ContainsKey(TerritoryType.Plains))
                {
                    s_cTerritories[TerritoryType.Plains] = new Territory(TerritoryType.Plains, LandscapeProperty.Flat | LandscapeProperty.Open | LandscapeProperty.Habitable, 1, "plains");
                    s_cTerritories[TerritoryType.Plains].SetResources(3, 0, 0, 0);
                }

                return s_cTerritories[TerritoryType.Plains];
            }
        }

        public static Territory Hills
        {
            get
            {
                if (!s_cTerritories.ContainsKey(TerritoryType.Hills))
                {
                    s_cTerritories[TerritoryType.Hills] = new Territory(TerritoryType.Hills, LandscapeProperty.Flat | LandscapeProperty.Habitable, 5, "hills");
                    s_cTerritories[TerritoryType.Hills].SetResources(1, 0.5f, 0, 5);
                }

                return s_cTerritories[TerritoryType.Hills];
            }
        }

        public static Territory Forest
        {
            get
            {
                if (!s_cTerritories.ContainsKey(TerritoryType.Forest))
                {
                    s_cTerritories[TerritoryType.Forest] = new Territory(TerritoryType.Forest, LandscapeProperty.Habitable, 3, "forest");
                    s_cTerritories[TerritoryType.Forest].SetResources(0, 1, 5, 0);
                }

                return s_cTerritories[TerritoryType.Forest];
            }
        }

        public static Territory Wastes
        {
            get
            {
                if (!s_cTerritories.ContainsKey(TerritoryType.Wastes))
                {
                    s_cTerritories[TerritoryType.Wastes] = new Territory(TerritoryType.Wastes, LandscapeProperty.Flat | LandscapeProperty.Open | LandscapeProperty.Hot | LandscapeProperty.Habitable, 2, "wastes");
                    s_cTerritories[TerritoryType.Wastes].SetResources(0.1f, 0.2f, 0, 0);
                }

                return s_cTerritories[TerritoryType.Wastes];
            }
        }
        #endregion

        private TerritoryType m_eType = TerritoryType.Undefined;

        public TerritoryType Type
        {
            get { return m_eType; }
        }

        private LandscapeProperty m_eProperties;

        public LandscapeProperty LandProperties
        {
            get { return m_eProperties; }
        }

        public enum Resource
        {
            Grain,
            Game,
            Wood,
            Ore
        }

        private Dictionary<Resource, float> m_cResources = new Dictionary<Resource, float>();

        public Dictionary<Resource, float> Resources
        {
            get { return m_cResources; }
        }

        public void SetResources(float fGrain, float fGame, float fWood, float fOre)
        {
            m_cResources[Resource.Grain] = fGrain;
            m_cResources[Resource.Game] = fGame;
            m_cResources[Resource.Wood] = fWood;
            m_cResources[Resource.Ore] = fOre;
        }

        public int m_iMovementCost = 100;
        public string m_sName;

        public Territory(TerritoryType eType, LandscapeProperty eProperties, int iMovementCost, string sName)
        {
            m_eProperties = eProperties;
            m_eType = eType;
            m_iMovementCost = iMovementCost;
            m_sName = sName;
        }

        public override string ToString()
        {
            return m_sName;
        }

        /// <summary>
        /// Вычисляет условную стоимость заселения территории указанной расой, в соответсвии с ландшафтом и фенотипом расы.
        /// Возвращает значение в диапазоне 1-100. 
        /// 1 - любая территория, идеально подходящая указанной расе (горы для гномов). Так же - простая для заселения территория, просто подходящая указанной расе.
        /// 10 - простая для заселения территория (равнины), но совсем не подходящая указанной расе (горы для эльфов). Так же - максимально сложная для заселения территория, просто подходящая указанной расе (горы для людей).
        /// 100 - максимально сложная для заселения территория (непроходимые горы), совсем не подходящая указанной расе.
        /// </summary>
        /// <param name="pNation"></param>
        /// <returns></returns>
        public int GetClaimingCost(CNation pNation)
        {
            if (!m_eProperties.HasFlag(LandscapeProperty.Habitable))
                return -1;

            float fCost = m_iMovementCost; // 1 - 10

            if (pNation.m_aPreferredLands.Contains(this))
                fCost /= 10;// (float)pLand.Type.m_iMovementCost;//2;

            if (pNation.m_aHatedLands.Contains(this))
                fCost *= 10;// (float)pLand.Type.m_iMovementCost;//2;

            if (pNation.m_bHegemon)
                fCost /= 2;

            if (fCost < 1)
                fCost = 1;

            if (fCost > int.MaxValue)
                fCost = int.MaxValue - 1;

            return (int)fCost;
        }
    }

}
