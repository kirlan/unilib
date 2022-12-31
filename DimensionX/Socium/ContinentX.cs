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
    public class ContinentX: Continent<LandX, LandTypeInfoX>
    {
        public List<Region> m_cRegions = new List<Region>();

        public List<State> m_cStates = new List<State>();

        public string m_sName;

        public override void Start(LandMass<LandX> pCenter)
        {
            m_cRegions.Clear();

            base.Start(pCenter);

            m_sName = NameGenerator.GetAbstractName();
        }

        public override string ToString()
        {
            return m_sName;
        }

        public Dictionary<LandMass<LandX>, List<Nation>> m_cLocalNations = new Dictionary<LandMass<LandX>,List<Nation>>();

        public void BuildRegions(float fCycleShift, int iMaxSize)
        {
            foreach (LandMass<LandX> pLandMass in m_cContents)
                foreach (LandX pLand in pLandMass.m_cContents)
                    if (!pLand.Forbidden && pLand.Region == null)
                    {
                        Region pRegion = new Region();
                        pRegion.Start(pLand, iMaxSize);
                        while (pRegion.Grow()) { }
                        pRegion.Finish(fCycleShift);
                        m_cRegions.Add(pRegion);
                    }
        }
    }
}
