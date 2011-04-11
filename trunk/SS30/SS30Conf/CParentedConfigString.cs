using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nsUniLibXML;
using System.Xml;

namespace SS30Conf
{
    public abstract class CParentedConfigString<T> : CConfigObject where T : CConfigObject 
    {
        protected CBindingConfigProperty<T> m_pParent;

        public T Parent
        {
            get { return m_pParent.Object; }
        }

        protected override void InitProperties()
        {
            base.InitProperties();

            m_pParent = new CBindingConfigProperty<T>(this, "Owner", StringCategory.CONFIGURATION);
        }

        public CParentedConfigString(StringCategory eCategory, CConfigObject pParent)
            : base(eCategory)
        {
            m_pParent.UpdateCategory(pParent.Category);

            m_pParent.Value = pParent.Value;
        }

        public CParentedConfigString(UniLibXML xml, XmlNode pNode, StringCategory eParentCategory)
            : base(xml, pNode)
        {
            m_pParent.UpdateCategory(eParentCategory);
        }
    }
}
