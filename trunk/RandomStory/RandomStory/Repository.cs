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

        public List<World> m_cWorlds = new List<World>();

        public World m_pCommon = new World();

        public List<string> m_cProblems = new List<string>();

        public List<string> m_cSolutions = new List<string>();

        public List<string> m_cRelations = new List<string>();

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
            foreach (World pWorld in m_cWorlds)
            {
                XmlNode pWorldNode = pXml.CreateNode(pWorlds, "World");
                pWorld.WriteXML(pXml, pWorldNode);
            }

            StringsHelper.WriteXML(pXml, pModuleNode, "Relations", m_cRelations);

            StringsHelper.WriteXML(pXml, pModuleNode, "Problems", m_cProblems);
            StringsHelper.WriteXML(pXml, pModuleNode, "Solutions", m_cSolutions);

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

            m_cWorlds.Clear();

            bool bCommonLoaded = false;

            if (pXml.Root.ChildNodes.Count == 1 && pXml.Root.ChildNodes[0].Name == "Module")
            {
                XmlNode pModuleNode = pXml.Root.ChildNodes[0];

                foreach (XmlNode pSection in pModuleNode.ChildNodes)
                {
                    if (pSection.Name == "Common")
                    {
                        m_pCommon = new World(pXml, pSection);
                        m_pCommon.m_sName = "Общие параметры";
                        bCommonLoaded = true;
                    }

                    if (pSection.Name == "Worlds")
                    {
                        foreach (XmlNode pWorldNode in pSection.ChildNodes)
                        {
                            if (pWorldNode.Name == "World")
                            {
                                World pWorld = new World(pXml, pWorldNode);
                                m_cWorlds.Add(pWorld);
                            }
                        }
                    }

                    if (pSection.Name == "Relations")
                        StringsHelper.ReadXML(pXml, pSection, ref m_cRelations);

                    if (pSection.Name == "Problems")
                        StringsHelper.ReadXML(pXml, pSection, ref m_cProblems);

                    if (pSection.Name == "Solutions")
                        StringsHelper.ReadXML(pXml, pSection, ref m_cSolutions);
                }
            }

            if (!bCommonLoaded)
            {
                m_pCommon = new World(m_cWorlds);
                bCommonLoaded = true;
            }

            foreach (World pWorld in m_cWorlds)
                pWorld.m_pCommon = m_pCommon;
        }

        private List<World> m_cAllowedWorlds = null;

        public World GetRandomWorld(int iCrossProbability)
        {
            if (iCrossProbability < 2)
                iCrossProbability = 2;

            List<World> cWorlds = m_cAllowedWorlds;

            if (cWorlds == null || cWorlds.Count == 0)
                cWorlds = m_cWorlds;

            if (Rnd.OneChanceFrom(iCrossProbability) && cWorlds.Count > 1)
            {
                //World pWorld1 = GetRandomWorld(false);
                //World pWorld2 = GetRandomWorld(false);
                World pWorld1 = GetRandomWorld(iCrossProbability * iCrossProbability);
                World pWorld2 = GetRandomWorld(iCrossProbability * iCrossProbability);
                while (pWorld1.m_sName == pWorld2.m_sName)
                {
                    pWorld2 = GetRandomWorld(iCrossProbability * iCrossProbability);
                }

                World pNewWorld = new World(pWorld1, pWorld2);
                m_cAllowedWorlds.Add(pNewWorld);
                return pNewWorld;
            }
            else
            {
                if (cWorlds.Count > 0)
                    return cWorlds[Rnd.Get(cWorlds.Count)];
            }
            return null;
        }

        public string GetRandomProblem()
        {
            char[] aFlags = null;
            return StringsHelper.GetRandom(m_cProblems, "просто беда", ref aFlags);
        }

        public string GetRandomSolution()
        {
            char[] aFlags = null;
            return StringsHelper.GetRandom(m_cSolutions, "как-то само рассосалось", ref aFlags);
        }

        public string GetRandomRelation(ref char[] aFlags)
        {
            return StringsHelper.GetRandom(m_cRelations, "случайный знакомый", ref aFlags);
        }

        internal void MarkPossibleWorlds(List<World> cAllowed)
        {
            m_cAllowedWorlds = cAllowed;
        }

        public string FixCommon()
        {
            StringBuilder sLog = new StringBuilder();

            List<string> cLog = new List<string>();
            foreach (World pWorld in m_cWorlds)
            {
                cLog = StringsHelper.RemoveCommon(ref pWorld.m_cRaces, m_pCommon.m_cRaces);
                if(cLog.Count > 0)
                {
                    sLog.AppendLine(string.Format("{0} записей в списке рас '{1}' дублируются в списке '{2}' - удалены:", cLog.Count, pWorld.ToString(), m_pCommon.ToString()));
                    foreach (string sRemoved in cLog)
                        sLog.AppendLine(sRemoved);
                    sLog.AppendLine("");
                }
                cLog = StringsHelper.RemoveCommon(ref pWorld.m_cPerks, m_pCommon.m_cPerks);
                if (cLog.Count > 0)
                {
                    sLog.AppendLine(string.Format("{0} записей в списке особенностей '{1}' дублируются в списке '{2}' - удалены:", cLog.Count, pWorld.ToString(), m_pCommon.ToString()));
                    foreach (string sRemoved in cLog)
                        sLog.AppendLine(sRemoved);
                    sLog.AppendLine("");
                }
                cLog = StringsHelper.RemoveCommon(ref pWorld.m_cProfessions, m_pCommon.m_cProfessions);
                if (cLog.Count > 0)
                {
                    sLog.AppendLine(string.Format("{0} записей в списке профессий героя '{1}' дублируются в списке '{2}' - удалены:", cLog.Count, pWorld.ToString(), m_pCommon.ToString()));
                    foreach (string sRemoved in cLog)
                        sLog.AppendLine(sRemoved);
                    sLog.AppendLine("");
                }
                cLog = StringsHelper.RemoveCommon(ref pWorld.m_cProfessionsElite, m_pCommon.m_cProfessionsElite);
                if (cLog.Count > 0)
                {
                    sLog.AppendLine(string.Format("{0} записей в списке профессий злодея '{1}' дублируются в списке '{2}' - удалены:", cLog.Count, pWorld.ToString(), m_pCommon.ToString()));
                    foreach (string sRemoved in cLog)
                        sLog.AppendLine(sRemoved);
                    sLog.AppendLine("");
                }
                cLog = StringsHelper.RemoveCommon(ref pWorld.m_cLocations, m_pCommon.m_cLocations);
                if (cLog.Count > 0)
                {
                    sLog.AppendLine(string.Format("{0} записей в списке мест '{1}' дублируются в списке '{2}' - удалены:", cLog.Count, pWorld.ToString(), m_pCommon.ToString()));
                    foreach (string sRemoved in cLog)
                        sLog.AppendLine(sRemoved);
                    sLog.AppendLine("");
                }
                cLog = StringsHelper.RemoveCommon(ref pWorld.m_cItems, m_pCommon.m_cItems);
                if (cLog.Count > 0)
                {
                    sLog.AppendLine(string.Format("{0} записей в списке предметов '{1}' дублируются в списке '{2}' - удалены:", cLog.Count, pWorld.ToString(), m_pCommon.ToString()));
                    foreach (string sRemoved in cLog)
                        sLog.AppendLine(sRemoved);
                    sLog.AppendLine("");
                }
            }

            World pNewCommon = new World(m_cWorlds);

            if (pNewCommon.m_cRaces.Count > 0)
            {
                StringsHelper.Merge(ref m_pCommon.m_cRaces, pNewCommon.m_cRaces);
                sLog.AppendLine(string.Format("{0} записей перенесены в список рас '{1}':", pNewCommon.m_cRaces.Count, m_pCommon.ToString()));
                foreach (string sAdded in pNewCommon.m_cRaces)
                    sLog.AppendLine(sAdded);
                sLog.AppendLine("");
            }
            if (pNewCommon.m_cRaces.Count > 0)
            {
                StringsHelper.Merge(ref m_pCommon.m_cPerks, pNewCommon.m_cPerks);
                sLog.AppendLine(string.Format("{0} записей перенесены в список особенностей '{1}':", pNewCommon.m_cPerks.Count, m_pCommon.ToString()));
                foreach (string sAdded in pNewCommon.m_cPerks)
                    sLog.AppendLine(sAdded);
                sLog.AppendLine("");
            }
            if (pNewCommon.m_cRaces.Count > 0)
            {
                StringsHelper.Merge(ref m_pCommon.m_cProfessions, pNewCommon.m_cProfessions);
                sLog.AppendLine(string.Format("{0} записей перенесены в список профессий героя '{1}':", pNewCommon.m_cProfessions.Count, m_pCommon.ToString()));
                foreach (string sAdded in pNewCommon.m_cProfessions)
                    sLog.AppendLine(sAdded);
                sLog.AppendLine("");
            }
            if (pNewCommon.m_cRaces.Count > 0)
            {
                StringsHelper.Merge(ref m_pCommon.m_cProfessionsElite, pNewCommon.m_cProfessionsElite);
                sLog.AppendLine(string.Format("{0} записей перенесены в список профессий злодея '{1}':", pNewCommon.m_cProfessionsElite.Count, m_pCommon.ToString()));
                foreach (string sAdded in pNewCommon.m_cProfessionsElite)
                    sLog.AppendLine(sAdded);
                sLog.AppendLine("");
            }
            if (pNewCommon.m_cRaces.Count > 0)
            {
                StringsHelper.Merge(ref m_pCommon.m_cLocations, pNewCommon.m_cLocations);
                sLog.AppendLine(string.Format("{0} записей перенесены в список мест '{1}':", pNewCommon.m_cLocations.Count, m_pCommon.ToString()));
                foreach (string sAdded in pNewCommon.m_cLocations)
                    sLog.AppendLine(sAdded);
                sLog.AppendLine("");
            }
            if (pNewCommon.m_cRaces.Count > 0)
            {
                StringsHelper.Merge(ref m_pCommon.m_cItems, pNewCommon.m_cItems);
                sLog.AppendLine(string.Format("{0} записей перенесены в список предметов '{1}':", pNewCommon.m_cItems.Count, m_pCommon.ToString()));
                foreach (string sAdded in pNewCommon.m_cItems)
                    sLog.AppendLine(sAdded);
                sLog.AppendLine("");
            }

            return sLog.ToString();
        }
    }
}
