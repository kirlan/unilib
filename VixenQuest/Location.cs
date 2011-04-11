using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;

namespace VixenQuest
{
    enum Sapience
    { 
        Animal,
        Human,
        God
    }

    class Race
    {
        public Sapience m_eSapience;
        public string m_sName;
        public string m_sNameF;
        public int m_iRank;

        public VixenSkill m_ePreferredSexType = VixenSkill.Traditional;
        public Orientation m_ePreferredOrientation = Orientation.Stright;

        public Race(Sapience eSapience, string sName, int iRank)
            : this(eSapience, sName, sName, iRank)
        { 
        }

        public Race(Sapience eSapience, string sName, string sNameF, int iRank)
        {
            m_eSapience = eSapience;
            m_sName = sName;
            m_sNameF = sNameF;
            m_iRank = iRank;

            int skillId = Rnd.Get(Enum.GetValues(typeof(VixenSkill)).Length);
            m_ePreferredSexType = (VixenSkill)Enum.GetValues(typeof(VixenSkill)).GetValue(skillId);

            int orientationId = Rnd.Get(Enum.GetValues(typeof(Orientation)).Length);
            m_ePreferredOrientation = (Orientation)Enum.GetValues(typeof(Orientation)).GetValue(orientationId);
        }

        public override string ToString()
        {
            if(m_eSapience == Sapience.Animal)
                return m_sName + " <lv." + m_iRank.ToString() + ">";
            else
                return m_sName + " <lv." + m_iRank.ToString() + ", " + Enum.GetName(typeof(VixenSkill), m_ePreferredSexType).ToLower() + ">";
        }
    }

    class Location
    {
        private static string[] m_aEpithet = 
        {
            "Drunken ",
            "Silent ",
            "Fucked ",
            "Lustful ",
            "Sinful ",
            "Naked ",
            "Cursed ",
            "Posessed ",
            "Black ",
            "Pink ",
            "Horny ",
            "Diseased ",
            //"Dead ",
            "Hot ",
            "Wet ",
            "Happy ",
            "Extasy ",
            "Blessed ",
            "Dragon's ",
            "Siren's ",
            "Maid's ",
            "Lady's ",
            "Sexy ",
            "Cumming ",
            "Unreal ",
            "Burned ",
            //"Bloody ",
            "Burning ",
            "Haunted ",
            "Corrupted ",
            "Warped ",
            "Desired ",
            "Deep ",
            "Muddy ",
            "Orgazming ",
            "Singing ",
            "Screaming ",
            "Dangerous ",
            "Forgotten ",
            "Secret ",
            "Mysterious ",
            "Mystical ",
            "Beloved ",
            "Summer ",
            "Winter ",
            "Northern ",
            "Southern ",
            "Western ",
            "Eastern ",
            "Sweet ",
        };

        private static string[] m_aPlace = 
        {
            "Forest",
            "Woods",
            "Grove",
            "Sands",
            "Desert",
            "Wastes",
            "Lake",
            "River",
            "Mansion",
            "Castle",
            "Palace",
            "Cave",
            "Country",
            "Kingdom",
            "Cathedral",
            "Plains",
            "Shore",
            "Swamp",
            "Spot",
            "Gates",
            "Valley",
            "Grotto",
            "Peak",
            "Mountain",
            "Weil",
            "Wall",
            "Cemetary",
            "Graveyard",
            "Ruins",
            "Realm",
            "Plane",
            "Dimension",
            "Hole",
            "Hills",
            "Hill",
            "Mountains",
            "Hideout",
            "Nest",
            "Garden",
            "Lair",
        };

        private static string[] m_aDescription = 
        {
            " of Lust",
            " of Love",
            " of Eros",
            " of Sex",
            " of Erotic Dreams",
            " of Wet Dreams",
            " of Hot Dreams",
            " of Incredible Dreams",
            " of Forbidden Dreams",
            " of Fucking Good",
            " of Flying Dildos",
            " of Singing Dildos",
            " of Screaming Dildos",
            " of Jumping Dildos",
            " of Flying Cocks",
            " of Singing Cocks",
            " of Screaming Cocks",
            " of Jumping Cocks",
            " of Flying Whores",
            " of Screaming Whores",
            " of Singing Whores",
            " of Whores",
            " of Lustful Whores",
            " of Virgins",
            " of Nudity",
            " of Desire",
            " of Earthly Delights",
            " of Extasy",
            " of Fertylity"
        };


