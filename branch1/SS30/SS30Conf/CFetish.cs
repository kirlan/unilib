using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using nsUniLibXML;

namespace SS30Conf
{
    public class CFetish: CConfigObject
    {
        public CFetish()
            : base(StringCategory.FETISH)
        {
            Name = "New Fetish";
            Description = "Enter description for a new fetish here...";
        }

        public CFetish(UniLibXML xml, XmlNode pNode, CWorld pWorld)
            : base(xml, pNode)
        {
            pWorld.Fetishes.Add(this);
        }
    }
}
