using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;
using Socium.Nations;
using nsUniLibXML;
using System.Xml;

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
        
        public List<Race> m_cNatives = new List<Race>();

        public int m_iInvadersMinTechLevel;
        public int m_iInvadersMaxTechLevel;
        public int m_iInvadersMinMagicLevel;
        public int m_iInvadersMaxMagicLevel;
        
        public List<Race> m_cInvaders = new List<Race>();

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

            m_cNatives.AddRange(Race.m_cAllRaces);
            m_cInvaders.AddRange(Race.m_cAllRaces);

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

        public Epoch(UniLibXML pXml, XmlNode pEpochNode)
        {
            pXml.GetStringAttribute(pEpochNode, "name", ref m_sName);
            pXml.GetIntAttribute(pEpochNode, "length", ref m_iLength);

            pXml.GetIntAttribute(pEpochNode, "natives", ref m_iNativesCount);
            
            pXml.GetIntAttribute(pEpochNode, "nativesTLmin", ref m_iNativesMinTechLevel);
            pXml.GetIntAttribute(pEpochNode, "nativesTLmax", ref m_iNativesMaxTechLevel);

            pXml.GetIntAttribute(pEpochNode, "nativesMLmin", ref m_iNativesMinMagicLevel);
            pXml.GetIntAttribute(pEpochNode, "nativesMLmax", ref m_iNativesMaxMagicLevel);

            pXml.GetIntAttribute(pEpochNode, "invaders", ref m_iInvadersCount);

            pXml.GetIntAttribute(pEpochNode, "invadersTLmin", ref m_iInvadersMinTechLevel);
            pXml.GetIntAttribute(pEpochNode, "invadersTLmax", ref m_iInvadersMaxTechLevel);

            pXml.GetIntAttribute(pEpochNode, "invadersMLmin", ref m_iInvadersMinMagicLevel);
            pXml.GetIntAttribute(pEpochNode, "invadersMLmax", ref m_iInvadersMaxMagicLevel);

            foreach (XmlNode pSubNode in pEpochNode.ChildNodes)
            {
                if (pSubNode.Name == "Native")
                {
                    string sRaceName = "";
                    pXml.GetStringAttribute(pSubNode, "name", ref sRaceName);

                    foreach (Race pRace in Race.m_cAllRaces)
                        if (pRace.m_sName == sRaceName)
                        {
                            m_cNatives.Add(pRace);
                            break;
                        }
                }
                if (pSubNode.Name == "Invader")
                {
                    string sRaceName = "";
                    pXml.GetStringAttribute(pSubNode, "name", ref sRaceName);

                    foreach (Race pRace in Race.m_cAllRaces)
                        if (pRace.m_sName == sRaceName)
                        {
                            m_cInvaders.Add(pRace);
                            break;
                        }
                }
            }
        }

        public void Write(UniLibXML pXml, XmlNode pEpochNode)
        {
            pXml.AddAttribute(pEpochNode, "name", m_sName);
            pXml.AddAttribute(pEpochNode, "length", m_iLength);

            pXml.AddAttribute(pEpochNode, "natives", m_iNativesCount);

            pXml.AddAttribute(pEpochNode, "nativesTLmin", m_iNativesMinTechLevel);
            pXml.AddAttribute(pEpochNode, "nativesTLmax", m_iNativesMaxTechLevel);

            pXml.AddAttribute(pEpochNode, "nativesMLmin", m_iNativesMinMagicLevel);
            pXml.AddAttribute(pEpochNode, "nativesMLmax", m_iNativesMaxMagicLevel);

            pXml.AddAttribute(pEpochNode, "invaders", m_iInvadersCount);

            pXml.AddAttribute(pEpochNode, "invadersTLmin", m_iInvadersMinTechLevel);
            pXml.AddAttribute(pEpochNode, "invadersTLmax", m_iInvadersMaxTechLevel);

            pXml.AddAttribute(pEpochNode, "invadersMLmin", m_iInvadersMinMagicLevel);
            pXml.AddAttribute(pEpochNode, "invadersMLmax", m_iInvadersMaxMagicLevel);

            foreach (Race pRace in m_cNatives)
            {
                XmlNode pRaceNode = pXml.CreateNode(pEpochNode, "Native");
                pXml.AddAttribute(pRaceNode, "name", pRace.m_sName);
            }

            foreach (Race pRace in m_cInvaders)
            {
                XmlNode pRaceNode = pXml.CreateNode(pEpochNode, "Invader");
                pXml.AddAttribute(pRaceNode, "name", pRace.m_sName);
            }
        }
    }
}