        //Человеческие профессии имеют ранги от 1 до 8, плюс настроение 1-5, 
        //поэтому мы будем наращивать ранги рас с шагом 10, 
        //чтобы лучшие из более слабой расы были так же круты, как худшие из более сильной
        private static Race[] m_aAllRaces = 
        {
            //new Race(Sapience.Animal, "giant rat ", 1),
            new Race(Sapience.Animal, "badger ", 1),
            new Race(Sapience.Animal, "rabbit ", 1),
            //new Race(Sapience.Animal, "giant frog ", 1),
            //new Race(Sapience.Animal, "squirrel ", 1),
            new Race(Sapience.Animal, "wombat ", 1),
            new Race(Sapience.Animal, "opossum ", 1),
            new Race(Sapience.Animal, "fox ", "vixen ", 1),
            //new Race(Sapience.Animal, "hamster ", 1),
            new Race(Sapience.Human, "", 1),
            new Race(Sapience.Human, "", 1),
            new Race(Sapience.Human, "", 1),
            new Race(Sapience.Human, "", 1),
            new Race(Sapience.Human, "", 1),
            new Race(Sapience.Human, "barbarian ", 1),
            new Race(Sapience.Human, "black ", 1),
            new Race(Sapience.Human, "", 1),
            new Race(Sapience.Animal, "bear ", 10),
            new Race(Sapience.Animal, "wolf ", 10),
            new Race(Sapience.Animal, "fox ", 10),
            new Race(Sapience.Animal, "dog ", 10),
            new Race(Sapience.Animal, "stallion ", "horse ", 10),
            new Race(Sapience.Human, "gypsy ", 10),
            new Race(Sapience.Human, "nomad ", 10),
            new Race(Sapience.Human, "viking ", "amazon ", 10),
            new Race(Sapience.Human, "caveman ", "cavewoman ", 10),
            new Race(Sapience.Human, "undead ", 10),
            new Race(Sapience.Human, "zombie ", 10),
            new Race(Sapience.Human, "werewolf ", 10),
            new Race(Sapience.Human, "wererat ", 10),
            new Race(Sapience.Human, "werebear ", 10),
            new Race(Sapience.Animal, "monkey ", 10),
            new Race(Sapience.Animal, "panther ", 10),
            new Race(Sapience.Animal, "jaguar ", 10),
            new Race(Sapience.Animal, "tiger ", "tigress ", 10),
            new Race(Sapience.Animal, "lion ", "lioness ", 10),
            new Race(Sapience.Human, "yeti ", 20),
            new Race(Sapience.Human, "little ", 20),
            new Race(Sapience.Human, "orc ", 20),
            new Race(Sapience.Human, "goblin ", 20),
            new Race(Sapience.Human, "centaur ", 20),
            new Race(Sapience.Human, "ogre ", 20),
            new Race(Sapience.Human, "gnoll ", 20),
            new Race(Sapience.Human, "halfling ", 20),
            new Race(Sapience.Human, "cobold ", 20),
            new Race(Sapience.Human, "minotaur ", 20),
            //new Race(Sapience.Human, "snake ", 20),
            new Race(Sapience.Human, "lizard ", 20),
            new Race(Sapience.Human, "satyr ", "nimph ", 20),
            new Race(Sapience.Human, "merman ", "mermaid ", 20),
            new Race(Sapience.Human, "reptile ", 20),
            new Race(Sapience.Animal, "ape ", 20),
            new Race(Sapience.Animal, "ooze ", 20),
            new Race(Sapience.Human, "half-elf ", 30),
            new Race(Sapience.Human, "half-orc ", 30),
            new Race(Sapience.Human, "half-dragon ", 30),
            new Race(Sapience.Human, "half-dwarf ", 30),
            new Race(Sapience.Human, "half-faery ", 30),
            new Race(Sapience.Human, "weredragon ", 30),
            new Race(Sapience.Human, "golem ", 30),
            new Race(Sapience.Human, "phantom ", 30),
            new Race(Sapience.Animal, "viviern ", 30),
            new Race(Sapience.Animal, "griffon ", 30),
            new Race(Sapience.Human, "elven ", 40),
            new Race(Sapience.Human, "dwarven ", 40),
            new Race(Sapience.Human, "naga ", 40),
            new Race(Sapience.Human, "faery ", 40),
            new Race(Sapience.Human, "pixie ", 40),
            new Race(Sapience.Human, "dragon ", 40),
            new Race(Sapience.Animal, "sphinx ", 40),
            new Race(Sapience.Animal, "unicorn ", 40),
            new Race(Sapience.Human, "drow ", 40),
            new Race(Sapience.Human, "lich ", 50),
            new Race(Sapience.Human, "vampire ", 50),
            new Race(Sapience.Human, "incubus ", "succubus ", 60),
            new Race(Sapience.God, "spirit ", 60),
            new Race(Sapience.God, "lesser demon ", "lesser demoness ", 60),
            new Race(Sapience.God, "demon ", "demoness ", 70),
            new Race(Sapience.God, "angel ", 70),
            new Race(Sapience.God, "greater demon ", "greater demoness ", 80),
            new Race(Sapience.God, "avatar ", 80),
            new Race(Sapience.God, "lesser god ", "lesser goddess ", 90),
            new Race(Sapience.God, "god ", "goddess ", 100),
        };

        public string m_sName;

        public List<Race> m_cLocalRaces = new List<Race>();

        public Location(string sName)
        {
            m_sName = sName;
            m_iTier = 0;

            foreach (Race pRace in m_aAllRaces)
            {
                m_cLocalRaces.Add(pRace);
            }
        }

        public int m_iTier = 0;

