using SimpleWorld.Geography;
using System.Collections.Generic;
using System.Linq;

namespace LostIsland
{
    public class CLostTerritory : ITerritory
    {
        #region Static Members
        public enum TerritoryType
        {
            //Undefined,
            DeepSea,
            Mountains,
            Shallows,
            Swamp,
            Desert,
            Plains,
            Hills,
            Forest,
            Wastes
        }

        private static readonly Dictionary<TerritoryType, CLostTerritory> s_cTerritories = new Dictionary<TerritoryType, CLostTerritory>();

        public static CLostTerritory[] AllTerritories
        {
            get
            {
                return s_cTerritories.Values.ToArray();
            }
        }

        //public static CLostTerritory Undefined
        //{
        //    get
        //    {
        //        if (!s_cTerritories.ContainsKey(TerritoryType.Undefined))
        //            s_cTerritories[TerritoryType.Undefined] = new CLostTerritory(TerritoryType.Undefined, int.MaxValue, "Unknown");

        //        return s_cTerritories[TerritoryType.Undefined];
        //    }
        //}

        public static CLostTerritory DeepSea
        {
            get
            {
                if (!s_cTerritories.ContainsKey(TerritoryType.DeepSea))
                {
                    s_cTerritories[TerritoryType.DeepSea] = new CLostTerritory(TerritoryType.DeepSea, 10, "open ocean");
                }

                return s_cTerritories[TerritoryType.DeepSea];
            }
        }

        public static CLostTerritory Mountains
        {
            get
            {
                if (!s_cTerritories.ContainsKey(TerritoryType.Mountains))
                {
                    s_cTerritories[TerritoryType.Mountains] = new CLostTerritory(TerritoryType.Mountains, 10, "mountains");
                }

                return s_cTerritories[TerritoryType.Mountains];
            }
        }

        public static CLostTerritory Shallows
        {
            get
            {
                if (!s_cTerritories.ContainsKey(TerritoryType.Shallows))
                {
                    s_cTerritories[TerritoryType.Shallows] = new CLostTerritory(TerritoryType.Shallows, 2, "shallow waters");
                }

                return s_cTerritories[TerritoryType.Shallows];
            }
        }

        public static CLostTerritory Swamp
        {
            get
            {
                if (!s_cTerritories.ContainsKey(TerritoryType.Swamp))
                {
                    s_cTerritories[TerritoryType.Swamp] = new CLostTerritory(TerritoryType.Swamp, 10, "marshes");
                }

                return s_cTerritories[TerritoryType.Swamp];
            }
        }

        public static CLostTerritory Desert
        {
            get
            {
                if (!s_cTerritories.ContainsKey(TerritoryType.Desert))
                {
                    s_cTerritories[TerritoryType.Desert] = new CLostTerritory(TerritoryType.Desert, 5, "dunes");
                }

                return s_cTerritories[TerritoryType.Desert];
            }
        }

        public static CLostTerritory Plains
        {
            get
            {
                if (!s_cTerritories.ContainsKey(TerritoryType.Plains))
                {
                    s_cTerritories[TerritoryType.Plains] = new CLostTerritory(TerritoryType.Plains, 1, "plains");
                }

                return s_cTerritories[TerritoryType.Plains];
            }
        }

        public static CLostTerritory Hills
        {
            get
            {
                if (!s_cTerritories.ContainsKey(TerritoryType.Hills))
                {
                    s_cTerritories[TerritoryType.Hills] = new CLostTerritory(TerritoryType.Hills, 5, "hills");
                }

                return s_cTerritories[TerritoryType.Hills];
            }
        }

        public static CLostTerritory Forest
        {
            get
            {
                if (!s_cTerritories.ContainsKey(TerritoryType.Forest))
                {
                    s_cTerritories[TerritoryType.Forest] = new CLostTerritory(TerritoryType.Forest, 3, "forest");
                }

                return s_cTerritories[TerritoryType.Forest];
            }
        }

        public static CLostTerritory Wastes
        {
            get
            {
                if (!s_cTerritories.ContainsKey(TerritoryType.Wastes))
                {
                    s_cTerritories[TerritoryType.Wastes] = new CLostTerritory(TerritoryType.Wastes, 2, "wastes");
                }

                return s_cTerritories[TerritoryType.Wastes];
            }
        }
        #endregion

        public TerritoryType Type { get; }

        public string Name { get; }

        public int MovementCost { get; }

        public CLostTerritory(TerritoryType eType, int iMovementCost, string sName)
        {
            Type = eType;
            MovementCost = iMovementCost;
            Name = sName;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}