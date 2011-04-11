using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using nsUniLibXML;

namespace SS30Conf.Items
{
    public class CHouseholdMachinery: CItem
    {
        public CHouseholdMachinery()
            : base(StringSubCategory.ITEM_HHM)
        {
        }

        public CHouseholdMachinery(UniLibXML xml, XmlNode pNode, CWorld pWorld)
            : base(xml, pNode, pWorld)
        {
        }
    }
}
