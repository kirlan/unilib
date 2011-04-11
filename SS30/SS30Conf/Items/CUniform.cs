using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using nsUniLibXML;

namespace SS30Conf.Items
{
    public class CUniform: CItem
    {
        public CUniform()
            : base(StringSubCategory.ITEM_UNIFORM)
        {
        }

        public CUniform(UniLibXML xml, XmlNode pNode, CWorld pWorld)
            : base(xml, pNode, pWorld)
        {
        }

        private Dictionary<CFetish, int> m_cFetishCompliance = new Dictionary<CFetish, int>();
    }
}
