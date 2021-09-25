using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using nsUniLibXML;

namespace CinemaEngine
{
    public class StatusCondition
    {
        private List<string> m_cAnyOf = new List<string>();

        public string[] AnyOf
        {
            get { return m_cAnyOf.ToArray(); }
        }

        private List<string> m_cExcept = new List<string>();

        public string[] Except
        {
            get { return m_cExcept.ToArray(); }
        }

        void AllowStatus(string sStatus)
        {
            if (m_cAnyOf.Contains(sStatus))
                return;

            if (!Repository.Instance.Statuses.Contains(sStatus))
                Repository.Instance.Statuses.Add(sStatus);

            if (m_cExcept.Contains(sStatus))
                m_cExcept.Remove(sStatus);
            else
                m_cAnyOf.Add(sStatus);
        }

        void ForbidStatus(string sStatus)
        {
            if (m_cExcept.Contains(sStatus))
                return;

            if (!Repository.Instance.Statuses.Contains(sStatus))
                Repository.Instance.Statuses.Add(sStatus);

            if (m_cAnyOf.Contains(sStatus))
                m_cAnyOf.Remove(sStatus);
            else
                m_cExcept.Add(sStatus);
        }

        internal void Write(UniLibXML pXml, XmlNode pConditionsNode)
        {
            foreach (string sStatus in m_cAnyOf)
            {
                XmlNode pStatusNode = pXml.CreateNode(pConditionsNode, "AnyOf");
                pXml.AddAttribute(pStatusNode, "status", sStatus);
            }

            foreach (string sStatus in m_cExcept)
            {
                XmlNode pStatusNode = pXml.CreateNode(pConditionsNode, "Except");
                pXml.AddAttribute(pStatusNode, "status", sStatus);
            }
        }

        internal void Load(UniLibXML pXml, XmlNode pConditionsNode)
        {
            foreach (XmlNode pSubNode in pConditionsNode.ChildNodes)
            {
                if (pSubNode.Name == "AnyOf")
                {
                    string sStatus = "";
                    pXml.GetStringAttribute(pSubNode, "status", ref sStatus);
                    AllowStatus(sStatus);
                }
                if (pSubNode.Name == "Except")
                {
                    string sStatus = "";
                    pXml.GetStringAttribute(pSubNode, "status", ref sStatus);
                    ForbidStatus(sStatus);
                }
            }
        }
    }
}
