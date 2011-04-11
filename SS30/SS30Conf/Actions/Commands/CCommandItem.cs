using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS30Conf.Items;
using System.Xml;
using nsUniLibXML;
using SS30Conf.Persons;

namespace SS30Conf.Actions.Commands
{
    public class CCommandItem: CCommand
    {
        private CBindingConfigProperty<CItem> m_pItem;

        public CItem Item
        {
            get { return m_pItem.Object; }
            set { m_pItem.Object = value; }
        }
        private CConfigProperty<int> m_pAmountChange;

        public int AmountChange
        {
            get { return m_pAmountChange.Value; }
            set { m_pAmountChange.Value = value; }
        }

        public override void Execute(CPerson actor, CPerson target)
        {
            throw new NotImplementedException();
        }

        protected override void InitProperties()
        {
            base.InitProperties();

            m_pItem = new CBindingConfigProperty<CItem>(this, "Item", StringCategory.ITEM);

            m_pAmountChange = new CConfigProperty<int>(this, "Change", 0);
        }

        public CCommandItem(CReaction pParent)
            : base(StringSubCategory.ITEM, pParent)
        { }

        public CCommandItem(UniLibXML xml, XmlNode pNode)
            : base(xml, pNode)
        { }

        public override string ToString()
        {
            return string.Format("{0} ({1}{2})", Item != null ? Item.Value:"WRONG ITEM", AmountChange >= 0 ? "+" : "", AmountChange);
        }
    }
}
