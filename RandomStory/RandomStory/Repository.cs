using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nsUniLibXML;
using System.Xml;
using Microsoft.Win32;
using System.Windows.Forms;
using Random;

namespace RandomStory
{
    public class Repository
    {
        private static string m_sPresetFile = Preload();

        public static string Preload()
        {
            RegistryKey key = null;
            try
            {
                key = Application.CommonAppDataRegistry;//Registry.LocalMachine.OpenSubKey("SOFTWARE\\RandomStoryKvB", true);

                string sPreset = (string)key.GetValue("Repository", Application.CommonAppDataPath + "\\repository.xml");

                key.Close();

                return sPreset;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                if (key != null)
                    key.Close();
                return Application.CommonAppDataPath + "\\repository.xml"; 
            }
        }

        public List<Setting> m_cAllSettings = new List<Setting>();

        public Setting m_pCommon = new Setting();

        public Strings m_cProblems = new Strings();

        public Strings m_cSolutions = new Strings();

        public Strings m_cRelations = new Strings();

        public Repository()
        {
            m_pCommon.m_sName = "Общие параметры";
        }

        public void SaveXML()
        {
            SaveXML(m_sPresetFile);
        }

        public void SaveXML(string sFilename)
        {
            UniLibXML pXml = new UniLibXML("RandomStory");

            XmlNode pModuleNode = pXml.CreateNode(pXml.Root, "Module");

            XmlNode pCommonNode = pXml.CreateNode(pModuleNode, "Common");
            m_pCommon.WriteXML(pXml, pCommonNode);

            XmlNode pSettings = pXml.CreateNode(pModuleNode, "Settings");
            foreach (Setting pSetting in m_cAllSettings)
            {
                XmlNode pSettingNode = pXml.CreateNode(pSettings, "Setting");
                pSetting.WriteXML(pXml, pSettingNode);
            }

            m_cRelations.WriteXML(pXml, pModuleNode, "Relations");

            m_cProblems.WriteXML(pXml, pModuleNode, "Problems");
            m_cSolutions.WriteXML(pXml, pModuleNode, "Solutions");

            pXml.Write(sFilename);
        }

        public void LoadXML()
        {
            LoadXML(m_sPresetFile);
        }

        public void LoadXML(string sFilename)
        {
            UniLibXML pXml = new UniLibXML("RandomStory");

            if (!pXml.Load(sFilename))
                return;

            m_cAllSettings.Clear();

            bool bCommonLoaded = false;

            if (pXml.Root.ChildNodes.Count == 1 && pXml.Root.ChildNodes[0].Name == "Module")
            {
                XmlNode pModuleNode = pXml.Root.ChildNodes[0];

                foreach (XmlNode pSection in pModuleNode.ChildNodes)
                {
                    if (pSection.Name == "Common")
                    {
                        m_pCommon = new Setting(pXml, pSection);
                        m_pCommon.m_sName = "Общие параметры";
                        bCommonLoaded = true;
                    }

                    if (pSection.Name == "Worlds" || pSection.Name == "Settings")
                    {
                        foreach (XmlNode pSettingNode in pSection.ChildNodes)
                        {
                            if (pSettingNode.Name == "World" || pSettingNode.Name == "Setting")
                            {
                                Setting pSetting = new Setting(pXml, pSettingNode);
                                m_cAllSettings.Add(pSetting);
                            }
                        }
                    }

                    if (pSection.Name == "Relations")
                        m_cRelations = new Strings(pXml, pSection);

                    if (pSection.Name == "Problems")
                        m_cProblems = new Strings(pXml, pSection);

                    if (pSection.Name == "Solutions")
                        m_cSolutions = new Strings(pXml, pSection);
                }
            }

            if (!bCommonLoaded)
            {
                m_pCommon = new Setting(m_cAllSettings);
                bCommonLoaded = true;
            }

            foreach (Setting pWorld in m_cAllSettings)
                pWorld.m_pCommon = m_pCommon;
        }

