using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nsUniLibXML;
using System.Xml;

namespace CinemaEngine
{
    public class Repository
    {
        static private Repository m_pThis = null;

        static public Repository Instance
        {
            get
            {
                if (m_pThis == null)
                    m_pThis = new Repository();

                return m_pThis;
            }
        }

        private List<Action> m_cActions = new List<Action>();

        public List<Action> Actions
        {
            get { return m_cActions; }
        }

        private List<string> m_cSubGenres = new List<string>();

        public List<string> SubGenres
        {
            get { return m_cSubGenres; }
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
    }
}
