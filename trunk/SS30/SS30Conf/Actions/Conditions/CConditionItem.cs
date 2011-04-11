using System;
using System.Collections.Generic;
using System.Text;
using SS30Conf.Items;
using System.Xml;
using nsUniLibXML;
using SS30Conf.Persons;

namespace SS30Conf.Actions.Conditions
{
    /// <summary>
    /// Условие наличия у исполнителя(?) указанного предмета.
    /// </summary>
    public class CConditionItem: CCondition
    {
        private CBindingConfigProperty<CItem> m_pItem;

        public CItem Item
        {
            get { return m_pItem.Object; }
            set { m_pItem.Object = value; }
        }
        public override bool Check(CPerson actor, CPerson target)
        {
            throw new NotImplementedException();
        }

        protected override void InitProperties()
        {
            base.InitProperties();

            m_pItem = new CBindingConfigProperty<CItem>(this, "Item", StringCategory.ITEM);
        }

        public CConditionItem(CReaction pParent)
            : base(StringSubCategory.ITEM, pParent)
        { }

        public CConditionItem(UniLibXML xml, XmlNode pNode)
            : base(xml, pNode)
        { }

        public override string ToString()
        {
            return string.Format("have {0}{1}", Not ? "not ":"", Item != null ? Item.Value:"WRONG ITEM");
        }
    }
}
