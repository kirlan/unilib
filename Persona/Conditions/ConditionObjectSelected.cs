using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Persona.Parameters;
using nsUniLibXML;
using System.Xml;
using Persona.Collections;

namespace Persona.Conditions
{
    public class ConditionObjectSelected: Condition
    {
        public ObjectsCollection m_pCollection;

        public int m_iSelectID;

        public ConditionObjectSelected(ObjectsCollection pCollection, int iSelected)
            : base(null)
        {
            m_pCollection = pCollection;
            m_iSelectID = iSelected;
        }

        public ConditionObjectSelected(UniLibXML pXml, XmlNode pCollNode, List<ObjectsCollection> cCollections)
            :base(null)
        {
            pXml.GetIntAttribute(pCollNode, "id", ref m_iSelectID);

            string sColl = "";
            pXml.GetStringAttribute(pCollNode, "collection", ref sColl);
            foreach (ObjectsCollection pColl in cCollections)
                if (pColl.m_sName == sColl)
                {
                    m_pCollection = pColl;
                    break;
                }

            pXml.GetBoolAttribute(pCollNode, "inverse", ref m_bNot);
        }

        internal override void WriteXML(UniLibXML pXml, XmlNode pConditionNode)
        {
            pXml.AddAttribute(pConditionNode, "id", m_iSelectID);
            pXml.AddAttribute(pConditionNode, "collection", m_pCollection.m_sName);
            pXml.AddAttribute(pConditionNode, "inverse", m_bNot);
        }

        public override string ToString()
        {
            return string.Format("{0}{1} [{2}]", m_bNot ? "НЕ " : "", m_pCollection != null ? m_pCollection.m_sName : "НЕВЕРНЫЙ ПАРАМЕТР", m_pCollection.m_cObjects.ContainsKey(m_iSelectID) ? m_pCollection.m_cObjects[m_iSelectID].m_sName : "НЕВЕРНЫЙ ПАРАМЕТР");
        }

        public override Condition Clone()
        {
            ConditionObjectSelected pNew = new ConditionObjectSelected(m_pCollection, m_iSelectID);
            pNew.m_bNot = m_bNot;

            return pNew;
        }

        public override bool Check()
        {
            bool bValue = m_pCollection.m_iSelectedID == m_iSelectID;

            return m_bNot ? !bValue : bValue;
        }
    }
}
