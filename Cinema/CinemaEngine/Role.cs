using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nsUniLibXML;
using System.Xml;

namespace CinemaEngine
{
    public enum FurnitureType
    { 
        Any,
        CanSeatOn,
        CanLieOn,
        CanLeanTo,
        CanLieUnder,
        CanStandOn,
        CanStayInside,
        CanSitInside,
        CanLieInside
    }

    public class Role
    {
        private string m_sName;

        public string Name
        {
            get { return m_sName; }
            set { m_sName = value; }
        }

        private FurnitureType m_eFurnitureAnchor = FurnitureType.Any;

        public FurnitureType FurnitureAnchor
        {
            get { return m_eFurnitureAnchor; }
            set { m_eFurnitureAnchor = value; }
        }

        private StatusCondition m_cStatus = new StatusCondition();

        public StatusCondition Status
        {
            get { return m_cStatus; }
        }

        public Role(string sName)
        {
            m_sName = sName;
        }

        public Role(UniLibXML pXml, XmlNode pRoleNode)
        {
            pXml.GetStringAttribute(pRoleNode, "name", ref m_sName);
            m_eFurnitureAnchor = (FurnitureType)pXml.GetEnumAttribute(pRoleNode, "anchor", m_eFurnitureAnchor.GetType());

            foreach (XmlNode pSubNode in pRoleNode.ChildNodes)
            {
                if (pSubNode.Name == "Status")
                {
                    m_cStatus.Load(pXml, pSubNode);
                }
            }
        }

        internal void Write(UniLibXML pXml, XmlNode pRoleNode)
        {
            pXml.AddAttribute(pRoleNode, "name", m_sName);
            pXml.AddAttribute(pRoleNode, "anchor", m_eFurnitureAnchor);

            XmlNode pConditionsNode = pXml.CreateNode(pRoleNode, "Status");
            m_cStatus.Write(pXml, pConditionsNode);
        }
    }
}
