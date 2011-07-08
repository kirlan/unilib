using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;
using Socium.Nations;

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
            "Age of Blood",
            "Lost Age",
            "Age of Hate",
            "Age of Wars",
            "Age of Freedom",
        };

        public static List<string> s_cUsedNames = new List<string>();

        public string m_sName;

        public int m_iNativesMinTechLevel;
        public int m_iNativesMaxTechLevel;
        public int m_iNativesMinMagicLevel;
        public int m_iNativesMaxMagicLevel;
        
        public List<Race> m_cNatives;

        public int m_iInvadersMinTechLevel;
        public int m_iInvadersMaxTechLevel;
        public int m_iInvadersMinMagicLevel;
        public int m_iInvadersMaxMagicLevel;
        
        public List<Race> m_cInvaders;

        public int m_iNativesCount;
        public int m_iInvadersCount;

        public int m_iLength;

        public Epoch()
        {
            m_iNativesMinTechLevel = 1;
            m_iNativesMaxTechLevel = 3;

            m_iNativesMinMagicLevel = 1;
            m_iNativesMaxMagicLevel = 3;

            
            m_iInvadersMinTechLevel = 5;
            m_iInvadersMaxTechLevel = 8;

            m_iInvadersMinMagicLevel = 5;
            m_iInvadersMaxMagicLevel = 8;

            
            m_cNatives = new List<Race>(Race.m_cAllRaces);
            m_cInvaders = new List<Race>(Race.m_cAllRaces);

            m_iNativesCount = 60;

            m_iInvadersCount = 0;

            m_iLength = 3;

            do
            {
                m_sName = s_cNames[Rnd.Get(s_cNames.Length)];
            }
            while (s_cUsedNames.Contains(m_sName));

            s_cUsedNames.Add(m_sName);
        }
    }
}
