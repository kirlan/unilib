using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Persona.Collections;
using nsUniLibXML;
using System.Xml;

namespace Persona.Consequences
{
    public class CollectionSelect : Consequence
    {
        public ObjectsCollection m_pCollection;

        public int m_iSelectID;

        public CollectionSelect()
        { }

        public CollectionSelect(ObjectsCollection pCollection, int iSelect)
        {
            m_pCollection = pCollection;
            m_iSelectID = iSelect;
        }

        public CollectionSelect(UniLibXML pXml, XmlNode pCollNode, List<ObjectsCollection> cCollections)
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
        }

        internal override void WriteXML(nsUniLibXML.UniLibXML pXml, System.Xml.XmlNode pConsequenceNode)
        {
            pXml.AddAttribute(pConsequenceNode, "id", m_iSelectID);
            pXml.AddAttribute(pConsequenceNode, "collection", m_pCollection.m_sName);
        }

        public override string ToString()
        {
            return m_pCollection.m_sName + ".Выбрать(" + m_pCollection.m_cObjects[m_iSelectID].m_sName + ")";
        }

        public override Consequence Clone()
        {
            CollectionSelect pNew = new CollectionSelect(m_pCollection, m_iSelectID);

            return pNew;
        }

        internal override void Apply(Module pModule)
        {
            pModule.m_sLog.AppendLine("\tDO " + this.ToString());
            m_pCollection.Select(pModule, m_iSelectID);
        }
    }
}
