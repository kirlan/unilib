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

        public SimpleVector3d m_pDrift;

        public int m_iMaxSize = -1;

        public LandMass()
            : base()
        {
            m_pDrift = new SimpleVector3d(1.0 - Rnd.Get(2.0f), 1.0 - Rnd.Get(2.0f), 0);
            m_pDrift = m_pDrift / !m_pDrift;
            //m_iElevation = (int)(Math.Tan(1.2f - Rnd.Get(2.22f)));
        }

        public override void Start(Land pSeed)
        {
            base.Start(pSeed);

            pSeed.SetOwner(this);

            if (Rnd.OneChanceFrom(2))
                m_iMaxSize = Rnd.Get(5) + 1;
        }

        public override Land Grow(int iMaxSize)
        {
            if (m_iMaxSize > 0 && Contents.Count >= m_iMaxSize)
                return null;

            return base.Grow(iMaxSize);
        }

        public bool HaveForbiddenBorders()
        {
            foreach (var pLand in m_cBorder)
                if (pLand.Key.Forbidden)
                    return true;

            return false;
        }

        public string GetLandsString()
        {
            string sLands = "";

            foreach (var pLand in Contents)
                sLands += String.Format("({0}), ", pLand.GetLandsString());

            return sLands;
        }

        public override string ToString()
        {
            return GetLandsString();

            //return (IsWater ? "ocean": "land") + " " + Contents.Count.ToString(); 
        }

        public override float GetMovementCost()
        {
            return (IsWater ? 10 : 1);// *Contents.Count;
        }
    }
}
