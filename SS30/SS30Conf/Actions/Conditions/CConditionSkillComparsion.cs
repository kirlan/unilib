using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using nsUniLibXML;
using SS30Conf.Persons;

namespace SS30Conf.Actions.Conditions
{
    public enum ComparsionType
    {
        /// <summary>
        /// много меньше
        /// </summary>
        LOLO,
        /// <summary>
        /// меньше
        /// </summary>
        LO,
        /// <summary>
        /// равно
        /// </summary>
        EQ,
        /// <summary>
        /// больше
        /// </summary>
        HI,
        /// <summary>
        /// много больше
        /// </summary>
        HIHI
    }

    /// <summary>
    /// Сравнение уровня указанного навыка у исполнителя и цели действия
    /// </summary>
    public class CConditionSkillComparsion : CCondition
    {
        private CBindingConfigProperty<CSkill> m_pSkill;

        public CSkill Skill
        {
            get { return m_pSkill.Object; }
            set { m_pSkill.Object = value;  }
        }
        private CConfigProperty<ComparsionType> m_pComparsionType;
        /// <summary>
        /// Соотношение значения навыка у исполнителя и цели действия.
        /// Т.е. если стоит значение "много больше", то условие выполнится,
        /// если уровень навыка _исполнителя_ много больше уровня этого же
        /// навыка у _цели_ действия.
        /// </summary>
        public ComparsionType ComparsionType
        {
            get { return m_pComparsionType.Value; }
            set { m_pComparsionType.Value = value; }
        }

        public override bool Check(CPerson actor, CPerson target)
        {
            int A = actor.Skills[Skill.Value];
            int B = A - 100;
            int C = target.Skills[Skill.Value] - actor.Skills[Skill.Value];
            switch (ComparsionType)
            {
                case ComparsionType.LOLO:
                    return C > A;
                case ComparsionType.LO:
                    return C > 0;
                case ComparsionType.EQ:
                    return C == 0;
                case ComparsionType.HIHI:
                    return C < B;   //C < 0 && |C| > 100-A
                case ComparsionType.HI:
                    return C < 0;
            }

            return false;
        }

        protected override void InitProperties()
        {
            base.InitProperties();

            m_pSkill = new CBindingConfigProperty<CSkill>(this, "Skill", StringCategory.SKILL);

            m_pComparsionType = new CConfigProperty<ComparsionType>(this, "Type", ComparsionType.EQ);
        }

        public CConditionSkillComparsion(CReaction pParent)
            : base(StringSubCategory.SKILL_COMP, pParent)
        { }

        public CConditionSkillComparsion(UniLibXML xml, XmlNode pNode)
            : base(xml, pNode)
        { }

        public override string ToString()
        {
            string sType = "?";
            switch (ComparsionType)
            {
                case ComparsionType.LOLO:
                    sType = "much lower";
                    break;
                case ComparsionType.LO:
                    sType = "lower";
                    break;
                case ComparsionType.EQ:
                    sType = "equal";
                    break;
                case ComparsionType.HI:
                    sType = "greater";
                    break;
                case ComparsionType.HIHI:
                    sType = "much greater";
                    break;
            }
            return string.Format("actor's {0} is {1}{2}", Skill != null ? Skill.Value:"WRONG SKILL", Not? "not ":"", sType);
        }
    }
}
