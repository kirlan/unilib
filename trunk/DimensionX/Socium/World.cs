using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;
using BenTools.Mathematics;
using System.Drawing;
using LandscapeGeneration;
using LandscapeGeneration.PathFind;
using Socium.Languages;
using System.Diagnostics;

namespace Socium
{
    public enum MagicAbilityPrevalence
    {
        rare,
        common,
        almost_everyone
    }

    public enum MagicAbilityDistribution
    {
        mostly_weak,
        mostly_average,
        mostly_powerful
    }

    public class World : Landscape<LocationX, LandX, AreaX, ContinentX, LandTypeInfoX>
    {
        #region Races
        public static Race[] m_cAllRaces =
        {
        //rank 1 - usual people
            new Race("european ", 1, Language.European, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Tundra}),
            new Race("slavic ", 1, Language.Slavic, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains, LandTypes<LandTypeInfoX>.Forest}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Tundra}),
            new Race("indian ", 1, Language.Hindu, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Taiga}),
            new Race("asian ", 1, Language.Asian, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Tundra}),
            new Race("indean ", 1, Language.Aztec, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains, LandTypes<LandTypeInfoX>.Savanna}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Tundra}),
            new Race("greek ", 1, Language.Greek, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains/*, LandTypes<LandTypeInfoX>.Savanna*/}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Taiga}),
            new Race("arabian ", 1, Language.Arabian, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert/*, LandTypes<LandTypeInfoX>.Savanna*/}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Taiga}),
            new Race("northern ", 1, Language.Northman, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Taiga}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains, LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Mountains, LandTypes<LandTypeInfoX>.Savanna}),
            //new Race("highlander ", 1, Language.Highlander, 
            //    new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Mountains}, 
            //    new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains, LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Forest}),
            new Race("black ", 1, Language.African, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Jungle/*, LandTypes<LandTypeInfoX>.Desert*/, LandTypes<LandTypeInfoX>.Savanna}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Plains, LandTypes<LandTypeInfoX>.Taiga}),
        //rank 10 - common non-humans
            new Race("orc ", 10, Language.Orkish, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains, /*LandTypes<LandTypeInfoX>.Mountains, */LandTypes<LandTypeInfoX>.Savanna}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert}),
            new Race("goblin ", 10, Language.Orkish, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains, /*LandTypes<LandTypeInfoX>.Mountains, */LandTypes<LandTypeInfoX>.Savanna}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert}),
            new Race("centaur ", 10, Language.Greek, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains, LandTypes<LandTypeInfoX>.Savanna}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Mountains, LandTypes<LandTypeInfoX>.Forest}),
            new Race("ogre ", 10, Language.Orkish, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Savanna, LandTypes<LandTypeInfoX>.Mountains, LandTypes<LandTypeInfoX>.Forest}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Jungle}),
            new Race("halfling ", 10, Language.European, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Mountains}),
            new Race("minotaur ", 10, Language.Greek, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Mountains}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra}),
            new Race("elven ", 10, Language.Elven, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Plains}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Mountains, LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Tundra}),
            new Race("dwarven ", 10, Language.Dwarwen, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Mountains}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Tundra}),
            new Race("vampire ", 10, Language.European, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Tundra}),
        //rank 20 - not so common non-humans
            new Race("cobold ", 20, Language.Dwarwen, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Mountains}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Tundra}),
            new Race("gnoll ", 20, Language.Orkish, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Savanna, LandTypes<LandTypeInfoX>.Swamp}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Desert}),
            new Race("satyr ", "nimph ", 20, Language.Greek, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Jungle}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Desert}),
            new Race("werewolf ", 20, Language.European, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Swamp}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert}),
            new Race("jaguar people ", 20, Language.African, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Savanna, LandTypes<LandTypeInfoX>.Forest}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Taiga, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Tundra}),
            new Race("yeti ", 20, Language.Eskimoid, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Mountains, LandTypes<LandTypeInfoX>.Tundra}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains, LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Jungle}),
            //new Race("littlefolk ", 20, Language.Elven, 
            //    new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest}, 
            //    new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Plains}),
            new Race("lizard ", 20, Language.Aztec, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Swamp}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Plains, LandTypes<LandTypeInfoX>.Mountains}),
            new Race("reptile ", 20, Language.Aztec, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Jungle}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Taiga}),
            new Race("half-elf ", 20, Language.Elven, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains, LandTypes<LandTypeInfoX>.Forest}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Mountains, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Taiga, LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Savanna}),
            new Race("half-orc ", 20, Language.Orkish, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains, LandTypes<LandTypeInfoX>.Mountains, LandTypes<LandTypeInfoX>.Forest}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra}),
        //rank 30 - exotic non-humans
            new Race("wererat ", 30, Language.Asian, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert}),
            new Race("werebear ", 30, Language.Slavic, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert}),
            //new Race("half-dragon ", 30, Language.Drow, 
            //    new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Mountains, LandTypes<LandTypeInfoX>.Desert}, 
            //    new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Taiga}),
            //new Race("half-dwarf ", 30, NameGenerator.Language.Dwarf, 
            //    new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Mountains, LandTypes<LandTypeInfoX>.Plains}, 
            //    new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Swamp}),
            //new Race("half-faery ", 30, NameGenerator.Language.Elf1, 
            //    new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Plains}, 
            //    new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Swamp}),
            //new Race("golem ", 30, NameGenerator.Language.Aztec, 
            //    new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Savanna, }, 
            //    new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Mountains}),
            new Race("naga ", 30, Language.Greek, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Jungle}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Mountains, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Savanna, LandTypes<LandTypeInfoX>.Taiga}),
            new Race("harpy ", 30, Language.Greek, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Mountains}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Taiga, LandTypes<LandTypeInfoX>.Swamp}),
            new Race("faery ", 30, Language.Elven, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Jungle}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Mountains}),
            new Race("pixie ", 30, Language.Elven, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Jungle}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Mountains}),
            new Race("drow ", 30, Language.Drow, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Mountains}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra}),
        //rank 40 - powerful mythic creatures
            new Race("rakshasa ", "rakshasi ", 40, Language.Hindu, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Mountains}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Taiga}),
            new Race("asura ", 40, Language.Hindu, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains, LandTypes<LandTypeInfoX>.Savanna}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Taiga}),
            new Race("weredragon ", 40, Language.Drow, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Mountains}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Taiga}),
        //rank 50 - complete aliens
            new Race("insectoid ", 50, Language.African, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Savanna, LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Desert}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Plains}),
            new Race("arachnid ", 50, Language.Drow, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Swamp, LandTypes<LandTypeInfoX>.Jungle}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Desert, LandTypes<LandTypeInfoX>.Tundra}),
            new Race("illithid ", 50, Language.Aztec, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Mountains, LandTypes<LandTypeInfoX>.Jungle, LandTypes<LandTypeInfoX>.Desert}, 
                new LandTypeInfoX[] {LandTypes<LandTypeInfoX>.Taiga, LandTypes<LandTypeInfoX>.Tundra, LandTypes<LandTypeInfoX>.Forest, LandTypes<LandTypeInfoX>.Plains, LandTypes<LandTypeInfoX>.Savanna}),
        };
