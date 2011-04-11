using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using nsUniLibXML;
using Random;

namespace SS30Conf.Persons
{
    public class CMaid: CPerson
    {
        public CMaid()
            : base(StringSubCategory.PERSON_MAID, Gender.FEMALE)
        {
        }

        public CMaid(UniLibXML xml, XmlNode pNode, CWorld pWorld)
            : base(xml, pNode, pWorld)
        {
        }

        protected override void InitProperties()
        {
            base.InitProperties();

            foreach(CPersonStat stat in CConfigRepository.Instance.Strings[StringCategory.STAT].Values)
            {
                if(stat.Availability[StringSubCategory.PERSON_MAID])
                    Stats[stat.Value] = Rnd.Get(20);
            }
            foreach (CSkill skill in CConfigRepository.Instance.Strings[StringCategory.SKILL].Values)
            {
                Skills[skill.Value] = Rnd.Get(20);
            }
        }
    }
}
