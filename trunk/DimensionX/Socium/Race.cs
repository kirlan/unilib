using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LandscapeGeneration;
using Random;
using Socium.Languages;

namespace Socium
{
    public class RaceTemplate
    {
        public string m_sName;
        public string m_sNameF;
        public int m_iRank;
        public Language m_pLanguage;

        public LandTypeInfoX[] m_aPrefferedLands;
        public LandTypeInfoX[] m_aHatedLands;

        public RaceTemplate(string sName, int iRank, Language pLanguage, LandTypeInfoX[] aPrefferedLands, LandTypeInfoX[] aHatedLands)
            :this(sName, sName, iRank, pLanguage, aPrefferedLands, aHatedLands)
        { }

        public RaceTemplate(string sNameM, string sNameF, int iRank, Language pLanguage, LandTypeInfoX[] aPrefferedLands, LandTypeInfoX[] aHatedLands)
        {
            m_sName = sNameM;
            m_sNameF = sNameF;
            m_iRank = iRank;

            m_pLanguage = pLanguage;

            m_aPrefferedLands = aPrefferedLands;
            m_aHatedLands = aHatedLands;
        }

        public override string ToString()
        {
            return m_sName.Trim();
        }
    }
    
    public class Race 
    {
        #region Races
        public static RaceTemplate[] m_cTemplates =
        {
        //rank 1 - usual people
            new RaceTemplate("european", 1, Language.European, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Tundra}),
            new RaceTemplate("slavic", 1, Language.Slavic, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains, LandTypes<LandTypeInfoX>.Forest}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Tundra}),
            new RaceTemplate("indian", 1, Language.Hindu, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Taiga, LandTypes<LandTypeInfoX>.Desert}),
            new RaceTemplate("asian", 1, Language.Asian, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Tundra}),
            new RaceTemplate("aztec", 1, Language.Aztec, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains, LandTypes<LandTypeInfoX>.Savanna}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Tundra}),
            new RaceTemplate("greek", 1, Language.Greek, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains/*, LandTypes<LandTypeInfoX>.Savanna*/}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Taiga}),
            new RaceTemplate("arabian", 1, Language.Arabian, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert/*, LandTypes<LandTypeInfoX>.Savanna*/}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Taiga}),
            new RaceTemplate("northern", 1, Language.Northman, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Taiga}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Savanna}),
            new RaceTemplate("chukchee", 1, Language.Eskimoid, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains, LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Savanna, LandTypes<LandTypeInfoX>.Mountains, LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Swamp}),
            new RaceTemplate("eskimo", 1, Language.Eskimoid, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains, LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Savanna, LandTypes<LandTypeInfoX>.Mountains, LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Swamp}),
            new RaceTemplate("highlander", 1, Language.Highlander, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Mountains}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains, LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Forest}),
            new RaceTemplate("black", 1, Language.African, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Jungle/*, LandTypes<LandTypeInfoX>.Desert*/, LandTypes<LandTypeInfoX>.Savanna}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Plains, LandTypes<LandTypeInfoX>.Taiga}),
        //rank 10 - common non-humans
            new RaceTemplate("orc", 10, Language.Orkish, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains, /*LandTypes<LandTypeInfoX>.Mountains, */LandTypes<LandTypeInfoX>.Savanna}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert}),
            new RaceTemplate("goblin", 10, Language.Orkish, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains, /*LandTypes<LandTypeInfoX>.Mountains, */LandTypes<LandTypeInfoX>.Savanna}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert}),
            new RaceTemplate("centaur", 10, Language.Greek, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains, LandTypes<LandTypeInfoX>.Savanna}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Mountains, LandTypes<LandTypeInfoX>.Forest}),
            new RaceTemplate("ogre", 10, Language.Orkish, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Savanna, LandTypes<LandTypeInfoX>.Mountains, LandTypes<LandTypeInfoX>.Forest}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Jungle}),
            new RaceTemplate("halfling", 10, Language.European, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Mountains}),
            new RaceTemplate("minotaur", 10, Language.Greek, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Mountains}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra}),
            new RaceTemplate("elven", 10, Language.Elven, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Plains}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Mountains, LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Tundra}),
            new RaceTemplate("dwarven", 10, Language.Dwarwen, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Mountains}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Tundra}),
            new RaceTemplate("vampire", 10, Language.European, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Tundra}),
        //rank 20 - not so common non-humans
            new RaceTemplate("cobold", 20, Language.Dwarwen, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Mountains}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Tundra}),
            new RaceTemplate("gnoll", 20, Language.Orkish, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Savanna, LandTypes<LandTypeInfoX>.Swamp}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Desert}),
            new RaceTemplate("satyr", "nimph ", 20, Language.Greek, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Jungle}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Desert}),
            new RaceTemplate("werewolf", 20, Language.European, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Swamp}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert}),
            new RaceTemplate("feline", 20, Language.African, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Savanna, LandTypes<LandTypeInfoX>.Forest}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Taiga, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Tundra}),
            new RaceTemplate("yeti", 20, Language.Eskimoid, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Mountains, LandTypes<LandTypeInfoX>.Tundra}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains, LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Jungle}),
            //new RaceTemplate("littlefolk ", 20, Language.Elven, 
            //    new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest}, 
            //    new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Plains}),
            new RaceTemplate("lizard", 20, Language.Aztec, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Swamp}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Plains, LandTypes<LandTypeInfoX>.Mountains}),
            new RaceTemplate("reptile", 20, Language.Aztec, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Jungle}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Taiga}),
            //new RaceTemplate("half-elf ", 20, Language.Elven, 
            //    new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains, LandTypes<LandTypeInfoX>.Forest}, 
            //    new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Mountains, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Taiga, LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Savanna}),
            //new RaceTemplate("half-orc ", 20, Language.Orkish, 
            //    new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains, LandTypes<LandTypeInfoX>.Mountains, LandTypes<LandTypeInfoX>.Forest}, 
            //    new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra}),
        //rank 30 - exotic non-humans
            new RaceTemplate("ratling", 30, Language.Asian, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert}),
            new RaceTemplate("ursan", 30, Language.Slavic, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert}),
            //new RaceTemplate("half-dragon ", 30, Language.Drow, 
            //    new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Mountains, LandTypes<LandTypeInfoX>.Desert}, 
            //    new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Taiga}),
            //new RaceTemplate("half-dwarf ", 30, NameGenerator.Language.Dwarf, 
            //    new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Mountains, LandTypes<LandTypeInfoX>.Plains}, 
            //    new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Swamp}),
            //new RaceTemplate("half-faery ", 30, NameGenerator.Language.Elf1, 
            //    new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Plains}, 
            //    new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Swamp}),
            //new RaceTemplate("golem ", 30, NameGenerator.Language.Aztec, 
            //    new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Savanna, }, 
            //    new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Mountains}),
            new RaceTemplate("naga", 30, Language.Hindu, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Jungle}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Mountains, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Savanna, LandTypes<LandTypeInfoX>.Taiga}),
            new RaceTemplate("harpy", 30, Language.Greek, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Mountains}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Taiga, LandTypes<LandTypeInfoX>.Swamp}),
            new RaceTemplate("faery", 30, Language.Elven, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Jungle}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Mountains}),
            new RaceTemplate("pixie", 30, Language.Elven, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Jungle}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Mountains}),
            new RaceTemplate("drow", 30, Language.Drow, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Mountains}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra}),
        //rank 40 - powerful mythic creatures
            new RaceTemplate("rakshasa", "rakshasi", 40, Language.Hindu, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Mountains}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Taiga}),
            new RaceTemplate("asura", 40, Language.Hindu, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains, LandTypes<LandTypeInfoX>.Savanna}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Taiga}),
            new RaceTemplate("drakonid", 40, Language.Drow, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Mountains}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Taiga}),
        //rank 50 - complete aliens
            new RaceTemplate("insectoid", 50, Language.African, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Savanna, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Desert}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains}),
            new RaceTemplate("arachnid", 50, Language.Drow, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Jungle}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Tundra}),
            new RaceTemplate("illithid", 50, Language.Aztec, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Mountains, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Desert}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Taiga, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Plains, LandTypes<LandTypeInfoX>.Savanna}),
        };
        #endregion

        public int m_iTechLevel = 0;
        public int m_iMagicLimit = 0;

        public MagicAbilityPrevalence m_eMagicAbilityPrevalence = MagicAbilityPrevalence.rare;
        public MagicAbilityDistribution m_eMagicAbilityDistribution = MagicAbilityDistribution.mostly_weak;

        public Culture m_pCulture = null;
        public Customs m_pCustoms = null;

        public RaceTemplate m_pTemplate = null;

        public bool m_bDying = false;
        public bool m_bHegemon = false;

        public Epoch m_pEpoch = null;

        public Race(RaceTemplate pTemplate, Epoch pEpoch)
        {
            m_pTemplate = pTemplate;

            m_pEpoch = pEpoch;

            m_pCulture = new Culture();
            m_pCustoms = new Customs();
        }

        public override string ToString()
        {
            if (m_bDying)
                return string.Format("ancient {0}", m_pTemplate);
            else
                if(m_bHegemon)
                    return string.Format("great {0}", m_pTemplate);
                else
                    return m_pTemplate.ToString();
        }

        /// <summary>
        /// Согласовать параметры расы с параметрами мира.
        /// </summary>
        /// <param name="pWorld">мир</param>
        public void Accommodate(World pWorld, Epoch pEpoch)
        {
            int iOldLevel = Math.Max(m_iTechLevel, m_iMagicLimit);

            if (!m_bDying)
            {
                if (m_iMagicLimit <= pWorld.m_iMagicLimit)
                    m_iMagicLimit = pWorld.m_iMagicLimit;
                else
                    m_iMagicLimit = (m_iMagicLimit + pWorld.m_iMagicLimit + 1) / 2;

                if (m_iTechLevel <= pWorld.m_iTechLevel && !m_bDying)
                    m_iTechLevel = pWorld.m_iTechLevel;
                else
                    m_iTechLevel = (m_iTechLevel + pWorld.m_iTechLevel + 1) / 2;
            }

            int iMagicLimit = (int)(Math.Pow(Rnd.Get(15), 3) / 1000);
            if (Rnd.OneChanceFrom(10))
                m_iMagicLimit += iMagicLimit;
            else
                m_iMagicLimit -= iMagicLimit;

            int iOldTechLevel = m_iTechLevel;

            int iTechLevel = (int)(Math.Pow(Rnd.Get(15), 3) / 1000);
            if (Rnd.OneChanceFrom(10))
                m_iTechLevel += iTechLevel;
            else
                m_iTechLevel -= iTechLevel;

            if (!m_bDying)
            {
                if (m_iMagicLimit < pEpoch.m_iMinMagicLevel)
                    m_iMagicLimit = pEpoch.m_iMinMagicLevel;
                if (m_iMagicLimit > pEpoch.m_iMaxMagicLevel)
                    m_iMagicLimit = pEpoch.m_iMaxMagicLevel;

                if (m_iTechLevel < pEpoch.m_iMinTechLevel)
                    m_iTechLevel = pEpoch.m_iMinTechLevel;
                if (m_iTechLevel > pEpoch.m_iMaxTechLevel)
                    m_iTechLevel = pEpoch.m_iMaxTechLevel;
            }
            else
            {
                if (m_iMagicLimit < 0)
                    m_iMagicLimit = 0;
                if (m_iMagicLimit > 8)
                    m_iMagicLimit = 8;

                if (m_iTechLevel < 0)
                    m_iTechLevel = 0;
                if (m_iTechLevel > 8)
                    m_iTechLevel = 8;
            }

            //m_pCustoms = new Customs();

            if (Rnd.OneChanceFrom(3))
                m_eMagicAbilityPrevalence = (MagicAbilityPrevalence)Rnd.GetExp(typeof(MagicAbilityPrevalence), 4);
            else
                if (iOldLevel == 0)
                    m_eMagicAbilityPrevalence = pWorld.m_eMagicAbilityPrevalence;

            if (Rnd.OneChanceFrom(3))
                m_eMagicAbilityDistribution = (MagicAbilityDistribution)Rnd.GetExp(typeof(MagicAbilityDistribution), 4);
            else
                if (iOldLevel == 0)
                    m_eMagicAbilityDistribution = pWorld.m_eMagicAbilityDistribution;

            if (iOldLevel == 0 && Rnd.Chances(pEpoch.m_iInvadersCount, 100))
            {
                //int iBalance = Rnd.Get(201);

                //if (iBalance > 100)
                //{
                //    m_iMagicLimit = pWorld.m_iMagicLimit + (9 - pWorld.m_iMagicLimit) / 2 + (int)(Math.Pow(Rnd.Get(10), 3) * (4 - pWorld.m_iMagicLimit / 2) / 1000);
                //    m_iTechLevel = (200 - iBalance) * m_iMagicLimit / iBalance;
                //}
                //else
                //{
                //    m_iTechLevel = pWorld.m_iTechLevel + (9 - pWorld.m_iTechLevel) / 2 + (int)(Math.Pow(Rnd.Get(10), 3) * (4 - pWorld.m_iTechLevel / 2) / 1000);
                //    m_iMagicLimit = iBalance * m_iTechLevel / (200 - iBalance);
                //} 
                m_iTechLevel = Math.Min(pEpoch.m_iInvadersMaxTechLevel, pEpoch.m_iInvadersMinTechLevel + 1 + (int)(Math.Pow(Rnd.Get(20), 3) / 1000));
                m_iMagicLimit = Math.Min(pEpoch.m_iInvadersMaxMagicLevel, pEpoch.m_iInvadersMinMagicLevel + (int)(Math.Pow(Rnd.Get(21), 3) / 1000));
                
                m_eMagicAbilityPrevalence = (MagicAbilityPrevalence)Rnd.Get(typeof(MagicAbilityPrevalence));
                m_eMagicAbilityDistribution = (MagicAbilityDistribution)Rnd.Get(typeof(MagicAbilityDistribution));
            }

            int iNewLevel = Math.Max(m_iTechLevel, m_iMagicLimit);
            if (iNewLevel > iOldLevel)
                for (int i = 0; i < iNewLevel - iOldLevel; i++)
                {
                    m_pCulture.Evolve();
                    m_pCustoms.Evolve();
                }
            else
                for (int i = 0; i < iOldLevel - iNewLevel; i++)
                {
                    m_pCulture.Degrade();
                    m_pCustoms.Degrade();
                }
        }
    }
}
