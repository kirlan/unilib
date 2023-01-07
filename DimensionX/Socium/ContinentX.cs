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
    public class ContinentX: IInfoLayer
    {
        public List<Region> m_cRegions = new List<Region>();

        public List<State> m_cStates = new List<State>();

        public string m_sName;

        public ContinentX(Continent pOrigin)
        {
            m_cRegions.Clear();

            Layers.Add(pOrigin);

            m_sName = NameGenerator.GetAbstractName();
        }

        public override string ToString()
        {
            return m_sName;
        }

        public Dictionary<LandMass, List<Nation>> m_cLocalNations = new Dictionary<LandMass,List<Nation>>();

        public void BuildRegions(float fCycleShift, int iMaxSize)
        {
            foreach (LandMass<LandX> pLandMass in m_cContents)
                foreach (LandX pLand in pLandMass.m_cContents)
                    if (!pLand.Forbidden && pLand.Region == null)
                    {
                        Region pRegion = new Region();
                        pRegion.Start(pLand, iMaxSize);
                        while (pRegion.Grow() != null) { }
                        m_cRegions.Add(pRegion);
                        pRegion.Owner = pLandMass;
                    }

            foreach (Region pRegion in m_cRegions)
                pRegion.Finish(fCycleShift);
        }
    }
}
