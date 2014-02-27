using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nsUniLibXML;
using System.Xml;
using Random;

namespace RandomStory
{
    public class World
    {
        public World m_pCommon = null;

        public string m_sName;

        public List<string> m_cRaces = new List<string>();

        public List<string> m_cPerks = new List<string>();

        public List<string> m_cProfessions = new List<string>();

        public List<string> m_cProfessionsEvil = new List<string>();

        public List<string> m_cLocations = new List<string>();

        public List<string> m_cItems = new List<string>();

        public World()
        {
            m_sName = "Новый мир";
        }

        public World(List<World> cWorlds)
        {
            m_sName = "Общие параметры";

            if (cWorlds.Count > 0)
            {
                m_cRaces.AddRange(cWorlds[0].m_cRaces);
                m_cPerks.AddRange(cWorlds[0].m_cPerks);
                m_cProfessions.AddRange(cWorlds[0].m_cProfessions);
                m_cProfessionsEvil.AddRange(cWorlds[0].m_cProfessionsEvil);
                m_cLocations.AddRange(cWorlds[0].m_cLocations);
                m_cItems.AddRange(cWorlds[0].m_cItems);

                for (int i = 1; i < cWorlds.Count; i++ )
                {
                    World pOther = cWorlds[i];

                    StringsHelper.KeepCommon(ref m_cRaces, pOther.m_cRaces);
                    StringsHelper.KeepCommon(ref m_cPerks, pOther.m_cPerks);
                    StringsHelper.KeepCommon(ref m_cProfessions, pOther.m_cProfessions);
                    StringsHelper.KeepCommon(ref m_cProfessionsEvil, pOther.m_cProfessionsEvil);
                    StringsHelper.KeepCommon(ref m_cLocations, pOther.m_cLocations);
                    StringsHelper.KeepCommon(ref m_cItems, pOther.m_cItems);
                }
            }

            foreach (World pOther in cWorlds)
            {
                StringsHelper.RemoveCommon(ref pOther.m_cRaces, m_cRaces);
                StringsHelper.RemoveCommon(ref pOther.m_cPerks, m_cPerks);
                StringsHelper.RemoveCommon(ref pOther.m_cProfessions, m_cProfessions);
                StringsHelper.RemoveCommon(ref pOther.m_cProfessionsEvil, m_cProfessionsEvil);
                StringsHelper.RemoveCommon(ref pOther.m_cLocations, m_cLocations);
                StringsHelper.RemoveCommon(ref pOther.m_cItems, m_cItems);
            }
        }

        public World(UniLibXML pXml, XmlNode pWorldNode)
        {
            pXml.GetStringAttribute(pWorldNode, "name", ref m_sName);

            foreach (XmlNode pSubNode in pWorldNode.ChildNodes)
            {
                if (pSubNode.Name == "Races")
                    StringsHelper.ReadXML(pXml, pSubNode, ref m_cRaces);
                if (pSubNode.Name == "Perks")
                    StringsHelper.ReadXML(pXml, pSubNode, ref m_cPerks);
                if (pSubNode.Name == "Professions")
                    StringsHelper.ReadXML(pXml, pSubNode, ref m_cProfessions);
                if (pSubNode.Name == "ProfessionsEvil")
                    StringsHelper.ReadXML(pXml, pSubNode, ref m_cProfessionsEvil);
                if (pSubNode.Name == "Locations")
                    StringsHelper.ReadXML(pXml, pSubNode, ref m_cLocations);
                if (pSubNode.Name == "Artefacts")
                    StringsHelper.ReadXML(pXml, pSubNode, ref m_cItems);
            }
        }