        private List<Setting> m_cAllowedSettings = null;
        private List<Setting> m_cPrimeSettings = null;

        public Setting GetRandomSetting(int iCrossProbability, bool bPrime)
        {
            List<Setting> cSettings = bPrime ? m_cPrimeSettings : m_cAllowedSettings;

            if (cSettings == null || cSettings.Count == 0)
            {
                if (!bPrime && m_cPrimeSettings != null && m_cPrimeSettings.Count > 0)
                    cSettings = m_cPrimeSettings;
                else
                    cSettings = m_cAllSettings;
            }

            bool bPrime2 = bPrime || Rnd.OneChanceFrom(2);

            if (Rnd.OneChanceFrom(iCrossProbability) && cSettings.Count > 1 && (bPrime2 ? m_cPrimeSettings : m_cAllowedSettings).Count > 1)
            {
                if (iCrossProbability < 2)
                    iCrossProbability = 2;

                Setting pSetting1 = GetRandomSetting(iCrossProbability * iCrossProbability, bPrime);
                Setting pSetting2 = GetRandomSetting(iCrossProbability * iCrossProbability, bPrime2);
                while (pSetting1.Equals(pSetting2))
                {
                    pSetting2 = GetRandomSetting(iCrossProbability * iCrossProbability, bPrime2);
                }

                Setting pNewSetting = new Setting(pSetting1, pSetting2);
                if(bPrime)
                    m_cPrimeSettings.Add(pNewSetting);
                else
                    m_cAllowedSettings.Add(pNewSetting);
                return pNewSetting;
            }
            else
            {
                if (cSettings.Count > 0)
                    return cSettings[Rnd.Get(cSettings.Count)];
            }
            return null;
        }

        public string GetRandomProblem()
        {
            char[] aFlags = null;
            return m_cProblems.GetRandom("просто беда", ref aFlags);
        }

        public string GetRandomSolution()
        {
            char[] aFlags = null;
            return m_cSolutions.GetRandom("как-то само рассосалось", ref aFlags);
        }

        public string GetRandomRelation(ref char[] aFlags)
        {
            return m_cRelations.GetRandom("случайный знакомый", ref aFlags);
        }

        internal void MarkPossibleWorlds(List<Setting> cAllowed, List<Setting> cPrimed)
        {
            m_cAllowedSettings = cAllowed;
            m_cPrimeSettings = cPrimed;
        }

