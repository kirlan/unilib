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
        private static readonly string[] s_cNames =
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

        private static readonly List<string> s_cUsedNames = new List<string>();

        public static void ResetUsedList()
        {
            s_cUsedNames.Clear();
        }

        //can't make them auto-implement properties, since they are needed to be passed as ref parameters
#pragma warning disable S2292 // Trivial properties should be auto-implemented
        private string m_sName;
        public string Name { get => m_sName; set => m_sName = value; }

        private int m_iNativesMinTechLevel;
        public int NativesMinTechLevel { get => m_iNativesMinTechLevel; set => m_iNativesMinTechLevel = value; }

        private int m_iNativesMaxTechLevel;
        public int NativesMaxTechLevel { get => m_iNativesMaxTechLevel; set => m_iNativesMaxTechLevel = value; }

        private int m_iNativesMinMagicLevel;
        public int NativesMinMagicLevel { get => m_iNativesMinMagicLevel; set => m_iNativesMinMagicLevel = value; }

        private int m_iNativesMaxMagicLevel;
        public int NativesMaxMagicLevel { get => m_iNativesMaxMagicLevel; set => m_iNativesMaxMagicLevel = value; }

        private int m_iInvadersMinTechLevel;
        public int InvadersMinTechLevel { get => m_iInvadersMinTechLevel; set => m_iInvadersMinTechLevel = value; }

        private int m_iInvadersMaxTechLevel;
        public int InvadersMaxTechLevel { get => m_iInvadersMaxTechLevel; set => m_iInvadersMaxTechLevel = value; }

        private int m_iInvadersMinMagicLevel;
        public int InvadersMinMagicLevel { get => m_iInvadersMinMagicLevel; set => m_iInvadersMinMagicLevel = value; }

        private int m_iInvadersMaxMagicLevel;
        public int InvadersMaxMagicLevel { get => m_iInvadersMaxMagicLevel; set => m_iInvadersMaxMagicLevel = value; }

        private int m_iNativesCount;
        public int NativesCount { get => m_iNativesCount; set => m_iNativesCount = value; }

        private int m_iInvadersCount;
        public int InvadersCount { get => m_iInvadersCount; set => m_iInvadersCount = value; }

        private int m_iLength;
        public int Length { get => m_iLength; set => m_iLength = value; }
#pragma warning restore S2292 // Trivial properties should be auto-implemented

        public List<Race> Natives { get; } = new List<Race>();

        public List<Race> Invaders { get; } = new List<Race>();

        public Epoch()
        {
            NativesMinTechLevel = 1;
            NativesMaxTechLevel = 3;

            NativesMinMagicLevel = 1;
            NativesMaxMagicLevel = 3;

            InvadersMinTechLevel = 5;
            InvadersMaxTechLevel = 8;

            InvadersMinMagicLevel = 5;
            InvadersMaxMagicLevel = 8;

            Natives.AddRange(Race.m_cAllRaces);
            Invaders.AddRange(Race.m_cAllRaces);

            NativesCount = 60;

            InvadersCount = 0;

            Length = 3;

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
                    {
                        if (pRace.Name == sRaceName)
                        {
                            Natives.Add(pRace);
                            break;
                        }
                    }
                }
                if (pSubNode.Name == "Invader")
                {
                    string sRaceName = "";
                    pXml.GetStringAttribute(pSubNode, "name", ref sRaceName);

                    foreach (Race pRace in Race.m_cAllRaces)
                    {
                        if (pRace.Name == sRaceName)
                        {
                            Invaders.Add(pRace);
                            break;
                        }
                    }
                }
            }
        }

        public void Write(UniLibXML pXml, XmlNode pEpochNode)
        {
            pXml.AddAttribute(pEpochNode, "name", m_sName);
            pXml.AddAttribute(pEpochNode, "length", Length);

            pXml.AddAttribute(pEpochNode, "natives", NativesCount);

            pXml.AddAttribute(pEpochNode, "nativesTLmin", m_iNativesMinTechLevel);
            pXml.AddAttribute(pEpochNode, "nativesTLmax", NativesMaxTechLevel);

            pXml.AddAttribute(pEpochNode, "nativesMLmin", NativesMinMagicLevel);
            pXml.AddAttribute(pEpochNode, "nativesMLmax", NativesMaxMagicLevel);

            pXml.AddAttribute(pEpochNode, "invaders", InvadersCount);

            pXml.AddAttribute(pEpochNode, "invadersTLmin", InvadersMinTechLevel);
            pXml.AddAttribute(pEpochNode, "invadersTLmax", InvadersMaxTechLevel);

            pXml.AddAttribute(pEpochNode, "invadersMLmin", InvadersMinMagicLevel);
            pXml.AddAttribute(pEpochNode, "invadersMLmax", InvadersMaxMagicLevel);

            foreach (Race pRace in Natives)
            {
                XmlNode pRaceNode = pXml.CreateNode(pEpochNode, "Native");
                pXml.AddAttribute(pRaceNode, "name", pRace.Name);
            }

            foreach (Race pRace in Invaders)
            {
                XmlNode pRaceNode = pXml.CreateNode(pEpochNode, "Invader");
                pXml.AddAttribute(pRaceNode, "name", pRace.Name);
            }
        }
    }
}
