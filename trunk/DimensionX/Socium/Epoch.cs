using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;

namespace Socium
{
    public class Epoch
    {
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
            "Lost Age",
            "Age of Hate",
            "Age of Wars",
            "Age of Freedom",
        };

        public static List<string> s_cUsedNames = new List<string>();

        public string m_sName;

        public int m_iMinTechLevel;
        public int m_iMaxTechLevel;
        public int m_iMinMagicLevel;
        public int m_iMaxMagicLevel;

        public List<RaceTemplate> m_cRaceTemplates;

        public int m_iInvadersMinTechLevel;
        public int m_iInvadersMaxTechLevel;
        public int m_iInvadersMinMagicLevel;
        public int m_iInvadersMaxMagicLevel;

        public List<RaceTemplate> m_cInvadersRaceTemplates;

        public int m_iNativesCount;
        public int m_iInvadersCount;

        public int m_iLength;

        public Epoch()
        {
            m_iMinTechLevel = 0;
            m_iMaxTechLevel = 8;

            m_iMinMagicLevel = 0;
            m_iMaxMagicLevel = 8;


            m_iInvadersMinTechLevel = 0;
            m_iInvadersMaxTechLevel = 8;

            m_iInvadersMinMagicLevel = 0;
            m_iInvadersMaxMagicLevel = 8;

            m_cRaceTemplates = new List<RaceTemplate>(Race.m_cTemplates);
            m_cInvadersRaceTemplates = new List<RaceTemplate>(Race.m_cTemplates);

            m_iNativesCount = 10;

            m_iInvadersCount = 0;

            m_iLength = 1;

            do
            {
                m_sName = s_cNames[Rnd.Get(s_cNames.Length)];
            }
            while (s_cUsedNames.Contains(m_sName));

            s_cUsedNames.Add(m_sName);
        }
    }
}
