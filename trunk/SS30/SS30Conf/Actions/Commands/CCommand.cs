using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using nsUniLibXML;
using SS30Conf.Persons;

namespace SS30Conf.Actions.Commands
{
    /// <summary>
    /// Базовый класс для всех команд, реализующих Реакцию
    /// </summary>
    public abstract class CCommand : CParentedConfigString<CReaction>
    {
        private CConfigProperty<StringSubCategory> m_pSubCategory;
        /// <summary>
        /// Выполняем команду.
        /// </summary>
        public abstract void Execute(CPerson actor, CPerson target);

        public CCommand(StringSubCategory eSubCategory, CReaction pParent)
            : base(StringCategory.COMMAND, pParent)
        {
            m_pSubCategory.Value = eSubCategory;
            Value = Value.Insert(3, pParent.Value.Substring(3));
        }

        public CCommand(UniLibXML xml, XmlNode pNode)
            : base(xml, pNode, StringCategory.REACTION)
        {
        }

        protected override void InitProperties()
        {
            base.InitProperties();

            m_pSubCategory = new CConfigProperty<StringSubCategory>(this, "SubCategory", StringSubCategory.NULL);
        }

        public override void PostParse()
        {
            Parent.Commands.Add(this);
        }

        public abstract string ToString();
    }
}
