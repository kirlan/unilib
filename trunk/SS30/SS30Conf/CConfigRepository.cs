using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SS30Conf.Actions;
using SS30Conf.Actions.Commands;
using SS30Conf.Actions.Conditions;
using SS30Conf.Items;
using nsUniLibXML;
using System.Xml;

namespace SS30Conf
{
    public class CConfigRepository
    {
        static CConfigRepository m_pThis = null;
        public static CConfigRepository Instance
        {
            get
            {
                if (m_pThis == null)
                    m_pThis = new CConfigRepository();

                return m_pThis;
            }
        }

        public CConfigRepository()
        {
            foreach (StringCategory cat in Enum.GetValues(typeof(StringCategory)))
            {
                Strings[cat] = new Dictionary<string, CConfigObject>();
            }

            m_scCategoryNames[StringCategory.ACTION] = "act_";
            m_scCategoryNames[StringCategory.COMMAND] = "cmd_";
            m_scCategoryNames[StringCategory.COMMODITY] = "com_";
            m_scCategoryNames[StringCategory.CONDITION] = "con_";
            m_scCategoryNames[StringCategory.CONFIGURATION] = "cfg_";
            m_scCategoryNames[StringCategory.FETISH] = "fet_";
            m_scCategoryNames[StringCategory.ITEM] = "itm_";
            m_scCategoryNames[StringCategory.LEISURE] = "lsr_";
            m_scCategoryNames[StringCategory.REACTION] = "rct_";
            m_scCategoryNames[StringCategory.ROOM] = "roo_";
            m_scCategoryNames[StringCategory.SKILL] = "skl_";
            m_scCategoryNames[StringCategory.STAT] = "stt_";
            m_scCategoryNames[StringCategory.PERSON] = "prs_";
        }

        private Dictionary<StringCategory, string> m_scCategoryNames = new Dictionary<StringCategory,string>();

        public Dictionary<StringCategory, string> CategoryNames
        {
            get { return m_scCategoryNames; }
        }

        private Dictionary<StringCategory, Dictionary<string, CConfigObject>> m_cStrings = new Dictionary<StringCategory, Dictionary<string, CConfigObject>>();

        public Dictionary<StringCategory, Dictionary<string, CConfigObject>> Strings
        {
            get { return m_cStrings; }
        }

        private List<string> m_cStatuses = new List<string>();

        public List<string> Statuses
        {
            get { return m_cStatuses; }
        }

        private Dictionary<PersonStats, string> m_cStatsNames = new Dictionary<PersonStats, string>();

        public Dictionary<PersonStats, string> StatsNames
        {
            get { return m_cStatsNames; }
        }

        public CConfigObject FindID(string sID)
        {
            foreach (Dictionary<string, CConfigObject> cat in m_cStrings.Values)
            {
                foreach (CConfigObject pString in cat.Values)
                {
                    if (pString.Value == sID)
                        return pString;
                }
            }
            return null;
        }

        public void SaveXML(string sFilename)
        {
            UniLibXML xml = new UniLibXML("ss30");

            foreach (StringCategory eCat in m_cStrings.Keys)
            {
                Dictionary<string, CConfigObject> cat = m_cStrings[eCat];
                XmlNode catNode = xml.CreateNode(xml.Root, Enum.GetName(typeof(StringCategory), eCat));
                foreach (CConfigObject pString in cat.Values)
                {
                    pString.Write2XML(xml, catNode);
                }
            }

            xml.Write(sFilename);
        }