        public World(World pBase1, World pBase2)
        {
            m_pCommon = pBase1.m_pCommon;

            if (pBase1 == pBase2)
            {
                m_sName = pBase1.m_sName;
                m_cRaces.AddRange(pBase1.m_cRaces);
                m_cPerks.AddRange(pBase1.m_cPerks);
                m_cProfessions.AddRange(pBase1.m_cProfessions);
                m_cProfessionsEvil.AddRange(pBase1.m_cProfessionsEvil);
                m_cLocations.AddRange(pBase1.m_cLocations);
                m_cItems.AddRange(pBase1.m_cItems);
            }
            else
            {
                List<string> cBase1 = new List<string>(pBase1.m_sName.Split(new string[] {" / "},  StringSplitOptions.RemoveEmptyEntries));
                List<string> cBase2 = new List<string>(pBase2.m_sName.Split(new string[] {" / "},  StringSplitOptions.RemoveEmptyEntries));
                List<string> cName = new List<string>();

                StringsHelper.Merge(cBase1, cBase2, ref cName);

                m_sName = string.Join(" / ", cName.ToArray());

                StringsHelper.Merge(pBase1.m_cRaces, pBase2.m_cRaces, ref m_cRaces);
                StringsHelper.Merge(pBase1.m_cPerks, pBase2.m_cPerks, ref m_cPerks);
                StringsHelper.Merge(pBase1.m_cProfessions, pBase2.m_cProfessions, ref m_cProfessions);
                StringsHelper.Merge(pBase1.m_cProfessionsEvil, pBase2.m_cProfessionsEvil, ref m_cProfessionsEvil);
                StringsHelper.Merge(pBase1.m_cLocations, pBase2.m_cLocations, ref m_cLocations);
                StringsHelper.Merge(pBase1.m_cItems, pBase2.m_cItems, ref m_cItems);
            }
        }

        internal void WriteXML(UniLibXML pXml, XmlNode pWorldNode)
        {
            pXml.AddAttribute(pWorldNode, "name", m_sName);

            StringsHelper.WriteXML(pXml, pWorldNode, "Races", m_cRaces);
            StringsHelper.WriteXML(pXml, pWorldNode, "Perks", m_cPerks);
            StringsHelper.WriteXML(pXml, pWorldNode, "Professions", m_cProfessions);
            StringsHelper.WriteXML(pXml, pWorldNode, "ProfessionsEvil", m_cProfessionsEvil);
            StringsHelper.WriteXML(pXml, pWorldNode, "Locations", m_cLocations);
            StringsHelper.WriteXML(pXml, pWorldNode, "Artefacts", m_cItems);
        }

        public override string ToString()
        {
            if (m_pCommon == null)
                return string.Format("[{0}]", m_sName);
            else
                return m_sName;
        }

        public string GetRandomProfession()
        {
            List<string> cMerged = m_cProfessions;
            if (m_pCommon != null)
            {
                cMerged = new List<string>();
                StringsHelper.Merge(m_cProfessions, m_pCommon.m_cProfessions, ref cMerged);
            }
            return StringsHelper.GetRandom(cMerged, "бездельник");
        }

        public string GetRandomProfessionEvil()
        {
            List<string> cMerged = m_cProfessionsEvil;
            if (m_pCommon != null)
            {
                cMerged = new List<string>();
                StringsHelper.Merge(m_cProfessionsEvil, m_pCommon.m_cProfessionsEvil, ref cMerged);
            }
            return StringsHelper.GetRandom(cMerged, "злодей");
        }

        public string GetRandomRace()
        {
            List<string> cMerged = m_cRaces;
            if (m_pCommon != null)
            {
                cMerged = new List<string>();
                StringsHelper.Merge(m_cRaces, m_pCommon.m_cRaces, ref cMerged);
            }
            return StringsHelper.GetRandom(cMerged, "человек");
        }

        public string GetRandomRace(string sRelative)
        {
            List<string> cMerged = m_cRaces;
            if (m_pCommon != null)
            {
                cMerged = new List<string>();
                StringsHelper.Merge(m_cRaces, m_pCommon.m_cRaces, ref cMerged);
            }
            return StringsHelper.GetRelative(cMerged, sRelative);
        }

        public string GetRandomLocation(List<string> cExceptions)
        {
            List<string> cMerged = m_cLocations;
            if (m_pCommon != null)
            {
                cMerged = new List<string>();
                StringsHelper.Merge(m_cLocations, m_pCommon.m_cLocations, ref cMerged);
            }
            return StringsHelper.GetRandom(cMerged, cExceptions);
        }

        public string GetRandomPerk(List<string> cExceptions)
        {
            List<string> cMerged = m_cPerks;
            if (m_pCommon != null)
            {
                cMerged = new List<string>();
                StringsHelper.Merge(m_cPerks, m_pCommon.m_cPerks, ref cMerged);
            }
            return StringsHelper.GetRandom(cMerged, cExceptions);
        }

        public string GetRandomArtefact()
        {
            List<string> cMerged = m_cItems;
            if (m_pCommon != null)
            {
                cMerged = new List<string>();
                StringsHelper.Merge(m_cItems, m_pCommon.m_cItems, ref cMerged);
            }
            return StringsHelper.GetRandom(cMerged, "штуковина");
        }
    }
}
