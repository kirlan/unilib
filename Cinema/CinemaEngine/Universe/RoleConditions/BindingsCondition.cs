using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using nsUniLibXML;
using System.Collections.ObjectModel;
using ReadOnlyDictionary;

namespace CinemaEngine.RoleConditions
{
    /// <summary>
    /// Условие, основанное на связях персонажа с другими персонажами ("сосёт член", "целится в", "держит за руку"...)
    /// или с самим собой - если это просто состояния ("связаны ноги", "связаны руки"...)
    /// </summary>
    public class BindingsCondition : RoleCondition
    {
        private List<KeyValuePair<string, string>> m_cAllowed = new List<KeyValuePair<string, string>>();
        /// <summary>
        /// Состояния, наличие которых у персонажа необходимо для соответствия роли.
        /// </summary>
        public ReadOnlyCollection<KeyValuePair<string, string>> Allowed
        {
            get { return new ReadOnlyCollection<KeyValuePair<string, string>>(m_cAllowed); }
        }

        private List<KeyValuePair<string, string>> m_cForbidden = new List<KeyValuePair<string, string>>();
        /// <summary>
        /// Состояния, наличие которых у персонажа недопустимо для соответствия роли.
        /// </summary>
        public ReadOnlyCollection<KeyValuePair<string, string>> Forbidden
        {
            get { return new ReadOnlyCollection<KeyValuePair<string, string>>(m_cForbidden); }
        }

        public BindingsCondition()
            : base()
        {
        }

        public BindingsCondition(BindingsCondition pCondition)
            : base(pCondition)
        {
            m_cAllowed.AddRange(pCondition.m_cAllowed);
            m_cForbidden.AddRange(pCondition.m_cForbidden);
        }

        internal override void Write(UniLibXML pXml, XmlNode pConditionNode)
        {
            base.Write(pXml, pConditionNode);

            foreach (KeyValuePair<string, string> pPair in m_cAllowed)
            {
                XmlNode pLinkNode = pXml.CreateNode(pConditionNode, "Allowed");
                pXml.AddAttribute(pLinkNode, "actor", pPair.Key);
                pXml.AddAttribute(pLinkNode, "type", pPair.Value);
            }

            foreach (KeyValuePair<string, string> pPair in m_cForbidden)
            {
                XmlNode pLinkNode = pXml.CreateNode(pConditionNode, "Forbidden");
                pXml.AddAttribute(pLinkNode, "actor", pPair.Key);
                pXml.AddAttribute(pLinkNode, "type", pPair.Value);
            }
        }

        public BindingsCondition(UniLibXML pXml, XmlNode pConditionNode)
            : base(pXml, pConditionNode)
        {
            foreach (XmlNode pSubNode in pConditionNode.ChildNodes)
            {
                if (pSubNode.Name == "Allowed")
                {
                    string sActor = "";
                    string sLink = "";
                    pXml.GetStringAttribute(pSubNode, "actor", ref sActor);
                    pXml.GetStringAttribute(pSubNode, "type", ref sLink);
                    KeyValuePair<string, string> pPair = new KeyValuePair<string, string>(sActor, sLink);

                    m_cAllowed.Add(pPair);
                }
                if (pSubNode.Name == "Forbidden")
                {
                    string sActor = "";
                    string sLink = "";
                    pXml.GetStringAttribute(pSubNode, "actor", ref sActor);
                    pXml.GetStringAttribute(pSubNode, "type", ref sLink);
                    KeyValuePair<string, string> pPair = new KeyValuePair<string, string>(sActor, sLink);

                    m_cForbidden.Add(pPair);
                }
            }
        }

        public override RoleCondition Duplicate()
        {
            return new BindingsCondition(this);
        }
    }
}
