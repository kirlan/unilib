using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS30Conf.Actions;
using System.Xml;
using nsUniLibXML;

namespace SS30Conf
{
    /// <summary>
    /// Досуг - определённая форма времяпровождения клиента
    /// </summary>
    public class CLeisure: CConfigObject
    {
        private CBindingConfigProperty<CAction> m_pAction;

        public CAction Action
        {
            get { return m_pAction.Object; }
            set { m_pAction.Object = value; }
        }

        public CLeisure()
            : base(StringCategory.LEISURE)
        {
            Name = "New Leisure";
            Description = "Enter description for a new leisure here...";
        }

        public CLeisure(UniLibXML xml, XmlNode pNode, CWorld pWorld)
            : base(xml, pNode)
        {
            pWorld.Leisures.Add(this);
        }

        protected override void InitProperties()
        {
            base.InitProperties();

            m_pAction = new CBindingConfigProperty<CAction>(this, "Action", StringCategory.ACTION);
        }
    }
}
