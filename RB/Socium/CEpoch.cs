using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;

namespace RB.Socium
{
    public class CEpoch
    {
        public class ProgressPreset
        {
            #region Static members

            private enum Progress
            {
                HistoricalAntiquity,
                HistoricalMedieval,
                HistoricalRenaissance,
                HistoricalModern,
                Cyberpunk,
                SpaceOpera,
                AntiqueMythology,
                FantasyLowMagic,
                FantasyHighMagic,
                Technomagic,
                UrbanFantasy,
                Superheroes
            }

            private static Dictionary<Progress, ProgressPreset> s_cPresets = new Dictionary<Progress, ProgressPreset>();

            /// <summary>
            /// An antique world without magic - like Ancient Greece, Rome, Egypt, Assyria, etc.
            /// </summary>
            public static ProgressPreset HistoricalAntiquity
            {
                get
                {
                    if (!s_cPresets.ContainsKey(Progress.HistoricalAntiquity))
                        s_cPresets[Progress.HistoricalAntiquity] = new ProgressPreset("Historical - antiquity", "An antique world without magic - like Ancient Greece, Rome, Egypt, Assyria, etc.", 0, 1, 0, 0);

                    return s_cPresets[Progress.HistoricalAntiquity];
                }
            }

            /// <summary>
            /// A medieval world without magic - castles, knights, tournaments, etc.
            /// </summary>
            public static ProgressPreset HistoricalMedieval
            {
                get
                {
                    if (!s_cPresets.ContainsKey(Progress.HistoricalMedieval))
                        s_cPresets[Progress.HistoricalMedieval] = new ProgressPreset("Historical - medieval", "A medieval world without magic - castles, knights, tournaments, etc.", 0, 2, 0, 0);

                    return s_cPresets[Progress.HistoricalMedieval];
                }
            }

            /// <summary>
            /// A renaissance world without magic - musketeers, geographic exploration, etc.
            /// </summary>
            public static ProgressPreset HistoricalRenaissance
            {
                get
                {
                    if (!s_cPresets.ContainsKey(Progress.HistoricalRenaissance))
                        s_cPresets[Progress.HistoricalRenaissance] = new ProgressPreset("Historical - renaissance", "A renaissance world without magic - musketeers, geographic exploration, etc.", 1, 3, 0, 0);

                    return s_cPresets[Progress.HistoricalRenaissance];
                }
            }

            /// <summary>
            /// A modern world without magic - railroads, aviation, world wars, etc.
            /// </summary>
            public static ProgressPreset HistoricalModern
            {
                get
                {
                    if (!s_cPresets.ContainsKey(Progress.HistoricalModern))
                        s_cPresets[Progress.HistoricalModern] = new ProgressPreset("Historical - modern", "A modern world without magic - railroads, aviation, world wars, etc.", 4, 5, 0, 0);

                    return s_cPresets[Progress.HistoricalModern];
                }
            }

            /// <summary>
            /// Nearest future world without magic - advanced technologies, mega-corporations, industrial espionage, etc.
            /// </summary>
            public static ProgressPreset Cyberpunk
            {
                get
                {
                    if (!s_cPresets.ContainsKey(Progress.Cyberpunk))
                        s_cPresets[Progress.Cyberpunk] = new ProgressPreset("Cyberpunk", "Nearest future world without magic - advanced technologies, mega-corporations, industrial espionage, etc.", 4, 6, 0, 0);

                    return s_cPresets[Progress.Cyberpunk];
                }
            }

            /// <summary>
            /// Far future world with a bit of magic (aka psi-abilities) - like Star Wars, Star Trek, etc.
            /// </summary>
            public static ProgressPreset SpaceOpera
            {
                get
                {
                    if (!s_cPresets.ContainsKey(Progress.SpaceOpera))
                        s_cPresets[Progress.SpaceOpera] = new ProgressPreset("Space opera", "Far future world with a bit of magic (aka psi-abilities) - like Star Wars, Star Trek, etc.", 6, 7, 1, 3);

                    return s_cPresets[Progress.SpaceOpera];
                }
            }

            /// <summary>
            /// A world of antique mythology - just a usual antique world, but with a bit of magic...
            /// (Usual people in myths has very little magic...)
            /// </summary>
            public static ProgressPreset AntiqueMythology
            {
                get
                {
                    if (!s_cPresets.ContainsKey(Progress.AntiqueMythology))
                        s_cPresets[Progress.AntiqueMythology] = new ProgressPreset("Antique mythology", "A world of antique mythology - just a usual antique world, but with a bit of magic...", 0, 1, 1, 2);

                    return s_cPresets[Progress.AntiqueMythology];
                }
            }

            /// <summary>
            /// A medieval world with a bit of magic - like Knights of the Round Table, Lord of the Rings, etc.
            /// </summary>
            public static ProgressPreset FantasyLowMagic
            {
                get
                {
                    if (!s_cPresets.ContainsKey(Progress.FantasyLowMagic))
                        s_cPresets[Progress.FantasyLowMagic] = new ProgressPreset("Fantasy - low magic", "A medieval world with a bit of magic - like Knights of the Round Table, Lord of the Rings, etc.", 1, 2, 1, 3);

                    return s_cPresets[Progress.FantasyLowMagic];
                }
            }