        public Location(int iTier)
        {
            m_iTier = iTier;

            int variant = Rnd.Get(3);

            switch (variant)
            {
                case 0:
                    {
                        int iEpithet = Rnd.Get(m_aEpithet.Length);
                        m_sName = m_aEpithet[iEpithet];

                        int iPlace = Rnd.Get(m_aPlace.Length);
                        m_sName += m_aPlace[iPlace];
                    }
                    break;
                case 1:
                    {
                        int iPlace = Rnd.Get(m_aPlace.Length);
                        m_sName = m_aPlace[iPlace];

                        int iDescription = Rnd.Get(m_aDescription.Length);
                        m_sName += m_aDescription[iDescription];
                    }
                    break;
                default:
                    {
                        int iEpithet = Rnd.Get(m_aEpithet.Length);
                        m_sName = m_aEpithet[iEpithet];

                        int iPlace = Rnd.Get(m_aPlace.Length);
                        m_sName += m_aPlace[iPlace];

                        int iDescription = Rnd.Get(m_aDescription.Length);
                        m_sName += m_aDescription[iDescription];
                    }
                    break;
            }

            //m_sName += " (T" + m_iTier.ToString();// +")";

            int iDiversity = 4 + Rnd.Get(4);
            Race pLocalAnimals = GetRandomAnimal();
            m_cLocalRaces.Add(pLocalAnimals);
            //m_sName += ", " + pLocalAnimals.ToString();

            for (int i = 1; i < iDiversity; i++)
            {
                Race pNewRace = GetRandomRace();

                m_cLocalRaces.Add(pNewRace);

                //m_sName += ", " + pNewRace.ToString();
            }
            //m_sName += ")";
        }

        private Race GetRandomAnimal()
        {
            Race pPretendent;

            Race pBestPretendent = null;
            int iBestDifference = int.MaxValue;

            int iCounter = 0;
            do
            {
                int index = Rnd.Get(m_aAllRaces.Length);
                pPretendent = m_aAllRaces[index];

                int iDifference = Math.Abs(pPretendent.m_iRank - (m_iTier-1)*10);

                if (pPretendent.m_iRank < m_iTier * 10)
                    iDifference = 2 * iDifference;

                if (pPretendent.m_eSapience != Sapience.Animal)
                    iDifference = int.MaxValue;

                if (pBestPretendent == null || iBestDifference > iDifference)
                {
                    pBestPretendent = pPretendent;
                    iBestDifference = iDifference;
                }

                if (Rnd.Gauss(iDifference, 3))
                    return pPretendent;

                if (iCounter++ > 100)
                    return pBestPretendent;
            }
            while (true);
        }

        private Race GetRandomRace()
        {
            Race pPretendent;

            Race pBestPretendent = null;
            int iBestDifference = int.MaxValue;

            int iCounter = 0;
            do
            {
                int index = Rnd.Get(m_aAllRaces.Length);
                pPretendent = m_aAllRaces[index];

                int iDifference = Math.Abs(pPretendent.m_iRank - (m_iTier-1)*10);

                if (pPretendent.m_iRank < m_iTier * 10)
                    iDifference = 2 * iDifference;

                if (pPretendent.m_eSapience == Sapience.Animal && !Rnd.OneChanceFrom(4))
                    iDifference = int.MaxValue;

                if (pBestPretendent == null || iBestDifference > iDifference)
                {
                    pBestPretendent = pPretendent;
                    iBestDifference = iDifference;
                }


                if (Rnd.Gauss(iDifference, 3))
                    return pPretendent;

                if (iCounter++ > 100)
                    return pBestPretendent;
            }
            while (true);
        }

        public Opponent GetLover(Vixen pVixen, bool bBoss)
        {
            int index;

            do
            {
                index = Rnd.Get(m_cLocalRaces.Count);
            }
            while (bBoss && m_cLocalRaces[index].m_eSapience == Sapience.Animal);

            Opponent pPretendent = GetDweller(pVixen, m_cLocalRaces[index], bBoss);

            return pPretendent;
        }

        private Opponent GetDweller(Vixen pVixen, Race pRace, bool bBoss)
        {
            int iTargetLevel = pVixen.EffectiveLevel;
            if (bBoss)
                iTargetLevel += (int)Math.Sqrt(iTargetLevel);

            Opponent pBestPretendent = null;
            int iBestDifference = int.MaxValue;

            int iCounter = 0;
            do
            {
                Opponent pPretendent = new Opponent(pRace);

                int iDifference = Math.Abs(pPretendent.EncounterRank - iTargetLevel);

                if (pPretendent.EncounterRank < iTargetLevel)
                    iDifference = iDifference * iDifference;

                if (!pVixen.WannaFuck(pPretendent) && (!pPretendent.WannaFuck(pVixen) || bBoss))
                    iDifference = int.MaxValue;

                if (pBestPretendent == null || iBestDifference > iDifference)
                {
                    pBestPretendent = pPretendent;
                    iBestDifference = iDifference;
                }

                if (Rnd.Gauss(iDifference, (int)Math.Sqrt(iTargetLevel)))
                    return pPretendent;

                if (iCounter++ > 1000)
                {
                    return pBestPretendent;
                }
            }
            while (true);
        }
    }
}
