using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using nsUniLibXML;

namespace SS30Conf.Items
{
    public class CGift: CItem
    {
        public CGift()
            : base(StringSubCategory.ITEM_GIFT)
        {
        }

        public CGift(UniLibXML xml, XmlNode pNode, CWorld pWorld)
            : base(xml, pNode, pWorld)
        {
        }
    }
}
