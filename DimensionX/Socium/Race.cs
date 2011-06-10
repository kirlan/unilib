using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LandscapeGeneration;
using Random;
using Socium.Languages;

namespace Socium
{
    public enum RaceGroup
    {
        Humans,
        Mithical,
        Fantasy,
        Supernatural,
        Aliens
    }

    public class RaceTemplate
    {
        public RaceGroup m_eGroup;

        public string m_sName;
        public string m_sNameF;
        public int m_iRank;
        public Language m_pLanguage;

        public LandTypeInfoX[] m_cPrefferedLands;
        public LandTypeInfoX[] m_cHatedLands;

        public RaceTemplate(RaceGroup eGroup, string sName, int iRank, Language pLanguage, LandTypeInfoX[] cPrefferedLands, LandTypeInfoX[] cHatedLands)
            :this(eGroup, sName, sName, iRank, pLanguage, cPrefferedLands, cHatedLands)
        { }

        public RaceTemplate(RaceGroup eGroup, string sNameM, string sNameF, int iRank, Language pLanguage, LandTypeInfoX[] cPrefferedLands, LandTypeInfoX[] cHatedLands)
        {
            m_eGroup = eGroup;

            m_sName = sNameM;
            m_sNameF = sNameF;
            m_iRank = iRank;

            m_pLanguage = pLanguage;

            m_cPrefferedLands = cPrefferedLands;
            m_cHatedLands = cHatedLands;
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
            new RaceTemplate(RaceGroup.Humans, "european ", 1, Language.European, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Tundra}),
            new RaceTemplate(RaceGroup.Humans, "slavic ", 1, Language.Slavic, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains, LandTypes<LandTypeInfoX>.Forest}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Tundra}),
            new RaceTemplate(RaceGroup.Humans, "indian ", 1, Language.Hindu, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Taiga, LandTypes<LandTypeInfoX>.Desert}),
            new RaceTemplate(RaceGroup.Humans, "asian ", 1, Language.Asian, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Tundra}),
            new RaceTemplate(RaceGroup.Humans, "aztec ", 1, Language.Aztec, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains, LandTypes<LandTypeInfoX>.Savanna}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Tundra}),
            new RaceTemplate(RaceGroup.Humans, "greek ", 1, Language.Greek, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains/*, LandTypes<LandTypeInfoX>.Savanna*/}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Taiga}),
            new RaceTemplate(RaceGroup.Humans, "arabian ", 1, Language.Arabian, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert/*, LandTypes<LandTypeInfoX>.Savanna*/}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Taiga}),
            new RaceTemplate(RaceGroup.Humans, "northern ", 1, Language.Northman, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Taiga}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Savanna}),
            new RaceTemplate(RaceGroup.Humans, "chukchee ", 1, Language.Eskimoid, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains, LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Savanna, LandTypes<LandTypeInfoX>.Mountains}),
            new RaceTemplate(RaceGroup.Humans, "eskimo ", 1, Language.Eskimoid, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains, LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Savanna, LandTypes<LandTypeInfoX>.Mountains}),
            new RaceTemplate(RaceGroup.Humans, "highlander ", 1, Language.Highlander, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Mountains}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains, LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Forest}),
            new RaceTemplate(RaceGroup.Humans, "black ", 1, Language.African, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Jungle/*, LandTypes<LandTypeInfoX>.Desert*/, LandTypes<LandTypeInfoX>.Savanna}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Plains, LandTypes<LandTypeInfoX>.Taiga}),
        //rank 10 - common non-humans
            new RaceTemplate(RaceGroup.Fantasy, "orc ", 10, Language.Orkish, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains, /*LandTypes<LandTypeInfoX>.Mountains, */LandTypes<LandTypeInfoX>.Savanna}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert}),
            new RaceTemplate(RaceGroup.Fantasy, "goblin ", 10, Language.Orkish, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains, /*LandTypes<LandTypeInfoX>.Mountains, */LandTypes<LandTypeInfoX>.Savanna}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert}),
            new RaceTemplate(RaceGroup.Mithical, "centaur ", 10, Language.Greek, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains, LandTypes<LandTypeInfoX>.Savanna}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Mountains, LandTypes<LandTypeInfoX>.Forest}),
            new RaceTemplate(RaceGroup.Fantasy, "ogre ", 10, Language.Orkish, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Savanna, LandTypes<LandTypeInfoX>.Mountains, LandTypes<LandTypeInfoX>.Forest}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Jungle}),
            new RaceTemplate(RaceGroup.Fantasy, "halfling ", 10, Language.European, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Mountains}),
            new RaceTemplate(RaceGroup.Mithical, "minotaur ", 10, Language.Greek, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Mountains}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra}),
            new RaceTemplate(RaceGroup.Fantasy, "elven ", 10, Language.Elven, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Plains}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Mountains, LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Tundra}),
            new RaceTemplate(RaceGroup.Fantasy, "dwarven ", 10, Language.Dwarwen, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Mountains}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Tundra}),
            new RaceTemplate(RaceGroup.Supernatural, "vampire ", 10, Language.European, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Tundra}),
        //rank 20 - not so common non-humans
            new RaceTemplate(RaceGroup.Fantasy, "cobold ", 20, Language.Dwarwen, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Mountains}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Tundra}),
            new RaceTemplate(RaceGroup.Fantasy, "gnoll ", 20, Language.Orkish, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Savanna, LandTypes<LandTypeInfoX>.Swamp}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Desert}),
            new RaceTemplate(RaceGroup.Mithical, "satyr ", "nimph ", 20, Language.Greek, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Jungle}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Desert}),
            new RaceTemplate(RaceGroup.Supernatural, "werewolf ", 20, Language.European, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Swamp}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert}),
            new RaceTemplate(RaceGroup.Supernatural, "jaguar people ", 20, Language.African, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Savanna, LandTypes<LandTypeInfoX>.Forest}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Taiga, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Tundra}),
            new RaceTemplate(RaceGroup.Supernatural, "yeti ", 20, Language.Eskimoid, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Mountains, LandTypes<LandTypeInfoX>.Tundra}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains, LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Jungle}),
            //new RaceTemplate("littlefolk ", 20, Language.Elven, 
            //    new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest}, 
            //    new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Plains}),
            new RaceTemplate(RaceGroup.Aliens, "lizard ", 20, Language.Aztec, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Swamp}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Plains, LandTypes<LandTypeInfoX>.Mountains}),
            new RaceTemplate(RaceGroup.Aliens, "reptile ", 20, Language.Aztec, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Jungle}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Taiga}),
            //new RaceTemplate("half-elf ", 20, Language.Elven, 
            //    new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains, LandTypes<LandTypeInfoX>.Forest}, 
            //    new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Mountains, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Taiga, LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Savanna}),
            //new RaceTemplate("half-orc ", 20, Language.Orkish, 
            //    new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains, LandTypes<LandTypeInfoX>.Mountains, LandTypes<LandTypeInfoX>.Forest}, 
            //    new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra}),
        //rank 30 - exotic non-humans
            new RaceTemplate(RaceGroup.Fantasy, "ratling ", 30, Language.Asian, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert}),
            new RaceTemplate(RaceGroup.Fantasy, "ursan ", 30, Language.Slavic, 
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
            new RaceTemplate(RaceGroup.Mithical, "naga ", 30, Language.Greek, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Jungle}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Mountains, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Savanna, LandTypes<LandTypeInfoX>.Taiga}),
            new RaceTemplate(RaceGroup.Mithical, "harpy ", 30, Language.Greek, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Mountains}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Taiga, LandTypes<LandTypeInfoX>.Swamp}),
            new RaceTemplate(RaceGroup.Mithical, "faery ", 30, Language.Elven, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Jungle}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Mountains}),
            new RaceTemplate(RaceGroup.Mithical, "pixie ", 30, Language.Elven, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Jungle}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Mountains}),
            new RaceTemplate(RaceGroup.Fantasy, "drow ", 30, Language.Drow, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Mountains}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra}),
        //rank 40 - powerful mythic creatures
            new RaceTemplate(RaceGroup.Mithical, "rakshasa ", "rakshasi ", 40, Language.Hindu, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Mountains}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Taiga}),
            new RaceTemplate(RaceGroup.Mithical, "asura ", 40, Language.Hindu, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains, LandTypes<LandTypeInfoX>.Savanna}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Taiga}),
            new RaceTemplate(RaceGroup.Fantasy, "drakonid ", 40, Language.Drow, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Mountains}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Taiga}),
        //rank 50 - complete aliens
            new RaceTemplate(RaceGroup.Aliens, "insectoid ", 50, Language.African, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Savanna, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Desert}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains}),
            new RaceTemplate(RaceGroup.Aliens, "arachnid ", 50, Language.Drow, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Jungle}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Tundra}),
            new RaceTemplate(RaceGroup.Aliens, "illithid ", 50, Language.Aztec, 
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

        public Race(RaceTemplate pTemplate)
        {
            m_pTemplate = pTemplate;

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
        public void Accommodate(World pWorld, bool bLastRun)
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

            if (bLastRun && !m_bDying)
            {
                if (m_iMagicLimit < pWorld.m_iMinMagicLevel)
                    m_iMagicLimit = pWorld.m_iMinMagicLevel;
                if (m_iMagicLimit > pWorld.m_iMaxMagicLevel)
                    m_iMagicLimit = pWorld.m_iMaxMagicLevel;

                if (m_iTechLevel < pWorld.m_iMinTechLevel)
                    m_iTechLevel = pWorld.m_iMinTechLevel;
                if (m_iTechLevel > pWorld.m_iMaxTechLevel)
                    m_iTechLevel = pWorld.m_iMaxTechLevel;
            }
            else
            {
                if (m_iMagicLimit < pWorld.m_iAncientsMinMagicLevel)
                    m_iMagicLimit = pWorld.m_iAncientsMinMagicLevel;
                if (m_iMagicLimit > pWorld.m_iAncientsMaxMagicLevel)
                    m_iMagicLimit = pWorld.m_iAncientsMaxMagicLevel;

                if (m_iTechLevel < pWorld.m_iAncientsMinTechLevel)
                    m_iTechLevel = pWorld.m_iAncientsMinTechLevel;
                if (m_iTechLevel > pWorld.m_iAncientsMaxTechLevel)
                    m_iTechLevel = pWorld.m_iAncientsMaxTechLevel;
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

            if (iOldLevel == 0 && Rnd.Chances(pWorld.m_iInvasionProbability, 100))
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
                m_iTechLevel = Math.Min(pWorld.m_iInvadersMaxTechLevel, pWorld.m_iTechLevel + (9 - pWorld.m_iTechLevel) / 2 + (int)(Math.Pow(Rnd.Get(10), 3) * (4 - pWorld.m_iTechLevel / 2) / 1000));
                m_iMagicLimit = Math.Min(pWorld.m_iInvadersMaxMagicLevel, pWorld.m_iMagicLimit + (9 - pWorld.m_iMagicLimit) / 2 + (int)(Math.Pow(Rnd.Get(10), 3) * (4 - pWorld.m_iMagicLimit / 2) / 1000));
                
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
