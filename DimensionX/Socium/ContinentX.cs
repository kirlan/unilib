using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;
using NameGen;
using LandscapeGeneration;
using Socium.Nations;

namespace Socium
{
    /// <summary>
    /// расширение LandscapeGeneration.Continent
    /// добавлены списки регионов, государств, имя, метод для постройки регионов 
    /// </summary>
    public class ContinentX: TerritoryExtended<ContinentX, Continent>
    {
        public List<Region> m_cRegions = new List<Region>();

        public List<State> Contents { get; } = new List<State>();

        public string m_sName;

        public ContinentX(Continent pOrigin) : base(pOrigin)
        {
            m_cRegions.Clear();

            m_sName = NameGenerator.GetAbstractName();
        }

        public ContinentX()
        { }

        public override string ToString()
        {
            return m_sName;
        }

        public Dictionary<LandMass, List<Nation>> m_cLocalNations = new Dictionary<LandMass,List<Nation>>();

        public void BuildRegions(float fCycleShift, int iMaxSize)
        {
            foreach (LandMass pLandMass in Origin.Contents)
                foreach (Land pLand in pLandMass.Contents)
                    if (!pLand.Forbidden && !pLand.IsWater && !pLand.As<LandX>().HasOwner())
                    {
                        Region pRegion = new Region();
                        pRegion.Start(pLand.As<LandX>(), iMaxSize);
                        while (pRegion.Grow() != null) { }
                        m_cRegions.Add(pRegion);
                    }

            foreach (Region pRegion in m_cRegions)
                pRegion.Finish(fCycleShift);
        }
    }
}
