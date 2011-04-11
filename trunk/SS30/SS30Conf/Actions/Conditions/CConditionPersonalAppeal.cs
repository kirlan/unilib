using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using nsUniLibXML;
using SS30Conf.Persons;

namespace SS30Conf.Actions.Conditions
{
    public enum PersonalAppeal
    {
        /// <summary>
        /// отвращение
        /// </summary>
        HATE,
        /// <summary>
        /// лёгкая неприязнь
        /// </summary>
        DISLIKE,
        /// <summary>
        /// равнодушие
        /// </summary>
        FLAT,
        /// <summary>
        /// одобрение
        /// </summary>
        LIKE,
        /// <summary>
        /// восхищение
        /// </summary>
        ADMIRE
    }

    /// <summary>
    /// Отношение исполнителя к цели действия - вычисляется на основании множества
    /// факторов, таких как внешность, фетиши, прошлая совместная история и т.д.
    /// Конкретный механизм вычисления отношения может различаться в зависимости от
    /// того, кто исполнитель и кто цель.
    /// Условие выполняется, если вычисленное отношение описывается одним из 5 базовых
    /// уровней отношения (см. enum PersonalAppeal)
    /// </summary>
    public class CConditionPersonalAppeal : CCondition
    {
        private CConfigProperty<PersonalAppeal> m_pPersonalAppeal;
        /// <summary>
        /// Соотношение значения навыка у исполнителя и цели действия.
        /// Т.е. если стоит значение "много больше", то условие выполнится,
        /// если уровень навыка _исполнителя_ много больше уровня этого же
        /// навыка у _цели_ действия.
        /// </summary>
        public PersonalAppeal PersonalAppeal
        {
            get { return m_pPersonalAppeal.Value; }
            set { m_pPersonalAppeal.Value = value; }
        }

        public override bool Check(CPerson actor, CPerson target)
        {
            throw new NotImplementedException();
        }

        protected override void InitProperties()
        {
            base.InitProperties();

            m_pPersonalAppeal = new CConfigProperty<PersonalAppeal>(this, "Appeal", PersonalAppeal.FLAT);
        }

        public CConditionPersonalAppeal(CReaction pParent)
            : base(StringSubCategory.APPEAL, pParent)
        { }

        public CConditionPersonalAppeal(UniLibXML xml, XmlNode pNode)
            : base(xml, pNode)
        { }

        public override string ToString()
        {
            string sType = "?";
            switch (PersonalAppeal)
            {
                case PersonalAppeal.HATE:
                    sType = "hates";
                    break;
                case PersonalAppeal.DISLIKE:
                    sType = "dislikes";
                    break;
                case PersonalAppeal.FLAT:
                    sType = "feels nothing to";
                    break;
                case PersonalAppeal.LIKE:
                    sType = "likes";
                    break;
                case PersonalAppeal.ADMIRE:
                    sType = "admires";
                    break;
            }
            return string.Format("actor {0}{1} action's target", Not? "not ":"", sType);
        }
    }
}
