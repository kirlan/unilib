using System;
using System.Collections.Generic;
using System.Text;
using nsUniLibXML;
using System.Xml;
using Random;

namespace RandomStory
{
    public class Setting
    {
        public Setting m_pCommon = null;

        public string m_sName;

        public Strings m_cRaces = new Strings();

        public Strings m_cPerks = new Strings();

        public Strings m_cProfessions = new Strings();

        public Strings m_cProfessionsElite = new Strings();

        public Strings m_cLocations = new Strings();

        public Strings m_cGeography = new Strings();

        public Strings m_cItems = new Strings();

        public Strings m_cEvents = new Strings();

        public Setting()
        {
            m_sName = "Новый сеттинг";
        }

        /// <summary>
        /// Создаёт сеттинг, хранящий в себе все строки, общие для перечисленных сеттингов.
        /// Из перечисленных сеттингов общие строки при этом удаляются
        /// </summary>
        /// <param name="cWorlds"></param>
        public Setting(List<Setting> cSettings)
        {
            m_sName = "Общие параметры";

            if (cSettings.Count > 0)
            {
                m_cRaces = new Strings(cSettings[0].m_cRaces);
                m_cPerks = new Strings(cSettings[0].m_cPerks);
                m_cProfessions = new Strings(cSettings[0].m_cProfessions);
                m_cProfessionsElite = new Strings(cSettings[0].m_cProfessionsElite);
                m_cLocations = new Strings(cSettings[0].m_cLocations);
                m_cGeography = new Strings(cSettings[0].m_cGeography);
                m_cItems = new Strings(cSettings[0].m_cItems);
                m_cEvents = new Strings(cSettings[0].m_cEvents);

                for (int i = 1; i < cSettings.Count; i++)
                {
                    Setting pOther = cSettings[i];

                    m_cRaces.KeepCommon(pOther.m_cRaces);
                    m_cPerks.KeepCommon(pOther.m_cPerks);
                    m_cProfessions.KeepCommon(pOther.m_cProfessions);
                    m_cProfessionsElite.KeepCommon(pOther.m_cProfessionsElite);
                    m_cLocations.KeepCommon(pOther.m_cLocations);
                    m_cGeography.KeepCommon(pOther.m_cGeography);
                    m_cItems.KeepCommon(pOther.m_cItems);
                    m_cEvents.KeepCommon(pOther.m_cEvents);
                }
            }

            foreach (Setting pOther in cSettings)
            {
                pOther.m_cRaces.RemoveCommon(m_cRaces);
                pOther.m_cPerks.RemoveCommon(m_cPerks);
                pOther.m_cProfessions.RemoveCommon(m_cProfessions);
                pOther.m_cProfessionsElite.RemoveCommon(m_cProfessionsElite);
                pOther.m_cLocations.RemoveCommon(m_cLocations);
                pOther.m_cGeography.RemoveCommon(m_cGeography);
                pOther.m_cItems.RemoveCommon(m_cItems);
                pOther.m_cEvents.RemoveCommon(m_cEvents);
            }
        }

        /// <summary>
        /// Считывает сеттинг из XML
        /// </summary>
        /// <param name="pXml"></param>
        /// <param name="pWorldNode"></param>
        public Setting(UniLibXML pXml, XmlNode pWorldNode)
        {
            pXml.GetStringAttribute(pWorldNode, "name", ref m_sName);

            foreach (XmlNode pSubNode in pWorldNode.ChildNodes)
            {
                if (pSubNode.Name == "Races")
                    m_cRaces = new Strings(pXml, pSubNode);
                if (pSubNode.Name == "Perks")
                    m_cPerks = new Strings(pXml, pSubNode);
                if (pSubNode.Name == "Professions")
                    m_cProfessions = new Strings(pXml, pSubNode);
                if (pSubNode.Name == "ProfessionsEvil")
                    m_cProfessionsElite = new Strings(pXml, pSubNode);
                if (pSubNode.Name == "ProfessionsElite")
                    m_cProfessionsElite = new Strings(pXml, pSubNode);
                if (pSubNode.Name == "Locations")
                    m_cLocations = new Strings(pXml, pSubNode);
                if (pSubNode.Name == "Geography")
                    m_cGeography = new Strings(pXml, pSubNode);
                if (pSubNode.Name == "Artefacts")
                    m_cItems = new Strings(pXml, pSubNode);
                if (pSubNode.Name == "Events")
                    m_cEvents = new Strings(pXml, pSubNode);
            }
        }