#endregion

        public int m_iTechLevel;
        public int m_iMagicLimit;

        public MagicAbilityPrevalence m_eMagicAbilityPrevalence = MagicAbilityPrevalence.rare;
        public MagicAbilityDistribution m_eMagicAbilityDistribution = MagicAbilityDistribution.mostly_weak;

        public Province[] m_aProvinces = null;

        private int m_iProvincesCount = 300;

        public State[] m_aStates = null;

        private int m_iStatesCount = 30;

        public Race[] m_aLocalRaces = null;

        private void SetWorldLevels()
        {
            m_eMagicAbilityPrevalence = (MagicAbilityPrevalence)Rnd.GetExp(typeof(MagicAbilityPrevalence), 4);
            m_eMagicAbilityDistribution = (MagicAbilityDistribution)Rnd.GetExp(typeof(MagicAbilityDistribution), 4);

            //int iTotalLevel = 1 + (int)(Math.Pow(Rnd.Get(20), 3) / 1000);
            //int iBalance = Rnd.Get(201);

            //if (iBalance > 100)
            //{
            //    m_iMagicLimit = Rnd.Get(iTotalLevel + 1);//iTotalLevel * iBalance / 100;
            //    //m_iTechLevel = 1 + Rnd.Get(iTotalLevel);//iTotalLevel - m_iBioLevel;
            //    m_iTechLevel = (200 - iBalance) * m_iMagicLimit / iBalance;
            //}
            //else
            //{
            //    m_iTechLevel = 1 + Rnd.Get(iTotalLevel);//iTotalLevel - m_iBioLevel;
            //    //m_iMagicLimit = Rnd.Get(iTotalLevel + 1);//iTotalLevel * iBalance / 100;
            //    m_iMagicLimit = iBalance * m_iTechLevel / (200 - iBalance);
            //}

            m_iTechLevel = Math.Min(m_iMaxTechLevel, m_iMinTechLevel + 1 + (int)(Math.Pow(Rnd.Get(20), 3) / 1000));
            m_iMagicLimit = Math.Min(m_iMaxMagicLevel, m_iMinMagicLevel + (int)(Math.Pow(Rnd.Get(21), 3) / 1000));
        }

        private void AddRaces()
        {
            AddRaces(6 + Rnd.Get(4));
        }

        private void AddRaces(int iDiversity)
        {
            List<Race> cRaces = new List<Race>();

            //Все расы, оставшиеся с прошлых исторических циклов, адаптируем к новым условиям.
            if(m_aLocalRaces != null)
                foreach (Race pRace in m_aLocalRaces)
                {
                    pRace.Accommodate(this);
                    cRaces.Add(pRace);
                }

            //Рассчитываем шансы новых рас попасть в новый мир - учитывая, его параметры
            Dictionary<Race, float> cRaceChances = new Dictionary<Race, float>();
            foreach (Race pRace in World.m_cAllRaces)
                cRaceChances[pRace] = (m_aLocalRaces != null && m_aLocalRaces.Contains(pRace)) ? 0 : 100.0f / pRace.m_iRank;
            //cRaceChances[pRace] = (m_aLocalRaces != null && m_aLocalRaces.Contains(pRace)) ? 0 : Math.Max(1, 10 + m_iMagicLimit * 10 - pRace.m_iRank);// : 100.0f / pRace.m_iRank;

            //Добавляем необходимое количесто новых рас.
            while (cRaces.Count < iDiversity)
            {
                int iChance = Rnd.ChooseOne(cRaceChances.Values, 1);
                foreach (Race pRace in cRaceChances.Keys)
                {
                    iChance--;
                    if (iChance < 0)
                    {
                        pRace.Accommodate(this);

                        cRaces.Add(pRace);
                        cRaceChances[pRace] = 0;
                        break;
                    }
                }
            }
            m_aLocalRaces = cRaces.ToArray();
        }

        private void DistributeRacesToLandMasses()
        {
            //Распределим расы по наиболее подходящим им тектоническим плитам (чтобы все расы хоть где-то да жили)
            foreach (Race pRace in m_aLocalRaces)
            {
                bool bHomeless = true;
                foreach (ContinentX pConti in m_aContinents)
                {
                    foreach (LandMass<LandX> pLandMass in pConti.m_cContents)
                        if (pConti.m_cLocalRaces.ContainsKey(pLandMass) && pConti.m_cLocalRaces[pLandMass].Contains(pRace))
                        {
                            bHomeless = false;
                            break;
                        }
                    if (!bHomeless)
                        break;
                }

                if (!bHomeless)
                    continue;

                Dictionary<LandX, float> cLandChances = new Dictionary<LandX, float>();
                foreach (LandX pLand in m_aLands)
                {
                    if (pLand.Forbidden ||
                        pLand.m_pProvince != null ||
                        pLand.m_pRace != null ||
                        pLand.IsWater ||
                        pLand.Owner == null ||
                        (pLand.Owner as LandMass<LandX>).IsWater)
                        continue;

                    cLandChances[pLand] = 1.0f;// / pRace.m_iRank;

                    foreach (LandTypeInfoX pType in pRace.m_cPrefferedLands)
                        if (pLand.Type == pType)
                            cLandChances[pLand] *= 100;

                    foreach (LandTypeInfoX pType in pRace.m_cHatedLands)
                        if (pLand.Type == pType)
                            cLandChances[pLand] /= 1000;
                }

                int iChance = Rnd.ChooseOne(cLandChances.Values, 2);
                LandX pCradle = null;
                foreach (LandX pLand in cLandChances.Keys)
                {
                    iChance--;
                    if (iChance < 0)
                    {
                        pCradle = pLand;
                        break;
                    }
                }

                if (pCradle != null && pCradle.Area != null)
                {
                    (pCradle.Area as AreaX).m_pRace = pRace;
                    (pCradle.Area as AreaX).m_sName = pRace.m_pLanguage.RandomCountryName();
                    foreach (LandX pLand in (pCradle.Area as AreaX).m_cContents)
                    {
                        pLand.m_sName = (pCradle.Area as AreaX).m_sName;
                        pLand.m_pRace = (pCradle.Area as AreaX).m_pRace;
                    }

                    LandMass<LandX> pLandMass = pCradle.Owner as LandMass<LandX>;
                    if (pLandMass != null)
                    {
                        if (!pCradle.Continent.m_cLocalRaces.ContainsKey(pLandMass))
                            pCradle.Continent.m_cLocalRaces[pLandMass] = new List<Race>();
                        
                        if(pCradle.Continent.m_cLocalRaces[pLandMass].Contains(pRace))
                            pCradle.Continent.m_cLocalRaces[pLandMass].Add(pRace);
                        else
                            pCradle.Continent.m_cLocalRaces[pLandMass].Add(pRace);
                    }
                }
            }

            //Позаботимся о том, чтобы на каждой тектонической плите жила бы хоть одна раса
            foreach (ContinentX pConti in m_aContinents)
            {
                //Не нужно населять КАЖДУЮ плиту, достаточно чтобы хотя бы одна плита на континенте была населена.
                int iPop = 0;
                foreach (List<Race> pList in pConti.m_cLocalRaces.Values)
                {
                    iPop += pList.Count;
                }
                if (iPop > 1)
                    continue;

                foreach (LandMass<LandX> pLandMass in pConti.m_cContents)
                {
                    if (pConti.m_cLocalRaces.ContainsKey(pLandMass) && pConti.m_cLocalRaces[pLandMass].Count > 0)
                        continue;

                    pConti.m_cLocalRaces[pLandMass] = new List<Race>();

                    int iLocalsCount = 1 + Rnd.Get(2);

                    while (pConti.m_cLocalRaces[pLandMass].Count < iLocalsCount)
                    {
                        Dictionary<LandTypeInfoX, int> cLandTypesCount = new Dictionary<LandTypeInfoX, int>();
                        foreach (LandX pLand in pLandMass.m_cContents)
                        {
                            if (pLand.IsWater)
                                continue;

                            if (!cLandTypesCount.ContainsKey(pLand.Type))
                                cLandTypesCount[pLand.Type] = 0;

                            cLandTypesCount[pLand.Type] += pLand.m_cContents.Count;
                        }

                        Dictionary<Race, float> cRaceChances = new Dictionary<Race, float>();
                        foreach (Race pRace in m_aLocalRaces)
                        {
                            if (pConti.m_cLocalRaces[pLandMass].Contains(pRace))
                                continue;

                            cRaceChances[pRace] = 1.0f / pRace.m_iRank;

                            foreach (LandTypeInfoX pType in pRace.m_cPrefferedLands)
                                if (cLandTypesCount.ContainsKey(pType))
                                    cRaceChances[pRace] *= cLandTypesCount[pType];

                            foreach (LandTypeInfoX pType in pRace.m_cHatedLands)
                                if (cLandTypesCount.ContainsKey(pType))
                                    cRaceChances[pRace] /= cLandTypesCount[pType];
                        }

                        int iChance = Rnd.ChooseOne(cRaceChances.Values, 2);
                        Race pChoosenRace = null;
                        foreach (Race pRace in cRaceChances.Keys)
                        {
                            iChance--;
                            if (iChance < 0)
                            {
                                pChoosenRace = pRace;
                                break;
                            }
                        }

                        if (pChoosenRace != null)
                        {
                            if (pConti.m_cLocalRaces[pLandMass].Contains(pChoosenRace))
                                pConti.m_cLocalRaces[pLandMass].Add(pChoosenRace);
                            else
                                pConti.m_cLocalRaces[pLandMass].Add(pChoosenRace);
                            if (pConti.m_cContents.Count == 1)
                                pConti.m_sName = pChoosenRace.m_pLanguage.RandomCountryName();
                        }
                    }
                }
            }
        }

        private void PopulateAreas()
        {
            foreach (ContinentX pConti in m_aContinents)
            {
                List<Race> cAvailableContinentRaces = new List<Race>();
                foreach (LandMass<LandX> pLandMass in pConti.m_cLocalRaces.Keys)
                {
                    foreach (Race pRace in pConti.m_cLocalRaces[pLandMass])
                        if (!cAvailableContinentRaces.Contains(pRace))
                            cAvailableContinentRaces.Add(pRace);
                }

                foreach (AreaX pArea in pConti.m_cAreas)
                    if (!pArea.IsWater && pArea.m_pRace == null)
                    {
                        List<LandMass<LandX>> cLandMasses = new List<LandMass<LandX>>();
                        foreach (LandX pLand in pArea.m_cContents)
                        {
                            LandMass<LandX> pLandMass = pLand.Owner as LandMass<LandX>;
                            if (pLandMass != null)
                            {
                                if (!cLandMasses.Contains(pLandMass))
                                    cLandMasses.Add(pLandMass);
                            }
                        }

                        List<Race> cAvailableRaces = new List<Race>();
                        foreach (LandMass<LandX> pLandMass in cLandMasses)
                        {
                            if (!pConti.m_cLocalRaces.ContainsKey(pLandMass))
                                continue;

                            foreach (Race pRace in pConti.m_cLocalRaces[pLandMass])
                                if (!cAvailableRaces.Contains(pRace))
                                {
                                    cAvailableRaces.Add(pRace);
                                    if (!m_aLocalRaces.Contains(pRace))
                                        throw new Exception();
                                }
                        }

                        if (cAvailableRaces.Count > 0)
                            pArea.SetRace(cAvailableRaces);
                        else
                            pArea.SetRace(cAvailableContinentRaces);
                    }
            }
        }

        /// <summary>
        /// Генерация мира
        /// </summary>
        /// <param name="cLocations">Сетка локаций, на которой будет строиться мир.</param>
        /// <param name="iContinents">Общее количество континентов - обособленных, разделённых водой участков суши.</param>
        /// <param name="bGreatOcean">Если true, то карта со всех сторон окружена водой.</param>
        /// <param name="iLands">Общее количество `земель` - групп соседних локаций с одинаковым типом территории</param>
        /// <param name="iProvinces">Общее количество провинций. Каждая провинция объединяет несколько сопредельных земель.</param>
        /// <param name="iStates">Общее количество государств. Каждое государство объединяет несколько сопредельных провинций.</param>
        /// <param name="iLandMasses">Общее количество тектонических плит, являющихся строительными блоками при составлении континентов.</param>
        /// <param name="iOcean">Процент тектонических плит, лежащих на дне океана - от 10 до 90.</param>
        /// <param name="iEquator">Положение экватора на карте в процентах по вертикали. 50 - середина карты, 0 - верхний край, 100 - нижний край</param>
        /// <param name="iPole">Расстояние от экватора до полюсов в процентах по вертикали. Если экватор расположен посередине карты, то значение 50 даст для полюсов верхний и нижний края карты соответственно.</param>
        /// <param name="iRacesCount">Количество различных рас, населяющих мир.</param>
        /// <param name="iMinTechLevel">Минимальный возможный в этом мире уровень технического развития.</param>
        /// <param name="iMaxTechLevel">Максимальный возможный в этом мире уровень технического развития.</param>
        /// <param name="iMinMagicLevel">Минимальный возможный в этом мире уровень владения магией.</param>
        /// <param name="iMaxMagicLevel">Максимальный возможный в этом мире уровень владения магией.</param>
        /// <param name="iInvasionProbability">Вероятность вторжения из более развитого мира (0-100%).</param>
        public World(LocationsGrid<LocationX> cLocations, 
                     int iContinents, 
                     bool bGreatOcean, 
                     int iLands, 
                     int iProvinces, 
                     int iStates, 
                     int iLandMasses, 
                     int iOcean, 
                     int iEquator, 
                     int iPole, 
                     int iRacesCount,
                     int iMinTechLevel,
                     int iMaxTechLevel,
                     int iMinMagicLevel,
                     int iMaxMagicLevel,
                     int iInvasionProbability,
                     int iInvadersMaxTechLevel,
                     int iInvadersMaxMagicLevel)
            : base(cLocations, iContinents, bGreatOcean, iLands, iLandMasses, iOcean, iEquator, iPole)
        {
            Create(iProvinces, iStates, iRacesCount, iMinTechLevel, iMaxTechLevel, iMinMagicLevel, iMaxMagicLevel, iInvasionProbability, iInvadersMaxTechLevel, iInvadersMaxMagicLevel);
        }

        public int m_iMinTechLevel;
        public int m_iMaxTechLevel;
        public int m_iMinMagicLevel;
        public int m_iMaxMagicLevel;
        public int m_iInvasionProbability;
        public int m_iInvadersMaxTechLevel;
        public int m_iInvadersMaxMagicLevel;

        private void Create(int iProvinces, int iStates, int iRacesCount, int iMinTechLevel, int iMaxTechLevel, int iMinMagicLevel, int iMaxMagicLevel, int iInvasionProbability, int iInvadersMaxTechLevel, int iInvadersMaxMagicLevel)
        {
            Language.ResetUsedLists();

            m_iProvincesCount = iProvinces;
            m_iStatesCount = iStates;

            m_iMinTechLevel = iMinTechLevel;
            m_iMaxTechLevel = iMaxTechLevel;

            m_iMinMagicLevel = iMinMagicLevel;
            m_iMaxMagicLevel = iMaxMagicLevel;

            m_iInvasionProbability = iInvasionProbability;

            m_iInvadersMaxTechLevel = iInvadersMaxTechLevel;
            m_iInvadersMaxMagicLevel = iInvadersMaxMagicLevel;

            PopulateWorld(iRacesCount, true);

            int iAge = Rnd.Get(4);//4 - снижено для отладки
            for (int i = 0; i < iAge; i++)
            {
                Reset();
                PopulateWorld(iRacesCount, true);
            }

            Reset();
            PopulateWorld(iRacesCount, false);
        }

        private void PopulateWorld(int iRacesCount, bool bFast)
        {
            SetWorldLevels();

            AddRaces(iRacesCount);
            DistributeRacesToLandMasses();
            PopulateAreas();

            BuildProvinces();
            BuildStates();

            BuildCities(m_cGrid.CycleShift, bFast);

            foreach (ContinentX pConti in m_aContinents)
            {
                if (pConti.m_cAreas.Count == 1)
                    pConti.m_sName = pConti.m_cAreas[0].m_sName;

                if (pConti.m_cStates.Count == 1)
                    if (pConti.m_cAreas.Count == 1)
                        pConti.m_cStates[0].m_sName = pConti.m_cAreas[0].m_sName;
                    else
                        pConti.m_sName = pConti.m_cStates[0].m_sName;

                foreach (State pState in pConti.m_cStates)
                    pState.CalculateMagic();
            }
        }

        private void BuildProvinces()
        {
            List<ContinentX> cUsed = new List<ContinentX>();

            List<Province> cProvinces = new List<Province>();

            //Заново стартуем провинции, выжившие с прошлой эпохи
            if(m_aProvinces != null)
                foreach (Province pProvince in m_aProvinces)
                {
                    if (pProvince.m_pCenter.Continent != null && !cUsed.Contains(pProvince.m_pCenter.Continent))
                        cUsed.Add(pProvince.m_pCenter.Continent);

                    pProvince.Start(pProvince.m_pCenter);
                    if (!m_aLocalRaces.Contains(pProvince.m_pRace))
                        throw new Exception();

                    cProvinces.Add(pProvince);
                }
            
            //Позаботимся о том, чтобы на каждом континенте была хотя бы одна провинция
            foreach (ContinentX pConti in m_aContinents)
            {
                if (cUsed.Contains(pConti))
                    continue;

                LandX pSeed = null;
                do
                {
                    int iIndex = Rnd.Get(pConti.m_cAreas.Count);
                    if (!pConti.m_cAreas[iIndex].IsWater)
                    {
                        int iIndex2 = Rnd.Get(pConti.m_cAreas[iIndex].m_cContents.Count);
                        pSeed = pConti.m_cAreas[iIndex].m_cContents[iIndex2];

                        bool bBorder = false;
                        foreach (LocationX pLoc in pSeed.m_cContents)
                            if (pLoc.m_bBorder)
                                bBorder = true;

                        if (bBorder && !Rnd.OneChanceFrom(25))
                            pSeed = null;
                    }
                }
                while (pSeed == null);

                Province pProvince = new Province();
                pProvince.Start(pSeed);
                cProvinces.Add(pProvince);

                if (!m_aLocalRaces.Contains(pProvince.m_pRace))
                    throw new Exception();
                cUsed.Add(pConti);
            }

            while (cProvinces.Count < m_iProvincesCount)
            {
                LandX pSeed = null;
                int iCounter = 0;
                do
                {
                    int iIndex = Rnd.Get(m_aLands.Length);
                    pSeed = m_aLands[iIndex];

                    if (pSeed.Forbidden ||
                        pSeed.m_pProvince != null ||
                        pSeed.IsWater ||
                        pSeed.Owner == null ||
                        (pSeed.Owner as LandMass<LandX>).IsWater)
                    {
                        pSeed = null;
                    }
                    else
                    {
                        bool bBorder = false;
                        foreach (LocationX pLoc in pSeed.m_cContents)
                            if (pLoc.m_bBorder)
                                bBorder = true;

                        if (bBorder)
                            pSeed = null;
                    }
                    if (pSeed == null)
                        iCounter++;
                    else
                        iCounter = 0;
                }
                while (pSeed == null && iCounter < m_aLands.Length*2);

                if (pSeed == null)
                    break;

                Province pProvince = new Province();
                pProvince.Start(pSeed);
                cProvinces.Add(pProvince);

                if (!m_aLocalRaces.Contains(pProvince.m_pRace))
                    throw new Exception();
            }

            int iMaxProvinceSize = (m_aLands.Length * (100 - m_iOceansPercentage) / 100) / m_iProvincesCount;

            List<Province> cFinished = new List<Province>();
            do
            {
                foreach (Province pProvince in cProvinces)
                {
                    if (cFinished.Contains(pProvince))
                        continue;

                    if (!pProvince.Grow(iMaxProvinceSize))
                        cFinished.Add(pProvince);
                }
            }
            while (cFinished.Count < cProvinces.Count);

            foreach (LandX pLand in m_aLands)
            {
                if (!pLand.Forbidden && !pLand.IsWater && pLand.m_pProvince == null)
                {
                    Province pProvince = new Province();
                    pProvince.Start(pLand);
                    cProvinces.Add(pProvince);
                    while (pProvince.Grow(iMaxProvinceSize)) { };
                }
            }

            m_aProvinces = cProvinces.ToArray();

            foreach (Province pProvince in m_aProvinces)
                pProvince.Finish(m_cGrid.CycleShift);
        }

        private void BuildStates()
        {
            List<ContinentX> cUsed = new List<ContinentX>();

            List<State> cStates = new List<State>();

            //Заново стартуем государства, выжившие с прошлой эпохи
            if(m_aStates != null)
                foreach (State pState in m_aStates)
                {
                    pState.Start(pState.m_pMethropoly);
                    if(!m_aLocalRaces.Contains(pState.m_pRace))
                        throw new Exception();
            
                    ContinentX pConti = pState.Owner as ContinentX;
                    if (!cUsed.Contains(pConti))
                        cUsed.Add(pConti);

                    cStates.Add(pState);
                }
            
            //Позаботимся о том, чтобы каждая раса имела хотя бы одно государство
            /*
            foreach (Race pRace in m_cLocalRaces)
            {
                bool bHomeless = true;
                foreach (State pState in m_cStates)
                    if (pState.m_pRace == pRace)
                    {
                        bHomeless = false;
        
                        ContinentX pConti = pState.Owner as ContinentX;
                        if (!cUsed.Contains(pConti))
                            cUsed.Add(pConti);
                    }

                if (!bHomeless)
                    continue;

                Dictionary<Province, float> cChances = new Dictionary<Province, float>();
                foreach (Province pProvince in m_cProvinces)
                {
                    if (pProvince.Forbidden ||
                        pProvince.Owner != null ||
                        //pLand.Type == LandType.Sea ||
                        pProvince.m_pRace != pRace ||
                        pProvince.m_pCenter.IsWater)
                        continue;

                    bool bBorder = false;
                    foreach (LandX pLand in pProvince.m_cContents)
                        foreach (LocationX pLoc in pLand.m_cContents)
                            if (pLoc.m_bBorder)
                                bBorder = true;

                    if (bBorder)
                        continue;

                    cChances[pProvince] = 1.0f;// / pRace.m_iRank;

                    foreach (LandTypeInfoX pType in pRace.m_cPrefferedLands)
                        if (pProvince.m_pCenter.Type == pType)
                            cChances[pProvince] *= 100;

                    foreach (LandTypeInfoX pType in pRace.m_cHatedLands)
                        if (pProvince.m_pCenter.Type == pType)
                            cChances[pProvince] /= 100;
                }

                int iChance = Rnd.ChooseOne(cChances.Values, 2);
                Province pMethropoly = null;
                foreach (Province pProvince in cChances.Keys)
                {
                    iChance--;
                    if (iChance < 0)
                    {
                        pMethropoly = pProvince;
                        break;
                    }
                }

                if (pMethropoly != null && pMethropoly.m_pCenter.Area != null)
                {
                    (pMethropoly.m_pCenter.Area as AreaX).m_pRace = pRace;
                    (pMethropoly.m_pCenter.Area as AreaX).m_sName = NameGenerator.GetEthnicName(pRace.m_eLanguage);
                    foreach (LandX pLand in (pMethropoly.m_pCenter.Area as AreaX).m_cContents)
                    {
                        pLand.m_sName = (pMethropoly.m_pCenter.Area as AreaX).m_sName;
                        pLand.m_pRace = (pMethropoly.m_pCenter.Area as AreaX).m_pRace;
                    }

                    State pState = new State();
                    pState.Start(pMethropoly);
                    m_cStates.Add(pState);

                    if (!m_cLocalRaces.Contains(pState.m_pRace))
                        throw new Exception();

                    cUsed.Add(pMethropoly.m_pCenter.Continent);
                }
            }
             */

            //Позаботимся о том, чтобы на каждом континенте было хотя бы одно государство
            foreach (ContinentX pConti in m_aContinents)
            {
                if (cUsed.Contains(pConti))
                    continue;

                Province pSeed = null;
                do
                {
                    int iIndex = Rnd.Get(pConti.m_cAreas.Count);
                    if(!pConti.m_cAreas[iIndex].IsWater)
                    {
                        int iIndex2 = Rnd.Get(pConti.m_cAreas[iIndex].m_cContents.Count);
                        pSeed = pConti.m_cAreas[iIndex].m_cContents[iIndex2].m_pProvince;

                        bool bBorder = true;
                        foreach (LandX pLand in pSeed.m_cContents)
                            foreach (LocationX pLoc in pLand.m_cContents)
                                if (!pLoc.m_bBorder)
                                    bBorder = false;

                        if (bBorder)
                            pSeed = null;

                        if (pSeed.Owner != null)
                            pSeed = null;
                    }
                }
                while (pSeed == null); 
                
                State pState = new State();
                pState.Start(pSeed);
                cStates.Add(pState);

                if (!m_aLocalRaces.Contains(pState.m_pRace))
                    throw new Exception();
                cUsed.Add(pConti);
            }

            while(cStates.Count < m_iStatesCount)
            {
                Province pSeed = null;
                do
                {
                    int iIndex = Rnd.Get(m_aProvinces.Length);
                    pSeed = m_aProvinces[iIndex];

                    if (pSeed.Forbidden ||
                        pSeed.m_pCenter.IsWater ||
                        pSeed.Owner != null)
                    {
                        pSeed = null;
                    }
                    else
                    {
                        bool bBorder = false;
                        foreach (LandX pLand in pSeed.m_cContents)
                            foreach (LocationX pLoc in pLand.m_cContents)
                                if (pLoc.m_bBorder)
                                    bBorder = true;

                        if (bBorder)
                            pSeed = null;
                    }
                }
                while (pSeed == null); 

                State pState = new State();
                pState.Start(pSeed);
                cStates.Add(pState);
                if (!m_aLocalRaces.Contains(pState.m_pRace))
                    throw new Exception();
            }

            bool bContinue = false;
            do
            {
                bContinue = false;
                foreach (State pState in cStates)
                    if (pState.Grow())
                        bContinue = true;
            }
            while (bContinue);

            foreach(Province pProvince in m_aProvinces)
            {
                if (!pProvince.Forbidden && pProvince.Owner == null)
                {
                    State pState = new State();
                    pState.Start(pProvince);
                    cStates.Add(pState);
                    while (pState.Grow()) { }
                }
            }

            m_aStates = cStates.ToArray();

            foreach (State pState in m_aStates)
            {
                pState.Finish(m_cGrid.CycleShift);

                ContinentX pConti = null;
                foreach (Province pProvince in pState.m_cContents)
                {
                    foreach (LandX pLand in pProvince.m_cContents)
                    {
                        pConti = (pLand.Owner as LandMass<LandX>).Owner as ContinentX;
                        break;
                    }
                    if (pConti != null)
                        break;
                }

                pState.Owner = pConti;
                pConti.m_cStates.Add(pState);
            }
        }

        public override void PresetLandTypesInfo()
        {
            base.PresetLandTypesInfo();

            LandTypes<LandTypeInfoX>.Sea.SetResources(0, 0, 0);
            LandTypes<LandTypeInfoX>.Sea.SetStandAloneBuildingsProbability(0, 0, 1);
            LandTypes<LandTypeInfoX>.Sea.SetSettlementsDensity(0, 0, 0);
            LandTypes<LandTypeInfoX>.Sea.SetColor(Color.FromArgb(0x1e, 0x5e, 0x69));//(0x2a, 0x83, 0x93);//(0x36, 0xa9, 0xbd);//FromArgb(0xa2, 0xed, 0xfa);//LightSkyBlue;//LightCyan;

            LandTypes<LandTypeInfoX>.Tundra.SetResources(0.5f, 0, 0);
            LandTypes<LandTypeInfoX>.Tundra.SetStandAloneBuildingsProbability(1, 0, 10);
            LandTypes<LandTypeInfoX>.Tundra.SetSettlementsDensity(0.004f, 0.01f, 0.003f);
            LandTypes<LandTypeInfoX>.Tundra.SetColor(Color.FromArgb(0xc9, 0xff, 0xff));//(0xc9, 0xe0, 0xff);//PaleGreen;
            
            LandTypes<LandTypeInfoX>.Plains.SetResources(5, 0, 0);
            LandTypes<LandTypeInfoX>.Plains.SetStandAloneBuildingsProbability(1, 3, 30);
            LandTypes<LandTypeInfoX>.Plains.SetSettlementsDensity(0.01f, 0.026f, 0.1f);
            LandTypes<LandTypeInfoX>.Plains.SetColor(Color.FromArgb(0xd3, 0xfa, 0x5f));//(0xdc, 0xfa, 0x83);//LightGreen;
            
            LandTypes<LandTypeInfoX>.Savanna.SetResources(2, 0, 0);
            LandTypes<LandTypeInfoX>.Savanna.SetStandAloneBuildingsProbability(1, 3, 20);
            LandTypes<LandTypeInfoX>.Savanna.SetSettlementsDensity(0.01f, 0.023f, 0.02f);
            LandTypes<LandTypeInfoX>.Savanna.SetColor(Color.FromArgb(0xf0, 0xff, 0x8a));//(0xbd, 0xb0, 0x6b);//PaleGreen;
            
            LandTypes<LandTypeInfoX>.Desert.SetResources(0.5f, 0, 0);
            LandTypes<LandTypeInfoX>.Desert.SetStandAloneBuildingsProbability(1, 2, 30);
            LandTypes<LandTypeInfoX>.Desert.SetSettlementsDensity(0.006f, 0.01f, 0.003f);
            LandTypes<LandTypeInfoX>.Desert.SetColor(Color.FromArgb(0xfa, 0xdc, 0x36));//(0xf9, 0xfa, 0x8a);//LightYellow;
            
            LandTypes<LandTypeInfoX>.Forest.SetResources(1, 5, 0);
            LandTypes<LandTypeInfoX>.Forest.SetStandAloneBuildingsProbability(10, 5, 10);
            LandTypes<LandTypeInfoX>.Forest.SetSettlementsDensity(0.008f, 0.01f, 0.01f);
            LandTypes<LandTypeInfoX>.Forest.SetColor(Color.FromArgb(0x56, 0x78, 0x34));//(0x63, 0x78, 0x4e);//LightGreen;//ForestGreen;
            
            LandTypes<LandTypeInfoX>.Taiga.SetResources(1, 5, 0);
            LandTypes<LandTypeInfoX>.Taiga.SetStandAloneBuildingsProbability(10, 5, 10);
            LandTypes<LandTypeInfoX>.Taiga.SetSettlementsDensity(0.008f, 0.01f, 0.01f);
            LandTypes<LandTypeInfoX>.Taiga.SetColor(Color.FromArgb(0x63, 0x78, 0x4e));//LightGreen;//ForestGreen;
            
            LandTypes<LandTypeInfoX>.Jungle.SetResources(0.5f, 2, 0);
            LandTypes<LandTypeInfoX>.Jungle.SetStandAloneBuildingsProbability(10, 5, 10);
            LandTypes<LandTypeInfoX>.Jungle.SetSettlementsDensity(0.008f, 0.006f, 0.006f);
            LandTypes<LandTypeInfoX>.Jungle.SetColor(Color.FromArgb(0x8d, 0xb7, 0x31));//(0x72, 0x94, 0x28);//PaleGreen;
            
            LandTypes<LandTypeInfoX>.Swamp.SetResources(0.5f, 1, 0);
            LandTypes<LandTypeInfoX>.Swamp.SetStandAloneBuildingsProbability(10, 8, 0);
            LandTypes<LandTypeInfoX>.Swamp.SetSettlementsDensity(0.003f, 0.005f, 0.026f);
            LandTypes<LandTypeInfoX>.Swamp.SetColor(Color.FromArgb(0xa7, 0xbd, 0x6b));// DarkKhaki;
            
            LandTypes<LandTypeInfoX>.Mountains.SetResources(1, 0, 10);
            LandTypes<LandTypeInfoX>.Mountains.SetStandAloneBuildingsProbability(1, 1, 7);
            LandTypes<LandTypeInfoX>.Mountains.SetSettlementsDensity(0.004f, 0.005f, 0.006f);
            LandTypes<LandTypeInfoX>.Mountains.SetColor(Color.FromArgb(0xbd, 0x6d, 0x46));//Tan;
        }

        private void BuildInterstateRoads(float fCycleShift)
        {
            foreach (State pState in m_aStates)
            {
                if (!pState.Forbidden)
                {
                    foreach (State pBorderState in pState.m_aBorderWith)
                    //foreach (State pBorderState in m_cStates)
                    {
                        if (!pBorderState.Forbidden && pBorderState != pState)
                        {
                            float fMinLength = float.MaxValue;
                            LocationX pBestTown1 = null;
                            LocationX pBestTown2 = null;
                            foreach (LocationX pTown in pState.m_cSettlements)
                            {
                                foreach (LocationX pOtherTown in pBorderState.m_cSettlements)
                                {
                                    if (pTown != pOtherTown && !pTown.m_cHaveRoadTo.ContainsKey(pOtherTown))
                                    {
                                        float fDist = pTown.DistanceTo(pOtherTown, fCycleShift);// (float)Math.Sqrt((pTown.X - pOtherTown.X) * (pTown.X - pOtherTown.X) + (pTown.Y - pOtherTown.Y) * (pTown.Y - pOtherTown.Y));

                                        if (fDist < fMinLength)
                                        {
                                            fMinLength = fDist;

                                            pBestTown1 = pTown;
                                            pBestTown2 = pOtherTown;
                                        }
                                    }
                                }
                            }
                            if (pBestTown1 != null && State.InfrastructureLevels[pState.m_iInfrastructureLevel].m_iMaxGroundRoad > 0 && State.InfrastructureLevels[pBorderState.m_iInfrastructureLevel].m_iMaxGroundRoad > 0)
                            {
                                int iMaxRoadLevel = 1;
                                foreach(var pRoad in pBestTown1.m_cRoads)
                                    if(pRoad.Value.Count > 0 && pRoad.Key > iMaxRoadLevel)
                                        iMaxRoadLevel = pRoad.Key;
                                foreach(var pRoad in pBestTown2.m_cRoads)
                                    if (pRoad.Value.Count > 0 && pRoad.Key > iMaxRoadLevel)
                                        iMaxRoadLevel = pRoad.Key;
                                //int iRoadLevel = 2;
                                //if (State.LifeLevels[pState.m_iLifeLevel].m_iMaxGroundRoad <= 1 && State.LifeLevels[pBorderState.m_iLifeLevel].m_iMaxGroundRoad <= 1)
                                //    iRoadLevel = 1;
                                //if (State.LifeLevels[pState.m_iLifeLevel].m_iMaxGroundRoad > 2 && State.LifeLevels[pBorderState.m_iLifeLevel].m_iMaxGroundRoad > 2)
                                //    iRoadLevel = 3;

                                BuildRoad(pBestTown1, pBestTown2, iMaxRoadLevel, fCycleShift);
                            }
                        }
                    }
                }
            }
        }

        private void BuildSeaRoutes(float fCycleShift)
        {
            foreach (LandX pLand in m_aLands)
                pLand.m_bHarbor = false;

            foreach (LandMass<LandX> pLandMass in m_aLandMasses)
                pLandMass.m_bHarbor = false;

            List<LocationX> cHarbors = new List<LocationX>();
            foreach (State pState in m_aStates)
            {
                if (!pState.Forbidden)
                {
                    foreach (LocationX pTown in pState.m_cSettlements)
                    {
                        pTown.m_bHarbor = false;

                        if (pTown.m_pSettlement.m_iRuinsAge > 0)
                            continue;

                        if (pTown.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Fort ||
                            //pTown.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Village ||
                            pTown.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Hamlet)
                            continue;

                        bool bCoastral = false;
                        foreach (ITerritory pTerr in pTown.m_aBorderWith)
                        {
                            if (pTerr.Forbidden || pTerr.Owner == null)
                                continue;

                            if ((pTerr.Owner as LandX).IsWater)
                            {
                                bCoastral = true;
                                //SetLink(pTown.m_cBorderWith[pTerr][0].m_pPoint1, pTown);
                                //SetLink(pTown.m_cBorderWith[pTerr][0].m_pPoint2, pTown);
                            }
                        }
                        if (bCoastral)
                        {
                            cHarbors.Add(pTown);
                            pTown.m_bHarbor = true;
                            (pTown.Owner as LandX).m_bHarbor = true;
                            ((pTown.Owner as LandX).Owner as LandMass<LandX>).m_bHarbor = true;
                        }
                    }
                }
            }

            LocationX[] aHarbors = cHarbors.ToArray();
            foreach (LocationX pHarbor in aHarbors)
            {
                //if (pHarbor.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Village)// ||
                //pHarbor.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Town)
                //continue;

                State pState = (pHarbor.Owner as LandX).m_pProvince.Owner as State;
                int iMaxNavalPath = State.InfrastructureLevels[pState.m_iInfrastructureLevel].m_iMaxNavalPath;
                if (iMaxNavalPath == 0)
                    continue;

                int iMaxLength = m_cGrid.RX * 10;
                if (iMaxNavalPath == 1)
                    iMaxLength /= 10;
                if (iMaxNavalPath == 2)
                    iMaxLength /= 4;

                float fMinDist = float.MaxValue;
                LocationX pClosestHarbor1 = null;
                LocationX pClosestHarbor2 = null;
                LocationX pClosestHarbor3 = null;
                foreach (LocationX pOtherHarbor in aHarbors)
                {
                    if (pHarbor.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Village &&
                        (pOtherHarbor.m_pSettlement.m_pInfo.m_eSize == SettlementSize.City ||
                         pOtherHarbor.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Capital))
                        continue;

                    if (pHarbor.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Town &&
                        pOtherHarbor.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Village)
                        continue;

                    if (pHarbor.m_pSettlement.m_pInfo.m_eSize == SettlementSize.City &&
                        pOtherHarbor.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Village)
                        continue;

                    if (pHarbor.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Capital &&
                        pOtherHarbor.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Village)
                        continue;

                    if (pHarbor != pOtherHarbor && !pHarbor.m_cHaveSeaRouteTo.Contains(pOtherHarbor))
                    {
                        float fDist = pHarbor.DistanceTo(pOtherHarbor, fCycleShift);
                        if (fDist < fMinDist)
                        {
                            fMinDist = fDist;

                            if (pClosestHarbor1 != null)
                            {
                                if (pClosestHarbor2 != null)
                                    pClosestHarbor3 = pClosestHarbor2;
                                pClosestHarbor2 = pClosestHarbor1;
                            }
                            pClosestHarbor1 = pOtherHarbor;
                        }
                    }
                }

                int iPathLevel = 3;

                if (pHarbor.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Village)
                {
                    iPathLevel = 1;
                    iMaxLength = m_cGrid.RX * 10 / 10;
                }
                if (pHarbor.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Town)
                    iPathLevel = 2;

                if (pClosestHarbor1 != null)
                    BuildSeaRoute(pHarbor, pClosestHarbor1, iPathLevel, fCycleShift, iMaxLength);
                if (pClosestHarbor2 != null && iPathLevel >= 1)
                    BuildSeaRoute(pHarbor, pClosestHarbor2, iPathLevel, fCycleShift, iMaxLength);
                if (pClosestHarbor3 != null && iPathLevel >= 3)
                    BuildSeaRoute(pHarbor, pClosestHarbor3, iPathLevel, fCycleShift, iMaxLength);
            }

            FinishSeaRoutes(fCycleShift);
        }

        private Dictionary<ShortestPath, int> m_cSeaRoutesProjects = new Dictionary<ShortestPath,int>();

        private void BuildSeaRoute(LocationX pTown1, LocationX pTown2, int iPathLevel, float fCycleShift, int iMaxLength)
        {
            //TransportationNode[] aBestPath = FindBestPath(pTown1, pTown2, fCycleShift, true).m_aNodes;
            //if (aBestPath == null || aBestPath.Length == 0)
            //    aBestPath = FindBestPath(pTown2, pTown1, fCycleShift, true).m_aNodes;
            ShortestPath pBestPath = FindReallyBestPath(pTown1, pTown2, fCycleShift, true);

            if (pBestPath.m_aNodes != null && pBestPath.m_aNodes.Length > 1 && pBestPath.m_fLength < iMaxLength)
            //if (aBestPath != null && aBestPath.Length > 1)
            {
                m_cSeaRoutesProjects[pBestPath] = iPathLevel;

                pTown1.m_cHaveSeaRouteTo.Add(pTown2);
                pTown2.m_cHaveSeaRouteTo.Add(pTown1);
            }
        }

        private void FinishSeaRoutes(float fCycleShift)
        {
            foreach (var pPath in m_cSeaRoutesProjects)
            {
                ShortestPath pBestPath = FindReallyBestPath(pPath.Key.m_aNodes[0] as LocationX, 
                                                            pPath.Key.m_aNodes[pPath.Key.m_aNodes.Length - 1] as LocationX, 
                                                            fCycleShift, false);

                TransportationLink pNewLink = new TransportationLink(pPath.Key.m_aNodes);
                pNewLink.m_bEmbark = true;
                pNewLink.m_bSea = true;
                pNewLink.BuildRoad(pPath.Value);

                if (pBestPath.m_aNodes != null && 
                    pBestPath.m_aNodes.Length > 1 && 
                    pBestPath.m_fLength < pNewLink.MovementCost)
                    continue;

                SetLink(pPath.Key.m_aNodes[0], pPath.Key.m_aNodes[pPath.Key.m_aNodes.Length - 1], pNewLink);//aSeaRoute);

                LocationX pHarbor1 = pPath.Key.m_aNodes[0] as LocationX;
                LocationX pHarbor2 = pPath.Key.m_aNodes[pPath.Key.m_aNodes.Length - 1] as LocationX;

                if ((pHarbor1.Owner as LandX).m_pProvince.Owner != (pHarbor2.Owner as LandX).m_pProvince.Owner)
                {
                    State pState1 = (pHarbor1.Owner as LandX).m_pProvince.Owner as State;
                    State pState2 = (pHarbor2.Owner as LandX).m_pProvince.Owner as State;

                    List<object> pState1Neighbours = new List<object>(pState1.m_aBorderWith);
                    if (!pState1Neighbours.Contains(pState2))
                        pState1Neighbours.Add(pState2);
                    pState1.m_aBorderWith = pState1Neighbours.ToArray();

                    List<object> pState2Neighbours = new List<object>(pState2.m_aBorderWith);
                    if (!pState2Neighbours.Contains(pState1))
                        pState2Neighbours.Add(pState1);
                    pState2.m_aBorderWith = pState2Neighbours.ToArray();
                }
            }

            m_cSeaRoutesProjects.Clear();
        }

        private void BuildCities(float fCycleShift, bool bFast)
        {
            foreach (State pState in m_aStates)
            {
                if (!pState.Forbidden)
                {
                    pState.BuildCapital(m_aProvinces.Length / (2*m_iStatesCount), m_aProvinces.Length / m_iStatesCount, bFast, m_iMinTechLevel, m_iMaxTechLevel);
                    if(!bFast)
                        pState.BuildRoads(3, fCycleShift);
                }
            }

            Dictionary<State, Dictionary<State, int>> cHostility = new Dictionary<State, Dictionary<State, int>>();

            foreach (State pState in m_aStates)
            {
                if (!pState.Forbidden)
                {
                    pState.BuildSettlements(SettlementSize.City, bFast);
                    if (!bFast)
                        pState.BuildRoads(3, fCycleShift);

                    pState.BuildSettlements(SettlementSize.Town, bFast);
                    pState.BuildForts(cHostility, bFast);

                    if (!bFast)
                        pState.BuildRoads(2, fCycleShift);
                }
            }

            if (!bFast)
                BuildInterstateRoads(fCycleShift);

            foreach (State pState in m_aStates)
            {
                if (!pState.Forbidden)
                {
                    pState.BuildSettlements(SettlementSize.Village, bFast);

                    if (!bFast)
                        pState.BuildRoads(1, fCycleShift);

                    pState.BuildSettlements(SettlementSize.Hamlet, bFast);

                    pState.BuildLairs(m_aLands.Length / 125);
                }
            }

            if (!bFast)
                BuildSeaRoutes(fCycleShift);
        }

        public void Reset()
        {
            List<Race> cEraseRace = new List<Race>();
            foreach (Race pRace in m_aLocalRaces)
                if (!Rnd.OneChanceFrom(3))
                    cEraseRace.Add(pRace);

            List<Race> cRaces = new List<Race>(m_aLocalRaces);

            foreach (Race pRace in cEraseRace)
            {
                cRaces.Remove(pRace);

                foreach (ContinentX pConti in m_aContinents)
                {
                    foreach (LandMass<LandX> pLandMass in pConti.m_cLocalRaces.Keys)
                    {
                        if (pConti.m_cLocalRaces[pLandMass].Contains(pRace))
                            pConti.m_cLocalRaces[pLandMass].Remove(pRace);

                        foreach (Race pRce in pConti.m_cLocalRaces[pLandMass])
                            if (pRce == pRace)
                            {
                                if (pConti.m_cLocalRaces[pLandMass].Contains(pRace))
                                    pConti.m_cLocalRaces[pLandMass].Remove(pRace);
                                throw new Exception();
                            }
                    }

                    foreach (AreaX pArea in pConti.m_cAreas)
                    {
                        if (pArea.m_pRace == pRace)
                            pArea.m_pRace = null;

                        foreach (LandX pLand in pArea.m_cContents)
                        {
                            if (pLand.m_pRace == pRace)
                                pLand.m_pRace = pArea.m_pRace;
                        }
                    }
                }
            }

            m_aLocalRaces = cRaces.ToArray();

            //foreach (ContinentX pConti in m_cContinents)
            //{
            //    foreach (LandMass<LandX> pLandMass in pConti.m_cLocalRaces.Keys)
            //    {
            //        foreach (Race pRace in pConti.m_cLocalRaces[pLandMass])
            //            if (!m_cLocalRaces.Contains(pRace))
            //                throw new Exception();
            //    }
            //    foreach (AreaX pArea in pConti.m_cAreas)
            //        foreach (LandX pLand in pArea.m_cContents)
            //            if(pLand.m_pRace != null && !m_cLocalRaces.Contains(pLand.m_pRace))
            //                throw new Exception();
            //}

            List<State> cEraseState = new List<State>();
            foreach (State pState in m_aStates)
            {
                if (!pState.Forbidden)
                {
                    foreach (Province pProvince in pState.m_cContents)
                    {
                        pProvince.Owner = null;
                        foreach (LandX pLand in pProvince.m_cContents)
                        {
                            pLand.m_pProvince = null;
                            foreach (LocationX pLoc in pLand.m_cContents)
                            {
                                if (pLoc.m_pSettlement != null)
                                {
                                    if (pLoc.m_pSettlement.m_iRuinsAge > 0)
                                        if (!Rnd.OneChanceFrom((int)pLand.MovementCost))
                                        {
                                            pLoc.m_pSettlement.m_iRuinsAge++;
                                            pLoc.m_bHarbor = false;
                                        }
                                        else
                                        {
                                            pLoc.m_pSettlement = null;
                                            pLoc.m_bHarbor = false;
                                            if (pState.m_cSettlements.Contains(pLoc))
                                                pState.m_cSettlements.Remove(pLoc);
                                        }
                                }

                                pLoc.m_pBuilding = null;
                            }
                        }
                    }

                    foreach (LocationX pSett in pState.m_cSettlements)
                    {
                        switch (pSett.m_pSettlement.m_pInfo.m_eSize)
                        { 
                            case SettlementSize.Village:
                                break;
                            case SettlementSize.Town:
                                if (Rnd.OneChanceFrom(1 + (int)(24 / pSett.GetMovementCost())))
                                    pSett.m_pSettlement.m_iRuinsAge++;
                                break;
                            case SettlementSize.City:
                                if (Rnd.OneChanceFrom(1 + (int)(12 / pSett.GetMovementCost())))
                                    pSett.m_pSettlement.m_iRuinsAge++;
                                break;
                            case SettlementSize.Capital:
                                if (Rnd.OneChanceFrom(1 + (int)(6 / pSett.GetMovementCost())))
                                    pSett.m_pSettlement.m_iRuinsAge++;
                                break;
                            case SettlementSize.Fort:
                                if (Rnd.OneChanceFrom(1 + (int)(6 / pSett.GetMovementCost())))
                                    pSett.m_pSettlement.m_iRuinsAge++;
                                break;
                        }

                        if (pSett == pState.m_pCapital && pSett.m_pSettlement.m_pInfo.m_eSize != SettlementSize.Village)
                            pSett.m_pSettlement.m_iRuinsAge = 1;

                        if (pSett.m_pSettlement.m_iRuinsAge == 0)
                            pSett.m_pSettlement = null;

                        pSett.m_bHarbor = false;
                    }
                    pState.m_cSettlements.Clear();

                    if (!m_aLocalRaces.Contains(pState.m_pRace) || Rnd.OneChanceFrom(2))
                        cEraseState.Add(pState);
                }
            }

            List<State> cStates = new List<State>(m_aStates);
            foreach (State pState in cEraseState)
                cStates.Remove(pState);
            m_aStates = cStates.ToArray();

            List<Province> cProvinces = new List<Province>();
            foreach (State pState in m_aStates)
                cProvinces.Add(pState.m_pMethropoly);
            m_aProvinces = cProvinces.ToArray();

            foreach (ContinentX pConti in m_aContinents)
                pConti.m_cStates.Clear();

            foreach (TransportationLink pRoad in m_cTransportGrid)
                pRoad.ClearRoad();
            foreach (TransportationLink pRoad in m_cLandsTransportGrid)
                pRoad.ClearRoad();
            foreach (TransportationLink pRoad in m_cLMTransportGrid)
                pRoad.ClearRoad();

            foreach (LocationX pLoc in m_cGrid.m_aLocations)
            {
                pLoc.m_cRoads.Clear();
                pLoc.m_cRoads[1] = new List<Road>();
                pLoc.m_cRoads[2] = new List<Road>();
                pLoc.m_cRoads[3] = new List<Road>();
                pLoc.m_cHaveRoadTo.Clear();
                pLoc.m_cHaveSeaRouteTo.Clear();

                if (pLoc.Owner != null && !(pLoc.Owner as LandX).IsWater)
                {
                    List<TransportationNode> cErase = new List<TransportationNode>();
                    foreach (TransportationNode pNode in pLoc.m_cLinks.Keys)
                    {
                        if (pLoc.m_cLinks[pNode].m_bSea)
                            cErase.Add(pNode);
                    }
                    foreach (TransportationNode pNode in cErase)
                    {
                        pLoc.m_cLinks.Remove(pNode);
                        pNode.m_cLinks.Remove(pLoc);
                    }
                }
            }
        }

        private static void DestroyRoad(Road pOldRoad)
        {
            LocationX pTown1 = pOldRoad.Locations[0];
            LocationX pTown2 = pOldRoad.Locations[pOldRoad.Locations.Length - 1];

            //удаляем старую дорогу
            foreach (LocationX pLoc in pOldRoad.Locations)
            {
                if (pLoc != pTown2)
                    pLoc.m_cHaveRoadTo.Remove(pTown2);
                if (pLoc != pTown1)
                    pLoc.m_cHaveRoadTo.Remove(pTown1);

                pLoc.m_cRoads[pOldRoad.m_iLevel].Remove(pOldRoad);
            }
        }

        /// <summary>
        /// Подготовка к строительству новой дороги:
        /// проверяем, нет ли уже между городами старой дороги, и если есть, то не лучше ли она чем та, которую хотим построоить.
        /// если хуже, то удаляем старую дорогу.
        /// Возвращаемое значение: true - можно строить новую дорогу, false - нельзя (уже есть такого же уровня или лучше)
        /// </summary>
        /// <param name="pTown1">первый город</param>
        /// <param name="pTown2">второй город</param>
        /// <param name="iRoadLevel">уровень новой дороги</param>
        /// <returns>true - можно строить новую дорогу, false - нельзя</returns>
        private static bool CheckOldRoad(LocationX pTown1, LocationX pTown2, int iRoadLevel)
        {
            //проверим, а нет ли уже между этими городами дороги
            while (pTown1.m_cHaveRoadTo.ContainsKey(pTown2) && pTown2.m_cHaveRoadTo.ContainsKey(pTown1))
            {
                Road pOldRoad = pTown1.m_cHaveRoadTo[pTown2];

                //если существующая дорога лучше или такая же, как та, которую мы собираемся строить, то нафиг строить?
                if (pOldRoad.m_iLevel >= iRoadLevel)
                    return false;

                //удаляем старую дорогу
                DestroyRoad(pOldRoad);
            }

            return true;
        }

        /// <summary>
        /// Отстраивает заданный участок дороги.
        /// Предполагается, что на этом участке дорога не проходит ни через какие населённые пункты.
        /// </summary>
        /// <param name="pRoad">участок дороги</param>
        /// <param name="iRoadLevel">уровень дороги</param>
        private static void BuildRoad(Road pRoad, int iRoadLevel)
        {
            LocationX pTown1 = pRoad.Locations[0];
            LocationX pTown2 = pRoad.Locations[pRoad.Locations.Length - 1];

            //если между этими двумя городами уже есть дорога не хуже - новую не строим
            if (!CheckOldRoad(pTown1, pTown2, iRoadLevel))
                return;

            LocationX pLastNode = null;
            foreach (LocationX pNode in pRoad.Locations)
            {
                if (pNode != pTown1 && !pNode.m_cHaveRoadTo.ContainsKey(pTown1))
                    pNode.m_cHaveRoadTo[pTown1] = pRoad;
                if (pNode != pTown2 && !pNode.m_cHaveRoadTo.ContainsKey(pTown2))
                    pNode.m_cHaveRoadTo[pTown2] = pRoad;

                pNode.m_cRoads[iRoadLevel].Add(pRoad);

                if (pLastNode != null)
                {
                    pLastNode.m_cLinks[pNode].BuildRoad(iRoadLevel);
                    //pNode.m_cLinks[pLastNode].BuildRoad(iRoadLevel);

                    if (pLastNode.Owner != pNode.Owner)
                    {
                        try
                        {
                            (pLastNode.Owner as LandX).m_cLinks[pNode.Owner as LandX].BuildRoad(iRoadLevel);

                            if ((pLastNode.Owner as LandX).Owner != (pNode.Owner as LandX).Owner)
                            {
                                ((pLastNode.Owner as LandX).Owner as LandMass<LandX>).m_cLinks[(pNode.Owner as LandX).Owner as LandMass<LandX>].BuildRoad(iRoadLevel);
                            }
                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine(ex.Message);
                        }
                    }
                }

                pLastNode = pNode;
            }
        }

        /// <summary>
        /// "обновляем" дорогу.
        /// если она проходит через какие-то населённые пункты, то разбиваем её на отдельный сегменты 
        /// от одного поселения до другого.
        /// </summary>
        /// <param name="pRoad">дорога</param>
        public static void RenewRoad(Road pOldRoad)
        {
            //разобьем найденный путь на участки от одного населённого пункта до другого
            List<Road> cRoadsChain = new List<Road>();
            Road pNewRoad = null;
            foreach (LocationX pNode in pOldRoad.Locations)
            {
                if (pNewRoad == null)
                    pNewRoad = new Road(pNode, pOldRoad.m_iLevel);
                else
                {
                    pNewRoad.BuidTo(pNode);

                    if (pNode.m_pSettlement != null && pNode.m_pSettlement.m_iRuinsAge == 0)
                    {
                        cRoadsChain.Add(pNewRoad);
                        pNewRoad = new Road(pNode, pOldRoad.m_iLevel);
                    }
                }
            }
            if (cRoadsChain.Count == 1)
                return;

            DestroyRoad(pOldRoad);
        
            Road[] aRoadsChain = cRoadsChain.ToArray();

            foreach (Road pRoad in aRoadsChain)
                BuildRoad(pRoad, pOldRoad.m_iLevel);
        }

        /// <summary>
        /// Строит дорогу из одного города в другой. Если дорога проходит через другие населённые пункты, она разбивается на 
        /// отдельные участки от одного поселения до другого.
        /// </summary>
        /// <param name="pTown1">первый город</param>
        /// <param name="pTown2">второй город</param>
        /// <param name="iRoadLevel">уровень строящейся дороги</param>
        /// <param name="fCycleShift">циклический сдвиг координат по горизонтали для закольцованных карт</param>
        public static void BuildRoad(LocationX pTown1, LocationX pTown2, int iRoadLevel, float fCycleShift)
        {
            if (!CheckOldRoad(pTown1, pTown2, iRoadLevel))
                return;

            //PathFinder pBestPath = new PathFinder(pTown1, pTown2, fCycleShift, -1);
            ShortestPath pBestPath = FindReallyBestPath(pTown1, pTown2, fCycleShift, false);

            if (pBestPath.m_aNodes != null && pBestPath.m_aNodes.Length > 1)
            {
                //разобьем найденный путь на участки от одного населённого пункта до другого
                List<Road> cRoadsChain = new List<Road>();
                Road pNewRoad = null;
                foreach (LocationX pNode in pBestPath.m_aNodes)
                {
                    if (pNewRoad == null)
                        pNewRoad = new Road(pNode, iRoadLevel);
                    else
                    {
                        pNewRoad.BuidTo(pNode);

                        if (pNode.m_pSettlement != null && pNode.m_pSettlement.m_iRuinsAge == 0)
                        {
                            cRoadsChain.Add(pNewRoad);
                            pNewRoad = new Road(pNode, iRoadLevel);
                        }
                    }
                }

                Road[] aRoadsChain = cRoadsChain.ToArray();

                foreach (Road pRoad in aRoadsChain)
                    BuildRoad(pRoad, iRoadLevel);
            }
        }
    }
}
