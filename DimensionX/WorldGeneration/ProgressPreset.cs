using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorldGeneration
{
    class ProgressPreset
    {
        public static ProgressPreset[] s_aSocietyPresets = new[]        
        {
            new ProgressPreset("Historical - antiquity", "An antique world without magic - like Ancient Greece, Rome, Egypt, Assyria, etc.", 0, 1, 0, 0),
            new ProgressPreset("Historical - medieval", "A medieval world without magic - castles, knights, tournaments, etc.", 0, 2, 0, 0),
            new ProgressPreset("Historical - renaissance", "A renaissance world without magic - musketeers, geographic exploration, etc.", 1, 3, 0, 0),
            new ProgressPreset("Historical - modern", "A modern world without magic - railroads, aviation, world wars, etc.", 4, 5, 0, 0),
            new ProgressPreset("Antique mythology", "A world of antique mythology - just a usual antique world, but with a bit of magic...", 0, 1, 1, 2),
            new ProgressPreset("Fantasy - low magic", "A medieval world with a bit of magic - like Knights of the Round Table, Lord of the Rings, etc.", 1, 2, 1, 3),
            new ProgressPreset("Fantasy - high magic", "A medieval world with a lot of magic - like Dragonlance, Wheel of Time, etc.", 1, 2, 2, 4),
            new ProgressPreset("Technomagic", "A renaissance world with a lot of magic - like Arcanum, Final Fantasy, etc.", 1, 3, 2, 4),
            new ProgressPreset("Superheroes", "A modern world with a bit of magic (aka supernatural abilities) - like Superman, Fantastic Four, Spiderman, etc.", 4, 5, 1, 3),
            //new SocietyPreset("Urban fantasy", "A modern world with a lot of magic - vampires, werewolfs, voodoo, secret societies, etc.", 4, 5, 2, 4),
            new ProgressPreset("Cyberpunk", "Nearest future world without magic - advanced technologies, mega-corporations, industrial espionage, etc.", 4, 6, 0, 0),
            new ProgressPreset("Space opera", "Far future world with a bit of magic (aka psi-abilities) - like Star Wars, Star Trek, etc.", 6, 7, 1, 3),
        };

        public string m_sName;

        public override string ToString()
        {
            return m_sName;
        }
    
        public string m_sDescription;

        public int m_iMinTechLevel;
        public int m_iMaxTechLevel;
        public int m_iMinMagicLevel;
        public int m_iMaxMagicLevel;

        public ProgressPreset(string sName, string sDescription, int iMinTechLevel, int iMaxTechLevel, int iMinMagicLevel, int iMaxMagicLevel)
        {
            m_sName = sName;
            m_sDescription = sDescription;

            m_iMinTechLevel = iMinTechLevel;
            m_iMaxTechLevel = iMaxTechLevel;

            m_iMinMagicLevel = iMinMagicLevel;
            m_iMaxMagicLevel = iMaxMagicLevel;
        }
    }
}
