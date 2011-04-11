using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nsUniLibXML;
using System.Xml;

namespace SS30Conf.Items
{
    /// <summary>
    /// Базовый класс для всех покупаемых предметов
    /// </summary>
    public abstract class CItem: CConfigObject
    {
        private CConfigProperty<StringSubCategory> m_pSubCategory;

        public CItem(StringSubCategory eSubCategory)
            : base(StringCategory.ITEM)
        {
            Name = "New Item";
            Description = "Enter description for a new item here...";

            m_pSubCategory.Value = eSubCategory;
        }

        public CItem(UniLibXML xml, XmlNode pNode, CWorld pWorld)
            : base(xml, pNode)
        {
            pWorld.Items.Add(this);
        }

        protected override void InitProperties()
        {
            base.InitProperties();
            m_pSubCategory = new CConfigProperty<StringSubCategory>(this, "SubCategory", StringSubCategory.NULL);
        }
    }
}
