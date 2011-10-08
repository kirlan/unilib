using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nsUniLibXML;
using System.Xml;
using ReadOnlyDictionary;

namespace CinemaEngine
{
    /// <summary>
    /// Хранит всю подгружаемую из внешнего файла принципиальную информацию об
    /// устройстве игрового мира - тэги, действия, типы мебели...
    /// </summary>
    public class Universe
    {
        static private Universe m_pThis = null;

        static public Universe Instance
        {
            get
            {
                if (m_pThis == null)
                    m_pThis = new Universe();

                return m_pThis;
            }
        }

        private List<Action> m_cActions = new List<Action>();

        public List<Action> Actions
        {
            get { return m_cActions; }
        }

        private Dictionary<string, GenreTag> m_cTags = new Dictionary<string, GenreTag>();

        public ReadOnlyDictionary<string, GenreTag> Tags
        {
            get { return new ReadOnlyDictionary<string,GenreTag>(m_cTags); }
        }

        internal bool RenameTag(GenreTag pTag, string sNewName)
        {
            if (!m_cTags.ContainsKey(pTag.Name))
                return false;

            if (pTag.Name == sNewName)
                return true;

            if (m_cTags.ContainsKey(sNewName))
                return false;

            m_cTags.Remove(pTag.Name);
            m_cTags[sNewName] = pTag;

            return true;
        }

        private List<string> m_cStatuses = new List<string>();

        public List<string> Statuses
        {
            get { return m_cStatuses; }
        }

        public void Clear()
        { 
            m_cActions.Clear();
        }

        public void Write(string sFileName)
        {
            UniLibXML pXml = new UniLibXML("CinemaEngine");

            XmlNode pTagsNode = pXml.CreateNode(pXml.Root, "Tags");

            foreach (GenreTag pTag in m_cTags.Values)
            {
                XmlNode pTagNode = pXml.CreateNode(pTagsNode, "Tag");
                pTag.Write(pXml, pTagNode);
            }

            XmlNode pActionsNode = pXml.CreateNode(pXml.Root, "Actions");

            foreach (Action pAction in m_cActions)
            {
                XmlNode pActionNode = pXml.CreateNode(pActionsNode, "Action");
                pAction.Write(pXml, pActionNode);
            }

            pXml.Write(sFileName);
        }

        public void Read(string sFileName)
        {
            UniLibXML pXml = new UniLibXML("CinemaEngine");

            if (!pXml.Load(sFileName))
                return;

            foreach (XmlNode pCatNode in pXml.Root.ChildNodes)
            {
                if (pCatNode.Name == "Tags")
                {
                    foreach (XmlNode pTagNode in pCatNode.ChildNodes)
                    {
                        if (pTagNode.Name == "Tag")
                        {
                            GenreTag pNewTag = new GenreTag(pXml, pTagNode);
                            m_cTags[pNewTag.Name] = pNewTag;
                        }
                    }
                }
                if (pCatNode.Name == "Actions")
                {
                    foreach (XmlNode pActionNode in pCatNode.ChildNodes)
                    {
                        if (pActionNode.Name == "Action")
                        {
                            Action pNewAction = new Action(pXml, pActionNode);
                            m_cActions.Add(pNewAction);
                        }
                    }
                }
            }
        }

        public bool AddTag(GenreTag eTag)
        {
            if (m_cTags.ContainsKey(eTag.Name))
                return false;

            m_cTags[eTag.Name] = eTag;
            return true;
        }
    }
}