        public string FixCommon()
        {
            StringBuilder sLog = new StringBuilder();

            string[] cLog;
            foreach (Setting pWorld in m_cAllSettings)
            {
                cLog = pWorld.m_cRaces.RemoveCommon(m_pCommon.m_cRaces);
                if(cLog.Length > 0)
                {
                    sLog.AppendLine(string.Format("{0} записей в списке рас '{1}' дублируются в списке '{2}' - удалены:", cLog.Length, pWorld.ToString(), m_pCommon.ToString()));
                    foreach (string sRemoved in cLog)
                        sLog.AppendLine(sRemoved);
                    sLog.AppendLine("");
                }
                cLog = pWorld.m_cPerks.RemoveCommon(m_pCommon.m_cPerks);
                if (cLog.Length > 0)
                {
                    sLog.AppendLine(string.Format("{0} записей в списке особенностей '{1}' дублируются в списке '{2}' - удалены:", cLog.Length, pWorld.ToString(), m_pCommon.ToString()));
                    foreach (string sRemoved in cLog)
                        sLog.AppendLine(sRemoved);
                    sLog.AppendLine("");
                }
                cLog = pWorld.m_cProfessions.RemoveCommon(m_pCommon.m_cProfessions);
                if (cLog.Length > 0)
                {
                    sLog.AppendLine(string.Format("{0} записей в списке профессий героя '{1}' дублируются в списке '{2}' - удалены:", cLog.Length, pWorld.ToString(), m_pCommon.ToString()));
                    foreach (string sRemoved in cLog)
                        sLog.AppendLine(sRemoved);
                    sLog.AppendLine("");
                }
                cLog = pWorld.m_cProfessionsElite.RemoveCommon(m_pCommon.m_cProfessionsElite);
                if (cLog.Length > 0)
                {
                    sLog.AppendLine(string.Format("{0} записей в списке профессий злодея '{1}' дублируются в списке '{2}' - удалены:", cLog.Length, pWorld.ToString(), m_pCommon.ToString()));
                    foreach (string sRemoved in cLog)
                        sLog.AppendLine(sRemoved);
                    sLog.AppendLine("");
                }
                cLog = pWorld.m_cLocations.RemoveCommon(m_pCommon.m_cLocations);
                if (cLog.Length > 0)
                {
                    sLog.AppendLine(string.Format("{0} записей в списке мест '{1}' дублируются в списке '{2}' - удалены:", cLog.Length, pWorld.ToString(), m_pCommon.ToString()));
                    foreach (string sRemoved in cLog)
                        sLog.AppendLine(sRemoved);
                    sLog.AppendLine("");
                }
                cLog = pWorld.m_cItems.RemoveCommon(m_pCommon.m_cItems);
                if (cLog.Length > 0)
                {
                    sLog.AppendLine(string.Format("{0} записей в списке предметов '{1}' дублируются в списке '{2}' - удалены:", cLog.Length, pWorld.ToString(), m_pCommon.ToString()));
                    foreach (string sRemoved in cLog)
                        sLog.AppendLine(sRemoved);
                    sLog.AppendLine("");
                }
            }

            Setting pNewCommon = new Setting(m_cAllSettings);

            cLog = m_pCommon.m_cRaces.Merge(pNewCommon.m_cRaces);
            if (cLog.Length > 0)
            {
                sLog.AppendLine(string.Format("{0} записей перенесены в список рас '{1}':", cLog.Length, m_pCommon.ToString()));
                foreach (string sAdded in cLog)
                    sLog.AppendLine(sAdded);
                sLog.AppendLine("");
            }
            cLog = m_pCommon.m_cPerks.Merge(pNewCommon.m_cPerks);
            if (cLog.Length > 0)
            {
                sLog.AppendLine(string.Format("{0} записей перенесены в список особенностей '{1}':", cLog.Length, m_pCommon.ToString()));
                foreach (string sAdded in cLog)
                    sLog.AppendLine(sAdded);
                sLog.AppendLine("");
            }
            cLog = m_pCommon.m_cProfessions.Merge(pNewCommon.m_cProfessions);
            if (cLog.Length > 0)
            {
                sLog.AppendLine(string.Format("{0} записей перенесены в список профессий героя '{1}':", cLog.Length, m_pCommon.ToString()));
                foreach (string sAdded in cLog)
                    sLog.AppendLine(sAdded);
                sLog.AppendLine("");
            }
            cLog = m_pCommon.m_cProfessionsElite.Merge(pNewCommon.m_cProfessionsElite);
            if (cLog.Length > 0)
            {
                sLog.AppendLine(string.Format("{0} записей перенесены в список профессий злодея '{1}':", cLog.Length, m_pCommon.ToString()));
                foreach (string sAdded in cLog)
                    sLog.AppendLine(sAdded);
                sLog.AppendLine("");
            }
            cLog = m_pCommon.m_cLocations.Merge(pNewCommon.m_cLocations);
            if (cLog.Length > 0)
            {
                sLog.AppendLine(string.Format("{0} записей перенесены в список мест '{1}':", cLog.Length, m_pCommon.ToString()));
                foreach (string sAdded in cLog)
                    sLog.AppendLine(sAdded);
                sLog.AppendLine("");
            }
            cLog = m_pCommon.m_cItems.Merge(pNewCommon.m_cItems);
            if (cLog.Length > 0)
            {
                sLog.AppendLine(string.Format("{0} записей перенесены в список предметов '{1}':", cLog.Length, m_pCommon.ToString()));
                foreach (string sAdded in cLog)
                    sLog.AppendLine(sAdded);
                sLog.AppendLine("");
            }

            return sLog.ToString();
        }
    }
}