        public bool Load(string sFilename, CWorld pWorld)
        {
            UniLibXML xml = new UniLibXML("ss30");

            if (!xml.Load(sFilename))
            {
                return false;
            }

            foreach (XmlNode cat in xml.Root.ChildNodes)
            {
                StringCategory eCat = (StringCategory)Enum.Parse(typeof(StringCategory), cat.Name);
                foreach (XmlNode obj in cat.ChildNodes)
                {
                    CConfigObject newString;
                    if (m_cStrings.ContainsKey(eCat) &&
                        m_cStrings[eCat] != null &&
                        m_cStrings[eCat].ContainsKey(obj.Name))
                    {
                        newString = m_cStrings[eCat][obj.Name];
                        newString.Parse(xml, obj);
                    }
                    else
                    {
                        string sSubCat = "";
                        StringSubCategory subCat;
                        switch (eCat)
                        {
                            case StringCategory.ACTION:
                                newString = new CAction(xml, obj, pWorld);
                                break;
                            case StringCategory.COMMAND:
                                xml.GetStringAttribute(obj, "SubCategory", ref sSubCat);
                                subCat = (StringSubCategory)Enum.Parse(typeof(StringSubCategory), sSubCat);
                                switch (subCat)
                                {
                                    case StringSubCategory.ROOM:
                                        newString = new CCommandBuildRoom(xml, obj);
                                        break;
                                    case StringSubCategory.SKILL:
                                        newString = new CCommandChangeSkill(xml, obj);
                                        break;
                                    case StringSubCategory.STAT:
                                        newString = new CCommandChangeStat(xml, obj);
                                        break;
                                    case StringSubCategory.STATUS:
                                        newString = new CCommandStatus(xml, obj);
                                        break;
                                    case StringSubCategory.ITEM:
                                        newString = new CCommandItem(xml, obj);
                                        break;
                                    case StringSubCategory.SHOP:
                                        newString = new CCommandOpenShop(xml, obj);
                                        break;
                                    case StringSubCategory.REST:
                                        newString = new CCommandRest(xml, obj);
                                        break;
                                }
                                break;
                            case StringCategory.CONDITION:
                                xml.GetStringAttribute(obj, "SubCategory", ref sSubCat);
                                subCat = (StringSubCategory)Enum.Parse(typeof(StringSubCategory), sSubCat);
                                switch (subCat)
                                {
                                    case StringSubCategory.ITEM:
                                        newString = new CConditionItem(xml, obj);
                                        break;
                                    case StringSubCategory.SKILL:
                                        newString = new CConditionSkillLevel(xml, obj);
                                        break;
                                    case StringSubCategory.RND:
                                        newString = new CConditionRnd(xml, obj);
                                        break;
                                    case StringSubCategory.SKILL_COMP:
                                        newString = new CConditionSkillComparsion(xml, obj);
                                        break;
                                    case StringSubCategory.STAT:
                                        newString = new CConditionStatLevel(xml, obj);
                                        break;
                                    case StringSubCategory.APPEAL:
                                        newString = new CConditionPersonalAppeal(xml, obj);
                                        break;
                                }
                                break;
                            case StringCategory.FETISH:
                                newString = new CFetish(xml, obj, pWorld);
                                break;
                            case StringCategory.ITEM:
                                xml.GetStringAttribute(obj, "SubCategory", ref sSubCat);
                                subCat = (StringSubCategory)Enum.Parse(typeof(StringSubCategory), sSubCat);
                                switch (subCat)
                                {
                                    case StringSubCategory.ITEM_GIFT:
                                        newString = new CGift(xml, obj, pWorld);
                                        break;
                                    case StringSubCategory.ITEM_HHM:
                                        newString = new CHouseholdMachinery(xml, obj, pWorld);
                                        break;
                                    case StringSubCategory.ITEM_SEX_TOY:
                                        newString = new CSexToy(xml, obj, pWorld);
                                        break;
                                    case StringSubCategory.ITEM_UNIFORM:
                                        newString = new CUniform(xml, obj, pWorld);
                                        break;
                                }
                                break;
                            case StringCategory.LEISURE:
                                newString = new CLeisure(xml, obj, pWorld);
                                break;
                            case StringCategory.REACTION:
                                newString = new CReaction(xml, obj);
                                break;
                            case StringCategory.ROOM:
                                newString = new CRoom(xml, obj, pWorld);
                                break;
                            case StringCategory.SKILL:
                                newString = new CSkill(xml, obj, pWorld);
                                break;
                        }
                    }
                }
            }

            foreach (Dictionary<string, CConfigObject> cat in m_cStrings.Values)
            {
                foreach (CConfigObject pString in cat.Values)
                {
                    pString.PostParse();
                }
            }

            return true;
        }

        internal void Reset()
        {
            Strings.Clear();
            foreach (StringCategory cat in Enum.GetValues(typeof(StringCategory)))
            {
                Strings[cat] = new Dictionary<string, CConfigObject>();
            }
        }
    }
}
