using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using nsUniLibXML;

namespace SS30Conf.Items
{
    public class CSexToy: CItem
    {
        public CSexToy()
            : base(StringSubCategory.ITEM_SEX_TOY)
        {
        }

        public CSexToy(UniLibXML xml, XmlNode pNode, CWorld pWorld)
            : base(xml, pNode, pWorld)
        {
        }
    }
}
