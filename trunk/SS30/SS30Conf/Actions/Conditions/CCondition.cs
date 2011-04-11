using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using nsUniLibXML;
using SS30Conf.Persons;

namespace SS30Conf.Actions.Conditions
{
    /// <summary>
    /// Базовый класс для всех условий применимости Реакции
    /// </summary>
    public abstract class CCondition : CParentedConfigString<CReaction>
    {
        private CConfigProperty<StringSubCategory> m_pSubCategory;

        private CConfigProperty<bool> m_pNot;
        /// <summary>
        /// Если true, то условие считается истинным, когда заданный критерий НЕ выполняется.
        /// </summary>
        public bool Not
        {
            get { return m_pNot.Value; }
            set { m_pNot.Value = value; }
        }

        /// <summary>
        /// Проверяет условие
        /// </summary>
        /// <returns>true - условие выполняется</returns>
        public abstract bool Check(CPerson actor, CPerson target);

        public CCondition(StringSubCategory eSubCategory, CReaction pParent)
            : base(StringCategory.CONDITION, pParent)
        {
            m_pSubCategory.Value = eSubCategory;
            Value = Value.Insert(3,pParent.Value.Substring(3));
       }

        public CCondition(UniLibXML xml, XmlNode pNode)
            : base(xml, pNode, StringCategory.REACTION)
        {
        }

        protected override void InitProperties()
        {
            base.InitProperties();

            m_pSubCategory = new CConfigProperty<StringSubCategory>(this, "SubCategory", StringSubCategory.NULL);
            m_pNot = new CConfigProperty<bool>(this, "Not", false);
        }

        public override void PostParse()
        {
            Parent.Conditions.Add(this);
        }
    }
}
