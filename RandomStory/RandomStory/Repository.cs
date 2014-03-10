using System;
using System.Collections.Generic;
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
                key.SetValue("Repository", sPreset);

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

        public List<Setting> m_cWorlds = new List<Setting>();

        public List<Setting> m_cJanres = new List<Setting>();

        public Setting m_pCommon = new Setting();

        public Strings m_cProblems = new Strings();

        public Strings m_cSolutions = new Strings();

        public Strings m_cBloodRelations = new Strings();

        public Strings m_cOtherRelations = new Strings();

        public Strings m_cEvents = new Strings();

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

            XmlNode pWorlds = pXml.CreateNode(pModuleNode, "Worlds");
            foreach (Setting pWorld in m_cWorlds)
            {
                XmlNode pWorldNode = pXml.CreateNode(pWorlds, "World");
                pWorld.WriteXML(pXml, pWorldNode);
            }

            XmlNode pJanres = pXml.CreateNode(pModuleNode, "Janres");
            foreach (Setting pJanre in m_cJanres)
            {
                XmlNode pJanreNode = pXml.CreateNode(pJanres, "Janre");
                pJanre.WriteXML(pXml, pJanreNode);
            }

            m_cBloodRelations.WriteXML(pXml, pModuleNode, "BloodRelations");
            m_cOtherRelations.WriteXML(pXml, pModuleNode, "OtherRelations");

            m_cProblems.WriteXML(pXml, pModuleNode, "Problems");
            m_cSolutions.WriteXML(pXml, pModuleNode, "Solutions");
            m_cEvents.WriteXML(pXml, pModuleNode, "Events");

            pXml.Write(sFilename);
        }

        public void LoadXML()
        {
            if (!LoadXML(m_sPresetFile))
            {
                if (LoadXML(Application.StartupPath + "\\repository.xml"))
                {
                    m_sPresetFile = Application.StartupPath + "\\repository.xml";
                    RegistryKey key = null;
                    try
                    {
                        key = Application.CommonAppDataRegistry;//Registry.LocalMachine.OpenSubKey("SOFTWARE\\RandomStoryKvB", true);

                        key.SetValue("Repository", m_sPresetFile);

                        key.Close();
                    }
                    catch (Exception ex)
                    {
                        if (key != null)
                            key.Close();
                    }
                }
            }
        }

        public bool LoadXML(string sFilename)
        {
            UniLibXML pXml = new UniLibXML("RandomStory");

            if (!pXml.Load(sFilename))
                return false;

            m_cWorlds.Clear();
            m_cJanres.Clear();

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
                                m_cWorlds.Add(pSetting);
                            }
                            if (pSettingNode.Name == "Janre")
                            {
                                Setting pSetting = new Setting(pXml, pSettingNode);
                                m_cJanres.Add(pSetting);
                            }
                        }
                    }

                    if (pSection.Name == "Janres")
                    {
                        foreach (XmlNode pSettingNode in pSection.ChildNodes)
                        {
                            if (pSettingNode.Name == "Janre")
                            {
                                Setting pSetting = new Setting(pXml, pSettingNode);
                                m_cJanres.Add(pSetting);
                            }
                        }
                    }

                    if (pSection.Name == "BloodRelations" || pSection.Name == "Relations")
                        m_cBloodRelations = new Strings(pXml, pSection);

                    if (pSection.Name == "OtherRelations")
                        m_cOtherRelations = new Strings(pXml, pSection);

                    if (pSection.Name == "Problems")
                        m_cProblems = new Strings(pXml, pSection);

                    if (pSection.Name == "Solutions")
                        m_cSolutions = new Strings(pXml, pSection);

                    if (pSection.Name == "Events")
                        m_cEvents = new Strings(pXml, pSection);
                }
            }

            if (!bCommonLoaded)
            {
                List<Setting> pAllSettings = new List<Setting>(m_cWorlds);
                pAllSettings.AddRange(m_cJanres);
                m_pCommon = new Setting(pAllSettings);
                bCommonLoaded = true;
            }

            foreach (Setting pWorld in m_cWorlds)
                pWorld.m_pCommon = m_pCommon;

            foreach (Setting pWorld in m_cJanres)
                pWorld.m_pCommon = m_pCommon;

            return true;
        }

        private SettingsSet m_cPossibleWorlds = new SettingsSet(null, null);

        private SettingsSet m_cPossibleJanres = new SettingsSet(null, null);

        public Setting GetPrimeSetting()
        {
            Setting pWorld = m_cPossibleWorlds.GetPrime();
            if (pWorld == null)
                return null;

            Setting pJanre = m_cPossibleJanres.GetPrime();
            if (pJanre == null)
                return null;

            m_cUsedSettings.Clear();

            Setting pNewSetting = new Setting(pWorld, pJanre);
            m_cUsedSettings.Add(pNewSetting);
            return pNewSetting;
        }

        private List<Setting> m_cUsedSettings = new List<Setting>();

        public Setting GetRandomSetting()
        {
            Setting pWorld = m_cPossibleWorlds.GetRandom(4);
            if (pWorld == null)
                return null;

            Setting pJanre = m_cPossibleJanres.GetRandom(4);
            if (pJanre == null)
                return null;

            string sNewName = Setting.GetName(pWorld, pJanre);

            foreach (Setting pUsedSetting in m_cUsedSettings)
                if (pUsedSetting.Equals(sNewName))
                    return pUsedSetting;

            Setting pNewSetting = new Setting(pWorld, pJanre);
            m_cUsedSettings.Add(pNewSetting);
            return pNewSetting;
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

        public string GetRandomEvent(Strings cExceptions)
        {
            char[] aFlags = null;
            return m_cEvents.GetRandom(cExceptions, ref aFlags);
        }

        public string GetRandomBloodRelation(ref char[] aFlags)
        {
            return m_cBloodRelations.GetRandom("дальний родственник", ref aFlags);
        }

        public string GetRandomOtherRelation(ref char[] aFlags)
        {
            return m_cOtherRelations.GetRandom("случайный знакомый", ref aFlags);
        }

        internal void MarkPossibleSettings(List<Setting> cAllowedWorlds, List<Setting> cPrimedWorlds, List<Setting> cAllowedJanres, List<Setting> cPrimedJanres)
        {
            m_cPossibleWorlds = new SettingsSet(cPrimedWorlds, cAllowedWorlds);
            m_cPossibleJanres = new SettingsSet(cPrimedJanres, cAllowedJanres);

            m_cUsedSettings.Clear();
        }

        public string FixCommon()
        {
            StringBuilder sLog = new StringBuilder();

            List<Setting> pAllSettings = new List<Setting>(m_cWorlds);
            pAllSettings.AddRange(m_cJanres);

            string[] cLog;
            foreach (Setting pSetting in pAllSettings)
            {
                cLog = pSetting.m_cRaces.RemoveCommon(m_pCommon.m_cRaces);
                if(cLog.Length > 0)
                {
                    sLog.AppendLine(string.Format("{0} записей в списке рас '{1}' дублируются в списке '{2}' - удалены:", cLog.Length, pSetting.ToString(), m_pCommon.ToString()));
                    foreach (string sRemoved in cLog)
                        sLog.AppendLine(sRemoved);
                    sLog.AppendLine("");
                }
                cLog = pSetting.m_cPerks.RemoveCommon(m_pCommon.m_cPerks);
                if (cLog.Length > 0)
                {
                    sLog.AppendLine(string.Format("{0} записей в списке особенностей '{1}' дублируются в списке '{2}' - удалены:", cLog.Length, pSetting.ToString(), m_pCommon.ToString()));
                    foreach (string sRemoved in cLog)
                        sLog.AppendLine(sRemoved);
                    sLog.AppendLine("");
                }
                cLog = pSetting.m_cProfessions.RemoveCommon(m_pCommon.m_cProfessions);
                if (cLog.Length > 0)
                {
                    sLog.AppendLine(string.Format("{0} записей в списке профессий героя '{1}' дублируются в списке '{2}' - удалены:", cLog.Length, pSetting.ToString(), m_pCommon.ToString()));
                    foreach (string sRemoved in cLog)
                        sLog.AppendLine(sRemoved);
                    sLog.AppendLine("");
                }
                cLog = pSetting.m_cProfessionsElite.RemoveCommon(m_pCommon.m_cProfessionsElite);
                if (cLog.Length > 0)
                {
                    sLog.AppendLine(string.Format("{0} записей в списке профессий злодея '{1}' дублируются в списке '{2}' - удалены:", cLog.Length, pSetting.ToString(), m_pCommon.ToString()));
                    foreach (string sRemoved in cLog)
                        sLog.AppendLine(sRemoved);
                    sLog.AppendLine("");
                }
                cLog = pSetting.m_cLocations.RemoveCommon(m_pCommon.m_cLocations);
                if (cLog.Length > 0)
                {
                    sLog.AppendLine(string.Format("{0} записей в списке мест '{1}' дублируются в списке '{2}' - удалены:", cLog.Length, pSetting.ToString(), m_pCommon.ToString()));
                    foreach (string sRemoved in cLog)
                        sLog.AppendLine(sRemoved);
                    sLog.AppendLine("");
                }
                cLog = pSetting.m_cItems.RemoveCommon(m_pCommon.m_cItems);
                if (cLog.Length > 0)
                {
                    sLog.AppendLine(string.Format("{0} записей в списке предметов '{1}' дублируются в списке '{2}' - удалены:", cLog.Length, pSetting.ToString(), m_pCommon.ToString()));
                    foreach (string sRemoved in cLog)
                        sLog.AppendLine(sRemoved);
                    sLog.AppendLine("");
                }
            }

            Setting pNewCommon = new Setting(pAllSettings);

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
