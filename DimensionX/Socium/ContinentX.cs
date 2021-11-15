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
    public class ContinentX: Continent<LandX, LandTypeInfoX>
    {
        public List<AreaX> m_cAreas = new List<AreaX>();

        public List<State> m_cStates = new List<State>();

        public string m_sName;

        public override void Start(LandMass<LandX> pCenter)
        {
            m_cAreas.Clear();

            base.Start(pCenter);

            m_sName = NameGenerator.GetAbstractName();
        }

        public override string ToString()
        {
            return m_sName;
        }

        public Dictionary<LandMass<LandX>, List<Nation>> m_cLocalNations = new Dictionary<LandMass<LandX>,List<Nation>>();

        public void BuildAreas(float fCycleShift, int iMaxSize)
        {
            foreach (LandMass<LandX> pLandMass in m_cContents)
                foreach (LandX pLand in pLandMass.m_cContents)
                    if (!pLand.Forbidden && pLand.Area == null)
                    {
                        AreaX pArea = new AreaX();
                        pArea.Start(pLand, iMaxSize);
                        while (pArea.Grow()) { }
                        pArea.Finish(fCycleShift);
                        m_cAreas.Add(pArea);
                    }
        }
    }
}
