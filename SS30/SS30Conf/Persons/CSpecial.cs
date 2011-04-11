using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nsUniLibXML;
using System.Xml;
using Random;

namespace SS30Conf.Persons
{
    public class CSpecial : CPerson
    {
        public CSpecial()
            : base(StringSubCategory.PERSON_SPECIAL, Gender.MALE)
        {
        }

        public CSpecial(string sId, string sName, Gender eGender)
            : base(StringSubCategory.PERSON_SPECIAL, eGender, sId)
        {
            Name = sName;
        }

        public CSpecial(UniLibXML xml, XmlNode pNode, CWorld pWorld)
            : base(xml, pNode, pWorld)
        {
        }

        protected override void InitProperties()
        {
            base.InitProperties();

            foreach (CPersonStat stat in CConfigRepository.Instance.Strings[StringCategory.STAT].Values)
            {
                if (stat.Availability[StringSubCategory.PERSON_SPECIAL])
                    Stats[stat.Value] = Rnd.Get(20);
            }
            foreach (CSkill skill in CConfigRepository.Instance.Strings[StringCategory.SKILL].Values)
            {
                Skills[skill.Value] = Rnd.Get(20);
            }
        }
    }
}
