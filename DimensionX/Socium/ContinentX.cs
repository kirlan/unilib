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
        public List<Region> Regions { get; } = new List<Region>();

        public List<State> Contents { get; } = new List<State>();

        public string Name { get; set; }

        public ContinentX(Continent pOrigin) : base(pOrigin)
        {
            Regions.Clear();

            Name = NameGenerator.GetAbstractName();
        }

        public ContinentX()
        { }

        public override string ToString()
        {
            return Name;
        }

        public Dictionary<LandMass, List<Nation>> LocalNations { get; } = new Dictionary<LandMass, List<Nation>>();

        public void BuildRegions(float fCycleShift, int iMaxSize)
        {
            foreach (LandMass pLandMass in Origin.Contents)
            {
                foreach (Land pLand in pLandMass.Contents)
                {
                    if (!pLand.Forbidden && !pLand.IsWater && !pLand.As<LandX>().HasOwner())
                    {
                        Region pRegion = new Region();
                        pRegion.Start(pLand.As<LandX>(), iMaxSize);
                        while (pRegion.Grow() != null) { }
                        Regions.Add(pRegion);
                    }
                }
            }

            foreach (Region pRegion in Regions)
                pRegion.Finish(fCycleShift);
        }
    }
}
