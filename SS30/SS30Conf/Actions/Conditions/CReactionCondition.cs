using System;
using System.Collections.Generic;
using System.Text;

namespace SS30Conf.Actions.Conditions
{
    /// <summary>
    /// Базовый класс для всех условий применимости Реакции
    /// </summary>
    public abstract class CReactionCondition : CParentedConfigString<CReaction>
    {
        private StringSubCategory m_eSubCategory;

        /// <summary>
        /// Проверяет условие
        /// </summary>
        /// <returns>true - условие выполняется</returns>
        public abstract bool Check();

        public CReactionCondition(StringSubCategory eSubCategory, CReaction pParent)
            : base(StringCategory.CONDITION, pParent)
        {
            m_eSubCategory = eSubCategory;
        }

        public CReactionCondition(CConfigParser pParser)
            : base(pParser)
        {
        }

        public override string GetCfgString()
        {
            return string.Format("{0}|{1}", base.GetCfgString(), SubCategoryNames[m_eSubCategory]);
        }

        public override void Parse(CConfigParser pParser)
        {
            base.Parse(pParser);
            m_eSubCategory = pParser.SubCategory;
            pParser.Skip();
        }
    }
}
