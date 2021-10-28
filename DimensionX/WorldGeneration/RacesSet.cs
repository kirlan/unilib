using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorldGeneration
{
    public class RacesSet
    {
        public static RacesSet[] s_aSets = 
        {
            new RacesSet("humans (west europe)", new string[] {"white", "barb"}),
            new RacesSet("humans (east europe)", new string[] {"white", "slavic", "hellene", "imperial"}),
            new RacesSet("humans (middle east)", new string[] {"hindu", "yellow", "dervish", "black"}),
            new RacesSet("humans (new world)", new string[] {"red", "nomad"}),
            new RacesSet("mithology (west europe)", new string[] {"faery", "pixie"}),
            new RacesSet("mithology (east europe)", new string[] {"centaur", "minotaur", "satyr", "harpy"}),
            new RacesSet("mithology (middle east)", new string[] {"naga", "rakshasa", "asura", "djinn"}),
            new RacesSet("fantasy (classic)", new string[] {"orc", "elf", "dwarf"}),
            new RacesSet("fantasy (extended)", new string[] {"goblin", "ogre", "halfling", "cobold", "gnoll", "drow", "drakonid"}),
            new RacesSet("fantasy (gothic)", new string[] {"vampire", "werewolf"}),
            new RacesSet("anthropomorphous animals", new string[] {"feline", "lizard", "reptile", "ratling", "ursan"}),
            new RacesSet("complete aliens", new string[] {"insectoid", "tranx", "arachnid", "illithid"}),
        };

        public string m_sName;

        public string[] m_aRaces;

        public RacesSet(string sName, string[] aRaces)
        {
            m_sName = sName;
            m_aRaces = aRaces;
        }

        public override string ToString()
        {
            return m_sName;
        }
    }
}