        public static string GetName(Setting pBase1, Setting pBase2)
        {
            Strings cBase1 = new Strings(pBase1.m_sName.Split(new string[] { " / " }, StringSplitOptions.RemoveEmptyEntries));
            Strings cBase2 = new Strings(pBase2.m_sName.Split(new string[] { " / " }, StringSplitOptions.RemoveEmptyEntries));
            Strings cName = new Strings(cBase1, cBase2);

            return cName.ToString(" / ");
        }

        /// <summary>
        /// Создаёт новый смешанный сеттинг, объединяющий все строки из двух образующих сеттингов.
        /// </summary>
        /// <param name="pBase1"></param>
        /// <param name="pBase2"></param>
        public Setting(Setting pBase1, Setting pBase2)
        {
            m_pCommon = pBase1.m_pCommon;

            if (pBase1.Equals(pBase2))
            {
                m_sName = pBase1.m_sName;
                m_cRaces = new Strings(pBase1.m_cRaces);
                m_cPerks = new Strings(pBase1.m_cPerks);
                m_cProfessions = new Strings(pBase1.m_cProfessions);
                m_cProfessionsElite = new Strings(pBase1.m_cProfessionsElite);
                m_cLocations = new Strings(pBase1.m_cLocations);
                m_cGeography = new Strings(pBase1.m_cGeography);
                m_cItems = new Strings(pBase1.m_cItems);
                m_cEvents = new Strings(pBase1.m_cEvents);
            }
            else
            {
                Strings cBase1 = new Strings(pBase1.m_sName.Split(new string[] { " / " }, StringSplitOptions.RemoveEmptyEntries));
                Strings cBase2 = new Strings(pBase2.m_sName.Split(new string[] { " / " }, StringSplitOptions.RemoveEmptyEntries));
                Strings cName = new Strings(cBase1, cBase2);

                m_sName = cName.ToString(" / ");

                m_cRaces = new Strings(pBase1.m_cRaces, pBase2.m_cRaces);
                m_cPerks = new Strings(pBase1.m_cPerks, pBase2.m_cPerks);
                m_cProfessions = new Strings(pBase1.m_cProfessions, pBase2.m_cProfessions);
                m_cProfessionsElite = new Strings(pBase1.m_cProfessionsElite, pBase2.m_cProfessionsElite);
                m_cLocations = new Strings(pBase1.m_cLocations, pBase2.m_cLocations);
                m_cGeography = new Strings(pBase1.m_cGeography, pBase2.m_cGeography);
                m_cItems = new Strings(pBase1.m_cItems, pBase2.m_cItems);
                m_cEvents = new Strings(pBase1.m_cEvents, pBase2.m_cEvents);
            }
        }

        internal void WriteXML(UniLibXML pXml, XmlNode pWorldNode)
        {
            pXml.AddAttribute(pWorldNode, "name", m_sName);

            m_cRaces.WriteXML(pXml, pWorldNode, "Races");
            m_cPerks.WriteXML(pXml, pWorldNode, "Perks");
            m_cProfessions.WriteXML(pXml, pWorldNode, "Professions");
            m_cProfessionsElite.WriteXML(pXml, pWorldNode, "ProfessionsElite");
            m_cGeography.WriteXML(pXml, pWorldNode, "Geography");
            m_cLocations.WriteXML(pXml, pWorldNode, "Locations");
            m_cItems.WriteXML(pXml, pWorldNode, "Artefacts");
            m_cEvents.WriteXML(pXml, pWorldNode, "Events");
        }

        public override string ToString()
        {
            if (m_pCommon == null)
                return string.Format("[{0}]", m_sName);
            else
                return m_sName;
        }

