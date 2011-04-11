using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using nsUniLibXML;

namespace SS30Conf
{
    /// <summary>
    /// Навык, используемый девушками для удовлетворения клиента во время досуга
    /// </summary>
    public class CSkill: CConfigObject
    {
        public CSkill()
            : base(StringCategory.SKILL)
        {
            Name = "New Skill";
            Description = "Enter description for a new skill here...";
        }

        public CSkill(UniLibXML xml, XmlNode pNode, CWorld pWorld)
            : base(xml, pNode)
        {
            pWorld.Skills.Add(this);
        }
    }
}
