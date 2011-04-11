using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nsUniLibXML;
using System.Xml;

namespace VixenQuest
{
    public class Item
    {
        public string m_sName;
        public string m_sNames;
        public int m_iWeight;
        public int m_iPrice;

        public override string ToString()
        {
            return m_sName + " (#:" + m_iWeight.ToString() + ", $:" + m_iPrice.ToString() + ")";
        }

        internal virtual void Write2XML(UniLibXML pXml, XmlNode pRewardNode)
        {
            pXml.AddAttribute(pRewardNode, "name", m_sName);
            pXml.AddAttribute(pRewardNode, "names", m_sNames);
            pXml.AddAttribute(pRewardNode, "weight", m_iWeight);
            pXml.AddAttribute(pRewardNode, "price", m_iPrice);
        }
    }
}