            /// <summary>
            /// A medieval world with a lot of magic - like Dragonlance, Wheel of Time, etc.
            /// </summary>
            public static ProgressPreset FantasyHighMagic
            {
                get
                {
                    if (!s_cPresets.ContainsKey(Progress.FantasyHighMagic))
                        s_cPresets[Progress.FantasyHighMagic] = new ProgressPreset("Fantasy - high magic", "A medieval world with a lot of magic - like Dragonlance, Wheel of Time, etc.", 1, 2, 2, 4);

                    return s_cPresets[Progress.FantasyHighMagic];
                }
            }

            /// <summary>
            /// A renaissance world with a lot of magic - like Arcanum, Final Fantasy, etc.
            /// </summary>
            public static ProgressPreset Technomagic
            {
                get
                {
                    if (!s_cPresets.ContainsKey(Progress.Technomagic))
                        s_cPresets[Progress.Technomagic] = new ProgressPreset("Technomagic", "A renaissance world with a lot of magic - like Arcanum, Final Fantasy, etc.", 1, 3, 2, 4);

                    return s_cPresets[Progress.Technomagic];
                }
            }

            /// <summary>
            /// A modern world with a bit of magic (aka supernatural abilities) - like Superman, Fantastic Four, Spiderman, etc.
            /// </summary>
            public static ProgressPreset Superheroes
            {
                get
                {
                    if (!s_cPresets.ContainsKey(Progress.Superheroes))
                        s_cPresets[Progress.Superheroes] = new ProgressPreset("Superheroes", "A modern world with a bit of magic (aka supernatural abilities) - like Superman, Fantastic Four, Spiderman, etc.", 4, 5, 1, 3);

                    return s_cPresets[Progress.Superheroes];
                }
            }

            /// <summary>
            /// A modern world with a lot of magic - vampires, werewolfs, voodoo, secret societies, etc.
            /// </summary>
            public static ProgressPreset UrbanFantasy
            {
                get
                {
                    if (!s_cPresets.ContainsKey(Progress.UrbanFantasy))
                        s_cPresets[Progress.UrbanFantasy] = new ProgressPreset("Urban fantasy", "A modern world with a lot of magic - vampires, werewolfs, voodoo, secret societies, etc.", 4, 5, 2, 4);

                    return s_cPresets[Progress.UrbanFantasy];
                }
            }

            #endregion

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

        private static string[] s_cNames =
        { 
            "Age of Destruction",
            "Age of Legends",
            "Dark Age",
            "Forgotten Age",
            "Golden Age",
            "Silver Age",
            "Age of Steel",
            "Age of Light",
            "Age of Fire",
            "Age of Water",
            "Age of Rebirth",
            "Age of Heroes",
            "Age of Darkness",
            "Age of Oblivion",
            "Cursed Age",
            "Diamond Age",
            "Age of Sky",
            "Age of Fall",
            "Age of Miths",
            "Age of Blood",
            "Lost Age",
            "Age of Hate",
            "Age of Wars",
            "Age of Freedom",
            "Age of Storms",
            "Age of Turmoil",
            "Age of Destiny",
        };

        public static List<string> s_cUsedNames = new List<string>();

        public string m_sName;

        public int m_iNativesMinTechLevel;
        public int m_iNativesMaxTechLevel;
        public int m_iNativesMinMagicLevel;
        public int m_iNativesMaxMagicLevel;
        
        public CRace[] m_aNatives;

        public int m_iInvadersMinTechLevel;
        public int m_iInvadersMaxTechLevel;
        public int m_iInvadersMinMagicLevel;
        public int m_iInvadersMaxMagicLevel;
        
        public CRace[] m_aInvaders;

        public int m_iNativesCount;
        public int m_iInvadersCount;

        //public int m_iLength;

        public CEpoch(ProgressPreset pNativesPreset, CRace[] aNatives, int iNativesCount)
            : this(pNativesPreset, aNatives, iNativesCount, pNativesPreset, aNatives, 0)
        { }


        public CEpoch(ProgressPreset pNativesPreset, CRace[] aNatives, int iNativesCount, ProgressPreset pInvadersPreset, CRace[] aInvaders, int iInvadersCount)
        {
            m_iNativesMinTechLevel = pNativesPreset.m_iMinTechLevel;
            m_iNativesMaxTechLevel = pNativesPreset.m_iMaxTechLevel;

            m_iNativesMinMagicLevel = pNativesPreset.m_iMinMagicLevel;
            m_iNativesMaxMagicLevel = pNativesPreset.m_iMaxMagicLevel;


            m_iInvadersMinTechLevel = pInvadersPreset.m_iMinTechLevel;
            m_iInvadersMaxTechLevel = pInvadersPreset.m_iMaxTechLevel;

            m_iInvadersMinMagicLevel = pInvadersPreset.m_iMinMagicLevel;
            m_iInvadersMaxMagicLevel = pInvadersPreset.m_iMaxMagicLevel;

            m_aNatives = aNatives;
            m_aInvaders = aInvaders;

            m_iNativesCount = iNativesCount;

            if (m_aNatives.Length == 0 && m_iNativesCount > 0)
                throw new ArgumentException("Natives races array is empty!");

            m_iInvadersCount = iInvadersCount;

            if (m_aInvaders.Length == 0 && m_iInvadersCount > 0)
                throw new ArgumentException("Invaders races array is empty!");

            //m_iLength = 3;

            do
            {
                m_sName = s_cNames[Rnd.Get(s_cNames.Length)];
            }
            while (s_cUsedNames.Contains(m_sName));

            s_cUsedNames.Add(m_sName);
        }
    }
}
