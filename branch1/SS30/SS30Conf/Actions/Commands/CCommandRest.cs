using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nsUniLibXML;
using System.Xml;
using SS30Conf.Persons;

namespace SS30Conf.Actions.Commands
{
    public class CCommandRest: CCommand
    {
        public override void Execute(CPerson actor, CPerson target)
        {
            actor.Rest(false);
        }

        protected override void InitProperties()
        {
            base.InitProperties();
        }

        public CCommandRest(CReaction pParent)
            : base(StringSubCategory.REST, pParent)
        { }

        public CCommandRest(UniLibXML xml, XmlNode pNode)
            : base(xml, pNode)
        { }

        public override string ToString()
        {
            return "rest";
        }
    }

}
