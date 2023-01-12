using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;
using SimpleVectors;

namespace LandscapeGeneration
{
    /// <summary>
    /// Тектоническая плита, дрейф которых образует горы и впадины. Является объединением группы сопредельных земель (<see cref="Land"/>).
    /// Может быть или полностью затоплена океаном, или являться частью какого-то континента (<see cref="Continent"/>).
    /// </summary>
    /// <typeparam name="LAND"></typeparam>
    public class LandMass : TerritoryCluster<LandMass, Continent, Land>
    {
        public bool IsWater { get; set; } = false;

        public SimpleVector3d Drift { get; }

        public int MaxSize { get; private set; } = -1;

        public LandMass()
            : base()
        {
            Drift = new SimpleVector3d(1.0 - Rnd.Get(2.0f), 1.0 - Rnd.Get(2.0f), 0);
            Drift /= !Drift;
        }

        public override void Start(Land pSeed)
        {
            base.Start(pSeed);

            pSeed.SetOwner(this);

            if (Rnd.OneChanceFrom(2))
                MaxSize = Rnd.Get(5) + 1;
        }

        public override Land Grow(int iMaxSize)
        {
            if (MaxSize > 0 && Contents.Count >= MaxSize)
                return null;

            return base.Grow(iMaxSize);
        }

        public bool HaveForbiddenBorders()
        {
            foreach (var pLand in Border)
            {
                if (pLand.Key.Forbidden)
                    return true;
            }

            return false;
        }

        public string GetLandsString()
        {
            StringBuilder sLands = new StringBuilder();

            foreach (var pLand in Contents)
                sLands.AppendFormat("({0}), ", pLand.GetLandsString());

            return sLands.ToString();
        }

        public override string ToString()
        {
            return GetLandsString();

            //return (IsWater ? "ocean": "land") + " " + Contents.Count.ToString(); 
        }

        public override float GetMovementCost()
        {
            return IsWater ? 10 : 1;
        }
    }
}
