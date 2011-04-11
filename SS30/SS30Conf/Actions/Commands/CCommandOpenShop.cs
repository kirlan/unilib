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
    public class CCommodity: CParentedConfigString<CCommandOpenShop>
    {
        private CBindingConfigProperty<CItem> m_pItem;

        public CItem Item
        {
            get { return m_pItem.Object; }
            set { m_pItem.Object = value; }
        }
        
        private CConfigProperty<int> m_pCost;

        public int Cost
        {
            get { return m_pCost.Value; }
            set { m_pCost.Value = value; }
        }

        public CCommodity(CCommandOpenShop pParent)
            : base(StringCategory.COMMODITY, pParent)
        {
            Name = "New Item";
            Cost = 100;
            Description = "Enter description for a new item here...";
        }

        public CCommodity(UniLibXML xml, XmlNode pNode)
            : base(xml, pNode, StringCategory.COMMAND)
        {
        }

        protected override void InitProperties()
        {
            base.InitProperties();

            m_pItem = new CBindingConfigProperty<CItem>(this, "Item", StringCategory.ITEM);

            m_pCost = new CConfigProperty<int>(this, "Price", 100);
        }

        public override void PostParse()
        {
            Parent.Commodities.Add(this);
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}$", Item != null ? Item.Value:"WRONG ITEM", Cost);
        }
    }

    public class CCommandOpenShop: CCommand
    {
        private List<CCommodity> m_cCommodities = new List<CCommodity>();

        public List<CCommodity> Commodities
        {
            get { return m_cCommodities; }
        }

        public override void Execute(CPerson actor, CPerson target)
        {
            throw new NotImplementedException();
        }

        public CCommandOpenShop(CReaction pParent)
            : base(StringSubCategory.SHOP, pParent)
        { }

        public CCommandOpenShop(UniLibXML xml, XmlNode pNode)
            : base(xml, pNode)
        { }

        public override string ToString()
        {
            return string.Format("open shop ({0} items)", m_cCommodities.Count);
        }
    }

}
