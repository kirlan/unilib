using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;
using NameGen;

namespace VixenQuest.World
{
    public enum Sapience
    { 
        Animal,
        Human,
        God
    }

    public class Race
    {
        public Sapience m_eSapience;
        public string m_sName;
        public string m_sNameF;
        public int m_iRank;
        public NameGenerator.Language m_eLanguage;

        public VixenSkill m_ePreferredSexType = VixenSkill.Traditional;
        public Orientation m_ePreferredOrientation = Orientation.Stright;

        public Race(Sapience eSapience, string sName, int iRank, NameGenerator.Language eLanguage)
            : this(eSapience, sName, sName, iRank, eLanguage)
        { 
        }

        public Race(Sapience eSapience, string sName, string sNameF, int iRank, NameGenerator.Language eLanguage)
        {
            m_eSapience = eSapience;
            m_sName = sName;
            m_sNameF = sNameF;
            m_iRank = iRank;
            m_eLanguage = eLanguage;

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

    public abstract class Space
    {
        protected static string[] m_aEpithet = 
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

        //private static string[] m_aPlace = 
        //{
        //    "Forest",
        //    "Woods",
        //    "Grove",
        //    "Sands",
        //    "Desert",
        //    "Wastes",
        //    "Lake",
        //    "River",
        //    "Mansion",
        //    "Castle",
        //    "Palace",
        //    "Cave",
        //    "Country",
        //    "Kingdom",
        //    "Cathedral",
        //    "Plains",
        //    "Shore",
        //    "Swamp",
        //    "Spot",
        //    "Gates",
        //    "Valley",
        //    "Grotto",
        //    "Peak",
        //    "Mountain",
        //    "Weil",
        //    "Wall",
        //    "Cemetary",
        //    "Graveyard",
        //    "Ruins",
        //    "Realm",
        //    "Plane",
        //    "Dimension",
        //    "Hole",
        //    "Hills",
        //    "Hill",
        //    "Mountains",
        //    "Hideout",
        //    "Nest",
        //    "Garden",
        //    "Lair",
        //};

        protected static string[] m_aDescription = 
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


        //Человеческие профессии имеют ранги от 1 до 17, плюс настроение 1-20, 
        //поэтому мы будем наращивать ранги рас с шагом 10, 
        //чтобы лучшие из более слабой расы были так же круты, как худшие из более сильной
        protected static Race[] m_aAllRaces = 
        {
            //new Race(Sapience.Animal, "giant rat ", 1),
            new Race(Sapience.Animal, "badger ", 1, NameGenerator.Language.NONE),
            new Race(Sapience.Animal, "rabbit ", 1, NameGenerator.Language.NONE),
            //new Race(Sapience.Animal, "giant frog ", 1),
            //new Race(Sapience.Animal, "squirrel ", 1),
            new Race(Sapience.Animal, "wombat ", 1, NameGenerator.Language.NONE),
            new Race(Sapience.Animal, "opossum ", 1, NameGenerator.Language.NONE),
            new Race(Sapience.Animal, "fox ", "vixen ", 1, NameGenerator.Language.NONE),
            //new Race(Sapience.Animal, "hamster ", 1),
            new Race(Sapience.Human, "", 1, NameGenerator.Language.European),
            new Race(Sapience.Human, "", 1, NameGenerator.Language.Viking),
            new Race(Sapience.Human, "", 1, NameGenerator.Language.Greek),
            new Race(Sapience.Human, "", 1, NameGenerator.Language.Arabian),
            new Race(Sapience.Human, "", 1, NameGenerator.Language.Scotish),
            new Race(Sapience.Human, "barbarian ", 1, NameGenerator.Language.Scotish),
            new Race(Sapience.Human, "black ", 1, NameGenerator.Language.African),
            new Race(Sapience.Human, "", 1, NameGenerator.Language.European),
            new Race(Sapience.Animal, "bear ", 10, NameGenerator.Language.NONE),
            new Race(Sapience.Animal, "wolf ", 10, NameGenerator.Language.NONE),
            new Race(Sapience.Animal, "bull ", "cow ", 10, NameGenerator.Language.NONE),
            new Race(Sapience.Animal, "dog ", 10, NameGenerator.Language.NONE),
            new Race(Sapience.Animal, "stallion ", "horse ", 10, NameGenerator.Language.NONE),
            new Race(Sapience.Human, "gypsy ", 10, NameGenerator.Language.Drow),
            new Race(Sapience.Human, "nomad ", 10, NameGenerator.Language.Arabian),
            new Race(Sapience.Human, "viking ", "amazon ", 10, NameGenerator.Language.Viking),
            new Race(Sapience.Human, "caveman ", "cavewoman ", 10, NameGenerator.Language.Escimo),
            new Race(Sapience.Human, "werewolf ", 10, NameGenerator.Language.European),
            new Race(Sapience.Human, "wererat ", 10, NameGenerator.Language.European),
            new Race(Sapience.Human, "werebear ", 10, NameGenerator.Language.European),
            new Race(Sapience.Animal, "monkey ", 10, NameGenerator.Language.NONE),
            new Race(Sapience.Animal, "panther ", 10, NameGenerator.Language.NONE),
            new Race(Sapience.Animal, "jaguar ", 10, NameGenerator.Language.NONE),
            new Race(Sapience.Animal, "tiger ", "tigress ", 10, NameGenerator.Language.NONE),
            new Race(Sapience.Animal, "lion ", "lioness ", 10, NameGenerator.Language.NONE),
            new Race(Sapience.Human, "yeti ", 20, NameGenerator.Language.Escimo),
            new Race(Sapience.Human, "little ", 20, NameGenerator.Language.Elven),
            new Race(Sapience.Human, "orc ", 20, NameGenerator.Language.Orcish),
            new Race(Sapience.Human, "goblin ", 20, NameGenerator.Language.Orcish),
            new Race(Sapience.Human, "centaur ", 20, NameGenerator.Language.Greek),
            new Race(Sapience.Human, "ogre ", 20, NameGenerator.Language.Orcish),
            new Race(Sapience.Human, "gnoll ", 20, NameGenerator.Language.Orcish),
            new Race(Sapience.Human, "halfling ", 20, NameGenerator.Language.European),
            new Race(Sapience.Human, "cobold ", 20, NameGenerator.Language.Dwarven),
            new Race(Sapience.Human, "minotaur ", 20, NameGenerator.Language.Greek),
            //new Race(Sapience.Human, "snake ", 20),
            new Race(Sapience.Human, "lizard ", 20, NameGenerator.Language.Aztec),
            new Race(Sapience.Human, "satyr ", "nimph ", 20, NameGenerator.Language.Greek),
            new Race(Sapience.Human, "merman ", "mermaid ", 20, NameGenerator.Language.Greek),
            new Race(Sapience.Human, "reptile ", 20, NameGenerator.Language.Aztec),
            new Race(Sapience.Animal, "ape ", 20, NameGenerator.Language.NONE),
            new Race(Sapience.Animal, "ooze ", 20, NameGenerator.Language.NONE),
            new Race(Sapience.Animal, "harpy ", 20, NameGenerator.Language.NONE),
            new Race(Sapience.Human, "half-elf ", 30, NameGenerator.Language.Elfish),
            new Race(Sapience.Human, "half-orc ", 30, NameGenerator.Language.Orcish),
            new Race(Sapience.Human, "half-dragon ", 30, NameGenerator.Language.Drow),
            new Race(Sapience.Human, "half-dwarf ", 30, NameGenerator.Language.Dwarven),
            new Race(Sapience.Human, "half-faery ", 30, NameGenerator.Language.Elven),
            new Race(Sapience.Human, "weredragon ", 30, NameGenerator.Language.Drow),
            new Race(Sapience.Human, "golem ", 30, NameGenerator.Language.Aztec),
            new Race(Sapience.Human, "phantom ", 30, NameGenerator.Language.European),
            new Race(Sapience.Animal, "viviern ", 30, NameGenerator.Language.NONE),
            new Race(Sapience.Animal, "griffon ", 30, NameGenerator.Language.NONE),
            new Race(Sapience.Animal, "undead ", 30, NameGenerator.Language.Aztec),
            new Race(Sapience.Animal, "zombie ", 30, NameGenerator.Language.European),
            new Race(Sapience.Human, "elven ", 40, NameGenerator.Language.Elven),
            new Race(Sapience.Human, "dwarven ", 40, NameGenerator.Language.Dwarven),
            new Race(Sapience.Human, "naga ", 40, NameGenerator.Language.Greek),
            new Race(Sapience.Human, "faery ", 40, NameGenerator.Language.Elfish),
            new Race(Sapience.Human, "pixie ", 40, NameGenerator.Language.Elven),
            new Race(Sapience.Animal, "sphinx ", 40, NameGenerator.Language.NONE),
            new Race(Sapience.Animal, "manticore ", 40, NameGenerator.Language.NONE),
            new Race(Sapience.Human, "drow ", 40, NameGenerator.Language.Drow),
            new Race(Sapience.Animal, "dragon ", 50, NameGenerator.Language.NONE),
            new Race(Sapience.Human, "lich ", 50, NameGenerator.Language.Aztec),
            new Race(Sapience.Human, "vampire ", 50, NameGenerator.Language.European),
            new Race(Sapience.Animal, "unicorn ", 60, NameGenerator.Language.NONE),
            new Race(Sapience.Human, "incubus ", "succubus ", 60, NameGenerator.Language.European),
            new Race(Sapience.God, "spirit ", 60, NameGenerator.Language.Any),
            new Race(Sapience.God, "lesser demon ", "lesser demoness ", 60, NameGenerator.Language.Any),
            new Race(Sapience.God, "demon ", "demoness ", 70, NameGenerator.Language.Any),
            new Race(Sapience.God, "angel ", 70, NameGenerator.Language.Any),
            new Race(Sapience.God, "greater demon ", "greater demoness ", 80, NameGenerator.Language.Any),
            new Race(Sapience.God, "archangel ", 80, NameGenerator.Language.Any),
            new Race(Sapience.God, "lesser god ", "lesser goddess ", 90, NameGenerator.Language.Any),
            new Race(Sapience.God, "god ", "goddess ", 100, NameGenerator.Language.Any),
        };

        public string m_sName;

        public Space(string sName)
        {
            m_sName = sName;
            m_iTier = 0;
        }

        public int m_iTier = 0;

        public Space()
        { }

        public abstract Opponent[] AvailableOpponents();

        public override string ToString()
        {
            return m_sName;
        }
    }
}
