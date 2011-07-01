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
    public class ContinentX: Continent<AreaX, LandX, LandTypeInfoX>
    {
        public List<State> m_cStates = new List<State>();

        public string m_sName;

        public override void Start(LandMass<LandX> pCenter)
        {
            base.Start(pCenter);

            m_sName = NameGenerator.GetAbstractName();
        }

        public override string ToString()
        {
            return m_sName;
        }

        public Dictionary<LandMass<LandX>, List<Nation>> m_cLocalNations = new Dictionary<LandMass<LandX>,List<Nation>>();
    }
}
