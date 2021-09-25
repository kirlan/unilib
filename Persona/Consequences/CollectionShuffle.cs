using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Persona.Collections;
using nsUniLibXML;
using System.Xml;

namespace Persona.Consequences
{
    public class CollectionShuffle : Consequence
    {
        public ObjectsCollection m_pCollection;

        public CollectionShuffle()
        { }

        public CollectionShuffle(ObjectsCollection pCollection)
        {
            m_pCollection = pCollection;
        }

        public CollectionShuffle(UniLibXML pXml, XmlNode pCollNode, List<ObjectsCollection> cCollections)
        {
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
            pXml.AddAttribute(pConsequenceNode, "collection", m_pCollection.m_sName);
        }

        public override string ToString()
        {
            return m_pCollection.m_sName + ".Обновить";
        }

        public override Consequence Clone()
        {
            CollectionShuffle pNew = new CollectionShuffle(m_pCollection);

            return pNew;
        }

        internal override void Apply(Module pModule)
        {
            pModule.m_sLog.AppendLine("\tDO " + this.ToString());
            m_pCollection.Shuffle(pModule);
        }
    }
}
