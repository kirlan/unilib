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
            new RacesSet("humans (west europe)", new string[] {"european", "northern", "highlander"}),
            new RacesSet("humans (east europe)", new string[] {"european", "slavic", "greek"}),
            new RacesSet("humans (middle east)", new string[] {"indian", "asian", "arabian", "black"}),
            new RacesSet("humans (new world)", new string[] {"aztec", "chukchee", "eskimo"}),
            new RacesSet("mithology (european)", new string[] {"faery", "pixie"}),
            new RacesSet("mithology (greek)", new string[] {"centaur", "minotaur", "satyr", "harpy"}),
            new RacesSet("mithology (hindu)", new string[] {"naga", "rakshasa", "asura"}),
            new RacesSet("mithology (modern)", new string[] {"vampire", "werewolf", "yeti"}),
            new RacesSet("fantasy (classic)", new string[] {"orc", "elven", "dwarven"}),
            new RacesSet("fantasy (extended)", new string[] {"goblin", "ogre", "halfling", "cobold", "gnoll", "drow", "drakonid"}),
            new RacesSet("anthropomorphous animals", new string[] {"feline", "lizard", "reptile", "ratling", "ursan"}),
            new RacesSet("complete aliens", new string[] {"insectoid", "arachnid", "illithid"}),
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