        public bool Equals(Setting pOther)
        {
            Strings cName1 = new Strings(m_sName.Split(new string[] { " / " }, StringSplitOptions.RemoveEmptyEntries));
            Strings cName2 = new Strings(pOther.m_sName.Split(new string[] { " / " }, StringSplitOptions.RemoveEmptyEntries));

            return cName1.Equals(cName2);
        }

        public bool Equals(string sOtherName)
        {
            Strings cName1 = new Strings(m_sName.Split(new string[] { " / " }, StringSplitOptions.RemoveEmptyEntries));
            Strings cName2 = new Strings(sOtherName.Split(new string[] { " / " }, StringSplitOptions.RemoveEmptyEntries));

            return cName1.Equals(cName2);
        }

        public string GetRandomProfession(ref char[] aFlags)
        {
            Strings cMerged = m_cProfessions;
            if (m_pCommon != null)
            {
                cMerged = new Strings(m_cProfessions, m_pCommon.m_cProfessions);
            }
            return cMerged.GetRandom("бездельник", ref aFlags);
        }

        public string GetRandomProfessionElite(ref char[] aFlags)
        {
            Strings cMerged = m_cProfessionsElite;
            if (m_pCommon != null)
            {
                cMerged = new Strings(m_cProfessionsElite, m_pCommon.m_cProfessionsElite);
            }
            return cMerged.GetRandom("злодей", ref aFlags);
        }

        public string GetRandomRace(ref char[] aFlags)
        {
            Strings cMerged = m_cRaces;
            if (m_pCommon != null)
            {
                cMerged = new Strings(m_cRaces, m_pCommon.m_cRaces);
            }
            return cMerged.GetRandom("мужчина", ref aFlags);
        }

        public string GetRandomRace(string sRelative, ref char[] aFlags)
        {
            Strings cMerged = m_cRaces;
            if (m_pCommon != null)
            {
                cMerged = new Strings(m_cRaces, m_pCommon.m_cRaces);
            }
            return cMerged.GetRelative(sRelative, ref aFlags);
        }

        public string GetRandomLocation(Strings cExceptions)
        {
            Strings cMerged = m_cLocations;
            if (m_pCommon != null)
            {
                cMerged = new Strings(m_cLocations, m_pCommon.m_cLocations);
            }
            char[] aFlags = null;
            return cMerged.GetRandom(cExceptions, ref aFlags);
        }

        public string GetRandomGeography(Strings cExceptions)
        {
            Strings cMerged = m_cGeography;
            if (m_pCommon != null)
            {
                cMerged = new Strings(m_cGeography, m_pCommon.m_cGeography);
            }
            char[] aFlags = null;
            return cMerged.GetRandom(cExceptions, ref aFlags);
        }

        private Strings m_cMergedPerks = null;

        public string GetRandomPerk(Strings cExceptions, ref char[] aFlags)
        {
            if (m_cMergedPerks == null)
                //m_cMergedPerks = new Strings(m_cPerks, m_pCommon.m_cPerks);
                m_cMergedPerks = new Strings(m_pCommon.m_cPerks, m_cPerks);
            
            Strings cMerged = m_cPerks;
            if (m_pCommon != null && Rnd.ChooseOne((int)Math.Sqrt(m_pCommon.m_cPerks.Count), (int)Math.Sqrt(m_cPerks.Count)))
            {
                cMerged = m_cMergedPerks;
            }
            return cMerged.GetRandom(cExceptions, ref aFlags);
        }

        public string GetRandomArtefact(Strings cExceptions)
        {
            Strings cMerged = m_cItems;
            if (m_pCommon != null)
            {
                cMerged = new Strings(m_cItems, m_pCommon.m_cItems);
            }
            char[] aFlags = null;
            return cMerged.GetRandom(cExceptions, "штуковина", ref aFlags);
        }

        public string GetRandomEvent(Strings cExceptions)
        {
            Strings cMerged = m_cEvents;
            if (m_pCommon != null)
            {
                cMerged = new Strings(m_cEvents, m_pCommon.m_cEvents);
            }
            char[] aFlags = null;
            return cMerged.GetRandom(cExceptions, ref aFlags);
        }
    }
}
